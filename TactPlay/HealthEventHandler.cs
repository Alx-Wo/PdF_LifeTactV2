using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TactPlay.Pattern;

namespace TactPlay
{
    class HealthEventHandler
    {
        private Dictionary<string, BodyPart> bodyParts = new Dictionary<string, BodyPart>();
        private double damage = -1;
        private readonly object syncLock = new object();
        private readonly IPattern DEFAULT_PATTERN = Utils.HEALT_PATTERN_1(1.0, 200);
        private RepeatablePattern repeatablePattern;
        private bool enabled = false;

        public HealthEventHandler()
        {
            ResetPattern();
        }

        private void ResetPattern()
        {
            repeatablePattern = new RepeatablePattern(DEFAULT_PATTERN);
        }

        public void EventDamaged(string bodyPart, double damage)
        {
            if (!enabled)
            {
                return;
            }
            UpdateDamage(bodyPart, damage);
        }

        public void EventTimer(string bodyPart, double damage)
        {
            if (!enabled)
            {
                return;
            }
            UpdateDamage(bodyPart, damage);
        }

        private void UpdateDamage(string bodyPart, double damage)
        {
            lock (syncLock)
            {
                if (!bodyParts.ContainsKey(bodyPart))
                {
                    bodyParts[bodyPart] = new BodyPart(bodyPart, damage);
                }
                else
                {
                    bodyParts[bodyPart].SetDamage(damage);
                }
                double sum = 0;
                foreach (KeyValuePair<string, BodyPart> entry in bodyParts)
                {
                    if (entry.Value.GetName().Equals("head") || entry.Value.GetName().Equals("body"))
                    {
                        sum += entry.Value.GetDamage();
                    }
                    else
                    {
                        sum += entry.Value.GetDamage() * 0.9;
                    }
                }
                this.damage = sum / bodyParts.Count;
                if (bodyParts.ContainsKey("head"))
                {
                    this.damage = Math.Max(this.damage, bodyParts["head"].GetDamage());
                }
                if (bodyParts.ContainsKey("body"))
                {
                    this.damage = Math.Max(this.damage, bodyParts["body"].GetDamage());
                }

                repeatablePattern.UpdatePattern(Utils.HEALT_PATTERN_2(1.0 - this.damage));
                Main.vibrationDevice.PlayPattern(repeatablePattern);
            }
        }

        public void Enable()
        {
            enabled = true;
        }

        public void Disable()
        {
            enabled = false;
            repeatablePattern.Stop();
            ResetPattern();
        }

        private class BodyPart
        {
            private string name;
            private double damage;

            public BodyPart(string name, double damage)
            {
                this.name = name;
                this.damage = damage;
            }

            public string GetName()
            {
                return name;
            }

            public double GetDamage()
            {
                return damage;
            }

            public void SetDamage(double damage)
            {
                this.damage = damage;
            }
        }
    }
}
