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
using System.Reflection;
using System.Text.RegularExpressions;

using eBay.Service.Core.Soap;
using eBay.Service.Core.Sdk;

namespace eBay.Service.SDK.Attribute
{
	/// <summary>
	/// Summary description for SimpleValidator.
	/// </summary>
	internal class SimpleValidator
	{
		private static Type _type;

		private static  int YEAR_LEN = 4;
		private static  int MONTH_DAY_LEN = 2;
		private static  String HIGH_YEAR = "9999";
		private static  String EMPTY_YEAR = "0000";
		private static  String HIGH_MONTH_DAY = "99";
		private static  String EMPTY_MONTH_DAY = "00";


		public SimpleValidator(){}

		private static Type getType()
		{
			if (_type == null) 
			{
				_type = typeof(eBay.Service.SDK.Attribute.SimpleValidator);
			}
			return _type;
		}

		public static object Validate(string method, object [] args)
		{
			Type type = getType();
			return Validate(type, method, args);
		}

		public static object Validate(Type type, string method, object [] args)
		{
			object obj = Activator.CreateInstance(type);
			MethodInfo info = type.GetMethod(method);
			if (info != null) 
			{
				return info.Invoke(obj, BindingFlags.InvokeMethod, null, args, null);
			}
			else 
			{
				ValidationResult result = new ValidationResult();
				result.ErrorMessage = "Validation rule " + method + " is not available.";
				return result;
			}
		}

		//TODO
		public static ValidationResult DateNullCheckRule(int id, int cnt, string target, ValidationRule rule)
		{
			ValidationResult result = new ValidationResult();
			if (isValid(target)) 
			{
				result.Success = (new SimpleDate(target)).isValidDate();
			}
			result.ErrorMessage = "Please specify both To and From dates.";
			return result;
		}

		public static ValidationResult DateRangeRule(int id, int cnt, string target, ValidationRule rule)
		{
			ValidationResult result = new ValidationResult();
			if (isValid(target)) 
			{
				SimpleDate dt0 = new SimpleDate(target);
				SimpleDate dt1 = new SimpleDate(rule.min);
				SimpleDate dt2 = new SimpleDate(rule.max);
				result.Success = dt0.compare(dt1) >= 0 && dt0.compare(dt2) <= 0;
			}
			result.ErrorMessage = "DateRangeRule violation.";
			return result;
		}

		public static ValidationResult DateValidationRule(int id, int cnt, string target, ValidationRule rule)
		{
			ValidationResult result = new ValidationResult();
			if (isValid(target)) 
			{
				int full = rule.full;
				result.Success = checkDate(target, full);
			}
			result.ErrorMessage = "Argument must be a valid date.";
			return result;
		}

		private static bool checkDate(String date, int full) 
		{

			string day = null;
			string month = null;
			string year = null;
		
			bool result = true;
	    
			if(full == 1) 
			{
				year = date.Substring(0, YEAR_LEN);
				month = date.Substring(YEAR_LEN, MONTH_DAY_LEN);	
				day = date.Substring(YEAR_LEN + MONTH_DAY_LEN);
			
				if(isYearBad(year) || isMonthDayBad(month) || isMonthDayBad(day))
					result = false;
			}
		        
			return result;
		}
	
		private static bool isYearBad(string year) 
		{
			if(year.Equals(HIGH_YEAR) || year.Equals(EMPTY_YEAR))
				return true;
			if (isNonnumeric(year))
				return true;
			return false;
		}
	
		private static bool isMonthDayBad(string value) 
		{
			if(value.Equals(HIGH_MONTH_DAY) || value.Equals(EMPTY_MONTH_DAY))
				return true;
			if (isNonnumeric(value))
				return true;
			return false;			
		}

		private static bool isNonnumeric(string value)
		{
			for (int i = 0; i < value.Length; i++) 
			{
				if (!char.IsDigit(value[i]))
					return true;
			}
			return false;
		}

