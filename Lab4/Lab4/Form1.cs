using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace Lab4
{
    public partial class Form1 : Form
    {
        string strCon = @"Data Source=ADMIN-PC\MYSERVER01;Initial Catalog=quanlylop;Integrated Security=True";
        SqlConnection sqlCon = null;
        SqlDataAdapter adapter = null;
        DataTable dt = null;

        // Controls
        DataGridView dataGridView1;
        TextBox txtMaSV, txtTenSV, txtQueQuan, txtMaLop;
        CheckBox chkGioiTinh;
        DateTimePicker dtNgaySinh;
        Button btnMoKetNoi, btnDongKetNoi, btnLoadData, btnAdd, btnUpdate, btnDelete;

        public Form1()
        {
            InitializeComponent();
            BuildUI();
        }

        private void BuildUI()
        {
            this.Text = "Quản lý Sinh Viên";
            this.Size = new Size(900, 600);

            Label lblMaSV = new Label() { Text = "Mã SV:", Location = new Point(20, 20) };
            txtMaSV = new TextBox() { Location = new Point(120, 20), Width = 200 };

            Label lblTenSV = new Label() { Text = "Tên SV:", Location = new Point(20, 60) };
            txtTenSV = new TextBox() { Location = new Point(120, 60), Width = 200 };

            Label lblGioiTinh = new Label() { Text = "Giới tính:", Location = new Point(20, 100) };
            chkGioiTinh = new CheckBox() { Text = "Nam", Location = new Point(120, 100) };

            Label lblNgaySinh = new Label() { Text = "Ngày sinh:", Location = new Point(20, 140) };
            dtNgaySinh = new DateTimePicker() { Location = new Point(120, 140), Format = DateTimePickerFormat.Short };

            Label lblQueQuan = new Label() { Text = "Quê quán:", Location = new Point(20, 180) };
            txtQueQuan = new TextBox() { Location = new Point(120, 180), Width = 200 };

            Label lblMaLop = new Label() { Text = "Mã lớp:", Location = new Point(20, 220) };
            txtMaLop = new TextBox() { Location = new Point(120, 220), Width = 200 };

            // Buttons
            btnMoKetNoi = new Button() { Text = "Mở kết nối", Location = new Point(400, 20), Width = 120 };
            btnDongKetNoi = new Button() { Text = "Đóng kết nối", Location = new Point(550, 20), Width = 120 };
            btnLoadData = new Button() { Text = "Load dữ liệu", Location = new Point(400, 60), Width = 120 };
            btnAdd = new Button() { Text = "Thêm", Location = new Point(550, 60), Width = 120 };
            btnUpdate = new Button() { Text = "Cập nhật", Location = new Point(400, 100), Width = 120 };
            btnDelete = new Button() { Text = "Xóa", Location = new Point(550, 100), Width = 120 };

            // DataGridView
            dataGridView1 = new DataGridView()
            {
                Location = new Point(20, 280),
                Size = new Size(820, 250),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            // Add controls
            this.Controls.Add(lblMaSV);
            this.Controls.Add(txtMaSV);
            this.Controls.Add(lblTenSV);
            this.Controls.Add(txtTenSV);
            this.Controls.Add(lblGioiTinh);
            this.Controls.Add(chkGioiTinh);
            this.Controls.Add(lblNgaySinh);
            this.Controls.Add(dtNgaySinh);
            this.Controls.Add(lblQueQuan);
            this.Controls.Add(txtQueQuan);
            this.Controls.Add(lblMaLop);
            this.Controls.Add(txtMaLop);

            this.Controls.Add(btnMoKetNoi);
            this.Controls.Add(btnDongKetNoi);
            this.Controls.Add(btnLoadData);
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnUpdate);
            this.Controls.Add(btnDelete);
            this.Controls.Add(dataGridView1);

            // Gán sự kiện
            btnMoKetNoi.Click += btnMoKetNoi_Click;
            btnDongKetNoi.Click += btnDongKetNoi_Click;
            btnLoadData.Click += btnLoadData_Click;
            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;

            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            this.FormClosing += Form1_FormClosing;
        }

        // ================== CODE XỬ LÝ ===================
        private void btnMoKetNoi_Click(object sender, EventArgs e)
        {
            try
            {
                if (sqlCon == null)
                    sqlCon = new SqlConnection(strCon);

                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                    MessageBox.Show("Kết nối thành công!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnDongKetNoi_Click(object sender, EventArgs e)
        {
            if (sqlCon != null && sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
                MessageBox.Show("Đã đóng kết nối.");
            }
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            try
            {
                if (sqlCon == null) sqlCon = new SqlConnection(strCon);
                if (sqlCon.State == ConnectionState.Closed) sqlCon.Open();

                string sql = "SELECT MaSV, TenSV, GioiTinh, NgaySinh, QueQuan, MaLop FROM SinhVien";
                adapter = new SqlDataAdapter(sql, sqlCon);
                dt = new DataTable();
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dữ liệu: " + ex.Message);
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;
            if (dataGridView1.CurrentRow.DataBoundItem is DataRowView drv)
            {
                txtMaSV.Text = drv["MaSV"].ToString();
                txtTenSV.Text = drv["TenSV"].ToString();
                chkGioiTinh.Checked = drv["GioiTinh"].ToString() == "True";
                dtNgaySinh.Value = drv["NgaySinh"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(drv["NgaySinh"]);
                txtQueQuan.Text = drv["QueQuan"].ToString();
                txtMaLop.Text = drv["MaLop"].ToString();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = "INSERT INTO SinhVien (MaSV, TenSV, GioiTinh, NgaySinh, QueQuan, MaLop) VALUES (@MaSV, @TenSV, @GioiTinh, @NgaySinh, @QueQuan, @MaLop)";
                using (SqlCommand cmd = new SqlCommand(sql, sqlCon))
                {
                    cmd.Parameters.AddWithValue("@MaSV", txtMaSV.Text.Trim());
                    cmd.Parameters.AddWithValue("@TenSV", txtTenSV.Text.Trim());
                    cmd.Parameters.AddWithValue("@GioiTinh", chkGioiTinh.Checked ? 1 : 0);
                    cmd.Parameters.AddWithValue("@NgaySinh", dtNgaySinh.Value);
                    cmd.Parameters.AddWithValue("@QueQuan", txtQueQuan.Text.Trim());
                    cmd.Parameters.AddWithValue("@MaLop", txtMaLop.Text.Trim());
                    cmd.ExecuteNonQuery();
                }
                btnLoadData_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm: " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = "UPDATE SinhVien SET TenSV=@TenSV, GioiTinh=@GioiTinh, NgaySinh=@NgaySinh, QueQuan=@QueQuan, MaLop=@MaLop WHERE MaSV=@MaSV";
                using (SqlCommand cmd = new SqlCommand(sql, sqlCon))
                {
                    cmd.Parameters.AddWithValue("@MaSV", txtMaSV.Text.Trim());
                    cmd.Parameters.AddWithValue("@TenSV", txtTenSV.Text.Trim());
                    cmd.Parameters.AddWithValue("@GioiTinh", chkGioiTinh.Checked ? 1 : 0);
                    cmd.Parameters.AddWithValue("@NgaySinh", dtNgaySinh.Value);
                    cmd.Parameters.AddWithValue("@QueQuan", txtQueQuan.Text.Trim());
                    cmd.Parameters.AddWithValue("@MaLop", txtMaLop.Text.Trim());
                    cmd.ExecuteNonQuery();
                }
                btnLoadData_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = "DELETE FROM SinhVien WHERE MaSV=@MaSV";
                using (SqlCommand cmd = new SqlCommand(sql, sqlCon))
                {
                    cmd.Parameters.AddWithValue("@MaSV", txtMaSV.Text.Trim());
                    cmd.ExecuteNonQuery();
                }
                btnLoadData_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xóa: " + ex.Message);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sqlCon != null && sqlCon.State == ConnectionState.Open)
                sqlCon.Close();
        }
    }
}
