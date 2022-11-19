namespace MultirateErlang.LossSystem
{
    public class TrafficClass
    {
        public double Mu
        {
            get; set;
        }

        public double Lambda
        {
            get; set;
        }

        public int Size
        {
            get; set;
        }

        public int Id
        {
            get; set;
        }

        public TrafficClass(int id, double lambda, double mu, int size)
        {
            Id = id;
            Lambda = lambda;
            Mu = mu;
            Size = size;
        }
    }
}
