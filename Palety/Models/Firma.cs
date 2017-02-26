using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Palety.Models
{
    [Serializable]
    public class Firma
    {
        public Firma()
        {

        }

        public Firma(ulong id, string nazwa)
        {
            Id = id;
            Nazwa = nazwa;
        }

        public ulong Id { get; set; }
        public string Nazwa { get; set; }
    }
}
