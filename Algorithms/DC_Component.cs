using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class DC_Component: Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }

        public override void Run()
        {
            OutputSignal = new Signal(new List<float>(), InputSignal.Periodic);
            OutputSignal.SamplesIndices.AddRange(InputSignal.SamplesIndices);
            float total = 0, average = 0;
            for(int i = 0; i < InputSignal.Samples.Count; i++)
                total += InputSignal.Samples[i];


            if(total != 0)
                average = total / InputSignal.Samples.Count;

            for(int i = 0; i< InputSignal.Samples.Count; i++)
                OutputSignal.Samples.Add(InputSignal.Samples[i] - average);
        }
    }
}
