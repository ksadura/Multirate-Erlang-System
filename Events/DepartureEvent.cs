using MultirateErlang.LossSystem;

namespace MultirateErlang.Events
{
    public class DepartureEvent : IEvent
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
            get; private set;
        }

        public TrafficClass TrafficClass
        {
            get; set;
        }

        public event EventHandler? Handling;

        public void Handle()
        {
            Handling?.Invoke(this);
        }

        public DepartureEvent() => Type = nameof(DepartureEvent);
    }
}
