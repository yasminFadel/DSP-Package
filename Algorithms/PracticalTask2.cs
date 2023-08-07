﻿using DSPAlgorithms.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace DSPAlgorithms.Algorithms
{
    public class PracticalTask2 : Algorithm
    {
        public String SignalPath { get; set; }
        public float Fs { get; set; }
        public float miniF { get; set; }
        public float maxF { get; set; }
        public float newFs { get; set; }
        public int L { get; set; } //upsampling factor
        public int M { get; set; } //downsampling factor
        public Signal OutputFreqDomainSignal { get; set; }

        public override void Run()
        {
            Signal InputSignal = LoadSignal(SignalPath);

            OutputFreqDomainSignal = new Signal(new List<float>(), InputSignal.Periodic);
            FIR filterObj = new FIR();
            filterObj.InputStopBandAttenuation = 50;
            filterObj.InputTransitionBand = 500;
            filterObj.InputFS = Fs;
            filterObj.InputF1 = miniF;
            filterObj.InputF2 = maxF;
            filterObj.InputFilterType = DSPAlgorithms.DataStructures.FILTER_TYPES.BAND_PASS;
            filterObj.InputTimeDomainSignal = InputSignal;
            filterObj.Run();
            display(filterObj.OutputYn, 0, "Filtered signal");
            Sampling sampleObj = new Sampling();
            if(newFs >= 2* maxF)
            {
                sampleObj.L = L;
                sampleObj.M = M;
                sampleObj.InputSignal = filterObj.OutputYn;
                sampleObj.Run();
            }
            display(sampleObj.OutputSignal, 0,"Sampled signal");

            DC_Component dcObj = new DC_Component();
            dcObj.InputSignal = sampleObj.OutputSignal;
            dcObj.Run();
            display(dcObj.OutputSignal, 0, "DC signal");

            Normalizer normObj = new Normalizer();
            normObj.InputSignal = dcObj.OutputSignal;
            normObj.InputMinRange = -1;
            normObj.InputMaxRange = 1;
            normObj.Run();
            display(normObj.OutputNormalizedSignal, 0 ,"Normalised signal");

            DiscreteFourierTransform dftObj = new DiscreteFourierTransform();
            dftObj.InputTimeDomainSignal = normObj.OutputNormalizedSignal;
            dftObj.InputSamplingFrequency = Fs;
            dftObj.Run();
            OutputFreqDomainSignal = dftObj.OutputFreqDomainSignal;
            display(dftObj.OutputFreqDomainSignal, 1 , "DFT signal");
        }
        public void display(Signal sig, int domain,string name)
        {
            int var = 0;
            String fullPath = "C:\\Users\\DELL\\Desktop\\" + name + ".txt";
            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                if(sig.Periodic)
                    var = 1;

                writer.WriteLine(domain);
                writer.WriteLine(var);
                writer.WriteLine(sig.Samples.Count);
                for(int i =0; i < sig.Samples.Count; i++)
                {
                    if(domain == 1)
                    {
                        writer.Write(sig.Frequencies[i]);
                        writer.Write(" ");
                        writer.Write(sig.FrequenciesAmplitudes[i]);
                        writer.Write(" ");
                        writer.WriteLine(sig.FrequenciesPhaseShifts[i]);
                    }
                    else
                    {
                        writer.Write(sig.SamplesIndices[i]);
                        writer.Write(" ");
                        writer.WriteLine(sig.Samples[i]);
                    }
                }
            }
        }
        public Signal LoadSignal(string filePath)
        {
            Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            var sr = new StreamReader(stream);

            var sigType = byte.Parse(sr.ReadLine());
            var isPeriodic = byte.Parse(sr.ReadLine());
            long N1 = long.Parse(sr.ReadLine());

            List<float> SigSamples = new List<float>(unchecked((int)N1));
            List<int> SigIndices = new List<int>(unchecked((int)N1));
            List<float> SigFreq = new List<float>(unchecked((int)N1));
            List<float> SigFreqAmp = new List<float>(unchecked((int)N1));
            List<float> SigPhaseShift = new List<float>(unchecked((int)N1));

            if (sigType == 1)
            {
                SigSamples = null;
                SigIndices = null;
            }

            for (int i = 0; i < N1; i++)
            {
                if (sigType == 0 || sigType == 2)
                {
                    var timeIndex_SampleAmplitude = sr.ReadLine().Split();
                    SigIndices.Add(int.Parse(timeIndex_SampleAmplitude[0]));
                    SigSamples.Add(float.Parse(timeIndex_SampleAmplitude[1]));
                }
                else
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            if (!sr.EndOfStream)
            {
                long N2 = long.Parse(sr.ReadLine());

                for (int i = 0; i < N2; i++)
                {
                    var Freq_Amp_PhaseShift = sr.ReadLine().Split();
                    SigFreq.Add(float.Parse(Freq_Amp_PhaseShift[0]));
                    SigFreqAmp.Add(float.Parse(Freq_Amp_PhaseShift[1]));
                    SigPhaseShift.Add(float.Parse(Freq_Amp_PhaseShift[2]));
                }
            }

            stream.Close();
            return new Signal(SigSamples, SigIndices, isPeriodic == 1, SigFreq, SigFreqAmp, SigPhaseShift);
        }
    }
}
