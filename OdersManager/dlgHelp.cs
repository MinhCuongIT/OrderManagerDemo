using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OdersManager
{
    /// <summary>
    /// Màn hình hiển thị trợ giúp
    /// </summary>
    public partial class dlgHelp : Form
    {
        #region Variables
        /// <summary>
        /// Lưu trữ ID hiện tại được chọn
        /// </summary>
        private string id;
        /// <summary>
        /// Lưu trữ Name hiện tại được chọn
        /// </summary>
        private string name;
        #endregion

        #region Properties
        /// <summary>
        /// Lấy giá trị của Item ID
        /// </summary>
        public string GetIDItem
        {
            get
            {
                return id;
            }
        }
        /// <summary>
        /// Lấy giá trị của Item Name
        /// </summary>
        public string GetNameItem
        {
            get
            {
                return name;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Hàm dựng cho dialog Help
        /// </summary>
        /// <param name="dtSource"></param>
        public dlgHelp(DataTable dtSource)
        {
            // Kiểm tra giá trị đầu vào của hàm
            if (dtSource == null)
            {
                return;
            }
            //Khởi tạo các biến
            this.id = string.Empty;
            this.name = string.Empty;
            //Khởi tạo control mặc định của chương trình
            InitializeComponent();
            //Gán dữ liệu cho grid
            this.dgvHelp.DataSource = dtSource.DefaultView;
            //Định dạng lại các cột trong grid
            FormatGrid();
        }

        



        #endregion

        #region Events
        /// <summary>
        /// Xử lí khi một cell được double click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvHelp_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //Kiểm tra điều kiện thực thi, nếu không thỏa thì dừng lại
            if (e.ColumnIndex<0 || e.RowIndex < 0 )
            {
                return;
            }
            //Trường hợp còn lại thì trả về kết quả
            this.id = this.dgvHelp.Rows[e.RowIndex].Cells[0].ToString();
            this.name = this.dgvHelp.Rows[e.RowIndex].Cells[1].ToString();
            //Set kết quả của Dialog và đóng cửa sổ
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        /// <summary>
        /// Xử lý sự kiện Load dialoag Help
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dlgHelp_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Xử lý sự kiện Click button Close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = DialogResult.Cancel;
        }
        /// <summary>
        /// Xử lý sự kiện Click button Process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProcess_Click(object sender, EventArgs e)
        {
            this.id = this.dgvHelp.Rows[this.dgvHelp.CurrentRow.Index].Cells[0].ToString();
            this.name = this.dgvHelp.Rows[this.dgvHelp.CurrentRow.Index].Cells[1].ToString();
            this.Close();
            this.DialogResult = DialogResult.OK;
        }
        /// <summary>
        /// Xử lý sự kiện dùng để bắt phím nóng F2 và F8
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dlgHelp_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F2:
                    //thực hiện chức năng như là btnCloseClick
                    btnClose_Click(null, null);
                    break;
                case Keys.F8:
                    //Thực hiện như chức năng btnProcessClick
                    btnProcess_Click(null, null);
                    break;
                default:
                    //Không thực hiện gì
                    break;
            }
        }


        #endregion

        #region Method
        private void FormatGrid()
        {
            //Ẩn dòng tiêu đề
            this.dgvHelp.RowHeadersVisible = false;
            //Định dạng lại dòng ID
            this.dgvHelp.Columns[0].DefaultCellStyle.BackColor = Color.LightCyan;
            this.dgvHelp.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;

            //Định dạng lại dòng Name
            this.dgvHelp.Columns[1].DefaultCellStyle.BackColor = Color.LightCyan;
            this.dgvHelp.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;
            //Những dòng không cần thiết thì ẩn đi
            for (int i = 2; i < this.dgvHelp.ColumnCount; i++)
            {
                this.dgvHelp.Columns[i].Visible = false;
            }
            //Thiết lập chế độ hiển thị
            this.dgvHelp.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
        }
        #endregion
    }
}
