﻿using System;
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
        private BindingSource bindingSource_listBox1, bindingSource_checkedListBox1;

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
            bindingSource_listBox1 = new BindingSource();
            bindingSource_listBox1.DataSource = appData.appFrageboegen;

            listBox1.DataSource = bindingSource_listBox1;
            listBox1.DisplayMember = "strName";

            bindingSource_checkedListBox1 = new BindingSource();
            bindingSource_checkedListBox1.DataSource = appData.appFragen;

            ((ListBox)checkedListBox1).DataSource = bindingSource_checkedListBox1;
            ((ListBox)checkedListBox1).DisplayMember = "strFragetext";

            if (listBox1.Items.Count > 0)
                listBox1.SetSelected(0, true);

            refreshViews();
        }

        private void refreshViews()
        {
            bindingSource_listBox1.ResetBindings(false);
            bindingSource_checkedListBox1.ResetBindings(false);

            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                //MessageBox.Show(checkedListBox1.GetItemText(checkedListBox1.Items[i]), "checkedListBox1.GetItemText(i)", MessageBoxButtons.OK);
                if (appData.appFrageboegen[listBox1.SelectedIndex].isContaining(checkedListBox1.GetItemText(checkedListBox1.Items[i])))
                {
                    checkedListBox1.SetItemChecked(i, true);
                }
            }
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
            this.Close();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            NeueFrage dlgNeueFrage = new NeueFrage();
            dlgNeueFrage.textBox1.Focus();

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
                //bindingSource_listBox3.ResetBindings(false);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            NeuerFragebogen dlgNeuerFragebogen = new NeuerFragebogen();

            dlgNeuerFragebogen.textBox1.Focus();

            dlgNeuerFragebogen.ShowDialog();
            if (dlgNeuerFragebogen.DialogResult == DialogResult.OK)
            {
                appData.appFrageboegen.Add(new Fragebogen(dlgNeuerFragebogen.textBox1.Text, new List<Frage>()));
                saveData();

                bindingSource_listBox1.ResetBindings(false);
            }
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*bindingSource_listBox2.DataSource = appData.appFrageboegen[listBox1.SelectedIndex].Fragen;
            listBox2.DisplayMember = "strFragetext";
            //bindingSource_listBox2.ResetBindings(false);*/
        }

        private void ListBox3_DoubleClick(object sender, EventArgs e)
        {
            /*if (appData.appFrageboegen[listBox1.SelectedIndex].isContaining(appData.appFragen[listBox3.SelectedIndex]))
            {
                MessageBox.Show("Die ausgewählte Frage ist im Fragebogen schon vorhanden.", "Frage vorhanden",
                    MessageBoxButtons.OK);
                return;
            }*/
            //appData.appFrageboegen[listBox1.SelectedIndex].Fragen.Add(appData.appFragen[listBox3.SelectedIndex]);
            saveData();
            //bindingSource_listBox2.ResetBindings(false);
            MessageBox.Show("Die ausgewählte Frage wurde dem Fragebogen hinzugefügt.", "Frage hinzugefügt",
                MessageBoxButtons.OK);
        }

        private void ListBox2_DoubleClick(object sender, EventArgs e)
        {
            //appData.appFrageboegen[listBox1.SelectedIndex].Fragen.Remove(appData.appFrageboegen[listBox1.SelectedIndex].Fragen[listBox2.SelectedIndex]);
            saveData();
            //bindingSource_listBox2.ResetBindings(false);
            MessageBox.Show("Die ausgewählte Frage wurde aus dem Fragebogen entfernt.", "Frage entfernt",
                MessageBoxButtons.OK);
        }

        private void Button4_Click(object sender, EventArgs e)
        {

        }

        private void CheckedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                appData.appFrageboegen[listBox1.SelectedIndex].Fragen.Add(appData.appFragen[e.Index]);
                MessageBox.Show("Die Frage: \n\n" + appData.appFragen[e.Index].strFragetext + "\n\nwurde dem Fragebogen "
                    + appData.appFrageboegen[listBox1.SelectedIndex].strName + " hinzugefügt.,",
                    "Frage hinzugefügt", MessageBoxButtons.OK);
                saveData();
            }
            else if (e.NewValue == CheckState.Unchecked)
            {
                appData.appFrageboegen[listBox1.SelectedIndex].Fragen.Remove(appData.appFragen[e.Index]);
                saveData();
                MessageBox.Show("appData.appFrageboegen[listBox1.SelectedIndex].Fragen.Count() = " 
                    + appData.appFrageboegen[listBox1.SelectedIndex].Fragen.Count,
                    "Anzahl", MessageBoxButtons.OK);
                foreach (Frage item in appData.appFrageboegen[listBox1.SelectedIndex].Fragen)
                {
                    if (item.strFragetext.Equals(appData.appFragen[e.Index].strFragetext))
                    {
                        if (appData.appFrageboegen[listBox1.SelectedIndex].Fragen.Remove(item))
                        {
                            MessageBox.Show("Die Frage: \n\n" + appData.appFragen[e.Index].strFragetext + "\n\nwurde aus dem Fragebogen entfernt.,",
                                "Frage entfernt", MessageBoxButtons.OK);
                            saveData();
                        }
                        else
                        {
                            MessageBox.Show("Die Frage: \n\n" + appData.appFragen[e.Index].strFragetext + "\n\nkonnte nicht aus dem Fragebogen entfernt werden.,",
                                "Fehler", MessageBoxButtons.OK);
                        }
                        return;
                    }
                }
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {

        }
    }
}
