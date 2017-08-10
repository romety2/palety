using System;

namespace Pallets.Models
{
    [Serializable]
    public class Palette
    {
        public Palette()
        {

        }

        public Palette(ulong id, string name, int quantity, string description)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            Description = description;
        }

        public ulong Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
    }
}
