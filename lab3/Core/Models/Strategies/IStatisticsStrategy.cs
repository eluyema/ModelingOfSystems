using ModelingSystem3.Core.Elements;
using ModelingSystem3.Core.Nodes;

namespace ModelingSystem3.Core.Models.Strategies
{
    public interface IStatisticsStrategy
    {
        public void DoStatistics(List<AbstractElement> elements, List<INode> nodes, double delta);

        public void PrintStepStats(List<AbstractElement> elements, List<INode> nodes, double tcurr);

        public void PrintFinalStats(List<AbstractElement> elements, List<INode> nodes, double tcurr);
    }
}
