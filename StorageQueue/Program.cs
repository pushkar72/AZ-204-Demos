using System;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

namespace StorageQueue {
    class Program {

        static void Main () {
            MainAsync ().GetAwaiter ().GetResult ();
        }
        public static async Task MainAsync () {
            //Create Queue
            string connectionString = "";
            // Create a unique name for the queue
            string queueName = "quickstartqueues";

            Console.WriteLine ($"Creating queue: {queueName}");

            // Instantiate a QueueClient which will be
            // used to create and manipulate the queue
            QueueClient queueClient = new QueueClient (connectionString, queueName);

            // Create the queue
            await queueClient.CreateAsync ();
        }
        public static async Task SendMessage (QueueClient queueClient) {
            Console.WriteLine ("\nAdding messages to the queue...");

            // Send several messages to the queue
            await queueClient.SendMessageAsync ("First message");
            await queueClient.SendMessageAsync ("Second message");

            // Save the receipt so we can update this message later
            SendReceipt receipt = await queueClient.SendMessageAsync ("Third message");
        }
        public static async Task PeekMessage (QueueClient queueClient) {
            Console.WriteLine ("\nPeek at the messages in the queue...");

            // Peek at messages in the queue
            PeekedMessage[] peekedMessages = await queueClient.PeekMessagesAsync (maxMessages: 10);

            foreach (PeekedMessage peekedMessage in peekedMessages) {
                // Display the message
                Console.WriteLine ($"Message: {peekedMessage.MessageText}");
            }
        }
        public static async Task ReceiveMessage (QueueClient queueClient) {
            Console.WriteLine ("\nReceiving messages from the queue...");

            // Get messages from the queue
            QueueMessage[] messages = await queueClient.ReceiveMessagesAsync (maxMessages: 10);
        }
        public static async Task DeleteQueue(QueueClient queueClient) {
            Console.WriteLine ("\nPress Enter key to delete the queue...");
            Console.ReadLine ();

            // Clean up
            Console.WriteLine ($"Deleting queue: {queueClient.Name}");
            await queueClient.DeleteAsync ();

            Console.WriteLine ("Done");
        }
    }
}