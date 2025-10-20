using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace LAB6
{
    public class Form2 : Form
    {
        
        private readonly string _connStr =
           @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\File_word_baitap\PTPMHDT\Lab_thuc_hanh\LAB6\LAB6\QuanLyBanSach.mdf;Integrated Security=True;Connect Timeout=30";

        private ListView lsvDanhSach;
        private TextBox txtMaXB, txtTenXB, txtDiaChi;
        private Button btnRefresh, btnThem;
        private Label lblHeader;

        public Form2()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            // ===== Form =====
            this.Text = "Đoàn Phúc - 1150080031";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(950, 560);
            this.BackColor = Color.WhiteSmoke;
            this.Font = new Font("Segoe UI", 10F);

            // ===== Header =====
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.SeaGreen
            };
            this.Controls.Add(pnlHeader);

            lblHeader = new Label
            {
                Text = "QUẢN LÝ NHÀ XUẤT BẢN",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlHeader.Controls.Add(lblHeader);

            // ===== Danh sách =====
            lsvDanhSach = new ListView
            {
                Location = new Point(20, 80),
                Size = new Size(560, 430),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                HideSelection = false,
                BackColor = Color.White
            };
            lsvDanhSach.Columns.Add("Mã XB", 100, HorizontalAlignment.Left);
            lsvDanhSach.Columns.Add("Tên NXB", 220, HorizontalAlignment.Left);
            lsvDanhSach.Columns.Add("Địa chỉ", 220, HorizontalAlignment.Left);
            this.Controls.Add(lsvDanhSach);

            // ===== GroupBox nhập thông tin =====
            GroupBox grpChiTiet = new GroupBox
            {
                Text = "Thêm nhà xuất bản mới",
                Location = new Point(600, 80),
                Size = new Size(320, 300)
            };
            this.Controls.Add(grpChiTiet);

            Label lblMa = new Label { Text = "Mã XB:", Location = new Point(20, 40), AutoSize = true };
            txtMaXB = new TextBox { Location = new Point(100, 35), Width = 180 };
            Label lblTen = new Label { Text = "Tên NXB:", Location = new Point(20, 85), AutoSize = true };
            txtTenXB = new TextBox { Location = new Point(100, 80), Width = 180 };

            Label lblDiaChi = new Label { Text = "Địa chỉ:", Location = new Point(20, 130), AutoSize = true };
            txtDiaChi = new TextBox
            {
                Location = new Point(100, 125),
                Width = 180,
                Height = 100,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };

            grpChiTiet.Controls.Add(lblMa);
            grpChiTiet.Controls.Add(txtMaXB);
            grpChiTiet.Controls.Add(lblTen);
            grpChiTiet.Controls.Add(txtTenXB);
            grpChiTiet.Controls.Add(lblDiaChi);
            grpChiTiet.Controls.Add(txtDiaChi);

            // ===== Nút Thêm dữ liệu =====
            btnThem = new Button
            {
                Text = "Thêm dữ liệu",
                Location = new Point(620, 400),
                Width = 140,
                Height = 40,
                BackColor = Color.RoyalBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat
            };
            btnThem.FlatAppearance.BorderSize = 0;
            btnThem.MouseEnter += (s, e) => btnThem.BackColor = Color.MidnightBlue;
            btnThem.MouseLeave += (s, e) => btnThem.BackColor = Color.RoyalBlue;
            btnThem.Click += BtnThem_Click;
            this.Controls.Add(btnThem);

            // ===== Nút Làm mới =====
            btnRefresh = new Button
            {
                Text = "Làm mới",
                Location = new Point(780, 400),
                Width = 130,
                Height = 40,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.MediumSeaGreen,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.MouseEnter += (s, e) => btnRefresh.BackColor = Color.ForestGreen;
            btnRefresh.MouseLeave += (s, e) => btnRefresh.BackColor = Color.MediumSeaGreen;
            btnRefresh.Click += (s, e) => HienThiDanhSachNXB();
            this.Controls.Add(btnRefresh);

            this.Load += (s, e) => HienThiDanhSachNXB();
        }

        // ======================= CHỨC NĂNG =======================
        private void HienThiDanhSachNXB()
        {
            try
            {
                using (var con = new SqlConnection(_connStr))
                using (var cmd = new SqlCommand("sp_HienThiNXB", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        lsvDanhSach.Items.Clear();
                        while (reader.Read())
                        {
                            var item = new ListViewItem(reader.GetString(0).Trim());
                            item.SubItems.Add(reader.GetString(1));
                            item.SubItems.Add(reader.IsDBNull(2) ? "" : reader.GetString(2));
                            lsvDanhSach.Items.Add(item);
                        }
                    }
                }

                txtMaXB.Clear();
                txtTenXB.Clear();
                txtDiaChi.Clear();
                txtMaXB.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị danh sách:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaXB.Text) ||
                string.IsNullOrWhiteSpace(txtTenXB.Text) ||
                string.IsNullOrWhiteSpace(txtDiaChi.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var con = new SqlConnection(_connStr))
                using (var cmd = new SqlCommand("sp_ThemDuLieu", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@MaXB", SqlDbType.Char, 10).Value = txtMaXB.Text.Trim();
                    cmd.Parameters.Add("@TenNXB", SqlDbType.NVarChar, 100).Value = txtTenXB.Text.Trim();
                    cmd.Parameters.Add("@DiaChi", SqlDbType.NVarChar, 500).Value = txtDiaChi.Text.Trim();

                    con.Open();
                    int kq = cmd.ExecuteNonQuery();

                    if (kq > 0)
                    {
                        MessageBox.Show(" Thêm dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        HienThiDanhSachNXB();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm dữ liệu:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
