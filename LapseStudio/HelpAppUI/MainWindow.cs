using Gtk;
using HelpAppUI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using PathTool = System.IO.Path;

public partial class MainWindow: Gtk.Window
{
	CultureInfo culture = new CultureInfo("en-US");
	MessageDialog dlg;

    List<PP3Entry> SingleEntryList = new List<PP3Entry>();
    ListStore SingleEntryStore;

	public MainWindow() : base(Gtk.WindowType.Toplevel)
	{
		Build();
		dlg = new MessageDialog(this, DialogFlags.Modal, MessageType.Error, ButtonsType.Ok, "");
	}

	protected void OnDeleteEvent(object sender, DeleteEventArgs a)
	{
		Application.Quit();
		a.RetVal = true;
	}

	protected void OnBrowseButton1Clicked(object sender, EventArgs e)
	{
		using(FileChooserDialog fdlg = new FileChooserDialog("Open PP3", this, FileChooserAction.Open, "Cancel", ResponseType.Cancel, "Select", ResponseType.Ok))
		{
			FileFilter ft = new FileFilter();
			ft.Name = "PP3 File";
			ft.AddPattern("*.pp3");
			ft.AddPattern("*.PP3");
			fdlg.AddFilter(ft);
			if ((ResponseType)fdlg.Run() == ResponseType.Ok)
			{
				PP3PathBox1.Text = fdlg.Filename;
				fdlg.Destroy();
			}
		}
	}

	protected void OnBrowseButton2Clicked(object sender, EventArgs e)
	{
		using(FileChooserDialog fdlg = new FileChooserDialog("Open PP3", this, FileChooserAction.Open, "Cancel", ResponseType.Cancel, "Select", ResponseType.Ok))
		{
			FileFilter ft = new FileFilter();
			ft.Name = "PP3 File";
			ft.AddPattern("*.pp3");
			ft.AddPattern("*.PP3");
			fdlg.AddFilter(ft);
			if ((ResponseType)fdlg.Run() == ResponseType.Ok)
			{
				PP3PathBox2.Text = fdlg.Filename;
				fdlg.Destroy();
			}
		}
	}

	/// <summary>
	/// Compares 2 PP3 versions for different entries
	/// </summary>
	protected void OnCompareVersionsButtonClicked(object sender, EventArgs e)
	{
		if(!File.Exists(PP3PathBox1.Text) || !File.Exists(PP3PathBox2.Text))
		{
			dlg.Text = "Files not found";
			dlg.ShowNow();
			return;
		}

	}

	/// <summary>
    /// Compares the original RT PP3 file with a newly written PP3 file
	/// </summary>
	protected void OnCompareSameButtonClicked(object sender, EventArgs e)
	{
        //TODO: compare written and original PP3 file
	}

	/// <summary>
	/// Creates datatype table from Nr.pp3, Nr_min.pp3 and Nr_max.pp3 
	/// </summary>
	protected void OnSingleVersionButtonClicked(object sender, EventArgs e)
	{
		SingleEntryList = GetEntries(PP3PathBox1.Text);
        TreeView1 = InitPP3EntryList(TreeView1, ref SingleEntryStore);
        UpdatePP3EntryTable(SingleEntryList, ref SingleEntryStore);
	}

	/// <summary>
	/// Compares a bunch of PP3 versions in a folder
	/// </summary>
	protected void OnBatchComparisonButtonClicked(object sender, EventArgs e)
	{
		string folderpath;
		using(FileChooserDialog fdlg = new FileChooserDialog("Select PP3 Folder", this, FileChooserAction.SelectFolder, "Cancel", ResponseType.Cancel, "Select", ResponseType.Ok))
		{
			if((ResponseType)fdlg.Run() == ResponseType.Ok)
			{
				folderpath = fdlg.Filename;
				fdlg.Destroy();
			}
			else return;
		}

		string[] files = Directory.GetFiles(folderpath, "*.pp3");
        string[] nfiles = files.Where(t => !t.Contains("_min") && !t.Contains("_max")).OrderBy(t => t).ToArray();

		List<PP3Entry>[] entries = new List<PP3Entry>[nfiles.Length];
        for (int i = 0; i < nfiles.Length; i++) entries[i] = GetEntries(nfiles[i]);

        foreach (List<PP3Entry> entrylist in entries) SaveCode(entrylist);
        foreach (List<PP3Entry> entrylist in entries) SaveDefinition(entrylist);
	}
    
