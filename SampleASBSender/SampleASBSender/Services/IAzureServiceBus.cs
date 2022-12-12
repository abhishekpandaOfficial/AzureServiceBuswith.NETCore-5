using SampleASBShared.Models;
using System.Threading.Tasks;

namespace SampleASBSender.Services
{
    public interface IAzureServiceBus
    {
        Task SendMessageAsync(Person personMessage, string queueName);
    }
}
