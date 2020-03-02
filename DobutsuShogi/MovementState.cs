using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DobutsuShogi
{
    class TurnState
    {


        public TurnState(int x, int y, bool s)
        {
            this.x = x;
            this.y = y;
            this.strike = s;
            this.blocked = false;
        }
        public TurnState(int x, int y, bool s,bool block)
        {
            this.x = x;
            this.y = y;
            this.strike = s;
            this.blocked = block;
        }
        public int x { get; set; }
        public int y { get; set; }
        public bool strike { get; set; }
        public bool blocked { get; set; }
    }
}
