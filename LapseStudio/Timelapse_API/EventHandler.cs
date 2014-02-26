using System;

namespace Timelapse_API
{
    internal delegate void ValueChangeHandler(object sender, ValueChangeEventArgs e);
    public delegate void ProgressChangeHandler(object sender, ProgressChangeEventArgs e);
    public delegate void WorkFinishedHandler(object sender, WorkFinishedEventArgs e);
    
    internal class ValueChangeEventArgs : EventArgs
    {
        public Type PropertyType { get; private set; }
        public object OldValue { get; private set; }
        public object NewValue { get; private set; }

        public ValueChangeEventArgs(Type PropertyType, object oldValue, object newValue)
        {
            this.PropertyType = PropertyType;
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }
    }
    public class ProgressChangeEventArgs : EventArgs
    {
        public int ProgressPercentage { get; private set; }
        public ProgressType Topic { get; private set; }
        public object UserState { get; private set; }

        public ProgressChangeEventArgs(int ProgressPercentage)
        {
            this.ProgressPercentage = ProgressPercentage;
            this.Topic = ProgressType.Unknown;
        }

        public ProgressChangeEventArgs(int ProgressPercentage, ProgressType Topic)
        {
            this.ProgressPercentage = ProgressPercentage;
            this.Topic = Topic;
        }

        public ProgressChangeEventArgs(int ProgressPercentage, object UserState)
        {
            this.ProgressPercentage = ProgressPercentage;
            this.Topic = ProgressType.Unknown;
            this.UserState = UserState;
        }

        public ProgressChangeEventArgs(int ProgressPercentage, ProgressType Topic, object UserState)
        {
            this.ProgressPercentage = ProgressPercentage;
            this.Topic = Topic;
            this.UserState = UserState;
        }
    }
    public class WorkFinishedEventArgs : EventArgs
    {
        public Work Topic { get; private set; }
        public bool Cancelled { get; private set; }
        
        public WorkFinishedEventArgs(bool Cancelled)
        {
            this.Cancelled = Cancelled;
            this.Topic = Work.Unknown;
        }

        public WorkFinishedEventArgs(bool Cancelled, Work Topic)
        {
            this.Cancelled = Cancelled;
            this.Topic = Topic;
        }
    }
}
