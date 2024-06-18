using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;

namespace AlarmDisplay
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await StartNamedPipeClient();
        }

        private static async Task StartNamedPipeClient()
        {
            using (var pipeClient = new NamedPipeClientStream(".", "AlarmPipe", PipeDirection.In))
            {
                try
                {
                    Console.WriteLine("Connecting to server...");
                    pipeClient.Connect();
                    Console.WriteLine("Connected to server.");

                    using (var sr = new StreamReader(pipeClient))
                    {
                        string message;
                        while ((message = await sr.ReadLineAsync()) != null)
                        {
                            Console.WriteLine($"Received notification: {message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to connect or read from server. Exception: {ex.Message}");
                }
            }
        }
    }
}
