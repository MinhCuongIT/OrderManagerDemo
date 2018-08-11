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

namespace OdersManager
{
    /// <summary>
    /// Màn hình thêm xóa sửa thông tin của Order Details theo OrderID
    /// Người tạo: Trần Minh Cường
    /// Ngày Tạo: 9/8/2018
    /// </summary>
    public partial class UpdateDeleteOrderDetails : Form
    {
        #region Variables
        /// <summary>
        /// connection string kết nối đến database
        /// </summary>
        private string stringConnection;
        /// <summary>
        /// Lưu giữ chế độ Delete hoặc Update
        /// </summary>
        private bool isDeleteMode;
        /// <summary>
        /// Driver kết nối đến database
        /// </summary>
        private SqlConnection scnNorthWind;
        /// <summary>
        /// Adapter cho dữ liệu Order Details
        /// </summary>
        private SqlDataAdapter sdaOrderDetails;
        /// <summary>
        /// Adapter cho dữ liệu Orders
        /// </summary>
        private SqlDataAdapter sdaOrders;
        /// <summary>
        /// Adapter cho dữ liệu Products
        /// </summary>
        private SqlDataAdapter sdaProducts;
        /// <summary>
        /// Adapter cho dữ liệu Customers
        /// </summary>
        private SqlDataAdapter sdaCustomers;
        /// <summary>
        /// Adapter cho dữ liệu Employees
        /// </summary>
        private SqlDataAdapter sdaEmployees;
        /// <summary>
        /// Thực thi các command trên Orders
        /// </summary>
        private SqlCommandBuilder scbOrders;
        /// <summary>
        /// Thực thi các command trên Order Details
        /// </summary>
        private SqlCommandBuilder scbOrderDetails;
        /// <summary>
        /// Đối tượng lưu giữ tất cả table để làm việc cho toàn chương trình
        /// </summary>
        private DataSet dsAllData;
        /// <summary>
        /// Đối tượng lưu giữ OrderID hiện tại
        /// </summary>
        DataRow drCurrentOrder;
        /// <summary>
        /// Đối tượng hiển thị các lỗi lên màn hình
        /// </summary>
        ErrorProvider errorProvider;
        #endregion

        #region Properties
        /// <summary>
        /// Lưu giữ Control cuối cùng được kích hoạt
        /// </summary>
        private Control lastControlActive;
        #endregion

