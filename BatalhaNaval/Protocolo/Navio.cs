namespace Protocolo
{
    /// <summary>
    /// Enumerador para os tipos de navio
    /// 
    /// É muita magia, não mexa
    /// </summary>
    public enum TipoDeNavio : uint
    {
        PortaAvioes = 0x0105,
        Encouracado = 0x0204,
        Cruzador    = 0x0303,
        Destroier   = 0x0202,
        Submarino   = 0x0201
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
