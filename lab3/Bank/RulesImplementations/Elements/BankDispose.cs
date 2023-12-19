using ModelingSystem3.Bank.RulesImplementations.ModelObjects;
using ModelingSystem3.Core.Elements;
using ModelingSystem3.Core.ModelObjects;

namespace ModelingSystem3.Bank.RulesImplementations.Elements
{
    public class BankDispose : AbstractDispose
    {
        private int quantity;
        private double lastT;
        private double averageInterval;
        public BankDispose(string disposeName) : base(disposeName)
        {
            quantity = 0;
            lastT = 0;
            averageInterval = 0;
        }

        public override void InAct(IModelObject modelObject) {
            if (modelObject is BankClient) {
                BankClient bC = (BankClient)modelObject;
                double t = bC.GetLastT();
                double interval = t - lastT;

                if (lastT == 0) {
                    lastT = t;
                    quantity++;
                    return;
                }

                averageInterval = ((averageInterval * quantity) + interval) / (quantity + 1);
                
                lastT = t;
                quantity++;
            }
        }

        public int GetQuantity() {
            return quantity;
        }

        public double GetAverageInterval() {
            return averageInterval;
        }
    }
}
