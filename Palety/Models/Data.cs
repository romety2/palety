using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palety.Models
{
    [Serializable]
    public class Data
    {
        private BindingList<Firma> firmy;
        private BindingList<Paleta> palety;
        private BindingList<Wydarzenie> wydarzenia;

        public Data()
        {
            firmy = new BindingList<Firma>();
            palety = new BindingList<Paleta>();
            wydarzenia = new BindingList<Wydarzenie>();
        }

        public BindingList<Firma> Firmy
        {
            get { return firmy; }
            set { firmy = value; }
        }

        public BindingList<Paleta> Palety
        {
            get { return palety; }
            set { palety = value; }
        }

        public BindingList<Wydarzenie> Wydarzenia
        {
            get { return wydarzenia; }
            set { wydarzenia = value; }
        }
    }
}
