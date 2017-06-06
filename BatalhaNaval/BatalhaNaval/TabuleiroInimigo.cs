using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatalhaNaval
{
    enum StatusCelula
    {
        Desconhecido,
        Navio,
        Agua
    }

    sealed class TabuleiroInimigo : TabuleiroGrafico
    {
        Dictionary<Point, StatusCelula> celulas = new Dictionary<Point, StatusCelula>();

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
            if (PodeAtirar)
            {
                if (mouseDownPosition != null)
                    DesenharNaCelulaDoMouse(g, width, height, clickImg);
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

        protected override void DesenharNavios(Graphics g, float width, float height)
        {
            //throw new NotImplementedException();
        }
    }
}
