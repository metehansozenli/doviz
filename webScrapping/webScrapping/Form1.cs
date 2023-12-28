using System.Threading.Tasks;
using System.Threading;
using HtmlAgilityPack;
using System.Diagnostics;
using System.Text;

namespace webScrapping
{
    public partial class Form1 : Form
    {
        private string dovizXpath = "//div[@class='tableBox srbstPysDvz']//div[@class='tBody']//ul";
        private int rowCount;

        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            Thread thread = new Thread(kurGuncelle);
            thread.Start();
        }
        void kurGuncelle()
        {

            HtmlWeb web = new HtmlWeb();
            web.OverrideEncoding = Encoding.UTF8;
            HtmlAgilityPack.HtmlDocument doc;
            HtmlNodeCollection dovizler;
            while (true)
            {
                doc = web.Load("https://bigpara.hurriyet.com.tr/doviz/");
                dovizler = doc.DocumentNode.SelectNodes(dovizXpath);

                for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                {
                  
                    dataGridView1.Rows[rowIndex].Cells[0].Value = dovizler[rowIndex].SelectSingleNode(".//li[1]//h3[1]//a[1]").InnerText.Replace("&#231;","ç");
                    dataGridView1.Rows[rowIndex].Cells[2].Value = dovizler[rowIndex].SelectSingleNode(".//li[3]").InnerText;
                    dataGridView1.Rows[rowIndex].Cells[3].Value = dovizler[rowIndex].SelectSingleNode(".//li[4]").InnerText;
                    dataGridView1.Rows[rowIndex].Cells[4].Value = dovizler[rowIndex].SelectSingleNode(".//li[5]").InnerText;
                    dataGridView1.Rows[rowIndex].Cells[5].Value = dovizler[rowIndex].SelectSingleNode(".//li[6]").InnerText;
                    if (dovizler[rowIndex].SelectSingleNode(".//li[5]").InnerText[0].Equals('-'))
                    {
                        dataGridView1.Rows[rowIndex].Cells[2].Style.ForeColor = Color.Red;
                        dataGridView1.Rows[rowIndex].Cells[3].Style.ForeColor = Color.Red;


                    }
                    else if (dovizler[rowIndex].SelectSingleNode(".//li[5]").InnerText.Equals("0,00%"))
                    {
                        dataGridView1.Rows[rowIndex].Cells[2].Style.ForeColor = Color.Black;
                        dataGridView1.Rows[rowIndex].Cells[3].Style.ForeColor = Color.Black;
                    }
                    else
                    {
                        dataGridView1.Rows[rowIndex].Cells[2].Style.ForeColor = Color.Green;
                        dataGridView1.Rows[rowIndex].Cells[3].Style.ForeColor = Color.Green;
                    }
                }
            }
        }
        void kurGetir()
        {
            HtmlWeb web = new HtmlWeb();
            web.OverrideEncoding= Encoding.UTF8;
            HtmlAgilityPack.HtmlDocument doc;
            HtmlNodeCollection dovizler;
            doc = web.Load("https://bigpara.hurriyet.com.tr/doviz/");
            dovizler = doc.DocumentNode.SelectNodes(dovizXpath);
            
            int rowIndex=0;
            foreach (var doviz in dovizler)
            {
                string dovizTitle = doviz.SelectSingleNode(".//li[1]//h3[1]//a[1]").InnerText.Replace("&#231;", "ç");
                string dovizAlis = doviz.SelectSingleNode(".//li[3]").InnerText;
                string dovizSatis = doviz.SelectSingleNode(".//li[4]").InnerText;
                string dovizDegisim = doviz.SelectSingleNode(".//li[5]").InnerText;
                string dovizSaat = doviz.SelectSingleNode(".//li[6]").InnerText;

                if (rowIndex >= dataGridView1.Rows.Count)
                {
                    dataGridView1.Rows.Add();
                }

                dataGridView1.Rows[rowIndex].Cells[0].Value = dovizTitle;
                dataGridView1.Rows[rowIndex].Cells[2].Value = dovizAlis;
                dataGridView1.Rows[rowIndex].Cells[3].Value = dovizSatis;
                dataGridView1.Rows[rowIndex].Cells[4].Value = dovizDegisim;
                dataGridView1.Rows[rowIndex].Cells[5].Value = dovizSaat;
                rowIndex++;
            }
             rowCount = rowIndex;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            kurGetir();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}