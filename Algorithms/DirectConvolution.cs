using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            OutputConvolvedSignal = new Signal(new List<float>(), InputSignal1.Periodic);
            float product = 0;
            float sum = 0;
            int counter = 0;
            int n;

            for (n = 0; ; n++)
            {
                for (int k = 0; k <= n; k++)
                {
                    if ((n - k) >= InputSignal2.Samples.Count || k >= InputSignal1.Samples.Count)
                    {
                        counter++;
                        continue;
                    }
                    product = InputSignal1.Samples[k] * InputSignal2.Samples[n - k];
                    sum += product;
                    counter = 0;
                }
                if (counter == n + 1)
                    break;
                counter = 0;
                OutputConvolvedSignal.Samples.Add(sum);
                sum = 0;
            }

            int conv_out_size = OutputConvolvedSignal.Samples.Count;
            //if last value is zero remove it.
            for (int i = conv_out_size - 1; ; i--)
            {
                if (OutputConvolvedSignal.Samples[i] == 0)
                    OutputConvolvedSignal.Samples.RemoveAt(i);
                else
                    break;
            }

            OutputConvolvedSignal.SamplesIndices = new List<int>();
            //get first index from 2 indices of h and x...ex: -1+-2 =-3 from test 5.
            int val = InputSignal1.SamplesIndices[0] + InputSignal2.SamplesIndices[0];
            for (int i = 0; i < OutputConvolvedSignal.Samples.Count; i++)
            {
                OutputConvolvedSignal.SamplesIndices.Add(val);
                val++;
            }
            //   var expectedOutput = new Signal(new List<float>() { -3, 2, 2, 3, 2 }, new List<int>() { -3, -2, -1, 0, 1 }, false);
        }
    }
}
