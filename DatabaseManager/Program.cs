using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using ScadaCore.Tags.Model;
using ServiceReference1;
using ServiceReference2;


namespace DatabaseManager
{
    class Program
    {
        static async Task Main(string[] args)
        {
          
            var tagServiceBinding = new BasicHttpBinding();
            var tagServiceEndpoint = new EndpointAddress("http://localhost:64310/DatabaseManagerService/TagService.svc");

            var authServiceBinding = new BasicHttpBinding();
            var authServiceEndpoint = new EndpointAddress("http://localhost:64310/Authentication.svc");

            var proxy = new SimpleServiceClient(/*authServiceBinding, authServiceEndpoint*/);
            var authProxy = new AuthenticationClient(/*authServiceBinding, authServiceEndpoint*/);
            var tagProxy = new TagServiceClient(/*tagServiceBinding, tagServiceEndpoint*/);

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
                    Console.WriteLine("2. Add Tag");
                    Console.WriteLine("3. Remove Tag");
                    Console.WriteLine("4. Enable Scan");
                    Console.WriteLine("5. Disable Scan");
                    Console.WriteLine("6. Set Output Value");
                    Console.WriteLine("7. View Output Values");
                    Console.WriteLine("8. Add Alarm");
                    Console.WriteLine("9. Remove Alarm");
                    Console.WriteLine("10. Logout");
                    Console.WriteLine("11. Exit");
                }

                var choice = Console.ReadLine();

                try
                {
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
                        string resultMessage;
                        if (choice == "1")
                        {
                            var authenticatedMessageResponse = await proxy.GetSomeMessageAsync(token);
                            Console.WriteLine(authenticatedMessageResponse);
                        }
                        else if (choice == "2")
                        {
                            Console.Write("Enter tag type (AnalogInput/AnalogOutput/DigitalInput/DigitalOutput): ");
                            var tagType = Console.ReadLine();
                            Console.Write("Enter tag ID: ");
                            var tagId = Console.ReadLine();
                            Console.Write("Enter description: ");
                            var description = Console.ReadLine();
                            Console.Write("Enter IO address: ");
                            var ioAddress = Console.ReadLine();
                            Console.Write("Enter initial value: ");
                            var value = double.Parse(Console.ReadLine());

                            if (tagType == "AnalogInput")
                            {
                                Console.Write("Enter scan time: ");
                                var scanTime = int.Parse(Console.ReadLine());
                                Console.Write("Enter low limit: ");
                                var lowLimit = double.Parse(Console.ReadLine());
                                Console.Write("Enter high limit: ");
                                var highLimit = double.Parse(Console.ReadLine());
                                Console.Write("Enter unit (Kg/Ms/C/F): ");
                                var unit =  Console.ReadLine();

                                resultMessage = await tagProxy.AddAITagAsync(tagId, description, ioAddress, value, scanTime, true, lowLimit, highLimit, unit);
                                Console.WriteLine(resultMessage);
                            }
                            else if (tagType == "AnalogOutput")
                            {
                                Console.Write("Enter low limit: ");
                                var lowLimit = double.Parse(Console.ReadLine());
                                Console.Write("Enter high limit: ");
                                var highLimit = double.Parse(Console.ReadLine());
                                Console.Write("Enter unit (Kg/Ms/C/F): ");
                                var unit = Console.ReadLine();

                                resultMessage = await tagProxy.AddAOTagAsync(tagId, description, ioAddress, value, value, lowLimit, highLimit, unit);
                                Console.WriteLine(resultMessage);
                            }
                            else if (tagType == "DigitalInput")
                            {
                                Console.Write("Enter scan time: ");
                                var scanTime = int.Parse(Console.ReadLine());

                                resultMessage = await tagProxy.AddDITagAsync(tagId, description, ioAddress, value, scanTime, true);
                                Console.WriteLine(resultMessage);
                            }
                            else if (tagType == "DigitalOutput")
                            {
                                resultMessage = await tagProxy.AddDOTagAsync(tagId, description, ioAddress, value, value);
                                Console.WriteLine(resultMessage);
                            }
                            else
                            {
                                Console.WriteLine("Invalid tag type.");
                            }
                        }
                        else if (choice == "3")
                        {
                            Console.Write("Enter tag ID to remove: ");
                            var tagId = Console.ReadLine();

                            resultMessage = await tagProxy.RemoveTagAsync(tagId);
                            Console.WriteLine(resultMessage);
                        }
                        else if (choice == "4")
                        {
                            Console.Write("Enter tag ID to enable scan: ");
                            var tagId = Console.ReadLine();

                            resultMessage = await tagProxy.EnableScanAsync(tagId);
                            Console.WriteLine(resultMessage);
                        }
                        else if (choice == "5")
                        {
                            Console.Write("Enter tag ID to disable scan: ");
                            var tagId = Console.ReadLine();

                            resultMessage = await tagProxy.DisableScanAsync(tagId);
                            Console.WriteLine(resultMessage);
                        }
                        else if (choice == "6")
                        {
                            Console.Write("Enter tag ID to set value: ");
                            var tagId = Console.ReadLine();
                            Console.Write("Enter value: ");
                            var value = double.Parse(Console.ReadLine());

                            resultMessage = await tagProxy.SetOutputValueAsync(tagId, value);
                            Console.WriteLine(resultMessage);
                        }
                        else if (choice == "7")
                        {
                            var tags = await tagProxy.GetAllTagsAsync();
                            foreach (var tag in tags)
                            {
                                if (tag is AnalogOutputTag || tag is DigitalOutputTag)
                                {
                                    Console.WriteLine($"Tag ID: {tag.Id}, Value: {tag.Value}");
                                }
                            }
                        }
                        else if (choice == "8")
                        {
                            /*
                           var analogInputs = await tagProxy.GetAnalogInputTagsAsync();
                            Console.WriteLine("Available Analog Input Tags:");
                            foreach (var input in analogInputs)
                            {
                                Console.WriteLine($"ID: {input.Id}, Description: {input.Description}");
                            }
                           */
                            Console.Write("Enter tag name to add alarm: ");
                            var tagName = Console.ReadLine();
                            Console.Write("Enter alarm type (High/Low): ");
                            var type = Console.ReadLine();
                            Console.Write("Enter priority (1-3): ");
                            var priority = int.Parse(Console.ReadLine());
                            Console.Write("Enter threshold: ");
                            var threshold = double.Parse(Console.ReadLine());

                            resultMessage = await tagProxy.AddAlarmAsync(tagName, type, priority, threshold);
                            Console.WriteLine(resultMessage);
                        }
                        else if (choice == "9")
                        {
                            Console.Write("Enter tag name to remove alarm: ");
                            var tagName = Console.ReadLine();

                            resultMessage = await tagProxy.RemoveAlarmAsync(tagName);
                            Console.WriteLine(resultMessage);
                        }
                        else if (choice == "10")
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
                        else if (choice == "11")
                        {
                            Console.WriteLine("Exiting the program.");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid choice. Please choose a valid option.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }

                Console.WriteLine();
            }
        }
    }
}
