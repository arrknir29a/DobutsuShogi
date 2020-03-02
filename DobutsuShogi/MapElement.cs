using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DobutsuShogi
{
    class MapElement
    {
        public override string ToString()
        {
            return string.Format("x={0},y={1},id={2}", x, y, id);
        }
        public MapElement() { }
        public MapElement(int x, int y, int id) {
            this.x = x;
            this.y = y;
            this.id = id;

        }
        public int id { get; set; }
        public int x { get; set; }
        public int y { get; set; }

        internal virtual MapElement clone()
        {
            return new MapElement(x, y, id);
        }
    }
}
