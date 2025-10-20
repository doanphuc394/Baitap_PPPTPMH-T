using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace LAB6
{
    public class Form3 : Form
    {
        private string _connStr =
          @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\File_word_baitap\PTPMHDT\Lab_thuc_hanh\LAB6\LAB6\QuanLyBanSach.mdf;Integrated Security=True;Connect Timeout=30";

        private ListView lsvNXB;
        private Label lblTitle, lblMa, lblTen, lblDiaChi;
        private TextBox txtMa, txtTen, txtDiaChi;
        private Button btnThem, btnSua, btnClear, btnRefresh, btnDong;

        public Form3()
        {
            InitializeComponent();
        }

        public Form3(string connStr) : this()
        {
            _connStr = connStr;
        }

        private void InitializeComponent()
        {
            this.Text = "Quản lý Nhà Xuất Bản";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 10F);
            this.ClientSize = new Size(980, 520);

            lblTitle = new Label
            {
                Text = "NHÀ XUẤT BẢN",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.ForestGreen,
                AutoSize = true,
                Location = new Point(20, 15)
            };
            this.Controls.Add(lblTitle);

            // ========== ListView ==========
            lsvNXB = new ListView
            {
                Location = new Point(20, 55),
                Size = new Size(560, 430),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                HideSelection = false
            };
            lsvNXB.Columns.Add("Mã XB", 100, HorizontalAlignment.Left);
            lsvNXB.Columns.Add("Tên NXB", 220, HorizontalAlignment.Left);
            lsvNXB.Columns.Add("Địa chỉ", 220, HorizontalAlignment.Left);
            lsvNXB.SelectedIndexChanged += LsvNXB_SelectedIndexChanged;
            this.Controls.Add(lsvNXB);

            // ========== Khối nhập ==========
            int baseX = 600;
            int baseY = 55;

            lblMa = new Label { Text = "Mã XB", AutoSize = true, Location = new Point(baseX, baseY) };
            txtMa = new TextBox { Location = new Point(baseX + 90, baseY - 4), Width = 260, MaxLength = 10 };

            lblTen = new Label { Text = "Tên NXB", AutoSize = true, Location = new Point(baseX, baseY + 45) };
            txtTen = new TextBox { Location = new Point(baseX + 90, baseY + 41), Width = 260, MaxLength = 100 };

            lblDiaChi = new Label { Text = "Địa chỉ", AutoSize = true, Location = new Point(baseX, baseY + 90) };
            txtDiaChi = new TextBox
            {
                Location = new Point(baseX + 90, baseY + 86),
                Width = 260,
                Height = 120,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                MaxLength = 500
            };

            // 🔹 Nút chức năng
            btnThem = new Button { Text = "Thêm", Location = new Point(baseX + 90, baseY + 220), Width = 110 };
            btnSua = new Button { Text = "Sửa", Location = new Point(baseX + 205, baseY + 220), Width = 110 };
            btnClear = new Button { Text = "Xóa ô nhập", Location = new Point(baseX + 90, baseY + 260), Width = 225 };
            btnRefresh = new Button { Text = "Làm mới DS", Location = new Point(baseX + 90, baseY + 300), Width = 225 };
            btnDong = new Button { Text = "Đóng", Location = new Point(baseX + 90, baseY + 340), Width = 225 };

            btnThem.Click += BtnThem_Click;
            btnSua.Click += BtnSua_Click;
            btnClear.Click += delegate { ClearInputs(); };
            btnRefresh.Click += delegate { LoadList(); };
            btnDong.Click += delegate { this.Close(); };

            this.Controls.AddRange(new Control[]
            {
                lblMa, txtMa, lblTen, txtTen, lblDiaChi, txtDiaChi,
                btnThem, btnSua, btnClear, btnRefresh, btnDong
            });

            this.Load += delegate { LoadList(); txtMa.Focus(); };
        }

        // ====== Khi chọn dòng trong ListView ======
        private void LsvNXB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lsvNXB.SelectedItems.Count == 0) return;
            var it = lsvNXB.SelectedItems[0];
            txtMa.Text = it.SubItems[0].Text;
            txtTen.Text = it.SubItems[1].Text;
            txtDiaChi.Text = it.SubItems[2].Text;
        }

        // ====== Nút Thêm ======
        private void BtnThem_Click(object sender, EventArgs e)
        {
            string ma = txtMa.Text.Trim();
            string ten = txtTen.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();

            if (ma.Length == 0) { MessageBox.Show("Nhập Mã XB."); txtMa.Focus(); return; }
            if (ten.Length == 0) { MessageBox.Show("Nhập Tên NXB."); txtTen.Focus(); return; }

            try
            {
                using (var con = new SqlConnection(_connStr))
                using (var cmd = new SqlCommand("dbo.ThemDuLieu", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@MaXB", SqlDbType.Char, 10).Value = ma;
                    cmd.Parameters.Add("@TenNXB", SqlDbType.NVarChar, 100).Value = ten;
                    cmd.Parameters.Add("@DiaChi", SqlDbType.NVarChar, 500).Value = diaChi;

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        MessageBox.Show("Thêm dữ liệu thành công!", "Thông báo");
                        LoadList();
                        ClearInputs();
                        txtMa.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi thêm: " + ex.Message);
            }
        }

        // ====== Nút Sửa (gọi SP CapNhatThongTin) ======
        private void BtnSua_Click(object sender, EventArgs e)
        {
            string ma = txtMa.Text.Trim();
            string ten = txtTen.Text.Trim();
            string diaChi = txtDiaChi.Text.Trim();

            if (ma == "") { MessageBox.Show("Chọn dòng cần sửa!"); return; }

            try
            {
                using (var con = new SqlConnection(_connStr))
                using (var cmd = new SqlCommand("dbo.CapNhatThongTin", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@MaXB", SqlDbType.Char, 10).Value = ma;
                    cmd.Parameters.Add("@TenNXB", SqlDbType.NVarChar, 100).Value = ten;
                    cmd.Parameters.Add("@DiaChi", SqlDbType.NVarChar, 500).Value = diaChi;

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        MessageBox.Show("Cập nhật thành công!", "Thông báo");
                        LoadList();
                        ClearInputs();
                    }
                    else
                    {
                        MessageBox.Show("Không có bản ghi nào được cập nhật!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message);
            }
        }

        // ====== Load danh sách ======
        private void LoadList()
        {
            try
            {
                using (var con = new SqlConnection(_connStr))
                using (var cmd = new SqlCommand(
                    "SELECT MaXB, TenNXB, ISNULL(DiaChi,N'') FROM dbo.NhaXuatBan ORDER BY MaXB ASC", con))
                {
                    con.Open();
                    using (var rd = cmd.ExecuteReader())
                    {
                        FillList(rd);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách: " + ex.Message);
            }
        }

        private void FillList(SqlDataReader rd)
        {
            lsvNXB.BeginUpdate();
            lsvNXB.Items.Clear();

            while (rd.Read())
            {
                string ma = rd.GetValue(0).ToString().Trim();
                string ten = rd.GetValue(1).ToString();
                string dc = rd.IsDBNull(2) ? "" : rd.GetValue(2).ToString();

                var item = new ListViewItem(ma);
                item.SubItems.Add(ten);
                item.SubItems.Add(dc);
                lsvNXB.Items.Add(item);
            }

            lsvNXB.EndUpdate();
        }

        private void ClearInputs()
        {
            txtMa.Text = "";
            txtTen.Text = "";
            txtDiaChi.Text = "";
        }
    }
}