		public static ValidationResult DecimalSeparatorNotAllowedRule(int id, int cnt, string target, ValidationRule rule)
		{
			ValidationResult result = new ValidationResult();
			if (isValidId(id) && cnt > 0) 
			{
				result.Success = true;
			}
			else if (isValid(target)) 
			{
				string separator = rule.separator;
				result.Success = target.IndexOf(separator) == -1;
			}
			result.ErrorMessage = "Please enter a number with no decimal place.";
			return result;
		}

		public static ValidationResult DoubleRangeValidationRule(int id, int cnt, string target, ValidationRule rule)
		{
			ValidationResult result = new ValidationResult();
			if (isValidId(id) && cnt > 0) 
			{
				result.Success = true;
			}
			else if (isValid(target)) 
			{
				try 
				{
					double d = double.Parse(target);
					result.Success = double.Parse(rule.min) <= d && d <= double.Parse(rule.max);
				}
				catch(Exception) 
				{
					//Data parsing error
				}
			}
			else
				result.Success = true; // null is okay
			result.ErrorMessage = "Please enter a value between [" + rule.min + "] and [" + rule.max + "].";
			return result;
		}

		public static ValidationResult IntRangeValidationRule(int id, int cnt, string target, ValidationRule rule)
		{
			ValidationResult result = new ValidationResult();
			if (isValidId(id) && cnt > 0) 
			{
				result.Success = true;
			}
			else if (isValid(target)) 
			{
				try 
				{
					double val = double.Parse(target);
					result.Success = (val >= int.Parse(rule.min)) && (val <= int.Parse(rule.max));
				}
				catch(Exception) 
				{
				}
			}
			result.ErrorMessage = "Please enter an value between [" + rule.min + "] and [" + rule.max + "].";
			return result;
		}

		public static ValidationResult MaskCheckRule(int id, int cnt, string target, ValidationRule rule)
		{
			ValidationResult result = new ValidationResult();
			if (isValidId(id) && cnt > 0) 
			{
				result.Success = true;
			}
			else if (isValid(target)) 
			{
				char [] valArray = target.ToCharArray();
				char [] maskArray = rule.mask.ToCharArray();

				int len = valArray.Length;
				bool success = len != maskArray.Length;
				if (success) 
				{	
					char val;
					char mask;
					for(int i=0; i<len && success; i++) 
					{
						val = valArray[i];
						mask = maskArray[i];
			
						switch(mask) 
						{
							case 'X':
								// The value must be a number
								success = char.IsDigit(val);
								break;
							case 'A':
								// the value must be a letter
								success = char.IsLetter(val);
								break;
							default:
								// these characters must match	
								success = val == mask;
								break;	
						}
					}
				}

				result.Success = success;
			}
			result.ErrorMessage = "Please enter this value in the format requested.";
			return result;
		}

		public static ValidationResult MaxDoubleValidationRule(int id, int cnt, string target, ValidationRule rule)
		{
			ValidationResult result = new ValidationResult();
			if (isValidId(id) && cnt > 0) 
			{
				result.Success = true;
			}
			else if (isValid(target)) 
			{
				try 
				{
					result.Success = double.Parse(target) < double.Parse(rule.max);
				}
				catch(Exception) 
				{
				}
			}
			result.ErrorMessage = "Please enter a value less than [" + rule.max + "].";
			return result;
		}

		public static ValidationResult MaxIntValidationRule(int id, int cnt, string target, ValidationRule rule)
		{
			ValidationResult result = new ValidationResult();
			if (isValidId(id) && cnt > 0) 
			{
				result.Success = true;
			}
			else if (isValid(target)) 
			{
				try 
				{
					double val = double.Parse(target);
					result.Success = val < int.Parse(rule.max);
				}
				catch(Exception) 
				{
				}
			}

			result.ErrorMessage = "Please enter an value less than [" + rule.max + "].";
			return result;
		}

		public static ValidationResult MinDoubleValidationRule(int id, int cnt, string target, ValidationRule rule)
		{
			ValidationResult result = new ValidationResult();
			if (isValidId(id) && cnt > 0) 
			{
				result.Success = true;
			}
			else if (isValid(target)) 
			{
				try 
				{
					result.Success = double.Parse(target) > double.Parse(rule.min);
				}
				catch(Exception) 
				{
				}
			}

			result.ErrorMessage = "Please enter a value greater than [" + rule.min + "].";
			return result;
		}

