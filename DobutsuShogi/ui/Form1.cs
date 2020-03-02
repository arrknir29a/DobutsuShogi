using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DobutsuShogi
{ 
    
    public partial class Form1 : Form
    {
         Graphic gr;
         Content c;
         GamePlay gp;
        public Form1()
        {
            InitializeComponent();
            c = new Content();
            gr = new Graphic(this.pictureBox1.Height, this.pictureBox1.Width,c);
            gp = new GamePlay(c,gr);
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.pictureBox1.Image = gr.render();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;

          gp.click(me.X,me.Y,(me.Button==MouseButtons.Right));
        pictureBox1.Image=  gr.render();
        
        if (gp.history.hasNewElement())
        {
            Turn lm = gp.history.getLast();
            textBox1.Text += string.Format("Player{5}({0}):from {1},{2} to {3},{4}\r\n",lm.player.name,lm.figure.x,lm.figure.y,lm.TurnState.x,lm.TurnState.y,lm.player.id);
        }
        if (gp.winner != null) {
            if (gp.winner.name != null && gp.winner.name != "")
            {
                textBox1.Text += gp.winner.name + " won!!!!\r\n";
            }
            else 
            {
                textBox1.Text += "player" + gp.winner.id + " won!!!!\r\n";
            }
        }
        GC.Collect();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            gp.restore();
            pictureBox1.Image = gr.render();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            gp.content.player2.name = textBox2.Text;
            gp.content.player1.name = textBox3.Text;
            textBox1.Text += "Player1.name="+gp.content.player1.name+"\r\n";
            textBox1.Text += "Player2.name=" + gp.content.player2.name+"\r\n";
        }
    }
}
