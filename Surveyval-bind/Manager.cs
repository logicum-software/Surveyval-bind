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

        private void saveData()
        {
            FileStream fs = new FileStream("udata.dat", FileMode.Create);

            // Construct a BinaryFormatter and use it to serialize the data to the stream.
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, appData);
            }
            catch (SerializationException ec)
            {
                MessageBox.Show(ec.Message, "Speicherfehler", MessageBoxButtons.OK);
                //Console.WriteLine("Failed to serialize. Reason: " + ec.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            NeueFrage dlgNeueFrage = new NeueFrage();
            dlgNeueFrage.textBox1.Text = "Neue Frage eingeben...";
            dlgNeueFrage.textBox1.Focus();
            dlgNeueFrage.button1.Enabled = false;

            dlgNeueFrage.ShowDialog();
            if (dlgNeueFrage.DialogResult == DialogResult.OK)
            {
                foreach (Frage item in appData.appFragen)
                {
                    if (String.Compare(item.strFragetext, dlgNeueFrage.textBox1.Text, true) > -1 &&
                        String.Compare(item.strFragetext, dlgNeueFrage.textBox1.Text, true) < 1)
                    {
                        if (MessageBox.Show("Die eingegebene Frage: \n\n" + dlgNeueFrage.textBox1.Text +
                            "\n\nscheint schon zu existieren.\n\nTrotzdem speichern?",
                            "Frage bereits vorhanden", MessageBoxButtons.YesNo) == DialogResult.No)
                            return;
                    }
                }

                if (dlgNeueFrage.radioButton2.Checked)
                    appData.appFragen.Add(new Frage(dlgNeueFrage.textBox1.Text, 1));
                else
                    appData.appFragen.Add(new Frage(dlgNeueFrage.textBox1.Text, 0));

                saveData();
                bindingSource1.ResetBindings(false);
            }
            else
            {

            }
        }
    }
}
