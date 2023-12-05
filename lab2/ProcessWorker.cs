namespace ModelingSystem2
{
    public class ProcessWorker
    {
        public ProcessWorker(int state, double tnext)
        {
            this.state = state;
            this.tnext = tnext;
            this.workDuration = 0;
        }

        private int state;
        private double tnext;
        private double workDuration;

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

        public void StartWork(double tnext, double duration)
        {
            this.tnext = tnext;
            this.workDuration = duration;
            this.state = 1;
        }

        public void StopWork()
        {
            this.state = 0;
            this.workDuration = 0;
            this.tnext = Double.MaxValue;
        }
    }
}
