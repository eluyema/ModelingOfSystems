namespace ModelingSystem2
{
    public class Create : Element
    {
        public Create(double delay) : base(delay)
        {
            
            base.SetTnext(0.0);
        }

        public override void OutAct()
        {
            base.OutAct();

            base.SetTnext(base.GetTcurr() + base.GetDelay());

            List<Element> elements = base.GetNextElements();
            int count = elements.Count;
            if (count == 0) {
                return;
            }
            Random rnd = new Random();
            int index = rnd.Next(count);
            Element element = elements[index];
            element.InAct();
        }
    }
}
