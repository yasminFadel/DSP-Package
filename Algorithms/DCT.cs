using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DCT : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            OutputSignal = new Signal(new List<float>(), InputSignal.Periodic);
            int N = InputSignal.Samples.Count;
            double summation;
            for (int k = 0; k < N; k++)
            {
                summation = 0;
                for (int n = 0; n < N; n++)
                {
                    summation += InputSignal.Samples[n] * Math.Cos(Math.PI / (4 * N) * (2 * n - 1) * (2 * k - 1));
                }
                OutputSignal.Samples.Add((float)(Math.Sqrt(2 / (double)N) * summation));
            }
        }
    }
}
