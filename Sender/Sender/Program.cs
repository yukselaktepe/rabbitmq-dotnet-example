using RabbitMQ.Client;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {

                Console.WriteLine("-----------------------------------------------------------------------");
                Console.WriteLine("> value<enter>");
                Console.WriteLine("Ctrl-C to quit.\n");

                var cancelled = false;
                Console.CancelKeyPress += (_, e) => {
                    e.Cancel = true;
                    cancelled = true;
                };

                while (!cancelled)
                {
                    Console.Write("> ");

                    string text;
                    try
                    {
                        text = Console.ReadLine();
                    }
                    catch (IOException)
                    {
                        break;
                    }
                    if (text == null)
                    {
                        break;
                    }

                    try
                    {
                        channel.QueueDeclare(queue: "Example",
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

                        var body = Encoding.UTF8.GetBytes(text);

                        channel.BasicPublish(exchange: "",
                                             routingKey: "Example",
                                             basicProperties: null,
                                             mandatory: false,
                                             body: body);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"failed to deliver message: {e.Message}");
                    }
                }

               


            }

            Console.WriteLine(" İlgili kişi gönderildi...");
        }
    }
}
