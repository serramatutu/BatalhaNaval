using BatalhaNaval;
using System.Drawing;
using System;
using System.Collections.Generic;
using Utils;

namespace Jogo
{
    sealed class TabuleiroJogador : TabuleiroGrafico
    {
        public Tabuleiro Tabuleiro { get; set; } = new Tabuleiro();

        private readonly Image hoverImg = Image.FromFile("../../resources/playerHover.png"),
                               clickImg = Image.FromFile("../../resources/playerClick.png");

        public TabuleiroJogador()
        {
            OnPaint += TabuleiroJogador_OnPaint;
        }

        #region Drag and Drop

        public bool DragDrop(float width, float height)
        {
            if (navioArrastado == null)
                throw new InvalidOperationException();

            navioArrastado.Pos = GetMouseGridPos(width, height);

            bool ret = true;
            try
            {
                Tabuleiro.PosicionarNavio(navioArrastado.Navio, navioArrastado.Pos.X, navioArrastado.Pos.Y, (Direcao)navioArrastado.Direcao);
            }
            catch
            {
                ret = false;
            }
            
            navioArrastado = null;
            return ret;
        }

        public void AbortDragDrop()
        {
            navioArrastado = null;
            MouseLeave();
        }

        private class NavioInfo
        {
            public Point Pos { get; set; }

            public TipoDeNavio Navio { get; set; }

            public int Direcao { get; set; }

            public bool Valido { get; set; }

            public NavioInfo(TipoDeNavio navio, int direcao, Point pos)
            {
                Navio = navio;
                Direcao = direcao;
                Pos = pos;
                Valido = false;
            }
        }

        NavioInfo navioArrastado = null;

        public void DragOver(Point pos, TipoDeNavio navio, int direcao)
        {
            MouseMove(pos);
            navioArrastado = new NavioInfo(navio, direcao, pos); // Inicialmente, coloca a posição literal do mouse no navio
        }

        #endregion

        private void TabuleiroJogador_OnPaint(Graphics g, float width, float height)
        {
            if (mouseDownPosition != null)
                DesenharNaCelulaDoMouse(g, width, height, clickImg);
            else if (mousePosition != null)
            {
                if (navioArrastado == null)
                    DesenharNaCelulaDoMouse(g, width, height, hoverImg);
                else
                {
                    navioArrastado.Pos = GetMouseGridPos(width, height);

                    navioArrastado.Valido = true;

                    Bitmap bmp = new Bitmap(GerenciadorDeNavios.Imagens[navioArrastado.Navio]);

                    int offsetX = 0,
                        offsetY = 0;

                    switch (navioArrastado.Direcao) // Calcula se a posição do navio é valida
                    {
                        case 0: // Baixo
                            bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);

                            if (navioArrastado.Pos.X < 0 || navioArrastado.Pos.X >= Tabuleiro.NumeroDeLinhas)
                                navioArrastado.Valido = false;

                            if (navioArrastado.Pos.Y < 0 || navioArrastado.Pos.Y + navioArrastado.Navio.Tamanho() - 1 >= Tabuleiro.NumeroDeColunas)
                                navioArrastado.Valido = false;
                            break;

                        case 1: // Esquerda
                            offsetX = navioArrastado.Navio.Tamanho() - 1;

                            if (navioArrastado.Pos.X - offsetX < 0 || navioArrastado.Pos.X >= Tabuleiro.NumeroDeLinhas)
                                navioArrastado.Valido = false;

                            if (navioArrastado.Pos.Y < 0 || navioArrastado.Pos.Y >= Tabuleiro.NumeroDeColunas)
                                navioArrastado.Valido = false;
                            break;

                        case 2: // Cima
                            offsetY = navioArrastado.Navio.Tamanho() - 1;
                            bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);

                            if (navioArrastado.Pos.X < 0 || navioArrastado.Pos.X >= Tabuleiro.NumeroDeLinhas)
                                navioArrastado.Valido = false;

                            if (navioArrastado.Pos.Y - offsetY < 0 || navioArrastado.Pos.Y >= Tabuleiro.NumeroDeColunas)
                                navioArrastado.Valido = false;
                            break;

                        case 3: // Direita
                            if (navioArrastado.Pos.X < 0 || navioArrastado.Pos.X + navioArrastado.Navio.Tamanho() - 1 >= Tabuleiro.NumeroDeLinhas)
                                navioArrastado.Valido = false;

                            if (navioArrastado.Pos.Y < 0 || navioArrastado.Pos.Y >= Tabuleiro.NumeroDeColunas)
                                navioArrastado.Valido = false;
                            break;
                    }

                    Brush brush = navioArrastado.Valido ? Brushes.Green : Brushes.Red;

                    g.FillRectangle(brush, 
                                    (navioArrastado.Pos.X - offsetX) * ((width - TAMANHO_LINHA) / TAMANHO_GRADE) + TAMANHO_LINHA,
                                    (navioArrastado.Pos.Y - offsetY) * ((height - TAMANHO_LINHA) / TAMANHO_GRADE) + TAMANHO_LINHA,
                                    Math.Max(navioArrastado.Navio.Tamanho() * (navioArrastado.Direcao % 2), 1) * ((width - TAMANHO_LINHA) / TAMANHO_GRADE) - TAMANHO_LINHA,
                                    Math.Max(navioArrastado.Navio.Tamanho() * ((navioArrastado.Direcao + 1) % 2), 1) * ((height - TAMANHO_LINHA) / TAMANHO_GRADE) - TAMANHO_LINHA);
                }
            }
                
        }

        protected override void DesenharNavios(Graphics g, float width, float height)
        {
            foreach(KeyValuePair<int[], TipoDeNavio> navio in Tabuleiro.Navios)
            {
                //Bitmap bmp = Util.RotateImage((Bitmap)GerenciadorDeNavios.Imagens[navio.Value], (navio.Key[2] + 1) % 4 * ((float)Math.PI / 2));
                Bitmap bmp = new Bitmap(GerenciadorDeNavios.Imagens[navio.Value]);

                int offsetX = 0,
                    offsetY = 0;

                switch (navio.Key[2])
                {
                    case 0: // Baixo
                        bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;

                    case 1: // Direita
                        offsetX = navio.Value.Tamanho() - 1;
                        break;

                    case 2: // Cima
                        offsetY = navio.Value.Tamanho() - 1;
                        bmp.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        break;
                    
                }

                g.DrawImage(bmp, (navio.Key[0] - offsetX) * ((width - TAMANHO_LINHA) / TAMANHO_GRADE) + TAMANHO_LINHA,
                                 (navio.Key[1] - offsetY) * ((height - TAMANHO_LINHA) / TAMANHO_GRADE) + TAMANHO_LINHA,
                                 Math.Max(navio.Value.Tamanho() * (navio.Key[2] % 2), 1) * ((width - TAMANHO_LINHA) / TAMANHO_GRADE) - TAMANHO_LINHA,
                                 Math.Max(navio.Value.Tamanho() * ((navio.Key[2] + 1) % 2), 1) * ((height - TAMANHO_LINHA) / TAMANHO_GRADE) - TAMANHO_LINHA);
            }
        }
    }
}
