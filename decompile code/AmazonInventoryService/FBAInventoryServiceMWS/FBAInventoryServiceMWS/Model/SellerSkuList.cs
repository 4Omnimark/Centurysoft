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
    public class SellerSkuList
    {
    
        private List<String> memberField;


        /// <summary>
        /// Gets and sets the member property.
        /// </summary>
        [XmlElementAttribute(ElementName = "member")]
        public List<String> member
        {
            get
            {
                if (this.memberField == null)
                {
                    this.memberField = new List<String>();
                }
                return this.memberField;
            }
            set { this.memberField =  value; }
        }



        /// <summary>
        /// Sets the member property
        /// </summary>
        /// <param name="list">member property</param>
        /// <returns>this instance</returns>
        public SellerSkuList Withmember(params String[] list)
        {
            foreach (String item in list)
            {
                member.Add(item);
            }
            return this;
        }          
 


        /// <summary>
        /// Checks of member property is set
        /// </summary>
        /// <returns>true if member property is set</returns>
        public Boolean IsSetmember()
        {
            return (member.Count > 0);
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
            List<String> memberObjList  =  this.member;
            foreach (String memberObj in memberObjList) { 
                xml.Append("<member>");
                xml.Append(EscapeXML(memberObj));
                xml.Append("</member>");
            }	
            return xml.ToString();
        }

        /**
         * 
         * Escape XML special characters
         */
        private String EscapeXML(String str) {
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