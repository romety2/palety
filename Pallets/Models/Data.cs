using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pallets.Models
{
    [Serializable]
    public class Data
    {
        private BindingList<Company> companies;
        private BindingList<Palette> pallets;
        private BindingList<Event> events;

        public Data()
        {
            companies = new BindingList<Company>();
            pallets = new BindingList<Palette>();
            events = new BindingList<Event>();
        }

        public BindingList<Company> Companies
        {
            get { return companies; }
            set { companies = value; }
        }

        public BindingList<Palette> Pallets
        {
            get { return pallets; }
            set { pallets = value; }
        }

        public BindingList<Event> Events
        {
            get { return events; }
            set { events = value; }
        }
    }
}
