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
 * Inbound Shipment Info
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
    public class InboundShipmentInfo : AbstractMwsObject
    {

        private string _shipmentId;
        private string _shipmentName;
        private Address _shipFromAddress;
        private string _destinationFulfillmentCenterId;
        private string _shipmentStatus;
        private string _labelPrepType;
        private bool? _areCasesRequired;

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
        public InboundShipmentInfo WithShipmentId(string shipmentId)
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
        /// Gets and sets the ShipmentName property.
        /// </summary>
        [XmlElementAttribute(ElementName = "ShipmentName")]
        public string ShipmentName
        {
            get { return this._shipmentName; }
            set { this._shipmentName = value; }
        }

        /// <summary>
        /// Sets the ShipmentName property.
        /// </summary>
        /// <param name="shipmentName">ShipmentName property.</param>
        /// <returns>this instance.</returns>
        public InboundShipmentInfo WithShipmentName(string shipmentName)
        {
            this._shipmentName = shipmentName;
            return this;
        }

        /// <summary>
        /// Checks if ShipmentName property is set.
        /// </summary>
        /// <returns>true if ShipmentName property is set.</returns>
        public bool IsSetShipmentName()
        {
            return this._shipmentName != null;
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
        public InboundShipmentInfo WithShipFromAddress(Address shipFromAddress)
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
        /// Gets and sets the DestinationFulfillmentCenterId property.
        /// </summary>
        [XmlElementAttribute(ElementName = "DestinationFulfillmentCenterId")]
        public string DestinationFulfillmentCenterId
        {
            get { return this._destinationFulfillmentCenterId; }
            set { this._destinationFulfillmentCenterId = value; }
        }

        /// <summary>
        /// Sets the DestinationFulfillmentCenterId property.
        /// </summary>
        /// <param name="destinationFulfillmentCenterId">DestinationFulfillmentCenterId property.</param>
        /// <returns>this instance.</returns>
        public InboundShipmentInfo WithDestinationFulfillmentCenterId(string destinationFulfillmentCenterId)
        {
            this._destinationFulfillmentCenterId = destinationFulfillmentCenterId;
            return this;
        }

        /// <summary>
        /// Checks if DestinationFulfillmentCenterId property is set.
        /// </summary>
        /// <returns>true if DestinationFulfillmentCenterId property is set.</returns>
        public bool IsSetDestinationFulfillmentCenterId()
        {
            return this._destinationFulfillmentCenterId != null;
        }

        /// <summary>
        /// Gets and sets the ShipmentStatus property.
        /// </summary>
        [XmlElementAttribute(ElementName = "ShipmentStatus")]
        public string ShipmentStatus
        {
            get { return this._shipmentStatus; }
            set { this._shipmentStatus = value; }
        }

        /// <summary>
        /// Sets the ShipmentStatus property.
        /// </summary>
        /// <param name="shipmentStatus">ShipmentStatus property.</param>
        /// <returns>this instance.</returns>
        public InboundShipmentInfo WithShipmentStatus(string shipmentStatus)
        {
            this._shipmentStatus = shipmentStatus;
            return this;
        }

        /// <summary>
        /// Checks if ShipmentStatus property is set.
        /// </summary>
        /// <returns>true if ShipmentStatus property is set.</returns>
        public bool IsSetShipmentStatus()
        {
            return this._shipmentStatus != null;
        }

        /// <summary>
        /// Gets and sets the LabelPrepType property.
        /// </summary>
        [XmlElementAttribute(ElementName = "LabelPrepType")]
        public string LabelPrepType
        {
            get { return this._labelPrepType; }
            set { this._labelPrepType = value; }
        }

        /// <summary>
        /// Sets the LabelPrepType property.
        /// </summary>
        /// <param name="labelPrepType">LabelPrepType property.</param>
        /// <returns>this instance.</returns>
        public InboundShipmentInfo WithLabelPrepType(string labelPrepType)
        {
            this._labelPrepType = labelPrepType;
            return this;
        }

        /// <summary>
        /// Checks if LabelPrepType property is set.
        /// </summary>
        /// <returns>true if LabelPrepType property is set.</returns>
        public bool IsSetLabelPrepType()
        {
            return this._labelPrepType != null;
        }

        /// <summary>
        /// Gets and sets the AreCasesRequired property.
        /// </summary>
        [XmlElementAttribute(ElementName = "AreCasesRequired")]
        public bool AreCasesRequired
        {
            get { return this._areCasesRequired.GetValueOrDefault(); }
            set { this._areCasesRequired = value; }
        }

        /// <summary>
        /// Sets the AreCasesRequired property.
        /// </summary>
        /// <param name="areCasesRequired">AreCasesRequired property.</param>
        /// <returns>this instance.</returns>
        public InboundShipmentInfo WithAreCasesRequired(bool areCasesRequired)
        {
            this._areCasesRequired = areCasesRequired;
            return this;
        }

        /// <summary>
        /// Checks if AreCasesRequired property is set.
        /// </summary>
        /// <returns>true if AreCasesRequired property is set.</returns>
        public bool IsSetAreCasesRequired()
        {
            return this._areCasesRequired != null;
        }


        public override void ReadFragmentFrom(IMwsReader reader)
        {
            _shipmentId = reader.Read<string>("ShipmentId");
            _shipmentName = reader.Read<string>("ShipmentName");
            _shipFromAddress = reader.Read<Address>("ShipFromAddress");
            _destinationFulfillmentCenterId = reader.Read<string>("DestinationFulfillmentCenterId");
            _shipmentStatus = reader.Read<string>("ShipmentStatus");
            _labelPrepType = reader.Read<string>("LabelPrepType");
            _areCasesRequired = reader.Read<bool?>("AreCasesRequired");
        }

        public override void WriteFragmentTo(IMwsWriter writer)
        {
            writer.Write("ShipmentId", _shipmentId);
            writer.Write("ShipmentName", _shipmentName);
            writer.Write("ShipFromAddress", _shipFromAddress);
            writer.Write("DestinationFulfillmentCenterId", _destinationFulfillmentCenterId);
            writer.Write("ShipmentStatus", _shipmentStatus);
            writer.Write("LabelPrepType", _labelPrepType);
            writer.Write("AreCasesRequired", _areCasesRequired);
        }

        public override void WriteTo(IMwsWriter writer)
        {
            writer.Write("http://mws.amazonaws.com/FulfillmentInboundShipment/2010-10-01/", "InboundShipmentInfo", this);
        }

        public InboundShipmentInfo() : base()
        {
        }
    }
}
