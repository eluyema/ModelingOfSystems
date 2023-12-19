using ModelingSystem3.Core.ModelObjects;

namespace ModelingSystem3.Bank.RulesImplementations.ModelObjects
{
    public class BankClient : IModelObject
    {
        private double lastT;

        public BankClient() {
            lastT = 0;
        }

        public void StartProcessing(string processName, double tcurr)
        {
            lastT = tcurr;
        }
        public void FinishProcessing(string processName, double tcurr)
        {
            lastT = tcurr;
        }

        public double GetLastT() {
            return lastT;
        }
    }
}
