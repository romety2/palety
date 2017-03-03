using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Pallets.Controllers;
using Pallets.Models;

namespace Pallets.Views
{
    public partial class CompanyV : Form
    {
        private App app;
        private DataC dc;
        private Data data;
        private CompanyMV fmv;
        public bool saved;

        public CompanyV()
        {
            dc = new DataC();
            InitializeComponent();
            saved = true;
        }

        public CompanyV(Data data)
        {
            dc = new DataC(data.Events);
            InitializeComponent();
            saved = true;
        }

        public CompanyV(App app)
        {
            dc = new DataC();
            this.app = app;
            InitializeComponent();
            saved = true;
        }

        public CompanyV(Data data, App app)
        {
            dc = new DataC(data.Events);
            this.app = app;
            InitializeComponent();
            saved = true;
        }

        private BindingList<Company> getCompanies()
        {
            data = dc.getData();
            return data.Companies;
        }

        private void getView()
        {
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "Firma";
            dataGridView1.Columns[1].Width = 250;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
        }

        public void addData(string text)
        {
            int id;
            if (dataGridView1.Rows.Count != 0 && String.Compare(text, dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[1].Value.ToString(), true) < 0)
                id = dataGridView1.CurrentCellAddress.Y + 1;
            else
                id = dataGridView1.CurrentCellAddress.Y;
            label4.Text = "Dodano firmę " + text + "!";
            saved = false;
            BindingSource firmy = new BindingSource();
            firmy.DataSource = dc.addCompany(text).OrderBy(o => o.Name);
            dataGridView1.DataSource = firmy;
            if (dataGridView1.Rows.Count != 1 && textBox1.Text == "")
                dataGridView1.CurrentCell = dataGridView1[1, id];
            else
                textBox1.Text = "";

        }

        public void editData(string text)
        {
            label4.Text = "Zmieniono nazwę firmy z " + dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[1].Value + " na " + text + "!";
            label3.Text = text;
            saved = false;
            //dc.EditFirma((ulong)dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[0].Value, textBox1.Text);
            dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[1].Value = text;
        }

        public void deleteSmallWindow()
        {
            fmv = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (fmv == null)
            {
                fmv = new CompanyMV(this);
                fmv.Text = "Dodaj firmę";
                fmv.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (fmv == null && dataGridView1.Rows.Count != 0)
            {
                fmv = new CompanyMV(this, dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[1].Value.ToString());
                fmv.Text = "Zmień nazwę firmy";
                fmv.Show();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (fmv == null && dataGridView1.Rows.Count != 0)
            {
                DialogResult dr = MessageBox.Show("Na pewno usunąć " + dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[1].Value + "?", "Pytanie", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    label4.Text = "Usunięto firmę " + dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[1].Value + "!";
                    saved = false;
                    //BindingSource companies = new BindingSource();
                    //dataGridView1.DataSource = companies;
                    dc.deleteCompany((ulong)dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[0].Value);
                    dataGridView1.Rows.RemoveAt(dataGridView1.CurrentCellAddress.Y);
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label4.Text = "Zapisano zmiany!";
            saved = true;
            dc.saveData();
            app.refreshDC();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CompanyV_Load(object sender, EventArgs e)
        {
            BindingSource companies = new BindingSource();
            companies.DataSource = getCompanies().OrderBy(o => o.Name).ToList();
            dataGridView1.DataSource = companies;
            getView();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if(dataGridView1.Rows.Count != 0)
            {
                label3.Text = dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y].Cells[1].Value.ToString();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = getCompanies().OrderBy(o => o.Name).Where(o => o.Name.ToLower().Contains(textBox1.Text.ToLower()) == true).ToList();
            dataGridView1.DataSource = bs;
        }

        private void CompanyV_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!saved)
            {
                DialogResult dr = MessageBox.Show("Zapisać zmiany?", "Pytanie", MessageBoxButtons.YesNoCancel);
                if (dr == DialogResult.Yes)
                {
                    saved = true;
                    dc.saveData();
                    app.refreshDC();
                }
                else if (dr == DialogResult.Cancel)
                    e.Cancel = true;
                if (dr != DialogResult.Cancel)
                {
                    if (fmv != null)
                        fmv.Close();
                }
            }
            else if (fmv != null)
                fmv.Close();
        }
    }
}
