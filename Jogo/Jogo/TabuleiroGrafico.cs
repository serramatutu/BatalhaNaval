using System;
using System.Drawing;
using Utils;

using BatalhaNaval;

namespace Jogo
{
    abstract class TabuleiroGrafico
    {
        protected const int TAMANHO_GRADE = 10;
        public readonly int TAMANHO_LINHA = 2; // em px
        public static readonly Color COR_LINHA = Color.Black;

        #region Metodos de Pintar

        protected void DesenharLinhas(Graphics g, float width, float height)
        {
            if (TAMANHO_LINHA <= 0)
                return;

            Pen p = new Pen(COR_LINHA, TAMANHO_LINHA);

            float offset = TAMANHO_LINHA / 2f;

            // +2 porque inclui as bordas.
            for (int i = 0; i < TAMANHO_GRADE + 2; i++)
            {
                float posy = (i * ((height - TAMANHO_LINHA) / TAMANHO_GRADE)) + offset,
                      posx = (i * ((width - TAMANHO_LINHA) / TAMANHO_GRADE)) + offset;

                lock (g)
                {
                    g.DrawLine(p, offset, posy, width + offset, posy);
                    g.DrawLine(p, posx, offset, posx, height + offset);
                }
            }
        }

        protected abstract void DesenharNavios(Graphics g, float width, float height);

        protected void DesenharNaCelulaDoMouse(Graphics g, float width, float height, Image img)
        {
            Point gridPos = GetMouseGridPos(width, height);

            if (gridPos.X > TAMANHO_GRADE || gridPos.Y > TAMANHO_GRADE)
                throw new InvalidOperationException("As definições do Graphics passado como parâmentro não condizem com" +
                                                    " a posição do mouse passada.");

            lock (g)
                g.DrawImage(img,
                            gridPos.X * ((width - TAMANHO_LINHA) / TAMANHO_GRADE) + TAMANHO_LINHA,
                            gridPos.Y * ((height - TAMANHO_LINHA) / TAMANHO_GRADE) + TAMANHO_LINHA,
                            width / TAMANHO_GRADE - TAMANHO_LINHA,
                            height / TAMANHO_GRADE - TAMANHO_LINHA);
        }

        #endregion

        #region Eventos Paint
        public void Paint(Graphics g)
        {
            lock (g)
                g.Clear(Color.White);

            float height = g.VisibleClipBounds.Height,
                  width = g.VisibleClipBounds.Width;

            DesenharLinhas(g, width, height);
            DesenharNavios(g, width, height);

            OnPaint(g, width, height);
        }

        protected delegate void PaintEvent(Graphics g, float width, float height);
        protected event PaintEvent OnPaint;

        #endregion

        #region Mouse
        protected Point? mousePosition;
        protected Point? mouseDownPosition;

        public Point GetMouseGridPos(float width, float height)
        {
            return new Point(Util.Range(0, width, TAMANHO_GRADE, mousePosition.Value.X),
                             Util.Range(0, height, TAMANHO_GRADE, mousePosition.Value.Y));
        }

        public void MouseLeave()
        {
            this.mousePosition = null;
        }

        public void MouseMove(Point pos)
        {
            this.mousePosition = pos;
        }

        public void MouseDown(Point pos)
        {
            this.mouseDownPosition = pos;
        }

        public void MouseUp()
        {
            this.mouseDownPosition = null;
        }

        #endregion
    }
}
