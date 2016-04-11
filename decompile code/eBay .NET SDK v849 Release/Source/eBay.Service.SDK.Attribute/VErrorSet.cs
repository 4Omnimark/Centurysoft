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
using System.Collections;
using System.Xml;
using System.Runtime.InteropServices;

namespace eBay.Service.SDK.Attribute
{
	//--------------------------------------------------------------------

	/// <summary>
	/// IError collection.
	/// </summary>
	public interface IErrorSet : IList, ICollection, IEnumerable
	{
		/// <summary>
		/// CSId of the AttributeSet that is associated with the error set.
		/// </summary>
		int CSId
		{
			get; set;
		}

		// ==== Strong typed methods ====

		/// <summary>
		/// Gets an element at the specified index in the collection.
		/// </summary>
		new IError this[int index]
		{
			get; set;
		}

		/// <summary>
		/// Add element.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		int Add(IError value);

		/// <summary>
		/// Add an array of IError.
		/// </summary>
		/// <param name="items"></param>
		void AddRange(IError[] items);

		/// <summary>
		/// Adds the contents of the specified IErrorSet 
		/// to the end of the collection.
		/// </summary>
		/// <param name="items"></param>
		void AddRange(IErrorSet items);

		/// <summary>
		/// Indicates whether a specified object is contained in the list.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		bool Contains(IError value);

		/// <summary>
		/// Copies the collection objects to a one-dimensional Array 
		/// instance beginning at the specified index.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		void CopyTo(
			IError[] array,
			int index
			);

		/// <summary>
		/// Get index of an element.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		int IndexOf(IError value);

		/// <summary>
		/// Insert element to list.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		void Insert(int index, IError value);

		/// <summary>
		/// Returns number of items in the collection.
		/// </summary>
		/// <returns>Number of items.</returns>
		int ItemCount();

		/// <summary>
		/// Returns item by index. 
		/// </summary>
		IError ItemAt(int index);

