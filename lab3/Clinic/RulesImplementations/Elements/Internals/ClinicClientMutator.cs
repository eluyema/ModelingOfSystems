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
    public class ClinicClientMutator : IOnActModelObjectMutator
    {
        private List<(ClinicClientType, ClinicClientType)> clientTypeTranslator;

        public ClinicClientMutator(List<(ClinicClientType, ClinicClientType)> clientTypeTranslator) {
            this.clientTypeTranslator = clientTypeTranslator;
        }

        public void MutateModelObject(IModelObject modelObject) {
            if (modelObject is not ClinicClient) {
                return;
            }
            ClinicClient client = (ClinicClient)modelObject;

            foreach (var (typeSource, typeOrigin) in clientTypeTranslator) {
                if (client.GetClientType() == typeSource) {
                    client.SetClientType(typeOrigin);
                    return;
                }
            }
        }
    }
}