		public static ValidationResult MinIntValidationRule(int id, int cnt, string target, ValidationRule rule)
		{
			ValidationResult result = new ValidationResult();
			if (isValidId(id) && cnt > 0) 
			{
				result.Success = true;
			}
			else if (isValid(target)) 
			{
				try 
				{
					double val = double.Parse(target);
					result.Success = val > int.Parse(rule.min);
				}
				catch(Exception) 
				{
				}
			}

			result.ErrorMessage = "Please enter an value greater than [" + rule.min + "].";
			return result;
		}

		public static ValidationResult MultiSelectMinNumberValuesRule(int id, int cnt, string target, ValidationRule rule)
		{
			ValidationResult result = new ValidationResult();
			try 
			{
				result.Success = cnt > int.Parse(rule.min);
			}
			catch(Exception) 
			{
			}

			result.ErrorMessage = "Please make no fewer than [" + rule.min + "] selections.";
			return result;
		}

		public static ValidationResult MultiSelectMaxNumberValuesRule(int id, int cnt, string target, ValidationRule rule)
		{
			ValidationResult result = new ValidationResult();
			result.Success = cnt < int.Parse(rule.max);
			result.ErrorMessage = "Please make no more than [" + rule.max + "] selections.";
			return result;
		}

		public static ValidationResult MultiSelectMinMaxNumberValuesRule(int id, int cnt, string target, ValidationRule rule)
		{
			ValidationResult result = new ValidationResult();
			result.Success = int.Parse(rule.min) < cnt && cnt < int.Parse(rule.max);
			result.ErrorMessage = "Please make between [" + rule.min + "] and [" + rule.max + "] selections.";
			return result;
		}

		public static ValidationResult NumberSeparatorNotAllowedRule(int id, int cnt, string target, ValidationRule rule)
		{
			ValidationResult result = new ValidationResult();
			if (isValidId(id) && cnt > 0) 
			{
				result.Success = true;
			}
			else if (isValid(target)) 
			{
				target = target.Trim();
				if (target.StartsWith("-")) 
				{
					target = target.Substring(1, target.Length - 1);
				}
				target = target.Trim();

				char [] ca = target.ToCharArray();
				int len = ca.Length;
				int i = 0;
				bool success = true;
				while (success && i < len) 
				{
					success = char.IsDigit(ca[i]);
					i ++;
				}

				result.Success = success;
			}
			result.ErrorMessage = "Please enter only numeric digits with no symbols.";
			return result;
		}

		public static ValidationResult PrecisionRule(int id, int cnt, string target, ValidationRule rule)
		{
			ValidationResult result = new ValidationResult();
			if (isValidId(id) && cnt > 0) 
			{
				result.Success = true;
			}
			else if (isNumber(target) != 0) 
			{
				result.Success = true;
			}

			result.ErrorMessage = "PrecisionRule violation.";
			return result;
		}

		public static ValidationResult RegularExpressionValidationRule(int id, int cnt, string target, ValidationRule rule)
		{
			ValidationResult result = new ValidationResult();
			if (isValidId(id) && cnt > 0) 
			{
				result.Success = true;
			}
			else if (isValid(target)) 
			{
				Regex regex = new Regex(rule.regex);
				MatchCollection mc = regex.Matches(target);
				result.Success = mc.Count > 0;
			}
			result.ErrorMessage = "Please enter this value in the format requested: " + rule.regex;
			return result;
		}

		public static ValidationResult RequiredRule(int id, int cnt, string target, ValidationRule rule)
		{
			ValidationResult result = new ValidationResult();
			result.Success = cnt > 0;
			result.ErrorMessage = "RequiredRule violation.";

			return result;
		}
		
