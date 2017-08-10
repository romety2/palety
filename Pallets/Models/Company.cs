using System;

namespace Pallets.Models
{
    [Serializable]
    public class Company
    {
        public Company()
        {

        }

        public Company(ulong id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public ulong Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
