using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DobutsuShogi
{
    class Turn
    {
        public  TurnState TurnState { get; private set; }
        public  Player player { get; private set; }
        public  Figure figure { get; private set; }
        public bool strike { get;  set; }
        private Figure newfigure;
        public Figure newFigure
        {
            get {return newfigure;}
            set
            {
                if (value == null) {
                    newfigure = null;
                }else{ 
                newfigure = value.clone() as Figure ;}
            }
        }
        private Figure sleevefigure;
        public Figure sleeveFigure
        {
            get { return sleevefigure; }
            set
            {
                if (value == null)
                {
                    sleevefigure = null;
                }
                else
                {
                    sleevefigure = value.clone() as Figure;
                }
            }
        }
        public Turn(Figure f, Player player, TurnState movst,Figure me)
        {
            this.figure = f.clone() as Figure;
            this.player = player;
            this.TurnState = movst;
            this.newFigure = me.clone() as Figure;

        }
        public Turn(Figure f, Player player, TurnState movst)
        {
            this.figure = f.clone() as Figure;
            this.player = player;
            this.TurnState = movst;
            this.newFigure = null;

        }
        public override string ToString()
        {
            return string.Format("Player{0}:from {5}{1},{2} to {3},{4}",player.id,figure.x,figure.y,TurnState.x,TurnState.y,figure.inSleeve?"sleeve ":"") ;
        }
    }
}
