using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AdviesTool
{
    class RabbitmqClient
    {
        public ConnectionFactory factory = new ConnectionFactory { Uri = new Uri("amqp://guest:guest@localhost:5672") };

        public RabbitmqClient()
        {
        }

        public List<IModel> CreateClient()
        {
            RabbitmqClient client = new RabbitmqClient();
            using var connection = client.factory.CreateConnection();
            using var advOutChannel = connection.CreateModel();
            using var advInChannel = connection.CreateModel();
            List<IModel> modelList = new List<IModel>();
            modelList.Add(advInChannel);
            modelList.Add(advOutChannel);
            return modelList;
        }

        public void sendToCore(List<IModel> advModels)
        {
            IModel input = advModels[0];
            IModel output = advModels[1];

            input.QueueDeclare("AdviesTool-input-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            output.QueueDeclare("AdviesTool-output-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(input);
            consumer.Received += (sender, e) =>
            {
                input.BasicPublish("", "AdviesTool-output-queue", null, e.Body);
            };
        }

    }
}
