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
using System.IO;
using System.Xml;
using System.Collections;
using System.Windows.Forms;
using eBay.WebService.CodeGenerator;

namespace CodeGen
{
	/// <summary>
	/// Summary description
	/// </summary>
	class CodeGen
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Hashtable ht = null;
			bool inputError = false;
			try 
			{
				ht = InputParamParser.getInputParams(args);
			}
			catch(Exception ex) 
			{
                inputError = true;
			}

			object obj = ht[InputParamParser.H];

			if (inputError || ht.Count == 0 || obj != null) 
			{
				Console.WriteLine("Usage:  CodeGen -f outputFile -l language -m outputMode [1 = one file, 0 = multiple files] -n namespace -p outputPath -w wsdlPath");
				return;
			}

			string ns = null;
			string fileName = null;
			string lang = "cs";
			string wsdl = null;
			bool oneFile = false;
			string outputPath = null;

			obj = ht[InputParamParser.F];
			if (obj != null) 
			{
				fileName = obj.ToString();
			}

			obj = ht[InputParamParser.L];
			if (obj != null) 
			{
				lang = obj.ToString();
			}

			string ONE = "1";
			obj = ht[InputParamParser.M];
			if (obj != null && ONE.Equals(obj.ToString()))
			{
				oneFile = true;
			}

			obj = ht[InputParamParser.N];
			if (obj != null) 
			{
				ns = obj.ToString();
			}

			obj = ht[InputParamParser.P];
			if (obj != null)
			{
				string path = obj.ToString();
				if (!Directory.Exists(path)) 
				{
					Directory.CreateDirectory(path);
				}
				outputPath = path;
			}

			obj = ht[InputParamParser.W];
			if (obj != null) 
			{
				wsdl = obj.ToString();
			}
					
			if (wsdl == null || wsdl.Length == 0) 
			{
				Console.WriteLine("Please provide a valid wsdl path.");
				return;
			}

			eBayCodeGenerator codeGen = new eBayCodeGenerator();
			
			if (oneFile) 
			{
				if (fileName == null || fileName.Length == 0) 
				{
					Console.WriteLine("Please provide a valid output file name.");
					return;
				}
			}
			codeGen.SetOutputOneFileOption(oneFile);

			if (outputPath == null) 
			{
				Console.WriteLine("Please provide a valid output path.");
				return;
			}

			codeGen.SetCodeLanguage(lang);
			if (ns != null) 
			{
				codeGen.SetNamespace(ns);
			}
			codeGen.SetOutputPath(outputPath);
			string fileContent = String.Format(WSDL, wsdl);
			codeGen.Generate(fileName, fileContent);
		}

		private static string WSDL = "<?xml version=\"1.0\" encoding=\"utf-8\"?><configuration><WSDLFile>{0}</WSDLFile></configuration>";
	}
}
