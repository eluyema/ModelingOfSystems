namespace ModelingSystem3.Core.ModelObjects
{
    public interface IModelObjectFabric
    {
        public IModelObject generateNextModelObject(double tcurr);

    }
}
