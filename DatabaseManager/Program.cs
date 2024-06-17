using ServiceReference1;
using ServiceReference3;
using ServiceReference4;
using System;
using System.Threading.Tasks;
using SimulationDriver;
using ScadaCore.Tags.Model;

namespace DatabaseManager
{
    class Program
    {
        static async Task Main(string[] args)
        {
            SimpleServiceClient proxy = new SimpleServiceClient();
            AuthenticationClient authProxy = new AuthenticationClient();
            AlarmServiceClient alarmProxy = new AlarmServiceClient();
            TagServiceClient tagProxy = new TagServiceClient();

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

                        ScadaCore.Tags.Model.Tag tag = null;
                        if (tagType == "AnalogInput")
                        {
                            Console.Write("Enter scan time: ");
                            var scanTime = int.Parse(Console.ReadLine());
                            Console.Write("Enter low limit: ");
                            var lowLimit = double.Parse(Console.ReadLine());
                            Console.Write("Enter high limit: ");
                            var highLimit = double.Parse(Console.ReadLine());
                            Console.Write("Enter unit (Kg/Ms/C/F): ");
                            var unit = (ScadaCore.Tags.Model.Unit)Enum.Parse(typeof(ScadaCore.Tags.Model.Unit), Console.ReadLine(), true);

                            tag = new ScadaCore.Tags.Model.AnalogInputTag(tagId, description, ioAddress, value, scanTime, true, lowLimit, highLimit, unit, new MainSimulationDriver()); // Assuming a simulation driver is being used here
                        }
                        else if (tagType == "AnalogOutput")
                        {
                            Console.Write("Enter low limit: ");
                            var lowLimit = double.Parse(Console.ReadLine());
                            Console.Write("Enter high limit: ");
                            var highLimit = double.Parse(Console.ReadLine());
                            Console.Write("Enter unit (Kg/Ms/C/F): ");
                            var unit = (ScadaCore.Tags.Model.Unit)Enum.Parse(typeof(ScadaCore.Tags.Model.Unit), Console.ReadLine(), true);

                            tag = new AnalogOutputTag(tagId, description, ioAddress, value, value, lowLimit, highLimit, unit);
                        }
                        else if (tagType == "DigitalInput")
                        {
                            Console.Write("Enter scan time: ");
                            var scanTime = int.Parse(Console.ReadLine());

                            tag = new DigitalInputTag(tagId, description, ioAddress, value, scanTime, true, new MainSimulationDriver());
                        }
                        else if (tagType == "DigitalOutput")
                        {
                            tag = new DigitalOutputTag(tagId, description, ioAddress, value, value);
                        }

                        if (tag != null)
                        {
                            await tagProxy.AddTagAsync(tag);
                            Console.WriteLine("Tag added successfully.");
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

                        await tagProxy.RemoveTagAsync(tagId);
                        Console.WriteLine("Tag removed successfully.");
                    }
                    else if (choice == "4")
                    {
                        Console.Write("Enter tag ID to enable scan: ");
                        var tagId = Console.ReadLine();

                        await tagProxy.EnableScanAsync(tagId);
                        Console.WriteLine("Scan enabled.");
                    }
                    else if (choice == "5")
                    {
                        Console.Write("Enter tag ID to disable scan: ");
                        var tagId = Console.ReadLine();

                        await tagProxy.DisableScanAsync(tagId);
                        Console.WriteLine("Scan disabled.");
                    }
                    else if (choice == "6")
                    {
                        Console.Write("Enter tag ID to set value: ");
                        var tagId = Console.ReadLine();
                        Console.Write("Enter value: ");
                        var value = double.Parse(Console.ReadLine());

                        await tagProxy.SetOutputValueAsync(tagId, value);
                        Console.WriteLine("Value set successfully.");
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
                        var analogInputs = await tagProxy.GetAnalogInputTagsAsync();
                        Console.WriteLine("Available Analog Input Tags:");
                        foreach (var input in analogInputs)
                        {
                            Console.WriteLine($"ID: {input.Id}, Description: {input.Description}");
                        }

                        Console.Write("Enter tag name to add alarm: ");
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
                    else if (choice == "9")
                    {
                        var analogInputs = await tagProxy.GetAnalogInputTagsAsync();
                        Console.WriteLine("Available Analog Input Tags:");
                        foreach (var input in analogInputs)
                        {
                            Console.WriteLine($"ID: {input.Id}, Description: {input.Description}");
                        }

                        Console.Write("Enter tag name to remove alarm: ");
                        var tagName = Console.ReadLine();

                        await alarmProxy.RemoveAlarmAsync(tagName);
                        Console.WriteLine("Alarm removed successfully.");
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

                Console.WriteLine();
            }
        }
    }
}

