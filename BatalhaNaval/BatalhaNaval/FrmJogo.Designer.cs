namespace BatalhaNaval
{
    partial class FrmJogo
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.telaInimigo = new System.Windows.Forms.PictureBox();
            this.telaJogador = new System.Windows.Forms.PictureBox();
            this.telaMenu = new System.Windows.Forms.PictureBox();
            this.animTimer = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.txtCoordenadas = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.telaInimigo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.telaJogador)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.telaMenu)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // telaInimigo
            // 
            this.telaInimigo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.telaInimigo.BackColor = System.Drawing.SystemColors.Control;
            this.telaInimigo.Location = new System.Drawing.Point(12, 12);
            this.telaInimigo.Name = "telaInimigo";
            this.telaInimigo.Size = new System.Drawing.Size(512, 512);
            this.telaInimigo.TabIndex = 0;
            this.telaInimigo.TabStop = false;
            this.telaInimigo.Paint += new System.Windows.Forms.PaintEventHandler(this.tela_Paint);
            this.telaInimigo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tela_MouseDown);
            this.telaInimigo.MouseLeave += new System.EventHandler(this.tela_MouseLeave);
            this.telaInimigo.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tela_MouseMove);
            this.telaInimigo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tela_MouseUp);
            // 
            // telaJogador
            // 
            this.telaJogador.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.telaJogador.BackColor = System.Drawing.SystemColors.Control;
            this.telaJogador.Location = new System.Drawing.Point(570, 12);
            this.telaJogador.Name = "telaJogador";
            this.telaJogador.Size = new System.Drawing.Size(512, 512);
            this.telaJogador.TabIndex = 1;
            this.telaJogador.TabStop = false;
            this.telaJogador.Paint += new System.Windows.Forms.PaintEventHandler(this.tela_Paint);
            this.telaJogador.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tela_MouseDown);
            this.telaJogador.MouseLeave += new System.EventHandler(this.tela_MouseLeave);
            this.telaJogador.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tela_MouseMove);
            this.telaJogador.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tela_MouseUp);
            // 
            // telaMenu
            // 
            this.telaMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.telaMenu.BackColor = System.Drawing.SystemColors.Control;
            this.telaMenu.Location = new System.Drawing.Point(12, 530);
            this.telaMenu.Name = "telaMenu";
            this.telaMenu.Size = new System.Drawing.Size(1070, 146);
            this.telaMenu.TabIndex = 2;
            this.telaMenu.TabStop = false;
            // 
            // animTimer
            // 
            this.animTimer.Enabled = true;
            this.animTimer.Interval = 16;
            this.animTimer.Tick += new System.EventHandler(this.animTimer_Tick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.txtCoordenadas});
            this.statusStrip1.Location = new System.Drawing.Point(0, 685);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1094, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // txtCoordenadas
            // 
            this.txtCoordenadas.Name = "txtCoordenadas";
            this.txtCoordenadas.Size = new System.Drawing.Size(30, 17);
            this.txtCoordenadas.Text = "X: Y:";
            // 
            // FrmJogo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1094, 707);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.telaMenu);
            this.Controls.Add(this.telaJogador);
            this.Controls.Add(this.telaInimigo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FrmJogo";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "Batalha Naval";
            ((System.ComponentModel.ISupportInitialize)(this.telaInimigo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.telaJogador)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.telaMenu)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox telaInimigo;
        private System.Windows.Forms.PictureBox telaJogador;
        private System.Windows.Forms.PictureBox telaMenu;
        private System.Windows.Forms.Timer animTimer;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel txtCoordenadas;
    }
}

