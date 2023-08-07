using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSPAlgorithms.DataStructures;

namespace DSPAlgorithms.Algorithms
{
    public class TimeDelay:Algorithm
    {
        public Signal InputSignal1 { get; set; }
        public Signal InputSignal2 { get; set; }
        public float InputSamplingPeriod { get; set; }
        public float OutputTimeDelay { get; set; }

        public override void Run()
        {
            //create corr obj to run it.
            DirectCorrelation dir_corr_obj= new DirectCorrelation();

            dir_corr_obj.InputSignal1 = new Signal(new List<float>(), InputSignal1.Periodic);
            dir_corr_obj.InputSignal1.Samples.AddRange(InputSignal1.Samples);

            dir_corr_obj.InputSignal2 = new Signal(new List<float>(), InputSignal2.Periodic);
            dir_corr_obj.InputSignal2.Samples.AddRange(InputSignal2.Samples);

            dir_corr_obj.Run();

            float larger_corr = float.MinValue;
            int delaay = 0;

            for(int index = 0; index < dir_corr_obj.InputSignal1.Samples.Count; index++)
            {

                //find largest correlation.
                if (dir_corr_obj.OutputNonNormalizedCorrelation[index] > larger_corr)
                {
                    delaay = index;
                }
            }
            OutputTimeDelay = InputSamplingPeriod * delaay;

        }
    }
}
