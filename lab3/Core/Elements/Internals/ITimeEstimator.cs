using ModelingSystem3.Core.ModelObjects;

namespace ModelingSystem3.Core.Elements.Internals
{
    public interface ITimeEstimator
    {
        public double GetEstimetedTime(IModelObject modelObject);
    }
}
