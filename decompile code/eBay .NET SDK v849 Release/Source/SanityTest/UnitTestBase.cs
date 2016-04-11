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
using NUnit.Framework;

namespace AllTestsSuite
{
	/// <summary>
	/// Summary description for UnitTestBase.
	/// </summary>
	/// 
	//[TestFixture]
	public class UnitTestBase
	{
		public UnitTestBase()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		[TestFixtureSetUp]
		public virtual void TestFixtureSetup(){}

		[TestFixtureTearDown]
		public virtual void TestFixtureTearDown(){}

		[SetUp]
		public virtual void Setup(){}

		[TearDown]
		public virtual void TearDown(){}
	}
}
