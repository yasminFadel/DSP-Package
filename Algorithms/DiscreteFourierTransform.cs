using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.IO;
using System.Numerics;

namespace DSPAlgorithms.Algorithms
{
    public class DiscreteFourierTransform : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; }
        public float InputSamplingFrequency { get; set; }
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            Complex temp = new Complex(0,0);
            OutputFreqDomainSignal = new Signal(new List<float>(), InputTimeDomainSignal.Periodic);
            OutputFreqDomainSignal.FrequenciesAmplitudes = new List<float>();
            OutputFreqDomainSignal.FrequenciesPhaseShifts = new List<float>();
            OutputFreqDomainSignal.Frequencies = new List<float>();

            double N = InputTimeDomainSignal.Samples.Count;
            for (double k = 0; k < N; k++)
            {
                Complex sum = new Complex(0,0);
                double n = InputTimeDomainSignal.SamplesIndices[(int)k];
                for (double i = 0; i < N; i++)
                {
                    double equation = (i * 2 * Math.PI * k) / N;
                    temp = new Complex(InputTimeDomainSignal.Samples[(int)i]* Math.Cos(equation), InputTimeDomainSignal.Samples[(int)i] *  Math.Sin(-equation));
                    sum += temp;
                    
                }

                OutputFreqDomainSignal.FrequenciesAmplitudes.Add((float)sum.Magnitude);
                OutputFreqDomainSignal.FrequenciesPhaseShifts.Add((float)sum.Phase);
                OutputFreqDomainSignal.Samples.Add((float)sum.Real);

                double val = (float)(2 * Math.PI * InputSamplingFrequency) / N;
                OutputFreqDomainSignal.Frequencies.Add((float)Math.Round(k * val, 1));
            }


        }
    }
}
