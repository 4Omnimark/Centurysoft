#region Copyright
//	Copyright (c) 2013 eBay, Inc.
//	
//	This program is licensed under the terms of the eBay Common Development and
//	Distribution License (CDDL) Version 1.0 (the "License") and any subsequent  
//	version thereof released by eBay.  The then-current version of the License can be 
//	found at http://www.opensource.org/licenses/cddl1.php and in the eBaySDKLicense 
//	file that is under the eBay SDK ../docs directory
#endregion

#region Namespaces
using System;
using NUnit.Framework;
using eBay.Service.Call;
using eBay.Service.Core.Soap;
using eBay.Service.Core.Sdk;
using eBay.Service.Util;
using UnitTests.Helper;
#endregion

namespace AllTestsSuite.T_030_CategoryTestsSuite
{
	[TestFixture]
	public class T_120_GetCategorySpecificsLibrary : SOAPTestBase
	{
		[Test]
		public void GetCategorySpecifics()
		{
			GetCategorySpecificsCall api = new GetCategorySpecificsCall(this.apiContext);
			StringCollection catList = new StringCollection();
			int number;
			CategoryTypeCollection categories;
			RecommendationsTypeCollection recommendations;
			
			number=1;
			getCatList(number,ref catList,out categories);
			// Make API call.
			api.GetCategorySpecifics(catList);
			//check whether the call is success.
			Assert.IsTrue(api.ApiResponse.Ack == AckCodeType.Success || api.ApiResponse.Ack == AckCodeType.Warning,"the call is failure!");
			recommendations = api.RecommendationList;
			Assert.IsNotNull(recommendations);
			Assert.IsTrue(recommendations.Count >= 0);
			
			number=3;
			getCatList(number,ref catList,out categories);
			// Make API call.
			api.GetCategorySpecifics(catList);
			//check whether the call is success.
			Assert.IsTrue(api.ApiResponse.Ack == AckCodeType.Success || api.ApiResponse.Ack == AckCodeType.Warning,"the call is failure!");
			recommendations = api.RecommendationList;
			Assert.IsNotNull(recommendations);
			Assert.IsTrue(recommendations.Count >= 0);
			
			api.Name="Product Type";
			// Make API call.
			api.GetCategorySpecifics(catList);
			//check whether the call is success.
			Assert.IsTrue(api.ApiResponse.Ack == AckCodeType.Success || api.ApiResponse.Ack == AckCodeType.Warning,"the call is failure!");
			recommendations = api.RecommendationList;
			Assert.IsNotNull(recommendations);
			Assert.IsTrue(recommendations.Count >= 0);

			DateTime dateTime=new DateTime(2005,1,1);
			api.LastUpdateTime=dateTime;
			// Make API call.
			api.GetCategorySpecifics(catList);
			//check whether the call is success.
			Assert.IsTrue(api.ApiResponse.Ack == AckCodeType.Success || api.ApiResponse.Ack == AckCodeType.Warning,"the call is failure!");
			recommendations = api.RecommendationList;
			Assert.IsNotNull(recommendations);
		}

		private void getCatList(int number,ref StringCollection catList,out CategoryTypeCollection categories)
		{
			bool isSuccess;
			string message;

			catList.Clear();
			//get a leaf category
			isSuccess=CategoryHelper.GetCISSupportLeafCategory(number,out categories,this.apiContext,out message);
			Assert.IsTrue(isSuccess,message);
			for(int i=0;i<number;i++)
			{
				//add to catList
				catList.Add(categories[i].CategoryID);
			}
		}

	}
}