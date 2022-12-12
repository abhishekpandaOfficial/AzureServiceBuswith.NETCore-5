using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using SampleASBShared.Models;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SampleASBSender.Services
{
    public class AzureServiceBus : IAzureServiceBus
    {
        private readonly IConfiguration _Config;

        public AzureServiceBus(IConfiguration configuration)
        {
            _Config = configuration;
        }
        public async Task SendMessageAsync(Person personMessage, string queueName)
        {
            // step 1 : Get the Connection String from the appsettings.json

            var connectionString = _Config.GetConnectionString("AzureServiceBusConnectionString");

            // Step 2: Initializing the Queue

            var qClint = new QueueClient(connectionString, queueName);

            // step 3: convert Person Object into the JSON (Message)

            var msgBody = JsonSerializer.Serialize(personMessage);

            // Step : 4 - Initialize the Queue Message or Push the MsgBody to the QUeue 

            var msg = new Message(Encoding.UTF8.GetBytes(msgBody));

            // step 5: Sent the Message 

            await qClint.SendAsync(msg);


            
        }
    }
}
