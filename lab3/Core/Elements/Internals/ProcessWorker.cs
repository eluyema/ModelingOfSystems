using ModelingSystem3.Core.ModelObjects;

namespace ModelingSystem3.Core.Elements.Internals
{
    public class ProcessWorker
    {
        public ProcessWorker(int state, double tnext)
        {
            this.state = state;
            this.tnext = tnext;
            workDuration = 0;
        }

        private int state;
        private double tnext;
        private double workDuration;
        private IModelObject? modelObject;

        public int GetState()
        {
            return state;
        }

        public double GetTnext()
        {
            return tnext;
        }

        public double GetWorkDuration()
        {
            return workDuration;
        }

        public void StartWork(IModelObject modelObject, double tnext, double duration)
        {
            this.modelObject = modelObject;
            this.tnext = tnext;
            workDuration = duration;
            state = 1;
        }

        public IModelObject StopWork()
        {
            state = 0;
            workDuration = 0;
            tnext = double.MaxValue;
            IModelObject? processedModelObject = modelObject;
            if (processedModelObject is null)
            {
                throw new InvalidOperationException("Stop work must always have a non-null modelObject");
            }
            modelObject = null;
            return processedModelObject;
        }
    }
}
