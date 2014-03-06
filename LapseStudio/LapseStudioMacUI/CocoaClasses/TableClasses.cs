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
		public delegate void BrightnessUpdate(int Row, string value);
		public event BrightnessUpdate BrightnessCellEdited;

		public override void SetObjectValue(NSTableView tableView, NSObject theObject, NSTableColumn tableColumn, int row)
		{
			NSTableColumn[] cols = tableView.TableColumns();
			int idx = cols.ToList().FindIndex(t => t.HeaderCell.StringValue == tableColumn.HeaderCell.StringValue);
			if (idx == (int)TableLocation.Brightness && BrightnessCellEdited != null) BrightnessCellEdited(row, theObject.ToString());
		}

		public override NSObject GetObjectValue(NSTableView tableView, NSTableColumn tableColumn, int row)
		{
			NSTableColumn[] cols = tableView.TableColumns();
			int idx = cols.ToList().FindIndex(t => t.HeaderCell.StringValue == tableColumn.HeaderCell.StringValue);

			switch(idx)
			{
				case (int)TableLocation.AV:
					return new NSString(Rows[row][idx].ToString());
				case (int)TableLocation.TV:
					return new NSString(Rows[row][idx].ToString());
				case (int)TableLocation.ISO:
					return new NSString(Rows[row][idx].ToString());
				case (int)TableLocation.Keyframe:
					return NSObject.FromObject(((bool)(Rows[row][idx])) ? NSCellStateValue.On : NSCellStateValue.Off);
				case (int)TableLocation.Filename:
					return new NSString(Rows[row][idx].ToString());
				case (int)TableLocation.Nr:
					return new NSString(Rows[row][idx].ToString());
				case (int)TableLocation.Brightness:
					return new NSString(Rows[row][idx].ToString());

				default:
					return new NSString("N/A");
			}
		}

		[Export ("numberOfRowsInTableView:")]
		public int NumberOfRowsInTableView(NSTableView table)
		{
			return Rows.Count;
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

	public class TableDelegate : NSTableViewDelegate
	{
		public delegate void ChangeDelegate();
		public event ChangeDelegate TableSelectionChanged;

		public override void SelectionDidChange (NSNotification notification)
		{
			if(TableSelectionChanged != null) TableSelectionChanged();
		}

		public override bool ShouldSelectRow (NSTableView tableView, int row)
		{
			return true;
		}
	}
}
