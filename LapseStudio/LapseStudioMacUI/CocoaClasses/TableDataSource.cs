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
		public List<ArrayList> Rows = new List<ArrayList>();

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
			switch(col.HeaderCell.StringValue)
			{
				case "AV":
					return new NSString(Rows[row][(int)TableLocation.AV].ToString());
				case "TV":
					return new NSString(Rows[row][(int)TableLocation.TV].ToString());
				case "ISO":
					return new NSString(Rows[row][(int)TableLocation.ISO].ToString());
				case "KF":
					return NSObject.FromObject(((bool)(Rows[row][(int)TableLocation.Keyframe])) ? NSCellStateValue.On : NSCellStateValue.Off);
				case "Filename":
					return new NSString(Rows[row][(int)TableLocation.Filename].ToString());
				case "Nr":
					return new NSString(Rows[row][(int)TableLocation.Nr].ToString());
				case "Brightness":
					return new NSString(Rows[row][(int)TableLocation.Brightness].ToString());

				default:
					return new NSString("N/A");
			}
		}

		public void Add(ArrayList row)
		{
			Rows.Add(new ArrayList(row));
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
