using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using MathNet.Numerics.Distributions;
using ModelingOfSystems1.Core.DistributionCalculator;
using ModelingOfSystems1.Core.EmbeddedDistribution;

namespace ModelingOfSystems1.Core.Generators
{
    public class NormalGenerator : BaseGenerator
    {
        private EmbeddedGenerator _embeddedGenerator;

        public NormalGenerator(EmbeddedGenerator embeddedGenerator) {
            _embeddedGenerator = embeddedGenerator;
        }

        private static double calculateCDF(double x, double mu, double sigma)
        {
            return 0.5 * (1 + SpecialFunctions.Erf((x - mu) / (sigma * Math.Sqrt(2))));
        }

        private double calculateTheoreticalPValue(double x1, double x2, double mu, double sigma)
        {
            double p = calculateCDF(x2, mu, sigma) - calculateCDF(x1, mu, sigma);
            return p;
        }

        public List<Bin> calculateTheoreticalP(List<Bin> n, double mu, double sigma)
        {
            List<Bin> pT = new List<Bin>();

            double pTValue = 0;
            for (int i = 0; i < n.Count; i++)
            {
                (double x1, double x2) = n[i].getRange();

                pTValue = calculateTheoreticalPValue(x1, x2, mu, sigma);

                pT.Add(new Bin(x1, x2, pTValue));
            }

            return pT;
        }

        public double computeChiSquared(List<Bin> n, double mu, double sigma)
        {
            const int merge_bin_size_limit = 5;
            double sum1 = n.Select(x => x.getSize()).Sum();
            List<Bin> preparedN = Bin.mergeSmallBins(n, merge_bin_size_limit);
            double sum = preparedN.Select(x => x.getSize()).Sum();
            List<Bin> pT = calculateTheoreticalP(preparedN, mu, sigma);
            double X2 = DistributionUtils.computeChiSquared(preparedN, pT);

            return X2;
        }

        public List<double> generateDistribution(int count, double a, double si)
        {
            List<double> distribution = new List<double>();
            double e = 0;
            double u = 0;
            double x = 0;
            int i = 0;
            int j = 0;
            for (i = 0; i < count; i++)
            {
                u = 0;
                for (j = 0; j < 12; j++) {
                    e = _embeddedGenerator.getRandomDouble();
                    u += e;
                }
                u -= 6;
                x = si * u + a;
                distribution.Add(x);
            }

            return distribution;
        }

        public List<Bin> generateTheoreticalBinsPDF(List<Bin> bins, double me, double sigma)
        {
            List<Bin> theoreticalBins = new List<Bin>();

            for (int i = 0; i < bins.Count; i++)
            {
                (double, double) range = bins[i].getRange();

                Bin bin = new Bin(range.Item1, range.Item2);
                double x = (range.Item1 + range.Item2) / 2.0;
                double v = (1 / (sigma * Math.Sqrt(2 * Math.PI))) * Math.Exp(-Math.Pow(x - me, 2) / (2 * sigma * sigma));
                bin.setSize(v);
                theoreticalBins.Add(bin);
            }

            return theoreticalBins;
        }
    }
}
