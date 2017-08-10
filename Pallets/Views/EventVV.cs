using Pallets.Controllers;
using Pallets.Models;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Pallets.Views
{
    public partial class EventVV : Form
    {
        private App app;
        private DataC dc;

        public EventVV()
        {
            InitializeComponent();
        }

        public EventVV(Data data)
        {
            this.dc = new DataC(data);
            InitializeComponent();
        }

        public EventVV(App app)
        {
            this.app = app;
            app.Enabled = false;
            InitializeComponent();
        }

        public EventVV(Data data, App app)
        {
            this.app = app;
            app.Enabled = false;
            this.dc = new DataC(data);
            InitializeComponent();
        }

        public EventVV(ulong id, Data data, App app)
        {
            this.app = app;
            app.Enabled = false;
            this.dc = new DataC(data);
            Event ev = data.Events.First(o => o.Id == id);
            InitializeComponent();
            getViews(ev);
        }

        private void EventVV_FormClosing(object sender, FormClosingEventArgs e)
        {
            app.Enabled = true;
        }

        private void getViews(Event ev)
        {
            int maxWidth, maxHeight = 0, temp;
            temp = label5.Height;
            label5.Text = ev.Date;
            maxHeight += label5.Height - temp;
            if (maxHeight != 0)
            {
                label2.Location = new Point(label2.Location.X, label2.Location.Y + maxHeight);
                label6.Location = new Point(label6.Location.X, label6.Location.Y + maxHeight);
            }
            temp = label6.Height;
            label6.Text = ev.Company.Name;
            maxHeight += label6.Height - temp;
            if (maxHeight != 0)
            {
                label3.Location = new Point(label3.Location.X, label3.Location.Y + maxHeight);
                label7.Location = new Point(label7.Location.X, label7.Location.Y + maxHeight);
            }
            if (label5.Size.Width < label6.Size.Width)
                maxWidth = label6.Size.Width;
            else
                maxWidth = label5.Size.Width;
            temp = label7.Height;
            label7.Text = ev.DisplayPallets;
            maxHeight += label7.Height - temp;
            if (maxHeight != 0)
            {
                label4.Location = new Point(label4.Location.X, label4.Location.Y + maxHeight);
                label8.Location = new Point(label8.Location.X, label8.Location.Y + maxHeight);
            }
            label7.Text = ev.DisplayPallets;
            if (maxWidth < label7.Size.Width)
                maxWidth = label7.Size.Width;
            maxHeight += label7.Height - temp;
            temp = label7.Height;
            label8.Text = ev.Comment;
            maxHeight += label7.Height - temp;
            if (maxWidth < label8.Size.Width)
                maxWidth = label8.Size.Width;
            maxHeight += label8.Height - temp;
            this.Size = new Size (this.Size.Width + maxWidth, this.Size.Height + maxHeight);
        }
    }
}
