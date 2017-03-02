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
        private BindingList<Company> firmy;
        private BindingList<Palette> palety;
        private BindingList<Event> wydarzenia;

        public Data()
        {
            firmy = new BindingList<Company>();
            palety = new BindingList<Palette>();
            wydarzenia = new BindingList<Event>();
        }

        public BindingList<Company> Firmy
        {
            get { return firmy; }
            set { firmy = value; }
        }

        public BindingList<Palette> Palety
        {
            get { return palety; }
            set { palety = value; }
        }

        public BindingList<Event> Wydarzenia
        {
            get { return wydarzenia; }
            set { wydarzenia = value; }
        }
    }
}
