using ModelingSystem3.Core.ModelObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingSystem3.Core.Elements.Internals
{
    public interface IOnActModelObjectMutator
    {
        public void MutateModelObject(IModelObject modelObject);
    }
}
