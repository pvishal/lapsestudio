using Timelapse_UI;
using System.Windows.Forms;

namespace LapseStudioWinFormsUI
{
	public static class WinFormHelper
	{
        public static MessageBoxIcon GetWinType(MessageWindowType typ)
        {
            switch (typ)
            {
                case MessageWindowType.Error:
                    return MessageBoxIcon.Error;

                case MessageWindowType.Question:
                    return MessageBoxIcon.Question;

                case MessageWindowType.Warning:
                    return MessageBoxIcon.Warning;

                case MessageWindowType.Info:
                    return MessageBoxIcon.Information;
                    
                case MessageWindowType.Other:
                default:
                    return MessageBoxIcon.None;
            }
        }

		public static WindowResponse GetResponse(DialogResult resp)
		{
			switch(resp)
			{
				case DialogResult.Abort:
					return WindowResponse.Cancel;

				case DialogResult.Ignore:
					return WindowResponse.Ignore;

                case DialogResult.Cancel:
                    return WindowResponse.Cancel;

                case DialogResult.No:
                    return WindowResponse.No;

                case DialogResult.OK:
                    return WindowResponse.Ok;

                case DialogResult.Retry:
                    return WindowResponse.Retry;

                case DialogResult.Yes:
                    return WindowResponse.Yes;

                case DialogResult.None:
				default:
					return WindowResponse.None;
			}
		}

        public static MessageBoxButtons GetButtons(MessageWindowButtons buttons)
		{
			switch(buttons)
			{
				case MessageWindowButtons.Cancel:
                    return MessageBoxButtons.OKCancel;

				case MessageWindowButtons.OkCancel:
                    return MessageBoxButtons.OKCancel;

				case MessageWindowButtons.YesNo:
                    return MessageBoxButtons.YesNo;

                case MessageWindowButtons.YesNoCancel:
                    return MessageBoxButtons.YesNoCancel;

                case MessageWindowButtons.RetryCancel:
                    return MessageBoxButtons.RetryCancel;

                case MessageWindowButtons.AbortRetryIgnore:
                    return MessageBoxButtons.AbortRetryIgnore;

                case MessageWindowButtons.Close:
                case MessageWindowButtons.None:
				case MessageWindowButtons.Ok:
				default:
                    return MessageBoxButtons.OK;
			}
		}
	}
}

