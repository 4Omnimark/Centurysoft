using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarketplaceWebService;
using MarketplaceWebService.Model;
using OmnimarkAmazon.Models;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using System.Data;
using com.ddmresources.EnumMetadata.Core.Attributes;

namespace OmnimarkAmazon
{
    public enum ReportTextType
    {
        CSV,
        TSV,
        XML
    }

    public class ReportTypeInfoAttribute : Attribute
    {
        public ReportTextType TextType;
    }

    public static partial class Library
    {
        public const string ReportTablePrefix = "zzzzReport_";

        [EnumInfoSpecification(InfoModeOptions.Static, typeof(ReportTypeInfoAttribute), typeof(ReportTypeInfoAttribute))]
        public enum ReportType
        {
            _GET_AFN_INVENTORY_DATA_,
            _GET_V2_SETTLEMENT_REPORT_DATA_FLAT_FILE_V2_,
            _GET_AMAZON_FULFILLED_SHIPMENTS_DATA_,
            _GET_CONVERGED_FLAT_FILE_ORDER_REPORT_DATA_,
            _GET_CONVERGED_FLAT_FILE_PENDING_ORDERS_DATA_,
            _GET_CONVERGED_FLAT_FILE_SOLD_LISTINGS_DATA_,
            _GET_FBA_ESTIMATED_FBA_FEES_TXT_DATA_,
            _GET_FBA_FULFILLMENT_CROSS_BORDER_INVENTORY_MOVEMENT_DATA_,
            _GET_FBA_FULFILLMENT_CURRENT_INVENTORY_DATA_,
            _GET_FBA_FULFILLMENT_CUSTOMER_RETURNS_DATA_,
            _GET_FBA_FULFILLMENT_CUSTOMER_SHIPMENT_PROMOTION_DATA_,
            _GET_FBA_FULFILLMENT_CUSTOMER_SHIPMENT_REPLACEMENT_DATA_,
            _GET_FBA_FULFILLMENT_CUSTOMER_SHIPMENT_SALES_DATA_,
            _GET_FBA_FULFILLMENT_CUSTOMER_TAXES_DATA_,
            _GET_FBA_FULFILLMENT_INBOUND_NONCOMPLIANCE_DATA_,
            _GET_FBA_FULFILLMENT_INVENTORY_ADJUSTMENTS_DATA_,
            _GET_FBA_FULFILLMENT_INVENTORY_HEALTH_DATA_,
            _GET_FBA_FULFILLMENT_INVENTORY_RECEIPTS_DATA_,
            _GET_FBA_FULFILLMENT_INVENTORY_SUMMARY_DATA_,
            _GET_FBA_FULFILLMENT_MONTHLY_INVENTORY_DATA_,
            _GET_FBA_HAZMAT_STATUS_CHANGE_DATA_,
            _GET_FBA_MYI_ALL_INVENTORY_DATA_,
            _GET_FBA_MYI_UNSUPPRESSED_INVENTORY_DATA_,
            _GET_FBA_RECOMMENDED_REMOVAL_DATA_,
            _GET_FLAT_FILE_ACTIONABLE_ORDER_DATA_,
            _GET_FLAT_FILE_ALL_ORDERS_DATA_BY_LAST_UPDATE_,
            _GET_FLAT_FILE_ALL_ORDERS_DATA_BY_ORDER_DATE_,
            _GET_FLAT_FILE_OPEN_LISTINGS_DATA_,
            _GET_FLAT_FILE_ORDERS_DATA_,
            _GET_V2_SETTLEMENT_REPORT_DATA_FLAT_FILE_,
            _GET_FLAT_FILE_PENDING_ORDERS_DATA_,
            _GET_MERCHANT_CANCELLED_LISTINGS_DATA_,
            _GET_MERCHANT_LISTINGS_DATA_,
            _GET_MERCHANT_LISTINGS_DATA_BACK_COMPAT_,
            _GET_MERCHANT_LISTINGS_DATA_LITER_,
            _GET_MERCHANT_LISTINGS_DATA_LITE_,
            _GET_MERCHANT_LISTINGS_DEFECT_DATA_,
            _GET_NEMO_MERCHANT_LISTINGS_DATA_,
            _GET_ORDERS_DATA_,
            _GET_PADS_PRODUCT_PERFORMANCE_OVER_TIME_DAILY_DATA_TSV_,
            _GET_PADS_PRODUCT_PERFORMANCE_OVER_TIME_DAILY_DATA_XML_,
            _GET_PADS_PRODUCT_PERFORMANCE_OVER_TIME_MONTHLY_DATA_TSV_,
            _GET_PADS_PRODUCT_PERFORMANCE_OVER_TIME_MONTHLY_DATA_XML_,
            _GET_PADS_PRODUCT_PERFORMANCE_OVER_TIME_WEEKLY_DATA_TSV_,
            _GET_PADS_PRODUCT_PERFORMANCE_OVER_TIME_WEEKLY_DATA_XML_,
            _GET_V2_SETTLEMENT_REPORT_DATA_XML_,
            _GET_PENDING_ORDERS_DATA_,
            _GET_SELLER_FEEDBACK_DATA_,
            _GET_XML_ALL_ORDERS_DATA_BY_LAST_UPDATE_,
            _GET_XML_ALL_ORDERS_DATA_BY_ORDER_DATE_
        }

