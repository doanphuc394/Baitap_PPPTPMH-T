using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB5
{
    public partial class Form1 : Form
    {
        // Chuỗi kết nối
        string strCon = @"Data Source=(LocalDB)\MSSQLLocalDB;
                          AttachDbFilename=D:\File_word_baitap\PTPMHDT\Lab_thuc_hanh\LAB5\LAB5\LAB5\DBConnect.mdf;
                          Integrated Security=True";

        SqlConnection sqlCon = null;

        private ListView lsvDanhSachSV;
        private TextBox txtMaSV;
        private TextBox txtTenSV;
        private ComboBox cbGioiTinh;
        private DateTimePicker dtpNgaySinh;
        private TextBox txtQueQuan;
        private TextBox txtMaLop;
        private Button btnThemSinhVien;
        private Label lblMaSV;
        private Label lblTenSV;
        private Label lblGioiTinh;
        private Label lblNgaySinh;
        private Label lblQueQuan;
        private Label lblMaLop;
        private Label lblTitle;

        public Form1()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Đoàn Phúc 1150080031 ";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            lblTitle = new Label();
            lblTitle.Text = "QUẢN LÝ SINH VIÊN";
            lblTitle.Font = new Font("Arial", 16, FontStyle.Bold);
            lblTitle.ForeColor = Color.Blue;
            lblTitle.Location = new Point(350, 10);
            lblTitle.Size = new Size(300, 30);
            this.Controls.Add(lblTitle);


            lblMaSV = new Label() { Text = "Mã SV:", Location = new Point(20, 60), Size = new Size(80, 20) };
            lblTenSV = new Label() { Text = "Tên SV:", Location = new Point(20, 95), Size = new Size(80, 20) };
            lblGioiTinh = new Label() { Text = "Giới tính:", Location = new Point(20, 130), Size = new Size(80, 20) };
            lblNgaySinh = new Label() { Text = "Ngày sinh:", Location = new Point(20, 165), Size = new Size(80, 20) };
            lblQueQuan = new Label() { Text = "Quê quán:", Location = new Point(20, 200), Size = new Size(80, 20) };
            lblMaLop = new Label() { Text = "Mã lớp:", Location = new Point(20, 235), Size = new Size(80, 20) };

            this.Controls.Add(lblMaSV);
            this.Controls.Add(lblTenSV);
            this.Controls.Add(lblGioiTinh);
            this.Controls.Add(lblNgaySinh);
            this.Controls.Add(lblQueQuan);
            this.Controls.Add(lblMaLop);

            txtMaSV = new TextBox() { Location = new Point(110, 58), Size = new Size(200, 25) };
            txtTenSV = new TextBox() { Location = new Point(110, 93), Size = new Size(200, 25) };
            txtQueQuan = new TextBox() { Location = new Point(110, 198), Size = new Size(200, 25) };
            txtMaLop = new TextBox() { Location = new Point(110, 233), Size = new Size(200, 25) };

            this.Controls.Add(txtMaSV);
            this.Controls.Add(txtTenSV);
            this.Controls.Add(txtQueQuan);
            this.Controls.Add(txtMaLop);

            cbGioiTinh = new ComboBox();
            cbGioiTinh.Location = new Point(110, 128);
            cbGioiTinh.Size = new Size(200, 25);
            cbGioiTinh.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Controls.Add(cbGioiTinh);

            dtpNgaySinh = new DateTimePicker();
            dtpNgaySinh.Location = new Point(110, 163);
            dtpNgaySinh.Size = new Size(200, 25);
            dtpNgaySinh.Format = DateTimePickerFormat.Short;
            this.Controls.Add(dtpNgaySinh);

            btnThemSinhVien = new Button();
            btnThemSinhVien.Text = "Thêm Sinh Viên";
            btnThemSinhVien.Location = new Point(110, 275);
            btnThemSinhVien.Size = new Size(200, 35);
            btnThemSinhVien.BackColor = Color.LightGreen;
            btnThemSinhVien.Font = new Font("Arial", 10, FontStyle.Bold);
            btnThemSinhVien.Click += new EventHandler(btnThemSinhVien_Click);
            this.Controls.Add(btnThemSinhVien);


            lsvDanhSachSV = new ListView();
            lsvDanhSachSV.Location = new Point(350, 50);
            lsvDanhSachSV.Size = new Size(620, 480);
            lsvDanhSachSV.View = View.Details;
            lsvDanhSachSV.FullRowSelect = true;
            lsvDanhSachSV.GridLines = true;

            lsvDanhSachSV.Columns.Add("Mã SV", 80);
            lsvDanhSachSV.Columns.Add("Tên SV", 150);
            lsvDanhSachSV.Columns.Add("Giới tính", 80);
            lsvDanhSachSV.Columns.Add("Ngày sinh", 100);
            lsvDanhSachSV.Columns.Add("Quê quán", 120);
            lsvDanhSachSV.Columns.Add("Mã lớp", 80);

            this.Controls.Add(lsvDanhSachSV);

            this.Load += new EventHandler(Form1_Load);
        }

        private void MoKetNoi()
        {
            if (sqlCon == null)
            {
                sqlCon = new SqlConnection(strCon);
            }
            if (sqlCon.State == ConnectionState.Closed)
            {
                sqlCon.Open();
            }
        }

        // Hàm đóng kết nối
        private void DongKetNoi()
        {
            if (sqlCon != null && sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }

        // Hàm hiển thị danh sách sinh viên
        private void HienThiDanhSach()
        {
            try
            {
                MoKetNoi();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "select * from SinhVien";
                sqlCmd.Connection = sqlCon;
                lsvDanhSachSV.Items.Clear();
                SqlDataReader reader = sqlCmd.ExecuteReader();

                while (reader.Read())
                {
                    string maSv = reader.GetString(0);
                    string tenSV = reader.GetString(1);
                    string gioiTinh = reader.GetString(2);
                    string ngaySinh = reader.GetDateTime(3).ToString("dd/MM/yyyy");
                    string queQuan = reader.GetString(4);
                    string maLop = reader.GetString(5);

                    ListViewItem lvi = new ListViewItem(maSv);
                    lvi.SubItems.Add(tenSV);
                    lvi.SubItems.Add(gioiTinh);
                    lvi.SubItems.Add(ngaySinh);
                    lvi.SubItems.Add(queQuan);
                    lvi.SubItems.Add(maLop);

                    lsvDanhSachSV.Items.Add(lvi);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThemSinhVien_Click(object sender, EventArgs e)
        {
            MoKetNoi();
            try
            {
                string maSV = txtMaSV.Text.Trim();
                string tenSV = txtTenSV.Text.Trim();
                string gioiTinh = cbGioiTinh.Text;
                string ngaySinh = dtpNgaySinh.Value.Month + "/" + dtpNgaySinh.Value.Day + "/" + dtpNgaySinh.Value.Year;
                string queQuan = txtQueQuan.Text.Trim();
                string maLop = txtMaLop.Text.Trim();

                // Kiểm tra dữ liệu đầu vào
                if (string.IsNullOrEmpty(maSV) || string.IsNullOrEmpty(tenSV) ||
                    string.IsNullOrEmpty(gioiTinh) || string.IsNullOrEmpty(queQuan) ||
                    string.IsNullOrEmpty(maLop))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                /* SqlCommand sqlCmd = new SqlCommand();
                 sqlCmd.CommandType = CommandType.Text;
                 sqlCmd.CommandText = "insert into SinhVien values ('" + maSV + "', '" + tenSV + "', '" + gioiTinh + "', '" + ngaySinh + "', '" + queQuan + "', '" + maLop + "')";
                 sqlCmd.Connection = sqlCon;*/
                // Dùng Parameter để thêm dữ liệu an toàn
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = @"INSERT INTO SinhVien 
                                (MaSV, TenSV, GioiTinh, NgaySinh, QueQuan, MaLop)
                                VALUES (@MaSV, @TenSV, @GioiTinh, @NgaySinh, @QueQuan, @MaLop)";
                sqlCmd.Connection = sqlCon;

                // Gán giá trị cho các parameter
                sqlCmd.Parameters.AddWithValue("@MaSV", maSV);
                sqlCmd.Parameters.AddWithValue("@TenSV", tenSV);
                sqlCmd.Parameters.AddWithValue("@GioiTinh", gioiTinh);
                sqlCmd.Parameters.AddWithValue("@NgaySinh", ngaySinh);
                sqlCmd.Parameters.AddWithValue("@QueQuan", queQuan);
                sqlCmd.Parameters.AddWithValue("@MaLop", maLop);

                int kq = sqlCmd.ExecuteNonQuery();

                if (kq > 0)
                {
                    MessageBox.Show("Thêm sinh viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HienThiDanhSach();

                    txtMaSV.Clear();
                    txtTenSV.Clear();
                    cbGioiTinh.SelectedIndex = -1;
                    txtQueQuan.Clear();
                    txtMaLop.Clear();
                    txtMaSV.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thêm dữ liệu bị lỗi! " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbGioiTinh.Items.Add("Nam");
            cbGioiTinh.Items.Add("Nữ");
            HienThiDanhSach();
        }
    }
}