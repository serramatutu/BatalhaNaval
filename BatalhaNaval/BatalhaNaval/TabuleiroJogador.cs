using Protocolo;
using System.Drawing;

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
    }
}
