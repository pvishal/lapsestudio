using System;
using Timelapse_API;
using System.IO;

namespace Timelapse_UI
{
	public static class LSSettings
	{
        private static string SettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "User.stg");

        public static string LastProjDir { get; set; }
        public static string LastImgDir { get; set; }
        public static string LastProcDir { get; set; }
        public static Language UsedLanguage { get; set; }
        public static ProjectType UsedProgram { get; set; }
        public static BrightnessCalcType BrCalcType { get; set; }
        public static int Threadcount { get; set; }
        public static bool Autothread { get; set; }
        public static bool RunRT { get; set; }
        public static FileFormat SaveFormat { get; set; }
        public static int JpgQuality { get; set; }
        public static TiffCompressionFormat TiffCompression { get; set; }
        public static ImageBitDepth BitDepth { get; set; }
        public static string LastMetaDir { get; set; }
        public static string RTPath { get; set; }
        public static bool KeepPP3 { get; set; }

        //if a setting isn't in use anymore, still let the read read/write its value to get backwards support
        //Important: keep order of all values
        //if a new setting is added, also add it to: Reset(), Read() and Save()

        public static void Init()
        {
            Reset();
            if (!File.Exists(SettingsPath)) { Recreate(); return; }
            Read();
        }
        
        private static void Recreate()
        {
            File.Create(SettingsPath).Close();
            Reset();
            Save();
        }


        private static void Reset()
        {
            LastProjDir = String.Empty;
            LastImgDir = String.Empty;
            LastProcDir = String.Empty;
            UsedLanguage = Language.English;
            UsedProgram = ProjectType.LapseStudio;
            BrCalcType = BrightnessCalcType.Advanced;
            Threadcount = 1;
            Autothread = false;
            RunRT = true;
            SaveFormat = FileFormat.tiff;
            JpgQuality = 100;
            TiffCompression = TiffCompressionFormat.LZW;
            BitDepth = ImageBitDepth.bit16;
            LastMetaDir = String.Empty;
            RTPath = String.Empty;
            KeepPP3 = false;
        }

        public static void Read()
        {
            try
            {
                using (FileStream stream = new FileStream(SettingsPath, FileMode.Open))
                {
                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        try { LastProjDir = reader.ReadString(); }
                        catch (EndOfStreamException) { return; }
                        try { LastProcDir = reader.ReadString(); }
                        catch (EndOfStreamException) { return; }
                        try { LastImgDir = reader.ReadString(); }
                        catch (EndOfStreamException) { return; }
                        try { UsedLanguage = (Language)reader.ReadInt32(); }
                        catch (EndOfStreamException) { return; }
                        try { UsedProgram = (ProjectType)reader.ReadInt32(); }
                        catch (EndOfStreamException) { return; }
                        try { BrCalcType = (BrightnessCalcType)reader.ReadInt32(); }
                        catch (EndOfStreamException) { return; }
                        try { Threadcount = reader.ReadInt32(); }
                        catch (EndOfStreamException) { return; }
                        try { Autothread = reader.ReadBoolean(); }
                        catch (EndOfStreamException) { return; }
                        try { RunRT = reader.ReadBoolean(); }
                        catch (EndOfStreamException) { return; }
                        try { SaveFormat = (FileFormat)reader.ReadInt32(); }
                        catch (EndOfStreamException) { return; }
                        try { JpgQuality = reader.ReadInt32(); }
                        catch (EndOfStreamException) { return; }
                        try { TiffCompression = (TiffCompressionFormat)reader.ReadInt32(); }
                        catch (EndOfStreamException) { return; }
                        try { BitDepth = (ImageBitDepth)reader.ReadInt32(); }
                        catch (EndOfStreamException) { return; }
                        try { LastMetaDir = reader.ReadString(); }
                        catch (EndOfStreamException) { return; }
                        try { RTPath = reader.ReadString(); }
                        catch (EndOfStreamException) { return; }
                        try { KeepPP3 = reader.ReadBoolean(); }
                        catch (EndOfStreamException) { return; }
                    }
                }
            }
            catch (FileNotFoundException) { Recreate(); }
        }

        public static void Save()
        {
            try
            {
                using (FileStream stream = new FileStream(SettingsPath, FileMode.Create))
                {
                    using (BinaryWriter writer = new BinaryWriter(stream))
                    {
                        writer.Write(LastProjDir);
                        writer.Write(LastProcDir);
                        writer.Write(LastImgDir);
                        writer.Write((int)UsedLanguage);
                        writer.Write((int)UsedProgram);
                        writer.Write((int)BrCalcType);
                        writer.Write(Threadcount);
                        writer.Write(Autothread);
                        writer.Write(RunRT);
                        writer.Write((int)SaveFormat);
                        writer.Write(JpgQuality);
                        writer.Write((int)TiffCompression);
                        writer.Write((int)BitDepth);
                        writer.Write(LastMetaDir);
                        writer.Write(RTPath);
                        writer.Write(KeepPP3);
                    }
                }
            }
            catch (FileNotFoundException) { Recreate(); }
        }
	}
}