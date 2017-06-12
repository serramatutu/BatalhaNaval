using BatalhaNaval;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jogo
{
    enum StatusCelula
    {
        Desconhecido, // Desconhecido é o valor padrão
        Navio,
        Agua
    }

    sealed class TabuleiroInimigo : TabuleiroGrafico
    {
        StatusCelula[,] celulas = new StatusCelula[TAMANHO_GRADE, TAMANHO_GRADE]; 

        public bool PodeAtirar { get; set; }

        private static readonly Image hoverImg = Image.FromFile("../../resources/enemyHover.png"),
                                      clickImg = Image.FromFile("../../resources/enemyClick.png"),
                                      idleHoverImg = Image.FromFile("../../resources/idleHover.png"),
                                      idleClickImg = Image.FromFile("../../resources/idleClick.png");

        public TabuleiroInimigo()
        {
            OnPaint += TabuleiroInimigo_OnPaint;
        }

        private void TabuleiroInimigo_OnPaint(Graphics g, float width, float height)
        {
            DesenharCelulas(g, width, height);
            if (PodeAtirar)
            {
                if (mouseDownPosition != null)
                {
                    DesenharNaCelulaDoMouse(g, width, height, clickImg);
                    Point pos = GetMouseGridPos(width, height);

                    OnTiroDado?.Invoke(pos.X, pos.Y);
                    PodeAtirar = false;
                }
                    
                else if (mousePosition != null)
                    DesenharNaCelulaDoMouse(g, width, height, hoverImg);
            }
            else
            {
                if (mouseDownPosition != null)
                    DesenharNaCelulaDoMouse(g, width, height, idleClickImg);
                else if (mousePosition != null)
                    DesenharNaCelulaDoMouse(g, width, height, idleHoverImg);
            }
        }

        public delegate void EventoTiroDado(int x, int y);

        public event EventoTiroDado OnTiroDado;

        public void ResultadoTiro(Tiro t, ResultadoDeTiro r)
        {
            StatusCelula s = StatusCelula.Agua;

            if (r == ResultadoDeTiro.Acertou || r == ResultadoDeTiro.Afundou)
                s = StatusCelula.Navio;

            celulas[t.X, t.Y] = s;
        }

        public void DesenharCelulas(Graphics g, float width, float height)
        {
            for (int i=0; i<celulas.GetLength(0); i++)
                for (int j=0; j<celulas.GetLength(1); j++)
                {
                    Brush b = null;
                    switch (celulas[i, j])
                    {
                        case StatusCelula.Desconhecido:
                            b = Brushes.LightGray;
                            break;
                        case StatusCelula.Agua:
                            b = Brushes.Aquamarine;
                            break;
                        case StatusCelula.Navio:
                            b = Brushes.Red;
                            break;
                    }

                    g.FillRectangle(b,
                                    i * ((width - TAMANHO_LINHA) / TAMANHO_GRADE) + TAMANHO_LINHA,
                                    j * ((height - TAMANHO_LINHA) / TAMANHO_GRADE) + TAMANHO_LINHA,
                                    ((width - TAMANHO_LINHA) / TAMANHO_GRADE) - TAMANHO_LINHA,
                                    ((height - TAMANHO_LINHA) / TAMANHO_GRADE) - TAMANHO_LINHA);
                }
        }
    }
}
