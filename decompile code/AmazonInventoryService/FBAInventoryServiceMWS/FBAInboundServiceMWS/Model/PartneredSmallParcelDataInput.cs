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
 * Partnered Small Parcel Data Input
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
    public class PartneredSmallParcelDataInput : AbstractMwsObject
    {

        private PartneredSmallParcelPackageInputList _packageList;

        /// <summary>
        /// Gets and sets the PackageList property.
        /// </summary>
        [XmlElementAttribute(ElementName = "PackageList")]
        public PartneredSmallParcelPackageInputList PackageList
        {
            get { return this._packageList; }
            set { this._packageList = value; }
        }

        /// <summary>
        /// Sets the PackageList property.
        /// </summary>
        /// <param name="packageList">PackageList property.</param>
        /// <returns>this instance.</returns>
        public PartneredSmallParcelDataInput WithPackageList(PartneredSmallParcelPackageInputList packageList)
        {
            this._packageList = packageList;
            return this;
        }

        /// <summary>
        /// Checks if PackageList property is set.
        /// </summary>
        /// <returns>true if PackageList property is set.</returns>
        public bool IsSetPackageList()
        {
            return this._packageList != null;
        }


        public override void ReadFragmentFrom(IMwsReader reader)
        {
            _packageList = reader.Read<PartneredSmallParcelPackageInputList>("PackageList");
        }

        public override void WriteFragmentTo(IMwsWriter writer)
        {
            writer.Write("PackageList", _packageList);
        }

        public override void WriteTo(IMwsWriter writer)
        {
            writer.Write("http://mws.amazonaws.com/FulfillmentInboundShipment/2010-10-01/", "PartneredSmallParcelDataInput", this);
        }

        public PartneredSmallParcelDataInput() : base()
        {
        }
    }
}
