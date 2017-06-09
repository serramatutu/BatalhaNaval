using System;
using System.Collections.Generic;

namespace BatalhaNaval
{
    /// <summary>
    /// Classe para os mapas de batalha naval
    /// Na verdade é uma matriz esparsa modificada :P
    /// </summary>
    public partial class Tabuleiro
    {
        Celula head;

        /// <summary>
        /// Obtém um array com todos os navios do tabuleiro
        /// </summary>
        public Dictionary<int[], TipoDeNavio> Navios {
            get;
            private set;
        }

        /// <summary>
        /// Obtém ou define a célula na posição especificada da matriz
        /// </summary>
        /// <param name="row">Linha do dado que se deseja obter</param>
        /// <param name="col">Coluna do dado que se deseja obter</param>
        /// <returns>O número na posição passada por parâmetro</returns>
        private Celula this[int row, int col]
        {
            get
            {
                // Se o número de coluna ou o número de linha desejados estiverem
                // fora do limite do mapa, o acesso é inválido
                if (col < 0 || row < 0 ||
                    col >= NumeroDeColunas || row >= NumeroDeLinhas)
                    throw new IndexOutOfRangeException("A coordenada especificada não se encontra na matriz");

                Celula atual = SentinelaParaColuna(col);

                // Move o atual para baixo até que a linha de atual seja maior ou
                // igual à linha desejada
                while (atual != null && atual.Linha < row)
                    atual = atual.ProxVert;

                // Se a linha de atual bate com a desejada, retorna o atual
                if (atual != null && atual.Linha == row)
                    return atual;

                // Se não encontrou o item na posição desejada, retorna o valor padrão
                return null;
            }

            set
            {
                // Se o número de coluna ou o número de linha desejados estiver
                // fora do limite do mapa, o acesso é inválido
                if (col < 0 || row < 0 ||
                    col >= NumeroDeColunas || row >= NumeroDeLinhas)
                    throw new IndexOutOfRangeException("A coordenada especificada não se encontra na matriz");

                Celula atual = SentinelaParaColuna(col),
                       antVert = atual;

                // Move o atual para a linha desejada
                while (atual != null && atual.Linha < row)
                {
                    antVert = atual;
                    atual = atual.ProxVert;
                }

                // Se a linha de atual bate com a desejada, muda o valor do atual
                // Caso o valor seja nulo (0), não faz isso e segue para a exclusão
                // da célula do mapa
                if (atual != null && atual.Linha == row)
                {
                    atual.TipoDeNavio = value.TipoDeNavio;
                    atual.PrimeiraDoNavio = value.PrimeiraDoNavio;
                    atual.ProximaDoNavio = value.ProximaDoNavio;
                }
                else
                {
                    // Obtém o item anterior na horizontal
                    Celula aux = SentinelaParaLinha(row), antHorz = aux;

                    // Move o auxiliar para a maior coluna que seja menor que a 
                    // desejada e tenha uma célula na linha desejada
                    while (aux != null && aux.Coluna < col - 1)
                    {
                        antHorz = aux;
                        aux = aux.ProxHorz;
                    }

                    // Se o valor for 0 e houver uma célula na posição desejada, 
                    // apaga essa célula
                    if (value == null && atual != null && atual.Linha == row)
                    {
                        antVert.ProxVert = atual.ProxVert;
                        antHorz.ProxHorz = atual.ProxHorz;
                    }

                    // Se não, insere uma célula na posição dada
                    else
                    {
                        value.ProxHorz = antHorz.ProxHorz;
                        value.ProxVert = antVert.ProxVert;

                        antHorz.ProxHorz = value;
                        antVert.ProxVert = value;
                    }
                }
            }
        }

        /// <summary>
        /// Obtém a célula-cabeça para a coluna desejada
        /// </summary>
        /// <param name="col">Número da coluna</param>
        private Celula SentinelaParaColuna(int col)
        {
            if (col < 0 || col >= this.NumeroDeColunas)
                throw new Exception("A coordenada especificada não se encontra na matriz");

            Celula cell = this.head;
            while (cell.Coluna < col)
                cell = cell.ProxHorz;

            return cell;
        }

        /// <summary>
        /// Obtém o número de navios de um certo tipo no tabuleiro
        /// </summary>
        /// <param name="tipoDeNavio">O tipo do navio</param>
        /// <returns>O número de navios de um certo tipo no tabuleiro</returns>
        public int Contar(TipoDeNavio tipoDeNavio)
        {
            int n = 0;
            foreach (TipoDeNavio navio in Navios.Values)
                if (navio == tipoDeNavio) n++;

            return n;
        }

        /// <summary>
        /// Obtém a célula-cabeça para a linha desejada
        /// </summary>
        /// <param name="row">Número da linha</param>
        private Celula SentinelaParaLinha(int row)
        {
            if (row < 0 || row >= this.NumeroDeLinhas)
                throw new Exception("A coordenada especificada não se encontra na matriz");

            Celula cell = this.head;
            while (cell.Linha < row)
                cell = cell.ProxVert;

            return cell;
        }

        /// <summary>
        /// Número de colunas do mapa
        /// </summary>
        public int NumeroDeColunas
        {
            get { return 10; }
        }

