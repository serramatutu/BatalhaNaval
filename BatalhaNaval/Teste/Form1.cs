using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BatalhaNaval;

namespace Teste
{
    public partial class Form1 : Form
    {
        ClienteP2P cl;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Tabuleiro t = new Tabuleiro();
            t.PosicionarNavio(Navio.PortaAvioes, 0, 0, 3);
            t.PosicionarNavio(Navio.Encouracado, 0, 1, 3);
            t.PosicionarNavio(Navio.Encouracado, 0, 2, 3);
            t.PosicionarNavio(Navio.Cruzador, 0, 3, 3);
            t.PosicionarNavio(Navio.Cruzador, 0, 4, 3);
            t.PosicionarNavio(Navio.Cruzador, 0, 5, 3);
            //t.PosicionarNavio(Navio.Submarino, 6, 1, 3);
            //t.PosicionarNavio(Navio.Submarino, 6, 2, 3);
            //t.PosicionarNavio(Navio.Destroier, 0, 8, 3);
            //t.PosicionarNavio(Navio.Destroier, 0, 9, 3);

            cl = new ClienteP2P("Player", null);
            cl.Iniciar();

            cl.OnClienteDisponivel += Cl_OnClienteDisponivel;
        }

        private bool Cl_OnClienteDisponivel(System.Net.IPAddress addr)
        {
            if (InvokeRequired)
                Invoke(new Action(() => { Cl_OnClienteDisponivel(addr); })); 
                
            if (!(comboBox1?.IsDisposed ?? true))
                if (!comboBox1.Items.Contains(addr))
                    comboBox1.Items.Add(addr);

            return true;
        }
    }
}
