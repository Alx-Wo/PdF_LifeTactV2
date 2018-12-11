using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TactPlay.Pattern
{
    class RepeatablePattern : IPattern
    {
        private static long repeatablePatternID = 1;
        private readonly long id;
        private IPattern pattern;
        private readonly object myLock = new object();
        private bool repeating = true;

        public RepeatablePattern(IPattern pattern)
        {
            this.pattern = pattern;
            lock (this)
            {
                this.id = repeatablePatternID;
                repeatablePatternID++;
            }
        }

        public void UpdatePattern(IPattern newPattern)
        {
            pattern = newPattern;
        }

        public List<SimplePattern> GetSimplePatterns()
        {
            return pattern.GetSimplePatterns();
        }

        public long ID => id;

        public void Stop()
        {
            repeating = false;
        }

        public bool IsRepeating()
        {
            return repeating;
        }
    }
}
