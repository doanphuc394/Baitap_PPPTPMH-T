using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace LAB5
{
    public partial class Form3 : Form
    {
        // Chuỗi kết nối
        string strCon = @"Data Source=(LocalDB)\MSSQLLocalDB;
                          AttachDbFilename=D:\File_word_baitap\PTPMHDT\Lab_thuc_hanh\LAB5\LAB5\LAB5\DBConnect.mdf;
                          Integrated Security=True";

        SqlConnection sqlCon = null;
        ListView lsvDanhSach;
        Button btnXoaSV;
        string maSV = "";

        public Form3()
        {
            this.Text = "Đoàn Phúc 1150080031";
            this.Size = new Size(800, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            KhoiTaoGiaoDien();
        }

        private void KhoiTaoGiaoDien()
        {
            // Tiêu đề
            Label lblTitle = new Label
            {
                Text = "Xóa dữ liệu",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 40,
                TextAlign = ContentAlignment.MiddleCenter
            };
            this.Controls.Add(lblTitle);

            // Nhãn
            Label lblDanhSach = new Label
            {
                Text = "Danh sách sinh viên:",
                Location = new Point(30, 50),
                AutoSize = true
            };
            this.Controls.Add(lblDanhSach);

            // ListView hiển thị sinh viên
            lsvDanhSach = new ListView
            {
                Location = new Point(30, 80),
                Size = new Size(720, 300),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true
            };
            lsvDanhSach.Columns.Add("Mã SV", 80);
            lsvDanhSach.Columns.Add("Tên SV", 150);
            lsvDanhSach.Columns.Add("Giới tính", 80);
            lsvDanhSach.Columns.Add("Ngày sinh", 100);
            lsvDanhSach.Columns.Add("Quê quán", 150);
            lsvDanhSach.Columns.Add("Mã lớp", 80);
            lsvDanhSach.SelectedIndexChanged += lsvDanhSach_SelectedIndexChanged;
            this.Controls.Add(lsvDanhSach);

            // Nút xóa
            btnXoaSV = new Button
            {
                Text = "Xóa sinh viên",
                Location = new Point(320, 400),
                Width = 150,
                Height = 35,
                BackColor = Color.LightCoral
            };
            btnXoaSV.Click += btnXoaSV_Click;
            this.Controls.Add(btnXoaSV);

            this.Load += Form3_Load;
        }

        // Hàm mở kết nối
        private void MoKetNoi()
        {
            if (sqlCon == null)
                sqlCon = new SqlConnection(strCon);
            if (sqlCon.State == ConnectionState.Closed)
                sqlCon.Open();
        }
        // Hàm đóng kết nối
        private void DongKetNoi()
        {
            if (sqlCon != null && sqlCon.State == ConnectionState.Open)
                sqlCon.Close();
        }

        // Hiển thị danh sách sinh viên
        private void HienThiDSSinhVien()
        {
            try
            {
                MoKetNoi();
                SqlCommand sqlCmd = new SqlCommand("SELECT * FROM SinhVien", sqlCon);
                SqlDataReader reader = sqlCmd.ExecuteReader();
                lsvDanhSach.Items.Clear();

                while (reader.Read())
                {
                    string ma = reader.GetString(0);
                    string ten = reader.GetString(1);
                    string gt = reader.GetString(2);
                    string ngay = reader.GetDateTime(3).ToString("dd/MM/yyyy");
                    string que = reader.GetString(4);
                    string lop = reader.GetString(5);

                    ListViewItem lvi = new ListViewItem(ma);
                    lvi.SubItems.Add(ten);
                    lvi.SubItems.Add(gt);
                    lvi.SubItems.Add(ngay);
                    lvi.SubItems.Add(que);
                    lvi.SubItems.Add(lop);
                    lsvDanhSach.Items.Add(lvi);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị danh sách: " + ex.Message);
            }
            finally
            {
                DongKetNoi();
            }
        }

        // Sự kiện khi form load
        private void Form3_Load(object sender, EventArgs e)
        {
            HienThiDSSinhVien();
        }

        // Khi chọn sinh viên trong listview
        private void lsvDanhSach_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lsvDanhSach.SelectedItems.Count == 0)
                return;

            ListViewItem lvi = lsvDanhSach.SelectedItems[0];
            maSV = lvi.SubItems[0].Text.Trim(); // lấy mã sinh viên được chọn
        }

        // Nút xóa sinh viên
        private void btnXoaSV_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(maSV))
            {
                MessageBox.Show("Bạn chưa chọn sinh viên nào để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa sinh viên có mã: " + maSV + " ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                XoaSV(maSV);
            }
        }

        // Hàm xóa sinh viên không dùng parameter
        private void XoaSV(string maSV)
        {
            try
            {
                MoKetNoi();
                string sql = "DELETE FROM SinhVien WHERE MaSV='" + maSV + "'";
                SqlCommand cmd = new SqlCommand(sql, sqlCon);
                int kq = cmd.ExecuteNonQuery();

                if (kq > 0)
                {
                    MessageBox.Show("Xóa dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    HienThiDSSinhVien();
                }
                else
                {
                    MessageBox.Show("Không có dữ liệu nào bị xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa sinh viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DongKetNoi();
            }
        }
        // dung Parameter
/*        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                MoKetNoi();

                // Câu lệnh SQL dùng Parameter
                string sql = "DELETE FROM SinhVien WHERE MaSV = @MaSV";

                // Tạo đối tượng SqlCommand
                SqlCommand cmd = new SqlCommand(sql, conn);

                // Thêm Parameter và gán giá trị từ textbox
                cmd.Parameters.AddWithValue("@MaSV", txtMaSV.Text);

                // Thực thi lệnh
                int kq = cmd.ExecuteNonQuery();

                if (kq > 0)
                {
                    MessageBox.Show("Xóa dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sinh viên để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa sinh viên: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DongKetNoi();
            }
        }*/

        [STAThread]
        public static void Form3_Load()
        {
            Application.EnableVisualStyles();
            Application.Run(new Form3());
        }
    }
}