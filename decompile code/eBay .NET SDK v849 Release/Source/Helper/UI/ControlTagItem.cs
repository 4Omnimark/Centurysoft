#region Copyright
//	Copyright (c) 2013 eBay, Inc.
//	
//	This program is licensed under the terms of the eBay Common Development and
//	Distribution License (CDDL) Version 1.0 (the "License") and any subsequent  
//	version thereof released by eBay.  The then-current version of the License can be 
//	found at http://www.opensource.org/licenses/cddl1.php and in the eBaySDKLicense 
//	file that is under the eBay SDK ../docs directory
#endregion

using System;
using System.Runtime.InteropServices;

namespace Samples.Helper.UI
{
	/// <summary>
	/// Defines object to associate object with string.
	/// </summary>
	public interface IControlTagItem
	{
		/// <summary>
		/// The string.
		/// </summary>
		string Text
		{
			get; set;
		}

		/// <summary>
		/// The object that is associated with the string.
		/// </summary>
		object Tag
		{
			get; set;
		}
	}

	/// <summary>
	/// A helper class for associate string with an object.
	/// </summary>
	[ClassInterface(ClassInterfaceType.None)]
	[Serializable]
	public class ControlTagItem : IControlTagItem
	{
		private string mText;
		private object mTag;

		/// <summary>
		/// Constructor.
		/// </summary>
		public ControlTagItem()
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="text"></param>
		/// <param name="tag"></param>
		public ControlTagItem(string text, object tag)
		{
			this.Text = text;
			this.Tag = tag;
		}

		/// <summary>
		/// The string.
		/// </summary>
		public string Text
		{
			get { return mText; }
			set { mText = value; }
		}

		/// <summary>
		/// The object that is associated with the string.
		/// </summary>
		public object Tag
		{
			get { return mTag; }
			set { mTag = value; }
		}

		/// <summary>
		/// Overrided ToString().
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return this.Text;
		}
	}
}
