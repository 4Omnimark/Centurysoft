#region Copyright
//	Copyright (c) 2013 eBay, Inc.
//	
//	This program is licensed under the terms of the eBay Common Development and
//	Distribution License (CDDL) Version 1.0 (the "License") and any subsequent  
//	version thereof released by eBay.  The then-current version of the License can be 
//	found at http://www.opensource.org/licenses/cddl1.php and in the eBaySDKLicense 
//	file that is under the eBay SDK ../docs directory
#endregion

using System.Collections;

namespace CodeGen
{
	public class InputParamParser
	{
		private static Hashtable _keys = null;

		public static Hashtable getInputParams(string[] args)
		{
			if (null == _keys) 
			{
				init();
			}

			int plen = args.Length;

			if (plen % 2 > 0) 
			{
				throw new System.Exception("Input params should be in key/value pairs !");
			}

			Hashtable retHash = new Hashtable();
			string opt;

			for (int i = 0; i < plen; i++) 
			{
				opt = args[i];
				opt = opt.Trim();
				if (_keys.Contains(opt)) 
				{
					retHash.Add(opt, args[++i]);
				}
				else 
				{
					i++;
				}
			}

			return retHash;
		}

		private static void init()
		{
			if (null == _keys) 
			{
				 _keys = new Hashtable();
				 _keys.Add(F, "output file name");
				 _keys.Add(H, "help message");
				 _keys.Add(L, "language");
				 _keys.Add(M, "output file mode");
				 _keys.Add(N, "namespace");
				 _keys.Add(P, "output path");
				 _keys.Add(W, "wsdl file path");
			}
		}

		public static void setInputParamKeys(Hashtable keys) 
		{
		  _keys = keys;
		}

		public static string F = "-f";
		public static string H = "-h";
		public static string L = "-l";
		public static string M = "-m";
		public static string N = "-n";
		public static string P = "-p";
		public static string W = "-w";
	}
}