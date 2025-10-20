using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace QuanAnNhanh
{
    public partial class Form1 : Form
    {
        private DataTable dtOrder;
        private ComboBox cmbBan;
        private DataGridView dgvOrder;
        private Button btnOrder, btnXoa;

        public Form1()
        {

            KhoiTaoGiaoDien();
            KhoiTaoDataTable();
        }

        private void KhoiTaoGiaoDien()
        {
            this.Text = "QUÁN ĂN NHANH ĐOÀN PHÚC";
            this.Size = new Size(620, 700);
            this.StartPosition = FormStartPosition.CenterScreen;

            Panel pnlHeader = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(560, 120),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            PictureBox picLogo = new PictureBox
            {
                Location = new Point(10, 10),
                Size = new Size(180, 100),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White
            };
            picLogo.Load("https://www.foodandwine.com/thmb/pwFie7NRkq4SXMDJU6QKnUKlaoI=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/Ultimate-Veggie-Burgers-FT-Recipe-0821-5d7532c53a924a7298d2175cf1d4219f.jpg");
            this.Controls.Add(picLogo);

            Label lblTenQuan = new Label
            {
                Text = "QUÁN ĂN CỦA PHÚC",
                Location = new Point(200, 35),
                Size = new Size(350, 50),
                Font = new Font("Arial", 20, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Green,
                TextAlign = ContentAlignment.MiddleCenter
            };

            pnlHeader.Controls.Add(picLogo);
            pnlHeader.Controls.Add(lblTenQuan);

            // Label danh sách món ăn
            Label lblDanhSach = new Label
            {
                Text = "Danh sách món ăn:",
                Location = new Point(20, 150),
                Size = new Size(200, 20),
                Font = new Font("Arial", 10, FontStyle.Bold)
            };

            // Tạo các button món ăn - hàng 1
            Button btnComChien = TaoButtonMonAn("Cơm chiên trứng", 40, 180);
            Button btnBanhMy = TaoButtonMonAn("Bánh mỳ ốp la", 190, 180);
            Button btnCoca = TaoButtonMonAn("Coca", 340, 180);
            Button btnLipton = TaoButtonMonAn("Lipton", 470, 180);

            // Hàng 2
            Button btnOcRang = TaoButtonMonAn("Ốc rang muối", 40, 230);
            Button btnKhoaiTay = TaoButtonMonAn("Khoai tây chiên", 190, 230);
            Button btn7Up = TaoButtonMonAn("7 up", 340, 230);
            Button btnCam = TaoButtonMonAn("Cam", 470, 230);

            // Hàng 3
            Button btnMyXao = TaoButtonMonAn("Mỳ xào hải sản", 40, 280);
            Button btnCaVien = TaoButtonMonAn("Cá viên chiên", 190, 280);
            Button btnPepsi = TaoButtonMonAn("Pepsi", 340, 280);
            Button btnCafe = TaoButtonMonAn("Cafe", 470, 280);

            // Hàng 4
            Button btnBuger = TaoButtonMonAn("Buger bò nướng", 40, 330);
            Button btnDuiGa = TaoButtonMonAn("Đùi gà rán", 190, 330);
            Button btnBunBo = TaoButtonMonAn("Bún bò Huế", 340, 330);

            // Panel chức năng
            btnXoa = new Button
            {
                Text = "Xóa",
                Location = new Point(40, 380),
                Size = new Size(80, 35),
                Font = new Font("Arial", 10)
            };
            btnXoa.Click += BtnXoa_Click;

            Label lblChonBan = new Label
            {
                Text = "Chọn bàn:",
                Location = new Point(180, 387),
                Size = new Size(80, 20)
            };

            cmbBan = new ComboBox
            {
                Location = new Point(265, 385),
                Size = new Size(170, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            for (int i = 1; i <= 10; i++)
            {
                cmbBan.Items.Add($"Bàn {i}");
            }
            cmbBan.SelectedIndex = 0;

            btnOrder = new Button
            {
                Text = "Order",
                Location = new Point(490, 380),
                Size = new Size(80, 35),
                Font = new Font("Arial", 10)
            };
            btnOrder.Click += BtnOrder_Click;

            // DataGridView hiển thị order
            dgvOrder = new DataGridView
            {
                Location = new Point(20, 430),
                Size = new Size(560, 210),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            // Thêm controls vào form
            this.Controls.Add(pnlHeader);
            this.Controls.Add(lblDanhSach);
            this.Controls.Add(btnComChien);
            this.Controls.Add(btnBanhMy);
            this.Controls.Add(btnCoca);
            this.Controls.Add(btnLipton);
            this.Controls.Add(btnOcRang);
            this.Controls.Add(btnKhoaiTay);
            this.Controls.Add(btn7Up);
            this.Controls.Add(btnCam);
            this.Controls.Add(btnMyXao);
            this.Controls.Add(btnCaVien);
            this.Controls.Add(btnPepsi);
            this.Controls.Add(btnCafe);
            this.Controls.Add(btnBuger);
            this.Controls.Add(btnDuiGa);
            this.Controls.Add(btnBunBo);
            this.Controls.Add(btnXoa);
            this.Controls.Add(lblChonBan);
            this.Controls.Add(cmbBan);
            this.Controls.Add(btnOrder);
            this.Controls.Add(dgvOrder);
        }

        private Button TaoButtonMonAn(string tenMon, int x, int y)
        {
            Button btn = new Button
            {
                Text = tenMon,
                Location = new Point(x, y),
                Size = new Size(120, 40),
                Font = new Font("Arial", 9)
            };
            btn.Click += BtnMonAn_Click;
            return btn;
        }

        private void KhoiTaoDataTable()
        {
            dtOrder = new DataTable();
            dtOrder.Columns.Add("Tên món", typeof(string));
            dtOrder.Columns.Add("Số lượng", typeof(int));
            dgvOrder.DataSource = dtOrder;
        }

        private void BtnMonAn_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string tenMon = btn.Text;

            // Kiểm tra món đã có trong danh sách chưa
            DataRow[] rows = dtOrder.Select($"[Tên món] = '{tenMon}'");

            if (rows.Length > 0)
            {
                // Tăng số lượng
                rows[0]["Số lượng"] = (int)rows[0]["Số lượng"] + 1;
            }
            else
            {
                // Thêm món mới
                dtOrder.Rows.Add(tenMon, 1);
            }
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (dgvOrder.SelectedRows.Count > 0)
            {
                int index = dgvOrder.SelectedRows[0].Index;
                dtOrder.Rows[index].Delete();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn món cần xóa!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void BtnOrder_Click(object sender, EventArgs e)
        {
            if (dtOrder.Rows.Count == 0)
            {
                MessageBox.Show("Chưa có món nào được chọn!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string tenBan = cmbBan.SelectedItem.ToString();
            string fileName = $"Order_{tenBan}_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

            try
            {
                using (StreamWriter sw = new StreamWriter(fileName))
                {
                    sw.WriteLine("=====================================");
                    sw.WriteLine("     QUÁN ĂN NHANH PHÚC");
                    sw.WriteLine("=====================================");
                    sw.WriteLine($"Bàn: {tenBan}");
                    sw.WriteLine($"Thời gian: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                    sw.WriteLine("-------------------------------------");
                    sw.WriteLine("TÊN MÓN\t\t\tSỐ LƯỢNG");
                    sw.WriteLine("-------------------------------------");

                    foreach (DataRow row in dtOrder.Rows)
                    {
                        sw.WriteLine($"{row["Tên món"]}\t\t{row["Số lượng"]}");
                    }

                    sw.WriteLine("=====================================");
                }

                MessageBox.Show($"Order thành công!\nĐã lưu vào file: {fileName}",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Xóa danh sách sau khi order
                dtOrder.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi ghi file: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}