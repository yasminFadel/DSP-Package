using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Derivatives : Algorithm
    {
        public Signal InputSignal { get; set; }
        public Signal FirstDerivative { get; set; }
        public Signal SecondDerivative { get; set; }

        public override void Run()
        {
            float first = 0, second = 0;
            FirstDerivative = new Signal(new List<float>(), InputSignal.Periodic);
            SecondDerivative = new Signal(new List<float>(), InputSignal.Periodic);

            for (int i = 1; i < InputSignal.Samples.Count; i++)
            {
                first = InputSignal.Samples[i] - InputSignal.Samples[i - 1];
                FirstDerivative.Samples.Add(first);
            }
            for (int i = 1; i < InputSignal.Samples.Count - 1; i++)
            {
                second = InputSignal.Samples[i + 1] - (2 * InputSignal.Samples[i]) + InputSignal.Samples[i - 1];
                SecondDerivative.Samples.Add(second);
            }
        }
    }
}
