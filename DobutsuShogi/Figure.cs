using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DobutsuShogi
{
    class Figure :MapElement{
        
       public Player player { get; set; }
        public Figure(int x, int y, EFigure f,Player player) {
            this.player = player;
            this.x = x;
            this.y = y;
            this.id = (int)f;
        }
        public Figure(int x, int y, EFigure f, Player player,bool inSleeve)
        {
            this.player = player;
            this.x = x;
            this.y = y;
            this.id = (int)f;
            this.inSleeve = inSleeve;
        }


        public bool inSleeve { get; set; }
        internal override MapElement clone()
        {
            return new Figure(this.x, this.y, (EFigure)this.id, this.player,this.inSleeve);
        }
        public bool isEnemy(Player pl)
        {
            return player.id != pl.id;
        }
        public Turn[] getTurns()
        {

            return null;
        }
    }
}
