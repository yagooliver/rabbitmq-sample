using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace RabbitMqSample.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MensagemController : ControllerBase
    {
        private IConnection connection;
        private IModel channel;

        [HttpGet]
        public dynamic Get(string mensagem)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
            };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.QueueDeclare(queue: "TesteRabbitMQ",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

            string message =
                $"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")} - " +
                $"Conteúdo da Mensagem: {mensagem}";
            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                                 routingKey: "TesteRabbitMQ",
                                 basicProperties: null,
                                 body: body);

            return new
            {
                Retorno = "mensagem enviada com sucesso!"
            };
        }
    }
}