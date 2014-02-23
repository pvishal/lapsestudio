using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gtk;
using MessageTranslation;
using Timelapse_API;
using Timelapse_UI;

namespace LapseStudioGtkUI
{
    public static class TreeViewHandler
    {
        public static int SelectedRow { get; private set; }
        private static ListStore TheTable;
		private static FileTreeHelper Helper = new FileTreeHelper(new GtkMessageBox(), new GtkFileDialog());

        public static TreeView Init(TreeView TheTree)
        {
            foreach (TreeViewColumn col in TheTree.Columns) { TheTree.RemoveColumn(col); }
            TreeView MainTree = TheTree;
            TheTable = new ListStore(typeof(string), typeof(bool), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string));

            TreeViewColumn NrColumn = new TreeViewColumn();
            TreeViewColumn FileColumn = new TreeViewColumn();
            TreeViewColumn BrightnessColumn = new TreeViewColumn();
            TreeViewColumn AVColumn = new TreeViewColumn();
            TreeViewColumn TVColumn = new TreeViewColumn();
            TreeViewColumn ISOColumn = new TreeViewColumn();
            TreeViewColumn KeyframeColumn = new TreeViewColumn();

            CellRendererText NrCell = new CellRendererText();
            CellRendererText FileCell = new CellRendererText();
            CellRendererText BrightnessCell = new CellRendererText();
            CellRendererText AVCell = new CellRendererText();
            CellRendererText TVCell = new CellRendererText();
            CellRendererText ISOCell = new CellRendererText();
            CellRendererToggle KeyframeCell = new CellRendererToggle();

            NrColumn.Title = Message.GetString("Nr");
            NrColumn.MinWidth = 35;
            NrColumn.Resizable = false;
            NrColumn.PackStart(NrCell, true);

            FileColumn.Title = Message.GetString("Filename");
            FileColumn.MinWidth = 100;
            FileColumn.Resizable = true;
            FileColumn.PackStart(FileCell, true);

            BrightnessColumn.Title = Message.GetString("Brightness");
            BrightnessColumn.MinWidth = 70;
            BrightnessColumn.Resizable = true;
            BrightnessColumn.PackStart(BrightnessCell, true);
            BrightnessCell.Editable = true;
            BrightnessCell.Edited += CellEdited;

            AVColumn.Title = Message.GetString("AV");
            AVColumn.MinWidth = 40;
            AVColumn.Resizable = true;
            AVColumn.PackStart(AVCell, true);

            TVColumn.Title = Message.GetString("TV");
            TVColumn.MinWidth = 40;
            TVColumn.Resizable = true;
            TVColumn.PackStart(TVCell, true);

            ISOColumn.Title = Message.GetString("ISO");
            ISOColumn.MinWidth = 40;
            ISOColumn.Resizable = true;
            ISOColumn.PackStart(ISOCell, true);

            KeyframeColumn.Title = Message.GetString("KF");
            KeyframeColumn.MinWidth = 30;
            KeyframeColumn.MaxWidth = 40;
            KeyframeColumn.PackStart(KeyframeCell, true);
            KeyframeColumn.AddAttribute(KeyframeCell, "active", (int)TableLocation.Keyframe);
            KeyframeCell.Activatable = true;
            KeyframeCell.Active = false;
            KeyframeCell.Toggled += KeyframeCell_Toggled;

            NrColumn.AddAttribute(NrCell, "text", (int)TableLocation.Nr);
            FileColumn.AddAttribute(FileCell, "text", (int)TableLocation.Filename);
            BrightnessColumn.AddAttribute(BrightnessCell, "text", (int)TableLocation.Brightness);
            AVColumn.AddAttribute(AVCell, "text", (int)TableLocation.AV);
            TVColumn.AddAttribute(TVCell, "text", (int)TableLocation.TV);
            ISOColumn.AddAttribute(ISOCell, "text", (int)TableLocation.ISO);
            KeyframeColumn.AddAttribute(KeyframeCell, "toggle", (int)TableLocation.Keyframe);


