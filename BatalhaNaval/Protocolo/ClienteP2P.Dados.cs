using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Timers;

namespace Protocolo
{
    /// <summary>
    /// Cliente Peer-to-Peer para a batalha naval
    /// </summary>
    public sealed partial class ClienteP2P
    {
        /// <summary>
        /// Delegado para a função de controle de eventos de tiro recebido
        /// </summary>
        /// <param name="t">Objeto representando o tiro recebido</param>
        public delegate void EventoDeTiroRecebido(Tiro t);

        /// <summary>
        /// Delegado para função de controle de evento de quando deve-se dar um tiro
        /// </summary>
        /// <returns>Um objeto do tipo tiro, com uma coordenada X e Y</returns>
        public delegate Tiro EventoDeDarTiro();

        /// <summary>
        /// Delegado para função de controle de evento de quando se recebe o resultado
        /// de um tiro dado
        /// </summary>
        /// <param name="t">Objeto representando o tiro recebido</param>
        /// <param name="resultado">Resultado do tiro</param>
        public delegate void EventoDeResultadoDeTiro(Tiro t, ResultadoDeTiro resultado);

        /// <summary>
        /// Evento chamado quando é seu turno de atirar
        /// </summary>
        public event EventoDeDarTiro OnDarTiro;

        /// <summary>
        /// Evento chamado quando recebe-se o resultado do último tiro dado
        /// </summary>
        public event EventoDeResultadoDeTiro OnResultadoDeTiro;

        /// <summary>
        /// Evento de tiro recebido
        /// </summary>
        public event EventoDeTiroRecebido OnTiroRecebido;
        
        /// <summary>
        /// Sinaliza se o cliente pode atirar ou não
        /// </summary>
        private bool PodeAtirar { get; set; }

        /// <summary>
        /// Mapa usado pelo cliente
        /// </summary>
        public Tabuleiro Tabuleiro { get; private set; }

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="nome">Nome do jogador, passado para os clientes remotos</param>
        /// <param name="tabuleiro">Tabuleiro com o qual se quer jogar</param>
        /// <exception cref="Exception">Se o tabuleiro estiver incompleto</exception>
        public ClienteP2P(string nome, Tabuleiro tabuleiro) : this(nome)
        {
            if (!tabuleiro.EstaCompleto())
                throw new Exception("Tabuleiro incompleto");

            PodeAtirar = true;
            Tabuleiro = tabuleiro;
            OnClienteConectado += OnClienteConectado;
            OnClienteDesconectado += Dados_OnClienteDesconectado;
            OnTiroRecebido += Dados_OnTiroRecebido;
        }

        /// <summary>
        /// Evento de tiro recebido
        /// </summary>
        private void Dados_OnTiroRecebido(Tiro t)
        {
            t.Aplicar(Tabuleiro);
            PodeAtirar = true;
        }

        /// <summary>
        /// Evento de desconexão
        /// </summary>
        private bool Dados_OnClienteDesconectado(IPAddress addr)
        {
            if (addr.Equals((cliente.Client.RemoteEndPoint as IPEndPoint).Address))
                Conectado = false;

            return true;
        }

        /// <summary>
        /// Evento de sucesso de conexão com cliente
        /// </summary>
        /// <param name="addr">Endereço do cliente</param>
        private bool Dados_OnClienteConectado(IPAddress addr)
        {
            Task.Run(() => Jogar());
            return true;
        }
        
        /// <summary>
        /// Executa o jogo se comunicando com o par remoto
        /// </summary>
        private void Jogar()
        {
            BinaryWriter writer = new BinaryWriter(cliente.GetStream());
            BinaryReader reader = new BinaryReader(cliente.GetStream());

            // Envio de mensagens
            Task.Run(() =>
            {
                try {
                    while (Conectado)
                    {
                        do { } while (!PodeAtirar);

                        Tiro t;
                        lock (writer)
                        {
                            t = OnDarTiro();
                                
                            writer.Write((char)1);
                            writer.Write(t.X);
                            writer.Write(t.Y);
                        }

                        PodeAtirar = false;

                        do { } while (reader.PeekChar() != 2);

                        lock (reader)
                        {
                            OnResultadoDeTiro(t, (ResultadoDeTiro)reader.ReadUInt32());
                        }
                    }
                } catch {
                    OnClienteDesconectado((cliente.Client.RemoteEndPoint as IPEndPoint).Address);
                }
            });

            // Leitura de mensagens
            try
            {
                while (Conectado)
                {
                    lock (reader)
                    lock (writer)
                    {
                        if (reader.PeekChar() == 1)
                        {
                            int x = reader.ReadInt32();
                            int y = reader.ReadInt32();
                            OnTiroRecebido(new Tiro(x, y));
                                
                            writer.Write((char)2);
                            writer.Write((uint)Tabuleiro.Atirar(x, y));
                        }
                    }
                }
            } catch {
                OnClienteDesconectado((cliente.Client.RemoteEndPoint as IPEndPoint).Address);
            }
        }
    }
}
