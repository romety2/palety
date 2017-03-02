using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pallets.Models;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.ComponentModel;

namespace Pallets.Controllers
{
    public class DataC
    {
        private ulong idF = 1;
        private ulong idP = 1;
        private ulong idW = 1;

        Data data;

        public DataC()
        {
            data = new Data();
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = null;
            try
            {
                fs = new FileStream("data.dat", FileMode.Open, FileAccess.Read);
                data = (Data)bf.Deserialize(fs);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Blad {0}", ex.Message);
            }
            finally
            {
                if (fs != null)
                    fs.Close();
                if (data.Firmy.Count != 0)
                    idF = data.Firmy[data.Firmy.Count - 1].Id + 1;
                if (data.Palety.Count != 0)
                    idP = data.Palety[data.Palety.Count - 1].Id + 1;
                if (data.Wydarzenia.Count != 0)
                    idW = data.Wydarzenia[data.Wydarzenia.Count - 1].Id + 1;
            }
        }

        public DataC(Data data)
        {
            this.data = data;
            if (data.Firmy.Count != 0)
                idF = data.Firmy[data.Firmy.Count - 1].Id + 1;
            if (data.Palety.Count != 0)
                idP = data.Palety[data.Palety.Count - 1].Id + 1;
            if (data.Wydarzenia.Count != 0)
                idW = data.Wydarzenia[data.Wydarzenia.Count - 1].Id + 1;
        }

        public BindingList<Company> AddFirma(string nazwa)
        {
            data.Firmy.Add(new Company(idF, nazwa));
            idF++;
            return getData().Firmy;
        }

        public BindingList<Palette> AddPaleta(string nazwa, int ilosc)
        {
            Palette paleta = new Palette(idP, nazwa, ilosc);
            data.Palety.Add(paleta);
            data.Wydarzenia.ToLookup(w => AddWydarzenieMP(w.Id, paleta));
            idP++;
            return getData().Palety;
        }

        public BindingList<Event> AddWydarzenie(string date, BindingList<Event.MojePalety> mpalety, Company firma, string uwagi)
        {
            data.Wydarzenia.Add(new Event(idW, date, mpalety, firma, uwagi));
            idW++;
            return getData().Wydarzenia;
        }


        public BindingList<Event> AddWydarzenieMP(ulong id, Palette paleta)
        {
            data.Wydarzenia[data.Wydarzenia.IndexOf(data.Wydarzenia.First(o => o.Id == id))].MPalety.Add(new Event.MojePalety(paleta, 0, 0));
            return getData().Wydarzenia;
        }

        public BindingList<Company> EditFirma(ulong id, string nazwa)
        {
            data.Firmy[data.Firmy.IndexOf(data.Firmy.First(o => o.Id == id))].Nazwa = nazwa;
            return getData().Firmy;
        }

        public BindingList<Palette> EditPaleta(ulong id, string nazwa, int ilosc)
        {
            data.Palety[data.Palety.IndexOf(data.Palety.First(o => o.Id == id))].Nazwa = nazwa;
            data.Palety[data.Palety.IndexOf(data.Palety.First(o => o.Id == id))].Ilosc = ilosc;
            return getData().Palety;
        }

        public BindingList<Event> EditWydarzenie(ulong id, string data2, BindingList<Event.MojePalety> mpalety, Company firma, string uwagi)
        {
            data.Wydarzenia[data.Wydarzenia.IndexOf(data.Wydarzenia.First(o => o.Id == id))].Data = data2;
            data.Wydarzenia[data.Wydarzenia.IndexOf(data.Wydarzenia.First(o => o.Id == id))].MPalety = mpalety;
            data.Wydarzenia[data.Wydarzenia.IndexOf(data.Wydarzenia.First(o => o.Id == id))].Firma = firma;
            data.Wydarzenia[data.Wydarzenia.IndexOf(data.Wydarzenia.First(o => o.Id == id))].Uwagi = uwagi;
            return getData().Wydarzenia;
        }

        public BindingList<Event> EditWydarzenieMP(ulong id, BindingList<Event.MojePalety> mpalety)
        {
            data.Wydarzenia[data.Wydarzenia.IndexOf(data.Wydarzenia.First(o => o.Id == id))].MPalety = mpalety;
            return getData().Wydarzenia;
        }

        public BindingList<Company> DeleteFirma(ulong id)
        {
            data.Firmy.Remove(data.Firmy.First(o => o.Id == id));
            return getData().Firmy;
        }

        public BindingList<Palette> DeletePaleta(ulong id)
        {
            Palette paleta = data.Palety.First(o => o.Id == id);
            data.Wydarzenia.ToLookup(w => w.MPalety.Remove(w.MPalety.First(p => String.ReferenceEquals(p.Paleta.Nazwa, paleta.Nazwa))));
            data.Palety.Remove(paleta);
            return getData().Palety;
        }

        public BindingList<Event> DeleteWydarzenie(ulong id)
        {
            data.Wydarzenia.Remove(data.Wydarzenia.First(o => o.Id == id));
            return getData().Wydarzenia;
        }

        public void SaveData()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = null;
            try
            {
                fs = new FileStream("data.dat", FileMode.Create, FileAccess.Write, FileShare.None);
                bf.Serialize(fs, data);
            }
            catch (Exception e)
            {
                Console.WriteLine("Blad {0}", e.Message);
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
        }

        public Data getData()
        {
            return data;
        }
    }
}
