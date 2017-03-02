using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Pallets.Views;
using Pallets.Models;
using Pallets.Controllers;

namespace Pallets
{

    public partial class App : Form
    {
        private DataC dc;
        private Data data;
        private EventMV wmv;
        public bool zapisano;

        public App()
        {
            dc = new DataC();
            InitializeComponent();
            zapisano = true;
        }

        private BindingList<Event> getWydarzenia()
        {
            data = dc.getData();
            return data.Wydarzenia;
        }

        private void getWidok()
        {
            MaximizeBox = false;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].Width = 100;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[3].HeaderText = "Firma";
            dataGridView1.Columns[3].Width = 100;
            dataGridView1.Columns[3].ReadOnly = true;
            dataGridView1.Columns[4].HeaderText = "Palety";
            dataGridView1.Columns[4].Width = 150;
            dataGridView1.Columns[4].ReadOnly = true;
            dataGridView1.Columns[5].Width = 300;
            dataGridView1.Columns[5].ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
        }

        private void Zarzadzanie_Load(object sender, EventArgs e)
        {
            BindingSource wydarzenia = new BindingSource();
            wydarzenia.DataSource = getWydarzenia().OrderBy(o => o.Data).ToList();
            dataGridView1.DataSource = wydarzenia;
            getWidok();
        }

        private void firmaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CompanyV nazwa = new CompanyV(data, this);
            nazwa.Show();
        }

        private void paletaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PaletteV nazwa = new PaletteV(data, this);
            nazwa.Show();
        }

        public void addData(string data2, BindingList<Event.MojePalety> mpalety, Company firma, string uwagi)
        {
            int id;
            if (dataGridView1.Rows.Count != 0 && String.Compare(data2, dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[2].Value.ToString(), true) < 0)
                id = dataGridView1.CurrentCellAddress.Y + 1;
            else
                id = dataGridView1.CurrentCellAddress.Y;
            zapisano = false;
            BindingSource w = new BindingSource();
            w.DataSource = dc.AddWydarzenie(data2, mpalety, firma, uwagi).OrderBy(o => o.Data).ThenBy(t => t.Firma.Nazwa);
            dataGridView1.DataSource = w;
            if (dataGridView1.Rows.Count != 1 && textBox1.Text == "")
                dataGridView1.CurrentCell = dataGridView1[2, id];
            else
                textBox1.Text = "";
        }

        public void editData(string data2, BindingList<Event.MojePalety> mpalety, Company firma, string uwagi)
        {
            zapisano = false;
            //dc.EditFirma((ulong)dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[0].Value,  data2, mpalety, firma, uwagi");
            dc.EditWydarzenieMP((ulong)dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[0].Value, mpalety);
            dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[1].Value = firma;
            dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[2].Value = data2;
            dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[5].Value = uwagi;
        }

        public void deleteSmallWindow()
        {
            wmv = null;
        }

        public void refreshDC()
        {
            dc = new DataC();
        }

        /*public void refreshData()
        {
            this.dataGridView1.Refresh();
        }*/

        private void button1_Click(object sender, EventArgs e)
        {
            if (wmv == null)
            {
                wmv = new EventMV(this, dc);
                wmv.Text = "Dodaj";
                wmv.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (wmv == null && dataGridView1.Rows.Count != 0)
            {
                wmv = new EventMV(this, dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[0].Value.ToString(), dc);
                wmv.Text = "Edytuj";
                wmv.Show();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (wmv == null && dataGridView1.Rows.Count != 0)
            {
                DialogResult dr = MessageBox.Show("Na pewno usunąć ?", "Pytanie", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    zapisano = false;
                    //BindingSource w = new BindingSource();
                    //dataGridView1.DataSource = w;
                    dc.DeleteWydarzenie((ulong)dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[0].Value);
                    dataGridView1.Rows.RemoveAt(dataGridView1.CurrentCellAddress.Y);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            zapisano = true;
            dc.SaveData();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