	/// <summary>
	/// Saves C# code for one PP3 version
	/// </summary>
	protected void OnSaveCodeButtonClicked(object sender, EventArgs e)
	{
        SaveCode(SingleEntryList);
	}

	/// <summary>
	/// Saves datatypes and fields for a PP3 version
	/// </summary>
	protected void OnSavePP3DefinitionButtonClicked(object sender, EventArgs e)
	{
        SaveDefinition(SingleEntryList);
    }

    #region Methods
    
    /// <summary>
    /// Gets all entries of a pp3 file
    /// </summary>
    /// <returns>The entries</returns>
    /// <param name="lines_n">Lines with data type information</param>
    /// <param name="lines_min">Lines with min value</param>
    /// <param name="lines_max">Lines with max value</param>
    private List<PP3Entry> GetEntries(string path)
    {
        string fn = PathTool.GetFileNameWithoutExtension(path);
        string file_min = PathTool.Combine(PathTool.GetDirectoryName(path), fn + "_min.pp3");
        string file_max = PathTool.Combine(PathTool.GetDirectoryName(path), fn + "_max.pp3");
        if (!File.Exists(path) || !File.Exists(file_min) || !File.Exists(file_max))
        {
            dlg.Text = "File not found";
            dlg.ShowNow();
            return new List<PP3Entry>();
        }

        string[] lines_n = File.ReadAllLines(path).Where(t => !string.IsNullOrWhiteSpace(t)).ToArray();
        string[] lines_min = File.ReadAllLines(file_min).Where(t => !string.IsNullOrWhiteSpace(t)).ToArray();
        string[] lines_max = File.ReadAllLines(file_max).Where(t => !string.IsNullOrWhiteSpace(t)).ToArray();

        if (lines_n.Length != lines_min.Length || lines_n.Length != lines_max.Length)
        {
            dlg.Text = "Line count does not match";
            dlg.ShowNow();
            return new List<PP3Entry>();
        }

        List<PP3Entry> entries = new List<PP3Entry>();
        string CurrentTopic = "";

        for (int i = 0; i < lines_n.Length; i++)
        {
            if (lines_n[i].StartsWith("[")) CurrentTopic = lines_n[i].Trim('[', ']');
            else
            {
                string name = lines_n[i].Substring(0, lines_n[i].IndexOf("=")).Trim();
                string value = lines_n[i].Substring(lines_n[i].IndexOf("=") + 1).Trim().ToLower();
                string min = lines_min[i].Substring(lines_min[i].IndexOf("=") + 1).Trim();
                string max = lines_max[i].Substring(lines_max[i].IndexOf("=") + 1).Trim();
                
                entries.Add(new PP3Entry(CurrentTopic, name, GetType(value), min, max));
                if (name.ToLower() == "version") entries[entries.Count - 1].Value = value;
                else if (name.ToLower() == "appversion") entries[entries.Count - 1].Value = value;
            }
        }
        return entries;
    }
    
    private PP3DataType GetType(string value)
    {
        PP3DataType tp;
        double tmp_double;
        int tmp_int;

        int pc = value.Count(t => t == '.');
        if (pc == 1 && !value.Contains(';'))
        {
            if (double.TryParse(value, NumberStyles.Any, culture, out tmp_double)) tp = PP3DataType.Double;
            else tp = PP3DataType.String;
        }
        else if (pc > 1 && !value.Contains(';')) tp = PP3DataType.VersionNr;
        else if (value.Contains(';'))
        {
            string[] ce = value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (ce.Length == 0) tp = PP3DataType.String;
            else
            {
                double tmp;
                bool iscurve = true;
                foreach (string s in ce) { if (!double.TryParse(s, NumberStyles.Any, culture, out tmp)) iscurve = false; break; }
                if (iscurve) tp = PP3DataType.Curve;
                else tp = PP3DataType.String;
            }
        }
        else if (value == "false" || value == "true") tp = PP3DataType.Bool;
        else
        {
            if (int.TryParse(value, out tmp_int)) tp = PP3DataType.Int;
            else tp = PP3DataType.String;
        }
        return tp;
    }

