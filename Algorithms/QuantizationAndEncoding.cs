using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;



namespace DSPAlgorithms.Algorithms
{
    public class QuantizationAndEncoding : Algorithm
    {
        // You will have only one of (InputLevel or InputNumBits), the other property will take a negative value
        // If InputNumBits is given, you need to calculate and set InputLevel value and vice versa
        public int InputLevel { get; set; }
        public int InputNumBits { get; set; }
        public Signal InputSignal { get; set; }
        public Signal OutputQuantizedSignal { get; set; }
        public List<int> OutputIntervalIndices { get; set; }
        public List<string> OutputEncodedSignal { get; set; }
        public List<float> OutputSamplesError { get; set; }



        public override void Run()
        {
            OutputQuantizedSignal = new Signal(new List<float>(), true);
            OutputIntervalIndices = new List<int>();
            OutputEncodedSignal = new List<string>();
            OutputSamplesError = new List<float>();
            if (InputLevel <= 0)
                InputLevel = (int)Math.Pow(2, InputNumBits);
            if (InputNumBits <= 0)
                InputNumBits = (int)Math.Log(InputLevel, 2);
            float max = InputSignal.Samples.Max();
            float min = InputSignal.Samples.Min();
            float delta = (max - min) / InputLevel;
            List<float> intervals = new List<float>();
            intervals.Add(min);
            for (int i = 0; i < InputLevel; i++)
            {
                //end of last interval is the start of next one.
                intervals.Add(intervals[i] + delta);
            }
            int level = 0;
            for (int i = 0; i < InputSignal.Samples.Count; i++)
            {
                for (int j = 0; j < InputLevel; j++)
                {
                    level++;
                    if (InputSignal.Samples[i] <= intervals[j + 1])
                    {
                        float midpoint = (intervals[j] + intervals[j + 1]) / 2;
                        OutputQuantizedSignal.Samples.Add(midpoint);
                        OutputIntervalIndices.Add(level);
                        OutputSamplesError.Add(midpoint - InputSignal.Samples[i]);
                        OutputEncodedSignal.Add(Convert.ToString(level - 1, 2).PadLeft(InputNumBits, '0'));
                        break;
                    }
                    else if (InputSignal.Samples[i] == max)
                    {
                        float midpoint = (max - delta + max) / 2;
                        OutputQuantizedSignal.Samples.Add(midpoint);
                        OutputIntervalIndices.Add(InputLevel);
                        OutputSamplesError.Add(midpoint - InputSignal.Samples[i]);
                        OutputEncodedSignal.Add(Convert.ToString(InputLevel - 1, 2).PadLeft(InputNumBits, '0'));
                        break;
                    }

                }
                level = 0;
            }
        }
    }
}