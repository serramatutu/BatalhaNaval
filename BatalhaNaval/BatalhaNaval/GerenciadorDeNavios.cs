using System;
using System.Collections.Generic;
using System.Drawing;

using Protocolo;

namespace BatalhaNaval
{
    class GerenciadorDeNavios
    {
        Dictionary<Rectangle, TipoDeNavio> navios = new Dictionary<Rectangle, TipoDeNavio>();

        public static Dictionary<TipoDeNavio, Image> Imagens { get; } = new Dictionary<TipoDeNavio, Image>
        {
            { TipoDeNavio.Cruzador,    Image.FromFile("../../resources/navios/cruzador.png") },
            { TipoDeNavio.Destroier,   Image.FromFile("../../resources/navios/destroier.png") },
            { TipoDeNavio.Encouracado, Image.FromFile("../../resources/navios/encouracado.png") },
            { TipoDeNavio.PortaAvioes, Image.FromFile("../../resources/navios/porta-avioes.png") },
            { TipoDeNavio.Submarino,   Image.FromFile("../../resources/navios/submarino.png") }
        };

        public GerenciadorDeNavios(int width, int height)
        {
            // Coloca todos os navios necessarios no distribuidor
            TipoDeNavio[] ns = (TipoDeNavio[])Enum.GetValues(typeof(TipoDeNavio));

            int x = 15,
                y = 15;
            foreach (TipoDeNavio navio in ns)
            {
                int limite = navio.Limite();

                for (int i = 0; i < limite; i++)
                {
                    int w = Imagens[navio].Width,
                        h = Imagens[navio].Height;

                    if (x + w + 15 > width)
                    {
                        y += h + 15;
                        x = 15;
                    }

                    navios.Add(new Rectangle(x, y, w, h), navio);
                    x += w + 15;
                }
            }
        }

        public void Rearranjar(int width, int height)
        {
            throw new NotImplementedException();
        }

        public void Paint(Graphics g)
        {
            foreach (KeyValuePair<Rectangle, TipoDeNavio> navio in navios)
            {
                Image img = Imagens[navio.Value];
                g.DrawImage(img, navio.Key.X, navio.Key.Y, img.Width, img.Height);
            }
        }
    }
}
