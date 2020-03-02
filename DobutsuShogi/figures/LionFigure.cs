using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DobutsuShogi.figures
{
    class LionFigure : Figure
    {


        public LionFigure(int x, int y, EFigure f, Player player, bool inSleeve)
            : base(x, y, f, player, inSleeve)
        {
            this.player = player;
            this.x = x;
            this.y = y;
            this.id = (int)f;
            this.inSleeve = inSleeve;
        }
    }

        
}
