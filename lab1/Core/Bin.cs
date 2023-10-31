using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingOfSystems1.Core
{
    public class Bin
    {
        // [_startAt, _endAt) - range
        private double _startAt;
        private double _endAt;

        private double _size = 0;

        public (double, double) getRange() {
            return (_startAt, _endAt);
        }

        public Bin(double startAt, double endAt) {
            _startAt = startAt;
            _endAt = endAt;
        }

        public Bin(double startAt, double endAt, double size)
        {
            _startAt = startAt;
            _endAt = endAt;
            _size = size;
        }

        public Boolean tryAddToBin(double value, bool endInclude) {
            if (value  >= _startAt && value < _endAt) {
                _size++;
                return true;
            }
            if (endInclude && value == _endAt) {
                _size++;
                return true;
            }
            return false;
        }

        public double getMiddleNumber() {
            return (_endAt + _startAt) / 2;
        }

        public double getRangeLength() {
            return _startAt - _endAt;
        }

        public double getSize()
        {
            return _size;
        }

        public void setRange(double startAt, double endAt) {
            _startAt = startAt;
            _endAt = endAt;
        }

        public void setSize(double size) {
            _size = size;
        }

        public bool tryToJoinWithBin(Bin other) {
            const double epsilon = 1E-10; 

            if (Math.Abs(_endAt - other._startAt) < epsilon) 
            {
                _endAt = other._endAt;
                _size += other._size;
                return true;
            }
            else if (Math.Abs(_startAt - other._endAt) < epsilon)
            {
                _startAt = other._startAt;
                _size += other._size;
                return true;
            }
            return false;
        }

        public static void fillBins(List<Bin> bins, List<double> distribution) {
            int i = 0;
            int j = 0;

            bool inserted = false;

            for (i = 0; i < distribution.Count; i++) {
                inserted = false;

                for (j = 0; j < bins.Count; j++) {
                    inserted = bins[j].tryAddToBin(distribution[i], j == bins.Count - 1);
                    if (inserted) {
                        break;
                    }
                }
            }
        }

        public static List<Bin> mergeSmallBins(List<Bin> bins, int sizeLimit) {
            List<Bin> copiedList = Bin.getCopyList(bins);
            List<Bin> mergedBins = new List<Bin>(copiedList.OrderBy(bin => bin.getRange().Item1));

            if (!isBinsAdjacent(mergedBins)) {
                throw new ArgumentNullException("Bins must be be adjacent");
            }

            while (!isSmallBinsInList(mergedBins, sizeLimit)) {
                for (int i = 0; i < mergedBins.Count; i++) {
                    Bin bin = mergedBins[i];
                    Bin binForMerge = null;

                    if (i < mergedBins.Count - 1)
                    {
                        binForMerge = mergedBins[i + 1];
                    }
                    else {
                        binForMerge = mergedBins[i - 1];
                    }

                    if (bin.getSize() < sizeLimit) {

                        bool joinResult = binForMerge.tryToJoinWithBin(bin);

                        if (!joinResult) {
                            throw new InvalidOperationException($"Can't join bins ({bin.getRange()}) and {binForMerge.getRange()}");
                        }
                        mergedBins.RemoveAt(i);
                        break;
                    }
                }
            }

            return mergedBins;
        }

        public static bool binsTouch(Bin bin1,Bin bin2)
        {
            (double Start, double End) interval1 = bin1.getRange();
            (double Start, double End) interval2 = bin2.getRange();
            return interval1.End == interval2.Start || interval2.End == interval1.Start;
        }

        private static Boolean isBinsAdjacent(List<Bin> bins) {
            for (int i = 0; i < bins.Count - 1; i++)
            {

                if (!Bin.binsTouch(bins[i], bins[i+ 1]))
                {
                    return false;
                }
            }
            return true;
        }

        private static Boolean isSmallBinsInList(List<Bin> bins, int sizeLimit)
        {
            foreach (Bin bin in bins)
            {
                if (bin.getSize() < sizeLimit)
                {
                    return false;
                }
            }
            return true;
        }

        public static Bin getCopy(Bin bin) {
            Bin copiedBin = new Bin(bin._startAt, bin._endAt);

            copiedBin.setSize(bin.getSize());

            return copiedBin;
        }

        public static List<Bin> getCopyList(List<Bin> bins) {
            List<Bin> copiedBins = new List<Bin>();

            foreach (Bin bin in bins)
            {
                copiedBins.Add(Bin.getCopy(bin));
            }

            return copiedBins;
        }
    }
}
