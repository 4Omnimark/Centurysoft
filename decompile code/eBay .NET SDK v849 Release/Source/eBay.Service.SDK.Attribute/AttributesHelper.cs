using System;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using System.Resources;
using System.Runtime.InteropServices;
using MSXML2;

using eBay.Service.Core.Soap;
using eBay.Service.Core.Sdk;
using eBay.Service.Call;
using eBay.Service.Util;
using eBay.Service.SDK.Util;

namespace eBay.Service.SDK.Attribute
{
	/// <summary>
	/// The helper class to host attributes related functions.
	/// </summary>
	[ComVisible(false)]
	public class AttributesHelper
	{
		/// <summary>
		/// Convert <c>NameValueCollection</c> to <c>IKeyValueCollection</c>.
		/// </summary>
		/// <param name="nvc">the <c>NameValueCollection</c> object.</param>
		/// <returns>The generated <c>IKeyValueCollection</c> object.</returns>
		public static IKeyValueCollection ConvertFromNameValues(NameValueCollection nvc)
		{
			IKeyValueCollection kvc = new KeyValueCollection();

			foreach( string key in nvc.AllKeys )
			{
				IKeyValue kv = new KeyValue(key, nvc[key]);
				kvc.Add(kv);
			}

			return kvc;
		}

		/// <summary>
		/// Convert <c>IKeyValueCollection</c> to <c>NameValueCollection</c>.
		/// </summary>
		/// <param name="kvc">The <c>IKeyValueCollection</c> object.</param>
		/// <returns>The generated <c>NameValueCollection</c> object.</returns>
		public static NameValueCollection ConvertToNameValues(IKeyValueCollection kvc)
		{
			NameValueCollection nvc = new NameValueCollection();

			foreach( IKeyValue kv in kvc )
			{
				nvc.Add(kv.Key, kv.Value);
			}

			return nvc;
		}
	}

}