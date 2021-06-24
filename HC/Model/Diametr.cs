using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.Model
{
    public class Diametr : IVariable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Value { get; set; }

        public void UnitOfMeasure()
        {
            
        }
    }
}
