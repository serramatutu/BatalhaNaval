using System;
using System.Windows.Forms;

using Protocolo;

namespace BatalhaNaval
{
    public partial class FrmConectar : Form
    {
        ClienteP2P cliente;

        public FrmConectar(ClienteP2P cliente)
        {
            InitializeComponent();
            this.cliente = cliente;
        }
    }
}
