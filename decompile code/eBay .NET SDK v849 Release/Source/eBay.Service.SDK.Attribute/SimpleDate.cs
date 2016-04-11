#region Copyright
//	Copyright (c) 2013 eBay, Inc.
//	
//	This program is licensed under the terms of the eBay Common Development and
//	Distribution License (CDDL) Version 1.0 (the "License") and any subsequent  
//	version thereof released by eBay.  The then-current version of the License can be 
//	found at http://www.opensource.org/licenses/cddl1.php and in the eBaySDKLicense 
//	file that is under the eBay SDK ../docs directory
#endregion

using System;

namespace eBay.Service.SDK.Attribute
{
	/// <summary>
	/// Summary description for SimpleDate.
	/// The suported date formats by this class are:
	/// YYYY
	/// YYYY-MM
	/// YYYYMMDD
	/// YYYY-MM-DD
	/// </summary>
	internal class SimpleDate
	{
		int day;
		int month;
		int year;
		int type;

		public SimpleDate(string date)
		{
			init(date);
		}

		public int compare(SimpleDate dt)
		{
			int diff = this.year - dt.year;
			if (diff != 0)
				return diff;

			diff = this.month - dt.month;

			if (diff != 0)
				return diff;

			return this.day - dt.day;
		}

		protected void init(string date)
		{
			this.type = getDateFormat(date);

			if (this.type == DT_UNKNOWN)
				return;

			switch (this.type)
			{
				case DT_YEAR_MONTH:
					int index = date.IndexOf(DASH);
					this.year = int.Parse(date.Substring(0, index));
					this.month = int.Parse(date.Substring(index + 1));
					break;
				case DT_YEARMONTHDAY:
					string s = date.Substring(0, LEN_YEAR);
					this.year = int.Parse(s);
					s = date.Substring(LEN_YEAR, LEN_MONTH);
					this.month = int.Parse(s);
					s = date.Substring(IDX_DASH2);
					this.day = int.Parse(s);
					break;
				case DT_YEAR_MONTH_DAY:
					s = date.Substring(0, LEN_YEAR);
					this.year = int.Parse(s);
					s = date.Substring(LEN_YEAR + 1, LEN_MONTH);
					this.month = int.Parse(s);
					s = date.Substring(IDX_DASH2 + 1);
					this.day = int.Parse(s);
					break;
				default:
					this.year = int.Parse(date);
					break;
			}
		}

		public int getDateFormat(string date)
		{
			int len = date.Length;

			if (len == DT_YEAR)
				return DT_YEAR;
			else if (len == DT_YEAR_MONTH && date.IndexOf(DASH) == IDX_DASH1)
				return DT_YEAR_MONTH;
			else if (len ==  DT_YEARMONTHDAY && date.IndexOf(DASH) == -1)
				return DT_YEARMONTHDAY;
			else if (len == DT_YEAR_MONTH_DAY && date.IndexOf(DASH) == IDX_DASH1 && date.LastIndexOf(DASH) == IDX_DASH2)
				return DT_YEAR_MONTH_DAY;
			else 
				return DT_UNKNOWN;
		}

		public int getDay()
		{
			return this.day;
		}

		public int getMonth()
		{
			return this.month;
		}

		public int getYear()
		{
			return this.year;
		}

		public bool isValidDate()
		{
			return isValidDay() && isValidMonth() && isValidYear();
		}

		public bool isValidDay()
		{
			if(this.type == DT_YEAR || this.type == DT_YEAR_MONTH)
				return true;

			if (0 >= this.day || this.day > 31 )
				return false;

			if (this.month == FEB || 
				this.month == APR ||
				this.month == JUN ||
				this.month == SEP ||
				this.month == NOV
				) 
			{
				return this.day < 31;
			}
			else if (this.month == FEB) 
			{
				if(this.year % 4 != 0)
				{
					return this .day < 29;
				}
				else 
				{
					return this.day <= 29;
				}
			}

			return true;
		}

		public bool isValidMonth()
		{
			if (this.type == DT_YEAR)
				return true;
			else
				return JAN <= this.month && this.month <= DEC;
		}

		public bool isValidYear()
		{
			return 0 <= this.year && this.year < 9999;
		}

		const int DT_UNKNOWN = -1;
		const int DT_YEAR = 4;
		const int DT_YEAR_MONTH = 7;
		const int DT_YEARMONTHDAY = 8;
		const int DT_YEAR_MONTH_DAY = 10;

		const int LEN_YEAR = 4;
		const int LEN_MONTH = 2;
		const int LEN_DAY = 2;

		const int IDX_DASH1 = 4;
		const int IDX_DASH2 = 7;

		private const int JAN = 1;
		private const int FEB = 2;
		private const int APR = 4;
		private const int JUN = 6;
		private const int SEP = 9;
		private const int NOV = 11;
		private const int DEC = 12;

		private const int ONE = 1;
		private const int THIRTY_ONE = 31;

		private const string DASH = "-";
	}
}
