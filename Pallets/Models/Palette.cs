using System;

namespace Pallets.Models
{
    [Serializable]
    public class Palette
    {
        public Palette()
        {

        }

        public Palette(ulong id, string name, int quantity)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
        }

        public ulong Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
