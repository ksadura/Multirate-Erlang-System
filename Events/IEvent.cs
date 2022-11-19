using MultirateErlang.LossSystem;

namespace MultirateErlang.Events
{
    public delegate void EventHandler(IEvent e);

    public interface IEvent
    {
        public double Time
        {
            get; set; 
        }

        public int Id
        {
            get; set;
        }

        public string Type
        {
            get;
        }

        public TrafficClass TrafficClass
        {
            get; set;
        }

        public event EventHandler Handling;

        public abstract void Handle();
    }
}
