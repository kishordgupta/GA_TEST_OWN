using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdversarialImage
{
    public partial class HeuristicPopulation : Form
    {
       
        public HeuristicPopulation()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

          


            this.Close();
        }

        private void comboBox00_SelectedIndexChanged(object sender, EventArgs e)
        {
            AlgorithmList.G00 = (comboBox00.SelectedIndex + 1);
        }

        private void comboBox01_SelectedIndexChanged(object sender, EventArgs e)
        {
            AlgorithmList.G01 = (comboBox01.SelectedIndex + 1);
        }

        private void comboBox10_SelectedIndexChanged(object sender, EventArgs e)
        {
            AlgorithmList.G10 = (comboBox10.SelectedIndex + 1);
        }

        private void comboBox11_SelectedIndexChanged(object sender, EventArgs e)
        {
            AlgorithmList.G11 = (comboBox11.SelectedIndex + 1);
        }

        private void comboBox000_SelectedIndexChanged(object sender, EventArgs e)
        {
            AlgorithmList.G000 = (comboBox000.SelectedIndex + 1);
        }

        private void comboBox001_SelectedIndexChanged(object sender, EventArgs e)
        {
            AlgorithmList.G001 = (comboBox001.SelectedIndex + 1);
        }

        private void comboBox010_SelectedIndexChanged(object sender, EventArgs e)
        {
            AlgorithmList.G010 = (comboBox010.SelectedIndex + 1);
        }

        private void comboBox011_SelectedIndexChanged(object sender, EventArgs e)
        {
            AlgorithmList.G011 = (comboBox011.SelectedIndex + 1);
        }

        private void comboBox100_SelectedIndexChanged(object sender, EventArgs e)
        {
            AlgorithmList.G100 = (comboBox100.SelectedIndex + 1);
        }

        private void comboBox101_SelectedIndexChanged(object sender, EventArgs e)
        {
            AlgorithmList.G101 = (comboBox101.SelectedIndex + 1);
        }

        private void comboBox110_SelectedIndexChanged(object sender, EventArgs e)
        {
            AlgorithmList.G110 = (comboBox110.SelectedIndex + 1);
        }

        private void comboBox111_SelectedIndexChanged(object sender, EventArgs e)
        {
            AlgorithmList.G111 = (comboBox111.SelectedIndex + 1);
        }
    }
}
