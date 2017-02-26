using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palety.Models
{
    [Serializable]
    public class Paleta
    {
        public Paleta()
        {

        }

        public Paleta(ulong id, string nazwa, int ilosc)
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
