using ServiceReference1;
using ServiceReference2;
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
            AlarmServiceClient alarmProxy = new AlarmServiceClient();

            string token = null;

            while (true)
            {
                if (token == null)
                {
                    Console.WriteLine("Choose an option:");
                    Console.WriteLine("1. Register");
                    Console.WriteLine("2. Login");
                    Console.WriteLine("3. Exit");
                }
                else
                {
                    Console.WriteLine("Choose an option:");
                    Console.WriteLine("1. Get Authenticated Message");
                    Console.WriteLine("2. Add Alarm");
                    Console.WriteLine("3. Remove Alarm");
                    Console.WriteLine("4. Logout");
                    Console.WriteLine("5. Exit");
                }

                var choice = Console.ReadLine();

                if (token == null)
                {
                    if (choice == "1")
                    {
                        Console.Write("Enter username for registration: ");
                        var username = Console.ReadLine();
                        Console.Write("Enter password for registration: ");
                        var password = Console.ReadLine();

                        bool success = await authProxy.RegistrationAsync(username, password);
                        if (success)
                        {
                            Console.WriteLine("Registration successfully completed");
                        }
                        else
                        {
                            Console.WriteLine("There is already a user with this username");
                        }
                    }
                    else if (choice == "2")
                    {
                        Console.Write("Enter username for login: ");
                        var username = Console.ReadLine();
                        Console.Write("Enter password for login: ");
                        var password = Console.ReadLine();

                        token = await authProxy.LoginAsync(username, password);
                        if (token != "Login failed" && token != "Internal error")
                        {
                            Console.WriteLine($"Token for user {username}: {token}");
                        }
                        else
                        {
                            Console.WriteLine(token);
                            token = null; // Reset token on failed login
                        }
                    }
                    else if (choice == "3")
                    {
                        Console.WriteLine("Exiting the program.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice. Please choose 1, 2, or 3.");
                    }
                }
                else
                {
                    if (choice == "1")
                    {
                        var authenticatedMessageResponse = await proxy.GetSomeMessageAsync(token);
                        Console.WriteLine(authenticatedMessageResponse);
                    }
                    else if (choice == "2")
                    {
                        Console.Write("Enter tag name: ");
                        var tagName = Console.ReadLine();
                        Console.Write("Enter alarm type (High/Low): ");
                        var type = Console.ReadLine();
                        Console.Write("Enter priority (1-3): ");
                        var priority = int.Parse(Console.ReadLine());
                        Console.Write("Enter threshold: ");
                        var threshold = double.Parse(Console.ReadLine());

                        await alarmProxy.AddAlarmAsync(tagName, type, priority, threshold);
                        Console.WriteLine("Alarm added successfully.");
                    }
                    else if (choice == "3")
                    {
                        Console.Write("Enter tag name: ");
                        var tagName = Console.ReadLine();

                        await alarmProxy.RemoveAlarmAsync(tagName);
                        Console.WriteLine("Alarm removed successfully.");
                    }
                    else if (choice == "4")
                    {
                        bool success = await authProxy.LogoutAsync(token);
                        if (success)
                        {
                            Console.WriteLine("Logout successful.");
                            token = null;
                        }
                        else
                        {
                            Console.WriteLine("Logout failed.");
                        }
                    }
                    else if (choice == "5")
                    {
                        Console.WriteLine("Exiting the program.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid choice. Please choose 1, 2, 3, 4, or 5.");
                    }
                }

                Console.WriteLine();
            }
        }
    }
}
