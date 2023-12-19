using ModelingSystem3.Clinic.RulesImplementations.ModelObjects;
using ModelingSystem3.Core.Elements;
using ModelingSystem3.Core.ModelObjects;
using ModelingSystem3.Core.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingSystem3.Clinic.RulesImplementations.Nodes
{
    public class ClinicBranchingNode : INode
    {
        private List<(ClinicClientType, AbstractElement)> branches;
        private AbstractElement defaultElement;

        public ClinicBranchingNode(List<(ClinicClientType, AbstractElement)> branches, AbstractElement defaultElement)
        {
            this.branches = branches;
            this.defaultElement = defaultElement;
        }

        public void TriggerAct(IModelObject modelObject)
        {
            if (modelObject is not ClinicClient) {
                defaultElement.InAct(modelObject);
                return;
            }
            ClinicClient client = (ClinicClient)modelObject;


            foreach (var (clientType, element) in branches) {
                if (clientType == client.GetClientType()) {
                    element.InAct(client);
                    return;
                }
            }
            defaultElement.InAct(modelObject);
        }
    }
}
