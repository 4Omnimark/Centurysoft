/*******************************************************************************
 * Copyright 2009-2013 Amazon Services. All Rights Reserved.
 * Licensed under the Apache License, Version 2.0 (the "License"); 
 *
 * You may not use this file except in compliance with the License. 
 * You may obtain a copy of the License at: http://aws.amazon.com/apache2.0
 * This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
 * CONDITIONS OF ANY KIND, either express or implied. See the License for the 
 * specific language governing permissions and limitations under the License.
 *******************************************************************************
 * Inbound Shipment Plan Item
 * API Version: 2010-10-01
 * Library Version: 2013-11-01
 * Generated: Fri Nov 08 22:04:54 GMT 2013
 */


using System;
using System.Xml;
using System.Xml.Serialization;
using MWSClientCsRuntime;

namespace FBAInboundServiceMWS.Model
{
    [XmlTypeAttribute(Namespace = "http://mws.amazonaws.com/FulfillmentInboundShipment/2010-10-01/")]
    [XmlRootAttribute(Namespace = "http://mws.amazonaws.com/FulfillmentInboundShipment/2010-10-01/", IsNullable = false)]
    public class InboundShipmentPlanItem : AbstractMwsObject
    {

        private string _sellerSKU;
        private string _fulfillmentNetworkSKU;
        private decimal? _quantity;

        /// <summary>
        /// Gets and sets the SellerSKU property.
        /// </summary>
        [XmlElementAttribute(ElementName = "SellerSKU")]
        public string SellerSKU
        {
            get { return this._sellerSKU; }
            set { this._sellerSKU = value; }
        }

        /// <summary>
        /// Sets the SellerSKU property.
        /// </summary>
        /// <param name="sellerSKU">SellerSKU property.</param>
        /// <returns>this instance.</returns>
        public InboundShipmentPlanItem WithSellerSKU(string sellerSKU)
        {
            this._sellerSKU = sellerSKU;
            return this;
        }

        /// <summary>
        /// Checks if SellerSKU property is set.
        /// </summary>
        /// <returns>true if SellerSKU property is set.</returns>
        public bool IsSetSellerSKU()
        {
            return this._sellerSKU != null;
        }

        /// <summary>
        /// Gets and sets the FulfillmentNetworkSKU property.
        /// </summary>
        [XmlElementAttribute(ElementName = "FulfillmentNetworkSKU")]
        public string FulfillmentNetworkSKU
        {
            get { return this._fulfillmentNetworkSKU; }
            set { this._fulfillmentNetworkSKU = value; }
        }

        /// <summary>
        /// Sets the FulfillmentNetworkSKU property.
        /// </summary>
        /// <param name="fulfillmentNetworkSKU">FulfillmentNetworkSKU property.</param>
        /// <returns>this instance.</returns>
        public InboundShipmentPlanItem WithFulfillmentNetworkSKU(string fulfillmentNetworkSKU)
        {
            this._fulfillmentNetworkSKU = fulfillmentNetworkSKU;
            return this;
        }

        /// <summary>
        /// Checks if FulfillmentNetworkSKU property is set.
        /// </summary>
        /// <returns>true if FulfillmentNetworkSKU property is set.</returns>
        public bool IsSetFulfillmentNetworkSKU()
        {
            return this._fulfillmentNetworkSKU != null;
        }

        /// <summary>
        /// Gets and sets the Quantity property.
        /// </summary>
        [XmlElementAttribute(ElementName = "Quantity")]
        public decimal Quantity
        {
            get { return this._quantity.GetValueOrDefault(); }
            set { this._quantity = value; }
        }

        /// <summary>
        /// Sets the Quantity property.
        /// </summary>
        /// <param name="quantity">Quantity property.</param>
        /// <returns>this instance.</returns>
        public InboundShipmentPlanItem WithQuantity(decimal quantity)
        {
            this._quantity = quantity;
            return this;
        }

        /// <summary>
        /// Checks if Quantity property is set.
        /// </summary>
        /// <returns>true if Quantity property is set.</returns>
        public bool IsSetQuantity()
        {
            return this._quantity != null;
        }


        public override void ReadFragmentFrom(IMwsReader reader)
        {
            _sellerSKU = reader.Read<string>("SellerSKU");
            _fulfillmentNetworkSKU = reader.Read<string>("FulfillmentNetworkSKU");
            _quantity = reader.Read<decimal?>("Quantity");
        }

        public override void WriteFragmentTo(IMwsWriter writer)
        {
            writer.Write("SellerSKU", _sellerSKU);
            writer.Write("FulfillmentNetworkSKU", _fulfillmentNetworkSKU);
            writer.Write("Quantity", _quantity);
        }

        public override void WriteTo(IMwsWriter writer)
        {
            writer.Write("http://mws.amazonaws.com/FulfillmentInboundShipment/2010-10-01/", "InboundShipmentPlanItem", this);
        }

        public InboundShipmentPlanItem() : base()
        {
        }
    }
}
