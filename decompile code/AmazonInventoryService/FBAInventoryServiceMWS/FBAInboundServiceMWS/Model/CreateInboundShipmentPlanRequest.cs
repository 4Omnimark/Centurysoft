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
 * Create Inbound Shipment Plan Request
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
    public class CreateInboundShipmentPlanRequest : AbstractMwsObject
    {

        private string _sellerId;
        private string _marketplace;
        private Address _shipFromAddress;
        private string _labelPrepPreference;
        private string _shipToCountryCode;
        private InboundShipmentPlanRequestItemList _inboundShipmentPlanRequestItems;

        /// <summary>
        /// Gets and sets the SellerId property.
        /// </summary>
        [XmlElementAttribute(ElementName = "SellerId")]
        public string SellerId
        {
            get { return this._sellerId; }
            set { this._sellerId = value; }
        }

        /// <summary>
        /// Sets the SellerId property.
        /// </summary>
        /// <param name="sellerId">SellerId property.</param>
        /// <returns>this instance.</returns>
        public CreateInboundShipmentPlanRequest WithSellerId(string sellerId)
        {
            this._sellerId = sellerId;
            return this;
        }

        /// <summary>
        /// Checks if SellerId property is set.
        /// </summary>
        /// <returns>true if SellerId property is set.</returns>
        public bool IsSetSellerId()
        {
            return this._sellerId != null;
        }

        /// <summary>
        /// Gets and sets the Marketplace property.
        /// </summary>
        [XmlElementAttribute(ElementName = "Marketplace")]
        public string Marketplace
        {
            get { return this._marketplace; }
            set { this._marketplace = value; }
        }

        /// <summary>
        /// Sets the Marketplace property.
        /// </summary>
        /// <param name="marketplace">Marketplace property.</param>
        /// <returns>this instance.</returns>
        public CreateInboundShipmentPlanRequest WithMarketplace(string marketplace)
        {
            this._marketplace = marketplace;
            return this;
        }

        /// <summary>
        /// Checks if Marketplace property is set.
        /// </summary>
        /// <returns>true if Marketplace property is set.</returns>
        public bool IsSetMarketplace()
        {
            return this._marketplace != null;
        }

        /// <summary>
        /// Gets and sets the ShipFromAddress property.
        /// </summary>
        [XmlElementAttribute(ElementName = "ShipFromAddress")]
        public Address ShipFromAddress
        {
            get { return this._shipFromAddress; }
            set { this._shipFromAddress = value; }
        }

        /// <summary>
        /// Sets the ShipFromAddress property.
        /// </summary>
        /// <param name="shipFromAddress">ShipFromAddress property.</param>
        /// <returns>this instance.</returns>
        public CreateInboundShipmentPlanRequest WithShipFromAddress(Address shipFromAddress)
        {
            this._shipFromAddress = shipFromAddress;
            return this;
        }

        /// <summary>
        /// Checks if ShipFromAddress property is set.
        /// </summary>
        /// <returns>true if ShipFromAddress property is set.</returns>
        public bool IsSetShipFromAddress()
        {
            return this._shipFromAddress != null;
        }

        /// <summary>
        /// Gets and sets the LabelPrepPreference property.
        /// </summary>
        [XmlElementAttribute(ElementName = "LabelPrepPreference")]
        public string LabelPrepPreference
        {
            get { return this._labelPrepPreference; }
            set { this._labelPrepPreference = value; }
        }

        /// <summary>
        /// Sets the LabelPrepPreference property.
        /// </summary>
        /// <param name="labelPrepPreference">LabelPrepPreference property.</param>
        /// <returns>this instance.</returns>
        public CreateInboundShipmentPlanRequest WithLabelPrepPreference(string labelPrepPreference)
        {
            this._labelPrepPreference = labelPrepPreference;
            return this;
        }

        /// <summary>
        /// Checks if LabelPrepPreference property is set.
        /// </summary>
        /// <returns>true if LabelPrepPreference property is set.</returns>
        public bool IsSetLabelPrepPreference()
        {
            return this._labelPrepPreference != null;
        }

        /// <summary>
        /// Gets and sets the ShipToCountryCode property.
        /// </summary>
        [XmlElementAttribute(ElementName = "ShipToCountryCode")]
        public string ShipToCountryCode
        {
            get { return this._shipToCountryCode; }
            set { this._shipToCountryCode = value; }
        }

        /// <summary>
        /// Sets the ShipToCountryCode property.
        /// </summary>
        /// <param name="shipToCountryCode">ShipToCountryCode property.</param>
        /// <returns>this instance.</returns>
        public CreateInboundShipmentPlanRequest WithShipToCountryCode(string shipToCountryCode)
        {
            this._shipToCountryCode = shipToCountryCode;
            return this;
        }

        /// <summary>
        /// Checks if ShipToCountryCode property is set.
        /// </summary>
        /// <returns>true if ShipToCountryCode property is set.</returns>
        public bool IsSetShipToCountryCode()
        {
            return this._shipToCountryCode != null;
        }

        /// <summary>
        /// Gets and sets the InboundShipmentPlanRequestItems property.
        /// </summary>
        [XmlElementAttribute(ElementName = "InboundShipmentPlanRequestItems")]
        public InboundShipmentPlanRequestItemList InboundShipmentPlanRequestItems
        {
            get { return this._inboundShipmentPlanRequestItems; }
            set { this._inboundShipmentPlanRequestItems = value; }
        }

        /// <summary>
        /// Sets the InboundShipmentPlanRequestItems property.
        /// </summary>
        /// <param name="inboundShipmentPlanRequestItems">InboundShipmentPlanRequestItems property.</param>
        /// <returns>this instance.</returns>
        public CreateInboundShipmentPlanRequest WithInboundShipmentPlanRequestItems(InboundShipmentPlanRequestItemList inboundShipmentPlanRequestItems)
        {
            this._inboundShipmentPlanRequestItems = inboundShipmentPlanRequestItems;
            return this;
        }

        /// <summary>
        /// Checks if InboundShipmentPlanRequestItems property is set.
        /// </summary>
        /// <returns>true if InboundShipmentPlanRequestItems property is set.</returns>
        public bool IsSetInboundShipmentPlanRequestItems()
        {
            return this._inboundShipmentPlanRequestItems != null;
        }


        public override void ReadFragmentFrom(IMwsReader reader)
        {
            _sellerId = reader.Read<string>("SellerId");
            _marketplace = reader.Read<string>("Marketplace");
            _shipFromAddress = reader.Read<Address>("ShipFromAddress");
            _labelPrepPreference = reader.Read<string>("LabelPrepPreference");
            _shipToCountryCode = reader.Read<string>("ShipToCountryCode");
            _inboundShipmentPlanRequestItems = reader.Read<InboundShipmentPlanRequestItemList>("InboundShipmentPlanRequestItems");
        }

        public override void WriteFragmentTo(IMwsWriter writer)
        {
            writer.Write("SellerId", _sellerId);
            writer.Write("Marketplace", _marketplace);
            writer.Write("ShipFromAddress", _shipFromAddress);
            writer.Write("LabelPrepPreference", _labelPrepPreference);
            writer.Write("ShipToCountryCode", _shipToCountryCode);
            writer.Write("InboundShipmentPlanRequestItems", _inboundShipmentPlanRequestItems);
        }

        public override void WriteTo(IMwsWriter writer)
        {
            writer.Write("http://mws.amazonaws.com/FulfillmentInboundShipment/2010-10-01/", "CreateInboundShipmentPlanRequest", this);
        }

        public CreateInboundShipmentPlanRequest() : base()
        {
        }
    }
}
