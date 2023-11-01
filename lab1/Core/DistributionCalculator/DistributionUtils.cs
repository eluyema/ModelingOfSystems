using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingOfSystems1.Core.DistributionCalculator
{
    public class DistributionUtils
    {
        private static Dictionary<int, double> criticalValues = new Dictionary<int, double>
        {
            {1, 3.841},
            {2, 5.991},
            {3, 7.815},
            {4, 9.488},
            {5, 11.070},
            {6, 12.592},
            {7, 14.067},
            {8, 15.507},
            {9, 16.919},
            {10, 18.307},
            {11, 19.675},
            {12, 21.026},
            {13, 22.362},
            {14, 23.685},
            {15, 24.996},
            {16, 26.296},
            {17, 27.587},
            {18, 28.869},
            {19, 30.144},
            {20, 31.410},
            {21, 32.671},
            {22, 33.924},
            {23, 35.172},
            {24, 36.415},
            {25, 37.652},
            {26, 38.885},
            {27, 40.113},
            {28, 41.337},
            {29, 42.557},
            {30, 43.773}
        };



        static public double getCriticalChiSquared(int degreesOfFreedom) {
            double criticalValue = 0;
            if (criticalValues.TryGetValue(degreesOfFreedom, out criticalValue))
            {
                return criticalValue;
            }
            else
            {
                throw new ArgumentException($"Critical value for {degreesOfFreedom} degrees of freedom not found in the table.");
            }
        }
        static public double computeChiSquared(List<Bin> n, List<Bin> pT) {
            double nSum = n.Select(x => x.getSize()).Sum();
            double X2 = 0;
            
            for (int i = 0; i < n.Count; i++) {
                X2 += (n[i].getSize() - nSum * pT[i].getSize()) * (n[i].getSize() - nSum * pT[i].getSize()) / (nSum * pT[i].getSize());
            }

            return X2;
        }

        public static double calculateDispersion(List<double> distribution)
        {
            double u = distribution.Average();
            double d = 0;
            double count = distribution.Count;
            double sum = distribution.Sum(val => Math.Pow((val - u), 2));
            d = Math.Sqrt(sum / (count - 1));
            return d;
        }

        public static double getCriticalChiSquared(List<Bin> n)
        {
            const int merge_bin_size_limit = 5;

            List<Bin> preparedN = Bin.mergeSmallBins(n, merge_bin_size_limit);
            double X2cr = DistributionUtils.getCriticalChiSquared(preparedN.Count - 1);

            return X2cr;
        }

        public static List<Bin> generateBins(List<double> distribution, int barCount, double min, double max, double h)
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

        public static List<Bin> generateBinsPDF(List<double> distribution, int barCount, double min, double max, double h)
        {
            List<Bin> bins = generateBins(distribution, barCount, min, max, h);
            for (int i = 0; i < bins.Count; i++)
            {
                double value = bins[i].getSize() / (distribution.Count * h);
                bins[i].setSize(value);
            }
            return bins;
        }
    }
}
