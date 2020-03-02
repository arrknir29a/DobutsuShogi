using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DobutsuShogi
{
    class Content 
    {
 

        public Player player1 { get; private set; }
        public Player player2 { get; private set; }
        public Layer background { get; set; }
       public Layer figures { get; set; }
       public Sleeve sleeve { get; set; }
       public Layer sleeve_background { get; set; }
        public Content() {
           player1 = new Player(1);
           player2 = new Player(2);
           background = new Layer(3,4,-1);
           figures = new Layer(3, 4,666);
           sleeve = new Sleeve(6, 2, -5);
           sleeve_background = new Layer(6, 2,-5);
           init();
        }

        
        public Player[] GetPlayers() {
            return new Player[] { player1, player2 };
        }
        public void init() {
            //начальная позиция
           
            
            #region player2
            figures.SetElement(new Figure(0, 0, EFigure.GIRAFFE,player2));
            figures.SetElement(new Figure(1, 0, EFigure.LION, player2));
            figures.SetElement(new Figure(2, 0, EFigure.ELEPHANT, player2));
            figures.SetElement(new Figure(1, 1, EFigure.CHICK, player2));
            player1.figures.Add(figures.Get(0,1) as Figure);
            #endregion
            #region player1
            figures.SetElement(new Figure(0, 3, EFigure.ELEPHANT, player1));
            figures.SetElement(new Figure(1, 3, EFigure.LION, player1));
            figures.SetElement(new Figure(2, 3, EFigure.GIRAFFE, player1));
            figures.SetElement(new Figure(1, 2, EFigure.CHICK, player1));
            #endregion



        }



    }
}
