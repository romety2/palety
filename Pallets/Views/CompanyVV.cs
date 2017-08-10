using Pallets.Controllers;
using Pallets.Models;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;


namespace Pallets.Views
{
    public partial class CompanyVV : Form
    {
        private CompanyV cv;
        private DataC dc;

        public CompanyVV()
        {
            InitializeComponent();
        }

        public CompanyVV(Data data)
        {
            this.dc = new DataC(data);
            InitializeComponent();
        }

        public CompanyVV(CompanyV cv)
        {
            this.cv = cv;
            cv.Enabled = false;
            InitializeComponent();
        }

        public CompanyVV(Data data, CompanyV cv)
        {
            this.cv = cv;
            cv.Enabled = false;
            this.dc = new DataC(data);
            InitializeComponent();
        }

        public CompanyVV(ulong id, Data data, CompanyV cv)
        {
            this.cv = cv;
            cv.Enabled = false;
            this.dc = new DataC(data);
            Company c = data.Companies.First(o => o.Id == id);
            InitializeComponent();
            getViews(c);
        }

        private void getViews(Company c)
        {
            int maxWidth, maxHeight = 0, temp;
            temp = label3.Height;
            label3.Text = c.Name;
            maxHeight += label3.Height - temp;
            if (maxHeight != 0)
            {
                label2.Location = new Point(label2.Location.X, label2.Location.Y + maxHeight);
                label4.Location = new Point(label4.Location.X, label4.Location.Y + maxHeight);
            }
            temp = label4.Height;
            label4.Text = c.Description;
            if (label3.Size.Width < label4.Size.Width)
                maxWidth = label4.Size.Width;
            else
                maxWidth = label3.Size.Width;
            maxHeight += label4.Height - temp;
            this.Size = new Size(this.Size.Width + maxWidth, this.Size.Height + maxHeight);
        }

        private void CompanyVV_FormClosing(object sender, FormClosingEventArgs e)
        {
            cv.Enabled = true;
        }
    }
}
