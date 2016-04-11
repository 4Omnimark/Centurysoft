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
using System.Runtime.InteropServices;

namespace eBay.Service.SDK.Attribute
{
	internal interface IValidationResult
	{
		int ErrorCode
		{get;set;}

		bool Success
		{get;set;}

		string ErrorMessage
		{get;set;}
	}

	/// <summary>
	/// Summary description for ValidationResult.
	/// </summary>
	[ClassInterface(ClassInterfaceType.None)]
	internal class ValidationResult : IValidationResult
	{
		private int errorCode;
		private bool success;
		private string errorMessage;

		public ValidationResult()
		{
		}

		public int ErrorCode
		{
			get {return this.errorCode;}
			set {this.errorCode = value;}
		}
		
		public bool Success
		{
			get {return this.success;}
			set {this.success = value;}
		}

		public string ErrorMessage
		{
			get {return this.errorMessage;}
			set {this.errorMessage = value;}
		}
	}
}
