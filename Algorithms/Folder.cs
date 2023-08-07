using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Folder : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal OutputFoldedSignal { get; set; }
        public static int folding_counter = 0;
        public override void Run()
        {
            int size = InputSignal.Samples.Count;
            OutputFoldedSignal = new Signal(new List<float>(), InputSignal.Periodic);
            OutputFoldedSignal.SamplesIndices = new List<int>();
            for (int i = 0; i < size; i++)
            {
                OutputFoldedSignal.Samples.Add(InputSignal.Samples[i]);
                OutputFoldedSignal.SamplesIndices.Add(-(InputSignal.SamplesIndices[i]));
            }
            OutputFoldedSignal.SamplesIndices.Reverse();
            OutputFoldedSignal.Samples.Reverse();
            folding_counter++;
        }

    }
}