		//TODO
		public static ValidationResult SimpleRuleSetRule (int id, int cnt, string target, ValidationRule rule)
		{
			ValidationResult result = new ValidationResult();
			if (isValidId(id) && cnt > 0) 
			{
				result.Success = true;
			}
			else 
			{
				result.Success = true;
			}
			return result;
		}

		public static ValidationResult StringLengthRule(int id, int cnt, string target, ValidationRule rule)
		{
			ValidationResult result = new ValidationResult();
			if (isValidId(id)) 
			{
				result.Success = true; // cnt > 0;
				result.ErrorMessage = "Please select a valid item.";
			}
			else if (isValid(target)) 
			{
				int len = target.Length;
				result.Success = (0 < len && len <= rule.length);
				result.ErrorMessage = "Please enter no more than [" + rule.length + "] character(s).";
			}
			else 
			{
				result.Success = true;
			}
			
			return result;
		}

		public static ValidationResult ThousandsSeparatorNotAllowedRule(int id, int cnt, string target, ValidationRule rule)
		{
			ValidationResult result = new ValidationResult();
			if (isValidId(id) && cnt > 0) 
			{
				result.Success = true;
			}
			else if (isValid(target)) 
			{
				string separator = rule.separator;
				result.Success = target.IndexOf(separator) == -1;
			}
			result.ErrorMessage = "Please enter a number with no thousands separators.";
			return result;
		}

		public static ValidationResult UrlValidationRule(int id, int cnt, string target, ValidationRule rule)
		{
			ValidationResult result = new ValidationResult();
			if (isValidId(id) && cnt > 0) 
			{
				result.Success = true;
			}
			else if (isValid(target)) 
			{
				result.Success = checkUri(target, rule.protocol, rule.invalidhost, rule.invalidhostoverride);
			}
			else
				result.Success = true;
			result.ErrorMessage = "UrlValidationRule violation.";
			return result;
		}

		public static bool checkUri(String theURL, String protocol, String invalidHost,
			String invalidHostOverride) 
		{
			Uri url;
		
			try 
			{
				url = new Uri(theURL);
		
			} 
			catch(UriFormatException) 
			{
				// this is an invalid url (i.e. doesn't contain "://").
				// bail out
				return false;
			}
		
			// validate protocol
			string URLProtocol = url.Scheme;
			if(URLProtocol.Equals("") || URLProtocol.IndexOf(protocol) == -1) 
			{
				// invalid protocol
				return false;
			}
		
			// validate the host
			string URLHost = url.Host;
			if(URLHost.Equals("")) 
			{
				return false;
		
			} 
			else if(invalidHost != null) 
			{
				if(URLHost.IndexOf(invalidHost) != -1) 
				{
					if(invalidHostOverride != null) 
					{
						// make an exception if an override was specified
						if(URLHost.IndexOf(invalidHostOverride) == -1) 
						{
							return false;
						}
					}	
				}
			} 
	
			// illegal chars
			string rawURL = url.ToString();
			if((rawURL.IndexOf("<") != -1)  ||
				(rawURL.IndexOf(">") != -1)  ||
				(rawURL.IndexOf("*") != -1)	||
				(rawURL.IndexOf(" ") != -1)) 
			{
				return false;
			}

			return true;
		}

