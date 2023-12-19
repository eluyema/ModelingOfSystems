using ModelingSystem3.Bank.RulesImplementations.Elements;
using ModelingSystem3.Bank.RulesImplementations.Elements.Listeners;
using ModelingSystem3.Core.Elements;
using ModelingSystem3.Core.Elements.Listeners;
using ModelingSystem3.Core.Models.Strategies;
using ModelingSystem3.Core.Nodes;
using System.Xml.Linq;

namespace ModelingSystem3.Bank.RulesImplementations.Model.Strategies
{
    public class BankStatisticsStrategy : IStatisticsStrategy
    {
        private readonly double simulationTime;
        private double averageClientsInBank;
        public BankStatisticsStrategy(double simulationTime)
        {
            this.simulationTime = simulationTime;
            averageClientsInBank = 0;
        }

        public void DoStatistics(List<AbstractElement> elements, List<INode> nodes, double delta)
        {
            int clientsInBank = 0;
            foreach (var element in elements)
            {
                if (element is Process) {
                    Process p = (Process)element;
                    clientsInBank += p.GetQueueSize();
                    clientsInBank += p.GetBusyWorkersAmount();
                }
            }
            averageClientsInBank += clientsInBank * delta;
        }

        public void PrintStepStats(List<AbstractElement> elements, List<INode> nodes, double tcurr)
        {
            foreach (AbstractElement e in elements)
            {
                Console.Write("\n");
                if (e is Process)
                {
                    Process p = (Process)e;
                    string name = p.GetName();

                    Console.WriteLine(name);
                    Console.WriteLine("   quantity = " + p.GetQuantity());
                    Console.WriteLine("   queue = " + p.GetQueueSize());
                    Console.WriteLine("   failure = " + p.GetFailure());
                    Console.WriteLine("   state = " + p.GetState());

                }

                if (e is Create)
                {
                    Create c = (Create)e;
                    string name = c.GetName();

                    Console.WriteLine(name);
                    Console.WriteLine("   quantity = " + c.GetQuantity());
                }
                if (e is BankDispose) {
                    BankDispose d = (BankDispose)e;
                    string name = d.GetName();

                    Console.WriteLine(name);
                    Console.WriteLine("   quantity = " + d.GetQuantity());
                    Console.WriteLine("   average interval = " + d.GetAverageInterval());
                }
            }
        }

        public void PrintFinalStats(List<AbstractElement> elements, List<INode> nodes, double tcurr)
        {
            Console.WriteLine("\n-------------RESULTS-------------");
            Console.WriteLine("Simulation time - " + simulationTime);
            int totalSwapAmount = 0;
            foreach (AbstractElement e in elements)
            {
                Console.Write("\n");
                if (e is Create) {
                    Create c = (Create)e;
                    string name = c.GetName();
                    int quantity = c.GetQuantity();

                    Console.WriteLine(name);
                    Console.WriteLine("  quantity = " + quantity);
                }
                if (e is Process)
                {
                    Process p = (Process)e;

                    string name = p.GetName();
                    double meanLengthOfQueue = p.GetMeanQueue() / tcurr;
                    double workload = p.GetWorkTime() / tcurr;
                    double failureProbability = p.GetFailure() / (p.GetFailure() + (double)p.GetQuantity());
                    int quantity = p.GetQuantity();
                    IDequeueListener? listener = p.GetDequeueListener();
                    int swapAmount = 0;
                    if (listener is SwappingClientsDequeueListener) {
                        SwappingClientsDequeueListener l = (SwappingClientsDequeueListener)listener;
                        swapAmount = l.GetSwapAmount();
                        totalSwapAmount += swapAmount;
                    }

                    Console.WriteLine(name);
                    Console.WriteLine("  quantity = " + quantity);
                    Console.WriteLine("  mean length of queue = " + meanLengthOfQueue);
                    Console.WriteLine("  workload = " + workload);
                    Console.WriteLine("  failure probabilitye = " + failureProbability);
                    Console.WriteLine("  swapAmount = " + swapAmount);
                }
                if (e is BankDispose)
                {
                    BankDispose d = (BankDispose)e;
                    string name = d.GetName();

                    Console.WriteLine(name);
                    Console.WriteLine("   quantity = " + d.GetQuantity());
                    Console.WriteLine("   average interval = " + d.GetAverageInterval());
                }
            }
            Console.WriteLine("\nGlobal stats:");
            Console.WriteLine("  total swapAmount = " + totalSwapAmount);
            Console.WriteLine("  average clients in bank = " + averageClientsInBank / tcurr);
        }
    }
}
