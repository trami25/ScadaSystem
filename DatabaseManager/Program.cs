using ServiceReference1;
using System;
using System.Threading.Tasks;

namespace DatabaseManager
{
    class Program
    {
        static async Task Main(string[] args)
        {
            SimpleServiceClient proxy = new SimpleServiceClient();
            AuthenticationClient authProxy = new AuthenticationClient();

           
            var messageResponse = await proxy.GetSomeMessageAsync("fakeToken");
            Console.WriteLine(messageResponse);

            
            bool success = await authProxy.RegistrationAsync("alanford", "alanford123");
            if (success)
            {
                Console.WriteLine("Registration successfully completed");
            }
            else
            {
                Console.WriteLine("There are already user with username");
            }

           
            string token = await authProxy.LoginAsync("alanford", "alanford123");
            Console.WriteLine($"Token for user alanford: {token}");

           
            var authenticatedMessageResponse = await proxy.GetSomeMessageAsync(token);
            Console.WriteLine(authenticatedMessageResponse);

            Console.ReadLine();
        }
    }
}
