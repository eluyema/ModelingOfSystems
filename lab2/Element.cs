

namespace ModelingSystem2
{

    public enum DistributionType
    {
        EXP,
        NORM,
        UNIF,
        None
    }

    public class Element
    {
        private String name;
        private double tnext;
        private double delayMean, delayDev;
        private DistributionType distribution;
        private int quantity;
        private double workload;
        private double tcurr;
        private int state;
        private List<Element> nextElements = new List<Element>();
        private static int nextId = 0;
        private int id;


        public Element()
        {

            tnext = Double.MaxValue;
            delayMean = 1.0;
            distribution = DistributionType.EXP;
            workload = 0.0;
            tcurr = 0;
            state = 0;
            id = nextId;
            nextId++;
            name = "element" + id;
        }
        public Element(double delay)
        {
            name = "anonymus";
            workload = 0.0;
            tnext = Double.MaxValue;
            delayMean = delay;
            distribution = DistributionType.EXP;
            tcurr = 0;
            state = 0;
            id = nextId;
            nextId++;
            name = "element" + id;
        }
        public Element(String nameOfElement, double delay)
        {
            name = nameOfElement;
            tnext = Double.MaxValue;
            delayMean = delay;
            distribution = DistributionType.EXP;
            tcurr = 0;
            state = 0;
            id = nextId;
            nextId++;
            name = "element" + id;
        }

        public double GetDelay()
        {
            double delayedMean = GetDelayMean();
            DistributionType type = getDistribution();

            switch (type)
            {
                case DistributionType.UNIF:
                    return FunRand.Unif(delayedMean - GetDelayDev(), delayedMean + GetDelayDev());
                case DistributionType.NORM:
                    return FunRand.Norm(delayedMean, GetDelayDev());
                case DistributionType.EXP:
                    return FunRand.Exp(delayedMean);
                default:
                    return delayedMean;
            }
        }



        public double GetDelayDev()
        {
            return delayDev;
        }
        public void SetDelayDev(double delayDev)
        {
            this.delayDev = delayDev;
        }
        public DistributionType getDistribution()
        {
            return distribution;
        }
        public void SetDistribution(DistributionType distribution)
        {
            this.distribution = distribution;
        }

        public int GetQuantity()
        {
            return quantity;
        }
        public double GetTcurr()
        {
            return tcurr;
        }
        public void SetTcurr(double tcurr)
        {
            this.tcurr = tcurr;
        }
        public virtual int GetState()
        {
            return state;
        }
        public void SetState(int state)
        {
            this.state = state;
        }
        public List<Element> GetNextElements()
        {
            return this.nextElements;
        }
        public virtual void SetNextElement(Element nextElement)
        {
            this.nextElements = new List<Element> { nextElement };
        }

        public virtual void SetNextElements(List<Element> nextElements)
        {
            this.nextElements = nextElements;
        }
        public virtual void InAct()
        {

        }
        public virtual void OutAct()
        {
            quantity++;
        }

        public void AddWorkload(double workloadUnit) {
            workload += workloadUnit;
        }

        public virtual double GetTnext()
        {
            return tnext;
        }
        public void SetTnext(double tnext)
        {
            this.tnext = tnext;
        }
        public double GetDelayMean()
        {
            return delayMean;
        }

        public double GetWorkload() {
            return workload;
        }

        public void SetDelayMean(double delayMean)
        {
            this.delayMean = delayMean;
        }
        public int GetId()
        {
            return id;
        }
        public void SetId(int id)
        {
            this.id = id;
        }

        public void PrintResult()
        {
            System.Console.WriteLine(GetName() + " quantity = " + quantity);
        }

        public virtual void PrintInfo()
        {
            System.Console.WriteLine(GetName() + " state= " + state +
                " quantity = " + quantity +
                " tnext= " + tnext);
        }
        public String GetName()
        {
            return name;
        }
        public void SetName(String name)
        {
            this.name = name;
        }
        public virtual void DoStatistics(double delta)
        {

        }
    }
}
