using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC
{
    [Serializable]
    public class HydraulicCalculator
    {
        public bool Material { get; set; } = true;
        public int LotNumber { get; set; }
        public double Speed { get { return Consumption / (Math.Sqrt(Diametr) * Math.PI / 4); } set { } }
        public double Consumption { get; set; }
        public double Diametr { get; set; }
        public double Lenght { get; set; }
        public double LocalResistanceCoefficient { get; set; }
        public double ActualLenght {
            get
            {
                return Lenght + Diametr / 100 / HydraulicFrictionCoefficient * LocalResistanceCoefficient;
            }
            private set { }
        }
        public double Reynolds
        {
            get
            {
                return CalcReynolds();
            }
            private set { }
        }
        public double HydraulicFrictionCoefficient
        {
            get
            {
                return CalcHydraulicFrictionCoefficient();
            }
            private set { }
        }
        public double Pressure { get; set; }
        public double PressureDrop
        {
            get
            {
                return CalculatorPressureDrop();
            }
            private set{} }
        public HydraulicCalculator(int lotNumber, double consumption, double diametr, double lenght, double pressure )
        {
            LotNumber = lotNumber;
            Consumption = consumption;
            Diametr = diametr;
            Lenght = lenght;
            Pressure = pressure;
        }
        public HydraulicCalculator(int lotNumber) : this(lotNumber, 0, 0, 0, 0){ }
        private double CalculatorPressureDrop()
        {            
            if(Pressure == 0 || Pressure < 0)
                return 0;
            else
                return Pressure - (1.2687*Math.Pow(10, (-4))*HydraulicFrictionCoefficient*Math.Pow(Consumption, 2)*0.68*ActualLenght/Math.Pow(Diametr, 5));
        }
        private double CalcHydraulicFrictionCoefficient()
        {
            double n;
            if(Material == true)
            {
                n = 0.0007;
            }
            else
            {
                n = 0.01;
            }

            if (Reynolds <= 2000)
            {
                return 64 / Reynolds;
            }
            else if(Reynolds > 2000 && Reynolds <= 4000)
            {
                return 0.0025 * Math.Pow(Reynolds, 0.333);
            }
            else if(Reynolds>4000 && Reynolds <= 10000 && Reynolds*n/Diametr < 23)
            {
                return 0.3164 / (Math.Pow(Reynolds, 0.25));
            }
            else if (Reynolds > 10000 && Reynolds * n / Diametr < 23)
            {
                return 1 / Math.Sqrt(1.82 * Math.Log10(Reynolds) - 1.64);
            }
            else //(Reynolds > 4000 && Reynolds * n / Diametr > 23)
            {
                return 0.11 * Math.Pow((n / Diametr + 68 / Reynolds), 0.25);
            }
        }
        private double CalcReynolds()
        {
            double result = 0.0354 * Consumption / (Diametr * 14.71 / Math.Pow(10, 6));
            Reynolds = result;
            return result;
        }
        
    }
}
