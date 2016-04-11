#region Copyright
//	Copyright (c) 2010 eBay, Inc.
//
//	This program is licensed under the terms of the eBay Common Development and 
//	Distribution License (CDDL) Version 1.0 (the "License") and any subsequent 
//	version thereof released by eBay.  The then-current version of the License 
//	can be found at https://www.codebase.ebay.com/Licenses.html and in the 
//	eBaySDKLicense file that is under the eBay SDK install directory.
#endregion

#region Namespaces
using System;
using System.Net;
using System.IO;
using NUnit.Framework;
using eBay.Service.Call;
using eBay.Service.Core.Soap;
using eBay.Service.Core.Sdk;
using eBay.Service.Util;
using eBay.Service.EPS;
#endregion

namespace AllTestsSuite.T_020_OtherTestsSuite
{
    [TestFixture]
    public class T_400_SiteHostedPictures :SOAPTestBase
    {

        private string GetPicture()
        {
            string fileName = "eBayLogoTM.gif";

            string picPath = Directory.GetCurrentDirectory() + "\\" + fileName;
            Console.WriteLine("\nGot picture file from :\n\t" + picPath);

            return picPath;
        }

        [Test]
        public void UploadSiteHostedPictures()
        {
            //test UploadSiteHostedPictures
            string pic = GetPicture();
            eBayPictureService eps = new eBayPictureService(this.apiContext);
            UploadSiteHostedPicturesRequestType request = new UploadSiteHostedPicturesRequestType();
            request.PictureWatermark = new PictureWatermarkCodeTypeCollection();
            request.PictureWatermark.Add(PictureWatermarkCodeType.User);
            UploadSiteHostedPicturesResponseType response = eps.UpLoadSiteHostedPicture(request, pic);
            Console.WriteLine("Picture is uploaded to : " + response.SiteHostedPictureDetails.FullURL);

            Assert.IsNotNull(response);
            Assert.IsTrue(response.Ack != AckCodeType.Failure);

            Assert.IsNotNull(response.SiteHostedPictureDetails);
            string picUrl = response.SiteHostedPictureDetails.FullURL;
            Assert.IsTrue(picUrl.Length > 0);

            ExtendSiteHostedPicturesCall eshpCall = new ExtendSiteHostedPicturesCall(this.apiContext);
            eshpCall.PictureURLList = new StringCollection(new String[] { picUrl });
            eshpCall.ExtensionInDays = 10;
            eshpCall.Execute();
            StringCollection picURLList = eshpCall.PictureURLListReturn;
        }
    }
}

