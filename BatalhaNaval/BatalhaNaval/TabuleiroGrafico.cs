using System;
using System.Drawing;
using Utils;

using Protocolo;

namespace BatalhaNaval
{
    class TabuleiroGrafico : Tabuleiro
    {
        const int GRID_SIZE = 12;
        private readonly int LINE_WIDTH = 0; // em px
        public static readonly Color LINE_COLOR = Color.Black;

        private readonly Image hoverImage, clickImage, idleHoverImage, idleClickImage;

        public TabuleiroGrafico(string idleHoverImage, string idleClickImage, string hoverImage, string clickImage) : base()
        {
            this.idleHoverImage = Image.FromFile(idleHoverImage);
            this.idleClickImage = Image.FromFile(idleClickImage);
            this.hoverImage = Image.FromFile(hoverImage);
            this.clickImage = Image.FromFile(clickImage);
        }

        private void DrawGridLines(Graphics g, float width, float height)
        {
            if (LINE_WIDTH <= 0)
                return;

            Pen p = new Pen(LINE_COLOR, LINE_WIDTH);

            float offset = LINE_WIDTH / 2f;

            // +2 porque inclui as bordas.
            for (int i = 0; i < GRID_SIZE + 2; i++)
            {
                float posy = (i * ((height - LINE_WIDTH) / GRID_SIZE)) + offset,
                      posx = (i * ((width - LINE_WIDTH) / GRID_SIZE)) + offset;

                lock (g)
                {
                    g.DrawLine(p, offset, posy, width + offset, posy);
                    g.DrawLine(p, posx, offset, posx, height + offset);
                }
            }
        }

        private void DrawOnMousePos(Graphics g, float width, float height, Image img)
        {
            Point gridPos = GetMouseGridPos(width, height);

            if (gridPos.X > GRID_SIZE || gridPos.Y > GRID_SIZE)
                throw new InvalidOperationException("As definições do Graphics passado como parâmentro não condizem com" +
                                                    " a posição do mouse passada.");

            lock (g)
                g.DrawImage(img,
                            gridPos.X * ((width - LINE_WIDTH) / GRID_SIZE) + LINE_WIDTH,
                            gridPos.Y * ((height - LINE_WIDTH) / GRID_SIZE) + LINE_WIDTH,
                            width / GRID_SIZE - LINE_WIDTH,
                            height / GRID_SIZE - LINE_WIDTH);
        }

        public void Paint(Graphics g)
        {
            lock (g)
                g.Clear(Color.White);

            float height = g.VisibleClipBounds.Height,
                  width = g.VisibleClipBounds.Width;

            DrawGridLines(g, width, height);

            if (mouseDownPosition != null)
                DrawOnMousePos(g, width, height, clickImage);
            else if (mousePosition != null)
                DrawOnMousePos(g, width, height, hoverImage);
        }

        private Point? mousePosition;
        private Point? mouseDownPosition;

        public Point GetMouseGridPos(float width, float height)
        {
            return new Point(Util.Range(0, width, GRID_SIZE, mousePosition.Value.X),
                             Util.Range(0, height, GRID_SIZE, mousePosition.Value.Y));
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

        public new int NumeroDeColunas
        {
            get { return GRID_SIZE; }
        }

        public new int NumeroDeLinhas
        {
            get { return GRID_SIZE; }
        }
    }
}
