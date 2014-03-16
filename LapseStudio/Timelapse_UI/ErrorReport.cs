using System;
using System.Text;
using System.IO;
using MessageTranslation;
using System.Reflection;

namespace Timelapse_UI
{
	public static class Error
	{
        private static MessageBox MsgBox;
		private static string Filepath;

		public static void Init(MessageBox MsgBox)
        {
            Error.MsgBox = MsgBox;
			Filepath = Path.Combine(Timelapse_API.ProjectManager.ApplicationPath, "ErrorLog.txt");
        }

		public static void Report(string Sender, Exception exception)
	    {
	        try
	        {
	            string Text = string.Empty;
				if (File.Exists(Filepath)) { Text = File.ReadAllText(Filepath) + Environment.NewLine; }

				using (StreamWriter writer = new StreamWriter(Filepath, false))
                {
                    while (ASCIIEncoding.Unicode.GetByteCount(Text) > 80000) { Text = Text.Substring(Text.IndexOf('#', Text.Length / 2)); }

                    var info = Assembly.GetEntryAssembly();

                    writer.Write(Text);
                    writer.WriteLine("#" + DateTime.Now);
                    writer.WriteLine("System Information:");
                    writer.WriteLine("  " + info.FullName);
                    writer.WriteLine("  OS:   " + Environment.OSVersion);
                    writer.WriteLine("  Bit:  " + ((Environment.Is64BitOperatingSystem) ? "64 bit" : "32 bit"));
                    writer.WriteLine("  CLI:  " + Environment.Version);
                    writer.WriteLine("Sender: " + Sender);
                    writer.WriteLine("Message:" + Environment.NewLine + "  " + exception.Message);
                    writer.WriteLine("Stacktrace:" + Environment.NewLine + exception.StackTrace);
                }

                if (MsgBox != null) MsgBox.Show(Sender + ":" + Environment.NewLine + Message.GetString(exception.Message), MessageWindowType.Error);
	        }
	        catch (Exception ex)
	        {
                if (MsgBox != null) MsgBox.Show("Error while Error! Please show this to the developer:" + Environment.NewLine + "Original exception: " + exception.Message + Environment.NewLine + "New exception: " + ex.Message, MessageWindowType.Error);
	        }
	    }
	}
}