            MainTree.AppendColumn(NrColumn);
            MainTree.AppendColumn(KeyframeColumn);
            MainTree.AppendColumn(FileColumn);
            MainTree.AppendColumn(BrightnessColumn);
            MainTree.AppendColumn(AVColumn);
            MainTree.AppendColumn(TVColumn);
            MainTree.AppendColumn(ISOColumn);

            MainTree.Model = TheTable;
            return MainTree;
        }

        #region Events

        private static void CellEdited(object o, EditedArgs args)
        {
            try
            {
                double val;
                try { val = Convert.ToDouble(args.NewText); }
                catch { return; }

				Helper.UpdateBrightness(GetIndex(new TreePath(args.Path)), val);

                UpdateTable();
            }
            catch (Exception ex) { Error.Report("Cell edited", ex); }
        }

        private static void KeyframeCell_Toggled(object o, ToggledArgs args)
        {
            try
            {
                if (GetToggleState(new TreePath(args.Path), TableLocation.Keyframe) == false)
                {
					Helper.OpenMetaData(GetIndex(new TreePath(args.Path)));
                    UpdateTable();
                }
                else
                {
                    //TODO: does lapsestudio need keyframes?
                    ProjectManager.RemoveKeyframe(SelectedRow, true);
                    UpdateTable();
                }
            }
            catch (Exception ex) { Error.Report("Keyframecell toggled", ex); }
        }
        
        #endregion

        #region Methods

        public static void UpdateRow(TreePath path)
        {
            SelectedRow = GetIndex(path);
        }

        public static int GetIndex(TreePath path)
        {
            TreeIter iter;
            TheTable.GetIter(out iter, path);
            return TheTable.GetPath(iter).Indices[0];
        }

        public static TreePath GetFirstPath()
        {
            TreeIter iter;
            TheTable.GetIterFirst(out iter);
            return TheTable.GetPath(iter);
        }

        public static bool GetToggleState(TreePath path, TableLocation Column)
        {
            try
            {
                TreeIter iter;
                TheTable.GetIter(out iter, path);
                return (bool)TheTable.GetValue(iter, (int)Column);
            }
            catch { return false; }
        }

        public static void UpdateTable()
        {
            List<Timelapse_API.Frame> Framelist = ProjectManager.CurrentProject.Frames;
            TheTable.Clear();

            ArrayList LScontent = new ArrayList();
            int index;
            for (int i = 0; i < TheTable.NColumns; i++) { LScontent.Add("N/A"); }

            for (int i = 0; i < Framelist.Count; i++)
            {
                //Nr
                index = (int)TableLocation.Nr;
                LScontent[index] = Convert.ToString(i + 1);
                //Filenames
                index = (int)TableLocation.Filename;
                LScontent[index] = Framelist[i].Filename;
                //Brightness
                index = (int)TableLocation.Brightness;
                LScontent[index] = Framelist[i].OriginalBrightness.ToString("N3");
                //AV
                index = (int)TableLocation.AV;
                if (Framelist[i].AVstring != null) { LScontent[index] = Framelist[i].AVstring; }
                else { LScontent[index] = "N/A"; }
                //TV
                index = (int)TableLocation.TV;
                if (Framelist[i].TVstring != null) { LScontent[index] = Framelist[i].TVstring; }
                else { LScontent[index] = "N/A"; }
                //ISO
                index = (int)TableLocation.ISO;
                if (Framelist[i].SVstring != null) { LScontent[index] = Framelist[i].SVstring; }
                else { LScontent[index] = "N/A"; }
                //Keyframes
                index = (int)TableLocation.Keyframe;
                if (Framelist[i].IsKeyframe) { LScontent[index] = true; }
                else { LScontent[index] = false; }

                //filling the table
                TheTable.AppendValues(LScontent[0], LScontent[1], LScontent[2], LScontent[3], LScontent[4], LScontent[5], LScontent[6]);
            }
        }

        #endregion
    }
}
