using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSPAlgorithms.Algorithms
{
    public class Sampling : Algorithm
    {
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal InputSignal { get; set; }
        public Signal OutputSignal { get; set; }


        public override void Run()
        {

            OutputSignal = new Signal(new List<float>(), InputSignal.Periodic);
            FIR filterObj = new FIR();
            filterObj.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.LOW;
            filterObj.InputFS = 8000;
            filterObj.InputStopBandAttenuation = 50;
            filterObj.InputCutOffFrequency = 1500;
            filterObj.InputTransitionBand = 500;

            if (M == 0 && L != 0)
            {
                filterObj.InputTimeDomainSignal = upsampling(InputSignal);
                filterObj.Run();
                OutputSignal = filterObj.OutputYn;
            }
            else if (M != 0 && L == 0)
            {
                filterObj.InputTimeDomainSignal = InputSignal;
                filterObj.Run();
                OutputSignal = filterObj.OutputYn;
                OutputSignal = downsampling(OutputSignal);
            }
            else if (M != 0 && L != 0)
            {
                filterObj.InputTimeDomainSignal = upsampling(InputSignal);
                filterObj.Run();
                OutputSignal = filterObj.OutputYn;
                OutputSignal = downsampling(OutputSignal);
            }
            else
            {
                OutputSignal = InputSignal;
            }

        }

        public Signal upsampling(Signal s)
        {
            Signal upsamplingSignal = new Signal(new List<float>(), InputSignal.Periodic);

            for (int i = 0; i < s.Samples.Count; i++)
            {
                upsamplingSignal.Samples.Add(s.Samples[i]);
                for (int j = 0; j < L - 1; j++)
                {
                    upsamplingSignal.Samples.Add(0);
                }
            }
            for (int i = s.SamplesIndices[0]; i < upsamplingSignal.Samples.Count; i++)
            {
                upsamplingSignal.SamplesIndices.Add(i);
            }
            return upsamplingSignal;
        }
        public Signal downsampling(Signal s)
        {
            Signal downsamplingSignal = new Signal(new List<float>(), InputSignal.Periodic);
            int counter = M - 1;
            for (int i = 0; i < s.Samples.Count; i++)
            {
                if (counter != M - 1)
                {
                    counter++;
                    continue;
                }
                downsamplingSignal.Samples.Add(s.Samples[i]);
                counter = 0;
            }
            for (int i = s.SamplesIndices[0]; i < downsamplingSignal.Samples.Count; i++)
            {
                downsamplingSignal.SamplesIndices.Add(i);
            }
            return downsamplingSignal;
        }

    }

}


