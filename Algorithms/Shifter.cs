using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Shifter : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int ShiftingValue { get; set; }
        public Signal OutputShiftedSignal { get; set; }

        public override void Run()
        {
            int count = InputSignal.Samples.Count;
            OutputShiftedSignal = new Signal(new List<float>(), InputSignal.Periodic);
            OutputShiftedSignal.SamplesIndices = new List<int>();
            if(Folder.folding_counter % 2 == 1)
            {
                ShiftingValue = -ShiftingValue;
            }
            for(int i = 0; i < count; i++)
            {
                OutputShiftedSignal.SamplesIndices.Add(InputSignal.SamplesIndices[i] - ShiftingValue);
                OutputShiftedSignal.Samples.Add(InputSignal.Samples[i]);
            }
        }
    }
}
