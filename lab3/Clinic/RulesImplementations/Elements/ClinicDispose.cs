using ModelingSystem3.Bank.RulesImplementations.ModelObjects;
using ModelingSystem3.Clinic.RulesImplementations.ModelObjects;
using ModelingSystem3.Core.Elements;
using ModelingSystem3.Core.ModelObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingSystem3.Clinic.RulesImplementations.Elements
{
    public class ClinicDispose : AbstractDispose
    {
        private int quantity;
        private double averageTimeInClinic;
        private double timeInClinic;

        public ClinicDispose(string disposeName) : base(disposeName)
        {
            quantity = 0;
            averageTimeInClinic = 0;
        }

        public override void InAct(IModelObject modelObject)
        {
            if (modelObject is ClinicClient)
            {
                ClinicClient client = (ClinicClient)modelObject;

                double timeInClinic = client.GetFinishTime() - client.GetBirthTime();

                averageTimeInClinic = ((averageTimeInClinic * quantity) + timeInClinic) / (quantity + 1);

                quantity++;
            }
        }

        public int GetQuantity()
        {
            return quantity;
        }

        public double GetAverageTimeInClinic() {
            return averageTimeInClinic;
        }
    }
}
