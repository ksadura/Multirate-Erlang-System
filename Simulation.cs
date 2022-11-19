using MultirateErlang.Events;
using MultirateErlang.LossSystem;
using MultirateErlang.Utils;

namespace MultirateErlang
{
    public class Simulation
    {
        private EventsList<IEvent> eventsList;
        private ExponentialDistribution expDistribution;
        private TrafficClass[] trunks;

        private double time = 0.00;
        private Erlang erlangSystem;

        public Simulation(TrafficClass[] trunks, int seed, int capacity)
        {
            expDistribution = new ExponentialDistribution(seed);
            eventsList = new EventsList<IEvent>();
            erlangSystem = new Erlang(capacity, trunks.Length);

            this.trunks = trunks;
            InitiateEventsList();
        }

        public void InitiateEventsList()
        {
            foreach (var trunk in trunks)
            {
                var arrivalEvent = new ArrivalEvent()
                {
                    Time = 0 + expDistribution.GetArrivalTime(trunk.Lambda),
                    Id = eventsList.Counter + 1,
                    TrafficClass = trunk,
                };
                arrivalEvent.Handling += HandleArrival;

                eventsList.Put(arrivalEvent);
            }
        }

        // Start simulation
        public void Start(int maxTime)
        {
            while (time <= maxTime)
            {
                var _event = eventsList.Get();
                time = _event.Time;
                _event.Handle();
            }
        }

        // Handling arrival event
        private void HandleArrival(IEvent e)
        {
            bool isAccepted = erlangSystem.AcceptClient(new Client()
            {
                TrafficClass = e.TrafficClass,
            });

            var arrivalEvent = new ArrivalEvent()
            {
                Time = time + expDistribution.GetArrivalTime(e.TrafficClass.Lambda),
                Id = eventsList.Counter + 1,
                TrafficClass = e.TrafficClass,
            };
            arrivalEvent.Handling += HandleArrival;

            eventsList.Put(arrivalEvent);

            if (!isAccepted)
            {
                return;
            }

            var departureEvent = new DepartureEvent()
            {
                Time = time + expDistribution.GetServiceTime(mu: e.TrafficClass.Mu, uniform: true),
                Id = e.Id,
                TrafficClass = e.TrafficClass,
            };
            departureEvent.Handling += HandleDeparture;

            eventsList.Put(departureEvent);
        }

        // Handling departure event
        private void HandleDeparture(IEvent e)
        {
            erlangSystem.Release(e.TrafficClass);
        }

        // Output results
        public void PrintResult()
        {
            erlangSystem.PrintResults();
        }
    }
}
