using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DobutsuShogi
{
    class TextureContainer
    {

        List<int> keys;
        List<Bitmap> values;
        public TextureContainer() {
            keys = new List<int>();
            values = new List<Bitmap>();
        }
        internal void load()
        {
            Assembly thisAssembly = Assembly.GetExecutingAssembly();
            Dictionary<int,string> d = new Dictionary<int,string>();
            d.Add( (int)EFigure.CHICK, "textures.ch.png");
            d.Add( (int)EFigure.CHICKEN, "textures.chicken.png");
            d.Add( (int)EFigure.LION, "textures.li.png");
            d.Add( (int)EFigure.GIRAFFE, "textures.gi.png");
            d.Add( (int)EFigure.ELEPHANT, "textures.el.png");
            d.Add( -1, "textures.bc.png");
            d.Add( -2, "textures.bc_move_ok.png");
            d.Add( -3, "textures.bc_move_bad.png");
            d.Add( -4, "textures.bc_strike.png");
            d.Add(-5, "textures.sl_blank.png");
            d.Add(-6, "textures.bc_selected.png");
            d.Add(-7, "textures.winner.png");
            notransform.Add(-7);//TODO: исправить
            Bitmap b;
            Bitmap loadError = new Bitmap(256,256);
            using (Graphics gLoadError = Graphics.FromImage(loadError))
            {
                Pen lp=new Pen(Color.FromArgb(255, 255,0,0), 10.0f);
                gLoadError.DrawLine(lp, 0, 0, 0, 255);
                gLoadError.DrawLine(lp, 0, 255, 255, 255);
                gLoadError.DrawLine(lp, 255, 255,255, 0);
                gLoadError.DrawLine(lp, 255, 0, 0, 0);
                gLoadError.DrawLine(lp, 0, 0, 255, 255);
                gLoadError.DrawLine(lp, 255, 0, 0, 255);
            }
            foreach (var k in d) {
                using (Stream file = thisAssembly.GetManifestResourceStream("DobutsuShogi."+k.Value))
                {
                    try
                    {
                        b = (Bitmap)Bitmap.FromStream(file);
                        Add(k.Key, b);
                        file.Close();
                    }
                    catch {
                        Add(k.Key, loadError);
                        Console.WriteLine("Failed to load texture ("+k.Value+")!!!!!!!");
                    }
                }
            }
          
        }
        public List<int> notransform = new List<int>();
        private void Add(int p, Bitmap b)
        {
            keys.Add(p);
            values.Add(b);
        }

        internal void TransformTo(int textureSize)
        {

            for (int i = 0; i < values.Count;i++ )
            {
                if (!notransform.Contains(keys[i]))
                {
                    Bitmap b = new Bitmap(textureSize, textureSize);
                    Graphics g = Graphics.FromImage(b);
                    g.DrawImage(values[i], new Rectangle(new Point(0, 0), b.Size), new Rectangle(new Point(0, 0), values[i].Size), GraphicsUnit.Pixel);
                    values[i] = b;
                }
            }

            this.textureSize = textureSize;
        }

        public int textureSize { get; set; }

        internal Bitmap Get(int p)
        {
            return values[keys.IndexOf(p)];
        }
    }
}
