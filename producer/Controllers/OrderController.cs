using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using core.Entities;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace producer.Controllers
 {
    [ApiController]
    [Route("/Order")]
    public class OrderController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post()
        {

            try
            {
                var factory = new ConnectionFactory() { HostName = "localhost", UserName = "guest", Password = "guest" };
                using var connection = factory.CreateConnection();
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: "orderQueue",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

                    string message = JsonSerializer
                        .Serialize(new Order(1, new User(1, "Marcos", "marcos@test.com")));
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: "orderQueue",
                        basicProperties: null,
                        body: body);
                }

                return Ok("Order created!");

            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest(e.Message);
            }

        }
    }
    
}

