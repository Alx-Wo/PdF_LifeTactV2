using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using TactPlay.Pattern;

namespace TactPlay
{
    class AmmunitionEventHandler
    {
        private static readonly double AMMO_DIVIDER = 3;
        private int currentAmmo = -1;
        private int maxAmmo = -1;
        private long lastNotificationTime;
        private long lastVibrationBeforeDialogTime = 0;
        private int lastAmmo = -1;
        private int lastAmmoBeforeDialog = -1;
        private long dialogOpenedTime = 0;
        private long lastPlayerActiopnTime = 0;
        private System.Timers.Timer timer;
        private bool enabled = false;

        public AmmunitionEventHandler()
        {
            lastNotificationTime = Environment.TickCount;
            timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = 100;
        }

        public void OnDialogOpened()
        {
            lastVibrationBeforeDialogTime = lastNotificationTime;
            lastAmmoBeforeDialog = lastAmmo;
            dialogOpenedTime = Environment.TickCount;
            Logger.WriteLogLine("Ammunition.log", "AmmunitionOpened");
        }

        public void OnDialogClosed(String userInput)
        {
            Logger.WriteLogLine("Ammunition.log", "AmmunitionClosed\t" +
                "Eingabe: \t" + userInput + "\t" +
                "AktuellerWert: \t" + currentAmmo + "\t" +
                "LetzterWertVorOpened: \t" + lastAmmoBeforeDialog + "\t" +
                "ZeitZwischenOpenedUndClosed: \t" + (Environment.TickCount - dialogOpenedTime) + "\t" +
                "ZeitZwischenLetzterVibrationUndOpened: \t" + (dialogOpenedTime - lastVibrationBeforeDialogTime));
        }

        public void EventFired(int currentAmmo, int maxAmmo)
        {
            if (!enabled)
            {
                return;
            }
            Logger.WriteLogLine("Ammunition.log", "EventFired\t" +
                "CurrentAmmo: \t" + currentAmmo + "\t" +
                "MaxAmmo: \t" + maxAmmo + "\t");
            PlayerAction(currentAmmo, maxAmmo);
            for (int i = 0; i < AMMO_DIVIDER + 1; i++)
            {
                double difference = ((i / AMMO_DIVIDER) - ((double)currentAmmo / maxAmmo));
                if (difference <= (0.99 / maxAmmo) && difference >= -0.001)
                {
                    //PlayVibration(); //TODO maybe remove this
                }

            }
            //Main.vibrationDevice.PlayPattern(new SimplePattern(new int[] { 200 }, new double[] { 1.0 }));
        }

        public void EventReloaded(int currentAmmo, int maxAmmo)
        {
            if (!enabled)
            {
                return;
            }
            Logger.WriteLogLine("Ammunition.log", "EventReloaded\t" +
                "CurrentAmmo: \t" + currentAmmo + "\t" +
                "MaxAmmo: \t" + maxAmmo + "\t");
            PlayerAction(currentAmmo, maxAmmo);
            PlayVibration();
        }

        private void PlayerAction(int currentAmmo, int maxAmmo)
        {
            this.currentAmmo = currentAmmo;
            this.maxAmmo = maxAmmo;
            this.lastPlayerActiopnTime = Environment.TickCount;
        }

        private void PlayVibration()
        {
            lastNotificationTime = Environment.TickCount;
            lastAmmo = currentAmmo;
            double ammoToDisplay = (double)lastAmmo / maxAmmo;
            //Console.WriteLine("Ammo Status " + currentAmmo + "/" + maxAmmo);
            //Main.vibrationDevice.PlayPattern(new SimplePattern(new int[] { 1000 }, new double[] { 1.0 }));
            //Main.vibrationDevice.PlayPattern(Utils.GetSineWavePattern(5000,0.1,1,-Math.PI/2,5000,250));
            Main.vibrationDevice.PlayPattern(Utils.AMMO_PATTERN_STUDY_2(ammoToDisplay));
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (!enabled)
            {
                return;
            }
            if (Environment.TickCount - Math.Max(lastNotificationTime, 0) >= 3000 && currentAmmo != -1 && maxAmmo != -1)
            {
                PlayVibration();
            }
        }

        internal void EventFired(object p1, object p2)
        {
            throw new NotImplementedException();
        }

        public void Enable()
        {
            enabled = true;
            timer.Start();
        }

        public void Disable()
        {
            enabled = false;
            timer.Stop();
        }
    }
}
