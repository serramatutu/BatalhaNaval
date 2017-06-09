using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace BatalhaNaval
{
    /// <summary>
    /// Cliente Peer-to-Peer para a batalha naval
    /// </summary>
    public sealed partial class ClienteP2P
    {
        /// <summary>
        /// Timeout para dar um tiro
        /// </summary>
        const int TIMEOUT_TIRO = 30000;

        /// <summary>
        /// Delegado para a função de controle de eventos de tiro recebido
        /// </summary>
        /// <param name="t">Objeto representando o tiro recebido</param>
        public delegate void EventoDeTiroRecebido(Tiro t);

        /// <summary>
        /// Delegado para função de controle de evento de quando deve-se dar um tiro
        /// </summary>
        /// <returns>Um objeto do tipo tiro, com uma coordenada X e Y</returns>
        public delegate void EventoDeDarTiro();

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
        /// Mutex para esperar um tiro
        /// </summary>
        private Mutex mutexTiro;

        /// <summary>
        /// Tiro a ser dado
        /// </summary>
        private Tiro tiro;

        /// <summary>
        /// Aleatório
        /// </summary>
        private Random rnd;

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

            mutexTiro = new Mutex();
            rnd = new Random();
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
        private void Dados_OnClienteDesconectado(IPAddress addr)
        {
            if (addr.Equals((cliente.Client.RemoteEndPoint as IPEndPoint).Address))
                Conectado = false;
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
        /// Envia um tiro para o cliente
        /// </summary>
        /// <param name="x">Posição X do tiro</param>
        /// <param name="y">Posição Y do tiro</param>
        public void DarTiro(int x, int y)
        {
            tiro = new Tiro(x, y);
            mutexTiro.ReleaseMutex();
        }

        /// <summary>
        /// Executa o jogo se comunicando com o par remoto
        /// </summary>
        private void Jogar()
        {
            StreamWriter writer = new StreamWriter(cliente.GetStream());
            writer.AutoFlush = true;

            StreamReader reader = new StreamReader(cliente.GetStream());

            // Envio de mensagens
            Mutex mutex = new Mutex();

            Task.Run(() =>
            {
                try {
                    while (Conectado)
                    {
                        if (!PodeAtirar)
                            mutex.WaitOne();
                            
                        lock (writer)
                        {
                            OnDarTiro();
                            mutexTiro.WaitOne(TIMEOUT_TIRO);

                            if (tiro == null)
                                tiro = new Tiro(rnd.Next(Tabuleiro.NumeroDeColunas), rnd.Next(Tabuleiro.NumeroDeLinhas));

                            writer.WriteLine("Tiro " + tiro.X + "," + tiro.Y);
                        }

                        PodeAtirar = false;

                        lock (reader)
                        {
                            string r = reader.ReadLine();
                            OnResultadoDeTiro(tiro, (ResultadoDeTiro)Convert.ToUInt32(r));
                        }

                        tiro = null;
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
                        string line = reader.ReadLine();
                        if (line.StartsWith("Tiro "))
                        {
                            int x = Convert.ToInt32(line.Substring(5, line.IndexOf(',') - 5));
                            int y = Convert.ToInt32(line.Substring(line.IndexOf(',') + 1));
                            OnTiroRecebido(new Tiro(x, y));
                                
                            writer.WriteLine(((uint)Tabuleiro.Atirar(x, y)).ToString());
                            PodeAtirar = true;
                            mutex.ReleaseMutex();
                        }
                    }
                }
            } catch {
                OnClienteDesconectado((cliente.Client.RemoteEndPoint as IPEndPoint).Address);
            }
        }
    }
}
