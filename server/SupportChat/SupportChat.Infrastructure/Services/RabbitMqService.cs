// using System.Text;
// using System.Text.Json;
// using RabbitMQ.Client;
// using SupportChat.Infrastructure.Interfaces;
//
// namespace SupportChat.Infrastructure.Services;
//
// public class RabbitMqService : IRabbitMqService
// {
//     private readonly string _hostName;
//     private readonly int _port;
//     private readonly string _username;
//     private readonly string _password;
//     private readonly string _queueName;
//
//     public RabbitMqService(
//         string hostName = "rabbit-mq", 
//         int port = 5672, 
//         string username = "guest",
//         string password = "guest",
//         string queueName = "MyQueue")
//     {
//         _hostName = hostName;
//         _port = port;
//         _username = username;
//         _password = password;
//         _queueName = queueName;
//     }
//     
//     public void SendMessage(object obj)
//     {
//         var message = JsonSerializer.Serialize(obj);
//         SendMessage(message);
//     }
//
//     public void SendMessage(string message)
//     {
//         var factory = new ConnectionFactory { 
//             HostName = _hostName, 
//             Port = _port, 
//             UserName = _username, 
//             Password = _password };
//         // var factory = new ConnectionFactory { Uri = new Uri("amqps://tamquzft:9L7tjgv3UcrkN8ZARK23ztSAnLtMy8sG@cow.rmq2.cloudamqp.com/tamquzft")};
//         using (var connection = factory.CreateConnection())
//         using (var channel = connection.CreateModel())
//         {
//             channel.ExchangeDeclare("MyExchange", "fanout", true);
//             
//             channel.QueueDeclare(queue: _queueName,
//                 durable: true,
//                 exclusive: false,
//                 autoDelete: false,
//                 arguments: null);
//
//             var body = Encoding.UTF8.GetBytes(message);
//
//             channel.BasicPublish(exchange: "MyExchange",
//                 routingKey: _queueName,
//                 basicProperties: null,
//                 body: body);
//         }
//     }
// }