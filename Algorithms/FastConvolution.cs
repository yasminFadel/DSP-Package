using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;

namespace DSPAlgorithms.Algorithms
{
    public class FastConvolution : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public Signal OutputConvolvedSignal { get; set; }

        /// <summary>
        /// Convolved InputSignal1 (considered as X) with InputSignal2 (considered as H)
        /// </summary>
        public override void Run()
        {
            OutputConvolvedSignal = new Signal(new List<float>(), InputSignal1.Periodic);
            InputSignal1.SamplesIndices = new List<int>();
            InputSignal2.SamplesIndices = new List<int>();
            OutputConvolvedSignal.FrequenciesAmplitudes = new List<float>();
            OutputConvolvedSignal.FrequenciesPhaseShifts = new List<float>();
            int size = InputSignal1.Samples.Count + InputSignal2.Samples.Count - 1;
            while (InputSignal1.Samples.Count < size)
            {
                InputSignal1.Samples.Add(0);
            }
            while (InputSignal2.Samples.Count < size)
            {
                InputSignal2.Samples.Add(0);
            }

            Complex temp1 = new Complex(0, 0);
            Complex temp2 = new Complex(0, 0);
            for (int k = 0; k < size; k++)
            {
                Complex sum1 = new Complex(0, 0);
                Complex sum2 = new Complex(0, 0);
                Complex sum = new Complex(0, 0);
                int n = k;
                for (int i = 0; i < size; i++)
                {
                    double equation = (i * 2 * Math.PI * n) / size;
                    temp1 = new Complex(InputSignal1.Samples[i] * Math.Cos(equation), InputSignal1.Samples[i] * -Math.Sin(equation));
                    temp2 = new Complex(InputSignal2.Samples[i] * Math.Cos(equation), InputSignal2.Samples[i] * -Math.Sin(equation));
                    sum1 += temp1;
                    sum2 += temp2;

                }
                sum = sum1 * sum2;
                OutputConvolvedSignal.FrequenciesAmplitudes.Add((float)Math.Sqrt(Math.Pow(sum.Real, 2) + Math.Pow(sum.Imaginary, 2)));
                OutputConvolvedSignal.FrequenciesPhaseShifts.Add((float)Math.Atan2(sum.Imaginary, sum.Real));
            }
            InverseDiscreteFourierTransform Idisc= new InverseDiscreteFourierTransform();
            Idisc.InputFreqDomainSignal = OutputConvolvedSignal;
            Idisc.Run();
            OutputConvolvedSignal = Idisc.OutputTimeDomainSignal;
            int conv_out_size = OutputConvolvedSignal.Samples.Count;
            //if last value is zero remove it.
            for (int i = conv_out_size - 1; ; i--)
            {
                if (OutputConvolvedSignal.Samples[i] == 0)
                    OutputConvolvedSignal.Samples.RemoveAt(i);
                else
                    break;
            }


        }

    }
}
