using ModelingSystem3.Clinic.RulesImplementations.ModelObjects;
using ModelingSystem3.Core.ModelObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingSystem3.Clinic.RulesImplementations.Elements.Internals
{
    public class RegistrationQueue
    {
        private PriorityQueue<ClinicClient, int> queue;
        private Dictionary<ClinicClientType, int> priorityDict;

        public RegistrationQueue(List<(ClinicClientType, int)> priorityTypes)
        {

            queue = new PriorityQueue<ClinicClient, int>();
            Dictionary<ClinicClientType, int> priorityDict = new Dictionary<ClinicClientType, int>();

            foreach (var (clientType, priority) in priorityTypes)
            {
                priorityDict.Add(clientType, priority);
            }
            this.priorityDict = priorityDict;
        }

        private int GetPriorityOfClientType(ClinicClientType clientType)
        {
            int value = int.MaxValue;
            priorityDict.TryGetValue(clientType, out value);
            return value;
        }

        public void Enqueue(ClinicClient client)
        {
            int priority = GetPriorityOfClientType(client.GetClientType());
            queue.Enqueue(client, priority);
        }

        public int Count { get { return queue.Count; } }

        public IModelObject Dequeue()
        {
            return queue.Dequeue();
        }
    }
}
