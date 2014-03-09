namespace Timelapse_API
{
    /// <summary>
    /// States projects types for different kinds of programs
    /// </summary>
    public enum ProjectType
    {
		LapseStudio,
		RawTherapee,
        CameraRaw,
    }

    /// <summary>
    /// The platform this project is running on
    /// </summary>
    public enum Platform
    {
        Windows,
        Unix,
        MacOSX,
    }

    /// <summary>
    /// States different kinds of brightness calculations
    /// </summary>
    public enum BrightnessCalcType
    {
        Advanced,
        AdvancedII,
        Simple,
        Exif,
        Lab,
    }

    /// <summary>
    /// States different kinds of works
    /// </summary>
    public enum Work
    {
        Unknown,
        LoadFrames,
        CalculateBrightness,
        ProcessFiles,
        ProcessThumbs,
        ReadXMP,
        LoadProject,
    }

    /// <summary>
    /// States what work made a progress
    /// </summary>
    public enum ProgressType
    {
        Unknown,
        StartingWork,
        //Frame loading
        LoadFrames,
        ExtractThumbnails,
        LoadThumbnails,
        LoadMetadata,
        AnalyseMetadata,
        //Brightness calculation
        CalculateBrightness,
        EvaluateMask,
        StatisticsCheck,
        //Processing
        WritingFiles,
        ProcessingImages,
        ProcessingThumbs,
        //XMP
        ReadXMP,
        WriteXMP,
        //PP3
        WritePP3,
        //Loading
        LoadingProject,
    }

    /// <summary>
    /// Various image formats
    /// </summary>
    public enum FileFormat
    {
        jpg,
        png,
        tiff,
    }
    
    /// <summary>
    /// Bitdepth and channels of image
    /// </summary>
    public enum ImageType
    {
        RGB8,
        RGBA8,
        RGB16,
        RGBA16,
        RGB32,
        RGBA32,
    }
}
