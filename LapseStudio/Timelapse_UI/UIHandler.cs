using System;
using System.Collections;

namespace Timelapse_UI
{
    public interface IUIHandler
    {
        void ReleaseUIData();
        void InvokeUI(Action action);
        void QuitApplication();
        void InitOpenedProject();
        void SetProgress(int percent);
        void ResetPictureBoxes();
        void InitTable();
        void InitUI();
        void SelectTableRow(int index);
        void RefreshImages();
        void SetTableRow(int index, ArrayList values, bool fill);
        void SetStatusLabel(string text);
        void SetWindowTitle(string text);
        void InitAfterFrameLoad();
    }
}
