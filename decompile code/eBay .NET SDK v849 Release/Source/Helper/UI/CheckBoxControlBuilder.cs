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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace Samples.Helper.UI
{
	/// <summary>
	/// Summary description for CheckBoxControlBuilder.
	/// </summary>
	public class CheckBoxControlBuilder
	{
		private CheckBoxControlBuilder()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="panel"></param>
		/// <param name="labels"></param>
		/// <param name="xOffset"></param>
		/// <param name="yOffset"></param>
		/// <param name="height"></param>
		/// <param name="widthFactor"></param>
		/// <returns></returns>
		public static CheckBox[] CreateCheckBoxControls(
			Panel panel, string[] labels, int xOffset, int yOffset, int height, int widthFactor)
		{
			Rectangle rect = panel.DisplayRectangle;

			int numOfCheckBoxes = labels.Length;

			int w = GetMaxItemLableLength(labels)*widthFactor;

			int colSize = (rect.Width - xOffset*2)/w;

			xOffset = (rect.Width - w*colSize)/2;
			CheckBox[] controls = new CheckBox[numOfCheckBoxes];
			
			for (int i = 0; i < numOfCheckBoxes; i++) 
			{
				int row = i / colSize;
				int col = i % colSize;
				CheckBox checkBox = new System.Windows.Forms.CheckBox();
				checkBox.Width = w;
				checkBox.Location = new Point(xOffset + col*w, yOffset + row*height);
				checkBox.Name = "checkBox" + i;
				checkBox.TabIndex = i;
				checkBox.Height = height;
				checkBox.Text = labels[i];
				controls[i] = checkBox;
			}

			panel.Controls.AddRange(controls);
			return controls;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="panel"></param>
		/// <param name="items"></param>
		/// <param name="xOffset"></param>
		/// <param name="yOffset"></param>
		/// <param name="height"></param>
		/// <param name="widthFactor"></param>
		/// <returns></returns>
		public static CheckBox[] CreateCheckBoxControls(
			Panel panel, ControlTagItem[] items, int xOffset, int yOffset, int height, int widthFactor)
		{
			Rectangle rect = panel.DisplayRectangle;

			int numOfCheckBoxes = items.Length;

			int w = GetMaxItemLableLength(items)*widthFactor;
			int colSize = (rect.Width - xOffset*2)/w;
			if (colSize == 0) 
			{
				colSize = 1;
			}

			xOffset = (rect.Width - w*colSize)/2;
			if (xOffset < 0) 
			{
				xOffset = 10;
			}
			CheckBox[] controls = new CheckBox[numOfCheckBoxes];
			
			for (int i = 0; i < numOfCheckBoxes; i++) 
			{
				int row = i / colSize;
				int col = i % colSize;
				CheckBox checkBox = new System.Windows.Forms.CheckBox();
				checkBox.Width = w;
				checkBox.Location = new Point(xOffset + col*w, yOffset + row*height);
				checkBox.Name = items[i].Tag.ToString();
				checkBox.TabIndex = i;
				checkBox.Height = height;
				checkBox.Text = items[i].Text;
				controls[i] = checkBox;
			}

			panel.Controls.AddRange(controls);

			return controls;
		}

		static int GetMaxItemLableLength(string [] labels)
		{
			int max = 0;
			int len = 0;
			for (int i = 0; i < labels.Length; i++) 
			{
				len = labels[0].Length;
				if (max < len) 
				{
					max = len;
				}
			}

			return max;
		}

		static int GetMaxItemLableLength(ControlTagItem[] items)
		{
			int max = 0;
			int len = 0;
			for (int i = 0; i < items.Length; i++) 
			{
				len = items[0].Text.Length;
				if (max < len) 
				{
					max = len;
				}
			}

			return max;
		}
	}
}
