using ModelingOfSystems1.Core.DistributionCalculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingOfSystems1.Core.Generators
{
    public class BaseGenerator
    {
        public double getCriticalChiSquared(List<Bin> n)
        {
            const int merge_bin_size_limit = 5;

            List<Bin> preparedN = Bin.mergeSmallBins(n, merge_bin_size_limit);
            double X2cr = DistributionUtils.getCriticalChiSquared(preparedN.Count - 1);

            return X2cr;
        }

        public List<Bin> generateBinsPDF(List<double> distribution, int barCount, double min, double max, double h)
        {
            List<Bin> bins = generateBins(distribution, barCount, min, max, h);
            for (int i = 0; i < bins.Count; i++)
            {
                double value = bins[i].getSize() / (distribution.Count * h);
                bins[i].setSize(value);
            }
            return bins;
        }

        public List<Bin> generateBins(List<double> distribution, int barCount, double min, double max, double h)
        {
            List<Bin> bins = new List<Bin>();

            double startAt = min;
            for (int i = 0; i < barCount; i++)
            {
                if (i == barCount - 1)
                {
                    bins.Add(new Bin(startAt, max));
                }
                else
                {
                    bins.Add(new Bin(startAt, startAt + h));
                }

                startAt += h;
            }

            Bin.fillBins(bins, distribution);


            return bins;
        }
    }
}
