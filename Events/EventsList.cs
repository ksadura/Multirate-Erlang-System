
namespace MultirateErlang.Events
{
    public class EventsList<T> : List<T> where T : IEvent
    {
        public int Counter = 0;

        // Add event to list
        public void Put(T _event)
        {
            Add(_event);
            Counter += 1;
        }

        // Get event from list
        public T Get()
        {
            Sort((x, y) => x.Time.CompareTo(y.Time));
            T _event = this[0];
            RemoveAt(0);

            return _event;
        }
    }
}
