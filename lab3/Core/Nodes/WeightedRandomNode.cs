using ModelingSystem3.Core.ModelObjects;
using ModelingSystem3.Core.Elements;
using ModelingSystem3.Core.Distributions;

namespace ModelingSystem3.Core.Nodes
{
    public class WeightedRandomNode : INode
    {
        private readonly static double tolerance = 1e-6;
        private List<(AbstractElement, (double, double))> elementProbabilityRanges;

        public WeightedRandomNode(List<(AbstractElement, double)> elementProbabilities)
        {
            this.elementProbabilityRanges = GetProcessProbabilityRanges(elementProbabilities);
        }

        private List<(AbstractElement, (double, double))> GetProcessProbabilityRanges(List<(AbstractElement, double)> elementProbabilities) {
            List<(AbstractElement, (double, double))> processProbabilityRanges = new List<(AbstractElement, (double, double))>();

            double currMinRange = 0;

            foreach (var (process, probability) in elementProbabilities) {

                (double, double) range = (currMinRange, currMinRange + probability);

                currMinRange += probability;

                processProbabilityRanges.Add((process, range));
            }

            bool isProbabilityCorrect = Math.Abs(currMinRange - 1.0) < tolerance;

            if (!isProbabilityCorrect) {
                throw new ArgumentException("Sum of probabilities is not equal to 1");
            }

            var lastProcess = processProbabilityRanges[^1].Item1;
            var lastRangeStart = processProbabilityRanges[^1].Item2.Item1;
            processProbabilityRanges[^1] = (lastProcess, (lastRangeStart, 1));

            return processProbabilityRanges;
        }


        public void TriggerAct(IModelObject bankClient)
        {
            AbstractElement? selectedElement = null;

            double randomPoint = FunRand.Unif(0, 1);


            foreach (var (process, range) in elementProbabilityRanges) {
                if (IsPointInRange(randomPoint, range)) {
                    selectedElement = process;
                    break;
                }
            }
            if (selectedElement is null) {
                throw new InvalidOperationException("The elements have corrupted ranges, and none was found within the probability range (0, 1).");
            }
            selectedElement.InAct(bankClient);
        }

        private bool IsPointInRange(double point, (double, double) range)
        {
            return point >= range.Item1 - tolerance && point <= range.Item2 + tolerance;
        }

    }
}
