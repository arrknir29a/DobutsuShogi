using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DobutsuShogi
{
    class Sleeve:Layer
    {
        public Sleeve(int width, int heigth, int defaultId):base(width,heigth,defaultId)
        {
        
        }
        internal void put(Figure f,Player pl)
        {
            int y=2-pl.id;
            for (int x = 0; x < this.width; x++)
            {
                if (!(elements[x, y] is Figure)) {
                    f.inSleeve = true;
                    f.player = pl;
                    f.x = x;
                    f.y = y;
                    elements[x, y] = f;
                    break;
                }
            }
        }
    }
}