        #region Constructor
        /// <summary>
        /// Hàm dựng cho form chính
        /// </summary>
        public UpdateDeleteOrderDetails()
        {
            //phương thức khởi tạo các control mặc định
            InitializeComponent();
            //Khởi tạo Mode
            this.isDeleteMode = false;
            //Khởi tạo ConnectionString
            this.stringConnection = @"Data Source=.\SQLEXPRESS;Initial Catalog=Northwind;Integrated Security=True";
            //Khởi tạo Driver
            this.scnNorthWind = new SqlConnection(stringConnection);
            //Khởi tạo các Adapter
            this.sdaCustomers = new SqlDataAdapter("SELECT CustomerID, ContactName FROM Customers",this.scnNorthWind);
            this.sdaEmployees = new SqlDataAdapter("SELECT EmployeeID, LastName +' '+ FirstName as EmployeeName FROM Employees", this.scnNorthWind);
            this.sdaOrders = new SqlDataAdapter("SELECT OrderID, CustomerID, EmployeeID FROM Orders", this.scnNorthWind);
            this.sdaProducts = new SqlDataAdapter("SELECT ProductID, ProductName FROM Products", this.scnNorthWind);
            this.sdaOrderDetails = new SqlDataAdapter("SELECT OrderID, ProductID, UnitPrice, Quantity, Discount FROM [Order Details] WHERE OrderID = @ID", this.scnNorthWind);
            this.sdaOrderDetails.SelectCommand.Parameters.AddWithValue("@ID", DBNull.Value);
            //Khởi tạo DataSet
           this. dsAllData = new DataSet();
            //Khởi tạo lastControlActive
            this.lastControlActive = new Control();
            //Khởi tạo errorProvider
            errorProvider = new ErrorProvider();
            //Thêm sự kiện cho các Control trong chương trình
            foreach (Control control in this.Controls)
            {
                if (!(control is Button))
                {
                    control.Enter += Control_Enter;
                }
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Kiểm tra tính hợp lệ của dữ liệu trong txtOrderID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtOrderID_Validating(object sender, CancelEventArgs e)
        {
            //Nếu dữ liệu trong txtOrderID là rỗng thì dừng lại
            if (string.IsNullOrEmpty(this.txtOrderID.Text))
            {
                return;
            }
        }
        /// <summary>
        /// Gán sự kiện cho các control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Control_Enter(object sender, EventArgs e)
        {
            this.lastControlActive = sender as Control;
        }
        /// <summary>
        /// Xử lý sự kiện khi form chính được load lên
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateDeleteOrderDetails_Load(object sender, EventArgs e)
        {
            //Tải dữ liệu vào dataset
            LoadData();
            //Thêm các ràng buộc cho các bảng
            AddRelations();
            //Thêm các cột cần thiết cho datagridview
            AddColumns();
            //Thêm sự kiện cho table trong dataset để cập nhật total
            AddDataTableEvents();
            //Gán dữ liệu cho grid
            BindGridData();
            //Định dạng lại các control và thêm tính năng autocomplete
            FormatControls();
            //Xóa các control
            ClearControlData();
            //Set focus cho IDorderID
            this.txtOrderID.Focus();
        }
        #endregion

        #region Method
        /// <summary>
        /// Xóa các control và enable những control cần thiết
        /// </summary>
        private void ClearControlData()
        {
            //Xóa dữ liệu trên các texbox
            this.txtOrderID.Clear();
            this.txtCustomerID.Clear();
            this.txtCustomerName.Clear();
            this.txtEmployeeID.Clear();
            this.txtEmployeeName.Clear();
            //Xóa bỏ đối tượng lưu trữ order hiện tại
            this.drCurrentOrder = null;
            //Xóa dữ liệu trong table Order Details trong dataset
            this.dsAllData.Tables["Order Details"].Clear();
            //Thiết lập trạng thái cho các controls
            SetStateControl(true);
        }
        /// <summary>
        /// Xử lí định dạng và thêm chức năng autocomplete cho các Textbox
        /// </summary>
        private void FormatControls()
        {
            //Thêm tính năng autocomlete cho txtOrderID
            this.txtOrderID.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.txtOrderID.AutoCompleteSource = AutoCompleteSource.CustomSource;
            this.txtOrderID.AutoCompleteCustomSource = GetAutoCompleteStringCollection("Orders", "OrderID");

            //Thêm tính năng autocomlete cho txtCustomerID
            this.txtCustomerID.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.txtCustomerID.AutoCompleteSource = AutoCompleteSource.CustomSource;
            this.txtCustomerID.AutoCompleteCustomSource = GetAutoCompleteStringCollection("Customers", "CustomerID");

            //Thêm tính năng autocomlete cho txtEmployeeID
            this.txtEmployeeID.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.txtEmployeeID.AutoCompleteSource = AutoCompleteSource.CustomSource;
            this.txtEmployeeID.AutoCompleteCustomSource = GetAutoCompleteStringCollection("Employees", "EmployeeID");

        }
        /// <summary>
        /// Bind dữ liệu từ Dataset 
        /// </summary>
        private void BindGridData()
        {
            //Gán dữ liệu cho datasource của grid
            this.dgvOrderDetails.DataSource = this.dsAllData.Tables["Order Details"].DefaultView;
        }
        /// <summary>
        /// Thêm sự kiện rowchanged cho table Order Details để cập nhập total
        /// </summary>
        private void AddDataTableEvents()
        {
        }
        /// <summary>
        /// Xử lí thêm cột vào datagridview
        /// </summary>
        private void AddColumns()
        {
            //Thêm cột số thứ tự cho Grid
            this.dgvOrderDetails.Columns.Add("No.", "No.");
            //Thêm cột ProductName cho Table Order Details
            this.dsAllData.Tables["Order Details"].Columns.Add("ProductName", typeof(string), "parent(Products_OrderDetails).ProductName");
            //Thêm cột Sum cho Table Details
            this.dsAllData.Tables["Order Details"].Columns.Add("Sum", typeof(decimal), "UnitPrice*Quantity*(1-Discount)");
        }
        /// <summary>
        /// Xử lí tải dữ liệu vào dataset
        /// </summary>
        private void LoadData()
        {
            //Đổ dữ liệu vào các table trong dataset dsAllData
            this.sdaCustomers.Fill(this.dsAllData, "Customers");
            this.sdaEmployees.Fill(this.dsAllData, "Employees");
            this.sdaOrderDetails.Fill(this.dsAllData, "Order Details");
            this.sdaOrders.Fill(this.dsAllData, "Orders");
            this.sdaProducts.Fill(this.dsAllData, "Products");
        }
        /// <summary>
        /// Thêm các ràng buộc cho các bảng trong dataset, Mục đích là để thêm xóa sửa tự động
        /// </summary>
        private void AddRelations()
        {
            //Tao ra cac relation
            DataRelation drProducts_OrderDetails = new DataRelation("Products_OrderDetails", this.dsAllData.Tables["Products"].Columns["ProductID"], this.dsAllData.Tables["Order Details"].Columns["ProductID"]);
            DataRelation drOrders_OrderDetails = new DataRelation("Orders_OrderDetails", this.dsAllData.Tables["Orders"].Columns["OrderID"], this.dsAllData.Tables["Order Details"].Columns["OrderID"]);
            //Thêm các Relation vào dataset
            this.dsAllData.Relations.Add(drProducts_OrderDetails);
            this.dsAllData.Relations.Add(drOrders_OrderDetails);
        }
        /// <summary>
        /// Set trạng thái cho các control phụ thuộc vào TxtOrderID enable or disable
        /// </summary>
        /// <param name="isEnableOrderID"></param>
        private void SetStateControl(bool isEnableOrderID)
        {
            //Set Enable và backColor cho các txtCustomerID và txtEmployeeID
            this.txtOrderID.Enabled = isEnableOrderID;
            this.txtCustomerID.Enabled = this.isDeleteMode ? false : true;
            this.txtCustomerID.BackColor = this.isDeleteMode ? Color.LightCyan : Color.White;
            this.txtEmployeeID.Enabled = this.isDeleteMode ? false : true;
            this.txtEmployeeID.BackColor = this.isDeleteMode ? Color.LightCyan : Color.White;
            //Set enable cho datagridview
            this.dgvOrderDetails.Enabled = !this.isDeleteMode && !isEnableOrderID;
        }
        /// <summary>
        /// Xử lí tính toán total
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        private decimal CalculateTotalDetails(string orderID)
        {
            decimal total = 0;
            DataRow[] drRows = this.dsAllData.Tables["Order Details"].Select("OrderID = " + orderID, "OrderID ASC", DataViewRowState.CurrentRows);
            foreach (DataRow row in drRows)
            {
                total += Convert.ToDecimal(row["Sum"]);
            }
            return total;
        }
        /// <summary>
        /// Set giá trị mặc định để khi thêm dòng mới không bị lỗi
        /// </summary>
        private void SetDefaultValueOrderDetails()
        {
            this.dsAllData.Tables["Order Details"].Columns["OrderID"].DefaultValue = this.txtOrderID.Text;
            this.dsAllData.Tables["Order Details"].Columns["ProductName"].DefaultValue = string.Empty;
            this.dsAllData.Tables["Order Details"].Columns["UnitPrice"].DefaultValue = 0;
            this.dsAllData.Tables["Order Details"].Columns["Quantity"].DefaultValue = 1;
            this.dsAllData.Tables["Order Details"].Columns["Discount"].DefaultValue = 0;
        }
        /// <summary>
        /// Lấy dữ liệu làm AutoComplete cho các Textbox
        /// </summary>
        /// <param name="table">Tên bảng cần lấy</param>
        /// <param name="column">Tên cột cần lấy</param>
        /// <returns></returns>
        private AutoCompleteStringCollection GetAutoCompleteStringCollection(string table, string column)
        {
            AutoCompleteStringCollection acsc = new AutoCompleteStringCollection();
            for (int i = 0; i < this.dsAllData.Tables[table].Rows.Count; i++)
            {
                acsc.Add(this.dsAllData.Tables[table].Rows[i][column].ToString());
            }
            return acsc;
        }
        #endregion
    }
}