        /// <summary>
        /// Número de linhas do mapa
        /// </summary>
        public int NumeroDeLinhas
        {
            get { return 10; }
        }

        /// <summary>
        /// Construtor
        /// </summary>
        public Tabuleiro()
        {
            this.head = new Celula(-1, -1, default(TipoDeNavio), null, null);

            Celula aux = null;

            // Cria os nós cabeça
            for (int i = NumeroDeLinhas; i >= 0; --i)
                aux = new Celula(-1, i, 0, null, null, null, aux);
            head.ProxVert = aux;

            aux = null;
            for (int i = NumeroDeColunas; i >= 0; --i)
                aux = new Celula(i, -1, 0, null, null, aux, null);
            head.ProxHorz = aux;

            Navios = new Dictionary<int[], TipoDeNavio>();
        }

        /// <summary>
        /// Posiciona um navio no mapa com a primeira posição dada por X e Y e 
        /// a direção por d.
        /// 
        /// A direção 
        /// </summary>
        /// <param name="tipo">Tipo de navio a ser posicionado</param>
        /// <param name="x">Posição X do navio</param>
        /// <param name="y">Posição Y do navio</param>
        /// <param name="d">Direção do navio.</param>
        /// <exception cref="IndexOutOfRangeException">Se o navio sair dos limites do mapa</exception>
        /// <exception cref="ArgumentException">Se a direção for inválida</exception>
        /// <exception cref="Exception">Se o navio interseccionar com outro ou o tabuleiro já tiver navios demais do tipo passado</exception>
        public void PosicionarNavio(TipoDeNavio tipo, int x, int y, Direcao d)
        {
            if (Contar(tipo) == tipo.Limite())
                throw new Exception("O tabuleiro já tem navios demais desse tipo");

            // Determina o incremento na posição X e Y para a direção dada
            int ix = 0, iy = 0;

            switch ((int)d)
            {
                case 0:
                    iy = 1;
                    break;
                case 1:
                    ix = -1;
                    break;
                case 2:
                    iy = -1;
                    break;
                case 3:
                    ix = 1;
                    break;
                default:
                    throw new ArgumentException("Direção inválida");
            }

            // Gera as células para o navio
            int len = tipo.Tamanho();
            Celula[] celulas = new Celula[len];

            for (int i = celulas.Length - 1; i >= 0; i--)
            {
                int nx = x + ix * i, ny = y + iy * i;
                if (nx < 0 || nx >= NumeroDeColunas || ny < 0 || ny >= NumeroDeLinhas)
                    throw new Exception("Navio fora do tabuleiro");
                celulas[i] = new Celula(nx, ny, tipo, null, i == celulas.Length - 1 ? null : celulas[i + 1]);
            }
            
            // Define a primeiro célula do navio para cada célula do navio e verifica intersecção
            foreach (Celula celula in celulas)
            {
                celula.PrimeiraDoNavio = celulas[0];   // Define o ponteiro para a primeira célula do navio posicionado

                if (this[celula.Linha, celula.Coluna] != null)
                    throw new Exception("Intersecção de navios");
            }

            // Posiciona as células na matriz
            foreach (Celula celula in celulas)
                this[celula.Linha, celula.Coluna] = celula;
            
            Navios.Add(new int[] { x, y, (int) d }, tipo);
        }

        /// <summary>
        /// Atira em uma posição do tabuleiro e retorna o resultado do tiro
        /// </summary>
        /// <param name="x">Coordenada X do tabuleiro onde deve-se dar o tiro</param>
        /// <param name="y">Coordenada Y do tabuleiro onde deve-se dar o tiro</param>
        /// <returns>Um ResultadoDeTiro para o tiro dado</returns>
        /// <exception cref="Exception">Caso o tabuleiro não esteja completo</exception>
        /// <exception cref="IndexOutOfRangeException">Caso alguma das coordenadas esteja fora do mapa</exception>
        public ResultadoDeTiro Atirar(int x, int y)
        {
            if (!EstaCompleto())
                throw new Exception("O tabuleiro não está completo");

            Celula celula = this[y, x];

            // Se não tem nada naquela posição, errou
            if (celula == null)
                return ResultadoDeTiro.Errou;

            // Se não errou, acertou
            ResultadoDeTiro r = ResultadoDeTiro.Acertou | (ResultadoDeTiro)celula.TipoDeNavio;

            // Verifica se afundou o navio
            Celula atual = celula.PrimeiraDoNavio;
            bool afundou = true;
            while (atual != null && afundou)
                afundou = atual.FoiAcertada;

            if (afundou)
                r |= ResultadoDeTiro.Afundou;

            return r;
        }

        /// <summary>
        /// Verifica se o mapa tem todos os requisitos para ser usado no jogo
        /// O mapa é considerado completo se tiver o limite de cada tipo de navio
        /// </summary>
        /// <returns>Verdadeiro se o mapa estiver completo e falso caso contrário</returns>
        public bool EstaCompleto()
        {
            Array navios = (TipoDeNavio[])Enum.GetValues(typeof(TipoDeNavio));

            foreach (TipoDeNavio navio in navios)
                if (Contar(navio) != navio.Limite())
                    return false;

            return true;
        }
    }
}