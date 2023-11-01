using ModelingOfSystems1.Core.EmbeddedDistribution;
using ModelingOfSystems1.Core.DistributionCalculator;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace ModelingOfSystems1.Core.Generators
{
    public class ExponentialGenerator
    {
        private EmbeddedGenerator _embeddedGenerator;

        public ExponentialGenerator(EmbeddedGenerator embeddedGenerator) {
            _embeddedGenerator = embeddedGenerator;
        }

        private double calculateTheoreticalPValue(double x1, double x2, double l) {
            double p = (Math.Exp(-l * x2) - Math.Exp(-l * x1));
            return p;
        }

        public List<Bin> calculateTheoreticalP(List<Bin> n, double l) {
            List<Bin> pT = new List<Bin>();

            double pTValue = 0;
            for (int i = 0; i < n.Count; i++) {
                (double x1, double x2) = n[i].getRange();

                pTValue = calculateTheoreticalPValue(x2, x1, l);

                pT.Add(new Bin(x1, x2, pTValue));
            }

            return pT;
        }

        public double computeChiSquared(List<Bin> n, double l)
        {
            const int merge_bin_size_limit = 5;
            double sum1 = n.Select(x => x.getSize()).Sum();
            List<Bin> preparedN = Bin.mergeSmallBins(n, merge_bin_size_limit);
            double sum = preparedN.Select(x => x.getSize()).Sum();
            List<Bin> pT = calculateTheoreticalP(preparedN, l);
            double X2 = DistributionUtils.computeChiSquared(preparedN, pT);

            return X2;
        }

        public List<double> generateDistribution(int count, double l) {
            List<double> distribution = new List<double>();

            for (int i = 0; i < count; i++) {
                double randomNum = _embeddedGenerator.getRandomDouble();

                double x = -(1 / l) * (Math.Log(randomNum));

                distribution.Add(x);
            }

            return distribution;
        }

        public List<Bin> generateTheoreticalBinsPDF(List<Bin> bins, double l, double h)
        {
            List<Bin> theoreticalBins = new List<Bin>();
            double x = 0;
            double v = 0;
            for (int i = 0; i < bins.Count; i++)
            {
                (double, double) range = bins[i].getRange();

                Bin bin = new Bin(range.Item1, range.Item2);
                x = (range.Item1 + range.Item2) / 2.0;
                v = l * Math.Exp(-l*x);
                bin.setSize(v);
                theoreticalBins.Add(bin);
            }

            return theoreticalBins;
        }
    }
}
