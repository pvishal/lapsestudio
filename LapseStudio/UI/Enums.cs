
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
        KeyframecountLow,
        NotEnoughFrames,
        ProjectSaved,
        NotEnoughValidFiles,

        //Static:
        RemoveMetadataLink,
        UseReadXMP,
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
}