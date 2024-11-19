using RabbitMQ.Client;
using System.Text;

namespace ColasRabbit;

public partial class EnviarCorreoPage : ContentPage
{
    public EnviarCorreoPage()
    {
        InitializeComponent();
    }

    private void EnviarCorreo_Clicked(object sender, EventArgs e)
    {
        var mensaje = EntradaCorreo.Text;
        if (string.IsNullOrWhiteSpace(mensaje))
        {
            Estado.Text = "Estado: Por favor escribe un mensaje.";
            Estado.TextColor = Colors.Red;
            return;
        }

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

        Estado.Text = "Estado: Mensaje enviado.";
        Estado.TextColor = Colors.Green;
    }
}