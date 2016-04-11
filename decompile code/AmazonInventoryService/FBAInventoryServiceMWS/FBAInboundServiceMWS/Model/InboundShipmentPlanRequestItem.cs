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
 * Inbound Shipment Plan Request Item
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
    public class InboundShipmentPlanRequestItem : AbstractMwsObject
    {

        private string _sellerSKU;
        private string _asin;
        private string _condition;
        private decimal? _quantity;
        private decimal? _quantityInCase;

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
        public InboundShipmentPlanRequestItem WithSellerSKU(string sellerSKU)
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
        /// Gets and sets the ASIN property.
        /// </summary>
        [XmlElementAttribute(ElementName = "ASIN")]
        public string ASIN
        {
            get { return this._asin; }
            set { this._asin = value; }
        }

        /// <summary>
        /// Sets the ASIN property.
        /// </summary>
        /// <param name="asin">ASIN property.</param>
        /// <returns>this instance.</returns>
        public InboundShipmentPlanRequestItem WithASIN(string asin)
        {
            this._asin = asin;
            return this;
        }

        /// <summary>
        /// Checks if ASIN property is set.
        /// </summary>
        /// <returns>true if ASIN property is set.</returns>
        public bool IsSetASIN()
        {
            return this._asin != null;
        }

        /// <summary>
        /// Gets and sets the Condition property.
        /// </summary>
        [XmlElementAttribute(ElementName = "Condition")]
        public string Condition
        {
            get { return this._condition; }
            set { this._condition = value; }
        }

        /// <summary>
        /// Sets the Condition property.
        /// </summary>
        /// <param name="condition">Condition property.</param>
        /// <returns>this instance.</returns>
        public InboundShipmentPlanRequestItem WithCondition(string condition)
        {
            this._condition = condition;
            return this;
        }

        /// <summary>
        /// Checks if Condition property is set.
        /// </summary>
        /// <returns>true if Condition property is set.</returns>
        public bool IsSetCondition()
        {
            return this._condition != null;
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
        public InboundShipmentPlanRequestItem WithQuantity(decimal quantity)
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
        public InboundShipmentPlanRequestItem WithQuantityInCase(decimal quantityInCase)
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
            _sellerSKU = reader.Read<string>("SellerSKU");
            _asin = reader.Read<string>("ASIN");
            _condition = reader.Read<string>("Condition");
            _quantity = reader.Read<decimal?>("Quantity");
            _quantityInCase = reader.Read<decimal?>("QuantityInCase");
        }

        public override void WriteFragmentTo(IMwsWriter writer)
        {
            writer.Write("SellerSKU", _sellerSKU);
            writer.Write("ASIN", _asin);
            writer.Write("Condition", _condition);
            writer.Write("Quantity", _quantity);
            writer.Write("QuantityInCase", _quantityInCase);
        }

        public override void WriteTo(IMwsWriter writer)
        {
            writer.Write("http://mws.amazonaws.com/FulfillmentInboundShipment/2010-10-01/", "InboundShipmentPlanRequestItem", this);
        }

        public InboundShipmentPlanRequestItem() : base()
        {
        }
    }
}
