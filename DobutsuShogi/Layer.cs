using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DobutsuShogi
{
   public class Layer
    {
       public int defaultId { get; private set; }
       public Layer(int width, int heigth,int defaultId)
       {
            
            this.height = heigth;
            this.width = width;
            elements = new MapElement[ width,height];
            this.defaultId = defaultId;
            clear();

        }
     internal MapElement[,] elements;
        public int height { get;private set; }

        public int width { get;private set; }

        internal void SetElement(MapElement me)
        {
            if (me.x > this.width || me.y > this.height) {
                throw new IndexOutOfRangeException();
            }
            elements[me.x,me.y]=me;
        }
        internal MapElement Get(int x, int y)
        {
            return elements[x, y];
        }
    
        internal int GetId(int x, int y)
        {
            return elements[x, y].id;
        }

        internal void clear()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    elements[x, y] = new MapElement(x, y, defaultId);
                }
            }
        }

        internal void SetElement(MapElement me, int x, int y)
        {
            elements[x, y] = me;
            elements[x, y].x = x;
            elements[x, y].y = y;
        }
        internal void remove(int x, int y)
        {

            elements[x, y] = new MapElement(x, y, defaultId);
        }
    }
}
