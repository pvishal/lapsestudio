using System;
using System.Collections.Generic;

namespace Timelapse_API
{
	//TODO: make this more flexible

    [Serializable()]
	public class SavingStorage
    {
        internal int FileVersion = 1;
        internal ProjectType UsedProgram;

        internal List<Frame> Frames;
        internal bool IsBrightnessCalculated;
        internal Rectangle SimpleCalculationArea;
        internal string ImageSavePath;

        public SavingStorage()
        {
            UsedProgram = ProjectManager.UsedProgram;
            Frames = ProjectManager.CurrentProject.Frames;
            IsBrightnessCalculated = ProjectManager.CurrentProject.IsBrightnessCalculated;
            SimpleCalculationArea = ProjectManager.CurrentProject.SimpleCalculationArea;
            ImageSavePath = ProjectManager.ImageSavePath;
        }
    }
}
