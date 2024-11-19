using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace ColasRabbit
{   
    public class Consumidor
    {
        public event Action<string> OnMensajeRecibido;

        public void IniciarConsumo()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "correos",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumidor = new EventingBasicConsumer(channel);

            consumidor.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var mensaje = Encoding.UTF8.GetString(body);

                // Inicia el evento al recibir un mensaje
                OnMensajeRecibido?.Invoke(mensaje);
            };

            channel.BasicConsume(queue: "correos",
                                 autoAck: true,
                                 consumer: consumidor);
        }
    }
}