		public static ValidationResult VinValidationRule(int id, int cnt, string target, ValidationRule rule)
		{
			ValidationResult result = new ValidationResult();
			if (isValidId(id) && cnt > 0) 
			{
				result.Success = true;
			}
			else if (isValid(target)) 
			{
				int year = 1981; // TODO!
				result.Success = checkVin(target, year);
			}
			return result;
		}
		private static bool checkVin(string vin, int year){

			int vinLength = 17;
			int checkSumDigit = 8;
			// the integer value is getting appended with 4 0's
			year = year/10000;
			if(year < 1981){
				return true;
			}
		
			if(vin == null ){
				return false;
			}
			vin = vin.Trim();
			// If not 17 characters or if null invalid
			if(vin.Length != vinLength){
				return false;
			}
			// Get the character array as we need to get weight of every character
			char[] vinChars = vin.ToUpper().ToCharArray();

			int weight = 0;
			int sum = 0;
			int per = 0;
			bool isValidVin = true;

			// Calculate the sum iterating thru each character
			for(int i = 0;i < vinLength; i++){
				per = 0;
				weight = 0;
				//weight factor
				if (i < (checkSumDigit - 1)){		// first 7 digits.
					weight = 8 - i;
				}
				else if (i == (checkSumDigit - 1)){	// 7th digit
					weight = 10;
				}
				else if (i == checkSumDigit){		// checksum - so ignore
					weight = 0;
				}
				else if (i > checkSumDigit){		// last 8 digits
					weight = 18 - i;
				}
				// If character is a digit set per as the value of character
				if(char.IsDigit(vinChars[i])){
					// take the integer value of the character
					string s = vinChars[i].ToString();
					per = int.Parse(s);
				}
				// Check if character is Letter
				else if(char.IsLetter(vinChars[i])){
					// If VIN contains 'I' or 'O' return invalid
					if(vinChars[i] == 'I' || vinChars[i] == 'O'){
						isValidVin = false;
					}
					else if(vinChars[i] == 'P'){
						per = 7;
					}
					else if(vinChars[i] == 'R'){
						per = 9;
					}
					else if(vinChars[i] < 'J' ){
						per = vinChars[i] - 'A' + 1;
					}
					else if(vinChars[i] < 'P' ){
						per = vinChars[i] - 'J' + 1;
					}
					else {
						per= vinChars[i] - 'S' + 2;
					}
				}
				// If neither digit not letter invalid
				else {
					isValidVin = false;
				}

				// If the character is valid add its weight to sum
				if(isValidVin){
					sum = sum + (per * weight);
				}
				// If invalid break the loop
				else {
					break;
				}

			} // End of for loop

			// if valid check for checksum
			if(isValidVin){
				per = 0;
				per = (sum/11)*11;
				sum = sum - per; // sum is remainder after /11
				if(sum <10){
					isValidVin = (vinChars[checkSumDigit] == ('0' + sum));
				}
				else {
					isValidVin = (vinChars[checkSumDigit] == 'X');
				}
			}
			return isValidVin;
		}

		public static ValidationResult MotorUKValidationRule(int id, int cnt, string target, ValidationRule rule)
		{
			ValidationResult result = new ValidationResult();
			if (isValidId(id) && cnt > 0) 
			{
				result.Success = true;
			}
			else if (isValid(target)) 
			{
				result.Success = checkUKVin(target);
			}
			return result;
		}

		private static bool checkUKVin(String vin)
		{

			int vinLength = 7;
	
			if(vin == null )
			{
				return false;
			}
			vin = vin.Trim();
			// If not 7 characters or if null invalid
			if(vin.Length > vinLength)
			{
				return false;
			}
			char[] chars = vin.ToCharArray();
			for (int ii = 0; ii < vin.Length; ii++) 
			{
				char ch = chars[ii]; 
				if (!(char.IsLetter(ch) || char.IsDigit(ch))) 
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="target"></param>
		/// <returns>
		/// 0 = false, 1 = decimal, -1 = integr
		/// </returns>
		public static int isNumber(string target)
		{
			if (! isValid(target)) 
			{
				return 0;
			}

			int idx = target.IndexOf(DOT);
			int len = target.Length;
			for (int i = 0; i < idx; i++) 
			{
				if (!Char.IsDigit(target, i)) 
				{
						return 0;
				}
			}
			for (int i = idx + 1; i < len; i++) 
			{
				if (!Char.IsDigit(target, i)) 
				{
					return 0;
				}
			}

			return idx < 0 ? idx : 1;
		}

		public static bool isValid(string target)
		{
			return target != null && target.Length > 0;
		}

		public static bool isValidId(int id)
		{
			AttributeTypes type = (AttributeTypes)id;
			return type == AttributeTypes.ATTR_ID || type == AttributeTypes.ATTR_IDS;
		}

		public static string CLASS_NAME = "Attributes.SimpleValidator";
		public const string COMMA = ",";
		public const string DOT = ".";
	}
}
