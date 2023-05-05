using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace AracListesi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            if (File.Exists(temp))
            {
                string jsondata = File.ReadAllText(temp);
                araclar = JsonSerializer.Deserialize<List<Arac>>(jsondata);
            }

            

            ShowList();
        }

        List<Arac> araclar = new List<Arac>()
        {
            new Arac()
            {
                Plaka = "34 KCK 24",
                Marka = "FORD",
                Model = "FOCUS",
                Yakıt = "Benzin",
                Renk = "Siyah",
                Vites = "Düz",
                KasaTipi = "SUV",
                Açıklama = "4x4",
                AraçYılı = new DateTime(2012),
            },
            new Arac() 
            {
                Plaka = "67 TVS 95",
                Marka = "FORD",
                Model = "Mustang",
                Yakıt = "Benzin",
                Renk = "Kırmızı",
                Vites = "Düz",
                KasaTipi = "Sport",
                Açıklama = "600 HP",
                AraçYılı = new DateTime(2012),
            }
        };
        
        public void ShowList()
        {
            listView1.Items.Clear();
            foreach(Arac arac in araclar) 
            {
                AddAracToList(arac);
            }
        }

        public void AddAracToList(Arac arac)
        {
            ListViewItem item =  new ListViewItem(new string[]
                    {
                        arac.Plaka,
                        arac.Marka,
                        arac.Model,
                        arac.Yakıt,
                        arac.Renk,
                        arac.Vites,
                        arac.KasaTipi,
                        arac.Açıklama,
                        arac.AraçYılı.ToShortDateString(),
                    });
            item.Tag = arac;
            listView1.Items.Add(item);

                    
        }

        void EditAracOnList(ListViewItem aItem, Arac arac)
        {
            aItem.SubItems[0].Text = arac.Plaka;
            aItem.SubItems[1].Text = arac.Marka;
            aItem.SubItems[2].Text = arac.Model;
            aItem.SubItems[3].Text = arac.Yakıt;
            aItem.SubItems[4].Text = arac.Renk;
            aItem.SubItems[5].Text = arac.Vites;
            aItem.SubItems[6].Text = arac.KasaTipi;
            aItem.SubItems[7].Text = arac.Açıklama;
            aItem.SubItems[8].Text = arac.AraçYılı.ToShortDateString();

            aItem.Tag = arac;

        }

        private void AracEkle(object sender, EventArgs e)
        {
            AracFrm frm= new AracFrm() 
            {
                Text="Araç Ekle",
                StartPosition = FormStartPosition.CenterParent,
                arac = new Arac()
            };

            if(frm.ShowDialog() == DialogResult.OK )
            { 
                araclar.Add(frm.arac);
                AddAracToList(frm.arac);
            }
        }

        private void AracDuzenle(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            ListViewItem aItem = listView1.SelectedItems[0];
            Arac secili=aItem.Tag as Arac;

            AracFrm frm = new AracFrm()
            {
                Text = "Araç Düzenle",
                StartPosition = FormStartPosition.CenterParent,
                arac = Clone(secili),
            };

            if (frm.ShowDialog() == DialogResult.OK)
            {
                secili = frm.arac;
                EditAracOnList(aItem, secili);
            }


        }

        Arac Clone(Arac arac)
        {
            return new Arac()
            {
                id= arac.id,
                Plaka = arac.Plaka,
                Marka = arac.Marka,
                Model = arac.Model,
                Yakıt = arac.Yakıt,
                Renk = arac.Renk,
                Vites = arac.Vites,
                KasaTipi = arac.KasaTipi,
                Açıklama = arac.Açıklama,
                AraçYılı = arac.AraçYılı,

            };
        }

        private void AracDelete(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;

            ListViewItem aItem = listView1.SelectedItems[0];
            Arac secili = aItem.Tag as Arac;

           var sonuc= MessageBox.Show($"Seçili Araç Silinsin Mi?\n\n{secili.Marka} {secili.Model}",
                "Silmeyi Onaylıyor Musunuz",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if(sonuc == DialogResult.Yes)
            {
                araclar.Remove(secili);
                listView1.Items.Remove(aItem);
                
            }
                
        }

        private void Kaydet(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog()
            {
                Filter = "Json Dosyaları (*.json)|*.json|XML Dosyaları (*.xml)|*.xml",
            };

            if (sf.ShowDialog() == DialogResult.OK)
            {
                if (sf.FileName.EndsWith("json"))
                {
                    string data = JsonSerializer.Serialize(araclar);
                    File.WriteAllText(sf.FileName, data);
                }
                else if (sf.FileName.EndsWith("xml"))
                {
                    StreamWriter sw = new StreamWriter(sf.FileName);
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Arac>));
                    serializer.Serialize(sw, araclar);
                    sw.Close();

                }
            }

        }

        private void AraçYükle(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog()
            {
                Filter = "Json, XML Formatları (*.json)|*.json|XML Dosyaları (*.xml)|*.xml",
            };

            if (of.ShowDialog() == DialogResult.OK)
            {
                if(of.FileName.ToLower().EndsWith("json"))
                {
                    string jsondata = File.ReadAllText(of.FileName);
                    araclar = JsonSerializer.Deserialize<List<Arac>>(jsondata);  
                }
                else if (of.FileName.ToLower().EndsWith("xml"))
                {
                    StreamReader sr = new StreamReader(of.FileName);
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Arac>));
                    araclar = (List<Arac>)serializer.Deserialize(sr);
                    sr.Close();
                }
                ShowList();
            }
        }


        string temp = Path.Combine(Application.CommonAppDataPath, "data");
        protected override void OnClosing(CancelEventArgs e)
        {

            string data = JsonSerializer.Serialize(araclar);
            File.WriteAllText(temp, data);



            base.OnClosing(e);
        }

        private void toolStripSplitButton2_ButtonClick(object sender, EventArgs e)
        {
            new AboutBox1().ShowDialog();
        }
    }

    [Serializable]
    public class Arac
    {
        public string id;
        public string ID
        {
            get
            {
                if (id == null)
                    id = Guid.NewGuid().ToString();
                return id;
            }
            set { id = value; }

        }
        public string Plaka { get; set; }

        public string Marka { get; set; }

        public string Model { get; set; }

        public string Yakıt { get; set; }

        public string Renk { get; set; }

        public string Vites { get; set; }

        public string KasaTipi { get; set; }

        public string Açıklama { get; set; }

        public DateTime AraçYılı { get; set; }
    }
} 
