using ModelingSystem3.Core.Elements;
using ModelingSystem3.Core.Elements.Listeners;
using ModelingSystem3.Core.ModelObjects;

namespace ModelingSystem3.Bank.RulesImplementations.Elements.Listeners
{
    public class SwappingClientsDequeueListener : IDequeueListener
    {
        private List<Process> neighboursProcesses;
        private int swapAmount;
        public SwappingClientsDequeueListener()
        {
            swapAmount = 0;
            neighboursProcesses = new List<Process>();
        }

        public void AddNeighbour(Process p)
        {
            neighboursProcesses.Add(p);
        }

        public void OnDequeue(Process p) {
            int myQueueSize = p.GetQueueSize();
            foreach(Process neighbour in neighboursProcesses) {
                int queueSize = neighbour.GetQueueSize();
                if (queueSize - myQueueSize >= 2) {
                    IModelObject? modelObject = neighbour.Dequeue();
                    if (modelObject is not null)
                    {
                        swapAmount++;
                        System.Console.WriteLine("\nSwap triggered!");
                        p.InAct(modelObject);
                        break;
                    }
                }
            }
        }

        public int GetSwapAmount() {
            return swapAmount;
        }

    }
}
