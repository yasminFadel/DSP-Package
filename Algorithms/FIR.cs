using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;
using System.Numerics;

namespace DSPAlgorithms.Algorithms
{
    public class FIR : Algorithm
    {
        public Signal InputTimeDomainSignal { get; set; } 
        public FILTER_TYPES InputFilterType { get; set; }  //
        public float InputFS { get; set; } //sample frequency
        public float? InputCutOffFrequency { get; set; }  // fc (low pass aw high pass)
        public float? InputF1 { get; set; } //(band pass aw bande reject)
        public float? InputF2 { get; set; } //(band pass aw bande reject)
        public float InputStopBandAttenuation { get; set; }  // window fun
        public float InputTransitionBand { get; set; } // trans width
        public Signal OutputHn { get; set; }  // filter value  (hatetheseb hasab el window fn, ba3den n3melha convulotion m3 el input F1 aw inputF2)
        public Signal OutputYn { get; set; }  // filter value * window function  ()
        public float normalised1 { get; set; }
        public float normalised2 { get; set; }

        public override void Run()
        {
            OutputHn = new Signal(new List<float>(), new List<int>(), InputTimeDomainSignal.Periodic);
            OutputYn = new Signal(new List<float>(), new List<int>(), InputTimeDomainSignal.Periodic);
            float N; int i;
            List<double> window;
            Signal filter = new Signal(new List<float>(), new List<int>(), InputTimeDomainSignal.Periodic);
            Signal sig = new Signal(new List<float>(), new List<int>(), InputTimeDomainSignal.Periodic);
            DirectConvolution convObj = new DirectConvolution(); 
            if (InputStopBandAttenuation < 21) // rectangular
            {
                window = rectangular();
                N = (float) window[window.Count - 1];

                if ((int)InputFilterType == 0)
                    filter = lowPass(N);
                else if ((int)InputFilterType == 1)
                    filter = highPass(N);
                else if ((int)InputFilterType == 2)
                    filter = bandPass(N);
                else if ((int)InputFilterType == 3)
                    filter = bandStop(N);

                for (i = 0; i < window.Count-1; i++)
                {
                    OutputHn.Samples.Add((float) (window[i] * filter.Samples[i]));
                    OutputHn.SamplesIndices.Add(filter.SamplesIndices[i]);
                }
            }
            else if (InputStopBandAttenuation < 44) // hanning
            {
                window = hanning();
                N = (float)window[window.Count - 1];

                if ((int)InputFilterType == 0)
                    filter = lowPass(N);
                else if ((int)InputFilterType == 1)
                    filter = highPass(N);
                else if ((int)InputFilterType == 2)
                    filter = bandPass(N);
                else if ((int)InputFilterType == 3)
                    filter = bandStop(N);

                for (i = 0; i < window.Count - 1; i++)
                {
                    OutputHn.Samples.Add((float)(window[i] * filter.Samples[i]));
                    OutputHn.SamplesIndices.Add(filter.SamplesIndices[i]);
                }
            }      
            else if (InputStopBandAttenuation < 53) // hamming
            {
                window = hamming();
                N = (float)window[window.Count - 1];

                if ((int)InputFilterType ==0)
                    filter = lowPass(N);
                else if ((int)InputFilterType == 1)
                    filter = highPass(N);
                else if ((int)InputFilterType == 2)
                    filter = bandPass(N);
                else if((int)InputFilterType == 3)
                    filter = bandStop(N);

                for (i = 0; i < window.Count - 1; i++)
                {
                    OutputHn.Samples.Add((float)(window[i] * filter.Samples[i]));
                    OutputHn.SamplesIndices.Add(filter.SamplesIndices[i]);
                }
            }
            else // blackman
            {
                window = blackMan();
                N = (float)window[window.Count - 1];

                if ((int)InputFilterType == 0)
                    filter = lowPass(N);
                else if ((int)InputFilterType == 1)
                    filter = highPass(N);
                else if ((int)InputFilterType == 2)
                    filter = bandPass(N);
                else if ((int)InputFilterType == 3)
                    filter = bandStop(N);

                for (i = 0; i < window.Count - 1; i++)
                {
                    OutputHn.Samples.Add((float)(window[i] * filter.Samples[i]));
                    OutputHn.SamplesIndices.Add(filter.SamplesIndices[i]);
                }
            }
            sig.Samples.AddRange(OutputHn.Samples);
            sig.SamplesIndices.AddRange(OutputHn.SamplesIndices);
            convObj.InputSignal1 = InputTimeDomainSignal;
            convObj.InputSignal2 = sig;
            convObj.Run();
            OutputYn = convObj.OutputConvolvedSignal;
        }
        public List<double> rectangular()
        {
            float deltaF, N;
            float min, max, i;
            double windowFn;
            List<double> output = new List<double>();
            deltaF = InputTransitionBand / InputFS;
            N = (float)Math.Ceiling((double)(0.9 / deltaF));
            if (Math.Ceiling(N) % 2 == 0)
                N++;
            min = -((N - 1) / 2);
            max = (N - 1) / 2;
            for (i = min; i <= max; i++)
            {
                windowFn = 1;
                output.Add(windowFn);
            }
            output.Add((double)N);
            return output;
        }
        public List<double> hanning()
        {
            float deltaF, N;
            float min, max, i;
            double windowFn;
            List<double> output = new List<double>();
            deltaF = InputTransitionBand / InputFS;
            N = (float)Math.Ceiling((double)(3.1 / deltaF));
            if (Math.Ceiling(N) % 2 == 0)
                N++;
            min = -((N - 1) / 2);
            max = (N - 1) / 2;
            for (i = min; i <= max; i++)
            {
                windowFn = 0.5 + 0.5 * Math.Cos((2 * Math.PI * i) / N);
                output.Add(windowFn);
            }
            output.Add((double)N);
            return output;
        }
        public List<double> hamming()
        {
            float deltaF, N;
            float min, max, i;
            double windowFn;
            List<double> output = new List<double>();
            deltaF = InputTransitionBand / InputFS;
            N = (float)Math.Ceiling((double)(3.3 / deltaF));
            if (Math.Ceiling(N) % 2 == 0)
                N++;
            min = -((N - 1) / 2);
            max = (N - 1) / 2;
            for (i = min; i <= max; i++)
            {
                windowFn = 0.54 + 0.46 * Math.Cos((2 * Math.PI * i) / N);
                output.Add(windowFn);
            }
            output.Add((double)N);
            return output;
        }
        public List<double> blackMan()
        {
            float deltaF, N;
            float min, max, i;
            double windowFn;
            List<double> output = new List<double>();
            deltaF = InputTransitionBand / InputFS;
            N = (float)Math.Ceiling((double)(5.5 / deltaF));
            
            if (Math.Ceiling(N) % 2 == 0)
                N++;
            
            min = -((N - 1) / 2);
            max = (N - 1) / 2;
            for (i = min; i <= max; i++)
            {
                windowFn = 0.42 + 0.5 * Math.Cos((2 * Math.PI * i) / (N - 1)) + 0.08 * Math.Cos((4 * Math.PI * i) / (N - 1));
                output.Add(windowFn);
            }
            output.Add((double)N);
            return output;
        }
        public Signal lowPass(float N)
        {
            float min, max, fc, normalised, i;
            double filterOutput, wc;
            Signal filters = new Signal(new List<float>(), new List<int>(), InputTimeDomainSignal.Periodic);

            min = -((N - 1) / 2);
            max = (N - 1) / 2;
            fc = (float)InputCutOffFrequency + (InputTransitionBand / 2);
            normalised = fc / InputFS;
            for (i = min; i <= max; i++)
            {
                wc = 2 * Math.PI * normalised;
                if (i == 0)
                    filterOutput = 2 * normalised;
                else
                    filterOutput = 2 * normalised * (Math.Sin(i * wc) / (i * wc));
                filters.Samples.Add((float)filterOutput);
                filters.SamplesIndices.Add((int) i);
            }
            return filters;
        }
        public Signal highPass(float N)
        {
            float min, max, fc, normalised, i;
            double filterOutput, wc;
            Signal filters = new Signal(new List<float>(), new List<int>(), InputTimeDomainSignal.Periodic);

            min = -((N - 1) / 2);
            max = (N - 1) / 2;
            fc = (float)InputCutOffFrequency - (InputTransitionBand / 2);
            normalised = fc / InputFS;
            for (i = min; i <= max; i++)
            {
                wc = 2 * Math.PI * normalised;
                if (i == 0)
                    filterOutput = 1 - (2 * normalised);
                else
                    filterOutput = - 2 * normalised * (Math.Sin(i * wc) / (i * wc));
                filters.Samples.Add((float)filterOutput);
                filters.SamplesIndices.Add((int)i);
            }
            return filters;
        }
        public Signal bandPass(float N)
        {
            float min, max, fc1,fc2, i;
            double filterOutput, wc1, wc2, hd1,hd2;
            Signal filters = new Signal(new List<float>(), new List<int>(), InputTimeDomainSignal.Periodic);

            min = -((N - 1) / 2);
            max = (N - 1) / 2;

            fc1 = (float)InputF1 - (InputTransitionBand / 2);
            fc2 = (float)InputF2 + (InputTransitionBand / 2);

            normalised1 = fc1 / InputFS;
            normalised2 = fc2 / InputFS;

            for (i = min; i <= max; i++)
            {
                wc1 = 2 * Math.PI * normalised1;
                wc2 = 2 * Math.PI * normalised2;
                if (i == 0)
                    filterOutput = 2 * (normalised2 - normalised1);
                else
                {
                    hd1 = 2 * normalised1 * (Math.Sin(i * wc1) / (i * wc1));
                    hd2 = 2 * normalised2 * (Math.Sin(i * wc2) / (i * wc2));
                    filterOutput = hd2 - hd1;
                }
                filters.Samples.Add((float)filterOutput);
                filters.SamplesIndices.Add((int)i);
            }
            return filters;
        }
        public Signal bandStop(float N)
        {
            float min, max, fc1, fc2,i;
            double filterOutput, wc1, wc2, hd1, hd2;
            Signal filters = new Signal(new List<float>(), new List<int>(), InputTimeDomainSignal.Periodic);

            min = -((N - 1) / 2);
            max = (N - 1) / 2;

            fc1 = (float)InputF1 + (InputTransitionBand / 2);
            fc2 = (float)InputF2 - (InputTransitionBand / 2);

            normalised1 = fc1 / InputFS;
            normalised2 = fc2 / InputFS;

            for (i = min; i <= max; i++)
            {
                wc1 = 2 * Math.PI * normalised1;
                wc2 = 2 * Math.PI * normalised2;
                if (i == 0)
                    filterOutput = 1 - 2 * (normalised2 - normalised1);
                else
                {
                    hd1 = 2 * normalised1 * (Math.Sin(i * wc1) / (i * wc1));
                    hd2 = 2 * normalised2 * (Math.Sin(i * wc2) / (i * wc2));
                    filterOutput = hd1 - hd2;
                }
                filters.Samples.Add((float)filterOutput);
                filters.SamplesIndices.Add((int)i);
            }
            return filters;
        }
    }
}
