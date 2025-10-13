using LAB5;
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
    public partial class Form2 : Form
    {
        // Chuỗi kết nối
        string strCon = @"Data Source=(LocalDB)\MSSQLLocalDB;
                          AttachDbFilename=D:\File_word_baitap\PTPMHDT\Lab_thuc_hanh\LAB5\LAB5\LAB5\DBConnect.mdf;
                          Integrated Security=True";

        SqlConnection sqlCon = null;

        // Các control
        ComboBox cbMaLop, cbGioiTinh;
        ListView lsvDanhSach;
        TextBox txtMaSV, txtTenSV, txtQueQuan, txtMaLop;
        DateTimePicker dtpNgaySinh;
        Button btnSuaThongTin;

        public Form2()
        {
            this.Text = "Đoàn Phúc 1150080031";
            this.Size = new Size(820, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            KhoiTaoGiaoDien();
        }

        private void KhoiTaoGiaoDien()
        {
            Label lblTitle = new Label
            {
                Text = "sửa dữ liệu không dùng parameter",
                Font = new Font("Arial", 14),
                Dock = DockStyle.Top,
                Height = 40,
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblTitle);

            // ComboBox chọn mã lớp
            cbMaLop = new ComboBox
            {
                Location = new Point(200, 50),
                Width = 200
            };
            cbMaLop.SelectedIndexChanged += cbMaLop_SelectedIndexChanged;
            Label lblChonLop = new Label
            {
                Text = "Chọn mã lớp:",
                Location = new Point(100, 54),
                AutoSize = true
            };
            this.Controls.Add(lblChonLop);
            this.Controls.Add(cbMaLop);

            // ListView danh sách sinh viên
            lsvDanhSach = new ListView
            {
                Location = new Point(30, 90),
                Size = new Size(450, 300),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true
            };
            lsvDanhSach.Columns.Add("Mã SV", 70);
            lsvDanhSach.Columns.Add("Tên SV", 100);
            lsvDanhSach.Columns.Add("Giới tính", 60);
            lsvDanhSach.Columns.Add("Ngày sinh", 90);
            lsvDanhSach.Columns.Add("Quê quán", 80);
            lsvDanhSach.Columns.Add("Mã lớp", 60);
            lsvDanhSach.SelectedIndexChanged += lsvDanhSach_SelectedIndexChanged;
            this.Controls.Add(lsvDanhSach);

            // Nhóm nhập thông tin sinh viên
            Label lblThongTin = new Label
            {
                Text = "Thông tin sinh viên:",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(520, 90)
            };
            this.Controls.Add(lblThongTin);

            int xLabel = 520, xInput = 620, y = 120, step = 30;

            // Các textbox
            this.Controls.Add(TaoLabel("Mã SV:", xLabel, y));
            txtMaSV = TaoTextbox(xInput, y);

            this.Controls.Add(TaoLabel("Tên SV:", xLabel, y += step));
            txtTenSV = TaoTextbox(xInput, y);

            this.Controls.Add(TaoLabel("Giới tính:", xLabel, y += step));
            cbGioiTinh = new ComboBox
            {
                Location = new Point(xInput, y),
                Width = 150
            };
            cbGioiTinh.Items.Add("Nam");
            cbGioiTinh.Items.Add("Nữ");
            this.Controls.Add(cbGioiTinh);

            this.Controls.Add(TaoLabel("Ngày sinh:", xLabel, y += step));
            dtpNgaySinh = new DateTimePicker
            {
                Location = new Point(xInput, y),
                Width = 150
            };
            this.Controls.Add(dtpNgaySinh);

            this.Controls.Add(TaoLabel("Quê quán:", xLabel, y += step));
            txtQueQuan = TaoTextbox(xInput, y);

            this.Controls.Add(TaoLabel("Mã lớp:", xLabel, y += step));
            txtMaLop = TaoTextbox(xInput, y);

            // Nút Sửa thông tin
            btnSuaThongTin = new Button
            {
                Text = "Sửa thông tin",
                Location = new Point(580, y + 40),
                Width = 120
            };
            btnSuaThongTin.Click += btnSuaThongTin_Click;
            this.Controls.Add(btnSuaThongTin);

            this.Load += Form1_Load;
        }

        private Label TaoLabel(string text, int x, int y)
        {
            return new Label { Text = text, Location = new Point(x, y + 4), AutoSize = true };
        }

        private TextBox TaoTextbox(int x, int y)
        {
            TextBox t = new TextBox { Location = new Point(x, y), Width = 150 };
            this.Controls.Add(t);
            return t;
        }

        private void MoKetNoi()
        {
            if (sqlCon == null)
                sqlCon = new SqlConnection(strCon);
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
        }

        private void DongKetNoi()
        {
            if (sqlCon != null && sqlCon.State == ConnectionState.Open)
                sqlCon.Close();
        }
        private void HienThiDSMaLop()
        {
            MoKetNoi();
            SqlCommand cmd = new SqlCommand("SELECT DISTINCT MaLop FROM SinhVien", sqlCon);
            SqlDataReader reader = cmd.ExecuteReader();
            cbMaLop.Items.Clear();

            while (reader.Read())
            {
                string maLop = reader.GetString(0);
                cbMaLop.Items.Add(maLop);
            }

            reader.Close();
            DongKetNoi();
        }
        private void HienThiDSSinhVienTheoLop(string maLop)
        {
            MoKetNoi();
            SqlCommand cmd = new SqlCommand("SELECT * FROM SinhVien WHERE MaLop='" + maLop + "'", sqlCon);
            SqlDataReader reader = cmd.ExecuteReader();
            lsvDanhSach.Items.Clear();
            while (reader.Read())
            {
                string maSV = reader.GetString(0);
                string tenSV = reader.GetString(1);
                string gioiTinh = reader.GetString(2);
                string ngaySinh = reader.GetDateTime(3).ToString("dd/MM/yyyy");
                string queQuan = reader.GetString(4);
                ListViewItem lvi = new ListViewItem(maSV);
                lvi.SubItems.Add(tenSV);
                lvi.SubItems.Add(gioiTinh);
                lvi.SubItems.Add(ngaySinh);
                lvi.SubItems.Add(queQuan);
                lvi.SubItems.Add(maLop);
                lsvDanhSach.Items.Add(lvi);
            }
            reader.Close();
            DongKetNoi();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            HienThiDSMaLop();
        }

        private void cbMaLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbMaLop.SelectedIndex == -1) return;
            string maLop = cbMaLop.SelectedItem.ToString();
            HienThiDSSinhVienTheoLop(maLop);
        }

        private void lsvDanhSach_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lsvDanhSach.SelectedItems.Count == 0) return;
            ListViewItem lvi = lsvDanhSach.SelectedItems[0];
            txtMaSV.Text = lvi.SubItems[0].Text;
            txtTenSV.Text = lvi.SubItems[1].Text;
            cbGioiTinh.Text = lvi.SubItems[2].Text;

            DateTime ns;
            if (DateTime.TryParse(lvi.SubItems[3].Text, out ns))
                dtpNgaySinh.Value = ns;

            txtQueQuan.Text = lvi.SubItems[4].Text;
            txtMaLop.Text = lvi.SubItems[5].Text;
        }

        /*private void btnSuaThongTin_Click(object sender, EventArgs e)
        {
            try
            {
                MoKetNoi();
                string maSV = txtMaSV.Text;
                string tenSV = txtTenSV.Text;
                string gioiTinh = cbGioiTinh.Text;
                string ngaySinh = dtpNgaySinh.Value.ToString("yyyy/MM/dd");
                string queQuan = txtQueQuan.Text;
                string maLop = txtMaLop.Text;

                string sql = "UPDATE SinhVien SET TenSV=N'" + tenSV +
                             "', GioiTinh=N'" + gioiTinh +
                             "', NgaySinh='" + ngaySinh +
                             "', QueQuan=N'" + queQuan +
                             "', MaLop='" + maLop +
                             "' WHERE MaSV='" + maSV + "'";

                SqlCommand cmd = new SqlCommand(sql, sqlCon);
                int kq = cmd.ExecuteNonQuery();

                if (kq > 0)
                {
                    MessageBox.Show("Cập nhật thành công!");
                    HienThiDSSinhVienTheoLop(maLop);
                }
                else
                {
                    MessageBox.Show("Cập nhật không thành công!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                DongKetNoi();
            }
        }*/
        private void btnSuaThongTin_Click(object sender, EventArgs e)
        {
            try
            {
                MoKetNoi();

                string sql = @"UPDATE SinhVien
                       SET TenSV = @TenSV,
                           GioiTinh = @GioiTinh,
                           NgaySinh = @NgaySinh,
                           QueQuan = @QueQuan,
                           MaLop = @MaLop
                       WHERE MaSV = @MaSV";

                using (SqlCommand cmd = new SqlCommand(sql, sqlCon))
                {
                    // Gán giá trị cho các tham số (Parameter)
                    cmd.Parameters.AddWithValue("@MaSV", txtMaSV.Text);
                    cmd.Parameters.AddWithValue("@TenSV", txtTenSV.Text);
                    cmd.Parameters.AddWithValue("@GioiTinh", cbGioiTinh.Text);
                    cmd.Parameters.AddWithValue("@NgaySinh", dtpNgaySinh.Value);
                    cmd.Parameters.AddWithValue("@QueQuan", txtQueQuan.Text);
                    cmd.Parameters.AddWithValue("@MaLop", txtMaLop.Text);

                    int kq = cmd.ExecuteNonQuery();

                    if (kq > 0)
                    {
                        MessageBox.Show("Cập nhật dữ liệu thành công!");
                        HienThiDSSinhVienTheoLop(txtMaLop.Text);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sinh viên cần sửa!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                DongKetNoi();
            }
        }


        [STAThread]
        public static void Form2_Load()
        {
            Application.EnableVisualStyles();
            Application.Run(new Form1());
        }
    }
}