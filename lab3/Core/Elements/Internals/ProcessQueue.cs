using ModelingSystem3.Core.ModelObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingSystem3.Core.Elements.Internals
{
    public class ProcessQueue : IProcessQueue
    {
        private Queue<IModelObject> queue;

        public ProcessQueue() {
            this.queue = new Queue<IModelObject>();
        }

        public void Enqueue(IModelObject modelObject) {
            queue.Enqueue(modelObject);
        }

        public int Count { get { return queue.Count; } }

        public IModelObject Dequeue() {
            return queue.Dequeue();
        }
    }
}
