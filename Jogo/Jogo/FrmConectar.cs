using System;
using System.Windows.Forms;

using BatalhaNaval;

namespace Jogo
{
    public partial class FrmConectar : Form
    {
        ClienteP2P cliente;
        Tabuleiro tabuleiro;

        public FrmConectar(ClienteP2P cliente, Tabuleiro tabuleiro)
        {
            if (tabuleiro == null)
                throw new ArgumentException("Tabuleiro não pode ser nulo");

            if (!tabuleiro.EstaCompleto())
                throw new ArgumentException("Tabuleiro não pode estar incompleto");

            this.tabuleiro = tabuleiro;
            this.cliente = cliente;

            InitializeComponent();
        }

        private void Cliente_OnClienteIndisponivel(System.Net.IPAddress addr)
        {
            lsbClientes.Items.Remove(addr.ToString());
        }

        private void Cliente_OnClienteConectado(System.Net.IPAddress addr)
        {
            //throw new NotImplementedException();
        }

        private void Cliente_OnClienteDisponivel(System.Net.IPAddress addr)
        {
            lsbClientes.Items.Add(addr.ToString());
        }

        private void txtNome_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtNome.Text))
                btnConectar.Enabled = false;
            else
            {
                btnConectar.Enabled = true;

                cliente = new ClienteP2P(txtNome.Text, tabuleiro);

                cliente.OnClienteDisponivel += Cliente_OnClienteDisponivel;
                cliente.OnClienteIndisponivel += Cliente_OnClienteIndisponivel;
                cliente.OnClienteConectado += Cliente_OnClienteConectado;

                cliente.Iniciar();
            }
                
        }
    }
}
