using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Diagram.Database;

namespace Diagram
{
    public partial class SampleDiagram : Form
    {
        Database _db = new Database();
        List<Graph> _graphs = new List<Graph>();

        public SampleDiagram()
        {
            InitializeComponent();
            comboBoxRoms.Items.Clear();
            comboBoxRoms.DataSource = Enum.GetValues(typeof(RoomNames));
        }

        //Выводит время начала заготовки
        private void button1_Click(object sender, EventArgs e)
        {
            DateTime dateTime = dateTimePicker.Value;
            _graphs = _db.GetId(dateTime, (RoomNames) Enum.Parse(typeof(RoomNames), comboBoxRoms.SelectedValue.ToString()));
            if(_graphs == null) {return;}
            SetDataComboBox(_graphs);
        }

        private void SetDataComboBox(List<Graph> graphs)
        {
            for(int i = 0; i < graphs.Count; i++)
            {
                comboBoxPreparationStartDates.DataSource = graphs[i].GetPreparationStartDates();
            }
        }

        private void CountsNumberId(List<Graph> graphs)
        {
            for(int i = 0; i < graphs.Count; i++)
            {
                var id = graphs[i].GetCountID();
                MessageBox.Show(id.ToString());
            }
        }

        private void DrawGraph()
        {
        }
    }
}
