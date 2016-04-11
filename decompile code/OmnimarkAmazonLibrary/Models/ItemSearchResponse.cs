﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Xml.Serialization;


namespace Amazon.AWS
{
    // 
    // This source code was auto-generated by xsd, Version=4.0.30319.1.
    // 


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://webservices.amazon.com/AWSECommerceService/2011-08-01")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://webservices.amazon.com/AWSECommerceService/2011-08-01", IsNullable = false)]
    public partial class ItemSearchResponse
    {

        private ItemSearchResponseOperationRequest operationRequestField;

        private ItemSearchResponseItems itemsField;

        /// <remarks/>
        public ItemSearchResponseOperationRequest OperationRequest
        {
            get
            {
                return this.operationRequestField;
            }
            set
            {
                this.operationRequestField = value;
            }
        }

        /// <remarks/>
        public ItemSearchResponseItems Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://webservices.amazon.com/AWSECommerceService/2011-08-01")]
    public partial class ItemSearchResponseOperationRequest
    {

        private string requestIdField;

        private ItemSearchResponseOperationRequestArgument[] argumentsField;

        private decimal requestProcessingTimeField;

        /// <remarks/>
        public string RequestId
        {
            get
            {
                return this.requestIdField;
            }
            set
            {
                this.requestIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Argument", IsNullable = false)]
        public ItemSearchResponseOperationRequestArgument[] Arguments
        {
            get
            {
                return this.argumentsField;
            }
            set
            {
                this.argumentsField = value;
            }
        }

        /// <remarks/>
        public decimal RequestProcessingTime
        {
            get
            {
                return this.requestProcessingTimeField;
            }
            set
            {
                this.requestProcessingTimeField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://webservices.amazon.com/AWSECommerceService/2011-08-01")]
    public partial class ItemSearchResponseOperationRequestArgument
    {

        private string nameField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://webservices.amazon.com/AWSECommerceService/2011-08-01")]
    public partial class ItemSearchResponseItems
    {

        private ItemSearchResponseItemsRequest requestField;

        private uint totalResultsField;

        private uint totalPagesField;

        private string moreSearchResultsUrlField;

        private ItemSearchResponseItemsItem[] itemField;

        /// <remarks/>
        public ItemSearchResponseItemsRequest Request
        {
            get
            {
                return this.requestField;
            }
            set
            {
                this.requestField = value;
            }
        }

        /// <remarks/>
        public uint TotalResults
        {
            get
            {
                return this.totalResultsField;
            }
            set
            {
                this.totalResultsField = value;
            }
        }

        /// <remarks/>
        public uint TotalPages
        {
            get
            {
                return this.totalPagesField;
            }
            set
            {
                this.totalPagesField = value;
            }
        }

        /// <remarks/>
        public string MoreSearchResultsUrl
        {
            get
            {
                return this.moreSearchResultsUrlField;
            }
            set
            {
                this.moreSearchResultsUrlField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Item")]
        public ItemSearchResponseItemsItem[] Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://webservices.amazon.com/AWSECommerceService/2011-08-01")]
    public partial class ItemSearchResponseItemsRequest
    {

        private string isValidField;

        private ItemSearchResponseItemsRequestItemSearchRequest itemSearchRequestField;

        /// <remarks/>
        public string IsValid
        {
            get
            {
                return this.isValidField;
            }
            set
            {
                this.isValidField = value;
            }
        }

        /// <remarks/>
        public ItemSearchResponseItemsRequestItemSearchRequest ItemSearchRequest
        {
            get
            {
                return this.itemSearchRequestField;
            }
            set
            {
                this.itemSearchRequestField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://webservices.amazon.com/AWSECommerceService/2011-08-01")]
    public partial class ItemSearchResponseItemsRequestItemSearchRequest
    {

        private string keywordsField;

        private string responseGroupField;

        private string searchIndexField;

        /// <remarks/>
        public string Keywords
        {
            get
            {
                return this.keywordsField;
            }
            set
            {
                this.keywordsField = value;
            }
        }

        /// <remarks/>
        public string ResponseGroup
        {
            get
            {
                return this.responseGroupField;
            }
            set
            {
                this.responseGroupField = value;
            }
        }

        /// <remarks/>
        public string SearchIndex
        {
            get
            {
                return this.searchIndexField;
            }
            set
            {
                this.searchIndexField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://webservices.amazon.com/AWSECommerceService/2011-08-01")]
    public partial class ItemSearchResponseItemsItem
    {

        private string aSINField;

        private string parentASINField;

        private string detailPageURLField;

        private ItemSearchResponseItemsItemItemLink[] itemLinksField;

        private ItemSearchResponseItemsItemItemAttributes itemAttributesField;

        /// <remarks/>
        public string ASIN
        {
            get
            {
                return this.aSINField;
            }
            set
            {
                this.aSINField = value;
            }
        }

        /// <remarks/>
        public string ParentASIN
        {
            get
            {
                return this.parentASINField;
            }
            set
            {
                this.parentASINField = value;
            }
        }

        /// <remarks/>
        public string DetailPageURL
        {
            get
            {
                return this.detailPageURLField;
            }
            set
            {
                this.detailPageURLField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ItemLink", IsNullable = false)]
        public ItemSearchResponseItemsItemItemLink[] ItemLinks
        {
            get
            {
                return this.itemLinksField;
            }
            set
            {
                this.itemLinksField = value;
            }
        }

        /// <remarks/>
        public ItemSearchResponseItemsItemItemAttributes ItemAttributes
        {
            get
            {
                return this.itemAttributesField;
            }
            set
            {
                this.itemAttributesField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://webservices.amazon.com/AWSECommerceService/2011-08-01")]
    public partial class ItemSearchResponseItemsItemItemLink
    {

        private string descriptionField;

        private string uRLField;

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }
        

        /// <remarks/>
        public string URL
        {
            get
            {
                return this.uRLField;
            }
            set
            {
                this.uRLField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://webservices.amazon.com/AWSECommerceService/2011-08-01")]
    public partial class ItemSearchResponseItemsItemItemAttributes
    {

        private string authorField;

        private string manufacturerField;

        private string productGroupField;

        private string titleField;

        /// <remarks/>
        public string Author
        {
            get
            {
                return this.authorField;
            }
            set
            {
                this.authorField = value;
            }
        }

        /// <remarks/>
        public string Manufacturer
        {
            get
            {
                return this.manufacturerField;
            }
            set
            {
                this.manufacturerField = value;
            }
        }

        /// <remarks/>
        public string ProductGroup
        {
            get
            {
                return this.productGroupField;
            }
            set
            {
                this.productGroupField = value;
            }
        }

        /// <remarks/>
        public string Title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }
    }
}