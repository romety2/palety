using Pallets.Controllers;
using Pallets.Models;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Pallets.Views
{
    public partial class PaletteVV : Form
    {
        private PaletteV pv;
        private DataC dc;

        public PaletteVV()
        {
            InitializeComponent();
        }

        public PaletteVV(Data data)
        {
            this.dc = new DataC(data);
            InitializeComponent();
        }

        public PaletteVV(PaletteV pv)
        {
            this.pv = pv;
            pv.Enabled = false;
            InitializeComponent();
        }

        public PaletteVV(Data data, PaletteV pv)
        {
            this.pv = pv;
            pv.Enabled = false;
            this.dc = new DataC(data);
            InitializeComponent();
        }

        public PaletteVV(ulong id, Data data, PaletteV pv)
        {
            this.pv = pv;
            pv.Enabled = false;
            this.dc = new DataC(data);
            Palette p = data.Pallets.First(o => o.Id == id);
            InitializeComponent();
            getViews(p);
        }

        private void getViews(Palette p)
        {
            int maxWidth, maxHeight = 0, temp;
            temp = label4.Height;
            label4.Text = p.Name;
            maxHeight += label4.Height - temp;
            if (maxHeight != 0)
            {
                label2.Location = new Point(label2.Location.X, label2.Location.Y + maxHeight);
                label5.Location = new Point(label5.Location.X, label5.Location.Y + maxHeight);
            }
            temp = label5.Height;
            label5.Text = p.Description;
            maxHeight += label5.Height - temp;
            if (maxHeight != 0)
            {
                label3.Location = new Point(label3.Location.X, label3.Location.Y + maxHeight);
                label6.Location = new Point(label6.Location.X, label6.Location.Y + maxHeight);
            }
            if (label4.Size.Width < label5.Size.Width)
                maxWidth = label5.Size.Width;
            else
                maxWidth = label4.Size.Width;
            temp = label6.Height;
            label6.Text = p.Quantity.ToString();
            if (maxWidth < label6.Size.Width)
                maxWidth = label6.Size.Width;
            maxHeight += label6.Height - temp;
            this.Size = new Size(this.Size.Width + maxWidth, this.Size.Height + maxHeight);
        }

        private void PaletteVV_FormClosing(object sender, FormClosingEventArgs e)
        {
            pv.Enabled = true;
        }
    }
}
