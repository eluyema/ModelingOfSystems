using ModelingSystem3.Core.Distributions;
using ModelingSystem3.Core.ModelObjects;

namespace ModelingSystem3.Core.Elements
{

    public enum DistributionType
    {
        EXP,
        NORM,
        UNIF,
        ERLANG,
        None,
    }

    public abstract class AbstractElement
    {
        protected string name;
        private double tnext;
        private double delayMean, delayDev;
        private int shape;
        private DistributionType distribution;
        private double tcurr;
        private int state;
        private static int nextId = 0;
        private int id;


        public AbstractElement(string elementName, DistributionType type = DistributionType.EXP)
        {
            name = elementName;
            tnext = double.MaxValue;
            delayMean = 1.0;
            shape = 3;
            distribution = type;
            tcurr = 0;
            state = 0;
            id = nextId;
            nextId++;
        }
        public AbstractElement(string elementName, double delay, DistributionType type = DistributionType.EXP)
        {
            name = elementName;
            tnext = double.MaxValue;
            delayMean = delay;
            distribution = type;
            tcurr = 0;
            state = 0;
            id = nextId;
            nextId++;
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
                case DistributionType.ERLANG:
                    return FunRand.Erlang(delayedMean, shape);
                default:
                    return delayedMean;
            }
        }

        public double GetDelayDev()
        {
            return delayDev;
        }
        public int GetShape() {
            return shape;
        }

        public void SetShape(int shape)
        {
            this.shape = shape;
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

        public double GetTcurr()
        {
            return tcurr;
        }

        public virtual void SetName(string name)
        {
            this.name = name;
        }

        public virtual string GetName()
        {
            return name;
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

        public virtual void PrintInfo()
        {

        }

        public virtual void OutAct()
        {

        }

        public virtual void InAct(IModelObject modelObject)
        {

        }

        public virtual void DoStatistics(double delta)
        {

        }
    }
}
