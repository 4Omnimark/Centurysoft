/******************************************************************************* 
 *  Copyright 2008-2012 Amazon.com, Inc. or its affiliates. All Rights Reserved.
 *  Licensed under the Apache License, Version 2.0 (the "License"); 
 *  
 *  You may not use this file except in compliance with the License. 
 *  You may obtain a copy of the License at: http://aws.amazon.com/apache2.0
 *  This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
 *  CONDITIONS OF ANY KIND, either express or implied. See the License for the 
 *  specific language governing permissions and limitations under the License.
 * ***************************************************************************** 
 * 
 *  Marketplace Web Service Products CSharp Library
 *  API Version: 2011-10-01
 * 
 */


using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text;


namespace MarketplaceWebServiceProducts.Model
{
    [XmlTypeAttribute(Namespace = "http://mws.amazonservices.com/schema/Products/2011-10-01")]
    [XmlRootAttribute(Namespace = "http://mws.amazonservices.com/schema/Products/2011-10-01", IsNullable = false)]
    public class Categories
    {
    
        private String productCategoryIdField;

        private String productCategoryNameField;

        private  Categories parentField;

        /// <summary>
        /// Gets and sets the ProductCategoryId property.
        /// </summary>
        [XmlElementAttribute(ElementName = "ProductCategoryId")]
        public String ProductCategoryId
        {
            get { return this.productCategoryIdField ; }
            set { this.productCategoryIdField= value; }
        }



        /// <summary>
        /// Sets the ProductCategoryId property
        /// </summary>
        /// <param name="productCategoryId">ProductCategoryId property</param>
        /// <returns>this instance</returns>
        public Categories WithProductCategoryId(String productCategoryId)
        {
            this.productCategoryIdField = productCategoryId;
            return this;
        }



        /// <summary>
        /// Checks if ProductCategoryId property is set
        /// </summary>
        /// <returns>true if ProductCategoryId property is set</returns>
        public Boolean IsSetProductCategoryId()
        {
            return  this.productCategoryIdField != null;

        }


        /// <summary>
        /// Gets and sets the ProductCategoryName property.
        /// </summary>
        [XmlElementAttribute(ElementName = "ProductCategoryName")]
        public String ProductCategoryName
        {
            get { return this.productCategoryNameField ; }
            set { this.productCategoryNameField= value; }
        }



        /// <summary>
        /// Sets the ProductCategoryName property
        /// </summary>
        /// <param name="productCategoryName">ProductCategoryName property</param>
        /// <returns>this instance</returns>
        public Categories WithProductCategoryName(String productCategoryName)
        {
            this.productCategoryNameField = productCategoryName;
            return this;
        }



        /// <summary>
        /// Checks if ProductCategoryName property is set
        /// </summary>
        /// <returns>true if ProductCategoryName property is set</returns>
        public Boolean IsSetProductCategoryName()
        {
            return  this.productCategoryNameField != null;

        }


        /// <summary>
        /// Gets and sets the Parent property.
        /// </summary>
        [XmlElementAttribute(ElementName = "Parent")]
        public Categories Parent
        {
            get { return this.parentField ; }
            set { this.parentField = value; }
        }



        /// <summary>
        /// Sets the Parent property
        /// </summary>
        /// <param name="parent">Parent property</param>
        /// <returns>this instance</returns>
        public Categories WithParent(Categories parent)
        {
            this.parentField = parent;
            return this;
        }



        /// <summary>
        /// Checks if Parent property is set
        /// </summary>
        /// <returns>true if Parent property is set</returns>
        public Boolean IsSetParent()
        {
            return this.parentField != null;
        }






        /// <summary>
        /// XML fragment representation of this object
        /// </summary>
        /// <returns>XML fragment for this object.</returns>
        /// <remarks>
        /// Name for outer tag expected to be set by calling method. 
        /// This fragment returns inner properties representation only
        /// </remarks>


        protected internal String ToXMLFragment() {
            StringBuilder xml = new StringBuilder();
            if (IsSetProductCategoryId()) {
                xml.Append("<ProductCategoryId>");
                xml.Append(EscapeXML(this.ProductCategoryId));
                xml.Append("</ProductCategoryId>");
            }
            if (IsSetProductCategoryName()) {
                xml.Append("<ProductCategoryName>");
                xml.Append(EscapeXML(this.ProductCategoryName));
                xml.Append("</ProductCategoryName>");
            }
            if (IsSetParent()) {
                Categories  parentObj = this.Parent;
                xml.Append("<Parent>");
                xml.Append(parentObj.ToXMLFragment());
                xml.Append("</Parent>");
            } 
            return xml.ToString();
        }

        /**
         * 
         * Escape XML special characters
         */
        private String EscapeXML(String str) {
            if (str == null)
                return "null";
            StringBuilder sb = new StringBuilder();
            foreach (Char c in str)
            {
                switch (c) {
                case '&':
                    sb.Append("&amp;");
                    break;
                case '<':
                    sb.Append("&lt;");
                    break;
                case '>':
                    sb.Append("&gt;");
                    break;
                case '\'':
                    sb.Append("&#039;");
                    break;
                case '"':
                    sb.Append("&quot;");
                    break;
                default:
                    sb.Append(c);
                    break;
                }
            }
            return sb.ToString();
        }



    }

}