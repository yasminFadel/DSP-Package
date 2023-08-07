using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class DirectCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }

        public override void Run()
        {
            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();

            float sum = 0;
            int size = 0;
            float norm = 0;
            float sumPow1 = 0, sumPow2 = 0;

            if (InputSignal2 == null) // auto correlation
            {
                InputSignal2 = new Signal(new List<float>(), InputSignal1.Periodic);
                InputSignal2.Samples.AddRange(InputSignal1.Samples);
            }
            else
            {
                //check which is bigger and fill the smaller with zeros.
                bool sig2isbigger = false;
                while(InputSignal2.Samples.Count > InputSignal1.Samples.Count)
                {
                    InputSignal1.Samples.Add(0);
                    sig2isbigger = true;
                }
                if(!sig2isbigger)
                {
                   
                    while (InputSignal1.Samples.Count > InputSignal2.Samples.Count)
                    {
                        InputSignal2.Samples.Add(0);
                    }
                }
            }
            // the 2 signals are now the same size.
            size = InputSignal1.Samples.Count;
            //normalization term
            for (int i = 0; i < size; i++)
            {
                sumPow1 += (float)Math.Pow(InputSignal1.Samples[i], 2);
                sumPow2 += (float)Math.Pow(InputSignal2.Samples[i], 2);
            }
            norm = (float)Math.Sqrt(sumPow1 * sumPow2) / size;

            //correlation loop.
            for (int n = 0; n < size; n++)
            {
                // term of each correlation aka harmonicaya.
                for (int i = 0; i < size; i++)
                {
                    float product = InputSignal1.Samples[i] * InputSignal2.Samples[i];
                    sum += product;
                }
                //shift for next iteration.
                // in case of periodic circular shift...aka newlast = first.
                float newlast = InputSignal2.Samples[0];
                for (int j = 0; j < size - 1; j++)
                {
                    InputSignal2.Samples[j] = InputSignal2.Samples[j + 1];
                }
                // in case of not periodic add zeros in the end....aka newlast = 0.
                if (InputSignal2.Periodic == false)
                    newlast = 0;

               //add new last element
               InputSignal2.Samples[size - 1] = newlast;

                //Add correlation.
                float nonnorm = sum / size;
                OutputNonNormalizedCorrelation.Add(nonnorm);
                OutputNormalizedCorrelation.Add(nonnorm / norm);
                sum = 0;
            }
        }
    }
}