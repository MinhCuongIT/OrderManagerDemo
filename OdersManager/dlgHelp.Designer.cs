namespace OdersManager
{
    partial class dlgHelp
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnClose = new System.Windows.Forms.Button();
            this.btnProcess = new System.Windows.Forms.Button();
            this.dgvHelp = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHelp)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(0, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(86, 39);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "F2\r\nClose";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnProcess
            // 
            this.btnProcess.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProcess.Location = new System.Drawing.Point(85, 0);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(86, 39);
            this.btnProcess.TabIndex = 0;
            this.btnProcess.Text = "F8\r\nProcess";
            this.btnProcess.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // dgvHelp
            // 
            this.dgvHelp.AllowUserToAddRows = false;
            this.dgvHelp.AllowUserToDeleteRows = false;
            this.dgvHelp.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHelp.Location = new System.Drawing.Point(13, 46);
            this.dgvHelp.Name = "dgvHelp";
            this.dgvHelp.ReadOnly = true;
            this.dgvHelp.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHelp.Size = new System.Drawing.Size(259, 403);
            this.dgvHelp.TabIndex = 1;
            this.dgvHelp.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvHelp_CellDoubleClick);
            // 
            // dlgHelp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 461);
            this.Controls.Add(this.dgvHelp);
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "dlgHelp";
            this.ShowIcon = false;
            this.Text = "Help";
            this.Load += new System.EventHandler(this.dlgHelp_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dlgHelp_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHelp)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.DataGridView dgvHelp;
    }
}