using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
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
        /// Porta onde o cliente procura conexões
        /// </summary>
        const int PortaTcp = 1337;

        /// <summary>
        /// Porta onde o servidor de broadcast procura conexões
        /// </summary>
        const int PortaBroadcast = 1729;

        /// <summary>
        /// Intervalo do timer sinalziador
        /// </summary>
        const double IntervaloSinalizador = 1000;

        /// <summary>
        /// Intervalo do timer confirmador
        /// </summary>
        const double IntervaloConfirmador = 500;

        /// <summary>
        /// Nome do cliente, usado para se identificar para os clientes remotos
        /// </summary>
        public string Nome { get; private set; }

        /// <summary>
        /// Nome do cliente remoto conectado a este
        /// </summary>
        public string NomeRemoto { get; private set; }

        //Lista com os clientes conectados na rede
        List<IPAddress> clientes;

        // Servidor UDP para broadcasting, é isso que lista os IPs disponíveis para conexão
        // e responde requisições feitas por outros hosts para listar os computadores na rede
        UdpClient servidorBroadcast;

        // Servidor e clientes TCP para comunicação com os pontos remotos
        TcpListener servidor;
        TcpClient cliente;

        // Timer usado para sinalizar para os computadores da rede que você existe
        Timer sinalizador, confirmador;

        // Tasks
        Task taskBroadcasting, taskConexao;

        /// <summary>
        /// Delegado de evento que recebe um endereço IP por parâmetro
        /// </summary>
        /// <param name="addr">Endereço IP passado para o evento</param>
        /// <returns>True ou False conforme necessário.</returns>
        public delegate void EventoComEnderecoIP(IPAddress addr);

        /// <summary>
        /// Delegado de evento que recebe um endereço IP por parâmetro
        /// </summary>
        /// <param name="addr">Endereço IP passado para o evento</param>
        /// <param name="nome">Nome do cliente remoto</param>
        /// <returns>True ou False conforme necessário.</returns>
        public delegate bool EventoDeRequisicaoDeConexao(IPAddress addr);

        /// <summary>
        /// Evento de cliente disponível detectado na rede. O retorno não é usado.
        /// </summary>
        public event EventoComEnderecoIP OnClienteDisponivel;

        /// <summary>
        /// Indica quando um cliente desconecta da rede. O retorno não é usado.
        /// </summary>
        public event EventoComEnderecoIP OnClienteIndisponivel;

        /// <summary>
        /// Evento de requisição de conexão com um cliente. 
        /// O retorno indica se a conexão deve ser aceita.
        /// </summary>
        public event EventoDeRequisicaoDeConexao OnClienteRequisitandoConexao;

        /// <summary>
        /// Evento de conexão bem sucedida com um cliente.
        /// O retorno não é usado.
        /// </summary>
        public event EventoComEnderecoIP OnClienteConectado;

        /// <summary>
        /// Evento de falha na conexão com um cliente.
        /// O retorno não é usado.
        /// </summary>
        public event EventoComEnderecoIP OnClienteDesconectado;

        /// <summary>
        /// Determina se o cliente está conectado a um cliente remoto
        /// </summary>
        public bool Conectado { get; private set; }

        /// <summary>
        /// Construtor
        /// </summary>
        private ClienteP2P(string nome)
        {
            Nome = nome;

            servidorBroadcast = new UdpClient(new IPEndPoint(IPAddress.Any, PortaBroadcast));
            servidorBroadcast.EnableBroadcast = true;
            servidorBroadcast.MulticastLoopback = false;

            servidor = new TcpListener(IPAddress.Any, PortaTcp);

            Conectado = false;
            sinalizador = new Timer(IntervaloSinalizador);
            sinalizador.Elapsed += (object sender, ElapsedEventArgs e) => SinalizarNaRede();

            confirmador = new Timer(IntervaloConfirmador);
            confirmador.Elapsed += (object sender, ElapsedEventArgs e) => ConfirmarClientes();
        }

        /// <summary>
        /// Inicializa o cliente
        /// </summary>
        public void Iniciar()
        {
            clientes = new List<IPAddress>();

            servidor.Start();
            taskBroadcasting = Task.Run(() => TratarBroadcast());
            taskConexao = Task.Run(() => ResponderClientes());

            sinalizador.Start();
	    confirmador.Start();
        }

        /// <summary>
        /// Solicita uma conexão com um cliente no IP remoto dado.
        /// Esse método trava a execução do programa enquanto espera pela resposta do cliente remoto
        /// </summary>
        /// <param name="ipRemoto">IP do cliente remoto</param>
        /// <returns>True caso a conexão seja bem sucedida e Falso caso contrário.</returns>
        public bool SolicitarConexao(IPAddress ipRemoto)
        {
            try
            {
                cliente = new TcpClient();
                cliente.Connect(ipRemoto, PortaTcp);

                StreamWriter writer = new StreamWriter(cliente.GetStream());
                StreamReader reader = new StreamReader(cliente.GetStream());
                writer.AutoFlush = true;

                // Envia o nome para o cliente remoto
                writer.WriteLine(Nome);

                // Lê a confirmação
                if (reader.ReadLine() == "OK")
                {
                    // Envia uma confirmação
                    writer.WriteLine("OK");

                    NomeRemoto = reader.ReadLine();

                    OnClienteConectado(ipRemoto);
                    return true;
                }

                // Se não, deu ruim. Fecha o cliente.
                cliente.Close();

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Responde tentativas de conexão de clientes remotos
        /// </summary>
        private void ResponderClientes()
        {
            while (!Conectado)
            {
                try
                {
                    cliente = servidor.AcceptTcpClient();

                    StreamReader reader = new StreamReader(cliente.GetStream());
                    StreamWriter writer = new StreamWriter(cliente.GetStream());
                    writer.AutoFlush = true;

                    NomeRemoto = reader.ReadLine();

                    IPAddress addr = (cliente.Client.RemoteEndPoint as IPEndPoint).Address;

                    if (OnClienteRequisitandoConexao(addr))
                    {
                        try
                        {
                            // Envia uma confirmação
                            writer.WriteLine("OK");
                            writer.WriteLine(Nome);

                            // Espera a confirmação definitiva de conexão
                            if (reader.ReadLine() == "OK")
                            {
                                Conectado = true;
                                OnClienteConectado(addr);
                            }
                            else
                                throw new System.Exception("Falhou :(");
                        }
                        catch
                        {
                            OnClienteDesconectado(addr);
                            throw new System.Exception();
                        }
                    }
                    else
                    {
                        // Rejeita a conexão
                        writer.WriteLine("Reject");
                    }
                } catch {
                    if (cliente != null)
                        cliente.Close();
                }

                if (!Conectado)
                    NomeRemoto = null;
            }
        }

        /// <summary>
        /// Sinaliza para os outros clientes na rede que você existe
        /// </summary>
        private void SinalizarNaRede()
        {
            try
            {
                if (!Conectado)
                    // Envia um 0 para todos os clientes na rede sinalizando que você existe
                    servidorBroadcast.Send(new byte[] { 0 }, 1, new IPEndPoint(IPAddress.Broadcast, PortaBroadcast));
            }
            catch { }
        }

        /// <summary>
        /// Thread de tratamento do broadcast
        /// </summary>
        private void TratarBroadcast()
        {
            try
            {
                while (!Conectado)
                {
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 0);
                    byte[] data = servidorBroadcast.Receive(ref endPoint);

                    if (new List<IPAddress>(Dns.GetHostAddresses(Dns.GetHostName())).Contains(endPoint.Address))
                        continue;

                    if (!Conectado)
                    {
                        // Se recebeu dados, detectou um cliente na rede
                        IPAddress ip = endPoint.Address.MapToIPv4();
                        OnClienteDisponivel(ip);

                        if (!clientes.Contains(ip))
                            clientes.Add(ip);
                    }
                        
                }
            }
            catch (SocketException) {}
        }

        private void ConfirmarClientes()
        {
            for (int n = 0; n < clientes.Count; n++)
                try
                {
                    cliente = new TcpClient();
                    cliente.Connect(clientes[n], PortaTcp);
                }
                catch
                {
                    //As vezes o n fica maior que o limite de clientes.
                    //Sim,
                    //Mesmo com a condição do for.
                    if (n < clientes.Count)
                    {
                        OnClienteIndisponivel(clientes[n]);
                        clientes.RemoveAt(n);
                    }
                }
        }

        /// <summary>
        /// Fecha o cliente
        /// </summary>
        public void Close()
        {
            servidorBroadcast.Close();
            servidor.Stop();
        }
    }
}
