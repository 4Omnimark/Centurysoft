#region Copyright
//	Copyright (c) 2013 eBay, Inc.
//	
//	This program is licensed under the terms of the eBay Common Development and
//	Distribution License (CDDL) Version 1.0 (the "License") and any subsequent  
//	version thereof released by eBay.  The then-current version of the License can be 
//	found at http://www.opensource.org/licenses/cddl1.php and in the eBaySDKLicense 
//	file that is under the eBay SDK ../docs directory
#endregion

namespace eBay.WebService.CodeGenerator
{
  using System;
  using System.IO;
  using System.Text;
  using Microsoft.Win32;
  using System.Runtime.InteropServices;
  using CustomToolGenerator;

  /// <summary>
  /// Inheriter must supply the GuidAttribute and CustomToolAttribute 
  /// </summary>
  public abstract class CustomToolBase: BaseCodeGeneratorWithSite
  {

    #region Static Section

    internal static Guid CSharpCategoryGuid = new Guid("FAE04EC1-301F-11D3-BF4B-00C04F79EFBC");

    /// <summary>
    /// 
    /// </summary>
    /// <param name="t"></param>
    [ComRegisterFunction]
    public static void RegisterClass(Type t)
    {
      GuidAttribute guidAttribute = getGuidAttribute(t);
      CustomToolAttribute customToolAttribute = getCustomToolAttribute(t);
      using( RegistryKey key = Registry.LocalMachine.CreateSubKey(GetKeyName(CSharpCategoryGuid, customToolAttribute.Name)))
      {
        key.SetValue("", customToolAttribute.Description);
        key.SetValue("CLSID", "{" + guidAttribute.Value + "}");
        key.SetValue("GeneratesDesignTimeSource", 1);
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="t"></param>
    [ComUnregisterFunction]
    public static void UnregisterClass(Type t)
    {
      CustomToolAttribute customToolAttribute = getCustomToolAttribute(t);
      Registry.LocalMachine.DeleteSubKey(GetKeyName(CSharpCategoryGuid, customToolAttribute.Name), false);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    internal static GuidAttribute getGuidAttribute(Type t)
    {
      return (GuidAttribute)getAttribute(t, typeof(GuidAttribute));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    internal static CustomToolAttribute getCustomToolAttribute(Type t)
    {
      return (CustomToolAttribute)getAttribute(t, typeof(CustomToolAttribute));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="t"></param>
    /// <param name="attributeType"></param>
    /// <returns></returns>
    internal static Attribute getAttribute(Type t, Type attributeType)
    {
      object[] attributes = t.GetCustomAttributes(attributeType, /* inherit */ true);
      if(attributes.Length==0)
        throw new Exception(String.Format("Class '{0}' does not provide a '{1}' attribute.", t.FullName, attributeType.FullName));
      return (Attribute)attributes[0];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="categoryGuid"></param>
    /// <param name="toolName"></param>
    /// <returns></returns>
    internal static string GetKeyName(Guid categoryGuid, string toolName)
    {
#if VS2008
        String keyName = String.Format("SOFTWARE\\Microsoft\\VisualStudio\\9.0\\Generators\\{{{0}}}\\{1}\\", categoryGuid.ToString(), toolName);
#else
		String keyName = String.Format("SOFTWARE\\Microsoft\\VisualStudio\\8.0\\Generators\\{{{0}}}\\{1}\\", categoryGuid.ToString(), toolName);
#endif

		return keyName;
    }

    #endregion

    /// <summary>
    /// 
    /// </summary>
    protected CustomToolBase()
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputFileName"></param>
    /// <param name="inputFileContent"></param>
    /// <returns></returns>
    ///    
    //commented by william, 3/23/2008
    /*
    protected override byte[] GenerateCode(string inputFileName, string inputFileContent)
    {

      // get the generated code
      string returnString = this.DoGenerateCode(inputFileName, inputFileContent);

      // return the generated code
      return System.Text.Encoding.ASCII.GetBytes(returnString);

    }*/

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputFileName"></param>
    /// <param name="inputFileContent"></param>
    /// <returns></returns>
    //commented by william, 3/23/2008
    //public abstract string DoGenerateCode(string inputFileName, string inputFileContent);

  }
}
