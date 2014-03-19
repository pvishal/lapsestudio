
namespace Timelapse_UI
{
	public enum ClosingReason
	{
		Error,
		Program,
		User,
	}

	public enum WindowResponse
	{
		None,
		Accept,
		DeleteEvent,
		Ok,
		Cancel,
		Close,
		Yes,
		No,
		Apply,
		Help,
        Retry,
		Ignore,
	}

	public enum FileDialogType
	{
		OpenFile,
		SaveFile,
		SelectFolder,
	}

	public enum MessageWindowType
	{
		Error,
		Info,
		Other,
		Question,
		Warning,
	}

	public enum MessageWindowButtons
	{
		None,
		Ok,
		Close,
		Cancel,
		YesNo,
		OkCancel,
        AbortRetryIgnore,
        YesNoCancel,
        RetryCancel,
	}

	public enum MessageContent
	{
		SaveQuestion,
		IsBusy,
        FramesAlreadyAdded,
        BusyClose,
        BrightnessNotCalculatedWarning,
        BrightnessNotCalculatedError,
        BrightnessAlreadyCalculated,
        KeyframecountLow,
        NotEnoughFrames,
        ProjectSaved,
        NotEnoughValidFiles,
        RemoveMetadataLink,
        UseReadXMP,
        KeyframeAdded,
        KeyframeNotAdded,
	}

    public enum TableLocation
    {
        Nr,
        Keyframe,
        Filename,
        Brightness,
        AV,
        TV,
        ISO,
    }

    public enum TabLocation
    {
        Filelist,
        Calculation,
        Graph,
    }
	
    public enum TiffCompressionFormat
    {
        None,
        LZW,
    }
    
    public enum ImageBitDepth
    {
        bit8,
        bit16,
    }
	
    public enum Language
    {
        English,
        German,
    }

    /// <summary>
    /// For newer RT versions (not implemented in LS yet)
    /// </summary>
    public enum JpgCompression
    {
        Compression4x1x1 = 1,
        Normal4x2x2 = 2,
        Quality4x4x4 = 3,
    }
}