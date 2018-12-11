using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TactPlay.Connection;
using TactPlay.Pattern;

namespace TactPlay.Device
{
    class BLEVibrationDeviceMultipleThreads : IVibrationDevice
    {
        private int numberOfMotors;
        private MotorThread[] motorThreads;
        private readonly object[] motorLocks;
        private BLEMotor[] motors;
        private readonly object syncLock = new object();
        private static readonly int NUMBER_OF_MOTORS = 4;

        public BLEVibrationDeviceMultipleThreads(BLEConnection bleConnection)
        {
            this.numberOfMotors = NUMBER_OF_MOTORS;
            motorThreads = new MotorThread[numberOfMotors];
            motorLocks = new object[numberOfMotors];
            motors = new BLEMotor[numberOfMotors];
            for (int i = 0; i < motors.Length; i++)
            {
                motors[i] = new BLEMotor(bleConnection, 4 + i);
            }
        }


        public void PlayPattern(IPattern pattern)
        {
            foreach (SimplePattern p in pattern.GetSimplePatterns())
            {
                int motor = p.GetMotor();
                if (motor >= numberOfMotors || motor < 0)
                {
                    Console.Error.WriteLine("Invalid motor id: " + motor + ". Vibration device has " + numberOfMotors + " motorThreads.");
                }
                StartPatternOnMotor(motor, p);
            }
        }

        private void StartPatternOnMotor(int motorNumber, SimplePattern pattern)
        {
            lock (syncLock)
            {
                if (motorThreads[motorNumber] == null)
                {
                    motorThreads[motorNumber] = new MotorThread(motors[motorNumber]);
                }
            }
            MotorThread thread = motorThreads[motorNumber];
            lock (motorThreads[motorNumber])
            {
                thread.StartPattern(pattern);
            }
        }

        public int GetNumberOfMotors()
        {
            return numberOfMotors;
        }
    }
}
