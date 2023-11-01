using MathNet.Numerics;
using ModelingOfSystems1.Core.DistributionCalculator;
using ModelingOfSystems1.Core.EmbeddedDistribution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingOfSystems1.Core.Generators
{
    public class UniformGenerator 
    {
        private double z = 3.1415926;
        private (double x1, double x2) _range = (0, 1);

        public UniformGenerator((double x1, double x2) range) {
            _range = range;
        }

        private double calculateCDF(double x)
        {
            if (x < _range.x1)
            {
                return 0;
            }
            else if (x >= _range.x2)
            {
                return 1;
            }
            else
            {
                return (x - _range.x1) / (_range.x2 - _range.x1);
            }
        }

        private double calculateTheoreticalPValue(double x1, double x2)
        {
            double p = calculateCDF(x2) - calculateCDF(x1);
            return p;
        }

        public List<double> generateDistribution(int count, double a, double c)
        {

            List<double> distribution = new List<double>();
            double x = 0;
            while (count > distribution.Count)
            {
                z = (a * z) % (c);
                x = z / c;
                if (x >= _range.x1 && x <= _range.x2)
                {
                    distribution.Add(x);
                }
            }

            return distribution;
        }

        public List<Bin> generateTheoreticalBinsPDF(List<Bin> bins)
        {
            List<Bin> theoreticalBins = new List<Bin>();
            
            for (int i = 0; i < bins.Count; i++)
            {
                (double, double) range = bins[i].getRange();

                Bin bin = new Bin(range.Item1, range.Item2);

                double v = 1 / (_range.x2 - _range.x1);

                bin.setSize(v);
                theoreticalBins.Add(bin);
            }

            return theoreticalBins;
        }

        public List<Bin> calculateTheoreticalP(List<Bin> n)
        {
            List<Bin> pT = new List<Bin>();

            double pTValue = 0;
            for (int i = 0; i < n.Count; i++)
            {
                (double x1, double x2) = n[i].getRange();

                pTValue = calculateTheoreticalPValue(x1, x2);

                pT.Add(new Bin(x1, x2, pTValue));
            }

            return pT;
        }

        public double computeChiSquared(List<Bin> n)
        {
            const int merge_bin_size_limit = 5;
            double sum1 = n.Select(x => x.getSize()).Sum();
            List<Bin> preparedN = Bin.mergeSmallBins(n, merge_bin_size_limit);
            double sum = preparedN.Select(x => x.getSize()).Sum();
            List<Bin> pT = calculateTheoreticalP(preparedN);
            double X2 = DistributionUtils.computeChiSquared(preparedN, pT);

            return X2;
        }
    }
}
