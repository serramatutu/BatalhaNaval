namespace BatalhaNaval
{
    /// <summary>
    /// Classe para os mapas de batalha naval
    /// Na verdade é uma matriz esparsa :P
    /// </summary>
    public partial class Tabuleiro
    {
        /// <summary>
        /// Célula do mapa, tem uma posição e indica o tipo de navio
        /// </summary>
        private class Celula
        {
            /// <summary>
            /// Tipo de navio da célula
            /// </summary>
            public TipoDeNavio TipoDeNavio { get; set; }

            /// <summary>
            /// Número da coluna da célula
            /// </summary>
            public int Coluna { get; private set; }

            /// <summary>
            /// Número da linha da célula
            /// </summary>
            public int Linha { get; private set; }

            /// <summary>
            /// Ponteiro para a próxima célula no mapa que represente o mesmo navio
            /// </summary>
            public Celula ProximaDoNavio { get; set; }

            /// <summary>
            /// Ponteiro para a primeira célula representando o mesmo navio que esta
            /// </summary>
            public Celula PrimeiraDoNavio { get; set; }

            /// <summary>
            /// Próximo nó da lista na horizontal
            /// </summary>
            public Celula ProxHorz { get; set; }

            /// <summary>
            /// Próximo nó da lista na vertical
            /// </summary>
            public Celula ProxVert { get; set; }

            /// <summary>
            /// Define se a célula foi acertada pelo outro jogado ou não
            /// </summary>
            public bool FoiAcertada { get; set; }

            /// <summary>
            /// Construtor
            /// </summary>
            /// <param name="col">Número da coluna da célula</param>
            /// <param name="row">Número da linha da célula</param>
            /// <param name="tipo">Tipo de navio da célula</param>
            /// <param name="prox">Próxima célula do mesmo navio</param>
            /// <param name="proxHorz">Próxima célula na horizontal</param>
            /// <param name="proxVert">Próxima célula na vertical</param>
            public Celula(int col, int row, TipoDeNavio tipo, Celula primeira, Celula prox, Celula proxHorz = null, Celula proxVert = null)
            {
                Coluna = col;
                Linha = row;
                TipoDeNavio = tipo;
                ProximaDoNavio = prox;
                ProxHorz = proxHorz;
                ProxVert = proxVert;
                FoiAcertada = false;
            }
        }
    }
}
