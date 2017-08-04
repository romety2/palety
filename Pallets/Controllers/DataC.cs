using System;
using System.Linq;
using Pallets.Models;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.ComponentModel;

namespace Pallets.Controllers
{
    public class DataC
    {
        private ulong idC = 1;
        private ulong idP = 1;
        private ulong idE = 1;

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
                if (data.Companies.Count != 0)
                    idC = data.Companies[data.Companies.Count - 1].Id + 1;
                if (data.Pallets.Count != 0)
                    idP = data.Pallets[data.Pallets.Count - 1].Id + 1;
                if (data.Events.Count != 0)
                    idE = data.Events[data.Events.Count - 1].Id + 1;
            }
        }

        public DataC(Data data)
        {
            this.data = data;
            if (data.Companies.Count != 0)
                idC = data.Companies[data.Companies.Count - 1].Id + 1;
            if (data.Pallets.Count != 0)
                idP = data.Pallets[data.Pallets.Count - 1].Id + 1;
            if (data.Events.Count != 0)
                idE = data.Events[data.Events.Count - 1].Id + 1;
        }

        public DataC(BindingList<Event> events)
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
                if (data.Companies.Count != 0)
                    idC = data.Companies[data.Companies.Count - 1].Id + 1;
                if (data.Pallets.Count != 0)
                    idP = data.Pallets[data.Pallets.Count - 1].Id + 1;
            }
            data.Events = events;
        }

        public BindingList<Company> addCompany(string name)
        {
            data.Companies.Add(new Company(idC, name));
            idC++;
            return getData().Companies;
        }

        public BindingList<Palette> addPalette(string name, int quantity)
        {
            Palette palette = new Palette(idP, name, quantity);
            data.Pallets.Add(palette);
            data.Events.ToLookup(w => addEventMP(w.Id, palette));
            idP++;
            return getData().Pallets;
        }

        public BindingList<Event> addEvent(string date, BindingList<Event.MyPalette> mpallets, Company company, string coment)
        {
            data.Events.Add(new Event(idE, date, mpallets, company, coment));
            idE++;
            return getData().Events;
        }


        public BindingList<Event> addEventMP(ulong id, Palette palette)
        {
            data.Events[data.Events.IndexOf(data.Events.First(o => o.Id == id))].MPalette.Add(new Event.MyPalette(palette, 0, 0));
            return getData().Events;
        }

        public BindingList<Company> editCompany(ulong id, string name)
        {
            data.Companies[data.Companies.IndexOf(data.Companies.First(o => o.Id == id))].Name = name;
            return getData().Companies;
        }

        public BindingList<Palette> editPalette(ulong id, string name, int quantity)
        {
            data.Pallets[data.Pallets.IndexOf(data.Pallets.First(o => o.Id == id))].Name = name;
            data.Pallets[data.Pallets.IndexOf(data.Pallets.First(o => o.Id == id))].Quantity = quantity;
            return getData().Pallets;
        }

        public BindingList<Event> editEvent(ulong id, string data2, BindingList<Event.MyPalette> mpallets, Company company, string comment)
        {
            data.Events[data.Events.IndexOf(data.Events.First(o => o.Id == id))].Date = data2;
            data.Events[data.Events.IndexOf(data.Events.First(o => o.Id == id))].MPalette = mpallets;
            data.Events[data.Events.IndexOf(data.Events.First(o => o.Id == id))].Company = company;
            data.Events[data.Events.IndexOf(data.Events.First(o => o.Id == id))].Comment = comment;
            return getData().Events;
        }

        public BindingList<Event> editEventMP(ulong id, BindingList<Event.MyPalette> mpallets)
        {
            data.Events[data.Events.IndexOf(data.Events.First(o => o.Id == id))].MPalette = mpallets;
            return getData().Events;
        }

        public BindingList<Company> deleteCompany(ulong id)
        {
            data.Events.Remove(data.Events.Single(ev => ev.Company.Id == id));
            data.Companies.Remove(data.Companies.First(o => o.Id == id));
            return getData().Companies;
        }

        public BindingList<Palette> deletePalette(ulong id)
        {
            Palette palette = data.Pallets.First(o => o.Id == id);
            data.Events.ToLookup(ev => ev.MPalette.Remove(ev.MPalette.First(p => p.Palette.Id == palette.Id)));
            data.Pallets.Remove(palette);
            return getData().Pallets;
        }

        public BindingList<Event> deleteEvent(ulong id)
        {
            data.Events.Remove(data.Events.First(o => o.Id == id));
            return getData().Events;
        }

        public void saveData()
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

        public Data refreshData()
        {
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
            return data;
        }
    }
}
