namespace BatalhaNaval
{
    /// <summary>
    /// Objeto de tiro, tem uma posição X e uma posição Y
    /// </summary>
    public class Tiro
    {
        /// <summary>
        /// Posição X do tiro
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Posição Y do tiro
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Aplica o tiro em um tabuleiro
        /// </summary>
        /// <param name="tabuleiro">Tabuleiro onde aplicar o tiro</param>
        public ResultadoDeTiro Aplicar(Tabuleiro tabuleiro)
        {
            return tabuleiro.Atirar(X, Y);
        }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="x">Posição X do tiro</param>
        /// <param name="y">Posição Y do tiro</param>
        public Tiro(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}