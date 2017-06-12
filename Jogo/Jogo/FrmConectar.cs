using System;
using System.Windows.Forms;

using BatalhaNaval;
using System.Net;
using System.Collections.Generic;

namespace Jogo
{
    public partial class FrmConectar : Form
    {
        public ClienteP2P Cliente { get; private set; }

        Tabuleiro tabuleiro;

        List<IPAddress> clientesDisponiveis = new List<IPAddress>();

        public bool Conectado { get; private set; }

        public FrmConectar(ClienteP2P cliente, Tabuleiro tabuleiro)
        {
            if (tabuleiro == null)
                throw new ArgumentException("Tabuleiro não pode ser nulo");

            if (!tabuleiro.EstaCompleto())
                throw new ArgumentException("Tabuleiro não pode estar incompleto");

            this.tabuleiro = tabuleiro;
            Cliente = cliente;
            Conectado = false;

            InitializeComponent();
        }

        private void AtualizarListbox()
        {
            lsbClientes.Items.Clear();

            foreach (IPAddress c in clientesDisponiveis)
                lsbClientes.Items.Add(c);
        }

        private void Cliente_OnClienteIndisponivel(IPAddress addr)
        {
            if (InvokeRequired && !Disposing)
                try
                {
                    Invoke(new Action(delegate () {
                        Cliente_OnClienteDisponivel(addr);
                    }));
                }
                catch { }
            else
            {
                clientesDisponiveis.Remove(addr);
                AtualizarListbox();
            }
        }

        private void Cliente_OnClienteConectado(IPAddress addr)
        {
            if (InvokeRequired && !Disposing)
                try
                {
                    Invoke(new Action(delegate () {
                        Cliente_OnClienteConectado(addr);
                    }));
                }
                catch { }
            else
            {
                //Cliente.Close();

                //Cliente.OnClienteDisponivel -= Cliente_OnClienteDisponivel;
                //Cliente.OnClienteConectado -= Cliente_OnClienteConectado;
                //Cliente.OnClienteIndisponivel -= Cliente_OnClienteIndisponivel;
                //Cliente.OnClienteRequisitandoConexao -= Cliente_OnClienteRequisitandoConexao;

                DialogResult = DialogResult.OK;
            }
        }

        private void Cliente_OnClienteDisponivel(IPAddress addr)
        {
            if (InvokeRequired && !Disposing) 
                try
                {
                    Invoke(new Action(delegate () {
                        Cliente_OnClienteDisponivel(addr);
                    }));
                }
                catch { }
            else
            {
                if (!clientesDisponiveis.Contains(addr))
                {
                    clientesDisponiveis.Add(addr);
                    AtualizarListbox();
                }
            }
        }

        private bool Cliente_OnClienteRequisitandoConexao(IPAddress addr)
        {
            if (InvokeRequired && !Disposing)
            {
                try
                {
                    return (bool)Invoke(new Func<bool>(() => {
                        return Cliente_OnClienteRequisitandoConexao(addr);
                    }));
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                if (Conectado)
                    return false;

                return MessageBox.Show(this, "Deseja se conectar com " + addr.ToString() + "?",
                                       "Solicitação de " + addr.ToString(),
                                       MessageBoxButtons.YesNo,
                                       MessageBoxIcon.Question,
                                       MessageBoxDefaultButton.Button1) == DialogResult.Yes;
            }
            
        }

        private void txtNome_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtNome.Text))
                btnMain.Enabled = false;
            else
                btnMain.Enabled = true;
        }

        #region Botao

        private enum StatusBotao
        {
            Conectar,
            Procurar,
            Cancelar
        }

        private StatusBotao status = StatusBotao.Procurar;

        private StatusBotao Status
        {
            get
            {
                return status;
            }

            set
            {
                txtNome.Enabled = true;

                if (value == StatusBotao.Procurar)
                {
                    btnMain.Text = "Procurar";
                    txtNome.Enabled = false;
                }
                else if (value == StatusBotao.Cancelar)
                    btnMain.Text = "Cancelar";
                else
                    btnMain.Text = "Conectar";
                   
                status = value;
            }
        }

        private void btnMain_Click(object sender, EventArgs e)
        {
            if (status == StatusBotao.Procurar)
            {
                Status = StatusBotao.Cancelar;

                Cliente = new ClienteP2P(txtNome.Text, tabuleiro);

                OnConfigurarCliente?.Invoke(Cliente);

                Cliente.OnClienteDisponivel += Cliente_OnClienteDisponivel;
                Cliente.OnClienteConectado += Cliente_OnClienteConectado;
                Cliente.OnClienteIndisponivel += Cliente_OnClienteIndisponivel;
                Cliente.OnClienteRequisitandoConexao += Cliente_OnClienteRequisitandoConexao;
    
                Cliente.Iniciar();
            }
            else if (status == StatusBotao.Conectar)
            {
                Cliente.SolicitarConexao(clientesDisponiveis[lsbClientes.SelectedIndex]);
            }
            else
            {
                Cliente = null;
                AtualizarListbox();
                Status = StatusBotao.Procurar;
            }
        }

        #endregion

        #region Configurar Cliente

        public delegate void EventoConfigurarCliente(ClienteP2P cliente);

        public event EventoConfigurarCliente OnConfigurarCliente;

        #endregion

        private void lsbClientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lsbClientes.SelectedIndex < 0)
                Status = StatusBotao.Cancelar;
            else
                Status = StatusBotao.Conectar;
        }
    }
}
