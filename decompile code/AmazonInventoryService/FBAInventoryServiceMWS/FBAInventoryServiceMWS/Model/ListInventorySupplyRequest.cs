/******************************************************************************* 
 *  Copyright 2009 Amazon Services. All Rights Reserved.
 *  Licensed under the Apache License, Version 2.0 (the "License"); 
 *  
 *  You may not use this file except in compliance with the License. 
 *  You may obtain a copy of the License at: http://aws.amazon.com/apache2.0
 *  This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
 *  CONDITIONS OF ANY KIND, either express or implied. See the License for the 
 *  specific language governing permissions and limitations under the License.
 * ***************************************************************************** 
 * 
 *  FBA Inventory Service MWS CSharp Library
 *  API Version: 2010-10-01
 *  Generated: Fri Oct 22 09:53:30 UTC 2010 
 * 
 */

using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;


namespace FBAInventoryServiceMWS.Model
{
    [XmlTypeAttribute(Namespace = "http://mws.amazonaws.com/FulfillmentInventory/2010-10-01/")]
    [XmlRootAttribute(Namespace = "http://mws.amazonaws.com/FulfillmentInventory/2010-10-01/", IsNullable = false)]
    public class ListInventorySupplyRequest
    {
    
        private String sellerIdField;

        private String marketplaceField;

        private  SellerSkuList sellerSkusField;
        private DateTime? queryStartDateTimeField;

        private String responseGroupField;


        /// <summary>
        /// Gets and sets the SellerId property.
        /// </summary>
        [XmlElementAttribute(ElementName = "SellerId")]
        public String SellerId
        {
            get { return this.sellerIdField ; }
            set { this.sellerIdField= value; }
        }



        /// <summary>
        /// Sets the SellerId property
        /// </summary>
        /// <param name="sellerId">SellerId property</param>
        /// <returns>this instance</returns>
        public ListInventorySupplyRequest WithSellerId(String sellerId)
        {
            this.sellerIdField = sellerId;
            return this;
        }



        /// <summary>
        /// Checks if SellerId property is set
        /// </summary>
        /// <returns>true if SellerId property is set</returns>
        public Boolean IsSetSellerId()
        {
            return  this.sellerIdField != null;

        }


        /// <summary>
        /// Gets and sets the Marketplace property.
        /// </summary>
        [XmlElementAttribute(ElementName = "Marketplace")]
        public String Marketplace
        {
            get { return this.marketplaceField ; }
            set { this.marketplaceField= value; }
        }



        /// <summary>
        /// Sets the Marketplace property
        /// </summary>
        /// <param name="marketplace">Marketplace property</param>
        /// <returns>this instance</returns>
        public ListInventorySupplyRequest WithMarketplace(String marketplace)
        {
            this.marketplaceField = marketplace;
            return this;
        }



        /// <summary>
        /// Checks if Marketplace property is set
        /// </summary>
        /// <returns>true if Marketplace property is set</returns>
        public Boolean IsSetMarketplace()
        {
            return  this.marketplaceField != null;

        }


        /// <summary>
        /// Gets and sets the SellerSkus property.
        /// </summary>
        [XmlElementAttribute(ElementName = "SellerSkus")]
        public SellerSkuList SellerSkus
        {
            get { return this.sellerSkusField ; }
            set { this.sellerSkusField = value; }
        }



        /// <summary>
        /// Sets the SellerSkus property
        /// </summary>
        /// <param name="sellerSkus">SellerSkus property</param>
        /// <returns>this instance</returns>
        public ListInventorySupplyRequest WithSellerSkus(SellerSkuList sellerSkus)
        {
            this.sellerSkusField = sellerSkus;
            return this;
        }



        /// <summary>
        /// Checks if SellerSkus property is set
        /// </summary>
        /// <returns>true if SellerSkus property is set</returns>
        public Boolean IsSetSellerSkus()
        {
            return this.sellerSkusField != null;
        }




        /// <summary>
        /// Gets and sets the QueryStartDateTime property.
        /// </summary>
        [XmlElementAttribute(ElementName = "QueryStartDateTime")]
        public DateTime QueryStartDateTime
        {
            get { return this.queryStartDateTimeField.GetValueOrDefault() ; }
            set { this.queryStartDateTimeField= value; }
        }



        /// <summary>
        /// Sets the QueryStartDateTime property
        /// </summary>
        /// <param name="queryStartDateTime">QueryStartDateTime property</param>
        /// <returns>this instance</returns>
        public ListInventorySupplyRequest WithQueryStartDateTime(DateTime queryStartDateTime)
        {
            this.queryStartDateTimeField = queryStartDateTime;
            return this;
        }



        /// <summary>
        /// Checks if QueryStartDateTime property is set
        /// </summary>
        /// <returns>true if QueryStartDateTime property is set</returns>
        public Boolean IsSetQueryStartDateTime()
        {
            return  this.queryStartDateTimeField.HasValue;

        }


        /// <summary>
        /// Gets and sets the ResponseGroup property.
        /// </summary>
        [XmlElementAttribute(ElementName = "ResponseGroup")]
        public String ResponseGroup
        {
            get { return this.responseGroupField ; }
            set { this.responseGroupField= value; }
        }



        /// <summary>
        /// Sets the ResponseGroup property
        /// </summary>
        /// <param name="responseGroup">ResponseGroup property</param>
        /// <returns>this instance</returns>
        public ListInventorySupplyRequest WithResponseGroup(String responseGroup)
        {
            this.responseGroupField = responseGroup;
            return this;
        }



        /// <summary>
        /// Checks if ResponseGroup property is set
        /// </summary>
        /// <returns>true if ResponseGroup property is set</returns>
        public Boolean IsSetResponseGroup()
        {
            return  this.responseGroupField != null;

        }





    }

}