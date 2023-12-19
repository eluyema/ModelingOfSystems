using ModelingSystem3.Bank.RulesImplementations.Elements;
using ModelingSystem3.Bank.RulesImplementations.Elements.Listeners;
using ModelingSystem3.Clinic.RulesImplementations.Elements;
using ModelingSystem3.Core.Elements;
using ModelingSystem3.Core.Elements.Listeners;
using ModelingSystem3.Core.Models.Strategies;
using ModelingSystem3.Core.Nodes;
using System.Xml.Linq;

namespace ModelingSystem3.Clinic.RulesImplementations.Models.Strategies
{
    public class ClinicStatisticsStrategy : IStatisticsStrategy
    {
        private readonly double simulationTime;

        public ClinicStatisticsStrategy(double simulationTime)
        {
            this.simulationTime = simulationTime;

        }

        public void DoStatistics(List<AbstractElement> elements, List<INode> nodes, double delta)
        {

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
                if (e is ClinicDispose) {
                    ClinicDispose d = (ClinicDispose)e;
                    string name = d.GetName();

                    Console.WriteLine(name);
                    Console.WriteLine("   quantity = " + d.GetQuantity());
                }
            }
        }

        public void PrintFinalStats(List<AbstractElement> elements, List<INode> nodes, double tcurr)
        {
            Console.WriteLine("\n-------------RESULTS-------------");
            Console.WriteLine("\n        Clinic simulation!       ");
            Console.WriteLine("Simulation time - " + simulationTime);
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
                    double averageInterval = p.GetAverageInterval();
                    double failureProbability = p.GetFailure() / (p.GetFailure() + (double)p.GetQuantity());
                    int quantity = p.GetQuantity();

                    Console.WriteLine(name);
                    Console.WriteLine("  quantity = " + quantity);
                    Console.WriteLine("  mean length of queue = " + meanLengthOfQueue);
                    Console.WriteLine("  workload = " + workload);
                    Console.WriteLine("  failure probabilitye = " + failureProbability);
                    if (name == "Laboratory") {
                        Console.WriteLine("  average inerval = " + averageInterval);
                    }
                }
                if (e is ClinicDispose)
                {
                    ClinicDispose d = (ClinicDispose)e;
                    string name = d.GetName();
                    double averageTimeInClinic = d.GetAverageTimeInClinic();

                    Console.WriteLine(name);
                    Console.WriteLine("   quantity = " + d.GetQuantity());
                    Console.WriteLine("   time in clinic = " + averageTimeInClinic);
                }
            }

        }
    }
}
