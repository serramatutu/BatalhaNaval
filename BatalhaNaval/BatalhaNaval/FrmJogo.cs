using System;
using System.Drawing;
using System.Windows.Forms;

namespace BatalhaNaval
{
    public partial class FrmJogo : Form
    {
        const int FPS = 60;
        TabuleiroGrafico tInimigo, tJogador;

        public FrmJogo()
        {
            InitializeComponent();

            tInimigo = new TabuleiroGrafico("../../resources/idleHover.png", "../../resources/idleClick.png",
                                            "../../resources/enemyHover.png", "../../resources/enemyClick.png");
            tJogador = new TabuleiroGrafico("../../resources/idleHover.png", "../../resources/idleClick.png",
                                            "../../resources/playerHover.png", "../../resources/playerClick.png");

            animTimer.Interval = 1000 / FPS;
        }

        private void tela_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox tela = (PictureBox)sender;
            if (tela.Name == "telaInimigo")
                tInimigo.MouseDown(e.Location);
            else
                tJogador.MouseDown(e.Location);
        }

        private void tela_MouseUp(object sender, MouseEventArgs e)
        {
            PictureBox tela = (PictureBox)sender;
            if (tela.Name == "telaInimigo")
                tInimigo.MouseUp();
            else
                tJogador.MouseUp();
        }

        private void animTimer_Tick(object sender, EventArgs e)
        {
            telaInimigo.Invalidate();
            telaJogador.Invalidate();
        }

        private void tela_Paint(object sender, PaintEventArgs e)
        {
            PictureBox tela = (PictureBox)sender;
            if (tela.Name == "telaInimigo")
                tInimigo.Paint(e.Graphics);
            else
                tJogador.Paint(e.Graphics);
        }

        private void tela_MouseMove(object sender, MouseEventArgs e)
        {
            PictureBox pb = (PictureBox)sender;
            TabuleiroGrafico t = pb.Name == "telaInimigo" ? tInimigo : tJogador;
            t.MouseMove(e.Location);

            Point pos = t.GetMouseGridPos(pb.Width, pb.Height);
            txtCoordenadas.Text = "X: " + (pos.X + 1) + " / Y: " + (pos.Y + 1);
        }

        private void tela_MouseLeave(object sender, EventArgs e)
        {
            PictureBox tela = (PictureBox)sender;
            if (tela.Name == "telaInimigo")
                tInimigo.MouseLeave();
            else
                tJogador.MouseLeave();

            txtCoordenadas.Text = "X: * / Y: * ";
        }
    }
}
