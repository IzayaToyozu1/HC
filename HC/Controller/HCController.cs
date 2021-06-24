using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC.Controller
{
    

    public class HCController
    {
        public delegate void HCControllerDel();
        public event HCControllerDel EventAddRow; 
        private SaveLoader SL = new SaveLoader();
        public List<HydraulicCalculator> HCList { get; private set; }
        public HCController()
        {
            HCList = new List<HydraulicCalculator>();
            HCList.Add(new HydraulicCalculator(1));
        }       
        public void Remove(int row)
        {
            for (int i = row; i < HCList.Count-1; i++)
            {
                EnterValues(i,
                            HCList[i + 1].Consumption,
                            HCList[i + 1].Diametr,
                            HCList[i + 1].Lenght,
                            HCList[i + 1].Pressure,
                            HCList[i + 1].LocalResistanceCoefficient,
                            HCList[i + 1].Material);
            }
            HCList.Remove(HCList[HCList.Count-1]);
        }
        public void AddRowElsePressureIsNotNull(HydraulicCalculator HC)
        {
            if(HC.PressureDrop !=0 && HC.PressureDrop > 0 && HCList[HCList.Count - 1] == HC)
            {
                HCList.Add((new HydraulicCalculator(LotNumberSelection())));
                EventAddRow?.Invoke();
            }            
        }
        public void EnterValues(int row,
                                object consumption,
                                object diametr,
                                object lenght,
                                object pressure,
                                object localResistanceCoefficient,
                                bool material)
        {
            if (row == 0)
            {                
                HCList[row].Pressure = ParseDouble(pressure);
            }
            else
            {
                HCList[row].Pressure = HCList[row -1].PressureDrop;
            }
            HCList[row].Consumption = ParseDouble(consumption);
            HCList[row].Diametr = ParseDouble(diametr);
            HCList[row].Lenght = ParseDouble(lenght);
            HCList[row].LocalResistanceCoefficient = ParseDouble(localResistanceCoefficient);
            HCList[row].Material = material;
        }
        private int LotNumberSelection()
        {
            int nameLotNumber = 0;
                        
            for(int i = 0; i < HCList.Count; i++)
            {
                nameLotNumber = HCList[i].LotNumber + 1; 
            }
            return nameLotNumber;
        }
        
        public void Save(string path)
        {
            SaveLoader.Save<HydraulicCalculator>(path, HCList);
        }
        public void Load(string path)
        {
            HCList = SaveLoader.Load<HydraulicCalculator>(path) ?? HCList;
        }
        private double ParseDouble(object value)
        {
            if (double.TryParse(Convert.ToString(value), out double item))
                return item;
            else
                return 0;
        }
    }
}
