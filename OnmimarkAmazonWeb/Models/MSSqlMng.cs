namespace ReviewProduct
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;
    using System.Runtime.CompilerServices;

    public class MSSqlMng : IDisposable
    {
        protected const int DBCONN_TIMEOUT = 30;
        private bool m_bIsConnected = false;
        protected SqlConnection m_dbConnection = null;
        protected static MSSqlMng m_Instance;
        public SqlCommand m_sqlCommand = null;
        protected SqlDataReader m_sqlDataReader = null;
        private string m_strDBFileName;
        private string m_strDBID = "ukomnimarknew";
        private string m_strDBName = "UKOmnimarkNew";
        private string m_strDBPwd = "rzV7w43&";
        private string m_strDBServer = "104.238.95.68";
        //private string m_strDBServer = "127.0.0.1";
        private string m_strDBService;

        public event LogEvent EventListener;

        public SqlConnection GetConnection() { return m_dbConnection; }
        public bool AttachDatabase()
        {
            if (!this.m_bIsConnected)
            {
                this.EventListener(LOG_EVENT.ONNOTICEMSG, "Server is not connected to DB Server.");
                return false;
            }
            try
            {
                if (this.ExistDB(this.m_strDBName))
                {
                    string str = string.Format("USE {0}", this.m_strDBName);
                    this.m_sqlCommand.CommandText = str;
                    this.m_sqlDataReader = this.m_sqlCommand.ExecuteReader();
                    if (this.m_sqlDataReader != null)
                    {
                        this.m_sqlDataReader.Close();
                        this.m_sqlDataReader = null;
                    }
                    return true;
                }
                this.EventListener(LOG_EVENT.ONNOTICEMSG, "DB does not exist.");
                return false;
            }
            catch (SqlException exception)
            {
                string param = string.Format("Error to connect {0}. {1}", this.m_strDBName, exception.Message);
                this.EventListener(LOG_EVENT.ONERRORMSG, param);
                return false;
            }
        }

        public int CheckAdminInfo(string strAdminID, string strAdminPWD)
        {
            this.EventListener(LOG_EVENT.ONNOTICEMSG, "Checking Admin Account.");
            try
            {
                this.m_sqlCommand.CommandText = string.Format("USE {0}", this.m_strDBName);
                this.m_sqlCommand.ExecuteNonQuery();
            }
            catch (Exception exception)
            {
                string param = string.Format("Could not access main db.{0}", exception.Message);
                this.EventListener(LOG_EVENT.ONERRORMSG, param);
                return 0;
            }
            try
            {
                bool flag = false;
                int num = 0;
                string str2 = string.Format("SELECT AdmUid FROM SmAdmin WHERE LoginId='{0}' AND LoginPwd='{1}' and AdmUid = 1", strAdminID, strAdminPWD);
                this.m_sqlCommand.CommandText = str2;
                this.m_sqlDataReader = this.m_sqlCommand.ExecuteReader();
                if (this.m_sqlDataReader != null)
                {
                    if (this.m_sqlDataReader.HasRows && this.m_sqlDataReader.Read())
                    {
                        num = Convert.ToInt32(this.m_sqlDataReader.GetInt64(0));
                        flag = true;
                    }
                    this.m_sqlDataReader.Close();
                    this.m_sqlDataReader = null;
                }
                if (!flag)
                {
                    this.EventListener(LOG_EVENT.ONNOTICEMSG, "Admin password mismatch.");
                }
                else
                {
                    this.EventListener(LOG_EVENT.ONNOTICEMSG, "Successfully confirmed.");
                }
                return num;
            }
            catch (Exception exception2)
            {
                string str3 = string.Format("Cannot get account info.{0}", exception2.Message);
                this.EventListener(LOG_EVENT.ONERRORMSG, str3);
                return 0;
            }
        }

        public bool ConnectDBServer()
        {
            this.EventListener(LOG_EVENT.ONNOTICEMSG, string.Format("Connecting to Server ... {0}, {1}", m_strDBServer, m_strDBID));
            if (this.IsConnected)
            {
                string param = string.Format("Already connected to DB Server.", new object[0]);
                this.EventListener(LOG_EVENT.ONNOTICEMSG, param);
                return true;
            }
            string connectionString = string.Concat(new object[] { "Connection Timeout=", 30, ";Password=", this.m_strDBPwd, ";Persist Security Info=True;User ID=", this.m_strDBID, ";Initial Catalog=;Data Source=", this.m_strDBServer, @"\", this.m_strDBService });
            try
            {
                if (this.m_dbConnection != null)
                {
                    this.m_dbConnection.Close();
                }
                this.m_dbConnection = new SqlConnection(connectionString);
                this.m_dbConnection.Open();
            }
            catch (Exception exception)
            {
                connectionString = string.Format("Cannot connect to db server", new object[] { this.m_strDBServer + @"\" + this.m_strDBService, this.m_strDBName, this.m_strDBID, this.m_strDBPwd });
                this.EventListener(LOG_EVENT.ONERRORMSG, connectionString + exception.Message);
                this.m_dbConnection = null;
                return false;
            }
            this.EventListener(LOG_EVENT.ONNOTICEMSG, "Successfully connected.");
            if (this.m_sqlCommand == null)
            {
                this.m_sqlCommand = new SqlCommand();
            }
            this.m_sqlCommand.Connection = this.m_dbConnection;
            this.m_bIsConnected = true;
            return true;
        }

        public bool CreateDB()
        {
            string str;
            try
            {
                if (this.ExistDB(this.m_strDBName))
                {
                    str = "USE master;";
                    this.m_sqlCommand.CommandText = str;
                    this.m_sqlCommand.ExecuteNonQuery();
                    str = "DROP DATABASE " + this.m_strDBName;
                    this.m_sqlCommand.CommandText = str;
                    this.m_sqlCommand.ExecuteNonQuery();
                }
                this.EventListener(LOG_EVENT.ONNOTICEMSG, "Creating Game DB.");
                str = string.Format("CREATE DATABASE {0}", this.m_strDBName);
                this.m_sqlCommand.CommandText = str;
                this.m_sqlCommand.ExecuteNonQuery();
                str = string.Format("USE {0}", this.m_strDBName);
                this.m_sqlCommand.CommandText = str;
                this.m_sqlCommand.ExecuteNonQuery();
                this.EventListener(LOG_EVENT.ONNOTICEMSG, "Created Game DB.");
                string strDBFileName = this.m_strDBFileName;
                this.EventListener(LOG_EVENT.ONNOTICEMSG, "Reading( " + strDBFileName + " ) Game DB.");
                if (!File.Exists(strDBFileName))
                {
                    this.EventListener(LOG_EVENT.ONNOTICEMSG, "Game DB does not exist.");
                    return false;
                }
                StreamReader reader = new StreamReader(strDBFileName);
                string strQuery = reader.ReadToEnd();
                reader.Close();
                this.EventListener(LOG_EVENT.ONNOTICEMSG, "Installing Tables.");
                if (!this.UpdateQuery(strQuery))
                {
                    throw new ApplicationException("Error occurred while analyzing table.");
                }
                this.EventListener(LOG_EVENT.ONNOTICEMSG, "Successfully completed.");
            }
            catch (Exception)
            {
                str = "USE master; DROP DATABASE " + this.m_strDBName;
                this.m_sqlCommand.CommandText = str;
                this.m_sqlCommand.ExecuteNonQuery();
                return false;
            }
            return true;
        }

        public bool DeleteQuery(string strQuery)
        {
            try
            {
                lock (this)
                {
                    this.m_sqlCommand.CommandText = strQuery;
                    SqlDataAdapter adapter = new SqlDataAdapter
                    {
                        DeleteCommand = new SqlCommand(strQuery, this.m_dbConnection)
                    };
                    adapter.DeleteCommand.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception exception)
            {
                string param = string.Format("Query Error:{0} {1}", strQuery, exception.Message);
                this.EventListener(LOG_EVENT.ONERRORMSG, param);
                return false;
            }
        }

        public bool DisconnectDBServer()
        {
            if (this.m_bIsConnected)
            {
                this.EventListener(LOG_EVENT.ONNOTICEMSG, "Connection was closed.");
                if (this.m_sqlDataReader != null)
                {
                    this.m_sqlDataReader.Close();
                }
                if (this.m_sqlCommand != null)
                {
                    this.m_sqlCommand.Cancel();
                }
                this.m_dbConnection.Close();
                this.m_bIsConnected = false;
            }
            return true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && this.m_bIsConnected)
            {
                if (this.m_dbConnection != null)
                {
                    this.m_dbConnection.Close();
                    this.m_dbConnection.Dispose();
                    this.m_dbConnection = null;
                }
                this.m_bIsConnected = false;
                if (this.m_sqlDataReader != null)
                {
                    if (!this.m_sqlDataReader.IsClosed)
                    {
                        this.m_sqlDataReader.Close();
                    }
                    this.m_sqlDataReader.Dispose();
                    this.m_sqlDataReader = null;
                }
                if (this.m_sqlCommand != null)
                {
                    if (this.m_sqlCommand.Connection != null)
                    {
                        this.m_sqlCommand.Connection.Dispose();
                        this.m_dbConnection = null;
                    }
                    this.m_sqlCommand.Dispose();
                    this.m_sqlCommand = null;
                }
            }
        }

        public bool ExistDB(string strDBName)
        {
            bool hasRows = false;
            string str = string.Format("USE master", new object[0]);
            this.m_sqlCommand.CommandText = str;
            this.m_sqlDataReader = this.m_sqlCommand.ExecuteReader();
            if (this.m_sqlDataReader != null)
            {
                this.m_sqlDataReader.Close();
                this.m_sqlDataReader = null;
            }
            str = string.Format("SELECT name FROM sysdatabases WHERE name='{0}'", strDBName);
            this.m_sqlCommand.CommandText = str;
            this.m_sqlDataReader = this.m_sqlCommand.ExecuteReader();
            if (this.m_sqlDataReader != null)
            {
                hasRows = this.m_sqlDataReader.HasRows;
                this.m_sqlDataReader.Close();
                this.m_sqlDataReader = null;
            }
            return hasRows;
        }

        public static MSSqlMng GetInstance()
        {
            if (m_Instance == null)
            {
                m_Instance = new MSSqlMng();
            }
            return m_Instance;
        }

        public int InsertQuery(string strQuery)
        {
            try
            {
                lock (this)
                {
                    this.m_sqlCommand.CommandText = strQuery;
                    SqlDataAdapter adapter = new SqlDataAdapter
                    {
                        InsertCommand = new SqlCommand(strQuery, this.m_dbConnection)
                    };
                    return adapter.InsertCommand.ExecuteNonQuery();
                }
            }
            catch (Exception exception)
            {
                string param = string.Format("Query Error:{0} {1}", strQuery, exception.Message);
                this.EventListener(LOG_EVENT.ONERRORMSG, param);
                return 0;
            }
        }

        public int InsertQuery(string strQuery, string strField, string strTable)
        {
            int num2;
            try
            {
                lock (this)
                {
                    int num = 0;
                    num = this.InsertQuery(strQuery);
                    if (num > 0)
                    {
                        string str = string.Format("SELECT MAX({0}) FROM {1}", strField, strTable);
                        num = int.Parse(this.SelectQuery(str)[0][0]);
                    }
                    num2 = num;
                }
            }
            catch (Exception exception)
            {
                string param = string.Format("Query :{0} {1}", strQuery, exception.Message);
                this.EventListener(LOG_EVENT.ONERRORMSG, param);
                num2 = 0;
            }
            return num2;
        }

        public string[][] SelectQuery(string strQuery)
        {
            try
            {
                string[][] strArray;
               
                    this.m_sqlCommand.CommandText = strQuery;
                    SqlDataAdapter adapter = new SqlDataAdapter
                    {
                        SelectCommand = new SqlCommand(strQuery, this.m_dbConnection)
                    };
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet);
                    int count = dataSet.Tables[0].Rows.Count;
                    int num2 = dataSet.Tables[0].Columns.Count;
                    strArray = new string[count][];
                    DataRowCollection rows = dataSet.Tables[0].Rows;
                    for (int i = 0; i < count; i++)
                    {
                        DataRow row = rows[i];
                        strArray[i] = new string[num2];
                        for (int j = 0; j < num2; j++)
                        {
                            strArray[i][j] = row[j].ToString();
                        }
                    }
                
                return strArray;
            }
            catch (Exception exception)
            {
                string param = string.Format("Query Error:{0} {1}", strQuery, exception.Message);
                this.EventListener(LOG_EVENT.ONERRORMSG, param);
                return null;
            }
        }

        public bool UpdateQuery(string strQuery)
        {
            try
            {
                lock (this)
                {
                    this.m_sqlCommand.CommandText = strQuery;
                    SqlDataAdapter adapter = new SqlDataAdapter
                    {
                        UpdateCommand = new SqlCommand(strQuery, this.m_dbConnection)
                    };
                    adapter.UpdateCommand.CommandTimeout = 5000;
                    adapter.UpdateCommand.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception exception)
            {
                string param = string.Format("Query Error:{0} {1}", exception.Message, strQuery);
                this.EventListener(LOG_EVENT.ONERRORMSG, param);
                return false;
            }
        }

        public string DBFileName
        {
            get
            {
                return this.m_strDBFileName;
            }
            set
            {
                this.m_strDBFileName = value;
            }
        }

        public string DBID
        {
            get
            {
                return this.m_strDBID;
            }
            set
            {
                this.m_strDBID = value;
            }
        }

        public string DBName
        {
            get
            {
                return this.m_strDBName;
            }
            set
            {
                this.m_strDBName = value;
            }
        }

        public string DBPwd
        {
            get
            {
                return this.m_strDBPwd;
            }
            set
            {
                this.m_strDBPwd = value;
            }
        }

        public string DBServer
        {
            get
            {
                return this.m_strDBServer;
            }
            set
            {
                this.m_strDBServer = value;
            }
        }

        public string DBService
        {
            get
            {
                return this.m_strDBService;
            }
            set
            {
                this.m_strDBService = value;
            }
        }

        public bool IsConnected
        {
            get
            {
                return this.m_bIsConnected;
            }
        }
    }
}

