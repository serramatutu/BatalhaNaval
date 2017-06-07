using Protocolo;

namespace BatalhaNaval
{
    struct NavioInfo
    {
        public TipoDeNavio Navio { get; set; }

        public int Direcao { get; set; }

        public NavioInfo(TipoDeNavio navio, int direcao)
        {
            Navio = navio;
            Direcao = direcao;
        }
    }
}
