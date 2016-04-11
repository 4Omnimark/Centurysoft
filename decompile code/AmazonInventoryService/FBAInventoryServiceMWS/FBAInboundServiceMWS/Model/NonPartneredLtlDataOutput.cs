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
 * Non Partnered Ltl Data Output
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
    public class NonPartneredLtlDataOutput : AbstractMwsObject
    {

        private string _carrierName;
        private string _proNumber;

        /// <summary>
        /// Gets and sets the CarrierName property.
        /// </summary>
        [XmlElementAttribute(ElementName = "CarrierName")]
        public string CarrierName
        {
            get { return this._carrierName; }
            set { this._carrierName = value; }
        }

        /// <summary>
        /// Sets the CarrierName property.
        /// </summary>
        /// <param name="carrierName">CarrierName property.</param>
        /// <returns>this instance.</returns>
        public NonPartneredLtlDataOutput WithCarrierName(string carrierName)
        {
            this._carrierName = carrierName;
            return this;
        }

        /// <summary>
        /// Checks if CarrierName property is set.
        /// </summary>
        /// <returns>true if CarrierName property is set.</returns>
        public bool IsSetCarrierName()
        {
            return this._carrierName != null;
        }

        /// <summary>
        /// Gets and sets the ProNumber property.
        /// </summary>
        [XmlElementAttribute(ElementName = "ProNumber")]
        public string ProNumber
        {
            get { return this._proNumber; }
            set { this._proNumber = value; }
        }

        /// <summary>
        /// Sets the ProNumber property.
        /// </summary>
        /// <param name="proNumber">ProNumber property.</param>
        /// <returns>this instance.</returns>
        public NonPartneredLtlDataOutput WithProNumber(string proNumber)
        {
            this._proNumber = proNumber;
            return this;
        }

        /// <summary>
        /// Checks if ProNumber property is set.
        /// </summary>
        /// <returns>true if ProNumber property is set.</returns>
        public bool IsSetProNumber()
        {
            return this._proNumber != null;
        }


        public override void ReadFragmentFrom(IMwsReader reader)
        {
            _carrierName = reader.Read<string>("CarrierName");
            _proNumber = reader.Read<string>("ProNumber");
        }

        public override void WriteFragmentTo(IMwsWriter writer)
        {
            writer.Write("CarrierName", _carrierName);
            writer.Write("ProNumber", _proNumber);
        }

        public override void WriteTo(IMwsWriter writer)
        {
            writer.Write("http://mws.amazonaws.com/FulfillmentInboundShipment/2010-10-01/", "NonPartneredLtlDataOutput", this);
        }

        public NonPartneredLtlDataOutput() : base()
        {
        }
    }
}