        public enum ReportRequestStatus
        {
            _SUBMITTED_,
            _IN_PROGRESS_,
            _CANCELLED_,
            _DONE_,
            _DONE_NO_DATA_
        }

        public static List<ReportInfo> GetReportList(List<Throttler> Throttlers, AmazonAccount AmazonAccount, Nullable<ReportType> ReportType, List<string> ReportRequestIDs, Action<bool, string> Log)
        {
            MarketplaceWebServiceConfig config = new MarketplaceWebServiceConfig();
            config.ServiceURL = AmazonAccount.Country.ServiceURL;

            MarketplaceWebServiceClient service = new MarketplaceWebServiceClient(applicationName, applicationVersion, AmazonAccount.AccessKeyID, AmazonAccount.SecretAccessKey, config);

            if (Log != null)
                Log(false, "Retreiving Report Items for " + AmazonAccount.Name + " ");

            GetReportListRequest request = new GetReportListRequest().WithMerchant(AmazonAccount.MerchantID);

            if (ReportType != null)
            {
                TypeList tl = new TypeList();
                tl.Type.Add(ReportType.ToString());
            
                request = request.WithReportTypeList(tl);
            }

            if (ReportRequestIDs != null)
            {
                IdList il = new IdList();

                foreach (string rrid in ReportRequestIDs)
                    il.Id.Add(rrid);

                request = request.WithReportRequestIdList(il);
            }
            
            GetReportListResponse response = service.GetReportList(request);

            GetReportListResult getReportListResult = response.GetReportListResult;

            List<ReportInfo> rtn = new List<ReportInfo>();

            bool ResultIsSetReportInfo = getReportListResult.IsSetReportInfo();
            bool ResultIsSetNextReportInfo = getReportListResult.IsSetNextToken();
            List<ReportInfo> ReportList = getReportListResult.ReportInfo;
            string NextToken = getReportListResult.NextToken;

            while (true)
            {
                if (ResultIsSetReportInfo)
                {
                    if (Log != null)
                        Log(true, "Got " + ReportList.Count.ToString());

                    foreach (ReportInfo ri in ReportList)
                        rtn.Add(ri);
                }
                else
                    if (Log != null)
                        Log(true, "Got none");

                if (!ResultIsSetNextReportInfo)
                    break;

                if (NextToken == "")
                    break;

                GetReportListByNextTokenRequest request2 = new GetReportListByNextTokenRequest().WithMerchant(AmazonAccount.MerchantID);

                request2.NextToken = NextToken;

                if (Log != null)
                    Log(false, "Retreiving More Report Items for " + AmazonAccount.Name + " ");

                foreach (Throttler Throttler in Throttlers)
                    if (Throttler != null)
                        Throttler.DoWait(Log);

                GetReportListByNextTokenResponse response2 = service.GetReportListByNextToken(request2);
                GetReportListByNextTokenResult getReportListResult2 = response2.GetReportListByNextTokenResult;

                ResultIsSetReportInfo = getReportListResult2.IsSetReportInfo();
                ResultIsSetNextReportInfo = getReportListResult2.IsSetNextToken();
                ReportList = getReportListResult2.ReportInfo;
                NextToken = getReportListResult2.NextToken;

            }

            return rtn;

        }

        public static GetReportResult GetReport(AmazonAccount AmazonAccount, string ReportID, Stream ReportStream, Action<bool, string> Log)
        {
            MarketplaceWebServiceConfig config = new MarketplaceWebServiceConfig();
            config.ServiceURL = AmazonAccount.Country.ServiceURL;

            MarketplaceWebServiceClient service = new MarketplaceWebServiceClient(applicationName, applicationVersion, AmazonAccount.AccessKeyID, AmazonAccount.SecretAccessKey, config);

            GetReportRequest request = new GetReportRequest().WithReportId(ReportID).WithMerchant(AmazonAccount.MerchantID);
            request.Report = ReportStream;

            Log(false, String.Format("Downloading Report {0}...", ReportID));
            GetReportResponse response = service.GetReport(request);
            Log(true, "Done!");

            GetReportResult result = response.GetReportResult;

            return result;

        }

        public static DataTable ReportStreamToDataTable(Stream ReportStream)
        {
            return Startbutton.Library.TextStreamToDataTable(ReportStream, "\t", false, true);
        }

