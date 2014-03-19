using System;
using MessageTranslation;

namespace Timelapse_UI
{
    public abstract class MessageBox
    {
        public delegate void StringUpdate(string Value);
        public event StringUpdate InfoTextChanged;

        private void SendInfoText(string text)
        {
            if (InfoTextChanged != null) InfoTextChanged(text);
        }

		public abstract WindowResponse Show(object parent, string message, string title, MessageWindowType type, MessageWindowButtons bType);
        
		#region Show

		public WindowResponse Show(string message)
		{
			return this.Show(null, message, "", MessageWindowType.Info, MessageWindowButtons.Ok);
		}

		public WindowResponse Show(string message, MessageWindowType type)
		{
			return this.Show(null, message, GetTitle(type), type, MessageWindowButtons.Ok);
		}

		public WindowResponse Show(string message, string title, MessageWindowType type)
		{
			return this.Show(null, message,  title, type, MessageWindowButtons.Ok);
		}

		public WindowResponse Show(string message, MessageWindowType type, MessageWindowButtons bType)
		{
			return this.Show(null, message,  GetTitle(type), type, bType);
		}

		public WindowResponse Show(string message, string title, MessageWindowType type, MessageWindowButtons bType)
		{
			return this.Show(null, message,  title, type, bType);
		}
                

		public WindowResponse Show(object parent, string message)
		{
			return this.Show(parent, message, "", MessageWindowType.Info, MessageWindowButtons.Ok);
		}

		public WindowResponse Show(object parent, string message, MessageWindowType type)
		{
			return this.Show(parent, message, GetTitle(type), type, MessageWindowButtons.Ok);
		}

		public WindowResponse Show(object parent, string message, string title, MessageWindowType type)
		{
			return this.Show(parent, message,  title, type, MessageWindowButtons.Ok);
		}

		public WindowResponse Show(object parent, string message, MessageWindowType type, MessageWindowButtons bType)
		{
			return this.Show(parent, message,  GetTitle(type), type, bType);
		}
                
		#endregion
        
		#region Messages

		public WindowResponse ShowMessage(MessageContent content)
		{
			return ShowMessage(content, null);
		}

		public WindowResponse ShowMessage(MessageContent content, object args)
		{
			WindowResponse res = WindowResponse.None;

			switch (content)
			{
				case MessageContent.IsBusy:
                    SendInfoText(Message.GetString("LapseStudio is currently busy"));
					break;

				case MessageContent.SaveQuestion:
					res = this.Show(Message.GetString("Do you want to save the current project?"), Message.GetString("Save Project?"), MessageWindowType.Question, MessageWindowButtons.YesNoCancel);
					break;

				case MessageContent.FramesAlreadyAdded:
                    SendInfoText(Message.GetString("Frames already added"));
					break;

				case MessageContent.BusyClose:
					res = this.Show(Message.GetString("LapseStuio is currently busy. Do you want to close anyway?"), Message.GetString("Close LapseStudio?"), MessageWindowType.Question, MessageWindowButtons.YesNo);
					break;

				case MessageContent.BrightnessNotCalculatedError:
					SendInfoText(Message.GetString("Brightness is not calculated yet"));
					break;

				case MessageContent.BrightnessNotCalculatedWarning:
					res = this.Show(Message.GetString("Brightness is not calculated yet. Do you want to proceed anyway?"), Message.GetString("Proceed?"), MessageWindowType.Question, MessageWindowButtons.YesNo);
					break;

				case MessageContent.KeyframecountLow:
					SendInfoText(Message.GetString("Not enough keyframes added"));
					break;

				case MessageContent.NotEnoughFrames:
					SendInfoText(Message.GetString("Not enough frames loaded"));
					break;

				case MessageContent.ProjectSaved:
					SendInfoText(Message.GetString("Project saved"));
					break;

				case MessageContent.RemoveMetadataLink:
					res = this.Show(Message.GetString("Remove the link to the original metadata file?"), Message.GetString("Remove link?"), MessageWindowType.Question, MessageWindowButtons.YesNo);
					break;

				case MessageContent.UseReadXMP:
					res = this.Show(Message.GetString("There is no XMP linked to this file.") + Environment.NewLine + Message.GetString("Do you want to try to exctract it from the file?"), Message.GetString("Search for XMP?"), MessageWindowType.Question, MessageWindowButtons.YesNo);
					break;

                case MessageContent.NotEnoughValidFiles:
                    this.Show(Message.GetString("Not enough valid images found for this kind of project."), MessageWindowType.Warning);
                    break;

                case MessageContent.BrightnessAlreadyCalculated:
                    res = this.Show(Message.GetString("Brightness is already calculated. Do you want to calculate again?"), Message.GetString("Calculate Again?"), MessageWindowType.Question, MessageWindowButtons.YesNo);
                    break;

                case MessageContent.KeyframeAdded:
                    SendInfoText(Message.GetString("Keyframe set!"));
                    break;

                case MessageContent.KeyframeNotAdded:
                    SendInfoText(Message.GetString("Keyframe not set!"));
                    break;

				default:
					res = this.Show(Message.GetString("Something happened!") + Environment.NewLine + Message.GetString("(There is not more information available)"), MessageWindowType.Warning, MessageWindowButtons.Ok);
					break;
			}

			return res;
		}

		#endregion

		private string GetTitle(MessageWindowType type)
		{
			string title = "";
			switch (type)
			{
				case MessageWindowType.Error:
					title = "Error";
					break;

				case MessageWindowType.Info:
					title = "Info";
					break;

				case MessageWindowType.Other:
					title = "Message";
					break;

				case MessageWindowType.Question:
					title = "Question";
					break;

				case MessageWindowType.Warning:
					title = "Warning";
					break;
			}
			return title;
		}
    }
}
