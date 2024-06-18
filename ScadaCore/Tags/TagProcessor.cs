using ScadaCore.Tags.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace ScadaCore.Tags
{
    public class TagProcessor
    {
        private readonly Dictionary<string, Task> _tagIdToThread = new Dictionary<string, Task>();
        private readonly TagContext _context;

        public void AddTagTask(InputTag tag)
        {
            Task task = new Task(() => tag.ReadValue());
            _tagIdToThread[tag.Id] = task;
            task.Start();
        }

        public void DeleteTagTask(InputTag tag)
        {
            Task task;
            if (_tagIdToThread.TryGetValue(tag.Id, out task))
            {
                task.Dispose();
            }

        }
    }
}