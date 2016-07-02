namespace ReviewProduct
{
    using System;
    using System.Collections;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;

    public class UKProhibition
    {
        public string m_strLogFile;
        public FileStream m_fileStream = null;
        static private UKProhibition m_instance = new UKProhibition();
        public delegate void LogEventHandler(LOG_EVENT evt, object param);

        static public UKProhibition GetInstance() { return m_instance; }
        public UKProhibition()
        {
            MSSqlMng.GetInstance().EventListener += new LogEvent(this.OnLogEvent);
        }

        public void Run()
        {
            if (MSSqlMng.GetInstance().ConnectDBServer())
            {
               // InitProcess();
              //  DoProcess();

                MSSqlMng.GetInstance().DisconnectDBServer();
            }

            AppendLog(LOG_EVENT.ONNOTICEMSG, "UK Prohibition ends");

            if (m_fileStream != null)
                m_fileStream.Close();
        }

        protected void InitProcess()
        {
            try
            {
                AppendLog(LOG_EVENT.ONNOTICEMSG, string.Format("Initializing Temp Lock ..."));
                MSSqlMng.GetInstance().UpdateQuery("update tbl_TempLock set lock = 0");
                AppendLog(LOG_EVENT.ONNOTICEMSG, string.Format("OK!"));
            }
            catch (Exception ex)
            {
                AppendLog(LOG_EVENT.ONERRORMSG, string.Format("Failed to initialize: {0}", ex.Message));
                return;
            }
        }

        private string GetProhibitedKey(int nIndex, string[][] res)
        {
            return res[nIndex][0];
        }

        private string GetProhibitedID(int nIndex, string[][] res)
        {
            return res[nIndex][1];
        }

        private string SpecialSymbolFix(string str)
        {
            if (str.Contains("'"))
                str = str.Replace("'", "''");

            return str;
        }

        private bool IsFirstLastNumber(string str)
        {
            if (str.Length == 0)
                return false;

            string str1st = str.Substring(0, 1);
            string strLast = str.Substring(str.Length - 1, 1);

            if (str1st == "0" || str1st == "1" || str1st == "2" || str1st == "3" || str1st == "4" || str1st == "5" || str1st == "6" || str1st == "7" || str1st == "8" || str1st == "9")
                return true;

            if (strLast == "0" || strLast == "1" || strLast == "2" || strLast == "3" || strLast == "4" || strLast == "5" || strLast == "6" || strLast == "7" || strLast == "8" || strLast == "9")
                return true;

            return false;
        }

        private bool IsFistLastNodeNumber(string str)
        {
            string []res = str.Split(" ".ToCharArray());
            if (res.Length < 2)
                return false;

            if (Converter.GetInstance().ToInt32(res[0]).ToString() == res[0])
                return true;

            if (Converter.GetInstance().ToInt32(res[res.Length - 1]).ToString() == res[res.Length - 1])
                return true;

            return false;
        }

        private bool InitMatchesTable()
        {
            try
            {
                AppendLog(LOG_EVENT.ONNOTICEMSG, string.Format("Deleting Contents of Keyword_Matches Table ..."));
                MSSqlMng.GetInstance().DeleteQuery("delete from tbl_keyword_matches");
                AppendLog(LOG_EVENT.ONNOTICEMSG, string.Format("OK!"));
            }
            catch (Exception ex)
            {
                AppendLog(LOG_EVENT.ONERRORMSG, string.Format("Failed to delete contents of Keyword_Matches Table: {0}", ex.Message));
                return false;
            }

            return true;
        }

        public void DoProcess(string strTableName)
        {
            string[][] result;
            try
            {
                AppendLog(LOG_EVENT.ONNOTICEMSG, string.Format("Fetching Prohibited Keys ..."));
                result = MSSqlMng.GetInstance().SelectQuery("select ProhibitedKeys, [ID] from tbl_Prohibited_Keywords");
                AppendLog(LOG_EVENT.ONNOTICEMSG, string.Format("OK!"));
            }
            catch (Exception ex)
            {
                AppendLog(LOG_EVENT.ONERRORMSG, string.Format("Failed to get Prohibited Keys: {0}", ex.Message));
                return;
            }

            if (result.Length == 0)
            {
                AppendLog(LOG_EVENT.ONERRORMSG, "Failed to get Prohibited Keywords");
                return;
            }          
 
//             if (InitMatchesTable() == false)
//             {
//                 return;
//             }

            int nKeyCount = result.Length;

            AppendLog(LOG_EVENT.ONNOTICEMSG, string.Format("{0} Prohibited Keywords", nKeyCount));

            bool bSuccess = true;
           

                //SetTempLock(strTableName, true);
                //AppendLog(LOG_EVENT.ONNOTICEMSG, string.Format("Checking Table: {0}", strTableName));

                for (int j = 0; j < nKeyCount; j++)
                {
                    string strWord = GetProhibitedKey(j, result);
                    string strID = GetProhibitedID(j, result);
                    UInt32 nID = Converter.GetInstance().ToUInt32(strID);

//                     if (nID > 255)
//                         nID = 255;

                    strWord.Trim();

                    string lstrWord = strWord;
                    lstrWord = lstrWord.ToLower();

                    strWord = SpecialSymbolFix(strWord);
                    bool bCompound = IsKeywordCompound(strWord);
                    bool bDeepSearch = false;


                    if (strWord.Contains("&") || lstrWord.Contains("some"))
                        bDeepSearch = true;
                    else if (bCompound)
                    {
                        //if (IsFirstLastNumber(strWord))
                        if (IsFistLastNodeNumber(strWord))
                        {
                            bDeepSearch = true;
                        }
                    }

                    strWord = strWord.Replace("\"", "");

                    string strQuery/*, strQueryS*/;

                    if (bDeepSearch)
                    {
//                         strQuery = string.Format("Update {0} set ReviewerPriority=1 where ReviewerPriority=0 and (" 
//                             + "title like '{1} %' or description like '{1} %' or brand like '{1} %' or Manufacturer like '{1} %' or features1 like '{1} %' or features2 like '{1} %' or features3 like '{1} %' or features4 like '{1} %'"
//                             + "or title like '% {1}' or description like '% {1}' or brand like '% {1}' or Manufacturer like '% {1}' or features1 like '% {1}' or features2 like '% {1}' or features3 like '% {1}' or features4 like '% {1}'"
//                             + "or title like '% {1} %' or description like '% {1} %' or brand like '% {1} %' or Manufacturer like '% {1} %' or features1 like '% {1} %' or features2 like '% {1} %' or features3 like '% {1} %' or features4 like '% {1} %'"
//                             + "or title like '% {1}.%' or description like '% {1}.%' or brand like '% {1}.%' or Manufacturer like '% {1}.%' or features1 like '% {1}.%' or features2 like '% {1}.%' or features3 like '% {1}.%' or features4 like '% {1}.%')"
//                             , strTableName, strWord, nID);

                        strQuery = string.Format("Update {0} set UK_Prohibited=1 where UK_Prohibited!=1 and ("
                            + "title like '{1} %' or description like '{1} %' or brand like '{1} %' or Manufacturer like '{1} %' or features1 like '{1} %' or features2 like '{1} %' or features3 like '{1} %' or features4 like '{1} %'"
                            + "or title like '% {1}' or description like '% {1}' or brand like '% {1}' or Manufacturer like '% {1}' or features1 like '% {1}' or features2 like '% {1}' or features3 like '% {1}' or features4 like '% {1}'"
                            + "or title like '% {1} %' or description like '% {1} %' or brand like '% {1} %' or Manufacturer like '% {1} %' or features1 like '% {1} %' or features2 like '% {1} %' or features3 like '% {1} %' or features4 like '% {1} %'"
                            + "or title like '% {1}.%' or description like '% {1}.%' or brand like '% {1}.%' or Manufacturer like '% {1}.%' or features1 like '% {1}.%' or features2 like '% {1}.%' or features3 like '% {1}.%' or features4 like '% {1}.%')"
                            , strTableName, strWord, nID);

//                         strQueryS = string.Format("Select asin from {0} where ReviewerPriority=0 and ("
//                             + "title like '{1} %' or description like '{1} %' or brand like '{1} %' or Manufacturer like '{1} %' or features1 like '{1} %' or features2 like '{1} %' or features3 like '{1} %' or features4 like '{1} %'"
//                             + "or title like '% {1}' or description like '% {1}' or brand like '% {1}' or Manufacturer like '% {1}' or features1 like '% {1}' or features2 like '% {1}' or features3 like '% {1}' or features4 like '% {1}'"
//                             + "or title like '% {1} %' or description like '% {1} %' or brand like '% {1} %' or Manufacturer like '% {1} %' or features1 like '% {1} %' or features2 like '% {1} %' or features3 like '% {1} %' or features4 like '% {1} %'"
//                             + "or title like '% {1}.%' or description like '% {1}.%' or brand like '% {1}.%' or Manufacturer like '% {1}.%' or features1 like '% {1}.%' or features2 like '% {1}.%' or features3 like '% {1}.%' or features4 like '% {1}.%')"
//                             , strTableName, strWord);
                    }
                    else if (bCompound)
                    {
                        //strQuery = string.Format("Update {0} set ReviewerPriority=1 where ReviewerPriority=0 and CONTAINS((title, description, brand, Manufacturer, features1, features2, features3, features4), '\"{1}\"')", strTableName, strWord, nID);
                        strQuery = string.Format("Update {0} set UK_Prohibited=1 where UK_Prohibited!=1 and CONTAINS((title, description, brand, Manufacturer, features1, features2, features3, features4), '\"{1}\"')", strTableName, strWord, nID);
                        //strQueryS = string.Format("select asin from {0} where ReviewerPriority=0 and CONTAINS((title, description, brand, Manufacturer, features1, features2, features3, features4), '\"{1}\"')", strTableName, strWord, nID);
                    }
                    else
                    {
//                         strQuery = string.Format("Update {0} set ReviewerPriority=1 where ReviewerPriority=0 and CONTAINS((title, description, brand, Manufacturer, features1, features2, features3, features4), '\"{1}\"')" 
//                         + " and title not like '%-{1}%' and description not like '%-{1}%' and brand not like '%-{1}%' and Manufacturer not like '%-{1}%' and features1 not like '%-{1}%' and features2 not like '%-{1}%' and features3 not like '%-{1}%' and features4 not like '%-{1}%'"
//                         + " and title not like '%{1}-%' and description not like '%{1}-%' and brand not like '%{1}-%' and Manufacturer not like '%{1}-%' and features1 not like '%{1}-%' and features2 not like '%{1}-%' and features3 not like '%{1}-%' and features4 not like '%{1}-%'"
//                         , strTableName, strWord, nID);
                        strQuery = string.Format("Update {0} set UK_Prohibited=1 where UK_Prohibited!=1 and CONTAINS((title, description, brand, Manufacturer, features1, features2, features3, features4), '\"{1}\"')"
                        + " and title not like '%-{1}%' and description not like '%-{1}%' and brand not like '%-{1}%' and Manufacturer not like '%-{1}%' and features1 not like '%-{1}%' and features2 not like '%-{1}%' and features3 not like '%-{1}%' and features4 not like '%-{1}%'"
                        + " and title not like '%{1}-%' and description not like '%{1}-%' and brand not like '%{1}-%' and Manufacturer not like '%{1}-%' and features1 not like '%{1}-%' and features2 not like '%{1}-%' and features3 not like '%{1}-%' and features4 not like '%{1}-%'"
                        , strTableName, strWord, nID);

//                         strQueryS = string.Format("select asin from {0} where ReviewerPriority=0 and CONTAINS((title, description, brand, Manufacturer, features1, features2, features3, features4), '\"{1}\"')"
//                         + " and title not like '%-{1}%' and description not like '%-{1}%' and brand not like '%-{1}%' and Manufacturer not like '%-{1}%' and features1 not like '%-{1}%' and features2 not like '%-{1}%' and features3 not like '%-{1}%' and features4 not like '%-{1}%'"
//                         + " and title not like '%{1}-%' and description not like '%{1}-%' and brand not like '%{1}-%' and Manufacturer not like '%{1}-%' and features1 not like '%{1}-%' and features2 not like '%{1}-%' and features3 not like '%{1}-%' and features4 not like '%{1}-%'"
//                         , strTableName, strWord, nID);
                    }

                    AppendLog(LOG_EVENT.ONNOTICEMSG, string.Format("{0}/{1} Prohibited Word at {2}, \"{3}\"", j + 1, nKeyCount, strTableName, strWord));

                    try
                    {
//                         AppendLog(LOG_EVENT.ONNOTICEMSG, string.Format("Selecting Query..."));
//                         string[][] res = MSSqlMng.GetInstance().SelectQuery(strQueryS);
// 
//                         if (res.Length == 0)
//                             continue;
// 
//                         for (int mm = 0; mm < res.Length; mm++)
//                         {
//                             string asin_temp = res[mm][0];
// 
//                             AppendLog(LOG_EVENT.ONNOTICEMSG, string.Format("...{0}/{1} Adding to tbl_keyword_matches -> \"{2}\"", mm + 1, res.Length, asin_temp));
//                             try
//                             {
//                                 MSSqlMng.GetInstance().InsertQuery(string.Format("insert into tbl_keyword_matches(id, asin) values({0}, '{1}')", nID, asin_temp));
//                             }
//                            catch (Exception ex)
//                             {
//                                 AppendLog(LOG_EVENT.ONERRORMSG, string.Format("Insert SQL Error: {0}", ex.Message));
//                             }
//                         }

 //                       AppendLog(LOG_EVENT.ONNOTICEMSG, string.Format("Updating Query..."));
                        MSSqlMng.GetInstance().UpdateQuery(strQuery);
                    }
                    catch (Exception ex)
                    {
                        AppendLog(LOG_EVENT.ONERRORMSG, string.Format("SQL Error: {0}", ex.Message));
                        bSuccess = false;
                    }
                }

                //SetTempLock(strTableName, false);

                //if (!bSuccess)
                //    break;
           

            if (bSuccess)
            {
                AppendLog(LOG_EVENT.ONNOTICEMSG, "Completed Successfully.");
            }
            else
                AppendLog(LOG_EVENT.ONNOTICEMSG, "Completion Failed");
        }

        protected void SetTempLock(string strTable, bool bSet)
        {
            try
            {
                MSSqlMng.GetInstance().UpdateQuery(string.Format("update tbl_TempLock set lock = {0} where tblname='{1}'",
                                bSet == true ? 1 : 0,
                                strTable));

                AppendLog(LOG_EVENT.ONNOTICEMSG, string.Format("Table Lock changed: {0}", strTable));
            }
            catch (Exception ex)
            {
                AppendLog(LOG_EVENT.ONERRORMSG, string.Format("Table Lock Error: {0}", ex.Message));
            }
        }
        protected bool IsKeywordCompound(string strKeyword)
        {
            return strKeyword.Contains(" ");
        }
        public void AppendLog(LOG_EVENT evt, string msg)
        {
            OnLogEvent(evt, msg);
        }

        public void OnLogEvent(LOG_EVENT evt, object param)
        {
            DateTime now = DateTime.Now;
            string str2 = string.Format("{0:D4}/{1:D2}/{2:D2} {3:D2}:{4:D2}:{5:D2} ->", new object[] { now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second });
            string str3 = " ";
            switch (evt)
            {
                case LOG_EVENT.ONERRORMSG:
                    str3 = " === ERROR === ";
                    break;

                case LOG_EVENT.ONDEBUGMSG:
                    str3 = " ### DEBUG ### ";
                    break;
            }
            string strMsg = string.Format("{0}{1}{2}\n", str2, str3, param.ToString());
            Console.WriteLine(strMsg);
            this.LogToFile(strMsg);
        }
        private void LogToFile(string strMsg)
        {
            try
            {
                if (this.m_strLogFile.Length == 0)
                {
                    return;
                }

                lock (this)
                {
                    this.m_fileStream = File.Open(this.m_strLogFile, FileMode.Append, FileAccess.Write, FileShare.Read);

                    StreamWriter writer = new StreamWriter(this.m_fileStream, Encoding.UTF8);
                    writer.Write(strMsg);
                    writer.Close();

                    this.m_fileStream.Close();
                }
            }
            catch (Exception exception)
            {
                string str = string.Format("Log add failed: {0}", exception.Message);
            }
        }


    }

}