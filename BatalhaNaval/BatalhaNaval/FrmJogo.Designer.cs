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
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblCoord = new System.Windows.Forms.Label();
            this.pnlBotoes = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.telaInimigo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.telaJogador)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.telaMenu)).BeginInit();
            this.pnlStatus.SuspendLayout();
            this.pnlBotoes.SuspendLayout();
            this.SuspendLayout();
            // 
            // telaInimigo
            // 
            this.telaInimigo.BackColor = System.Drawing.Color.White;
            this.telaInimigo.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.telaInimigo.Location = new System.Drawing.Point(12, 12);
            this.telaInimigo.Name = "telaInimigo";
            this.telaInimigo.Size = new System.Drawing.Size(400, 400);
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
            this.telaJogador.BackColor = System.Drawing.Color.White;
            this.telaJogador.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.telaJogador.Location = new System.Drawing.Point(418, 12);
            this.telaJogador.Name = "telaJogador";
            this.telaJogador.Size = new System.Drawing.Size(400, 400);
            this.telaJogador.TabIndex = 1;
            this.telaJogador.TabStop = false;
            //this.telaJogador.DragDrop += new System.Windows.Forms.DragEventHandler(this.telaJogador_DragDrop);
            //this.telaJogador.DragOver += new System.Windows.Forms.DragEventHandler(this.telaJogador_DragOver);
            this.telaJogador.Paint += new System.Windows.Forms.PaintEventHandler(this.tela_Paint);
            this.telaJogador.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tela_MouseDown);
            this.telaJogador.MouseLeave += new System.EventHandler(this.tela_MouseLeave);
            this.telaJogador.MouseMove += new System.Windows.Forms.MouseEventHandler(this.tela_MouseMove);
            this.telaJogador.MouseUp += new System.Windows.Forms.MouseEventHandler(this.tela_MouseUp);
            // 
            // telaMenu
            // 
            this.telaMenu.BackColor = System.Drawing.Color.White;
            this.telaMenu.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.telaMenu.Location = new System.Drawing.Point(12, 424);
            this.telaMenu.Name = "telaMenu";
            this.telaMenu.Size = new System.Drawing.Size(806, 165);
            this.telaMenu.TabIndex = 2;
            this.telaMenu.TabStop = false;
            this.telaMenu.Paint += new System.Windows.Forms.PaintEventHandler(this.telaMenu_Paint);
            this.telaMenu.MouseDown += new System.Windows.Forms.MouseEventHandler(this.telaMenu_MouseDown);
            // 
            // animTimer
            // 
            this.animTimer.Enabled = true;
            this.animTimer.Interval = 16;
            this.animTimer.Tick += new System.EventHandler(this.animTimer_Tick);
            // 
            // pnlStatus
            // 
            this.pnlStatus.BackColor = System.Drawing.Color.White;
            this.pnlStatus.Controls.Add(this.lblStatus);
            this.pnlStatus.Controls.Add(this.lblCoord);
            this.pnlStatus.Location = new System.Drawing.Point(824, 12);
            this.pnlStatus.Name = "pnlStatus";
            this.pnlStatus.Padding = new System.Windows.Forms.Padding(10);
            this.pnlStatus.Size = new System.Drawing.Size(195, 400);
            this.pnlStatus.TabIndex = 3;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.ForeColor = System.Drawing.Color.Blue;
            this.lblStatus.Location = new System.Drawing.Point(13, 10);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(124, 15);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "Escolhendo posições";
            // 
            // lblCoord
            // 
            this.lblCoord.AutoSize = true;
            this.lblCoord.Location = new System.Drawing.Point(13, 372);
            this.lblCoord.Name = "lblCoord";
            this.lblCoord.Size = new System.Drawing.Size(53, 15);
            this.lblCoord.TabIndex = 0;
            this.lblCoord.Text = "X: * / Y: *";
            // 
            // pnlBotoes
            // 
            this.pnlBotoes.BackColor = System.Drawing.Color.White;
            this.pnlBotoes.Controls.Add(this.label1);
            this.pnlBotoes.Location = new System.Drawing.Point(824, 424);
            this.pnlBotoes.Name = "pnlBotoes";
            this.pnlBotoes.Padding = new System.Windows.Forms.Padding(10);
            this.pnlBotoes.Size = new System.Drawing.Size(194, 165);
            this.pnlBotoes.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(77, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Botoes aqui";
            // 
            // FrmJogo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1030, 601);
            this.Controls.Add(this.pnlBotoes);
            this.Controls.Add(this.pnlStatus);
            this.Controls.Add(this.telaMenu);
            this.Controls.Add(this.telaJogador);
            this.Controls.Add(this.telaInimigo);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FrmJogo";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "Batalha Naval";
            this.Shown += new System.EventHandler(this.FrmJogo_Shown);
            //this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FrmJogo_MouseDown);
            ((System.ComponentModel.ISupportInitialize)(this.telaInimigo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.telaJogador)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.telaMenu)).EndInit();
            this.pnlStatus.ResumeLayout(false);
            this.pnlStatus.PerformLayout();
            this.pnlBotoes.ResumeLayout(false);
            this.pnlBotoes.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox telaInimigo;
        private System.Windows.Forms.PictureBox telaJogador;
        private System.Windows.Forms.PictureBox telaMenu;
        private System.Windows.Forms.Timer animTimer;
        private System.Windows.Forms.Panel pnlStatus;
        private System.Windows.Forms.Label lblCoord;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Panel pnlBotoes;
        private System.Windows.Forms.Label label1;
    }
}

