using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pallets.Models
{
    [Serializable]
    public class Company
    {
        public Company()
        {

        }

        public Company(ulong id, string name)
        {
            Id = id;
            Name = name;
        }

        public ulong Id { get; set; }
        public string Name { get; set; }
    }
}
