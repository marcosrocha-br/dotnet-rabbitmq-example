using System;
using Microsoft.AspNetCore.Connections;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using core.Entities;

namespace contabilidade.Controllers
{
    [ApiController]
    [Route("contabilidade")]
    public class ContabilidadeController : ControllerBase
    {
        public ContabilidadeController() { }

        [HttpGet]
        public async Task<IActionResult> GetOrder()
        {
            try
            {
                var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "orderQueue",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var consumer = new EventingBasicConsumer(channel);

                    var lista = new List<String>();

                    consumer.Received += (sender, eventArgs) =>
                    {
                        var body = eventArgs.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        var pedido = JsonSerializer.Deserialize<Order>(message);

                        lista.Add(pedido?.ToString());
                        
                    };

                    channel.BasicConsume(
                        queue: "orderQueue",
                        autoAck: true,
                        consumer: consumer);

                    await Task.Delay(2000);

                    return Ok(lista);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }
    }
}
