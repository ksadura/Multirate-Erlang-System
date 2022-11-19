using MultirateErlang.LossSystem;

namespace MultirateErlang
{
    public class Program
    {
        private const int Duration = 500000;

        static void Main(string[] args)
        {
            TrafficClass[] classes =
            {
                new TrafficClass(id: 1, lambda: 0.75, mu: 1.0, size: 1),
                new TrafficClass(id: 2, lambda: 0.75, mu: 0.5, size: 2),
            };

            Simulation simulation = new Simulation(classes, seed: 4324314, capacity: 4);
            simulation.Start(Duration);
            simulation.PrintResult();
        }
    }
}