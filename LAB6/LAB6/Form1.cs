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

namespace LAB6
{
    public partial class Form1 : Form
    {


        
        private readonly string _connStr =
            @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\File_word_baitap\PTPMHDT\Lab_thuc_hanh\LAB6\LAB6\QuanLyBanSach.mdf;Integrated Security=True;Connect Timeout=30";
        // ===== Điều khiển UI =====
        private ListView lsvDanhSach;
        private Label lblTitle;

        private GroupBox grpChiTiet;
        private Label lblMaXB;
        private Label lblTenXB;
        private Label lblDiaChi;
        private TextBox txtMaXB;
        private TextBox txtTenXB;
        private TextBox txtDiaChi;

        private Button btnRefresh;

        public Form1()
        {
            InitializeComponent();
        }

        // ===================== UI =====================
        private void InitializeComponent()
        {
            // Form
            this.Text = "1150080031_Đoàn Phúc";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(900, 520);
            this.Font = new Font("Segoe UI", 10F);

            // Title
            lblTitle = new Label
            {
                Text = "DANH SÁCH NHÀ XUẤT BẢN",
                AutoSize = true,
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.ForestGreen,
                Location = new Point(20, 15)
            };
            this.Controls.Add(lblTitle);

            // Nút Refresh
            btnRefresh = new Button
            {
                Text = "Làm mới",
                AutoSize = true,
                Location = new Point(670, 18)
            };
            btnRefresh.Click += (s, e) => HienThiDanhSachNXB();
            this.Controls.Add(btnRefresh);

            // ListView
            lsvDanhSach = new ListView
            {
                Location = new Point(20, 55),
                Size = new Size(560, 430),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                HideSelection = false
            };
            lsvDanhSach.Columns.Add("Mã XB", 100, HorizontalAlignment.Left);
            lsvDanhSach.Columns.Add("Tên NXB", 220, HorizontalAlignment.Left);
            lsvDanhSach.Columns.Add("Địa chỉ", 220, HorizontalAlignment.Left);
            lsvDanhSach.SelectedIndexChanged += lsvDanhSach_SelectedIndexChanged;
            this.Controls.Add(lsvDanhSach);

            // GroupBox Chi tiết
            grpChiTiet = new GroupBox
            {
                Text = "Thông tin chi tiết",
                Location = new Point(600, 55),
                Size = new Size(280, 250)
            };
            this.Controls.Add(grpChiTiet);
            // Labels + TextBoxes
            lblMaXB = new Label { Text = "Mã XB:", AutoSize = true, Location = new Point(15, 35) };
            txtMaXB = new TextBox { Location = new Point(85, 30), Width = 170, ReadOnly = true };

            lblTenXB = new Label { Text = "Tên NXB:", AutoSize = true, Location = new Point(15, 80) };
            txtTenXB = new TextBox { Location = new Point(85, 75), Width = 170, ReadOnly = true };

            lblDiaChi = new Label { Text = "Địa chỉ:", AutoSize = true, Location = new Point(15, 125) };
            txtDiaChi = new TextBox
            {
                Location = new Point(85, 120),
                Width = 170,
                Height = 90,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true
            };

            grpChiTiet.Controls.Add(lblMaXB);
            grpChiTiet.Controls.Add(txtMaXB);
            grpChiTiet.Controls.Add(lblTenXB);
            grpChiTiet.Controls.Add(txtTenXB);
            grpChiTiet.Controls.Add(lblDiaChi);
            grpChiTiet.Controls.Add(txtDiaChi);

            // Sự kiện Form Load
            this.Load += Form1_Load;
        }

        // ===================== DATA =====================

        private void Form1_Load(object sender, EventArgs e)
        {
            HienThiDanhSachNXB();
        }

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
                        lsvDanhSach.BeginUpdate();
                        lsvDanhSach.Items.Clear();

                        while (reader.Read())
                        {
                            string ma = reader.GetString(0).Trim();
                            string ten = reader.GetString(1);
                            string diaChi = reader.IsDBNull(2) ? "" : reader.GetString(2);

                            var lvi = new ListViewItem(ma);
                            lvi.SubItems.Add(ten);
                            lvi.SubItems.Add(diaChi);
                            lsvDanhSach.Items.Add(lvi);
                        }

                        lsvDanhSach.EndUpdate();
                    }
                }

                // Xóa vùng chi tiết khi làm mới
                txtMaXB.Text = txtTenXB.Text = txtDiaChi.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách NXB:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lsvDanhSach_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lsvDanhSach.SelectedItems.Count == 0) return;

            var lvi = lsvDanhSach.SelectedItems[0];
            string ma = lvi.SubItems[0].Text;
            HienThiThongTinNXBTheoMa(ma);
        }

        private void HienThiThongTinNXBTheoMa(string maXB)
        {
            try
            {
                using (var con = new SqlConnection(_connStr))
                using (var cmd = new SqlCommand("sp_HienThiChiTietNXB", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@MaXB", SqlDbType.Char, 10).Value = maXB;

                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        txtMaXB.Text = txtTenXB.Text = txtDiaChi.Text = "";

                        if (reader.Read())
                        {
                            txtMaXB.Text = reader.GetString(0).Trim();
                            txtTenXB.Text = reader.GetString(1);
                            txtDiaChi.Text = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết NXB:\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}