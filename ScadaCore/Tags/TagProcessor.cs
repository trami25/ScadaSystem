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
        private readonly Dictionary<string, Task> _tagIdToTask = new Dictionary<string, Task>();
        private readonly TagContext _context;

        public void AddTagTask(InputTag tag)
        {
            Task task = new Task(() => tag.ReadValue());
            _tagIdToTask[tag.Id] = task;
            task.Start();
        }

        public void DeleteTagTask(string id)
        {
            Task task;
            if (_tagIdToTask.TryGetValue(id, out task))
            {
                task.Dispose();
                _tagIdToTask.Remove(id);
            }

        }
    }
}