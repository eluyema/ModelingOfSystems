using ModelingSystem3.Core.ModelObjects;

namespace ModelingSystem3.Bank.RulesImplementations.ModelObjects
{
    public class BankClientFabric : IModelObjectFabric
    {
        public IModelObject generateNextModelObject(double tcurr)
        {
            return new BankClient();
        }
    }
}
