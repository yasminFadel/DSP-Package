using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;

namespace DSPAlgorithms.Algorithms
{
    public class FastCorrelation : Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public List<float> OutputNonNormalizedCorrelation { get; set; }
        public List<float> OutputNormalizedCorrelation { get; set; }
        //start
        public override void Run()
        {
            OutputNonNormalizedCorrelation = new List<float>();
            OutputNormalizedCorrelation = new List<float>();
            InputSignal1.SamplesIndices = new List<int>();
            int size = InputSignal1.Samples.Count;

            Signal temp = new Signal(new List<float>(), true);
            temp.FrequenciesAmplitudes = new List<float>();
            temp.FrequenciesPhaseShifts = new List<float>();

            if (InputSignal2 == null)
            {
                InputSignal2 = new Signal(new List<float>(), InputSignal1.Periodic);
                InputSignal2.Samples.AddRange(InputSignal1.Samples);
                InputSignal2.SamplesIndices = new List<int>();
            }
            else if (InputSignal1.Samples.Count != InputSignal2.Samples.Count)
            {
                size = InputSignal1.Samples.Count + InputSignal2.Samples.Count - 1;
                while (InputSignal1.Samples.Count < size)
                {
                    InputSignal1.Samples.Add(0);
                }
                while (InputSignal2.Samples.Count < size)
                {
                    InputSignal2.Samples.Add(0);
                }
   
            }
            //normalization term
            float sumPow1 = 0, sumPow2 = 0,norm;
            for (int i = 0; i < size; i++)
            {
                sumPow1 += (float)Math.Pow(InputSignal1.Samples[i], 2);
                sumPow2 += (float)Math.Pow(InputSignal2.Samples[i], 2);
            }
            norm = (float)Math.Sqrt(sumPow1 * sumPow2) / size;

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
                sum1 = new Complex(sum1.Real , -sum1.Imaginary);
                sum = sum1 * sum2;
                temp.FrequenciesAmplitudes.Add((float)Math.Sqrt(Math.Pow(sum.Real, 2) + Math.Pow(sum.Imaginary, 2)));
                temp.FrequenciesPhaseShifts.Add((float)Math.Atan2(sum.Imaginary, sum.Real));
            }
            InverseDiscreteFourierTransform Idisc = new InverseDiscreteFourierTransform();
            Idisc.InputFreqDomainSignal = temp;
            Idisc.Run();
            temp = Idisc.OutputTimeDomainSignal;
            for (int k = 0; k < size; k++)
            {
                OutputNonNormalizedCorrelation.Add(temp.Samples[k]/size);
                OutputNormalizedCorrelation.Add((temp.Samples[k]/ size)/norm); 
            }

        }
    }
}