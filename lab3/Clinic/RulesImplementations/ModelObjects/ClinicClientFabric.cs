using ModelingSystem3.Core.Distributions;
using ModelingSystem3.Core.Elements;
using ModelingSystem3.Core.ModelObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingSystem3.Clinic.RulesImplementations.ModelObjects
{
    public class ClinicClientFabric : IModelObjectFabric
    {
        private readonly static double tolerance = 1e-6;
        private List<(ClinicClientType, (double, double))> typeProbabilityRanges;

        public ClinicClientFabric(List<(ClinicClientType, double)> typesProbabilities) {
            this.typeProbabilityRanges = GetClinicTypeProbabilityRanges(typesProbabilities); ;
        }

        private List<(ClinicClientType, (double, double))> GetClinicTypeProbabilityRanges(List<(ClinicClientType, double)> elementProbabilities)
        {
            List<(ClinicClientType, (double, double))> typeProbabilityRanges = new List<(ClinicClientType, (double, double))>();

            double currMinRange = 0;

            foreach (var (clinicType, probability) in elementProbabilities)
            {

                (double, double) range = (currMinRange, currMinRange + probability);

                currMinRange += probability;

                typeProbabilityRanges.Add((clinicType, range));
            }

            bool isProbabilityCorrect = Math.Abs(currMinRange - 1.0) < tolerance;

            if (!isProbabilityCorrect)
            {
                throw new ArgumentException("Sum of probabilities is not equal to 1");
            }

            var lastProcess = typeProbabilityRanges[^1].Item1;
            var lastRangeStart = typeProbabilityRanges[^1].Item2.Item1;
            typeProbabilityRanges[^1] = (lastProcess, (lastRangeStart, 1));

            return typeProbabilityRanges;
        }

        private ClinicClientType GetNextClientType() {
            double randomPoint = FunRand.Unif(0, 1);

            ClinicClientType? selectedClientType = null;

            foreach (var (clientType, range) in typeProbabilityRanges)
            {
                if (IsPointInRange(randomPoint, range))
                {
                    selectedClientType = clientType;
                    break;
                }
            }
            if (selectedClientType is null)
            {
                throw new InvalidOperationException("The elements have corrupted ranges, and none was found within the probability range (0, 1).");
            }
            return (ClinicClientType)selectedClientType;
        }

        private bool IsPointInRange(double point, (double, double) range)
        {
            return point >= range.Item1 - tolerance && point <= range.Item2 + tolerance;
        }

        public IModelObject generateNextModelObject(double tcurr) {
            ClinicClientType clinicType = GetNextClientType();

            return new ClinicClient(clinicType, tcurr);
        }
    }
}
