using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;
using Timelapse_UI;
using Timelapse_API;

namespace LapseStudioMacUI
{
	[Register ("TableViewDataSource")]
	public class TableDataSource : NSTableViewDataSource
	{
		List<ArrayList> Rows = new List<ArrayList>();

		public TableDataSource ()
		{
		}

		[Export ("numberOfRowsInTableView:")]
		public int NumberOfRowsInTableView(NSTableView table)
		{
			return Rows.Count;
		}

		[Export ("tableView:objectValueForTableColumn:row:")]
		public NSObject ObjectValueForTableColumn(NSTableView table, NSTableColumn col, int row)
		{
			switch(col.DataCell.StringValue)
			{
				case "AV":
					return new NSString(Rows[row][(int)TableLocation.AV].ToString());
				case "TV":
					return new NSString(Rows[row][(int)TableLocation.TV].ToString());
				case "ISO":
					return new NSString(Rows[row][(int)TableLocation.ISO].ToString());
				case "KF":
					return new NSString(Rows[row][(int)TableLocation.Keyframe].ToString());
				case "Filename":
					return new NSString(Rows[row][(int)TableLocation.Filename].ToString());
				case "Nr":
					return new NSString(Rows[row][(int)TableLocation.Nr].ToString());
				case "Brightness":
					return new NSString(Rows[row][(int)TableLocation.Brightness].ToString());

				default:
					return new NSObject();
			}
		}

		public void Add(ArrayList row)
		{
			Rows.Add(row);
		}

		public void Clear()
		{
			Rows.Clear();
		}

		public void Remove(int index)
		{
			if (index >= 0 && index < Rows.Count)
			{
				Rows.RemoveAt(index);
			}
		}

	}
}
