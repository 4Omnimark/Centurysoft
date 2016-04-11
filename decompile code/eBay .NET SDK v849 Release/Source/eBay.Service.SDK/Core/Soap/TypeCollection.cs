#region Copyright
//	Copyright (c) 2013 eBay, Inc.
//	
//	This program is licensed under the terms of the eBay Common Development and
//	Distribution License (CDDL) Version 1.0 (the "License") and any subsequent  
//	version thereof released by eBay.  The then-current version of the License can be 
//	found at http://www.opensource.org/licenses/cddl1.php and in the eBaySDKLicense 
//	file that is under the eBay SDK ../docs directory
#endregion

using System.Diagnostics;
using System.Xml.Serialization;
using System;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Web.Services;
using System.Collections;
using System.Xml;

namespace eBay.Service.Core.Soap
{
	/// <summary>
	/// 
	/// </summary>
	[Serializable()]
	public sealed class TypeCollection : System.Collections.CollectionBase 
	{
        
		/// <summary>
		/// 
		/// </summary>
		public TypeCollection() 
		{
		}
        
		/// <summary>
		/// 
		/// </summary>
		/// <param name="items"></param>
		public TypeCollection(Type[] items) 
		{
			this.AddRange(items);
		}
        
		/// <summary>
		/// 
		/// </summary>
		/// <param name="items"></param>
		public TypeCollection(TypeCollection items) 
		{
			this.AddRange(items);
		}
        
		/// <summary>
		/// 
		/// </summary>
		public Type this[int index] 
		{
			get 
			{
				return ((Type)(this.InnerList[index]));
			}
			set 
			{
				this.InnerList[index] = value;
			}
		}
        
		/// <summary>
		/// 
		/// </summary>
		public bool IsFixedSize 
		{
			get 
			{
				return this.InnerList.IsFixedSize;
			}
		}
        
		/// <summary>
		/// 
		/// </summary>
		public bool IsReadOnly 
		{
			get 
			{
				return this.InnerList.IsReadOnly;
			}
		}
        
		/// <summary>
		/// 
		/// </summary>
		public bool IsSynchronized 
		{
			get 
			{
				return this.InnerList.IsSynchronized;
			}
		}
        
		/// <summary>
		/// 
		/// </summary>
		public object SyncRoot 
		{
			get 
			{
				return this.InnerList.SyncRoot;
			}
		}
        
		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int Add(Type item) 
		{
			return this.InnerList.Add(item);
		}
        
		/// <summary>
		/// 
		/// </summary>
		/// <param name="items"></param>
		public void AddRange(Type[] items) 
		{
			this.InnerList.AddRange(items);
		}
        
		/// <summary>
		/// 
		/// </summary>
		/// <param name="items"></param>
		public void AddRange(TypeCollection items) 
		{
			this.InnerList.AddRange(items);
		}
        
		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool Contains(Type item) 
		{
			return this.InnerList.Contains(item);
		}
        
		/// <summary>
		/// 
		/// </summary>
		/// <param name="items"></param>
		/// <param name="index"></param>
		public void CopyTo(Type[] items, int index) 
		{
			this.InnerList.CopyTo(items, index);
		}
        
		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public int IndexOf(Type item) 
		{
			return this.InnerList.IndexOf(item);
		}
        
		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <param name="item"></param>
		public void Insert(int index, Type item) 
		{
			this.InnerList.Insert(index, item);
		}
        
		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public Type ItemAt(int index) 
		{
			return ((Type)(this.InnerList[index]));
		}
        
		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		public void Remove(Type item) 
		{
			this.InnerList.Remove(item);
		}
        
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Type[] ToArray() 
		{
			return ((Type[])(this.InnerList.ToArray(typeof(Type))));
		}
	}
}
