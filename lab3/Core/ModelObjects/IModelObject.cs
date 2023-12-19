namespace ModelingSystem3.Core.ModelObjects
{
    public interface IModelObject
    {
        public void StartProcessing(string processName, double tcurr);
        public void FinishProcessing(string processName, double tcurr);
    }
}
