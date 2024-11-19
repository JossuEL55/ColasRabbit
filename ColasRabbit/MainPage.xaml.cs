using RabbitMQ.Client;
using System.Text;
using ColasRabbit;

namespace ColasRabbit
{
    public partial class MainPage : ContentPage
    {
        private readonly Consumidor _consumidor;

        public MainPage()
        {
            InitializeComponent();

            // Inicia el consumidor
            _consumidor = new Consumidor();
            _consumidor.OnMensajeRecibido += MostrarMensaje; // Manejar mensajes recibidos
            _consumidor.IniciarConsumo(); // Iniciar el recibimiento de mensajes de la cola
        }

        // Método para manejar mensajes recibidos
        private void MostrarMensaje(string mensaje)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Estado.Text = $"Correo recibido: {mensaje}";
                Estado.TextColor = Colors.Blue;
            });
        }

        // Método para enviar mensajes
        private void EnviarCorreo_Clicked(object sender, EventArgs e)
        {
            var mensaje = EntradaCorreo.Text;
            if (string.IsNullOrWhiteSpace(mensaje))
            {
                Estado.Text = "Estado: Por favor escribe un mensaje.";
                Estado.TextColor = Colors.Red;
                return;
            }

            try
            {
                // Configuración de RabbitMQ
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "correos",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var body = Encoding.UTF8.GetBytes(mensaje);

                    channel.BasicPublish(exchange: "",
                                         routingKey: "correos",
                                         basicProperties: null,
                                         body: body);
                }

                // Actualizar estado si el mensaje fue enviado correctamente
                Estado.Text = "Estado: Mensaje enviado.";
                Estado.TextColor = Colors.Green;
            }
            catch (Exception ex)
            {
                // Mostrar error en la interfaz si es que algo falla
                Estado.Text = $"Estado: Error al enviar mensaje. {ex.Message}";
                Estado.TextColor = Colors.Red;
            }
        }
    }
}