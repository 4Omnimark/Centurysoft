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
using System.Runtime.InteropServices;

using eBay.Service.Core.Soap;
using eBay.Service.Core.Sdk;

namespace eBay.Service.SDK.Attribute
{
	/// <summary>
	/// Wrapper class for AttributeSetType.
	/// </summary>
	public class AttributeSet : AttributeSetType
	{
		private int mCategoryID = 0;
		private int mCategoryOrdinal = 0;
		private string mProductID;
		private string mProductFinderID;
		private string mName;

		/// <summary>
		/// Constructor.
		/// </summary>
		public AttributeSet()
		{
		}

		/// <summary>
		/// Gets/Sets the category ID of the AttributeSet (CS).
		/// </summary>
		public int CategoryID
		{
			get
			{
				return this.mCategoryID;
			}
			set
			{
				this.mCategoryID = value;
			}
		}

		/// <summary>
		/// Ordinal number of the category.
		/// </summary>
		public int CategoryOrdinal
		{
			get
			{
				return this.mCategoryOrdinal;
			}
			set
			{
				this.mCategoryOrdinal = value;
			}
		}

		/// <summary>
		/// Gets/Sets the product ID that is used by IAttributesMaster to idenfity the 
		/// catalog product that is associated with the attributes data.
		/// </summary>
		public string ProductID
		{
			get
			{
				return this.mProductID;
			}
			set
			{
				this.mProductID = value;
			}
		}

		/// <summary>
		/// Gets/Sets the name that is associated with the AttributeSet object.
		/// </summary>
		public string Name
		{
			get
			{
				return this.mName;
			}
			set
			{
				this.mName = value;
			}
		}

		/// <summary>
		/// Gets/Sets the product finder that is associated with the AttributeSet object.
		/// </summary>
		public string ProductFinderID
		{
			get
			{
				return this.mProductFinderID;
			}
			set
			{
				this.mProductFinderID = value;
			}
		}

		/// <summary>
		/// Clone the object.
		/// </summary>
		public AttributeSet Clone()
		{
			return (AttributeSet)this.MemberwiseClone();
		}
	}

	//--------------------------------------------------------------------

	/// <summary>
	/// AttributeSet collection.
	/// </summary>
	public interface IAttributeSetCollection : IList, ICollection, IEnumerable
	{
		/// <summary>
		/// Clone the object.
		/// </summary>
		IAttributeSetCollection Clone();

		// ==== Strong typed methods ====

		/// <summary>
		/// Gets an element at the specified index in the collection.
		/// </summary>
		new AttributeSet this[int index]
		{
			get; set;
		}

		/// <summary>
		/// Add element.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		int Add(AttributeSet value);

		/// <summary>
		/// Add an array of AttributeSet.
		/// </summary>
		/// <param name="items"></param>
		void AddRange(AttributeSet[] items);

		/// <summary>
		/// Adds the contents of the specified IAttributeSetCollection 
		/// to the end of the collection.
		/// </summary>
		/// <param name="items"></param>
		void AddRange(IAttributeSetCollection items);

		/// <summary>
		/// Indicates whether a specified object is contained in the list.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		bool Contains(AttributeSet value);

		/// <summary>
		/// Copies the collection objects to a one-dimensional Array 
		/// instance beginning at the specified index.
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		void CopyTo(
			AttributeSet[] array,
			int index
			);

		/// <summary>
		/// Get index of an element.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		int IndexOf(AttributeSet value);

		/// <summary>
		/// Insert element to list.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		void Insert(int index, AttributeSet value);

		/// <summary>
		/// Returns number of items in the collection.
		/// </summary>
		/// <returns>Number of items.</returns>
		int ItemCount();

		/// <summary>
		/// Returns item by index. 
		/// </summary>
		AttributeSet ItemAt(int index);

		/// <summary>
		/// Remove element from list.
		/// </summary>
		/// <param name="value"></param>
		void Remove(AttributeSet value);
	}

	//--------------------------------------------------------------------

	/// <summary>
	/// Strong typed AttributeSet collection
	/// </summary>
	[ClassInterface(ClassInterfaceType.None)]
	[Serializable]
	public sealed class AttributeSetCollection : CollectionBase, IAttributeSetCollection
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public AttributeSetCollection()
		{
		}

		/// <summary>
		/// Clone the object.
		/// </summary>
		public IAttributeSetCollection Clone()
		{
			AttributeSetCollection asc = (AttributeSetCollection)MemberwiseClone();
			asc.Clear();
			
			foreach( AttributeSet ast in this.InnerList )
				asc.Add(ast.Clone());

			return asc;
		}

		/// <summary>
		/// Initializes a new instance containing the specified array 
		/// of AttributeSet objects
		/// </summary>
		/// <param name="value"></param>
		public AttributeSetCollection(
			AttributeSet[] value
			)
		{
			AddRange(value);
		}

		/// <summary>
		/// Initializes a new instance containing the elements of 
		/// the specified source collection.
		/// </summary>
		/// <param name="value"></param>
		public AttributeSetCollection(
			IAttributeSetCollection value
			)
		{
			AddRange(value);
		}

		// ======== IAttributeSetCollection ========

		/// <summary>
		/// Gets an element at the specified index in the collection.
		/// </summary>
		public AttributeSet this[int index]
		{
			get { return (AttributeSet)InnerList[index]; }
			set { InnerList[index] = value; }
		}

		/// <summary>
		/// Add element.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public int Add(AttributeSet value)
		{
			return InnerList.Add(value);
		}

		/// <summary>
		/// Add an array of AttributeSet.
		/// </summary>
		/// <param name="items"></param>
		public void AddRange(AttributeSet[] items)
		{
			InnerList.AddRange(items);
		}

		/// <summary>
		/// Adds the contents of the specified IAttributeSetCollection 
		/// to the end of the collection.
		/// </summary>
		/// <param name="items"></param>
		public void AddRange(IAttributeSetCollection items)
		{
			InnerList.AddRange(items);
		}

		/// <summary>
		/// Indicates whether a specified object is contained in the list.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public bool Contains(AttributeSet value)
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
			AttributeSet[] array,
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
		public int IndexOf(AttributeSet value)
		{
			return InnerList.IndexOf(value);
		}

		/// <summary>
		/// Insert element to list.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		public void Insert(int index, AttributeSet value)
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
		public AttributeSet ItemAt(int index)
		{
			return (AttributeSet)InnerList[index];
		}

		/// <summary>
		/// Remove element from list.
		/// </summary>
		/// <param name="value"></param>
		public void Remove(AttributeSet value)
		{
			InnerList.Remove(value);
		}
	}
}
