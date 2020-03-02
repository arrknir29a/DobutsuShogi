using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DobutsuShogi
{
    class History
    {
        public History() {
            Turns = new List<Turn>();
        }
        List<Turn> Turns;
        internal Turn getLast()
        {
            if (Turns.Count > 0)
            {
                hasnewElement = false;
                return Turns[Turns.Count - 1];
            }
            else {
                return null;
            }

        }

        internal void add(Turn m)
        {
            Turns.Add(m);
            hasnewElement = true;
        }

        public bool isEmpty { get { return Turns.Count == 0; }  private set{} }
        bool hasnewElement;
        internal bool hasNewElement()
        {
            return hasnewElement;
        }
    }
}
