using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Breakout
{
    [Serializable()]
    public struct Score
    {
        public string name
        {
            get; private set;
        }
        public int value
        {
            get; private set;
        }
        public Score(string name, int value)
        {
            this.name = name;
            this.value = value;
        }
    }
}
