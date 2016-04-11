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
 * Inbound Shipment Item
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
    public class InboundShipmentItem : AbstractMwsObject
    {

        private string _shipmentId;
        private string _sellerSKU;
        private string _fulfillmentNetworkSKU;
        private decimal _quantityShipped;
        private decimal? _quantityReceived;
        private decimal? _quantityInCase;

        /// <summary>
        /// Gets and sets the ShipmentId property.
        /// </summary>
        [XmlElementAttribute(ElementName = "ShipmentId")]
        public string ShipmentId
        {
            get { return this._shipmentId; }
            set { this._shipmentId = value; }
        }

        /// <summary>
        /// Sets the ShipmentId property.
        /// </summary>
        /// <param name="shipmentId">ShipmentId property.</param>
        /// <returns>this instance.</returns>
        public InboundShipmentItem WithShipmentId(string shipmentId)
        {
            this._shipmentId = shipmentId;
            return this;
        }

        /// <summary>
        /// Checks if ShipmentId property is set.
        /// </summary>
        /// <returns>true if ShipmentId property is set.</returns>
        public bool IsSetShipmentId()
        {
            return this._shipmentId != null;
        }

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
        public InboundShipmentItem WithSellerSKU(string sellerSKU)
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
        public InboundShipmentItem WithFulfillmentNetworkSKU(string fulfillmentNetworkSKU)
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
        /// Gets and sets the QuantityShipped property.
        /// </summary>
        [XmlElementAttribute(ElementName = "QuantityShipped")]
        public decimal QuantityShipped
        {
            get { return this._quantityShipped; }
            set { this._quantityShipped = value; }
        }

        /// <summary>
        /// Sets the QuantityShipped property.
        /// </summary>
        /// <param name="quantityShipped">QuantityShipped property.</param>
        /// <returns>this instance.</returns>
        public InboundShipmentItem WithQuantityShipped(decimal quantityShipped)
        {
            this._quantityShipped = quantityShipped;
            return this;
        }

        /// <summary>
        /// Checks if QuantityShipped property is set.
        /// </summary>
        /// <returns>true if QuantityShipped property is set.</returns>
        public bool IsSetQuantityShipped()
        {
            return this._quantityShipped != null;
        }

        /// <summary>
        /// Gets and sets the QuantityReceived property.
        /// </summary>
        [XmlElementAttribute(ElementName = "QuantityReceived")]
        public decimal QuantityReceived
        {
            get { return this._quantityReceived.GetValueOrDefault(); }
            set { this._quantityReceived = value; }
        }

        /// <summary>
        /// Sets the QuantityReceived property.
        /// </summary>
        /// <param name="quantityReceived">QuantityReceived property.</param>
        /// <returns>this instance.</returns>
        public InboundShipmentItem WithQuantityReceived(decimal quantityReceived)
        {
            this._quantityReceived = quantityReceived;
            return this;
        }

        /// <summary>
        /// Checks if QuantityReceived property is set.
        /// </summary>
        /// <returns>true if QuantityReceived property is set.</returns>
        public bool IsSetQuantityReceived()
        {
            return this._quantityReceived != null;
        }

        /// <summary>
        /// Gets and sets the QuantityInCase property.
        /// </summary>
        [XmlElementAttribute(ElementName = "QuantityInCase")]
        public decimal QuantityInCase
        {
            get { return this._quantityInCase.GetValueOrDefault(); }
            set { this._quantityInCase = value; }
        }

        /// <summary>
        /// Sets the QuantityInCase property.
        /// </summary>
        /// <param name="quantityInCase">QuantityInCase property.</param>
        /// <returns>this instance.</returns>
        public InboundShipmentItem WithQuantityInCase(decimal quantityInCase)
        {
            this._quantityInCase = quantityInCase;
            return this;
        }

        /// <summary>
        /// Checks if QuantityInCase property is set.
        /// </summary>
        /// <returns>true if QuantityInCase property is set.</returns>
        public bool IsSetQuantityInCase()
        {
            return this._quantityInCase != null;
        }


        public override void ReadFragmentFrom(IMwsReader reader)
        {
            _shipmentId = reader.Read<string>("ShipmentId");
            _sellerSKU = reader.Read<string>("SellerSKU");
            _fulfillmentNetworkSKU = reader.Read<string>("FulfillmentNetworkSKU");
            _quantityShipped = reader.Read<decimal>("QuantityShipped");
            _quantityReceived = reader.Read<decimal?>("QuantityReceived");
            _quantityInCase = reader.Read<decimal?>("QuantityInCase");
        }

        public override void WriteFragmentTo(IMwsWriter writer)
        {
            writer.Write("ShipmentId", _shipmentId);
            writer.Write("SellerSKU", _sellerSKU);
            writer.Write("FulfillmentNetworkSKU", _fulfillmentNetworkSKU);
            writer.Write("QuantityShipped", _quantityShipped);
            writer.Write("QuantityReceived", _quantityReceived);
            writer.Write("QuantityInCase", _quantityInCase);
        }

        public override void WriteTo(IMwsWriter writer)
        {
            writer.Write("http://mws.amazonaws.com/FulfillmentInboundShipment/2010-10-01/", "InboundShipmentItem", this);
        }

        public InboundShipmentItem() : base()
        {
        }
    }
}
