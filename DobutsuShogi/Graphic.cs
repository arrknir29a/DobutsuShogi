using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace DobutsuShogi
{
    class Graphic
    {
       internal Content content { get; set; }
       TextureContainer textures ;
       int SleeveHeight;
       int BoardHeight;
       int boardStartX;
       int boardStartY;
       int sleeveTextureSize;
       float transform_c;
        public int height { get;  set; }
        public int width { get;  set; }
        public Graphic(int height, int width,Content c)
        {
            this.content = c;
           
            this.textures = new TextureContainer();
            this.textures.load();
            this.boardStartX = 0;

            this.SleeveHeight = height/10;
            this.boardStartY = SleeveHeight;
            this.BoardHeight = height - this.SleeveHeight-boardStartY;

            this.boardStartX = 0;
         
            int textureSize = Math.Min((BoardHeight / content.figures.height), width / content.figures.width);
            BoardHeight = textureSize * content.figures.height;
            sleeveTextureSize = this.SleeveHeight;
            textures.TransformTo(textureSize);
            transform_c = 0.9f;
            this.height = boardStartY+BoardHeight+sleeveTextureSize;
          //  this.width = textureSize*content.figures.width;
            this.width = Math.Max(textureSize * content.figures.width,sleeveTextureSize*content.sleeve_background.width);
        }
        
        public Bitmap render() {
            
            Bitmap bm = new Bitmap(this.width,this.height);
            int flipPlayerId = 2;
            using (Graphics g = Graphics.FromImage(bm))
            {

                #region board
                //render background

                for (int x = 0; x < content.background.width; x++)
                {
                    for (int y = 0; y < content.background.height; y++)
                    {
                        g.DrawImage(textures.Get(content.background.GetId(x, y)), new Point(boardStartX + (x * textures.textureSize), boardStartY + (y * textures.textureSize)));
                    }
                }
                //render figures
                int x_difference = textures.textureSize-(int)Math.Round((float)textures.textureSize * transform_c, MidpointRounding.AwayFromZero);
                int y_difference = x_difference;
                for (int x = 0; x < content.background.width; x++)
                {
                    for (int y = 0; y < content.background.height; y++)
                    {
                        var el = content.figures.Get(x, y);
                        if (el is Figure && el.id != 666)
                        {
                            Figure figure = (Figure)el;

                            using (Bitmap tempTexture = new Bitmap(textures.Get(figure.id)))
                            {
                                if (figure.player.id == flipPlayerId)
                                {
                                    tempTexture.RotateFlip(RotateFlipType.Rotate180FlipNone);
                                }

                                g.DrawImage(tempTexture,
                                    new Rectangle((figure.x * textures.textureSize) + boardStartX + x_difference, (figure.y * textures.textureSize) + boardStartY + y_difference, tempTexture.Width - (2 * x_difference), tempTexture.Height - (2 * y_difference)),
                                    
                                    new Rectangle(new Point(0, 0), tempTexture.Size),
                                    GraphicsUnit.Pixel);

                                tempTexture.Dispose();
                            }
                        }
                    }
                }
                

                #endregion

                #region sleeve
                for (int x = 0; x < content.sleeve_background.width; x++)
                {
                    var el = content.sleeve_background.Get(x, 0);
                    g.DrawImage(textures.Get(el.id), new Rectangle(x * sleeveTextureSize, 0, sleeveTextureSize, sleeveTextureSize), new Rectangle(new Point(0, 0), textures.Get(el.id).Size), GraphicsUnit.Pixel);
                    var el2 = content.sleeve_background.Get(x, 1);
                    g.DrawImage(textures.Get(el2.id), new Rectangle(x * sleeveTextureSize, boardStartY + BoardHeight, sleeveTextureSize, sleeveTextureSize), new Rectangle(new Point(0, 0), textures.Get(el2.id).Size), GraphicsUnit.Pixel);
                }
                x_difference = sleeveTextureSize-(int)Math.Round((float)sleeveTextureSize * transform_c, MidpointRounding.AwayFromZero);
                y_difference = x_difference;
                for (int x = 0; x < content.sleeve.width; x++)
                {
                     
                    var el = content.sleeve.Get(x, 0);
                    if (el is Figure)
                    {
                        using (Bitmap tempTexture = new Bitmap(textures.Get(el.id)))
                        {
                            tempTexture.RotateFlip(RotateFlipType.Rotate180FlipNone);
                            g.DrawImage(tempTexture,
                                new Rectangle((x * sleeveTextureSize) + x_difference, 0 + x_difference, sleeveTextureSize - (2 * x_difference), sleeveTextureSize - (2 * y_difference)),
                                new Rectangle(new Point(0, 0), tempTexture.Size),
                                GraphicsUnit.Pixel);
                            tempTexture.Dispose();
                        }
                    }
                    var el2 = content.sleeve.Get(x, 1);
                    if (el2 is Figure)
                    {
                        g.DrawImage(textures.Get(el2.id),
                            new Rectangle((x * sleeveTextureSize) + x_difference, boardStartY + BoardHeight + x_difference, sleeveTextureSize - (2 * x_difference), sleeveTextureSize - (2 * y_difference)),
                            new Rectangle(new Point(0, 0), textures.Get(el2.id).Size),
                            GraphicsUnit.Pixel);
                    }
                }

                #endregion
                if (content.player1.isWinner || content.player2.isWinner)
                {
                    using (Bitmap wi =new Bitmap( textures.Get(-7)))
                    {
                        int winTextureHeight = 150;
                        if (content.player2.isWinner)
                        {
                            g.DrawImage(wi, new Rectangle(boardStartX + 10, boardStartY + 10, 250, winTextureHeight), new Rectangle(new Point(0, 0), wi.Size), GraphicsUnit.Pixel);
                        }
                        else
                        {
                            g.DrawImage(wi, new Rectangle(boardStartX + 10, boardStartY + BoardHeight - winTextureHeight, 250, winTextureHeight), new Rectangle(new Point(0, 0), wi.Size), GraphicsUnit.Pixel);
                        }
                    }
                }
                
                g.Dispose();
                return bm;
            }
        }

        public ClickPoint getPoint(int absX, int absY)
        {

            int x;
            int y;
            if (absX > this.width || absY > this.height) {
                return new ClickPoint(-1, -1, EClickPointState.NOWHARE);
            }
            if (absY <= boardStartY)
            {
                x = absX / sleeveTextureSize;
                y = absY / sleeveTextureSize;
                return new ClickPoint(x, y, EClickPointState.SLEEVE1);
            }
            else if (absY >= boardStartY + BoardHeight)
            {
                x = absX / sleeveTextureSize;
                y = (absY-BoardHeight-boardStartY) / sleeveTextureSize;
                return new ClickPoint(x, y, EClickPointState.SLEEVE2);
            }
            else
            {
                x = (absX-boardStartX) / textures.textureSize;
                y = (absY-boardStartY) / textures.textureSize;
                return new ClickPoint(x, y, EClickPointState.BOARD);
            }
        }

        
    }
}
