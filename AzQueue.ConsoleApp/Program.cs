using Services.Services;
using System.Text;

Services.Config.AppSettings.AzureStorageConnectionString = "your-connection-string";

AzQueueService queue = new AzQueueService("sample-queue");

//send
//string base64message = Convert.ToBase64String(Encoding.UTF8.GetBytes("sample data"));
//queue.SendMessageAsync(base64message).Wait(); 

var message = queue.RetrieveNextMessageAsync().Result;

//get
//string text = Encoding.UTF8.GetString(Convert.FromBase64String(message.MessageText));
//Console.WriteLine("Message is " + text);

//delete
await queue.DeleteMessage(message.MessageId, message.PopReceipt);

Console.WriteLine("Message is deleted.");
