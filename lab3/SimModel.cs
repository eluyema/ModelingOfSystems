
using ModelingSystem3.Bank;
using ModelingSystem3.Clinic;

namespace ModelingSystem3
{

    public class SimModel
    {
        public static void Main(String[] args)
        {
            System.Console.WriteLine("Application started\n");

            Clinic();
        }

        public static void Bank()
        {
            BankSimulation simulation = new BankSimulation();
            simulation.StartSimulation();
        }

        public static void Clinic()
        {
            ClinicSimulation simulation = new ClinicSimulation();
            simulation.StartSimulation();
        }
    }
}