    private void SaveCode(List<PP3Entry> EntryList)
    {
        if (!Directory.Exists("PP3Code")) Directory.CreateDirectory("PP3Code");
        if (EntryList == null || EntryList.Count == 0)
        {
            dlg.Text = "PP3 hasn't been read yet";
            dlg.ShowNow();
            return;
        }

        string name = EntryList.Find(t => t.Name.ToLower() == "version").Value;
        if (name == null)
        {
            dlg.Text = "No version number found";
            dlg.ShowNow();
            return;
        }
        string path = PathTool.Combine("PP3Code", name + ".cs");

        using (StreamWriter writer = new StreamWriter(path, false))
        {

            //Write writing code:
            writer.WriteLine(@"using System;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;

namespace Timelapse_API
{
	public partial class PP3
	{");
            writer.WriteLine("		private void Write" + name + "(StreamWriter writer)");
            writer.WriteLine(@"		{
			CultureInfo culture = new CultureInfo(""en-US"");
			string digits = ""F16"";");
            writer.WriteLine();

            string CurrentTopic = "";
            foreach (PP3Entry entry in EntryList)
            {
                if (CurrentTopic != entry.Topic)
                {
                    if (CurrentTopic != "") writer.WriteLine("			writer.WriteLine();");
                    writer.WriteLine("			writer.WriteLine(\"[" + entry.Topic + "]\");");
                    CurrentTopic = entry.Topic;
                }

                string nextline = "			writer.WriteLine(\"" + entry.Name + "=\" + ";
                if (entry.Type == PP3DataType.Double)
                {
                    if (entry.Name.ToLower() == "compensation") nextline += "NewCompensation.ToString(digits, culture));";
                    else nextline += "((double)Values[\"" + entry.SafeTopic + "." + entry.Name + "\"].Value).ToString(digits, culture));";
                }
                else if (entry.Type == PP3DataType.Bool) nextline += "Values[\"" + entry.SafeTopic + "." + entry.Name + "\"].Value.ToString().ToLower());";
                else if (entry.Type == PP3DataType.Curve) nextline += "((PP3Curve)Values[\"" + entry.SafeTopic + "." + entry.Name + "\"].Value).ToString());";
                else if (entry.Name.ToLower() == "version") nextline += "FileVersion.ToString());";
                else nextline += "Values[\"" + entry.SafeTopic + "." + entry.Name + "\"].Value.ToString());";

                writer.WriteLine(nextline);
            }
            writer.WriteLine(@"
		}");

            //Write reading code:
            writer.WriteLine();
            writer.WriteLine("		private void Read" + name + "(string[] lines)");
            writer.WriteLine(@"		{
			CultureInfo culture = new CultureInfo(""en-US"");
			List<string> Llines = lines.ToList();
			Llines.RemoveAll(t => String.IsNullOrWhiteSpace(t) || t.StartsWith(""[""));
			int i = 0;");
            writer.WriteLine();
         
            foreach (PP3Entry entry in EntryList)
            {
                string subline = "GetValue(Llines[i])";
                if (entry.Name.ToLower() == "version") writer.WriteLine("			FileVersion = Convert.ToInt32(" + subline + "); i++;");
                else if (entry.Name.ToLower() == "compensation") writer.WriteLine("			Compensation = Convert.ToDouble(" + subline + ", culture); i++;");
                else
                {
                    string newline = "			Values.Add(\"" + entry.SafeTopic + "." + entry.Name + "\", new PP3entry(\"" + entry.SafeTopic + "." + entry.Name + "\", ";
                    if (entry.Type == PP3DataType.String || entry.Type == PP3DataType.VersionNr) newline += subline + ", null, null";
                    else if (entry.Type == PP3DataType.Int) newline += "Convert.ToInt32(" + subline + "), " + GetMinMaxString(entry);
                    else if (entry.Type == PP3DataType.Double) newline += "Convert.ToDouble(" + subline + ", culture), " + GetMinMaxString(entry);
                    else if (entry.Type == PP3DataType.Bool) newline += "Convert.ToBoolean(" + subline + "), null, null";
                    else if (entry.Type == PP3DataType.Curve) newline += "PP3Curve.GetCurve(" + subline + "), null, null";

                    writer.WriteLine(newline + ")); i++;");
                }
            }

            writer.WriteLine(@"
			NewCompensation = Compensation;
		}
	}
}");
        }
    }

    private string GetMinMaxString(PP3Entry entry)
    {
        if (entry.Topic.ToLower() == "crop" || (entry.Topic.ToLower() == "resize" && entry.Name.ToLower() != "scale")) return "0, 0";
        else if (entry.Type == PP3DataType.Int) return ((int)entry.MinValue).ToString() + ", " + ((int)entry.MaxValue).ToString();
        else if (entry.Type == PP3DataType.Double) return ((double)entry.MinValue).ToString(culture) + "d, " + ((double)entry.MaxValue).ToString(culture) + "d";
        else return "null, null";
    }

    private void SaveDefinition(List<PP3Entry> EntryList)
    {
        if (!Directory.Exists("PP3Definitions")) Directory.CreateDirectory("PP3Definitions");
        if (EntryList == null || EntryList.Count == 0)
        {
            dlg.Text = "PP3 hasn't been read yet";
            dlg.ShowNow();
            return;
        }

        string name = EntryList.Find(t => t.Name.ToLower() == "version").Value;
        if (name == null)
        {
            dlg.Text = "No version number found";
            dlg.ShowNow();
            return;
        }
        string path = PathTool.Combine("PP3Definitions", name + ".pp3d");
        int i = 1;
        while (i < 10 && File.Exists(path)) { path = PathTool.Combine("PP3Definitions", name + "_" + i + ".pp3d"); i++; }
        if (i >= 10)
        {
            dlg.Text = "Too much existing files already";
            dlg.ShowNow();
            return;
        }

        using (StreamWriter writer = new StreamWriter(path, false))
        {
            string CurrentTopic = "";
            foreach (PP3Entry entry in EntryList)
            {
                if (CurrentTopic != entry.Topic)
                {
                    if (CurrentTopic != "") writer.WriteLine();
                    writer.WriteLine("[" + entry.Topic + "]");
                    CurrentTopic = entry.Topic;
                }

                writer.WriteLine("#" + entry.Name + ":");
                writer.WriteLine("-DataType: " + entry.Type.ToString());
                if (entry.MinValue == null) { }
                else if (entry.Topic.ToLower() == "crop" || entry.Topic.ToLower() == "version" || (entry.Topic.ToLower() == "resize" && entry.Name.ToLower() != "scale")) { }
                else if (entry.Type == PP3DataType.Int) writer.WriteLine("-Min: " + ((int)entry.MinValue).ToString());
                else if (entry.Type == PP3DataType.Double) writer.WriteLine("-Min: " + ((double)entry.MinValue).ToString(culture));
                if (entry.MaxValue == null) { }
                else if (entry.Topic.ToLower() == "crop" || entry.Topic.ToLower() == "version" || (entry.Topic.ToLower() == "resize" && entry.Name.ToLower() != "scale")) { }
                else if (entry.Type == PP3DataType.Int) writer.WriteLine("-Max: " + ((int)entry.MaxValue).ToString(culture));
                else if (entry.Type == PP3DataType.Double) writer.WriteLine("-Max: " + ((double)entry.MaxValue).ToString(culture));
            }
        }
    }

    private TreeView InitPP3EntryList(TreeView TheTree, ref ListStore TheTable)
    {
        foreach (TreeViewColumn col in TheTree.Columns) { TheTree.RemoveColumn(col); }
        TreeView MainTree = TheTree;
        TheTable = new ListStore(typeof(string), typeof(string), typeof(string), typeof(string), typeof(string), typeof(string));

        TreeViewColumn TopicColumn = new TreeViewColumn();
        TreeViewColumn NameColumn = new TreeViewColumn();
        TreeViewColumn TypeColumn = new TreeViewColumn();
        TreeViewColumn ErrorColumn = new TreeViewColumn();
        TreeViewColumn MinColumn = new TreeViewColumn();
        TreeViewColumn MaxColumn = new TreeViewColumn();

        CellRendererText TopicCell = new CellRendererText();
        CellRendererText NameCell = new CellRendererText();
        CellRendererText TypeCell = new CellRendererText();
        CellRendererText ErrorCell = new CellRendererText();
        CellRendererText MinCell = new CellRendererText();
        CellRendererText MaxCell = new CellRendererText();

        TopicColumn.Title = "Topic";
        TopicColumn.MinWidth = 80;
        TopicColumn.Resizable = true;
        TopicColumn.PackStart(TopicCell, true);

        NameColumn.Title = "Name";
        NameColumn.MinWidth = 80;
        NameColumn.Resizable = true;
        NameColumn.PackStart(NameCell, true);

        TypeColumn.Title = "Datatype";
        TypeColumn.MinWidth = 80;
        TypeColumn.Resizable = true;
        TypeColumn.PackStart(TypeCell, true);

        ErrorColumn.Title = "Error";
        ErrorColumn.MinWidth = 60;
        ErrorColumn.Resizable = true;
        ErrorColumn.PackStart(ErrorCell, true);

        MinColumn.Title = "Min Value";
        MinColumn.MinWidth = 80;
        MinColumn.Resizable = true;
        MinColumn.PackStart(MinCell, true);

        MaxColumn.Title = "Max Value";
        MaxColumn.MinWidth = 80;
        MaxColumn.Resizable = true;
        MaxColumn.PackStart(MaxCell, true);

        TopicColumn.AddAttribute(TopicCell, "text", 0);
        NameColumn.AddAttribute(NameCell, "text", 1);
        TypeColumn.AddAttribute(TypeCell, "text", 2);
        ErrorColumn.AddAttribute(ErrorCell, "text", 3);
        MinColumn.AddAttribute(MinCell, "text", 4);
        MaxColumn.AddAttribute(MaxCell, "text", 5);

        MainTree.AppendColumn(TopicColumn);
        MainTree.AppendColumn(NameColumn);
        MainTree.AppendColumn(TypeColumn);
        MainTree.AppendColumn(ErrorColumn);
        MainTree.AppendColumn(MinColumn);
        MainTree.AppendColumn(MaxColumn);

        MainTree.Model = TheTable;
        return MainTree;
    }

    private void UpdatePP3EntryTable(List<PP3Entry> EntryList, ref ListStore TheTable)
    {
        TheTable.Clear();

        foreach (PP3Entry entry in EntryList)
        {
            string min, max;
            if (entry.MinMaxSuccesful)
            {
                if (entry.Type == PP3DataType.Double)
                {
                    min = ((double)entry.MinValue).ToString("F", culture);
                    max = ((double)entry.MaxValue).ToString("F", culture);
                }
                else if (entry.Type == PP3DataType.Int)
                {
                    min = ((int)entry.MinValue).ToString();
                    max = ((int)entry.MaxValue).ToString();
                }
                else min = max = "None";
            }
            else min = max = "None";

            TheTable.AppendValues(entry.Topic, entry.Name, entry.Type.ToString(), (!entry.MinMaxSuccesful).ToString(), min, max);
        }
    }

    #endregion

	/*
        /// <summary>
        /// Changes the Mono.Unix.Getstring from Gtk# to my own Msg.GetString
        /// </summary>
        private static void ChangeToMsg()
        {
            string basep = @"C:\Users\Johannes\Documents\Diverses\Programmieren\ImageEditing\LapseStudio\LapseStudioGUI\gtk-gui\";
            string[] paths = new string[] { basep +"LapseStudioSub.MyAboutDialog.cs", basep + "LapseStudioSub.MyHelpDialog.cs", 
            basep + "LapseStudioSub.MySettingsDialog.cs", basep + "MainWindow.cs", basep + "LapseStudioGUI.Graph.cs" };
            foreach (string p in paths)
            {
                string t = File.ReadAllText(p);
                if (!t.StartsWith("using LapseStudioSub;")) { t = t.Insert(0, "using LapseStudioSub;" + Environment.NewLine); }                
                t = t.Replace("global::Mono.Unix.Catalog.GetString", "Msg.GetString");
                File.WriteAllText(p, t);
            }
        }

        /// <summary>
        /// Code to read XMP
        /// </summary>
        private static void XMPread()
        {
            CultureInfo culture = new CultureInfo("en-US");
            string version = "2012";

            string[] lines_nor = File.ReadAllLines(@"C:\Users\Johannes\Desktop\" + version + "_nor.xmp");
            string[] lines_min = File.ReadAllLines(@"C:\Users\Johannes\Desktop\" + version + "_min.xmp");
            string[] lines_max = File.ReadAllLines(@"C:\Users\Johannes\Desktop\" + version + "_max.xmp");

            using (StreamWriter writer = new StreamWriter(@"C:\Users\Johannes\Desktop\xmp" + version + "_read.cs", false))
            {
                string nl = Environment.NewLine;
                int i = 0;

                while (i < lines_nor.Length)
                {
                    if (!String.IsNullOrWhiteSpace(lines_nor[i]))
                    {
                        if (lines_nor[i].Contains(':') && lines_nor[i].Contains("crs"))
                        {
                            lines_nor[i] = lines_nor[i].Trim();
                            lines_min[i] = lines_min[i].Trim();
                            lines_max[i] = lines_max[i].Trim();
                            string tag = lines_nor[i].Substring(1, 3).Trim();

                            if (tag == "crs")
                            {
                                try
                                {
                                    string name = lines_nor[i].Substring(lines_nor[i].IndexOf(':') + 1).Trim();
                                    name = name.Substring(0, name.IndexOf('>'));
                                    string value_nor = lines_nor[i].Substring(lines_nor[i].IndexOf(">") + 1);
                                    value_nor = value_nor.Substring(0, value_nor.IndexOf("<"));

                                    string value_min = lines_min[i].Substring(lines_min[i].IndexOf(">") + 1);
                                    value_min = value_min.Substring(0, value_min.IndexOf("<"));

                                    string value_max = lines_max[i].Substring(lines_max[i].IndexOf(">") + 1);
                                    value_max = value_max.Substring(0, value_max.IndexOf("<"));

                                    if (name.StartsWith("Exposure")) { writer.WriteLine("else if (name ==\"" + name + "\") { Exposure = Convert.ToDouble(value, culture); }"); }
                                    else if (name.StartsWith("ProcessVersion")) { writer.WriteLine("else if (name ==\"" + name + "\") { FileVersion = value; }"); }
                                    else if (value_nor.Contains('.'))
                                    {
                                        try
                                        {
                                            bool sign = (value_nor.Contains('-')) ? true : (value_nor.Contains('+')) ? true : false;
                                            double min = Convert.ToDouble(value_min, culture);
                                            double max = Convert.ToDouble(value_max, culture);
                                            writer.WriteLine("else if (name ==\"" + name + "\") { Values.Add(name, new XMPentry(name, Convert.ToDouble(value, culture), typeof(double), \"" + sign.ToString().ToLower() + "\", " + min.ToString(culture) + ", " + max.ToString(culture) + ")); }");
                                        }
                                        catch
                                        {
                                            writer.WriteLine("else if (name ==\"" + name + "\") { Values.Add(name, new XMPentry(name, value, typeof(string), false, String.Empty, String.Empty)); }");
                                        }
                                    }
                                    else if (value_nor.Contains("True") || value_nor.Contains("False"))
                                    {
                                        writer.WriteLine("else if (name ==\"" + name + "\") { Values.Add(name, new XMPentry(name, Convert.ToBoolean(value), typeof(bool), false, false, true)); }");
                                    }
                                    else
                                    {
                                        if (name == "LensProfileDigest" || name == "CameraProfileDigest")
                                        {
                                            writer.WriteLine("else if (name ==\"" + name + "\") { Values.Add(name, new XMPentry(name, value, typeof(string), false, String.Empty, String.Empty)); }");
                                        }
                                        else
                                        {
                                            try
                                            {
                                                bool sign = (value_nor.Contains('-')) ? true : (value_nor.Contains('+')) ? true : false;
                                                double min = Convert.ToInt32(value_min);
                                                double max = Convert.ToInt32(value_max);
                                                writer.WriteLine("else if (name ==\"" + name + "\") { Values.Add(name, new XMPentry(name, Convert.ToInt32(value), typeof(int), \"" + sign.ToString().ToLower() + "\", " + min + ", " + max + ")); }");
                                            }
                                            catch
                                            {
                                                writer.WriteLine("else if (name ==\"" + name + "\") { Values.Add(name, new XMPentry(name, value, typeof(string), false, String.Empty, String.Empty)); }");
                                            }
                                        }
                                    }
                                }
                                catch { }
                            }
                        }
                    }
                    i++;
                }
            }
        }
	 */
}
