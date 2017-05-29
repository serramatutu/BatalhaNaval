using System;
using System.Collections.Generic;
using System.Drawing;

using Utils;
using Protocolo;
using System.Linq;

namespace BatalhaNaval
{
    class GerenciadorDeNavios
    {
        const int ESPACAMENTO = 20;

        int width, height;

        public bool Arranjado { get; set; } = true;

        public int Width {
            get { return width; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException();

                width = value;
            }
        }

        public int Height
        {
            get { return height; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException();

                height = value;
            }
        }

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
            Height = height;
            Width = width;
            // Coloca todos os navios necessarios no distribuidor
            TipoDeNavio[] ns = (TipoDeNavio[])Enum.GetValues(typeof(TipoDeNavio));

            int x = ESPACAMENTO,
                y = ESPACAMENTO;
            foreach (TipoDeNavio navio in ns)
            {
                int limite = navio.Limite();

                for (int i = 0; i < limite; i++)
                {
                    int w = Imagens[navio].Width,
                        h = Imagens[navio].Height;

                    if (x + w + ESPACAMENTO > width)
                    {
                        y += h + ESPACAMENTO;
                        x = ESPACAMENTO;
                    }

                    navios.Add(new Rectangle(x, y, w, h), navio);
                    x += w + ESPACAMENTO;
                }
            }
        }

        /// <summary>
        /// Remove um navio em uma posição
        /// </summary>
        /// <param name="pos">Posição desejada</param>
        public void Remover(Point pos)
        {
            navios.Remove(navios.First(kvp => Util.PointInRectangle(pos, kvp.Key)).Key);
            Arranjado = false;
        }

        public void Remover(TipoDeNavio navio)
        {
            navios.Remove(navios.First(kvp => kvp.Value == navio).Key);
            Arranjado = false;
        }

        public void Adicionar(TipoDeNavio navio)
        {
            navios.Add(Rectangle.Empty, navio);
            Rearranjar();
        }

        public void Rearranjar()
        {
            // Hiper ineficiente mas muito facil :)
            TipoDeNavio[] ns = navios.Select(kvp => kvp.Value).ToArray();
            TipoDeNavio[] tipos = (TipoDeNavio[])Enum.GetValues(typeof(TipoDeNavio));
            ns = ns.OrderBy(a => Array.FindIndex(tipos, b => b == a)).ToArray();

            navios.Clear();

            int x = ESPACAMENTO,
                y = ESPACAMENTO;
            foreach (TipoDeNavio navio in ns)
            {
                int w = Imagens[navio].Width,
                    h = Imagens[navio].Height;

                if (x + w + ESPACAMENTO > width)
                {
                    y += h + ESPACAMENTO;
                    x = ESPACAMENTO;
                }

                navios.Add(new Rectangle(x, y, w, h), navio);
                x += w + ESPACAMENTO;
            }

            Arranjado = true;
        }

        public void Paint(Graphics g)
        {
            foreach (KeyValuePair<Rectangle, TipoDeNavio> navio in navios)
            {
                Image img = Imagens[navio.Value];
                g.DrawImage(img, navio.Key.X, navio.Key.Y, img.Width, img.Height);
            }
        }

        public TipoDeNavio? NavioEm(Point pos)
        {
            foreach(KeyValuePair<Rectangle, TipoDeNavio> navio in navios)
            {
                if (Util.PointInRectangle(pos, navio.Key))
                    return navio.Value;
            }
            return null;
        }
    }
}
