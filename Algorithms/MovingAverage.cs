using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class MovingAverage : Algorithm
    {
        public Signal InputSignal { get; set; }
        public int InputWindowSize { get; set; }
        public Signal OutputAverageSignal { get; set; }

        public override void Run()
        {
            OutputAverageSignal = new Signal(new List<float>(), InputSignal.Periodic);
            float average = 0;
            int newsize = InputSignal.Samples.Count - InputWindowSize + 1;

            for (int i = 0; i < newsize; i++)
            {
                int temp = i;
                for (int j = 0; j < InputWindowSize; j++)
                {
                    average += InputSignal.Samples[temp];
                    temp++;
                }
                OutputAverageSignal.Samples.Add(average / InputWindowSize);
                average = 0;
            }

        }
    }
}
