namespace ModelingSystem2
{
    public class Process : Element
    {
        private int queue, maxqueue, failure;
        private double meanQueue;
        private List<ProcessWorker> workers;

        public Process(double delay, int workerUnits = 1) : base(delay)
        {
            queue = 0;
            maxqueue = int.MaxValue;
            meanQueue = 0.0;
            workers = new List<ProcessWorker>();
            if (workerUnits < 1)
            {
                throw new ArgumentOutOfRangeException("workerUnits must be more than 0");
            }
            for (int i = 0; i < workerUnits; i++)
            {
                workers.Add(new ProcessWorker(state: 0, tnext: Double.MaxValue));
            }
        }

        private ProcessWorker? GetFreeWorker()
        {
            return this.workers.Find(worker => worker.GetState() == 0);
        }

        private List<ProcessWorker> GetAllBusyWorkers()
        {
            return this.workers.FindAll(worker => worker.GetState() == 1);
        }

        public override double GetTnext()
        {
            List<ProcessWorker> busyWorkers = GetAllBusyWorkers();

            if (busyWorkers.Count == 0)
            {
                return Double.MaxValue;
            }

            return busyWorkers.Min(worker => worker.GetTnext());
        }

        public ProcessWorker? GetNearestBusyWorker()
        {
            List<ProcessWorker> busyWorkers = GetAllBusyWorkers();

            double minTnext = busyWorkers.Min(worker => worker.GetTnext());

            return busyWorkers.Find(worker =>
                worker.GetTnext() == minTnext);
        }

        public override int GetState()
        {
            ProcessWorker? freeWorker = GetFreeWorker();

            if (freeWorker is not null)
            {
                return 0;
            }
            return 1;
        }

        public override void InAct()
        {
            ProcessWorker? freeWorker = GetFreeWorker();

            if (freeWorker is not null)
            {

                double workDuration = base.GetDelay();
                double tnext = base.GetTcurr() + workDuration;

                freeWorker.StartWork(tnext, workDuration);
            }
            else
            {
                if (GetQueue() < GetMaxqueue())
                {
                    SetQueue(GetQueue() + 1);
                }
                else
                {
                    failure++;
                }
            }
        }

        public override void OutAct()
        {
            base.OutAct();

            ProcessWorker? worker = GetNearestBusyWorker();

            if (worker is null)
            {
                throw new InvalidOperationException("PROCESS doesn't have busy workers");
            }

            base.AddWorkload(worker.GetWorkDuration());
            worker.StopWork();

            if (GetQueue() > 0)
            {
                SetQueue(GetQueue() - 1);
                this.InAct();
            }


            List<Element> elements = base.GetNextElements();
            int count = elements.Count;
            if (count == 0)
            {
                return;
            }
            Random rnd = new Random();
            int index = rnd.Next(count);
            Element element = elements[index];
            element.InAct();
        }

        public int GetFailure()
        {
            return failure;
        }
        public int GetQueue()
        {
            return queue;
        }

        public void SetQueue(int queue)
        {
            this.queue = queue;
        }
        public int GetMaxqueue()
        {
            return maxqueue;
        }
        public void SetMaxqueue(int maxqueue)
        {
            this.maxqueue = maxqueue;
        }
        public override void PrintInfo()
        {
            base.PrintInfo();
            System.Console.WriteLine("queue = " + this.GetQueue());
            System.Console.WriteLine("failure = " + this.GetFailure());
        }
        public override void DoStatistics(double delta)
        {
            meanQueue = GetMeanQueue() + queue * delta;
        }
        public double GetMeanQueue()
        {
            return meanQueue;
        }
    }
}