		/// <summary>
		/// Remove element from list.
		/// </summary>
		/// <param name="value"></param>
		void Remove(IError value);
	}

	//--------------------------------------------------------------------

	/// <summary>
	/// Strong typed IError collection
	/// </summary>
	[ClassInterface(ClassInterfaceType.None)]
	[Serializable]
	internal sealed class ErrorSet : CollectionBase, IErrorSet
	{
		private int mCSId;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public ErrorSet()
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="csid">The csid that is associated with the error set.</param>
		public ErrorSet(int csid)
		{
			this.mCSId = csid;
		}

		/// <summary>
		/// Initializes a new instance containing the specified array 
		/// of IError objects
		/// </summary>
		/// <param name="value"></param>
		public ErrorSet(
			IError[] value
			)
		{
			AddRange(value);
		}

		/// <summary>
		/// Initializes a new instance containing the elements of 
		/// the specified source collection.
		/// </summary>
		/// <param name="value"></param>
		public ErrorSet(
			IErrorSet value
			)
		{
			AddRange(value);
		}

		/// <summary>
		/// CSId of the AttributeSet that is associated with the error set.
		/// </summary>
		public int CSId
		{
			get { return mCSId; }
			set { mCSId = value; }
		}

		// ======== IErrorSet ========

		/// <summary>
		/// Gets an element at the specified index in the collection.
		/// </summary>
		public IError this[int index]
		{
			get { return (IError)InnerList[index]; }
			set { InnerList[index] = value; }
		}

		/// <summary>
		/// Add element.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public int Add(IError value)
		{
			return InnerList.Add(value);
		}

		/// <summary>
		/// Add an array of IError.
		/// </summary>
		/// <param name="items"></param>
		public void AddRange(IError[] items)
		{
			InnerList.AddRange(items);
		}

		/// <summary>
		/// Adds the contents of the specified IErrorSet 
		/// to the end of the collection.
		/// </summary>
		/// <param name="items"></param>
		public void AddRange(IErrorSet items)
		{
			InnerList.AddRange(items);
		}

		/// <summary>
		/// Indicates whether a specified object is contained in the list.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool Contains(IError value)
		{
			return InnerList.Contains(value);
		}

		/// <summary>
		/// Copies the collection objects to a one-dimensional Array 
		/// instance beginning at the specified index.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		public void CopyTo(
			IError[] array,
			int index
			)
		{
			InnerList.CopyTo(array, index);
		}

		/// <summary>
		/// Get index of an element.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public int IndexOf(IError value)
		{
			return InnerList.IndexOf(value);
		}

		/// <summary>
		/// Insert element to list.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public void Insert(int index, IError value)
		{
			InnerList.Insert(index, value);
		}

		/// <summary>
		/// Returns number of items in the collection.
		/// </summary>
		/// <returns>Number of items.</returns>
		public int ItemCount()
		{
			return InnerList.Count;
		}

		/// <summary>
		/// Returns item by index. 
		/// </summary>
		public IError ItemAt(int index)
		{
			return (IError)InnerList[index];
		}

		/// <summary>
		/// Remove element from list.
		/// </summary>
		/// <param name="value"></param>
		public void Remove(IError value)
		{
			InnerList.Remove(value);
		}

		//--------------------------------------------------------------------

		internal XmlNode toXml()
		{
			return toXml(new XmlDocument());
		}

		internal XmlNode toXml(XmlDocument doc)
		{
			XmlNode root = doc.CreateElement(ERROR_SET);
			XmlAttribute attr = doc.CreateAttribute("", "id", "");
			attr.Value = this.CSId.ToString();
			root.Attributes.Append(attr);
			
			foreach(Error err in this.InnerList )
			{
				XmlNode node = err.toXml(doc);
				root.AppendChild(node);
			}

			return root;
		}

		private const string ERROR_SET = "ErrorSet";
	}

	//--------------------------------------------------------------------

	/// <summary>
	/// IErrorSet collection.
	/// </summary>
	public interface IErrorSetCollection : IList, ICollection, IEnumerable
	{
		// ==== Strong typed methods ====

		/// <summary>
		/// Gets an element at the specified index in the collection.
		/// </summary>
		new IErrorSet this[int index]
		{
			get; set;
		}

		/// <summary>
		/// Add element.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		int Add(IErrorSet value);

		/// <summary>
		/// Add an array of IErrorSet.
		/// </summary>
		/// <param name="items"></param>
		void AddRange(IErrorSet[] items);

		/// <summary>
		/// Adds the contents of the specified IErrorSetCollection 
		/// to the end of the collection.
		/// </summary>
		/// <param name="items"></param>
		void AddRange(IErrorSetCollection items);

		/// <summary>
		/// Indicates whether a specified object is contained in the list.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		bool Contains(IErrorSet value);

		/// <summary>
		/// Copies the collection objects to a one-dimensional Array 
		/// instance beginning at the specified index.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		void CopyTo(
			IErrorSet[] array,
			int index
			);

		/// <summary>
		/// Get index of an element.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		int IndexOf(IErrorSet value);

		/// <summary>
		/// Insert element to list.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		void Insert(int index, IErrorSet value);

		/// <summary>
		/// Returns number of items in the collection.
		/// </summary>
		/// <returns>Number of items.</returns>
		int ItemCount();

		/// <summary>
		/// Returns item by index. 
		/// </summary>
		IErrorSet ItemAt(int index);

		/// <summary>
		/// Remove element from list.
		/// </summary>
		/// <param name="value"></param>
		void Remove(IErrorSet value);
	}

	//--------------------------------------------------------------------

	/// <summary>
	/// Strong typed IErrorSet collection
	/// </summary>
	[ClassInterface(ClassInterfaceType.None)]
	[Serializable]
	public sealed class ErrorSetCollection : CollectionBase, IErrorSetCollection
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public ErrorSetCollection()
		{
		}

		/// <summary>
		/// Initializes a new instance containing the specified array 
		/// of IErrorSet objects
		/// </summary>
		/// <param name="value"></param>
		public ErrorSetCollection(
			IErrorSet[] value
			)
		{
			AddRange(value);
		}

		/// <summary>
		/// Initializes a new instance containing the elements of 
		/// the specified source collection.
		/// </summary>
		/// <param name="value"></param>
		public ErrorSetCollection(
			IErrorSetCollection value
			)
		{
			AddRange(value);
		}

		// ======== IErrorSetCollection ========

		/// <summary>
		/// Gets an element at the specified index in the collection.
		/// </summary>
		public IErrorSet this[int index]
		{
			get { return (IErrorSet)InnerList[index]; }
			set { InnerList[index] = value; }
		}

		/// <summary>
		/// Add element.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public int Add(IErrorSet value)
		{
			return InnerList.Add(value);
		}

		/// <summary>
		/// Add an array of IErrorSet.
		/// </summary>
		/// <param name="items"></param>
		public void AddRange(IErrorSet[] items)
		{
			InnerList.AddRange(items);
		}

		/// <summary>
		/// Adds the contents of the specified IErrorSetCollection 
		/// to the end of the collection.
		/// </summary>
		/// <param name="items"></param>
		public void AddRange(IErrorSetCollection items)
		{
			InnerList.AddRange(items);
		}

		/// <summary>
		/// Indicates whether a specified object is contained in the list.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool Contains(IErrorSet value)
		{
			return InnerList.Contains(value);
		}

		/// <summary>
		/// Copies the collection objects to a one-dimensional Array 
		/// instance beginning at the specified index.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		public void CopyTo(
			IErrorSet[] array,
			int index
			)
		{
			InnerList.CopyTo(array, index);
		}

		/// <summary>
		/// Get index of an element.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public int IndexOf(IErrorSet value)
		{
			return InnerList.IndexOf(value);
		}

		/// <summary>
		/// Insert element to list.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public void Insert(int index, IErrorSet value)
		{
			InnerList.Insert(index, value);
		}

		/// <summary>
		/// Returns number of items in the collection.
		/// </summary>
		/// <returns>Number of items.</returns>
		public int ItemCount()
		{
			return InnerList.Count;
		}

		/// <summary>
		/// Returns item by index. 
		/// </summary>
		public IErrorSet ItemAt(int index)
		{
			return (IErrorSet)InnerList[index];
		}

		/// <summary>
		/// Remove element from list.
		/// </summary>
		/// <param name="value"></param>
		public void Remove(IErrorSet value)
		{
			InnerList.Remove(value);
		}
	}
}
