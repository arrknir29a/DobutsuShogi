using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DobutsuShogi
{
    class GamePlay
    {
        public Content content { get; private set; }
        public Graphic graphic { get; private set; }
        public GamePlay(Content c,Graphic g)
        {
            // TODO: Complete member initialization
            this.content = c;
            this.graphic = g;
            selectedFigure = null;
            pl = content.player1;
            winner = null;
            history = new History();
        }
        private Figure selectedFigure;
        Player pl;
        public Player winner { get; private set; }
        private List<TurnState> avalibleTurns;
        internal void click(int absX, int absY,bool unselect)
        {
            if (unselect) {
                selectedFigure = null;
                content.background.clear();
                content.sleeve_background.clear();
                return;
            }
            if (winner == null)
            {
                var p = graphic.getPoint(absX, absY);

                content.background.clear();
                content.sleeve_background.clear();
                switch (p.cs)
                {
                    case EClickPointState.BOARD:
                        if (selectedFigure == null)
                        {
                            var me = content.figures.Get(p.x, p.y);
                            if (me is Figure && ((Figure)me).player == pl)
                            {

                                selectedFigure = (Figure)me;
                            }
                            else
                            {
                                break;
                            }

                            avalibleTurns = getTurns(selectedFigure);
                            showSelectedElements();
                            showTurns();
                        }
                        else
                        {
                            foreach (var mov in avalibleTurns)
                            {
                                if (p.x == mov.x && p.y == mov.y && !mov.blocked)
                                {
                                    move(mov);
                                    if (pl == content.player1)
                                    {
                                        pl = content.player2;

                                    }
                                    else
                                    {
                                        pl = content.player1;
                                    }
                                    break;
                                }
                            }
                            selectedFigure = null;
                            content.background.clear();
                        }
                        break;
                    case EClickPointState.SLEEVE1:
                    case EClickPointState.SLEEVE2:
                        int y = (p.cs == EClickPointState.SLEEVE1) ? 0 : 1;
                        var m = content.sleeve.Get(p.x, y);
                        if (m is Figure && ((Figure)m).player == pl)
                        {
                            selectedFigure = (Figure)m;

                        }
                        else { break; }
                        avalibleTurns = getTurns(selectedFigure);
                        showSelectedElements();
                        showTurns();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Console.WriteLine(winner.id);
            }
            
        }
        private Player pl_;
        private void move(TurnState ms)
        {

           
            pl_ = pl ;
            Turn mo = new Turn(selectedFigure, selectedFigure.player, ms);
            
            
            
            if (ms.strike) {
                mo.strike = true;
                var f = (Figure)content.figures.Get(ms.x, ms.y);
                mo.newFigure = f;
                if(f.id==(int)EFigure.CHICKEN){
                f.id=(int)EFigure.CHICK;
                }
                content.sleeve.put(f, selectedFigure.player);
                mo.sleeveFigure = f;
                
            }
            if (selectedFigure.inSleeve) {
                selectedFigure.inSleeve = false;
                content.sleeve.remove(selectedFigure.x, selectedFigure.y);
                content.figures.SetElement(selectedFigure, ms.x, ms.y);
            }
            else
            {
                content.figures.remove(selectedFigure.x, selectedFigure.y);
                content.figures.SetElement(selectedFigure, ms.x, ms.y);
            }
            onMove(ms,mo);
            history.add(mo);
            
        }
        public  History history { get; set; }
        public void restore() {
            Turn m = history.getLast();
            if (m.figure.inSleeve) {
                content.figures.remove(m.TurnState.x, m.TurnState.y);
                content.sleeve.SetElement(m.figure);

            }
            else if (m.strike)
            {
                content.figures.remove(m.TurnState.x, m.TurnState.y);
                content.figures.SetElement(m.newFigure);
                content.figures.remove(m.figure.x, m.figure.y);
                content.figures.SetElement(m.figure);
                content.sleeve.remove(m.sleeveFigure.x, m.sleeveFigure.y);
               
            }
            else
            {
                content.figures.remove(m.TurnState.x, m.TurnState.y);
                content.figures.SetElement(m.figure);
            }
            pl = m.player;
            selectedFigure = null;
        }
        private void onMove(TurnState ms,Turn mo)
        {
            if (mo.figure.id == (int)EFigure.CHICK)
            {
                if ((mo.player.id == 1 && ms.y == 0) || mo.player.id == 2 && ms.y == content.figures.height - 1)
                {
                    selectedFigure.id = (int)EFigure.CHICKEN;
                }
                //win trigger
               
            }
            
            if (mo.strike)
            {
                
                if (mo.newFigure.id == (int)EFigure.LION)
                {
                    winner = mo.player;
                    winner.isWinner = true;
                    return;
                }
            }
            if (mo.figure.id == (int)EFigure.LION)
            {
                if ((mo.player.id == 1 && ms.y == 0) || selectedFigure.player.id == 2 && ms.y == content.figures.height - 1)
                {
                    bool asd = false;
                    for (int x = 0; x < content.figures.width; x++)
                    {
                        for (int y = 0; y < content.figures.height; y++)
                        {
                            if (content.figures.Get(x, y) is Figure)
                            {
                                var f = (Figure)content.figures.Get(x, y);
                                var mov = getTurns(f);
                                
                                foreach (var m in mov)
                                {
                                    if (m.x == ms.x && m.y == ms.y&&m.strike) {
                                        asd = true;
                                    }
                                }
                            }
                        }
                    }
                    if (!asd) {
                        winner = selectedFigure.player;
                        winner.isWinner = true;
                    }
                }
            }
        }
        private void showSelectedElements() {
            if (selectedFigure != null)
            {
                if (!selectedFigure.inSleeve)
                {
                    content.background.Get(selectedFigure.x, selectedFigure.y).id = -6;
                }
                else
                {
                    content.sleeve_background.Get(selectedFigure.x, selectedFigure.y).id = -6;
                }
            }       
            
        }
        private void showTurns() {
            foreach (var mov in avalibleTurns)
            {
                if (!mov.blocked)
                {
                    content.background.Get(mov.x, mov.y).id = mov.strike ? -4 : -2;
                }
            }
        }
        private List<TurnState> getTurns( Figure figure,int x, int y)
        {
            List<TurnState> Turns = new List<TurnState>();
            switch ((EFigure)figure.id)
            {
                case EFigure.CHICK:
                    Turns.AddRange(getChickTurns(figure,x,y));
                    break;
                case EFigure.ELEPHANT:
                    Turns.AddRange(getElephantTurns(figure,x,y));
                    break;
                case EFigure.GIRAFFE:
                    Turns.AddRange(getGiraffeTurns(figure,x,y));
                    break;
                case EFigure.LION:
                    Turns.AddRange(getLoinTurns(figure,x,y));
                    break;
                case EFigure.CHICKEN:
                    Turns.AddRange(getChickenTurns(figure,x,y));
                    break;
            }
            return Turns;
        }
        private List<TurnState> getTurns(Figure figure)
        {
            List<TurnState> Turns = new List<TurnState>();
            if (figure.inSleeve)
            {
                for (int x = 0; x < content.figures.width; x++)
                {
                    for (int y = 0; y < content.figures.height; y++)
                    {
                        if (!(content.figures.Get(x, y) is Figure))
                        {
                            if (figure.id == (int)EFigure.CHICK)
                            {
                                bool br = false;
                                for (int i = 0; i < content.figures.height; i++)
                                {
                                    if (content.figures.Get(x, i) is Figure && ((Figure)content.figures.Get(x, i)).player == figure.player && ((Figure)content.figures.Get(x, i)).id==(int)EFigure.CHICK)
                                    {
                                        br = true; 
                                    }
                                }
                                if (br) {
                                    y++;
                                        break;
                                }
                            }
                          if (getTurns(figure, x, y).Count>0)  Turns.Add(new TurnState(x,y,false));
                        }
                    }
                }
            }else {
                Turns.AddRange(getTurns( figure,figure.x, figure.y));
            }
            return Turns;
        }
        private List<TurnState> getLoinTurns(Figure figure, int x, int y)
        {
            List<TurnState> mov = new List<TurnState>();
            mov.AddRange(getGiraffeTurns(figure,x,y));
            mov.AddRange(getElephantTurns(figure,x,y));
            return mov;
        }
        private List<TurnState> getGiraffeTurns(Figure figure,int x,int y)
        {
            List<TurnState> mov = new List<TurnState>();
           if (x - 1 >= 0 ) {
                   var f = content.figures.Get(x-1, y); //left
                  
                   if (f is Figure)
                   {
                       if (((Figure)f).player != figure.player){
                           mov.Add(new TurnState(x - 1, y, true));
                       }
                       else
                       {
                           mov.Add(new TurnState(x - 1, y, false,true));
                       }
                   }else {
                       mov.Add(new TurnState(x - 1, y, false));
                   }
           }
           if (x + 1 < content.figures.width ) {//right
               var f = content.figures.Get(x + 1, y );
               if (f is Figure)
               {
                   if (((Figure)f).player != figure.player)
                   {
                       mov.Add(new TurnState(x + 1, y, true));
                   }
                   else
                   {
                       mov.Add(new TurnState(x + 1, y, false, true));
                   }
               }
               else
               {
                   mov.Add(new TurnState(x + 1, y, false));
               }
           }
           if ( y-1 >= 0)//up
           {
                   var f = content.figures.Get(x , y - 1);
                   if (f is Figure)
                   {
                       if (((Figure)f).player != figure.player)
                        {
                           mov.Add(new TurnState(x , y-1, true));
                        }
                       else
                       {
                           mov.Add(new TurnState(x , y-1, false, true));
                       }
                        }else{
                          mov.Add(new TurnState(x , y-1, false));
                        }
                    }
            if (y + 1 < content.figures.height)//down
             {
               var f = content.figures.Get(x, y + 1);
               if (f is Figure)
                   {
                       if (((Figure)f).player != figure.player)
                        {
                           mov.Add(new TurnState(x , y+1, true));
                        }
                       else
                       {
                           mov.Add(new TurnState(x, y+1, false, true));
                       }
                        }else{
                          mov.Add(new TurnState(x , y+1, false));
                        }
                   }
             
            return mov;
        }
        private List<TurnState> getElephantTurns(Figure figure, int x, int y)
        {
            List<TurnState> mov = new List<TurnState>();
            if (x - 1 >= 0 && y - 1 >= 0)
            {
                var f = content.figures.Get(x - 1, y - 1);
                if (f is Figure)
                   {
                       if (((Figure)f).player != figure.player)
                        {
                           mov.Add(new TurnState(x-1 , y-1, true));
                        }
                       else
                       {
                           mov.Add(new TurnState(x - 1, y-1, false, true));
                       }
                   }else{
                          mov.Add(new TurnState(x-1 , y-1, false));
                   }
                   
            }
            if (x - 1 >= 0 && y + 1 < content.figures.height)
            {
                var f = content.figures.Get(x - 1, y + 1);
                if (f is Figure)
                {
                    if (((Figure)f).player != figure.player)
                    {
                        mov.Add(new TurnState(x - 1, y + 1, true));
                    }
                    else
                    {
                        mov.Add(new TurnState(x - 1, y+1, false, true));
                    }
                }else{
                    mov.Add(new TurnState(x - 1, y + 1, false));
                }
            }
            if (x + 1 < content.figures.width && y - 1 >= 0)
            {
                var f = content.figures.Get(x + 1, y - 1);
                if (f is Figure)
                {
                    if (((Figure)f).player != figure.player)
                    {
                        mov.Add(new TurnState(x + 1, y - 1, true));
                    }
                    else
                    {
                        mov.Add(new TurnState(x + 1, y-1, false, true));
                    }
                }
                else
                {
                    mov.Add(new TurnState(x + 1, y - 1, false));
                }
            }
            if (x + 1 < content.figures.width && y + 1 < content.figures.height)
            {
                var f = content.figures.Get(x + 1, y + 1);
                if (f is Figure)
                {
                    if (((Figure)f).player != figure.player)
                    {
                        mov.Add(new TurnState(x + 1, y + 1, true));
                    }
                    else
                    {
                        mov.Add(new TurnState(x + 1, y+1, false, true));
                    }
                }
                else
                {
                    mov.Add(new TurnState(x + 1, y + 1, false));
                }
            }
            return mov;
        }
        private List<TurnState> getChickTurns(Figure figure, int x, int y)
        {
            List<TurnState> mov = new List<TurnState>();
            if (figure.player.id == 2)
            {
                if (y + 1 < content.figures.height)
                {
                    var f = content.figures.Get(x, y + 1);
                    if (f is Figure)
                    {
                        if (((Figure)f).player != figure.player)
                        {
                            mov.Add(new TurnState(x , y + 1, true));
                        }
                        else
                        {
                            mov.Add(new TurnState(x , y+1, false, true));
                        }
                    }
                    else
                    {
                        mov.Add(new TurnState(x , y + 1, false));
                    }
                }
            }
            else
            {
                if (y - 1 >= 0)
                {
                    var f = content.figures.Get(x, y - 1);
                    if (f is Figure)
                    {
                        if (((Figure)f).player != figure.player)
                        {
                            mov.Add(new TurnState(x, y - 1, true));
                        }
                        else
                        {
                            mov.Add(new TurnState(x , y-1, false, true));
                        }
                    }
                    else
                    {
                        mov.Add(new TurnState(x , y - 1, false));
                    }
                }

            }
            return mov;
        }
        private List<TurnState> getChickenTurns(Figure figure,int x,int y)
        {
            List<TurnState> mov = new List<TurnState>();
            mov.AddRange(getGiraffeTurns(figure,x,y));
            if (figure.player.id == 2)
            {
                if (x + 1 < content.figures.width && y + 1 < content.figures.height)
                {
                    var f = content.figures.Get(x + 1, y + 1);
                    if (f is Figure)
                    {
                        if (((Figure)f).player != figure.player)
                        {
                            mov.Add(new TurnState(x + 1, y + 1, true));
                        }
                        else
                        {
                            mov.Add(new TurnState(x + 1, y+1, false, true));
                        }
                    }
                    else
                    {
                        mov.Add(new TurnState(x + 1, y + 1, false));
                    }
                }
                if (x - 1 >= 0 && y + 1 < content.figures.height)
                {
                    var f = content.figures.Get(x - 1, y + 1);
                    if (f is Figure)
                    {
                        if (((Figure)f).player != figure.player)
                        {
                            mov.Add(new TurnState(x - 1, y + 1, true));
                        }
                        else
                        {
                            mov.Add(new TurnState(x - 1, y+1, false, true));
                        }
                    }
                    else
                    {
                        mov.Add(new TurnState(x - 1, y + 1, false));
                    }
                }
            }
            else
            {
                if (x + 1 < content.figures.width && y - 1 >= 0)
                {
                    var f = content.figures.Get(x + 1, y - 1);
                    if (f is Figure)
                    {
                        if (((Figure)f).player != figure.player)
                        {
                            mov.Add(new TurnState(x + 1, y - 1, true));
                        }
                        else
                        {
                            mov.Add(new TurnState(x + 1, y-1, false, true));
                        }
                    }
                    else
                    {
                        mov.Add(new TurnState(x + 1, y - 1, false));
                    }
                }
                if (x -1>=0 && y -1>=0)
                {
                    var f = content.figures.Get(x - 1, y - 1);
                    if (f is Figure)
                    {
                        if (((Figure)f).player != figure.player)
                        {
                            mov.Add(new TurnState(x - 1, y - 1, true));
                        }
                        else
                        {
                            mov.Add(new TurnState(x - 1, y-1, false, true));
                        }
                    }
                    else
                    {
                        mov.Add(new TurnState(x - 1, y - 1, false));
                    }
                }

            }
            return mov;
        }



       
    }
}
