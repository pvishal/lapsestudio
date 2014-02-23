using System;
using System.Drawing;
using System.Windows.Forms;

namespace LapseStudioWinFormsUI
{
    public partial class TrackBarEx : UserControl
    {
        public enum ValuePosition
        {
            None,
            Left,
            Right,
            Top,
            Bottom,
            LeftTop,
            LeftBottom,
            RightTop,
            RightBottom,
        }


        public int Value
        {
            get { return _Value; }
            set
            {
                if (value <= MaxValue && value >= MinValue)
                {
                    _Value = value;
                    UpdatePosition();
                }
            }
        }
        public int MinValue
        {
            get { return _MinValue; }
            set { if (value <= MaxValue) _MinValue = value; }
        }
        public int MaxValue
        {
            get { return _MaxValue; }
            set { if (value >= MinValue) _MaxValue = value; }
        }
        public ValuePosition ValueLabel
        {
            get { return _ValueLabel; }
            set
            {
                UpdateSize(value);
                _ValueLabel = value;
            }
        }

        private int _Value;
        private int _MinValue;
        private int _MaxValue;
        private ValuePosition _ValueLabel;
        private Label valLabel;
        private int ls = 15;

        public TrackBarEx()
        {
            InitializeComponent();
            valLabel = new Label();
            valLabel.Dock = DockStyle.Fill;
            valLabel.TextAlign = ContentAlignment.MiddleCenter;
            Value = 0;
            MinValue = 0;
            MaxValue = 100;
            ValueLabel = ValuePosition.None;
        }

        private void UpdatePosition()
        {
            valLabel.Text = Value.ToString();
        }

        private void UpdateSize(ValuePosition NewVal)
        {
            if (ValueLabel != NewVal)
            {
                Control ctrl;
                if (ValueLabel != ValuePosition.None)
                {
                    ctrl = GetControl(ValueLabel);
                    ctrl.Controls.RemoveAt(0);
                    ctrl.Size = new Size(0, 0);
                    if (ValueLabel == ValuePosition.Top || ValueLabel == ValuePosition.Bottom) { this.Height -= ls; this.MinimumSize = new Size(116, this.Height); }
                    else if (ValueLabel == ValuePosition.Left || ValueLabel == ValuePosition.Right) this.Width -= ls;
                    else { this.Width -= ls; this.Height -= ls; this.MinimumSize = new Size(116, this.Height); }
                }
                if (NewVal != ValuePosition.None)
                {
                    ctrl = GetControl(NewVal);
                    ctrl.Size = new Size(ls, ls);
                    ctrl.Controls.Add(valLabel);
                    if (NewVal == ValuePosition.Top || NewVal == ValuePosition.Bottom) { this.Height += ls; this.MinimumSize = new Size(116, this.Height); }
                    else if (NewVal == ValuePosition.Left || NewVal == ValuePosition.Right) this.Width += ls;
                    else { this.Width += ls; this.Height += ls; this.MinimumSize = new Size(116, this.Height); }
                }
                valLabel.Text = Value.ToString();
            }

            int w = 12;
            if (NewVal != ValuePosition.None && NewVal != ValuePosition.Bottom && NewVal != ValuePosition.Top) w += ls;
            ScrollPanel.Width = this.Width - w;
        }

        private Control GetControl(ValuePosition Pos)
        {
            switch (Pos)
            {
                case ValuePosition.Bottom: return MainLayout.GetControlFromPosition(1, 2);
                case ValuePosition.Left: return MainLayout.GetControlFromPosition(0, 1);
                case ValuePosition.LeftBottom: return MainLayout.GetControlFromPosition(0, 2);
                case ValuePosition.LeftTop: return MainLayout.GetControlFromPosition(0, 0);
                case ValuePosition.Right: return MainLayout.GetControlFromPosition(2, 1);
                case ValuePosition.RightBottom: return MainLayout.GetControlFromPosition(2, 2);
                case ValuePosition.RightTop: return MainLayout.GetControlFromPosition(2, 0);
                case ValuePosition.Top: return MainLayout.GetControlFromPosition(1, 0);
                default: return null;
            }
        }

        private void TrackBarEx_Resize(object sender, EventArgs e)
        {
            UpdateSize(ValueLabel);
            UpdatePosition();
        }

        private void MainButton_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void MainButton_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void MainButton_MouseUp(object sender, MouseEventArgs e)
        {

        }
    }
}
