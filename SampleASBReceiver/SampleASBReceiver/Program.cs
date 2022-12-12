using Microsoft.Azure.ServiceBus;
using SampleASBShared.Models;
using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SampleASBReceiver
{
    public  class Program
    {
        const string conString = "Endpoint=sb://samplewebservicebusazure.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=aMvVxiElDKFvEI+4HZFmnlpCI8b/Nt3y3L0tXjxWOuI=";
        static IQueueClient qClient;
        static async Task Main(string[] args)
        {
            qClient = new QueueClient(conString, "personqueue");

            var msgOption = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // How Many Messages we can Process at a time
                MaxConcurrentCalls = 1,

                // Need to wait until a message is fully processed
                AutoComplete = false

            };

            qClient.RegisterMessageHandler(ProcessMessageHandler, msgOption);

            Console.ReadLine();
            await qClient.CloseAsync();
            
        }

        private static async Task ProcessMessageHandler(Message msg, CancellationToken token)
        {
            // Deserialize the Message Body

            var jsonBody = Encoding.UTF8.GetString(msg.Body);
            var PersonObj = JsonSerializer.Deserialize<Person>(jsonBody);

            // We can write Email Service Here 

            Console.WriteLine($"First Name is {PersonObj.FirstName}");
            Console.WriteLine($"Last Name  is {PersonObj.LastName}");
            Console.WriteLine($"Email  is {PersonObj.Email}");


            // Updating the queue that the Messages has been Processed Successfully

            await qClient.CompleteAsync(msg.SystemProperties.LockToken);
        }

        private static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs arg)
        {
            Console.WriteLine($"Something Went Wrong,{arg.Exception}");
            return Task.CompletedTask;
        }
    }
}