        public static void WriteReportToTable(string ReportID, DataTable ReportData, bool WithUniqueClusteredGuid, Action<bool, string> Log)
        {
            string TableName = ReportTablePrefix + ReportID;

            Startbutton.SqlTableCreator.CreateFromDataTable(ReportData, "Main", TableName, WithUniqueClusteredGuid, Log);

            Log(true, "Table " + TableName + " created.");
        }

        public static string RequestReport(AmazonAccount AmazonAccount, ReportType ReportType, Nullable<DateTime> StartDate, Nullable<DateTime> EndDate, string Options, Action<bool, string> Log)
        {
            MarketplaceWebServiceConfig config = new MarketplaceWebServiceConfig();
            config.ServiceURL = AmazonAccount.Country.ServiceURL;

            MarketplaceWebServiceClient service = new MarketplaceWebServiceClient(applicationName, applicationVersion, AmazonAccount.AccessKeyID, AmazonAccount.SecretAccessKey, config);

            RequestReportRequest request = new RequestReportRequest().WithMerchant(AmazonAccount.MerchantID).WithReportType(ReportType.ToString());

            if (StartDate != null)
                request.StartDate = (DateTime)StartDate;

            if (EndDate != null)
                request.EndDate = (DateTime)EndDate;

            if (Options != null)
                request.ReportOptions = Options;

            RequestReportResponse response = service.RequestReport(request);

            RequestReportResult result = response.RequestReportResult;

            return result.ReportRequestInfo.ReportRequestId;
        }

        public static List<ReportRequestInfo> GetReportRequestList(List<Throttler> Throttlers, AmazonAccount AmazonAccount, List<string> ReportRequestIDs, Action<bool, string> Log)
        {
            MarketplaceWebServiceConfig config = new MarketplaceWebServiceConfig();
            config.ServiceURL = AmazonAccount.Country.ServiceURL;

            MarketplaceWebServiceClient service = new MarketplaceWebServiceClient(applicationName, applicationVersion, AmazonAccount.AccessKeyID, AmazonAccount.SecretAccessKey, config);

            if (Log != null)
                Log(false, "Retreiving Report Requests for " + AmazonAccount.Name + "... ");

            IdList il = new IdList();

            foreach (string ReportRequestID in ReportRequestIDs)
                il.Id.Add(ReportRequestID);

            GetReportRequestListRequest request = new GetReportRequestListRequest().WithMerchant(AmazonAccount.MerchantID).WithReportRequestIdList(il);

            GetReportRequestListResponse response = null;

            foreach (Throttler Throttler in Throttlers)
                if (Throttler != null)
                    Throttler.DoWait(Log);

            try
            {
                response = service.GetReportRequestList(request);
            }
            catch (Exception Ex)
            {
                Log(true, "ERROR: " + Ex.Message);
                return new List<ReportRequestInfo>();
            }

            GetReportRequestListResult GetReportRequestListResult = response.GetReportRequestListResult;

            List<ReportRequestInfo> rtn = new List<ReportRequestInfo>();

            bool ResultIsSetReportInfo = GetReportRequestListResult.IsSetReportRequestInfo();
            bool ResultIsSetNextReportInfo = GetReportRequestListResult.IsSetNextToken();
            List<ReportRequestInfo> ReportRequestList = GetReportRequestListResult.ReportRequestInfo;
            string NextToken = GetReportRequestListResult.NextToken;

            while (true)
            {
                if (ResultIsSetReportInfo)
                {
                    if (Log != null)
                        Log(true, "Got " + ReportRequestList.Count.ToString());

                    foreach (ReportRequestInfo ri in ReportRequestList)
                        rtn.Add(ri);
                }
                else
                    if (Log != null)
                        Log(true, "Got none");

                if (!ResultIsSetNextReportInfo)
                    break;

                if (NextToken == "")
                    break;

                GetReportRequestListByNextTokenRequest request2 = new GetReportRequestListByNextTokenRequest().WithMerchant(AmazonAccount.MerchantID);

                request2.NextToken = NextToken;

                if (Log != null)
                    Log(false, "Retreiving More Report Requests for " + AmazonAccount.Name + "... ");

                foreach (Throttler Throttler in Throttlers)
                    if (Throttler != null)
                        Throttler.DoWait(Log);

                GetReportRequestListByNextTokenResponse response2 = service.GetReportRequestListByNextToken(request2);
                GetReportRequestListByNextTokenResult GetReportRequestListResult2 = response2.GetReportRequestListByNextTokenResult;

                ResultIsSetReportInfo = GetReportRequestListResult2.IsSetReportRequestInfo();
                ResultIsSetNextReportInfo = GetReportRequestListResult2.IsSetNextToken();
                ReportRequestList = GetReportRequestListResult2.ReportRequestInfo;
                NextToken = GetReportRequestListResult2.NextToken;

            }

            return rtn;

        }

    }
}
