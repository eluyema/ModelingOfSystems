using ModelingSystem3.Core.ModelObjects;

namespace ModelingSystem3.Clinic.RulesImplementations.ModelObjects
{
    public enum ClinicClientType {
        type_1,
        type_2,
        type_3
    }
    public class ClinicClient : IModelObject
    {
        private double finishTime;
        private double birthTime;
        private ClinicClientType clientType;


        public ClinicClient(ClinicClientType clientType, double birthTime)
        {
            this.birthTime = birthTime;
            this.clientType = clientType;
        }

        public void StartProcessing(string processName, double tcurr)
        {

        }
        public void FinishProcessing(string processName, double tcurr)
        {
            this.finishTime = tcurr;
        }

        public ClinicClientType GetClientType() {
            return clientType;
        }

        public void SetClientType(ClinicClientType clientType)
        {
            this.clientType = clientType;
        }

        public double GetBirthTime()
        {
            return this.birthTime;
        }

        public void SetBirthTime(double birthTime)
        {
            this.birthTime = birthTime;
        }

        public double GetFinishTime()
        {
            return this.finishTime;
        }

        public void SetFinishTime(double finishTime)
        {
            this.finishTime = finishTime;
        }
    }
}
