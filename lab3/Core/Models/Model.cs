using ModelingSystem3.Core.Elements;
using ModelingSystem3.Core.Models.Strategies;
using ModelingSystem3.Core.Nodes;

namespace ModelingSystem3.Core.Models
{
    public class Model
    {
        private List<AbstractElement> elementList;
        private List<INode> nodeList;
        double tnext, tcurr;
        int eventId;
        IStatisticsStrategy statisticsStrategy;

        public Model(List<AbstractElement> elements, List<INode> nodes, IStatisticsStrategy statisticsStrategy)
        {
            elementList = elements;
            nodeList = nodes;
            this.statisticsStrategy = statisticsStrategy;
            tnext = 0.0;
            eventId = 0;
            tcurr = tnext;
        }

        public void Simulate(double time, bool showLogs)
        {
            while (tcurr < time)
            {
                tnext = double.MaxValue;
                foreach (AbstractElement e in elementList)
                {
                    if (e.GetTnext() < tnext)
                    {
                        tnext = e.GetTnext();
                        eventId = e.GetId();
                    }
                }
                if (showLogs)
                {
                    Console.WriteLine("\nIt's time for event in " +
                      elementList.ElementAt(eventId).GetName() +
                      ", time = " + tnext);
                }
                foreach (AbstractElement e in elementList)
                {
                    e.DoStatistics(tnext - tcurr);
                }
                statisticsStrategy.DoStatistics(elementList, nodeList, tnext - tcurr);

                tcurr = tnext;
                foreach (AbstractElement e in elementList)
                {
                    e.SetTcurr(tcurr);
                }
                elementList.ElementAt(eventId).OutAct();
                foreach (AbstractElement e in elementList)
                {
                    if (e.GetTnext() == tcurr)
                    {
                        e.OutAct();
                    }
                }
                if (showLogs)
                {
                    statisticsStrategy.PrintStepStats(elementList, nodeList, tcurr);
                }
            }

            statisticsStrategy.PrintFinalStats(elementList, nodeList, tcurr);
        }
    }

}