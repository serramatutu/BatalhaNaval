using System;
using System.Drawing;
using System.Windows.Forms;
using Protocolo;

namespace BatalhaNaval
{
    public partial class FrmJogo : Form
    {
        private enum Status
        {
            PosicionandoNavios,
            Conectando,
            Jogando
        }

        const int FPS = 60;

        TabuleiroInimigo tInimigo;
        TabuleiroJogador tJogador;

        GerenciadorDeNavios gerenciadorDeNavios;
        ClienteP2P cliente;

        private Status status = Status.PosicionandoNavios;

        public FrmJogo()
        {
            InitializeComponent();

            tJogador = new TabuleiroJogador();
            tInimigo = new TabuleiroInimigo();

            telaJogador.AllowDrop = true;
            telaMenu.AllowDrop = true;

            animTimer.Interval = 1000 / FPS;
        }

        private void Conectar()
        {
            FrmConectar frm = new FrmConectar(cliente);

            Status anterior = status;
            status = Status.Conectando;

            if (frm.ShowDialog(this) == DialogResult.OK)
                status = Status.Jogando;
            else
                status = anterior;
        }

        private void PosicionarNavios()
        {
            tJogador = new TabuleiroJogador();
            tInimigo = new TabuleiroInimigo();

            gerenciadorDeNavios = new GerenciadorDeNavios(telaMenu.Width, telaMenu.Height);

            status = Status.PosicionandoNavios;
        }

        #region Eventos do jogo
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
            telaMenu.Invalidate();
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
            TabuleiroGrafico t = pb.Name == "telaInimigo" ? (TabuleiroGrafico)tInimigo : (TabuleiroGrafico)tJogador;
            t.MouseMove(e.Location);

            Point pos = t.GetMouseGridPos(pb.Width, pb.Height);
            lblCoord.Text = "X: " + (pos.X + 1) + " / Y: " + (pos.Y + 1);
        }

        private void tela_MouseLeave(object sender, EventArgs e)
        {
            PictureBox tela = (PictureBox)sender;
            if (tela.Name == "telaInimigo")
                tInimigo.MouseLeave();
            else
                tJogador.MouseLeave();

            lblCoord.Text = "X: * / Y: * ";
        }
        #endregion



        private void FrmJogo_Shown(object sender, EventArgs e)
        {
            PosicionarNavios();
        }

        private void telaMenu_Paint(object sender, PaintEventArgs e)
        {
            if (status == Status.PosicionandoNavios)
                gerenciadorDeNavios.Paint(e.Graphics);
        }

        # region Drag Drop

        int direcao = 0;
        bool dragging;
        bool Dragging
        {
            get => dragging;
            set
            {
                direcao = 0;
                dragging = value;
            }
        }

        private void telaMenu_MouseDown(object sender, MouseEventArgs e)
        {
            if (status != Status.PosicionandoNavios)
                return;

            TipoDeNavio? navio = gerenciadorDeNavios.NavioEm(e.Location);
            if (navio.HasValue)
            {
                gerenciadorDeNavios.Remover(e.Location);
                gerenciadorDeNavios.Rearranjar();
                Dragging = true;
                DoDragDrop(navio, DragDropEffects.Move);
            }
        }

        private void AumentarDirecao()
        {
            if (Dragging)
                direcao = (direcao + 1) % 3;
        }

        private void FrmJogo_MouseDown(object sender, MouseEventArgs e)
        {
            if (Dragging && e.Button == MouseButtons.Right)
                AumentarDirecao();
        }

        private void telaBarco_DragOver(object sender, DragEventArgs e)
        {
            if (sender == null)
                return;

            if (e.Data.GetDataPresent(typeof(TipoDeNavio)))
                e.Effect = DragDropEffects.Move;
        }

        private void telaMenu_DragDrop(object sender, DragEventArgs e)
        {
            TipoDeNavio? navio = (TipoDeNavio)e.Data.GetData(typeof(TipoDeNavio));
            if (navio.HasValue)
                gerenciadorDeNavios.Adicionar(navio.Value);

            Dragging = false;
        }

        private void telaJogador_DragDrop(object sender, DragEventArgs e)
        {
            TipoDeNavio? navio = (TipoDeNavio)e.Data.GetData(typeof(TipoDeNavio));
            tJogador.MouseMove(telaJogador.PointToClient(new Point(e.X, e.Y)));

            Point gridPos = tJogador.GetMouseGridPos(telaJogador.Width, telaJogador.Height);
            if (navio.HasValue)
                tJogador.Tabuleiro.PosicionarNavio(navio.Value, gridPos.X, gridPos.Y, direcao);

            Dragging = false;
        }

        #endregion
    }
}
