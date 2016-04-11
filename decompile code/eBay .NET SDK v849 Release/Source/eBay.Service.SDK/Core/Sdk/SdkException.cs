#region Copyright
//	Copyright (c) 2013 eBay, Inc.
//	
//	This program is licensed under the terms of the eBay Common Development and
//	Distribution License (CDDL) Version 1.0 (the "License") and any subsequent  
//	version thereof released by eBay.  The then-current version of the License can be 
//	found at http://www.opensource.org/licenses/cddl1.php and in the eBaySDKLicense 
//	file that is under the eBay SDK ../docs directory
#endregion

#region Namespaces
using System;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
#endregion

namespace eBay.Service.Core.Sdk
{

	/// <summary>
    /// SdkException is used for general exceptions related to SDK kernel itself. 
    /// For example, if a token is not set the exception will be wrapped as SdkException.
	/// </summary>
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable()]
	public class SdkException: ApplicationException, ISerializable
	{

		#region Constructors
		/// <summary>
		/// 
		/// </summary>
		public SdkException(): base()
		{
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="message">A message that describes the error.</param>
		public SdkException(string message): base(message)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="message">A message that describes the error.</param>
		/// <param name="inner">The exception that is the cause of the current exception.</param>
		public SdkException(string message, Exception inner): base(message, inner)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="info">The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		protected SdkException(SerializationInfo info, StreamingContext context):base(info, context)
		{
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
		[SecurityPermissionAttribute(SecurityAction.Demand,SerializationFormatter=true)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
		}
		#endregion

	}
}
