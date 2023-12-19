using ModelingSystem3.Bank.RulesImplementations.Elements;
using ModelingSystem3.Bank.RulesImplementations.Elements.Listeners;
using ModelingSystem3.Bank.RulesImplementations.Model.Strategies;
using ModelingSystem3.Bank.RulesImplementations.ModelObjects;
using ModelingSystem3.Core.Elements;
using ModelingSystem3.Core.Models;
using ModelingSystem3.Core.Nodes;
using ModelingSystem3.Core.Simulations;

namespace ModelingSystem3.Bank
{
    public class BankSimulation : ISimulation
    {
        public void StartTestSimulation()
        {
            int simulationTime = 1000;

            BankClientFabric fabric = new BankClientFabric();

            Create create = new Create("Create", 0.5, fabric);

            Process cashier1 = new Process("cashier 1", 0.3);
            Process cashier2 = new Process("cashier 1", 0.3);

            PriorityNode priorityNode = new PriorityNode(new List<(Process, int)> { (cashier1, 1), (cashier2, 2) });

            SwappingClientsDequeueListener cashier1Listener = new SwappingClientsDequeueListener();
            cashier1Listener.AddNeighbour(cashier2);

            SwappingClientsDequeueListener cashier2Listener = new SwappingClientsDequeueListener();
            cashier2Listener.AddNeighbour(cashier1);



            create.SetNode(priorityNode);
            create.SetDistribution(DistributionType.EXP);

            cashier1.SetMaxqueue(3);
            cashier1.SetDequeueListener(cashier1Listener);
            cashier1.SetDistribution(DistributionType.EXP);

            cashier2.SetMaxqueue(3);
            cashier2.SetDequeueListener(cashier2Listener);
            cashier2.SetDistribution(DistributionType.EXP);


            List<INode> nodes = new List<INode> { priorityNode };
            List<AbstractElement> elements = new List<AbstractElement> { create, cashier1, cashier2 };

            BankStatisticsStrategy statisticsStrategy = new BankStatisticsStrategy(simulationTime);

            Model model = new Model(elements, nodes, statisticsStrategy);

            model.Simulate(simulationTime, true);
        }

        public void StartSimulation() {
            int simulationTime = 200;

            BankClientFabric fabric = new BankClientFabric();

            Create create = new Create("Create", 0.5, fabric);

            Process cashier1 = new Process("cashier 1", 1);
            Process cashier2 = new Process("cashier 2", 1);

            BankDispose dispose = new BankDispose("Dispose");

            CommonNode commonNode = new CommonNode(new List<AbstractElement>{dispose});

            cashier1.InAct(fabric.generateNextModelObject(0));
            cashier1.InAct(fabric.generateNextModelObject(0));
            cashier1.InAct(fabric.generateNextModelObject(0));

            cashier2.InAct(fabric.generateNextModelObject(0));
            cashier2.InAct(fabric.generateNextModelObject(0));
            cashier2.InAct(fabric.generateNextModelObject(0));

            PriorityNode priorityNode = new PriorityNode(new List<(Process, int)> { (cashier1, 1), (cashier2, 2) });

            SwappingClientsDequeueListener cashier1Listener = new SwappingClientsDequeueListener();
            cashier1Listener.AddNeighbour(cashier2);

            SwappingClientsDequeueListener cashier2Listener = new SwappingClientsDequeueListener();
            cashier2Listener.AddNeighbour(cashier1);



            create.SetNode(priorityNode);
            create.SetTnext(0.1);
            create.SetDistribution(DistributionType.EXP);

            cashier1.SetMaxqueue(3);
            cashier1.SetDequeueListener(cashier1Listener);
            cashier1.SetDistribution(DistributionType.NORM);
            cashier2.SetDelayDev(0.3);
            cashier2.SetNode(commonNode);

            cashier2.SetMaxqueue(3);
            cashier2.SetDequeueListener(cashier2Listener);
            cashier2.SetDistribution(DistributionType.NORM);
            cashier2.SetDelayDev(0.3);
            cashier2.SetNode(commonNode);


            List<INode> nodes = new List<INode> { priorityNode };
            List <AbstractElement> elements = new List<AbstractElement> { create, cashier1, cashier2, dispose };

            BankStatisticsStrategy statisticsStrategy = new BankStatisticsStrategy(simulationTime);

            Model model = new Model(elements, nodes, statisticsStrategy);

            model.Simulate(simulationTime, true);
        }
    }
}
