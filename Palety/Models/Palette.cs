using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pallets.Models
{
    [Serializable]
    public class Palette
    {
        public Palette()
        {

        }

        public Palette(ulong id, string nazwa, int ilosc)
        {
            Id = id;
            Nazwa = nazwa;
            Ilosc = ilosc;
        }

        public ulong Id { get; set; }
        public string Nazwa { get; set; }
        public int Ilosc { get; set; }
    }
}
