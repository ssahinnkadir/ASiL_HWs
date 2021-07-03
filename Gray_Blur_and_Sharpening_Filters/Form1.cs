using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASiL_HW1_Kadir_SAHIN
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();   //Resim seçmek için bir OpenFileDialog açıyoruz.
            file.Filter = "Resim dosyası  | *.jpg";
            file.Title = "Bir Resim Seçiniz";
            file.ShowDialog();
            string file_path = file.FileName;           
            pictureBox1.ImageLocation = file_path;       //seçilen resmin yolunu pictureBox'a gösteriyoruz.
        }

        private void Filtreler_SelectedIndexChanged(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Image);     // filtrelerde resmin orjinalini de korumamız gerekiyor.
            int width = bmp.Width;                      // resmin en ve boy değerlerini aldık
            int height = bmp.Height;
            int i, j;             
            Color px;                           

            if (Filtreler.SelectedIndex==0)
            {
                int avg;        // gray filtresinde her pikselin 3 bileşeninin ortalamasını alıp tüm bileşenlere bunu
                                // koyarsak o pikselin gray karşılığını bulabiliriz.
                for (j = 0; j < height; j++)
                {
                    for (i = 0; i < width; i++)
                    {
                        px = bmp.GetPixel(i, j);
                        avg = (px.R + px.G + px.B) / 3;
                        px = Color.FromArgb(px.A, avg, avg, avg);
                        bmp.SetPixel(i, j, px); // oluşturulan her yeni pikseli yeni açtığımız image a işliyoruz.
                    }
                }
                pictureBox2.Image = bmp; // oluşan yeni resim pictureBox2'de gösteriliyor.
            }

            if (Filtreler.SelectedIndex == 1)
            {
                Bitmap blurred = new Bitmap(pictureBox1.Image);

                for (j = 1; j < height-1; j++)
                {
                    for (i = 1; i < width-1; i++)
                    {
                        Color o, top, bottom, left, right;
                        int R, G, B;

                        // Resimde Blur etkisi oluşturabilmek için her pikselin belli bir sayıdaki komşu piksellerinin
                        // değerleri ortalaması kullanılabilir. Biz kendisi dahil 5 farklı pikselin ortalamasını aldık.

                        o = bmp.GetPixel(i,j);
                        top= bmp.GetPixel(i-1,j);
                        bottom = bmp.GetPixel(i+1,j);
                        left = bmp.GetPixel(i,j+1);
                        right = bmp.GetPixel(i,j+1);

                        R = (int)((o.R + left.R + right.R + top.R + bottom.R) / 5);
                        G = (int)((o.G + left.G + right.G + top.G + bottom.G) / 5);
                        B = (int)((o.B + left.B + right.B + top.B + bottom.B) / 5);

                        px = Color.FromArgb(R, G, B);
                        blurred.SetPixel(i, j, px);

                    }
                }
                pictureBox2.Image = blurred;
            }
            if (Filtreler.SelectedIndex == 2)
            {
                Bitmap sharpened = new Bitmap(pictureBox1.Image);

                for (j = 1; j < height - 1; j++)
                {
                    for (i = 1; i < width - 1; i++)
                    {
                        Color o, top, bottom, left, right;
                        int R, G, B, sR, sG, sB;

                        //Sharpening filtresinde, Blur filtresindeki mantığın tersini uygulayabiliriz.
                        // Yani her pikselden, Blur etkisi oluşturan değeri çıkartırsak elimizde net kısımların maskesi kalmış olur.
                        // Sonra bu netlik oluşturan maske ile orijinal resmi çakıştırırsak resmimiz netleşmiş olur.
                        o = bmp.GetPixel(i, j);
                        top = bmp.GetPixel(i - 1, j);
                        bottom = bmp.GetPixel(i + 1, j);
                        left = bmp.GetPixel(i, j + 1);
                        right = bmp.GetPixel(i, j + 1);

                        R = (int)((o.R + left.R + right.R + top.R + bottom.R) / 5);
                        G = (int)((o.G + left.G + right.G + top.G + bottom.G) / 5);
                        B = (int)((o.B + left.B + right.B + top.B + bottom.B) / 5);

                        sR = o.R - R;
                        sG = o.G - G;
                        sB = o.B - B;

                        // Üstte oluşan sR, sG ve sB değerleri, maskenin değerleridir.

                        

                        sR = o.R + 2*sR;
                        sG = o.G + 2*sG;
                        sB = o.B + 2*sB;

                        // Üstteki adımda ise pikselin orijinal hali ile maskedeki değer çakıştırılmıştır böylece 
                        // bulanıklık giderilmiştir.

                        if (sR > 255)
                            sR = 255;
                        if (sR < 0)
                            sR = 0;
                        if (sG > 255)
                            sG = 255;
                        if (sG < 0)
                            sG = 0;
                        if (sB > 255)
                            sB = 255;
                        if (sB < 0)
                            sB = 0;

                        // Yukaruda 255'i aşan ve 0'ın altına düşen değerler sınırlara limitleniyor.

                        px = Color.FromArgb(sR,sG,sB );
                        sharpened.SetPixel(i, j, px);
                        
                    }
                }
                pictureBox2.Image = sharpened;
            }
            if (Filtreler.SelectedIndex == 3)
            {
                Bitmap invert = new Bitmap(pictureBox1.Image);
                for (j = 1; j < height - 1; j++)
                {
                    for (i = 1; i < width - 1; i++)
                    {
                        px = bmp.GetPixel(i, j);
                        px = Color.FromArgb((byte)~px.R,(byte)~px.G, (byte)~px.B);

                        // Üstteki işlemde doğrudan yeni oluşturulacak pikselde orjinal pikseldeki bileşenin
                        // değilinin alınması yeterlidir.

                        invert.SetPixel(i, j, px);
      
                    }
                }
                pictureBox2.Image = invert;
            }

            // Filtrelerde belli bir orana göre uygulama olayına zaman ayıramadım ama,
            
            // gray filtresi için her pikselin  bileşenlerinin average ile orjinalleri arasında yüzdeye göre bi değer
            // bulma yaklaşımı uygulanabilir.
            
            //blur filtresi için bakılan komşu piksellerin sınırları genişletilebilir.

            //sharpening için yukarıda kullandığım netlik maskesini çarptığımız değer olan 2'nin yerine daha düşük
            // ya da daha yüksek değerler koyulabilir.
        }
    }
}


