using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using TactPlay.Pattern;

namespace TactPlay
{
    public partial class PatternTester : Form
    {
        private IPattern ammoPattern = null;
        private long nextAmmoTime = 0;

        private IPattern healthPattern = null;
        private long nextHealthTime = 0;

        private IPattern staminaPattern = null;
        private long nextStaminaTime = 0;

        private System.Timers.Timer timer;

        private int numberOfGroupsForTest = -1;

        private Random random = new Random();

        private string logName = "Study";

        public PatternTester()
        {
            InitializeComponent();

            Setup();
        }

        private void Setup()
        {
            object[] comboBox1Items = new object[] { "Pattern Study 2", "Pattern 1", "Pattern 1b", "Pattern 2b", "Pattern 2c", "Pattern 3s", "Pattern 3l"
                , "Pattern 4s", "Pattern 4l", "Pattern 5", "Pattern 6", "Pattern 7", "Pattern 8", "Pattern 9", "Pattern 10" };
            comboBox1.Items.AddRange(comboBox1Items);
            comboBox1.SelectedItem = comboBox1Items[0];

            object[] comboBox2Items = new object[] { "Pattern 1", "Pattern 1b", "Pattern 1c", "Pattern 1d", "Pattern 2", "Pattern 2b", "Pattern 3", "Test Motor 1", "Test Motor 2", "Test Motor 3", "Test Motor 4", "Test Motor 5", "Test Motor 6", "Test Motor 7", "Test Motor 8" };
            comboBox2.Items.AddRange(comboBox2Items);
            comboBox2.SelectedItem = comboBox2Items[0];

            object[] comboBox3Items = new object[] { "Pattern Study 2", "Pattern 1", "Pattern 2", "Pattern 3", "Pattern 4", "Pattern 5" };
            comboBox3.Items.AddRange(comboBox3Items);
            comboBox3.SelectedItem = comboBox3Items[0];

            timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = 20;
            timer.Start();
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (checkBox1.CheckState == CheckState.Checked && nextAmmoTime <= Environment.TickCount)
            {
                nextAmmoTime = Environment.TickCount + (long)numericUpDown3.Value;
                PlayPattern(ammoPattern, ammoPattern);
            }

            if (checkBox2.CheckState == CheckState.Checked && nextHealthTime <= Environment.TickCount)
            {
                nextHealthTime = Environment.TickCount + (long)numericUpDown4.Value;
                PlayPattern(healthPattern, healthPattern);
            }

            if (checkBox3.CheckState == CheckState.Checked && nextStaminaTime <= Environment.TickCount)
            {
                nextStaminaTime = Environment.TickCount + (long)numericUpDown7.Value;
                PlayPattern(staminaPattern, staminaPattern);
            }
        }

