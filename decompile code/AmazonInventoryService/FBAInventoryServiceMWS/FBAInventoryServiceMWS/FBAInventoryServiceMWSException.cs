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
using System.Net;
using FBAInventoryServiceMWS;
using FBAInventoryServiceMWS.Model;

namespace FBAInventoryServiceMWS
{
    /// <summary>
    /// FBA Inventory Service MWS  Exception provides details of errors 
    /// returned by FBA Inventory Service MWS  service
    /// </summary>
    public class FBAInventoryServiceMWSException : Exception 
    {
    
        private String message = null;
        private HttpStatusCode statusCode = default(HttpStatusCode);
        private String errorCode = null;
        private String errorType = null;
        private String requestId = null;
        private String xml = null;
    

        /// <summary>
        /// Constructs FBAInventoryServiceMWSException with message
        /// </summary>
        /// <param name="message">Overview of error</param>
        public FBAInventoryServiceMWSException(String message) {
            this.message = message;
        }
    
        /// <summary>
        /// Constructs FBAInventoryServiceMWSException with message and status code
        /// </summary>
        /// <param name="message">Overview of error</param>
        /// <param name="statusCode">HTTP status code for error response</param>
        public FBAInventoryServiceMWSException(String message, HttpStatusCode statusCode) : this (message)
        {
            this.statusCode = statusCode;
        }
    

        /// <summary>
        /// Constructs FBAInventoryServiceMWSException with wrapped exception
        /// </summary>
        /// <param name="t">Wrapped exception</param>
        public FBAInventoryServiceMWSException(Exception t) : this (t.Message, t) {

        }
    
        /// <summary>
        /// Constructs FBAInventoryServiceMWSException with message and wrapped exception
        /// </summary>
        /// <param name="message">Overview of error</param>
        /// <param name="t">Wrapped exception</param>
        public FBAInventoryServiceMWSException(String message, Exception t) : base (message, t) {
            this.message = message;
            if (t is FBAInventoryServiceMWSException)
            {
                FBAInventoryServiceMWSException ex = (FBAInventoryServiceMWSException)t;
                this.statusCode = ex.StatusCode;
                this.errorCode = ex.ErrorCode;
                this.errorType = ex.ErrorType;
                this.requestId = ex.RequestId;
                this.xml = ex.XML;
            }
        }
    

        /// <summary>
        /// Constructs FBAInventoryServiceMWSException with information available from service
        /// </summary>
        /// <param name="message">Overview of error</param>
        /// <param name="statusCode">HTTP status code for error response</param>
        /// <param name="errorCode">Error Code returned by the service</param>
        /// <param name="errorType">Error type. Possible types:  Sender, Receiver or Unknown</param>
        /// <param name="requestId">Request ID returned by the service</param>
        /// <param name="xml">Compete xml found in response</param>
        public FBAInventoryServiceMWSException(String message, HttpStatusCode statusCode, String errorCode, String errorType, String requestId, String xml) : this (message, statusCode)
        {
            this.errorCode = errorCode;
            this.errorType = errorType;
            this.requestId = requestId;
            this.xml = xml;
        }
    
        /// <summary>
        /// Gets and sets of the ErrorCode property.
        /// </summary>
        public String ErrorCode
        {
            get { return this.errorCode; }
        }

        /// <summary>
        /// Gets and sets of the ErrorType property.
        /// </summary>
        public String ErrorType
        {
            get { return this.errorType; }
        }

        /// <summary>
        /// Gets error message
        /// </summary>
        public override String Message
        {
            get { return this.message; }
        }
    
 
        /// <summary>
        /// Gets status code returned by the service if available. If status
        /// code is set to -1, it means that status code was unavailable at the
        /// time exception was thrown
        /// </summary>
        public HttpStatusCode StatusCode
        {
            get { return this.statusCode; }
        }

        /// <summary>
        /// Gets XML returned by the service if available.
        /// </summary>
        public String XML
        {
            get { return this.xml; }
        }
 
        /// <summary>
        /// Gets Request ID returned by the service if available.
        /// </summary>
        public String RequestId
        {
            get { return this.requestId; }
        }
    
    }
}
