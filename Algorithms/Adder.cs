using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Adder : Algorithm
    {
        public List<Signal> InputSignals { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            OutputSignal = new Signal(new List<float>(), InputSignals[0].Periodic);
            int numOfSignals = InputSignals.Count;
            int bigger = 0, smaller = 0;
            for (int i = 0; i < numOfSignals - 1; i+=2)
            {
                int first_length = InputSignals[i].Samples.Count;
                int second_length = InputSignals[i+1].Samples.Count;
                if(first_length >= second_length)
                {
                    OutputSignal.SamplesIndices.AddRange(InputSignals[i].SamplesIndices);
                    bigger = first_length;
                    smaller = second_length;
                }
                else
                {
                    OutputSignal.SamplesIndices.AddRange(InputSignals[i + 1].SamplesIndices);
                    bigger = second_length;
                    smaller = first_length;
                }
                for (int j = 0; j < smaller; j++)
                    OutputSignal.Samples.Add(InputSignals[i].Samples[j] + InputSignals[i+1].Samples[j]);
                for(int j = smaller; j < bigger; j++)
                    OutputSignal.Samples.Add(InputSignals[i].Samples[j]);

            }
        }
    }
}