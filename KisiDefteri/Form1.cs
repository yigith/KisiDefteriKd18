using System.Security.Cryptography;
using System.Text.Json;

namespace KisiDefteri
{
    public partial class Form1 : Form
    {
        List<Kisi> kisiler;

        public Form1()
        {
            InitializeComponent();
            VerileriOku();
            KisileriListele();
        }

        private void VerileriOku()
        {
            try
            {
                string json = File.ReadAllText("veri.json");
                kisiler = JsonSerializer.Deserialize<List<Kisi>>(json);
            }
            catch (Exception)
            {
                // Okurken bir hata olmas� durumunda
                OrnekVerileriYukle();
            }
        }

        private void KisileriListele()
        {
            lstKisiler.Items.Clear();

            foreach (Kisi kisi in kisiler)
                lstKisiler.Items.Add(kisi);
        }

        private void OrnekVerileriYukle()
        {
            kisiler = new List<Kisi>()
            {
                new Kisi() { Ad = "Ali", Soyad = "Y�lmaz" },
                new Kisi() { Ad = "Can", Soyad = "�zt�rk" },
                new Kisi() { Ad = "Cem", Soyad = "�ahin" },
                new Kisi() { Ad = "Ece", Soyad = "Do�an" }
            };
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            string ad = txtAd.Text.Trim();
            string soyad = txtSoyad.Text.Trim();

            if (ad == "" || soyad == "")
            {
                MessageBox.Show("Ad ve soyad alanlar� zorunlu!");
                return;
            }

            Kisi yeniKisi = new Kisi() { Ad = ad, Soyad = soyad };
            kisiler.Add(yeniKisi);
            KisileriListele();
            lstKisiler.SelectedItem = yeniKisi;
            txtAd.Clear();
            txtSoyad.Clear();
            txtAd.Focus();
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            int sid = lstKisiler.SelectedIndex;
            if (sid == -1)
            {
                MessageBox.Show("Silmek i�in �nce bir ki�i se�iniz.");
                return;
            }

            DialogResult cevap = MessageBox.Show("Silmek istedi�inize emin misiniz?", "Silme Onay�", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            if (cevap == DialogResult.Yes)
            {
                kisiler.RemoveAt(sid);
                KisileriListele();
                lstKisiler.SelectedIndex = Math.Min(sid, lstKisiler.Items.Count - 1);
            }
        }

        private void btnDuzenle_Click(object sender, EventArgs e)
        {
            if (lstKisiler.SelectedItem == null)
            {
                MessageBox.Show("D�zenlemek i�in �nce bir ki�i se�iniz.");
                return;
            }

            Kisi kisi = (Kisi)lstKisiler.SelectedItem;
            DuzenleForm frmDuzenle = new DuzenleForm(kisi);
            frmDuzenle.ShowDialog();
            KisileriListele();
            lstKisiler.SelectedItem = kisi;
        }
        private void SeciliKisiyiTasi(int yeniIndeks)
        {
            Kisi seciliKisi = (Kisi)lstKisiler.SelectedItem;
            kisiler.Remove(seciliKisi);
            kisiler.Insert(yeniIndeks, seciliKisi);
            KisileriListele();
            lstKisiler.SelectedIndex = yeniIndeks;
        }

        private void btnYukari_Click(object sender, EventArgs e)
        {
            int sid = lstKisiler.SelectedIndex;
            if (sid < 1) return;
            SeciliKisiyiTasi(sid - 1);
        }


        private void btnAsagi_Click(object sender, EventArgs e)
        {
            int sid = lstKisiler.SelectedIndex;
            if (sid < 0 || sid == kisiler.Count - 1) return;
            SeciliKisiyiTasi(sid + 1);
        }

        private void lstKisiler_KeyDown(object sender, KeyEventArgs e)
        {
            if (lstKisiler.SelectedIndex > -1 && e.KeyCode == Keys.Delete)
                btnSil.PerformClick();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // kisiler listesini serilize et ve kaydet
            string json = JsonSerializer.Serialize(kisiler);
            File.WriteAllText("veri.json", json);
        }
    }
}