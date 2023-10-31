using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingOfSystems1.Core.EmbeddedDistribution
{
    public class EmbeddedGenerator
    {
        private Random random;

        public EmbeddedGenerator() {
            random = new Random();
        }

        public double getRandomDouble()
        {
            return random.NextDouble();
        }
    }
}
