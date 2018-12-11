using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TactPlay.Pattern
{
    public class SimplePattern : IPattern
    {
        private int[] timings;
        private double[] amplitudes;
        private int motor = -1;

        public SimplePattern(int[] timings, double[] amplitudes)
            : this(0, timings, amplitudes)
        {

        }

        /**
         * Create a new Pattern wit the given motor, timings and amplitudes.
         * Length of timings and amplitudes array must be equal and greater 0.
         * <p>
         * Example:
         * [10,5,20][0.1,0.5,0.0] for 10ms at 10%, 5ms at 50% and 20ms at 0%
         */
        public SimplePattern(int motor, int[] timings, double[] amplitudes)
            : base()
        {
            if (timings.Length != amplitudes.Length || timings.Length == 0)
            {
                throw new Exception("Array length does not match or is 0");
            }
            this.timings = timings;
            this.amplitudes = amplitudes;
            this.motor = motor;
        }

        private SimplePattern(SimplePattern simplePattern)
            : this(simplePattern.motor, simplePattern.timings, simplePattern.amplitudes)
        {

        }

        public int[] GetTimings()
        {
            return timings;
        }

        public double[] GetAmplitudes()
        {
            return amplitudes;
        }

        public int GetMotor()
        {
            return motor;
        }

        public void SetMotor(int motor)
        {
            this.motor = motor;
        }

        public SimplePattern Copy()
        {
            return new SimplePattern(this);
        }


        public List<SimplePattern> GetSimplePatterns()
        {
            List<SimplePattern> patternList = new List<SimplePattern>
            {
                this
            };
            return patternList;
        }

        public long GetDuration()
        {
            long duration = 0;
            for (int i = 0; i < timings.Length; i++)
            {
                duration += timings[i];
            }
            return duration;
        }
    }
}
