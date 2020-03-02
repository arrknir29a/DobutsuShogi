using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DobutsuShogi
{
    class ClickPoint
    {
        public int x { get; private set; }
        public int y { get; private set; }
        public EClickPointState cs { get; private set; }
        public ClickPoint(int x, int y, EClickPointState s)
        {
            // TODO: Complete member initialization
            this.cs=s;
            this.x = x;
            this.y = y;
        }
    }
}
