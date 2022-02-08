using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Services.Config;

namespace Services.Services
{
    public class AzQueueService
    {
        private readonly QueueClient _queueClient;

        public AzQueueService(string queueName)
        {
            _queueClient = new QueueClient(AppSettings.AzureStorageConnectionString, queueName);
            _queueClient.CreateIfNotExists();
        }

        public async Task SendMessageAsync(string message) => await _queueClient.SendMessageAsync(message);

        public async Task<QueueMessage> RetrieveNextMessageAsync()
        {
            QueueProperties properties = await _queueClient.GetPropertiesAsync();

            if (properties.ApproximateMessagesCount > 0)
            {
                QueueMessage[] queueMessages = await _queueClient.ReceiveMessagesAsync(1, TimeSpan.FromMinutes(1));
                
                if (queueMessages.Any())
                    return queueMessages[0];
            }

            return null;
        }

        public async Task DeleteMessage(string messageId, string popReceipt) => await _queueClient.DeleteMessageAsync(messageId, popReceipt);
    }
}