using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
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
        public bool saved;

        public App()
        {
            dc = new DataC();
            InitializeComponent();
            saved = true;
        }

        private BindingList<Event> getEvents()
        {
            data = dc.getData();
            return data.Events;
        }

        private void getViews()
        {
            MaximizeBox = false;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].HeaderText = "Data";
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
            dataGridView1.Columns[5].HeaderText = "Uwagi";
            dataGridView1.AllowUserToAddRows = false;
        }

        private void searchEvents()
        {
            BindingSource events = new BindingSource();
            events.DataSource = getEvents()
                .Where(o => DateTime.Compare(DateTime.Parse(o.Date), dateTimePicker1.Value.Date) >= 0
                && DateTime.Compare(DateTime.Parse(o.Date), dateTimePicker2.Value.Date) <= 0
                && o.Company.Name.ToLower().Contains(textBox1.Text.ToLower()) == true)
                .OrderBy(o => DateTime.Parse(o.Date))
                .ThenBy(t => t.Company.Name).ToList();
            dataGridView1.DataSource = events;
            dateTimePicker1.MaxDate = dateTimePicker2.Value.Date;
            dateTimePicker2.MinDate = dateTimePicker1.Value.Date;
        }

        private void checkViewActualEvent()
        {
            if (dataGridView1.Rows.Count == 0)
            {
                label6.Text = "";
                label7.Text = "";
                button6.Enabled = false;
            }
            else
                button6.Enabled = true;
        }

        private void Management_Load(object sender, EventArgs e)
        {
            searchEvents();
            getViews();
            checkViewActualEvent();
        }

        private void companyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!saved)
            {
                DialogResult dr = MessageBox.Show("Zapisać zmiany?", "Pytanie", MessageBoxButtons.YesNoCancel);
                if (dr != DialogResult.Cancel)
                {
                    if (dr == DialogResult.Yes)
                    {
                        saved = true;
                        dc.saveData();
                    }
                    else
                    {
                        refreshData();
                        saved = true;
                        searchEvents();
                        checkViewActualEvent();
                        dataGridView1.Refresh();
                    }
                    CompanyV name = new CompanyV(data, this);
                    name.Show();
                }
            }
            else
            {
                CompanyV name = new CompanyV(data, this);
                name.Show();
            }
        }

        private void paletteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!saved)
            {
                DialogResult dr = MessageBox.Show("Zapisać zmiany?", "Pytanie", MessageBoxButtons.YesNoCancel);
                if (dr != DialogResult.Cancel)
                {
                    if (dr == DialogResult.Yes)
                    {
                        saved = true;
                        dc.saveData();
                    }
                    else
                    {
                        refreshData();
                        saved = true;
                        searchEvents();
                        checkViewActualEvent();
                        dataGridView1.Refresh();
                    }
                    PaletteV name = new PaletteV(data, this);
                    name.Show();
                }
            }
            else
            {
                PaletteV name = new PaletteV(data, this);
                name.Show();
            }
        }

        public void addData(string data2, BindingList<Event.MyPalette> mpallets, Company company, string comment)
        {
            int id;
            if (dataGridView1.Rows.Count != 0 && String.Compare(data2, dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[2].Value.ToString(), true) < 0)
                id = dataGridView1.CurrentCellAddress.Y + 1;
            else
                id = dataGridView1.CurrentCellAddress.Y;
            saved = false;
            label8.Text = "Dodano " + data2 + " (" + company.Name + ")!";
            BindingSource w = new BindingSource();
            w.DataSource = dc.addEvent(data2, mpallets, company, comment)
                .Where(o => DateTime.Compare(DateTime.Parse(o.Date), dateTimePicker1.Value.Date) >= 0
                && DateTime.Compare(DateTime.Parse(o.Date), dateTimePicker2.Value.Date) <= 0
                && o.Company.Name.ToLower().Contains(textBox1.Text.ToLower()) == true)
                .OrderBy(o => DateTime.Parse(o.Date))
                .ThenBy(t => t.Company.Name).ToList();
            dataGridView1.DataSource = w;
            if (dataGridView1.Rows.Count != 1 && textBox1.Text == "")
                dataGridView1.CurrentCell = dataGridView1[2, id];
            else
                textBox1.Text = "";
            checkViewActualEvent();
        }

        public void editData(string data2, BindingList<Event.MyPalette> mpallets, Company company, string comment)
        {
            saved = false;
            label8.Text = "Zmieniono " + data2 + " (" + company.Name + ")!";
            //dc.EditFirma((ulong)dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[0].Value,  data2, mpallets, company, comment");
            dc.editEventMP((ulong)dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[0].Value, mpallets);
            dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[1].Value = company;
            dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[2].Value = data2;
            dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[5].Value = comment;
            checkViewActualEvent();
        }

        public void deleteSmallWindow()
        {
            wmv = null;
        }

        public void refreshData()
        {
            data = dc.refreshData();
        }

        public void refreshDataGridView()
        {
            searchEvents();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (wmv == null)
            {
                wmv = new EventMV(data, this);
                wmv.Text = "Dodaj";
                wmv.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (wmv == null && dataGridView1.Rows.Count != 0)
            {
                wmv = new EventMV((ulong)dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[0].Value, data, this);
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
                    label8.Text = "Usunięto " + dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[2].Value
                        + " (" + dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[3].Value + ")!";
                    saved = false;
                    //BindingSource w = new BindingSource();
                    //dataGridView1.DataSource = w;
                    dc.deleteEvent((ulong)dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[0].Value);
                    dataGridView1.Rows.RemoveAt(dataGridView1.CurrentCellAddress.Y);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            saved = true;
            dc.saveData();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            EventVV name = new EventVV((ulong)dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[0].Value, data, this);
            name.Show();
        }

        private void App_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!saved)
            {
                DialogResult dr = MessageBox.Show("Zapisać zmiany?", "Pytanie", MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Yes)
                {
                    saved = true;
                    dc.saveData();
                }
                else if (dr == DialogResult.Cancel)
                    e.Cancel = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            searchEvents();
            checkViewActualEvent();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            searchEvents();
            checkViewActualEvent();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            searchEvents();
            checkViewActualEvent();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count != 0)
            {
                label6.Text = dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[2].Value.ToString();
                label7.Text = dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[3].Value.ToString();
            }
        }
    }
}
