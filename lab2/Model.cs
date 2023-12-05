namespace ModelingSystem2
{
    public class Model
    {
        private List<Element> list = new List<Element>();
        double tnext, tcurr;
        int eventId;
        public Model(List<Element> elements)
        {
            list = elements;
            tnext = 0.0;
            eventId = 0;
            tcurr = tnext;
        }

        public void Simulate(double time, bool showLogs)
        {
            while (tcurr < time)
            {
                tnext = Double.MaxValue;
                foreach (Element e in list)
                {
                    if (e.GetTnext() < tnext)
                    {
                        tnext = e.GetTnext();
                        eventId = e.GetId();
                    }
                }
                if (showLogs)
                {
                    System.Console.WriteLine("\nIt's time for event in " +
                      list.ElementAt(eventId).GetName() +
                      ", time = " + tnext);
                }
                foreach (Element e in list)
                {
                    e.DoStatistics(tnext - tcurr);
                }

                tcurr = tnext;
                foreach (Element e in list)
                {
                    e.SetTcurr(tcurr);
                }
                list.ElementAt(eventId).OutAct();
                foreach (Element e in list)
                {
                    if (e.GetTnext() == tcurr)
                    {
                        e.OutAct();
                    }
                }
                if (showLogs)
                {
                    PrintInfo();
                }
            }

            PrintResult(time);
        }

        public void PrintInfo()
        {
            foreach (Element e in list)
            {
                e.PrintInfo();
            }
        }
        public void PrintResult(double simulationTime)
        {
            System.Console.WriteLine("\n-------------RESULTS-------------");
            System.Console.WriteLine("Simulation time - " + simulationTime);
            foreach (Element e in list)
            {
                System.Console.Write("\n");
                e.PrintResult();
                if (e is Process)
                {
                    Process p = (Process)e;

                    double meanLengthOfQueue = p.GetMeanQueue() / tcurr;
                    double failureProbability = p.GetFailure() / (p.GetFailure() + (double)p.GetQuantity());
                    double workload = p.GetWorkload();
                    double queue = p.GetQueue();
                    double state = p.GetState();

                    System.Console.WriteLine("  mean length of queue = " +
                      meanLengthOfQueue +
                      "\n  last queue = " + queue +
                      "\n  last state = " + state +
                      "\n  failure probability = " +
                      failureProbability +
                      "\n  workload = " +
                      workload
                    );
                }
            }
        }

    }

}