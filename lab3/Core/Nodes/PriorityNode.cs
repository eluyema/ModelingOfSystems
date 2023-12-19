using ModelingSystem3.Core.ModelObjects;
using ModelingSystem3.Core.Elements;

namespace ModelingSystem3.Core.Nodes
{
    public class PriorityNode : INode
    {
        private List<(Process, int)> processQueue;

        public PriorityNode(List<(Process, int)> queue)
        {
            this.processQueue = queue;
        }


        public void TriggerAct(IModelObject bankClient)
        {
            var sortedByPriorityList = processQueue.OrderBy(item => item.Item2).ToList();

            if (sortedByPriorityList is null || sortedByPriorityList.Count == 0)
            {
                return;
            }

            foreach (var (process, priority) in sortedByPriorityList)
            {
                if (process.GetQueueSize() == 0)
                {
                    process.InAct(bankClient);
                    return;
                }
            }

            (Process, int) lowestQueueSizeProcess = (sortedByPriorityList[0].Item1, sortedByPriorityList[0].Item2);

            foreach (var (process, priority) in sortedByPriorityList)
            {
                int queueValue = process.GetQueueSize();

                if (queueValue < lowestQueueSizeProcess.Item2)
                {
                    lowestQueueSizeProcess.Item1 = process;
                    lowestQueueSizeProcess.Item2 = queueValue;
                }
            }

            lowestQueueSizeProcess.Item1.InAct(bankClient);
        }
    }
}
