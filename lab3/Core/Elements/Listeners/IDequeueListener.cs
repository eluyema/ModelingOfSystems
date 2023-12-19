using ModelingSystem3.Core.ModelObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingSystem3.Core.Elements.Listeners
{
    public interface IDequeueListener
    {
        public void OnDequeue(Process p);
    }
}
