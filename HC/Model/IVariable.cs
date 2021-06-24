using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.Model
{
    interface IVariable
    {
        int Id { get; set; }
        string Name { get; set; }
        double Value { get; set; }
        void UnitOfMeasure();
    }
}
