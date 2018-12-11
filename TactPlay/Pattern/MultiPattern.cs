using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TactPlay.Pattern
{
    public class MultiPattern : IPattern
    {
        private List<IPattern> patterns;

        public MultiPattern(List<IPattern> patterns)
            : base()
        {
            if (patterns.Count == 0)
            {
                throw new Exception("The list has to include at least one pattern");
            }
            this.patterns = patterns;
        }

        public MultiPattern(IPattern pattern1, IPattern pattern2)
            : base()
        {
            patterns = new List<IPattern>
            {
                pattern1,
                pattern2
            };
        }

        public MultiPattern(IPattern pattern1, IPattern pattern2, IPattern pattern3)
            : base()
        {
            patterns = new List<IPattern>
            {
                pattern1,
                pattern2,
                pattern3
            };
        }

        public MultiPattern(IPattern pattern1, IPattern pattern2, IPattern pattern3, IPattern pattern4)
            : base()
        {
            patterns = new List<IPattern>
            {
                pattern1,
                pattern2,
                pattern3,
                pattern4
            };
        }

        public List<IPattern> GetPatterns()
        {
            return patterns;
        }

        public List<SimplePattern> GetSimplePatterns()
        {
            List<SimplePattern> patternList = new List<SimplePattern>();
            foreach (IPattern p in patterns)
            {
                patternList.AddRange(p.GetSimplePatterns());
            }
            return patternList;
        }
    }
}
