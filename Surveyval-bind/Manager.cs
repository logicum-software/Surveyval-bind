using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Surveyval_bind
{
    public partial class Manager : Form
    {
        private AppData appData;
        private BindingSource bindingSource1;

        public Manager()
        {
            InitializeComponent();

            appData = new AppData();

            //Daten einlesen aus Datei "udata.dat"
            IFormatter formatter = new BinaryFormatter();
            try
            {
                Stream stream = new FileStream("udata.dat", FileMode.Open, FileAccess.Read, FileShare.Read);
                appData = (AppData)formatter.Deserialize(stream);
                stream.Close();
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show(e.Message, "Dateifehler", MessageBoxButtons.OK);
                //throw;
            }

            // Data Bindings
            bindingSource1 = new BindingSource();
            bindingSource1.DataSource = appData.appFragen;

            ((ListBox)checkedListBox1).DataSource = bindingSource1;
            ((ListBox)checkedListBox1).DisplayMember = "strFragetext";
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            NeueFrage dlgNeueFrage = new NeueFrage();

            dlgNeueFrage.textBox1.Text = "Bitte geben Sie einen Fragetext ein...";
            dlgNeueFrage.textBox1.Focus();
            dlgNeueFrage.textBox1.SelectAll();

            dlgNeueFrage.ShowDialog();
        }
    }
}
