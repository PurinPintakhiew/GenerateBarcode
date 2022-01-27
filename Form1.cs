using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BarcodeLib;
using System.Data.OleDb;
using System.IO;
using System.Drawing.Imaging;

namespace GenerateBarcode2
{
    public partial class Form1 : Form
    {
        OleDbConnection conn;
        OleDbDataAdapter da;
        DataSet ds;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            Barcode barcode = new Barcode();
            Color forecoler = Color.Black;
            Color blackColor = Color.Transparent;

            if (txtBarcode.Text == "")
            {
                MessageBox.Show("กรุณากรอกรหัสบาร์โค้ด");
            }
            else
            {
                int cn;
                cn = txtBarcode.Text.Length;
                if (cn >= 5)
                {
                    Image img = barcode.Encode(BarcodeLib.TYPE.CODE128,txtBarcode.Text, forecoler, blackColor, 300, 100);
                    pictureBarcode.Image = img;
                }
                else
                {
                    MessageBox.Show("กรุณากรอกรหัสบาร์โค้ด 5 หลัก ขึ้นไป");
                }

            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            
            Image img;
            img = pictureBarcode.Image;
            if (img != null)
            {
                this.dataSet1.Clear();
                using (MemoryStream ms = new MemoryStream())
                {
                    img.Save(ms, ImageFormat.Png);
                    for (int i = 0; i < number.Value; i++)
                    {
                        this.dataSet1.Barcode.AddBarcodeRow(txtBarcode.Text, txtName.Text, txtCost.Text, txtPrice.Text, ms.ToArray());
                    }
                }
                using (frmReport rfm = new frmReport(this.dataSet1.Barcode))
                {
                    rfm.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("กรุณาสร้างบาร์โค้ด");
            }
            
        }

        public void setProduct(int row)
        {
            try
            {
                txtBarcode.Text = ds.Tables["Barcode"].Rows[row]["Barcode"].ToString();
                txtName.Text = ds.Tables["Barcode"].Rows[row]["Itm_name"].ToString();
                txtPrice.Text = ds.Tables["Barcode"].Rows[row]["Price"].ToString();
                txtCost.Text = ds.Tables["Barcode"].Rows[row]["Max_stock"].ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("ไม่พบข้อมูลสินค้า");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new OleDbConnection("Provider = Microsoft.Jet.OLEDB.4.0; Data Source = C:\\MSN\\msn.mdb");
            conn.Open();
            string sql = "SELECT * FROM Barcode";
            da = new OleDbDataAdapter(sql, conn);
            ds = new DataSet();
            da.Fill(ds, "Barcode");
            if (ds.Tables["Barcode"].Rows.Count >= 0)
            {
                setProduct(0);
            }
            else
            {
                MessageBox.Show("ไม่พบข้อมูล");
            }
            conn.Close();

            Barcode barcode = new Barcode();
            Color forecoler = Color.Black;
            Color blackColor = Color.Transparent;

            if (txtBarcode.Text == "")
            {
                MessageBox.Show("ไม่มีรหัสบาร์โค้ด");
            }
            else
            {
                int cn;
                cn = txtBarcode.Text.Length;
                if (cn >= 5)
                {
                    Image img = barcode.Encode(BarcodeLib.TYPE.CODE128, txtBarcode.Text, forecoler, blackColor, 300, 100);
                    pictureBarcode.Image = img;
                }
                else
                {
                    MessageBox.Show("กรุณากรอกรหัสบาร์โค้ด 5 หลัก ขึ้นไป");
                }

            }
        }

    }
}
