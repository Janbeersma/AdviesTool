using System;

namespace AdviesTool
{
    class Program
    {
        static void Main(string[] args)
        {
            RabbitmqClient client = new RabbitmqClient();
            var channelModels = client.CreateClient();
            client.sendToCore(channelModels);
        }
    }
}
