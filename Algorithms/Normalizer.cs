using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class Normalizer : Algorithm
    {
        public Signal InputSignal { get; set; }
        public float InputMinRange { get; set; }
        public float InputMaxRange { get; set; }
        public Signal OutputNormalizedSignal { get; set; }

        public override void Run()
        {
            OutputNormalizedSignal = new Signal(new List<float>(), InputSignal.Periodic);
            OutputNormalizedSignal.SamplesIndices.AddRange(InputSignal.SamplesIndices);
            float min = float.MaxValue;
            float max = float.MinValue;
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                if (InputSignal.Samples[i] < min)
                    min = InputSignal.Samples[i];
                else if (InputSignal.Samples[i] > max)
                    max = InputSignal.Samples[i];
            }
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                float new_val = (InputSignal.Samples[i] - min)/(max - min) * (InputMaxRange - InputMinRange) + InputMinRange;
                OutputNormalizedSignal.Samples.Add(new_val);
            }
        }
    }
}
