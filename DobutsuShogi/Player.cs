using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DobutsuShogi
{
    class Player
    {
        public override int GetHashCode()
        {
            return 1;
        }
        public static bool operator==(Player pl,Player pl2){

            if (object.ReferenceEquals(pl, null))
            {
                return object.ReferenceEquals(pl2, null);
            }
            return pl.Equals(pl2) ;
        }
        public static bool operator !=(Player pl, Player pl2)
        {
            if (object.ReferenceEquals(pl, null))
            {
                return !object.ReferenceEquals(pl2, null);
            }
            return !pl.Equals(pl2);
        }
        public override bool Equals(object o)
        {
            if (object.ReferenceEquals(0, null)) { return false; }
             if(  o is Player){
                var p =(Player)o;
                return p.id==this.id;
            }
            return false;
        }
        public int id { get; set; }
        public String name { get; set; }
        public Player(int p)
        {  
            this.id = p;
            this.figures = new List<Figure>();
        }

        public List<Figure> figures { get; set; }
        public bool isWinner { get; set; }
       
    }
}
