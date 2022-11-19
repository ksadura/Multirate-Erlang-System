namespace MultirateErlang.LossSystem
{
    public class Erlang
    {
        private List<Server> servers;
        private readonly int MAX = 0;
        
        private Dictionary<int, int> clients;
        private Dictionary<int, int> blockedClients;
        private Dictionary<string, double> states;

        public Erlang(int capacity, int numOfClasses) 
        {
            servers = new List<Server>();
            clients = new Dictionary<int, int>();
            blockedClients = new Dictionary<int, int>();
            states = new Dictionary<string, double>();
            MAX = numOfClasses;

            for (int i = 0; i < capacity; i++)
            {
                servers.Add(new Server()
                {
                    IsBusy = false,
                    TrafficClass = null,
                });
            }
        }

        private bool IsAvailable(int clientSize)
        {
            var freeServers = servers.Where((server) => server.IsBusy == false).ToList();
            return freeServers.Count >= clientSize;
        }

        public int Busy()
        {
            return servers.Where((server) => server.IsBusy == true).ToList().Count;
        }

        public bool AcceptClient(Client incomingClient)
        {
            UpdateStates();
            IncrementClients(incomingClient.TrafficClass.Id);

            if (!IsAvailable(incomingClient.TrafficClass.Size))
            {
                IncrementBlockedClients(incomingClient.TrafficClass.Id);
                return false;
            }

            var occupiedServers = servers.Where(s => s.IsBusy == false).Take(incomingClient.TrafficClass.Size).ToList();
            occupiedServers.ForEach((s) =>
            {
                s.IsBusy = true;
                s.TrafficClass = incomingClient.TrafficClass;
            });
      
            return true;
        }

        public void Release(TrafficClass trafficClass)
        {
            var releasedServers = servers.Where(s => s.TrafficClass?.Id == trafficClass.Id).Take(trafficClass.Size).ToList();
            releasedServers.ForEach((s) =>
            {
                s.IsBusy = false;
                s.TrafficClass = null;
            });
        }

        private void IncrementClients(int id) 
        {
            if (!clients.ContainsKey(id))
            {
                clients.Add(id, 1);
                return;
            }

            clients[id] += 1;
        }

        private void IncrementBlockedClients(int id)
        {
            if (!blockedClients.ContainsKey(id))
            {
                blockedClients.Add(id, 1);
                return;
            }

            blockedClients[id] += 1;
        }

        private void UpdateStates()
        {
            string[] vector = new string[MAX];

            for (int i=0; i<MAX; i++)
            {
                vector[i] = "0";
            }

            var temp = servers.Select(s => s.TrafficClass?.Id).ToList();
            temp.Sort();
            var state = temp.GroupBy(i => i).Where(i => i.Key > 0);
            
            if (state.Any())
            {
                foreach (var s in state)
                    vector[s.Key.Value - 1] = $"{s.Count() / servers.First(x => x.TrafficClass?.Id == s.Key).TrafficClass?.Size}";
            }
            
            string key = string.Join(", ", vector);

            if (!states.ContainsKey(key))
            {
                states.Add(key, 1);
                return;
            }

            states[key]++;
        }

        public void PrintResults()
        {
            Console.WriteLine("".PadRight(40, '-'));
            foreach (var c in clients)
            {
                int key = c.Key;
                double probability = 0.00;

                try
                {
                    probability = (double)blockedClients[key] / c.Value;
                }
                catch {}
                Console.WriteLine($"Blocking probability of class {key}: {probability}");
            }

            Console.Write('\n');

            int allClients = clients.Values.Sum();
            foreach (var state in states)
            {
                Console.WriteLine($"State {'(' + state.Key + ')'} probability: {(double) state.Value / allClients}");
            }
            Console.WriteLine("".PadRight(40, '-'));
        }
    }
}
