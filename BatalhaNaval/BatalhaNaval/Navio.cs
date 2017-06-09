namespace BatalhaNaval
{
    public enum Direcao
    {
        Baixo    = 0,
        Esquerda = 1,
        Cima     = 2,
        Direita  = 3
    }

    /// <summary>
    /// Enumerador para os tipos de navio
    /// 
    /// É muita magia, não mexa
    /// </summary>
    public enum TipoDeNavio : uint
    {
        PortaAvioes = 0x00105,
        Encouracado = 0x00104,
        Cruzador    = 0x00103,
        Destroier   = 0x00102,
        Submarino   = 0x10102
    }

    /// <summary>
    /// Classe de extensão para o enumerador de tipos de navio
    /// </summary>
    public static class Navio_Ex
    {
        /// <summary>
        /// Obtém o tamanho do navio acertado por um tiro
        /// </summary>
        public static int Tamanho(this TipoDeNavio nav)
        {
            return (int)((uint)nav & 0xff);
        }

        /// <summary>
        /// Obtém o limite de um tipo de navio no mapa
        /// </summary>
        public static int Limite(this TipoDeNavio nav)
        {
            return (int)(((uint)nav & 0xff00) >> 8);
        }
    }
}
