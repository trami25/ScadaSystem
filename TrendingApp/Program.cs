using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrendingApp.TrendingServiceReference;

namespace TrendingApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TrendingServiceClient client = new TrendingServiceClient();
            var tags = client.GetTags();

            foreach (var tag in tags)
            {
                Console.WriteLine($"{tag.Id}: {tag.Value}");
            }
        }
    }
}
