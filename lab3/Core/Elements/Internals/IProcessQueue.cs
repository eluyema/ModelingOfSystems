using ModelingSystem3.Core.ModelObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingSystem3.Core.Elements.Internals
{
    public interface IProcessQueue
    {
        public void Enqueue(IModelObject modelObject);

        public int Count { get; }

        public IModelObject Dequeue();
    }
}
