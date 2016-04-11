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
 * Non Partnered Small Parcel Package Input
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
    public class NonPartneredSmallParcelPackageInput : AbstractMwsObject
    {

        private string _trackingId;

        /// <summary>
        /// Gets and sets the TrackingId property.
        /// </summary>
        [XmlElementAttribute(ElementName = "TrackingId")]
        public string TrackingId
        {
            get { return this._trackingId; }
            set { this._trackingId = value; }
        }

        /// <summary>
        /// Sets the TrackingId property.
        /// </summary>
        /// <param name="trackingId">TrackingId property.</param>
        /// <returns>this instance.</returns>
        public NonPartneredSmallParcelPackageInput WithTrackingId(string trackingId)
        {
            this._trackingId = trackingId;
            return this;
        }

        /// <summary>
        /// Checks if TrackingId property is set.
        /// </summary>
        /// <returns>true if TrackingId property is set.</returns>
        public bool IsSetTrackingId()
        {
            return this._trackingId != null;
        }


        public override void ReadFragmentFrom(IMwsReader reader)
        {
            _trackingId = reader.Read<string>("TrackingId");
        }

        public override void WriteFragmentTo(IMwsWriter writer)
        {
            writer.Write("TrackingId", _trackingId);
        }

        public override void WriteTo(IMwsWriter writer)
        {
            writer.Write("http://mws.amazonaws.com/FulfillmentInboundShipment/2010-10-01/", "NonPartneredSmallParcelPackageInput", this);
        }

        public NonPartneredSmallParcelPackageInput() : base()
        {
        }
    }
}
