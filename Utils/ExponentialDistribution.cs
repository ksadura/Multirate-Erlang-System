using MathNet.Numerics.Distributions;

namespace MultirateErlang.Utils
{
    public class ExponentialDistribution
    {
        private readonly int seed;
        private readonly Random random;

        public ExponentialDistribution(int seed) 
        {
            this.seed = seed;
            random = new Random(seed);
        }

        public double GetArrivalTime(double lambda)
        {
            var sample = random.NextDouble();
            return (-1 / lambda) * Math.Log(1-sample);
        }

        public double GetServiceTime(double mu, bool uniform)
        {
            if (uniform)
                return ContinuousUniform.Sample(random, 0, 2 / mu);

            var sample = random.NextDouble();
            return (-1 / mu) * Math.Log(1 - sample);
        }
    }
}
