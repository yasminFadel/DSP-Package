using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class InverseDiscreteFourierTransform : Algorithm
    {
        public Signal InputFreqDomainSignal { get; set; }
        public Signal OutputTimeDomainSignal { get; set; }

        public override void Run()
        {
            OutputTimeDomainSignal = new Signal(new List<float>(), true);
            int N = InputFreqDomainSignal.FrequenciesAmplitudes.Count;

            for (int k = 0; k < N; k++)
            {
                Complex sum = new Complex(0, 0);
                for (int i = 0; i < N; i++)
                {
                    float p = InputFreqDomainSignal.FrequenciesPhaseShifts[i];
                    float A = InputFreqDomainSignal.FrequenciesAmplitudes[i];
                    Complex Xk = new Complex(A * Math.Cos(p), A * Math.Sin(p));

                    double equation = 2 * k * i * Math.PI / N;

                    Complex temp = new Complex(Xk.Real * Math.Cos(equation), Xk.Imaginary * -Math.Sin(equation));
                    sum += temp;
                }
                OutputTimeDomainSignal.Samples.Add((float)(sum.Real + sum.Imaginary) / N);
            }



        }
    }
}