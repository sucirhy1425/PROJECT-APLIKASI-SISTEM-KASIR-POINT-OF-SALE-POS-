using System.ComponentModel;
using System.Windows.Forms;

namespace Kasir
{
    partial class Dashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Dashboard));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.metroLabel24 = new MetroFramework.Controls.MetroLabel();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.gridBarangTerlaris = new MetroFramework.Controls.MetroGrid();
            this.lblProfit = new MetroFramework.Controls.MetroLabel();
            this.lblTotalTransaksi = new MetroFramework.Controls.MetroLabel();
            this.lblTotalStock = new MetroFramework.Controls.MetroLabel();
            this.lblTotalBarang = new MetroFramework.Controls.MetroLabel();
            this.metroLabel19 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel17 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel18 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel16 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridBarangTerlaris)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // metroLabel24
            // 
            this.metroLabel24.AutoSize = true;
            this.metroLabel24.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel24.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel24.Location = new System.Drawing.Point(23, 30);
            this.metroLabel24.Name = "metroLabel24";
            this.metroLabel24.Size = new System.Drawing.Size(125, 25);
            this.metroLabel24.TabIndex = 1;
            this.metroLabel24.Text = "Barang Terlaris";
            this.metroLabel24.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.metroLabel24.Click += new System.EventHandler(this.metroLabel24_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.gridBarangTerlaris);
            this.groupBox7.Location = new System.Drawing.Point(23, 79);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(596, 253);
            this.groupBox7.TabIndex = 3;
            this.groupBox7.TabStop = false;
            // 
            // gridBarangTerlaris
            // 
            this.gridBarangTerlaris.AllowUserToAddRows = false;
            this.gridBarangTerlaris.AllowUserToDeleteRows = false;
            this.gridBarangTerlaris.AllowUserToResizeRows = false;
            this.gridBarangTerlaris.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridBarangTerlaris.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.gridBarangTerlaris.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridBarangTerlaris.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridBarangTerlaris.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.gridBarangTerlaris.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(188)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(219)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridBarangTerlaris.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridBarangTerlaris.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(165)))), ((int)(((byte)(219)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridBarangTerlaris.DefaultCellStyle = dataGridViewCellStyle2;
            this.gridBarangTerlaris.EnableHeadersVisualStyles = false;
            this.gridBarangTerlaris.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.gridBarangTerlaris.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridBarangTerlaris.Location = new System.Drawing.Point(6, 10);
            this.gridBarangTerlaris.MultiSelect = false;
            this.gridBarangTerlaris.Name = "gridBarangTerlaris";
            this.gridBarangTerlaris.ReadOnly = true;
            this.gridBarangTerlaris.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridBarangTerlaris.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.gridBarangTerlaris.RowHeadersVisible = false;
            this.gridBarangTerlaris.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gridBarangTerlaris.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridBarangTerlaris.Size = new System.Drawing.Size(584, 237);
            this.gridBarangTerlaris.TabIndex = 0;
            // 
            // lblProfit
            // 
            this.lblProfit.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.lblProfit.Location = new System.Drawing.Point(462, 61);
            this.lblProfit.Name = "lblProfit";
            this.lblProfit.Size = new System.Drawing.Size(111, 19);
            this.lblProfit.TabIndex = 27;
            this.lblProfit.Text = "?";
            this.lblProfit.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblTotalTransaksi
            // 
            this.lblTotalTransaksi.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.lblTotalTransaksi.Location = new System.Drawing.Point(462, 29);
            this.lblTotalTransaksi.Name = "lblTotalTransaksi";
            this.lblTotalTransaksi.Size = new System.Drawing.Size(111, 19);
            this.lblTotalTransaksi.TabIndex = 26;
            this.lblTotalTransaksi.Text = "?";
            this.lblTotalTransaksi.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblTotalStock
            // 
            this.lblTotalStock.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.lblTotalStock.Location = new System.Drawing.Point(99, 61);
            this.lblTotalStock.Name = "lblTotalStock";
            this.lblTotalStock.Size = new System.Drawing.Size(111, 19);
            this.lblTotalStock.TabIndex = 25;
            this.lblTotalStock.Text = "?";
            this.lblTotalStock.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblTotalBarang
            // 
            this.lblTotalBarang.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            this.lblTotalBarang.Location = new System.Drawing.Point(99, 29);
            this.lblTotalBarang.Name = "lblTotalBarang";
            this.lblTotalBarang.Size = new System.Drawing.Size(111, 19);
            this.lblTotalBarang.TabIndex = 24;
            this.lblTotalBarang.Text = "?";
            this.lblTotalBarang.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // metroLabel19
            // 
            this.metroLabel19.AutoSize = true;
            this.metroLabel19.Location = new System.Drawing.Point(369, 61);
            this.metroLabel19.Name = "metroLabel19";
            this.metroLabel19.Size = new System.Drawing.Size(75, 19);
            this.metroLabel19.TabIndex = 21;
            this.metroLabel19.Text = "Total Profit:";
            // 
            // metroLabel17
            // 
            this.metroLabel17.AutoSize = true;
            this.metroLabel17.Location = new System.Drawing.Point(6, 61);
            this.metroLabel17.Name = "metroLabel17";
            this.metroLabel17.Size = new System.Drawing.Size(74, 19);
            this.metroLabel17.TabIndex = 20;
            this.metroLabel17.Text = "Total Stock:";
            // 
            // metroLabel18
            // 
            this.metroLabel18.AutoSize = true;
            this.metroLabel18.Location = new System.Drawing.Point(369, 29);
            this.metroLabel18.Name = "metroLabel18";
            this.metroLabel18.Size = new System.Drawing.Size(93, 19);
            this.metroLabel18.TabIndex = 19;
            this.metroLabel18.Text = "Total Transaksi:";
            // 
            // metroLabel16
            // 
            this.metroLabel16.AutoSize = true;
            this.metroLabel16.Location = new System.Drawing.Point(6, 29);
            this.metroLabel16.Name = "metroLabel16";
            this.metroLabel16.Size = new System.Drawing.Size(85, 19);
            this.metroLabel16.TabIndex = 17;
            this.metroLabel16.Text = "Total Barang:";
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.FontSize = MetroFramework.MetroLabelSize.Tall;
            this.metroLabel1.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel1.Location = new System.Drawing.Point(29, 352);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(97, 25);
            this.metroLabel1.TabIndex = 26;
            this.metroLabel1.Text = "Detail Stok";
            this.metroLabel1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.metroLabel16);
            this.groupBox2.Controls.Add(this.metroLabel17);
            this.groupBox2.Controls.Add(this.lblProfit);
            this.groupBox2.Controls.Add(this.lblTotalBarang);
            this.groupBox2.Controls.Add(this.lblTotalTransaksi);
            this.groupBox2.Controls.Add(this.lblTotalStock);
            this.groupBox2.Controls.Add(this.metroLabel18);
            this.groupBox2.Controls.Add(this.metroLabel19);
            this.groupBox2.Location = new System.Drawing.Point(29, 381);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(590, 100);
            this.groupBox2.TabIndex = 28;
            this.groupBox2.TabStop = false;
            // 
            // Dashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(639, 502);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.metroLabel24);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Dashboard";
            this.Style = MetroFramework.MetroColorStyle.Lime;
            this.Load += new System.EventHandler(this.Dashboard_Load);
            this.Resize += new System.EventHandler(this.Dashboard_Resize);
            this.groupBox7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridBarangTerlaris)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
        private MetroFramework.Controls.MetroLabel metroLabel24;
        private System.Windows.Forms.GroupBox groupBox7;
        private MetroFramework.Controls.MetroLabel lblProfit;
        private MetroFramework.Controls.MetroLabel lblTotalTransaksi;
        private MetroFramework.Controls.MetroLabel lblTotalStock;
        private MetroFramework.Controls.MetroLabel lblTotalBarang;
        private MetroFramework.Controls.MetroLabel metroLabel19;
        private MetroFramework.Controls.MetroLabel metroLabel17;
        private MetroFramework.Controls.MetroLabel metroLabel18;
        private MetroFramework.Controls.MetroLabel metroLabel16;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private GroupBox groupBox2;
        private MetroFramework.Controls.MetroGrid gridBarangTerlaris;
    }
}
