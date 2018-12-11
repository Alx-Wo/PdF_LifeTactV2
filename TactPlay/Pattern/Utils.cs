using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TactPlay.Pattern
{
    class Utils
    {
        public static readonly int AMMO_MOTOR_NUMBER = 3;
        private static readonly int AMMO_MOTOR_DIRECTION = -1;

        public static readonly int HEALTH_MOTOR_NUMBER = 3;
        private static readonly int HEALTH_MOTOR_DIRECTION = -1;

        public static readonly int STAMINA_MOTOR_NUMBER = 5;
        private static readonly int STAMINA_MOTOR_DIRECTION = -1;

        public static SimplePattern GetSineWavePattern(int periodicDuration, double minValue, double maxValue, double phaseShift, int patternLength, int intervallTime)
        {
            int[] timings = new int[patternLength / intervallTime];
            double[] amplitudes = new double[patternLength / intervallTime];

            double amplitude = (maxValue - minValue) / 2;
            double offset = (maxValue + minValue) / 2;
            double mod = (2 * Math.PI / periodicDuration);

            for (int i = 0; i < timings.Length; i++)
            {
                int time = i * intervallTime;
                double value = amplitude * Math.Sin(mod * time + phaseShift) + offset;
                timings[i] = intervallTime;
                amplitudes[i] = value;
            }
            return new SimplePattern(timings, amplitudes);
        }

        public static SimplePattern SIMPLE_VIBRATION(int vibrationDuration)
        {
            return new SimplePattern(AMMO_MOTOR_NUMBER, new int[] { vibrationDuration }, new double[] { 1.0 });
        }

        private static SimplePattern SimpleVibrations(int motorNumber, int vibrationDuraion, int idleTime, int n)
        {
            int[] timings = new int[(n - 1) * 2 + 1];
            double[] amplitudes = new double[(n - 1) * 2 + 1];
            for (int i = 0; i < timings.Length; i++)
            {
                if (i % 2 == 0)
                {
                    timings[i] = vibrationDuraion;
                    amplitudes[i] = 1.0;
                }
                else
                {
                    timings[i] = idleTime;
                    amplitudes[i] = 0.0;
                }
            }
            return new SimplePattern(motorNumber, timings, amplitudes);
        }

        public static IPattern AMMO_PATTERN_STUDY_2(double ammoLevel)
        {
            int vibrationDuration = 200;
            if (ammoLevel > 20.0 / 30)
            {
                return SimpleVibrations(AMMO_MOTOR_NUMBER + 0 * AMMO_MOTOR_DIRECTION, vibrationDuration, 100, 1);
            }
            else if (ammoLevel > 12.0 / 30)
            {
                return SimpleVibrations(AMMO_MOTOR_NUMBER + 1 * AMMO_MOTOR_DIRECTION, vibrationDuration, 100, 2);

            }
            else if (ammoLevel > 5.0 / 30)
            {
                return SimpleVibrations(AMMO_MOTOR_NUMBER + 2 * AMMO_MOTOR_DIRECTION, vibrationDuration, 100, 3);
            }
            else
            {
                return SimpleVibrations(AMMO_MOTOR_NUMBER + 3 * AMMO_MOTOR_DIRECTION, vibrationDuration, 100, 4);
            }
        }

        public static IPattern AMMO_PATTERN_1(double ammoLevel, int vibrationDuration)
        {
            int numOfLevels = 4;
            int ammoLvl = (int)Math.Max(Math.Ceiling(ammoLevel * numOfLevels), 1);
            return SimpleVibrations(AMMO_MOTOR_NUMBER, vibrationDuration, 100, ammoLvl);
        }

        public static IPattern AMMO_PATTERN_1b(double ammoLevel, int vibrationDuration)
        {
            int numOfLevels = 4;
            int ammoLvl = (int)Math.Min(Math.Max(numOfLevels + 1 - Math.Ceiling(ammoLevel * numOfLevels), 1), numOfLevels);
            return SimpleVibrations(AMMO_MOTOR_NUMBER, vibrationDuration, 100, ammoLvl);
        }

        public static IPattern AMMO_PATTERN_2b(double ammoLevel, int vibrationDuration)
        {
            int numOfLevels = 4;
            int ammoLvl = 5 - ((int)Math.Min(Math.Max(numOfLevels + 1 - Math.Ceiling(ammoLevel * numOfLevels), 1), numOfLevels));
            return new SimplePattern(AMMO_MOTOR_NUMBER, new int[] { vibrationDuration * ammoLvl }, new double[] { 1.0 });
        }

        public static IPattern AMMO_PATTERN_2c(double ammoLevel, int vibrationDuration)
        {
            int numOfLevels = 4;
            int ammoLvl = (int)Math.Min(Math.Max(numOfLevels + 1 - Math.Ceiling(ammoLevel * numOfLevels), 1), numOfLevels);
            return new SimplePattern(AMMO_MOTOR_NUMBER, new int[] { vibrationDuration * ammoLvl }, new double[] { 0.5 + ((0.5 / (numOfLevels - 1)) * (ammoLvl - 1)) });
        }

        public static IPattern AMMO_PATTERN_3s(double ammoLevel, SimplePattern pattern)
        {
            SimplePattern result = pattern.Copy();
            if (ammoLevel > 3.0 / 4)
            {
                result.SetMotor(AMMO_MOTOR_NUMBER + 3 * AMMO_MOTOR_DIRECTION);
            }
            else if (ammoLevel > 2.0 / 4)
            {
                result.SetMotor(AMMO_MOTOR_NUMBER + 2 * AMMO_MOTOR_DIRECTION);
            }
            else if (ammoLevel > 1.0 / 4)
            {
                result.SetMotor(AMMO_MOTOR_NUMBER + 1 * AMMO_MOTOR_DIRECTION);
            }
            else
            {
                result.SetMotor(AMMO_MOTOR_NUMBER + 0 * AMMO_MOTOR_DIRECTION);
            }
            return result;
        }

        public static IPattern AMMO_PATTERN_3l(double ammoLevel, SimplePattern pattern)
        {
            SimplePattern result = pattern.Copy();
            if (ammoLevel > 3.0 / 4)
            {
                result.SetMotor(AMMO_MOTOR_NUMBER + 0 * AMMO_MOTOR_DIRECTION);
            }
            else if (ammoLevel > 2.0 / 4)
            {
                result.SetMotor(AMMO_MOTOR_NUMBER + 1 * AMMO_MOTOR_DIRECTION);
            }
            else if (ammoLevel > 1.0 / 4)
            {
                result.SetMotor(AMMO_MOTOR_NUMBER + 2 * AMMO_MOTOR_DIRECTION);
            }
            else
            {
                result.SetMotor(AMMO_MOTOR_NUMBER + 3 * AMMO_MOTOR_DIRECTION);
            }
            return result;
        }

        public static IPattern AMMO_PATTERN_4s(double ammoLevel, SimplePattern pattern)
        {
            if (ammoLevel > 3.0 / 4)
            {
                SimplePattern copy = pattern.Copy();
                copy.SetMotor(AMMO_MOTOR_NUMBER + 3 * AMMO_MOTOR_DIRECTION);
                return copy;
            }
            else if (ammoLevel > 2.0 / 4)
            {
                SimplePattern copy = pattern.Copy();
                copy.SetMotor(AMMO_MOTOR_NUMBER + 3 * AMMO_MOTOR_DIRECTION);
                SimplePattern copy2 = pattern.Copy();
                copy2.SetMotor(AMMO_MOTOR_NUMBER + 2 * AMMO_MOTOR_DIRECTION);
                return new MultiPattern(copy, copy2);
            }
            else if (ammoLevel > 1.0 / 4)
            {
                SimplePattern copy = pattern.Copy();
                copy.SetMotor(AMMO_MOTOR_NUMBER + 3 * AMMO_MOTOR_DIRECTION);
                SimplePattern copy2 = pattern.Copy();
                copy2.SetMotor(AMMO_MOTOR_NUMBER + 2 * AMMO_MOTOR_DIRECTION);
                SimplePattern copy3 = pattern.Copy();
                copy3.SetMotor(AMMO_MOTOR_NUMBER + 1 * AMMO_MOTOR_DIRECTION);
                return new MultiPattern(copy, copy2, copy3);
            }
            else
            {
                SimplePattern copy = pattern.Copy();
                copy.SetMotor(AMMO_MOTOR_NUMBER + 3 * AMMO_MOTOR_DIRECTION);
                SimplePattern copy2 = pattern.Copy();
                copy2.SetMotor(AMMO_MOTOR_NUMBER + 2 * AMMO_MOTOR_DIRECTION);
                SimplePattern copy3 = pattern.Copy();
                copy3.SetMotor(AMMO_MOTOR_NUMBER + 1 * AMMO_MOTOR_DIRECTION);
                SimplePattern copy4 = pattern.Copy();
                copy4.SetMotor(AMMO_MOTOR_NUMBER + 0 * AMMO_MOTOR_DIRECTION);
                return new MultiPattern(copy, copy2, copy3, copy4);
            }
        }

        public static IPattern AMMO_PATTERN_4l(double ammoLevel, SimplePattern pattern)
        {
            if (ammoLevel > 3.0 / 4)
            {
                SimplePattern copy = pattern.Copy();
                copy.SetMotor(AMMO_MOTOR_NUMBER + 0 * AMMO_MOTOR_DIRECTION);
                return copy;
            }
            else if (ammoLevel > 2.0 / 4)
            {
                SimplePattern copy = pattern.Copy();
                copy.SetMotor(AMMO_MOTOR_NUMBER + 0 * AMMO_MOTOR_DIRECTION);
                SimplePattern copy2 = pattern.Copy();
                copy2.SetMotor(AMMO_MOTOR_NUMBER + 1 * AMMO_MOTOR_DIRECTION);
                return new MultiPattern(copy, copy2);
            }
            else if (ammoLevel > 1.0 / 4)
            {
                SimplePattern copy = pattern.Copy();
                copy.SetMotor(AMMO_MOTOR_NUMBER + 0 * AMMO_MOTOR_DIRECTION);
                SimplePattern copy2 = pattern.Copy();
                copy2.SetMotor(AMMO_MOTOR_NUMBER + 1 * AMMO_MOTOR_DIRECTION);
                SimplePattern copy3 = pattern.Copy();
                copy3.SetMotor(AMMO_MOTOR_NUMBER + 2 * AMMO_MOTOR_DIRECTION);
                return new MultiPattern(copy, copy2, copy3);
            }
            else
            {
                SimplePattern copy = pattern.Copy();
                copy.SetMotor(AMMO_MOTOR_NUMBER + 0 * AMMO_MOTOR_DIRECTION);
                SimplePattern copy2 = pattern.Copy();
                copy2.SetMotor(AMMO_MOTOR_NUMBER + 1 * AMMO_MOTOR_DIRECTION);
                SimplePattern copy3 = pattern.Copy();
                copy3.SetMotor(AMMO_MOTOR_NUMBER + 2 * AMMO_MOTOR_DIRECTION);
                SimplePattern copy4 = pattern.Copy();
                copy4.SetMotor(AMMO_MOTOR_NUMBER + 3 * AMMO_MOTOR_DIRECTION);
                return new MultiPattern(copy, copy2, copy3, copy4);
            }
        }

        public static IPattern AMMO_PATTERN_5(double ammoLevel, int vibrationDuration)
        {
            double relativeLevel = ((ammoLevel * 90000) % 30000) / 30000.0;
            if (relativeLevel == 0 && ammoLevel > 0)
            {
                relativeLevel = 1;
            }
            double amplitude = 0.5 + (1 - relativeLevel) * 0.5;
            if (ammoLevel > 2.0 / 3)
            {
                SimplePattern p1 = new SimplePattern(AMMO_MOTOR_NUMBER + 0 * AMMO_MOTOR_DIRECTION, new int[] { vibrationDuration }, new double[] { amplitude });
                return p1;
            }
            else if (ammoLevel > 1.0 / 3)
            {
                SimplePattern p1 = new SimplePattern(AMMO_MOTOR_NUMBER + 0 * AMMO_MOTOR_DIRECTION, new int[] { vibrationDuration }, new double[] { amplitude });
                SimplePattern p2 = new SimplePattern(AMMO_MOTOR_NUMBER + 1 * AMMO_MOTOR_DIRECTION, new int[] { vibrationDuration }, new double[] { amplitude });
                return new MultiPattern(p1, p2);
            }
            else
            {
                SimplePattern p1 = new SimplePattern(AMMO_MOTOR_NUMBER + 0 * AMMO_MOTOR_DIRECTION, new int[] { vibrationDuration }, new double[] { amplitude });
                SimplePattern p2 = new SimplePattern(AMMO_MOTOR_NUMBER + 1 * AMMO_MOTOR_DIRECTION, new int[] { vibrationDuration }, new double[] { amplitude });
                SimplePattern p3 = new SimplePattern(AMMO_MOTOR_NUMBER + 2 * AMMO_MOTOR_DIRECTION, new int[] { vibrationDuration }, new double[] { amplitude });
                return new MultiPattern(p1, p2, p3);
            }
        }

        public static IPattern AMMO_PATTERN_6(double ammoLevel)
        {
            if (ammoLevel > 3.0 / 4)
            {
                SimplePattern p1 = new SimplePattern(AMMO_MOTOR_NUMBER + 3 * AMMO_MOTOR_DIRECTION, new int[] { 190, 80, 190, 80, 190, 80, 190 }, new double[] { 1.0, 0.0, 1.0, 0.0, 1.0, 0.0, 1.0 });
                return p1;
            }
            else if (ammoLevel > 2.0 / 4)
            {
                SimplePattern p1 = new SimplePattern(AMMO_MOTOR_NUMBER + 2 * AMMO_MOTOR_DIRECTION, new int[] { 190, 25, 190, 25, 190 }, new double[] { 1.0, 0.0, 1.0, 0.0, 1.0 });
                return p1;
            }
            else if (ammoLevel > 1.0 / 4)
            {
                SimplePattern p1 = new SimplePattern(AMMO_MOTOR_NUMBER + 1 * AMMO_MOTOR_DIRECTION, new int[] { 190, 40, 190 }, new double[] { 1.0, 0.0, 1.0 });
                return p1;
            }
            else
            {
                SimplePattern p1 = new SimplePattern(AMMO_MOTOR_NUMBER + 0 * AMMO_MOTOR_DIRECTION, new int[] { 190 }, new double[] { 1.0 });
                return p1;
            }
        }

        public static IPattern AMMO_PATTERN_7(double ammoLevel, double minAmplitude)
        {
            if (ammoLevel > 3.0 / 4)
            {
                SimplePattern p1 = new SimplePattern(AMMO_MOTOR_NUMBER + 3 * AMMO_MOTOR_DIRECTION, new int[] { 1000 }, new double[] { minAmplitude });
                return p1;
            }
            else if (ammoLevel > 2.0 / 4)
            {
                SimplePattern p1 = new SimplePattern(AMMO_MOTOR_NUMBER + 2 * AMMO_MOTOR_DIRECTION, new int[] { 620 }, new double[] { (1.0 - minAmplitude) / 3 + minAmplitude });
                return p1;
            }
            else if (ammoLevel > 1.0 / 4)
            {
                SimplePattern p1 = new SimplePattern(AMMO_MOTOR_NUMBER + 1 * AMMO_MOTOR_DIRECTION, new int[] { 420 }, new double[] { ((1.0 - minAmplitude) / 3) * 2 + minAmplitude });
                return p1;
            }
            else
            {
                SimplePattern p1 = new SimplePattern(AMMO_MOTOR_NUMBER + 0 * AMMO_MOTOR_DIRECTION, new int[] { 190 }, new double[] { 1.0 });
                return p1;
            }
        }

        public static IPattern AMMO_PATTERN_8(double ammoLevel)
        {
            int numOfLevels = 10;
            int ammoLvl = (int)Math.Max(Math.Ceiling(ammoLevel * numOfLevels), 1);
            SimplePattern preVibration = new SimplePattern(AMMO_MOTOR_NUMBER, new int[] { 700, 200 }, new double[] { 1.0, 0.0 });
            if (ammoLvl < 5)
            {
                return ConcatenatePatterns(preVibration, SimpleVibrations(AMMO_MOTOR_NUMBER, 150, 200, ammoLvl));
            }
            else if (ammoLvl % 5 == 0)
            {
                return ConcatenatePatterns(preVibration, SimpleVibrations(AMMO_MOTOR_NUMBER, 600, 200, ammoLvl / 5));
            }
            else
            {
                // preVibration + long Vibrations + pause + short Vibrations
                return ConcatenatePatterns(
                    preVibration,
                    SimpleVibrations(AMMO_MOTOR_NUMBER, 600, 200, ammoLvl / 5),
                    new SimplePattern(AMMO_MOTOR_NUMBER, new int[] { 200 }, new double[] { 0.0 })
                    , SimpleVibrations(AMMO_MOTOR_NUMBER, 150, 200, ammoLvl % 5));
            }
        }

        public static IPattern AMMO_PATTERN_9(double ammoLevel, int vibrationDuration)
        {
            int numOfLevels = 4;
            int ammoLvl = (int)Math.Max(Math.Ceiling(ammoLevel * numOfLevels), 1);
            SimplePattern result = SimpleVibrations(AMMO_MOTOR_NUMBER, vibrationDuration, 100, ammoLvl);
            if (ammoLevel > 3.0 / 4)
            {
                result.SetMotor(AMMO_MOTOR_NUMBER + 3 * AMMO_MOTOR_DIRECTION);
            }
            else if (ammoLevel > 2.0 / 4)
            {
                result.SetMotor(AMMO_MOTOR_NUMBER + 2 * AMMO_MOTOR_DIRECTION);
            }
            else if (ammoLevel > 1.0 / 4)
            {
                result.SetMotor(AMMO_MOTOR_NUMBER + 1 * AMMO_MOTOR_DIRECTION);
            }
            else
            {
                result.SetMotor(AMMO_MOTOR_NUMBER + 0 * AMMO_MOTOR_DIRECTION);
            }
            return result;
        }

        public static IPattern AMMO_PATTERN_10(double ammoLevel, int vibrationDuration)
        {
            int numOfLevels = 4;
            int ammoLvl = (int)Math.Max(Math.Ceiling(ammoLevel * numOfLevels), 1);
            SimplePattern result;
            if (ammoLvl <= 2)
            {
                result = SimpleVibrations(AMMO_MOTOR_NUMBER, vibrationDuration, 100, ammoLvl);
            }
            else
            {
                SimplePattern vibration1 = SimpleVibrations(AMMO_MOTOR_NUMBER, vibrationDuration, 100, 2);
                SimplePattern pause = new SimplePattern(AMMO_MOTOR_NUMBER, new int[] { 250 }, new double[] { 0.0 });
                SimplePattern vibration2 = SimpleVibrations(AMMO_MOTOR_NUMBER, vibrationDuration, 100, ammoLvl - 2);
                result = ConcatenatePatterns(vibration1, pause, vibration2);
            }
            if (ammoLevel > 3.0 / 4)
            {
                result.SetMotor(AMMO_MOTOR_NUMBER + 3 * AMMO_MOTOR_DIRECTION);
            }
            else if (ammoLevel > 2.0 / 4)
            {
                result.SetMotor(AMMO_MOTOR_NUMBER + 2 * AMMO_MOTOR_DIRECTION);
            }
            else if (ammoLevel > 1.0 / 4)
            {
                result.SetMotor(AMMO_MOTOR_NUMBER + 1 * AMMO_MOTOR_DIRECTION);
            }
            else
            {
                result.SetMotor(AMMO_MOTOR_NUMBER + 0 * AMMO_MOTOR_DIRECTION);
            }
            return result;
        }

        private static int HealthToBPM(double health)
        {
            int minBPM = 45;
            int maxBPM = 166;
            int bpm = (int)Math.Round(minBPM + (1.0 - health) * (maxBPM - minBPM));
            return Math.Max(minBPM, Math.Min(maxBPM, bpm));
        }
        private static SimplePattern ConcatenatePatterns(SimplePattern p1, SimplePattern p2, SimplePattern p3, SimplePattern p4)
        {
            return ConcatenatePatterns(ConcatenatePatterns(p1, p2, p3), p4);
        }

        private static SimplePattern ConcatenatePatterns(SimplePattern p1, SimplePattern p2, SimplePattern p3)
        {
            return ConcatenatePatterns(ConcatenatePatterns(p1, p2), p3);
        }

        private static SimplePattern ConcatenatePatterns(SimplePattern p1, SimplePattern p2)
        {
            int[] timings = new int[p1.GetTimings().Length + p2.GetTimings().Length];
            double[] amplitudes = new double[p1.GetAmplitudes().Length + p2.GetAmplitudes().Length];
            for (int i = 0; i < p1.GetTimings().Length; i++)
            {
                timings[i] = p1.GetTimings()[i];
                amplitudes[i] = p1.GetAmplitudes()[i];
            }
            int l1 = p1.GetTimings().Length;
            for (int i = l1; i < timings.Length; i++)
            {
                timings[i] = p2.GetTimings()[i - l1];
                amplitudes[i] = p2.GetAmplitudes()[i - l1];
            }
            return new SimplePattern(p1.GetMotor(), timings, amplitudes);
        }

        public static SimplePattern HEALT_PATTERN_1(double health, int vibrationDuration)
        {
            int bpm = HealthToBPM(health);
            int intervalTime = (int)Math.Round((60.0 * 1000) / bpm);
            vibrationDuration = Math.Min(vibrationDuration, intervalTime);
            SimplePattern pattern = new SimplePattern(HEALTH_MOTOR_NUMBER, new int[] { vibrationDuration, intervalTime - vibrationDuration }, new double[] { 1.0, 0.0 });
            return pattern;
        }

        public static SimplePattern HEALT_PATTERN_1b(double health, int vibrationDuration)
        {
            int bpm = HealthToBPM(health);
            int intervalTime = (int)Math.Round((60.0 * 1000) / bpm);
            vibrationDuration = Math.Min(vibrationDuration, intervalTime);
            SimplePattern wave = GetSineWavePattern(vibrationDuration * 2, 0, 1, 0, vibrationDuration, 50);
            SimplePattern pattern = ConcatenatePatterns(wave, new SimplePattern(new int[] { intervalTime - vibrationDuration }, new double[] { 0.0 }));
            pattern.SetMotor(HEALTH_MOTOR_NUMBER);
            return pattern;
        }

        public static SimplePattern HEALT_PATTERN_1c(double health)
        {
            int bpm = HealthToBPM(health);
            int intervalTime = (int)Math.Round((60.0 * 1000) / bpm);
            SimplePattern pattern = GetSineWavePattern(intervalTime, 0.01, 1, -Math.PI / 2, intervalTime, 50);
            pattern.SetMotor(HEALTH_MOTOR_NUMBER);
            return pattern;
        }

        public static IPattern HEALT_PATTERN_1d(double health, int vibrationDuration)
        {
            int bpm = HealthToBPM(health);
            int intervalTime = (int)Math.Round((60.0 * 1000) / bpm);
            vibrationDuration = Math.Min(vibrationDuration, intervalTime);
            SimplePattern p1 = new SimplePattern(HEALTH_MOTOR_NUMBER + 3 * HEALTH_MOTOR_DIRECTION, new int[] { vibrationDuration, intervalTime * 3 - vibrationDuration }, new double[] { 1.0, 0.0 });
            SimplePattern p2 = new SimplePattern(HEALTH_MOTOR_NUMBER + 2 * HEALTH_MOTOR_DIRECTION, new int[] { intervalTime, vibrationDuration, intervalTime * 2 - vibrationDuration }, new double[] { 0.0, 1.0, 0.0 });
            SimplePattern p3 = new SimplePattern(HEALTH_MOTOR_NUMBER + 1 * HEALTH_MOTOR_DIRECTION, new int[] { intervalTime * 2, vibrationDuration, intervalTime - vibrationDuration }, new double[] { 0.0, 1.0, 0.0 });
            MultiPattern pattern = new MultiPattern(p1, p2, p3);
            return pattern;
        }

        public static SimplePattern HEALT_PATTERN_2(double health)
        {
            int hp = (int)Math.Round(health * 100);
            SimplePattern pattern = new SimplePattern(HEALTH_MOTOR_NUMBER, new int[] { 80, 3 * hp + 60, 90, (int)Math.Round(6.5 * hp + 130) }, new double[] { 1.0, 0.0, 1.0, 0.0 });
            return pattern;
        }
        //public static IPattern HEALT_PATTERN_2(double health)
        //{
        //    int hp = (int)Math.Round(health * 100);
        //    SimplePattern p1 = new SimplePattern(HEALTH_MOTOR_NUMBER, new int[] { 80, 3 * hp + 60, 90, (int)Math.Round(6.5 * hp + 130) }, new double[] { 1.0, 0.0, 1.0, 0.0 });
        //    SimplePattern p2 = p1.Copy();
        //    p2.SetMotor(HEALTH_MOTOR_NUMBER + 1 * HEALTH_MOTOR_DIRECTION);
        //    SimplePattern p3 = p2.Copy();
        //    p3.SetMotor(HEALTH_MOTOR_NUMBER + 2 * HEALTH_MOTOR_DIRECTION);
        //    SimplePattern p4 = p3.Copy();
        //    p4.SetMotor(HEALTH_MOTOR_NUMBER + 3 * HEALTH_MOTOR_DIRECTION);
        //    return new MultiPattern(p1, p2, p3, p4);
        //}
        public static SimplePattern HEALT_PATTERN_2b(double health)
        {
            int hp = (int)Math.Round(health * 100);
            SimplePattern pattern = new SimplePattern(HEALTH_MOTOR_NUMBER, new int[] { 160, 3 * hp + 120, 180, (int)Math.Round(6.5 * hp + 260) }, new double[] { 1.0, 0.0, 1.0, 0.0 });
            return pattern;
        }

        public static IPattern HEALT_PATTERN_3(double health, SimplePattern pattern)
        {
            SimplePattern copy = pattern.Copy();
            copy.SetMotor(HEALTH_MOTOR_NUMBER);
            if (health > 0.75)
            {
                return copy;
            }
            else if (health > 0.5)
            {
                SimplePattern copy2 = copy.Copy();
                copy2.SetMotor(HEALTH_MOTOR_NUMBER + 1 * HEALTH_MOTOR_DIRECTION);
                return new MultiPattern(copy, copy2);
            }
            else if (health > 0.25)
            {
                SimplePattern copy2 = copy.Copy();
                copy2.SetMotor(HEALTH_MOTOR_NUMBER + 1 * HEALTH_MOTOR_DIRECTION);
                SimplePattern copy3 = copy.Copy();
                copy3.SetMotor(HEALTH_MOTOR_NUMBER + 2 * HEALTH_MOTOR_DIRECTION);
                return new MultiPattern(copy, copy2, copy3);
            }
            else
            {
                SimplePattern copy2 = copy.Copy();
                copy2.SetMotor(HEALTH_MOTOR_NUMBER + 1 * HEALTH_MOTOR_DIRECTION);
                SimplePattern copy3 = copy.Copy();
                copy3.SetMotor(HEALTH_MOTOR_NUMBER + 2 * HEALTH_MOTOR_DIRECTION);
                SimplePattern copy4 = copy.Copy();
                copy4.SetMotor(HEALTH_MOTOR_NUMBER + 3 * HEALTH_MOTOR_DIRECTION);
                return new MultiPattern(copy, copy2, copy3, copy4);
            }
        }

        private static int StaminaToBPM(double health)
        {
            int minBPM = 10;
            int maxBPM = 30;
            int bpm = (int)Math.Round(minBPM + (1.0 - health) * (maxBPM - minBPM));
            return Math.Max(minBPM, Math.Min(maxBPM, bpm));
        }

        public static SimplePattern STAMINA_PATTERN_STUDY_2(double stamina)
        {
            int bpm = HealthToBPM(stamina);
            int intervalTime = (int)Math.Round((60.0 * 1000) / bpm);
            SimplePattern pattern = GetSineWavePattern(intervalTime, 0.01, 1, -Math.PI / 2, intervalTime, 50);
            pattern.SetMotor(STAMINA_MOTOR_NUMBER);
            return pattern;
        }

        public static SimplePattern STAMINA_PATTERN_1(double stamina)
        {
            int bpm = StaminaToBPM(stamina);
            int intervalTime = (int)Math.Round((60.0 * 1000) / bpm);
            SimplePattern pattern = GetSineWavePattern(intervalTime, 0.01, 1, -Math.PI / 2, intervalTime, 50);
            pattern.SetMotor(STAMINA_MOTOR_NUMBER);
            return pattern;
        }

        public static SimplePattern STAMINA_PATTERN_2(double staminaChange)
        {
            if (staminaChange == 0)
            {
                return new SimplePattern(STAMINA_MOTOR_NUMBER, new int[] { 500 }, new double[] { 0.0 });
            }
            bool ascending = staminaChange > 0;
            if (ascending)
            {
                return new SimplePattern(STAMINA_MOTOR_NUMBER, new int[] { 300, 150, 150, 400 }, new double[] { 1.0, 0.0, 1.0, 0.0 });
            }
            else
            {
                return new SimplePattern(STAMINA_MOTOR_NUMBER, new int[] { 300, 150 }, new double[] { 1.0, 0.0 });
            }
        }

        public static SimplePattern STAMINA_PATTERN_3(double staminaChange)
        {
            if (staminaChange == 0)
            {
                return new SimplePattern(STAMINA_MOTOR_NUMBER, new int[] { 1000 }, new double[] { 0.0 });
            }
            bool ascending = staminaChange > 0;
            if (ascending)
            {
                return new SimplePattern(STAMINA_MOTOR_NUMBER, new int[] { 150, 150, 150, 550 }, new double[] { 0.5, 0.75, 1.0, 0.0 });
            }
            else
            {
                return new SimplePattern(STAMINA_MOTOR_NUMBER, new int[] { 150, 150, 150, 550 }, new double[] { 1.0, 0.75, 0.5, 0.0 });
            }
        }

        public static IPattern STAMINA_PATTERN_4(double staminaChange)
        {
            if (staminaChange == 0)
            {
                return new SimplePattern(STAMINA_MOTOR_NUMBER, new int[] { 2200 }, new double[] { 0.0 });
            }
            bool ascending = staminaChange > 0;
            if (ascending)
            {
                SimplePattern p1 = new SimplePattern(STAMINA_MOTOR_NUMBER + 2 * STAMINA_MOTOR_DIRECTION, new int[] { 900 + 300 + 200 + 300, 200, 300 }, new double[] { 0.0, 1.0, 0.0 });
                SimplePattern p2 = new SimplePattern(STAMINA_MOTOR_NUMBER + 1 * STAMINA_MOTOR_DIRECTION, new int[] { 900 + 300, 200, 300 + 200 + 300 }, new double[] { 0.0, 1.0, 0.0 });
                SimplePattern p3 = new SimplePattern(STAMINA_MOTOR_NUMBER + 0 * STAMINA_MOTOR_DIRECTION, new int[] { 900, 300 + 200 + 300 + 200 + 300 }, new double[] { 1.0, 0.0 });
                MultiPattern pattern = new MultiPattern(p1, p2, p3);
                return pattern;
            }
            else
            {
                SimplePattern p1 = new SimplePattern(STAMINA_MOTOR_NUMBER + 2 * STAMINA_MOTOR_DIRECTION, new int[] { 900, 300 + 200 + 300 + 200 + 300 }, new double[] { 1.0, 0.0 });
                SimplePattern p2 = new SimplePattern(STAMINA_MOTOR_NUMBER + 1 * STAMINA_MOTOR_DIRECTION, new int[] { 900 + 300, 200, 300 + 200 + 300 }, new double[] { 0.0, 1.0, 0.0 });
                SimplePattern p3 = new SimplePattern(STAMINA_MOTOR_NUMBER + 0 * STAMINA_MOTOR_DIRECTION, new int[] { 900 + 300 + 200 + 300, 200, 300 }, new double[] { 0.0, 1.0, 0.0 });
                MultiPattern pattern = new MultiPattern(p1, p2, p3);
                return pattern;
            }
        }

        public static IPattern STAMINA_PATTERN_5(double staminaChange)
        {
            int t1 = 200;
            int t2 = 300;
            int t3 = 1300;
            int duration = t1 + t2 + t1 + t3;

            if (staminaChange == 0)
            {
                return new SimplePattern(STAMINA_MOTOR_NUMBER, new int[] { duration }, new double[] { 0.0 });
            }
            bool ascending = staminaChange > 0;

            if (ascending)
            {
                int bumpDuration = (int)Math.Round((double)duration / 2);
                SimplePattern wave = GetSineWavePattern(bumpDuration, 0, 1, -Math.PI / 2, bumpDuration, 100);
                wave.SetMotor(STAMINA_MOTOR_NUMBER + 2 * STAMINA_MOTOR_DIRECTION);
                SimplePattern p1 = ConcatenatePatterns(new SimplePattern(STAMINA_MOTOR_NUMBER + 2 * STAMINA_MOTOR_DIRECTION, new int[] { duration - bumpDuration }, new double[] { 0.0 }), wave.Copy());

                int p2waitTime = (int)Math.Round(bumpDuration * 2.0 / 4);
                SimplePattern p2Wait = new SimplePattern(STAMINA_MOTOR_NUMBER + 1 * STAMINA_MOTOR_DIRECTION, new int[] { p2waitTime }, new double[] { 0.0 });
                wave.SetMotor(STAMINA_MOTOR_NUMBER + 1 * STAMINA_MOTOR_DIRECTION);
                SimplePattern p2 = ConcatenatePatterns(p2Wait.Copy(), wave.Copy(), p2Wait.Copy());

                wave.SetMotor(STAMINA_MOTOR_NUMBER + 0 * STAMINA_MOTOR_DIRECTION);
                SimplePattern p3 = ConcatenatePatterns(wave.Copy(), new SimplePattern(STAMINA_MOTOR_NUMBER + 0 * STAMINA_MOTOR_DIRECTION, new int[] { duration - bumpDuration }, new double[] { 0.0 }));

                MultiPattern pattern = new MultiPattern(p1, p2, p3);
                return pattern;
            }
            else
            {
                SimplePattern p1 = new SimplePattern(STAMINA_MOTOR_NUMBER + 2 * STAMINA_MOTOR_DIRECTION, new int[] { t1, t2 + t1 + t3 }, new double[] { 1.0, 0.0 });
                SimplePattern p2 = new SimplePattern(STAMINA_MOTOR_NUMBER + 1 * STAMINA_MOTOR_DIRECTION, new int[] { t1, t2, t1, t3 }, new double[] { 1.0, 0.0, 1.0, 0.0 });
                SimplePattern p3 = new SimplePattern(STAMINA_MOTOR_NUMBER + 0 * STAMINA_MOTOR_DIRECTION, new int[] { t1 + t2, t1, t3 }, new double[] { 0.0, 1.0, 0.0 });
                MultiPattern pattern = new MultiPattern(p1, p2, p3);
                return pattern;
            }
        }

        public static long GetMaxDuration(List<SimplePattern> patternList)
        {
            long duration = 0;
            foreach (SimplePattern pattern in patternList)
            {
                duration = Math.Max(duration, pattern.GetDuration());
            }
            return duration;
        }
    }
}