        // Return true if the oldPattern has been updated
        private bool PlayPattern(IPattern oldPattern, IPattern newPattern)
        {
            if (newPattern == null)
            {
                return false;
            }
            if (oldPattern != null && oldPattern.GetType() == typeof(RepeatablePattern))
            {
                if (newPattern.GetType() == typeof(RepeatablePattern) && ((RepeatablePattern)oldPattern).IsRepeating())
                {
                    //new pattern is also repeatable, so old one can be updated
                    List<IPattern> patterns = new List<IPattern>();
                    patterns.AddRange(newPattern.GetSimplePatterns());
                    MultiPattern mp = new MultiPattern(patterns);
                    ((RepeatablePattern)oldPattern).UpdatePattern(mp);
                    return true;
                }
                else
                {
                    //new pattern is not repeatable, so old one needs to be stopt and replaced
                    ((RepeatablePattern)oldPattern).Stop();
                    Main.vibrationDevice.PlayPattern(newPattern);
                    return false;
                }
            }
            else
            {
                Main.vibrationDevice.PlayPattern(newPattern);
                return false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double ammoLevel = (double)numericUpDown1.Value / (double)numericUpDown1.Maximum;
            UpdateAmmoLevel(ammoLevel);
        }

        private void UpdateAmmoLevel(double ammoLevel)
        {
            int vibrationDuration = (int)numericUpDown2.Value;

            String selection = comboBox1.SelectedItem.ToString();
            UpdateAmmoPattern(selection, ammoLevel, vibrationDuration);
            accuracyTestTextBox.Text = "Ammo" + ":" + selection;
        }

        private void UpdateAmmoPattern(String selection, double ammoLevel, int vibrationDuration)
        {
            IPattern newPattern = null;
            switch (selection)
            {
                case "Pattern Study 2":
                    newPattern = Utils.AMMO_PATTERN_STUDY_2(ammoLevel);
                    numberOfGroupsForTest = -1;
                    break;
                case "Pattern 1":
                    newPattern = Utils.AMMO_PATTERN_1(ammoLevel, vibrationDuration);
                    numberOfGroupsForTest = 4;
                    break;
                case "Pattern 1b":
                    newPattern = Utils.AMMO_PATTERN_1b(ammoLevel, vibrationDuration);
                    numberOfGroupsForTest = 4;
                    break;
                case "Pattern 2b":
                    newPattern = Utils.AMMO_PATTERN_2b(ammoLevel, vibrationDuration);
                    numberOfGroupsForTest = 4;
                    break;
                case "Pattern 2c":
                    newPattern = Utils.AMMO_PATTERN_2c(ammoLevel, vibrationDuration);
                    numberOfGroupsForTest = 4;
                    break;
                case "Pattern 3s":
                    newPattern = Utils.AMMO_PATTERN_3s(ammoLevel, Utils.SIMPLE_VIBRATION(vibrationDuration));
                    numberOfGroupsForTest = 4;
                    break;
                case "Pattern 3l":
                    newPattern = Utils.AMMO_PATTERN_3l(ammoLevel, Utils.SIMPLE_VIBRATION(vibrationDuration));
                    numberOfGroupsForTest = 4;
                    break;
                case "Pattern 4s":
                    newPattern = Utils.AMMO_PATTERN_4s(ammoLevel, Utils.SIMPLE_VIBRATION(vibrationDuration));
                    numberOfGroupsForTest = 4;
                    break;
                case "Pattern 4l":
                    newPattern = Utils.AMMO_PATTERN_4l(ammoLevel, Utils.SIMPLE_VIBRATION(vibrationDuration));
                    numberOfGroupsForTest = 4;
                    break;
                case "Pattern 5":
                    newPattern = Utils.AMMO_PATTERN_5(ammoLevel, vibrationDuration);
                    numberOfGroupsForTest = 30;
                    break;
                case "Pattern 6":
                    newPattern = Utils.AMMO_PATTERN_6(ammoLevel);
                    numberOfGroupsForTest = 4;
                    break;
                case "Pattern 7":
                    newPattern = Utils.AMMO_PATTERN_7(ammoLevel, 0.25);
                    numberOfGroupsForTest = 4;
                    break;
                case "Pattern 8":
                    newPattern = Utils.AMMO_PATTERN_8(ammoLevel);
                    numberOfGroupsForTest = 10;
                    break;
                case "Pattern 9":
                    newPattern = Utils.AMMO_PATTERN_9(ammoLevel, vibrationDuration);
                    numberOfGroupsForTest = 4;
                    break;
                case "Pattern 10":
                    newPattern = Utils.AMMO_PATTERN_10(ammoLevel, vibrationDuration);
                    numberOfGroupsForTest = 4;
                    break;
                default:
                    break;
            }
            if (newPattern != null)
            {
                nextAmmoTime = Environment.TickCount + (long)numericUpDown3.Value;
                bool result = PlayPattern(ammoPattern, newPattern);
                if (!result)
                {
                    ammoPattern = newPattern;
                }
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            double health = (double)numericUpDown6.Value / (double)numericUpDown6.Maximum;
            UpdateHealthLevel(health);
        }

        private void UpdateHealthLevel(double health)
        {
            int vibrationDuration = (int)numericUpDown5.Value;

            String selection = comboBox2.SelectedItem.ToString();
            UpdateHealthPattern(selection, health, vibrationDuration);
            accuracyTestTextBox.Text = "Health" + ":" + selection;
        }

        private void UpdateHealthPattern(String selection, double health, int vibrationDuration)
        {
            IPattern newPattern = null;
            switch (selection)
            {
                case "Pattern 1":
                    newPattern = new RepeatablePattern(Utils.HEALT_PATTERN_1(health, vibrationDuration));
                    numberOfGroupsForTest = -1;
                    break;
                case "Pattern 1b":
                    newPattern = new RepeatablePattern(Utils.HEALT_PATTERN_1b(health, vibrationDuration));
                    numberOfGroupsForTest = -1;
                    break;
                case "Pattern 1c":
                    newPattern = new RepeatablePattern(Utils.HEALT_PATTERN_1c(health));
                    numberOfGroupsForTest = -1;
                    break;
                case "Pattern 1d":
                    newPattern = new RepeatablePattern(Utils.HEALT_PATTERN_1d(health, vibrationDuration));
                    numberOfGroupsForTest = -1;
                    break;
                case "Pattern 2":
                    newPattern = new RepeatablePattern(Utils.HEALT_PATTERN_2(health));
                    numberOfGroupsForTest = -1;
                    break;
                case "Pattern 2b":
                    newPattern = new RepeatablePattern(Utils.HEALT_PATTERN_2b(health));
                    numberOfGroupsForTest = -1;
                    break;
                case "Pattern 3":
                    newPattern = new RepeatablePattern(Utils.HEALT_PATTERN_3(health, Utils.HEALT_PATTERN_1(health, vibrationDuration)));
                    numberOfGroupsForTest = -1;
                    break;
                case "Test Motor 1":
                    newPattern = new RepeatablePattern(Utils.HEALTH_PATTERN_MOTOR(1, health));
                    break;
                case "Test Motor 2":
                    newPattern = new RepeatablePattern(Utils.HEALTH_PATTERN_MOTOR(2, health));
                    break;
                case "Test Motor 3":
                    newPattern = new RepeatablePattern(Utils.HEALTH_PATTERN_MOTOR(3, health));
                    break;
                case "Test Motor 4":
                    newPattern = new RepeatablePattern(Utils.HEALTH_PATTERN_MOTOR(4, health));
                    break;
                case "Test Motor 5":
                    newPattern = new RepeatablePattern(Utils.HEALTH_PATTERN_MOTOR(5, health));
                    break;
                case "Test Motor 6":
                    newPattern = new RepeatablePattern(Utils.HEALTH_PATTERN_MOTOR(6, health));
                    break;
                case "Test Motor 7":
                    newPattern = new RepeatablePattern(Utils.HEALTH_PATTERN_MOTOR(7, health));
                    break;
                case "Test Motor 8":
                    newPattern = new RepeatablePattern(Utils.HEALTH_PATTERN_MOTOR(8, health));
                    break;
                default:
                    break;
            }
            if (newPattern != null)
            {
                nextHealthTime = Environment.TickCount + (long)numericUpDown4.Value;
                bool result = PlayPattern(healthPattern, newPattern);
                if (!result)
                {
                    healthPattern = newPattern;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            double stamina = (double)numericUpDown9.Value / (double)numericUpDown9.Maximum;
            UpdateStaminaLevel(stamina);
        }

        private void UpdateStaminaLevel(double stamina)
        {
            double staminaChange = (double)numericUpDown10.Value;
            int vibrationDuration = (int)numericUpDown8.Value;

            String selection = comboBox3.SelectedItem.ToString();
            UpdateStaminaPattern(selection, stamina, staminaChange, vibrationDuration);
            accuracyTestTextBox.Text = "Stamina" + ":" + selection;
        }

        private void UpdateStaminaPattern(String selection, double stamina, double staminaChange, int vibrationDuration)
        {
            IPattern newPattern = null;
            switch (selection)
            {
                case "Pattern Study 2":
                    newPattern = new RepeatablePattern(Utils.STAMINA_PATTERN_STUDY_2(stamina));
                    numberOfGroupsForTest = -1;
                    break;
                case "Pattern 1":
                    newPattern = new RepeatablePattern(Utils.STAMINA_PATTERN_1(stamina));
                    numberOfGroupsForTest = -1;
                    break;
                case "Pattern 2":
                    newPattern = new RepeatablePattern(Utils.STAMINA_PATTERN_2(staminaChange));
                    numberOfGroupsForTest = -1;
                    break;
                case "Pattern 3":
                    newPattern = new RepeatablePattern(Utils.STAMINA_PATTERN_3(staminaChange));
                    numberOfGroupsForTest = -1;
                    break;
                case "Pattern 4":
                    newPattern = new RepeatablePattern(Utils.STAMINA_PATTERN_4(staminaChange));
                    numberOfGroupsForTest = -1;
                    break;
                case "Pattern 5":
                    newPattern = new RepeatablePattern(Utils.STAMINA_PATTERN_5(staminaChange));
                    numberOfGroupsForTest = -1;
                    break;
                default:
                    break;
            }
            if (newPattern != null)
            {
                nextStaminaTime = Environment.TickCount + (long)numericUpDown7.Value;
                bool result = PlayPattern(staminaPattern, newPattern);
                if (!result)
                {
                    staminaPattern = newPattern;
                }
            }
        }

        private void StopAll()
        {
            checkBox1.CheckState = CheckState.Unchecked;
            if (ammoPattern != null && ammoPattern.GetType() == typeof(RepeatablePattern))
            {
                ((RepeatablePattern)ammoPattern).Stop();
            }
            checkBox2.CheckState = CheckState.Unchecked;
            if (healthPattern != null && healthPattern.GetType() == typeof(RepeatablePattern))
            {
                ((RepeatablePattern)healthPattern).Stop();
            }
            checkBox3.CheckState = CheckState.Unchecked;
            if (staminaPattern != null && staminaPattern.GetType() == typeof(RepeatablePattern))
            {
                ((RepeatablePattern)staminaPattern).Stop();
            }
        }

        private int CalculateDelay()
        {
            int waitTime = 1000;
            if (ammoPattern != null)
            {
                waitTime = Math.Max(waitTime, (int)Utils.GetMaxDuration(ammoPattern.GetSimplePatterns()));
            }
            if (healthPattern != null)
            {
                waitTime = Math.Max(waitTime, (int)Utils.GetMaxDuration(healthPattern.GetSimplePatterns()));
            }
            if (staminaPattern != null)
            {
                waitTime = Math.Max(waitTime, (int)Utils.GetMaxDuration(staminaPattern.GetSimplePatterns()));
            }
            return waitTime;
        }

        private async void AccuracyTest()
        {
            long patternIntervalForTests = 4000;
            numericUpDown3.Value = patternIntervalForTests;
            numericUpDown4.Value = patternIntervalForTests;
            numericUpDown7.Value = patternIntervalForTests;
            String category = accuracyTestTextBox.Text.Split(':')[0];
            String patternSelection = accuracyTestTextBox.Text.Split(':')[1];
            int numTests = 20;
            double[] testValues;
            if (numberOfGroupsForTest == -1)
            {
                testValues = MyRandomNumberGenerator(10, numTests);
            }
            else
            {
                numTests = (int)Math.Ceiling((double)numTests / numberOfGroupsForTest) * numberOfGroupsForTest;
                testValues = MyRandomNumberGenerator(numberOfGroupsForTest, numTests);
            }
            int[] userInputs = new int[numTests];
            long[] times = new long[numTests];
            for (int i = 0; i < numTests; i++)
            {
                double value = testValues[i];
                StopAll();
                await Task.Delay(CalculateDelay());
                switch (category)
                {
                    case "Ammo":
                        UpdateAmmoLevel(value);
                        checkBox1.CheckState = CheckState.Checked;
                        break;
                    case "Health":
                        UpdateHealthLevel(value);
                        checkBox2.CheckState = CheckState.Checked;
                        break;
                    case "Stamina":
                        UpdateStaminaLevel(value);
                        checkBox3.CheckState = CheckState.Checked;
                        break;
                }
                long startTime = Environment.TickCount;
                AccuracyTest ac = new AccuracyTest();
                while (ac.ShowDialog(this) != DialogResult.OK)
                {
                }
                int result = ac.GetResult();
                userInputs[i] = result;
                times[i] = Environment.TickCount - startTime;
                ac.Dispose();
            }
            String resultOut = "Testresults for " + accuracyTestTextBox.Text + " (Number of groups: " + numberOfGroupsForTest + "):";
            for (int i = 0; i < numTests; i++)
            {
                resultOut += Environment.NewLine + "Eingabe:" +
                    userInputs[i] +
                    "\tZielWert:" +
                    testValues[i] +
                    "\tZeit:" +
                    times[i] +
                    "\tms";
            }
            Console.WriteLine(resultOut);
            Logger.WriteLogLine(logName, resultOut);
            StopAll();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AccuracyTest();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            StopAll();
        }

        /*
         * Generates values for use in the test. When called with the arguments (3,3), random values from the following ranges will be returned in random order:
         * {[0-0.33]; [0,34-0.66]; [0,67-1]}
         */
        private double[] MyRandomNumberGenerator(int numberOfGroups, int numberOfRandomValues)
        {
            if (numberOfRandomValues % numberOfGroups != 0)
            {
                throw new Exception("NumberOfRandomValues has to be a multiple of numberOfGroups");
            }
            double[] values = new double[numberOfRandomValues];
            int stepSize = (int)Math.Floor(100.0 / numberOfGroups);
            for (int i = 0; i < numberOfRandomValues; i++)
            {
                int minValue = (int)Math.Ceiling((100.0 / numberOfGroups) * (i % numberOfGroups));
                if (Math.Abs(minValue - ((100.0 / numberOfGroups) * (i % numberOfGroups))) < 0.000001 && minValue != 0)
                {
                    minValue++;
                }
                int maxValue = (int)Math.Floor((100.0 / numberOfGroups) * ((i + 1) % numberOfGroups));
                if (i % numberOfGroups == numberOfGroups - 1)
                {
                    maxValue = 100;
                }
                int numberOfValues = maxValue - minValue + 1;
                int randomValue = random.Next(numberOfValues);
                int value = minValue + randomValue;
                values[i] = value / 100.0;
            }
            values = values.OrderBy(x => random.Next()).ToArray();
            return values;
        }

        private async void ShortTest()
        {
            long patternIntervalForTests = 4000;
            numericUpDown3.Value = patternIntervalForTests;
            numericUpDown4.Value = patternIntervalForTests;
            numericUpDown7.Value = patternIntervalForTests;
            String category = accuracyTestTextBox.Text.Split(':')[0];
            String patternSelection = accuracyTestTextBox.Text.Split(':')[1];
            int numTests = Math.Max(4, numberOfGroupsForTest);
            double[] testValues = new double[numTests];
            for (int i = 0; i < numTests; i++)
            {
                testValues[i] = (1.0 / (numTests - 1)) * i;
            }
            for (int i = 0; i < numTests; i++)
            {
                double value = testValues[i];
                if (category.Equals("Ammo") && patternSelection.Equals("Pattern Study 2"))
                {
                    value = testValues[numTests - 1 - i];
                }
                StopAll();
                await Task.Delay(CalculateDelay());
                switch (category)
                {
                    case "Ammo":
                        UpdateAmmoLevel(value);
                        checkBox1.CheckState = CheckState.Checked;
                        break;
                    case "Health":
                        UpdateHealthLevel(value);
                        checkBox2.CheckState = CheckState.Checked;
                        break;
                    case "Stamina":
                        UpdateStaminaLevel(value);
                        checkBox3.CheckState = CheckState.Checked;
                        break;
                }
                if (category.Equals("Ammo") && patternSelection.Equals("Pattern Study 2"))
                {
                    DialogResult res = MessageBox.Show("" + (numTests - i));
                }
                else if (numberOfGroupsForTest == -1)
                {
                    DialogResult res = MessageBox.Show("" + (value * 100));
                }
                else
                {
                    DialogResult res = MessageBox.Show("" + (i + 1));
                }
            }
            StopAll();
        }

        private void shortTestButton_Click(object sender, EventArgs e)
        {
            ShortTest();
        }
    }
}
