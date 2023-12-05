

namespace ModelingSystem2
{

    public class SimModel
    {
        public static void Main(String[] args)
        {
            System.Console.WriteLine("Application started\n");
            MultipleExitModelSimulation();
            //BasicModelSimulation();
            //ProcessChainModelSimulation();
           //WorkersModelSimulation();

        }

        public static void BasicModelSimulation() {
            bool showLogs = false;

            Create c = new Create(2.0);
            Process p = new Process(1.0);
            System.Console.WriteLine("id0 = " + c.GetId() + " id1=" + p.GetId());
            c.SetNextElement(p);
            p.SetMaxqueue(5);
            c.SetName("CREATOR");
            p.SetName("PROCESSOR");
            c.SetDistribution(DistributionType.EXP);
            p.SetDistribution(DistributionType.EXP);
            List<Element> list = new List<Element>
            {
                c,
                p
            };
            Model model = new Model(list);
            model.Simulate(1000.0, showLogs);
        }

        public static void ProcessChainModelSimulation() {
            bool showLogs = false;

            double simulationTime, delayC, delayDevC, delayP1, delayDevP1,
                delayP2, delayDevP2, delayP3, delayDevP3;
            int maxqueueP1, maxqueueP2, maxqueueP3;
            DistributionType distrType = DistributionType.EXP;
            {
                simulationTime = 1000;
                delayC =         2;
                delayDevC =      0.5;

                delayP1 =          2;
                delayDevP1 =       0.5;
                delayP2 =          2;
                delayDevP2 =       0.5;
                delayP3 =          2;
                delayDevP3 =       0.6;

                maxqueueP1 =       2;
                maxqueueP2 =       5;
                maxqueueP3 =       10;
            }

            List<Element> list = new List<Element>();

            Create c = new Create(delayC);
            Process p1 = new Process(delayP1);
            Process p2 = new Process(delayP2);
            Process p3 = new Process(delayP3);

            c.SetName("CREATOR");
            c.SetNextElement(p1);
            c.SetDistribution(distrType);
            c.SetDelayDev(delayDevC);
            list.Add(c);

            p1.SetMaxqueue(maxqueueP1);
            p1.SetName("PROCESSOR 1");
            p1.SetDistribution(distrType);
            p1.SetDelayDev(delayDevP1);
            p1.SetNextElement(p2);
            list.Add(p1);

            p2.SetMaxqueue(maxqueueP2);
            p2.SetName("PROCESSOR 2");
            p2.SetDistribution(distrType);
            p2.SetDelayDev(delayDevP2);
            p2.SetNextElement(p3);
            list.Add(p2);

            p3.SetMaxqueue(maxqueueP3);
            p3.SetName("PROCESSOR 3");
            p3.SetDistribution(distrType);
            p3.SetDelayDev(delayDevP3);
            list.Add(p3);

            Model model = new Model(list);
            model.Simulate(simulationTime, showLogs);
        }

        public static void WorkersModelSimulation()
        {
            bool showLogs = false;

            double simulationTime, delayC, delayDevC, delayP1, delayDevP1,
                delayP2, delayDevP2, delayP3, delayDevP3;
            int maxqueueP1, maxqueueP2, maxqueueP3;
            DistributionType distrType = DistributionType.EXP;
            {
                simulationTime = 1000;
                delayC = 2;
                delayDevC = 0.5;

                delayP1 = 2;
                delayDevP1 = 0.5;
                delayP2 = 2;
                delayDevP2 = 0.5;
                delayP3 = 2;
                delayDevP3 = 0.6;

                maxqueueP1 = 2;
                maxqueueP2 = 5;
                maxqueueP3 = 10;
            }

            List<Element> list = new List<Element>();

            Create c = new Create(delayC);
            Process p1 = new Process(delayP1);
            Process p2 = new Process(delayP2);
            Process p3 = new Process(delayP3);

            c.SetName("CREATOR");
            c.SetNextElement(p1);
            c.SetDistribution(distrType);
            c.SetDelayDev(delayDevC);
            list.Add(c);

            p1.SetMaxqueue(maxqueueP1);
            p1.SetName("PROCESSOR 1");
            p1.SetDistribution(distrType);
            p1.SetDelayDev(delayDevP1);
            p1.SetNextElement(p2);
            list.Add(p1);

            p2.SetMaxqueue(maxqueueP2);
            p2.SetName("PROCESSOR 2");
            p2.SetDistribution(distrType);
            p2.SetDelayDev(delayDevP2);
            p2.SetNextElement(p3);
            list.Add(p2);

            p3.SetMaxqueue(maxqueueP3);
            p3.SetName("PROCESSOR 3");
            p3.SetDistribution(distrType);
            p3.SetDelayDev(delayDevP3);
            list.Add(p3);

            Model model = new Model(list);
            model.Simulate(simulationTime, showLogs);
        }

        public static void MultipleExitModelSimulation()
        {
            bool showLogs = false;

            List<Element> list = new List<Element>();

            Create c = new Create(2);
            Process p1 = new Process(1);
            Process p2 = new Process(1);
            Process p3 = new Process(1);
            DistributionType distrType = DistributionType.EXP;

            c.SetName("CREATOR");
            c.SetNextElement(p1);
            c.SetDistribution(distrType);
            c.SetDelayDev(1);
            list.Add(c);

            p1.SetMaxqueue(5);
            p1.SetName("PROCESSOR 1");
            p1.SetDistribution(distrType);
            p1.SetDelayDev(1);
            p1.SetNextElements(new List<Element> { p2, p3 });
            list.Add(p1);

            p2.SetMaxqueue(5);
            p2.SetName("PROCESSOR 2");
            p2.SetDistribution(distrType);
            p2.SetDelayDev(1);
            p2.SetNextElement(p1);
            list.Add(p2);

            p3.SetMaxqueue(5);
            p3.SetName("PROCESSOR 3");
            p3.SetDistribution(distrType);
            p3.SetDelayDev(1);
            list.Add(p3);

            Model model = new Model(list);
            model.Simulate(1000.0, showLogs);
        }
    }
}