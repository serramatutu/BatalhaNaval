using System;
using System.Drawing;
using System.Windows.Forms;
using BatalhaNaval;

namespace Jogo
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

        #region Conectar

        bool podeConectar;

        bool PodeConectar {
            get { return podeConectar; }
            set
            {
                btnConectar.Enabled = value;
                podeConectar = value;
            }
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            Conectar();
        }

        private void Conectar()
        {
            if (PodeConectar)
            {
                FrmConectar frm = new FrmConectar(cliente, tJogador.Tabuleiro);

                Status anterior = status;
                status = Status.Conectando;

                if (frm.ShowDialog(this) == DialogResult.OK)
                {
                    status = Status.Jogando;
                    cliente = frm.Cliente;
                    lblInfo.Text = "Vez de asdasdasd";
                    lblInfo2.Text = "Jogando contra " + cliente.NomeRemoto;
                    ConfigurarCliente();
                }
                else
                    status = anterior;
            } 
        }

        public void ConfigurarCliente()
        {
            cliente.OnResultadoDeTiro += Cliente_OnResultadoDeTiro;
            cliente.OnDarTiro += Cliente_OnDarTiro;
            cliente.OnTiroRecebido += Cliente_OnTiroRecebido;
            cliente.OnClienteDesconectado += Cliente_OnClienteDesconectado;
        }

        #endregion

        #region Jogo
        private void Cliente_OnClienteDesconectado(System.Net.IPAddress addr)
        {
            throw new NotImplementedException();
        }

        private void Cliente_OnTiroRecebido(Tiro t)
        {
            throw new NotImplementedException();
        }

        private void Cliente_OnResultadoDeTiro(Tiro t, ResultadoDeTiro resultado)
        {
            throw new NotImplementedException();
        }

        private void Cliente_OnDarTiro()
        {
            tInimigo.PodeAtirar = true;
        }

        private void TInimigo_OnTiroDado(int x, int y)
        {
            cliente.DarTiro(x, y);
        }

        #endregion

        private void PosicionarNavios()
        {
            PodeConectar = false;

            tJogador = new TabuleiroJogador();
            tInimigo = new TabuleiroInimigo();

            tInimigo.OnTiroDado += TInimigo_OnTiroDado;
            gerenciadorDeNavios = new GerenciadorDeNavios(telaMenu.Width, telaMenu.Height);

            status = Status.PosicionandoNavios;
        }

        #region Eventos
        private void tela_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox tela = (PictureBox)sender;
            if (tela.Name == "telaInimigo")
                if (e.Button == MouseButtons.Left)
                    tInimigo.MouseDown(e.Location);
            else
            {
                if (e.Button == MouseButtons.Left)
                    tJogador.MouseDown(e.Location);
            }
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



        # region Drag and Drop

        int direcao = 0;

        private void telaMenu_MouseDown(object sender, MouseEventArgs e)
        {
            if (status != Status.PosicionandoNavios)
                return;

            TipoDeNavio? navio = gerenciadorDeNavios.NavioEm(e.Location);
            if (navio.HasValue)
            {
                DoDragDrop(navio.Value, DragDropEffects.Copy);
            }
        }

        private void AumentarDirecao()
        {
            direcao = (direcao + 1) % 4;

            AtualizarLabelDirecao();
        }

        private void DiminuirDirecao()
        {
            direcao = --direcao < 0 ? 3 : direcao; // Bem feio mas funciona :-D

            AtualizarLabelDirecao();
        }

        private void AtualizarLabelDirecao()
        {
            if (status ==  Status.PosicionandoNavios)
                switch (direcao)
                {
                    case 0:
                        lblInfo2.Text = "Direção: baixo";
                        break;
                    case 1:
                        lblInfo2.Text = "Direção: esquerda";
                        break;
                    case 2:
                        lblInfo2.Text = "Direção: cima";
                        break;
                    case 3:
                        lblInfo2.Text = "Direção: direita";
                        break;
                }
        }

        private void telaJogador_DragOver(object sender, DragEventArgs e)
        {
            if (sender == null)
                return;

            if (e.Data.GetDataPresent(typeof(TipoDeNavio)))
            {
                e.Effect = DragDropEffects.Copy;
                tJogador.DragOver(telaJogador.PointToClient(new Point(e.X, e.Y)), (TipoDeNavio)e.Data.GetData(typeof(TipoDeNavio)), direcao);
            }
        }

        private void telaJogador_DragDrop(object sender, DragEventArgs e)
        {
            Cursor = Cursors.Default;

            TipoDeNavio navio = (TipoDeNavio)e.Data.GetData(typeof(TipoDeNavio));

            tJogador.DragOver(telaJogador.PointToClient(new Point(e.X, e.Y)), navio, direcao);
            if (tJogador.DragDrop(telaJogador.Width, telaJogador.Height))
            {
                gerenciadorDeNavios.Remover(navio);
                gerenciadorDeNavios.Rearranjar();

                if (tJogador.Tabuleiro.EstaCompleto()) // Terminou de posicionar navios
                {
                    PodeConectar = true;
                }
            }
        }

        private void FrmJogo_KeyDown(object sender, KeyEventArgs e)
        {
            if (status == Status.PosicionandoNavios)
            {
                if (e.KeyCode == Keys.Q)
                {
                    DiminuirDirecao();
                }
                else if (e.KeyCode == Keys.E)
                {
                    AumentarDirecao();
                }
            }
        }

        private void telaJogador_DragLeave(object sender, EventArgs e)
        {
            tJogador.AbortDragDrop();
        }

        #endregion
    }
}
