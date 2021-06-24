using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HC;
using HC.Controller;

namespace HCForms
{
    public partial class MainForm : Form
    {
        DataGridViewComboBoxColumn MaterialColumn = new DataGridViewComboBoxColumn();
        bool material = true;
        HCController HCController = new HCController();
        private int row;
        OpenFileDialog OFD = new OpenFileDialog();
        SaveFileDialog SFD = new SaveFileDialog();
        SaveLoader SaveLoader = new SaveLoader();
        public MainForm()
        {
            MaterialColumn.HeaderText = "Material";
            MaterialColumn.Name = "Material";
            MaterialColumn.Items.AddRange("Steel", "Polyethylene");
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.ColumnCount = 12;
            dataGridView1.Columns[0].Name = "Lot number";
            dataGridView1.Columns.Insert(1, MaterialColumn);
            dataGridView1.Columns[2].Name = "Consumption";
            dataGridView1.Columns[3].Name = "Diametr";
            dataGridView1.Columns[4].Name = "Lenght";
            dataGridView1.Columns[5].Name = "Speed";
            dataGridView1.Columns[5].ReadOnly = true;
            dataGridView1.Columns[6].Name = "Local resistance coefficient";
            dataGridView1.Columns[7].Name = "Actual lenght";
            dataGridView1.Columns[7].ReadOnly = true;
            dataGridView1.Columns[8].Name = "Reynolds";
            dataGridView1.Columns[8].ReadOnly = true;
            dataGridView1.Columns[9].Name = "Hydraulic Friction Coefficient";
            dataGridView1.Columns[9].ReadOnly = true;
            dataGridView1.Columns[10].Name = "Pressure";
            dataGridView1.Columns[11].Name = "Pressure drop";

            dataGridView1.RowCount = 1;

            HCController.EventAddRow += HCController_EventAddRow;

            LoadTable();
        }

        public object ValueTable(int row, string nameColumns)
        {
            return dataGridView1[nameColumns, row].Value;
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            switch (Convert.ToString(dataGridView1["Material", row].Value))
            {
                case "Steel":
                    material = true;
                    break;
                case "Polyethylene":
                    material = false;
                    break;
            }
            HCController.EnterValues(row,
                ValueTable(row, "Consumption"),
                ValueTable(row, "Diametr"),
                ValueTable(row, "Lenght"),
                ValueTable(row, "Pressure"),
                ValueTable(row, "Local resistance coefficient"),
                material);
            HCController.AddRowElsePressureIsNotNull(HCController.HCList[row]);
            LoadTable();

        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            row = e.RowIndex;
        }
        private void LoadTable()
        {
            for (int i = 0; i < HCController.HCList.Count; i++)
            {
                dataGridView1["Consumption", i].Value = HCController.HCList[i].Consumption; ;
                dataGridView1["Diametr", i].Value = HCController.HCList[i].Diametr;
                dataGridView1["Lenght", i].Value = HCController.HCList[i].Lenght;
                dataGridView1["Local resistance coefficient", i].Value = HCController.HCList[i].LocalResistanceCoefficient;
                dataGridView1["Actual lenght", i].Value = HCController.HCList[i].ActualLenght;
                dataGridView1["Lot number", i].Value = HCController.HCList[i].LotNumber;
                dataGridView1["Speed", i].Value = HCController.HCList[i].Speed;
                dataGridView1["Reynolds", i].Value = HCController.HCList[i].Reynolds;
                dataGridView1["Hydraulic Friction Coefficient", i].Value = HCController.HCList[i].HydraulicFrictionCoefficient;
                dataGridView1["Pressure", i].Value = HCController.HCList[i].Pressure;
                dataGridView1["Pressure drop", i].Value = HCController.HCList[i].PressureDrop;
            }
            dataGridView1.Update();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SFD.ShowDialog() == DialogResult.Cancel)
                return;
            string path = SFD.FileName;
            HCController.Save(path);
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (OFD.ShowDialog() == DialogResult.Cancel)
                return;
            string path = OFD.FileName;
            HCController.Load(path);
            if (dataGridView1.Rows.Count < HCController.HCList.Count)
                for (int i = 0; i < HCController.HCList.Count - 1; i++)
                    dataGridView1.Rows.Add();
            LoadTable();
        }

        private void HCController_EventAddRow()
        {
            dataGridView1.Rows.Add();
        }

        private void dataGridView1_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            LoadTable();
        }

        private void dataGridView1_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        { 
            HCController.Remove(row);            
        }
    }
}
