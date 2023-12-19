using ModelingSystem3.Clinic.RulesImplementations.ModelObjects;
using ModelingSystem3.Core.Elements.Internals;
using ModelingSystem3.Core.ModelObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingSystem3.Clinic.RulesImplementations.Elements.Internals
{
    public class RegistrationTimeEstimator : ITimeEstimator
    {
        private Dictionary<ClinicClientType, double> processingTimes;
        private double baseProcessing;

        public RegistrationTimeEstimator(double baseProcessing) {
            this.baseProcessing = baseProcessing;
            processingTimes = new Dictionary<ClinicClientType, double>();
        }

        public void SetProcessingTime(ClinicClientType clientType, double time) {
            processingTimes.Add(clientType, time);
        }

        public double GetEstimetedTime(IModelObject modelObject) {
            if (modelObject is not ClinicClient) {
                return baseProcessing;
            }
            ClinicClient client = (ClinicClient)modelObject;
            double time = baseProcessing;

            processingTimes.TryGetValue(client.GetClientType(), out time);

            return time;
        }
    }
}
