using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Pallets.Models
{
    [Serializable]
    public class Event
    {
        public Event()
        {

        }

        public Event(ulong id, string date, BindingList<MyPalette> pallets, Company company, string comment)
        {
            Id = id;
            Date = date;
            MPalette = pallets;
            Company = company;
            Comment = comment;
        }

        [Serializable]
        public struct MyPalette
        {
            private Palette palette;
            private int plus;
            private int minus;

            public Palette Palette { get; set; }
            public int Plus { get; set; }
            public int Minus { get; set; }

            public  MyPalette(Palette palette, int plus, int minus) : this()
            {
                Palette = palette;
                Plus = plus;
                Minus = minus;
            }
        }

        ulong id;
        Company company;
        BindingList<MyPalette> pallets;
        string date;
        string comment;

        public ulong Id { get; set; }
        public Company Company { get; set; }
        public string Date { get; set; }
        public string ViewNameCompany
        {
            get {
                    return this.Company.Name;
                }
        }
        public string DisplayPallets
        {
              get   {
                        string p = "";
                        int r, ile = 0;
                        List<MyPalette> mp = pallets.Where(o => o.Plus != 0 || o.Minus != 0).ToList();
                        foreach(MyPalette pm in mp)
                        {
                            ile++;
                            r = pm.Plus - pm.Minus;
                            p += pm.Palette.Name + ": ";
                            if (r > 0)
                                p += "+";
                            p += r.ToString();
                            if (mp.Count != ile)
                                p += ", ";
                        }
                        return p;
                    }
        }
        public string Comment { get; set; }

        public BindingList<MyPalette> MPalette
        {
            get { return pallets; }
            set { pallets = value; }
        }
    }
}
