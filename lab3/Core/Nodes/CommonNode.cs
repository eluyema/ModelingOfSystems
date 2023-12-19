using ModelingSystem3.Core.Elements;
using ModelingSystem3.Core.ModelObjects;

namespace ModelingSystem3.Core.Nodes
{
    public class CommonNode : INode
    {
        private static readonly Random rnd = new Random();
        private List<AbstractElement> elements;
        
        public CommonNode(List<AbstractElement> elements) {
            this.elements = elements;
        }

        public void TriggerAct(IModelObject modelObject) {
            int count = elements.Count;

            if (count == 0)
            {
                return;
            }
            else if (count == 1) {
                elements[0].InAct(modelObject);
                return;
            }

            int index = rnd.Next(count);
            
            elements[index].InAct(modelObject);
        }
    }
}
