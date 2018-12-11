using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TactPlay
{
    class LogFileHandler : ILogFileHandler
    {
        private AmmunitionEventHandler ammunitionEventHandler;
        private HealthEventHandler healthEventHandler;
        private StaminaEventHandler staminaEventHandler;

        public LogFileHandler(AmmunitionEventHandler ammunitionEventHandler, HealthEventHandler healthEventHandler, StaminaEventHandler staminaEventHandler)
        {
            this.ammunitionEventHandler = ammunitionEventHandler;
            this.healthEventHandler = healthEventHandler;
            this.staminaEventHandler = staminaEventHandler;
        }

        /**
         * Format:
         * Timestamp "TactPlay Category Event Status"
         * Category{Event}: Ammunition{EventFired, EventReloaded}, Damage{EventDamaged}, Stamina{EventTimer},
         * Dialog{opened, closed}
         * TODO Add other events
         *
         * @param line
         */
        public void NewLine(string line)
        {

            String mLine = line.Replace("\"", "");
            while (mLine.Length > 8 && mLine.StartsWith(" "))
            {
                mLine = mLine.Substring(1);
            }
            if (mLine != null && mLine.Split(' ').Length >= 5 && mLine.Split(' ')[1].Equals("TactPlay"))
            {
                Logger.WriteLogLine("LogFileHandler.log", mLine);
                if(mLine.Contains("Challenge Started") || mLine.Contains("Challenge Finished"))
                {
                    Logger.WriteLogLine("Ammunition.log", mLine);
                    Logger.WriteLogLine("Stamina.log", mLine);
                }
                //System.out.println(line);
                String[] splits = mLine.Split(' ');
                String myCategory = splits[2];
                String myEvent = splits[3];
                String status = splits[4];
                switch (myCategory)
                {
                    case "Ammunition":
                        switch (myEvent)
                        {
                            case "EventFired":
                                ammunitionEventHandler.EventFired(int.Parse(status.Split('/')[0]), int.Parse(status.Split('/')[1]));
                                break;
                            case "EventReloaded":
                                ammunitionEventHandler.EventReloaded(int.Parse(status.Split('/')[0]), int.Parse(status.Split('/')[1]));
                                break;
                            default:
                                Console.Error.WriteLine("Unknown Event " + myEvent + " in line >" + mLine + "<");
                                break;
                        }
                        break;
                    case "Damage":
                        switch (myEvent)
                        {
                            case "EventDamaged":
                                healthEventHandler.EventDamaged(status.ToLower(), Double.Parse(splits[5], CultureInfo.InvariantCulture));
                                break;
                            case "EventTimer":
                                healthEventHandler.EventTimer(status.ToLower(), Double.Parse(splits[5], CultureInfo.InvariantCulture));
                                break;
                            default:
                                Console.Error.WriteLine("Unknown Event " + myEvent + " in line >" + mLine + "<");
                                break;
                        }
                        break;
                    case "Stamina":
                        switch (myEvent)
                        {
                            case "EventTimer":
                                staminaEventHandler.EventTimer(Double.Parse(status.Split('/')[0], CultureInfo.InvariantCulture), Double.Parse(status.Split('/')[1], CultureInfo.InvariantCulture));
                                break;
                            default:
                                Console.Error.WriteLine("Unknown Event " + myEvent + " in line >" + mLine + "<");
                                break;
                        }
                        break;
                    case "Dialog":
                        switch (myEvent)
                        {
                            case "Opened":
                                switch (status)
                                {
                                    case "Ammunition":
                                        ammunitionEventHandler.OnDialogOpened();
                                        break;
                                    case "Stamina":
                                        staminaEventHandler.OnDialogOpened();
                                        break;
                                    default:
                                        Console.Error.WriteLine("Unknown Status " + status + " in line >" + mLine + "<");
                                        break;
                                }
                                break;
                            case "Closed":
                                switch (status)
                                {
                                    case "Ammunition":
                                        ammunitionEventHandler.OnDialogClosed(splits[5]);
                                        break;
                                    case "Stamina":
                                        staminaEventHandler.OnDialogClosed(splits[5]);
                                        break;
                                    default:
                                        Console.Error.WriteLine("Unknown Status " + status + " in line >" + mLine + "<");
                                        break;
                                }
                                break;
                            default:
                                Console.Error.WriteLine("Unknown Event " + myEvent + " in line >" + mLine + "<");
                                break;
                        }
                        break;
                    default:
                        Console.Error.WriteLine("Unknown Category " + myCategory + " in line >" + mLine + "<");
                        break;
                }
            }
        }

    }
}

