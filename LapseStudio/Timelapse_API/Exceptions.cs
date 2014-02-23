using System;

namespace Timelapse_API
{
    /// <summary>
    /// Used when the format of a file is not supported
    /// </summary>
    public class FileFormatNotSupportedException : Exception
    {
        public FileFormatNotSupportedException() { }

        public FileFormatNotSupportedException(string message) : base(message) { }
    }

    /// <summary>
    /// Used when an interpolation of a certain value is not possible
    /// </summary>
    public class InterpolationNotPossibleException : Exception
    {
        public InterpolationNotPossibleException() { }

        public InterpolationNotPossibleException(string message) : base(message) { }
    }

    /// <summary>
    /// Used when a whitepoint is not supported when doing a color conversion of some sort
    /// </summary>
    public class WhitepointNotSupportedException : Exception
    {
        public WhitepointNotSupportedException() { }

        public WhitepointNotSupportedException(string message) : base(message) { }
    }

    /// <summary>
    /// Used when an image is too dark to calculate the brightness effectively
    /// </summary>
    public class ImageTooDarkException : Exception
    {
        public ImageTooDarkException() { }

        public ImageTooDarkException(string message) : base(message) { }
    }

    /// <summary>
    /// Used when not all requirments are fulfilled for a certain operation
    /// </summary>
    public class RequirementsNotFulfilledException : Exception
    {
        public RequirementsNotFulfilledException() { }

        public RequirementsNotFulfilledException(string message) : base(message) { }
    }

    /// <summary>
    /// Used when a file or directory couldn't get deleted
    /// </summary>
    public class FileDeleteException : Exception
    {
        public FileDeleteException() { }

        public FileDeleteException(string message) : base(message) { }
    }

    /// <summary>
    /// Used when a file or directory couldn't get created
    /// </summary>
    public class FileCreateException : Exception
    {
        public FileCreateException() { }

        public FileCreateException(string message) : base(message) { }
    }

    /// <summary>
    /// Used when a file has a wrong version number
    /// </summary>
    public class FileVersionException : Exception
    {
        public FileVersionException() { }

        public FileVersionException(string message) : base(message) { }
    }
    
    /// <summary>
    /// Used when a method of a different project is used
    /// </summary>
    public class ProjectTypeException : Exception
    {
        public ProjectTypeException() { }

        public ProjectTypeException(string message) : base(message) { }
    }

}
