using Protocolo;
using System.Drawing;
using System;
using System.Collections.Generic;
using Utils;

namespace BatalhaNaval
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


        private void TabuleiroJogador_OnPaint(Graphics g, float width, float height)
        {
            if (mouseDownPosition != null)
                DesenharNaCelulaDoMouse(g, width, height, clickImg);
            else if (mousePosition != null)
                DesenharNaCelulaDoMouse(g, width, height, hoverImg);
        }

        protected override void DesenharNavios(Graphics g, float width, float height)
        {
            foreach(KeyValuePair<int[], TipoDeNavio> navio in Tabuleiro.Navios)
            {
                Bitmap bmp = Util.RotateImage((Bitmap)GerenciadorDeNavios.Imagens[navio.Value], (navio.Key[2] + 1) % 4 * ((float)Math.PI / 2));

                g.DrawImage(bmp, navio.Key[0] * ((width - TAMANHO_LINHA) / TAMANHO_GRADE) + TAMANHO_LINHA,
                                 navio.Key[1] * ((height - TAMANHO_LINHA) / TAMANHO_GRADE) + TAMANHO_LINHA,
                                 Math.Max(navio.Value.Tamanho() * (navio.Key[2] % 2), 1) * ((width - TAMANHO_LINHA) / TAMANHO_GRADE) - TAMANHO_LINHA,
                                 Math.Max(navio.Value.Tamanho() * ((navio.Key[2] + 1) % 2), 1) * ((height - TAMANHO_LINHA) / TAMANHO_GRADE) - TAMANHO_LINHA);
            }
        }
    }
}
