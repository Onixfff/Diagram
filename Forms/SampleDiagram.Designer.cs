namespace Diagram
{
    partial class SampleDiagram
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SampleDiagram));
            this.comboBoxRoms = new System.Windows.Forms.ComboBox();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.comboBoxPreparationStartDates = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.zedGraphControlFilter = new ZedGraph.ZedGraphControl();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxRoms
            // 
            this.comboBoxRoms.FormattingEnabled = true;
            this.comboBoxRoms.Location = new System.Drawing.Point(217, 7);
            this.comboBoxRoms.Name = "comboBoxRoms";
            this.comboBoxRoms.Size = new System.Drawing.Size(121, 21);
            this.comboBoxRoms.TabIndex = 2;
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Location = new System.Drawing.Point(11, 7);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker.TabIndex = 3;
            // 
            // comboBoxPreparationStartDates
            // 
            this.comboBoxPreparationStartDates.FormattingEnabled = true;
            this.comboBoxPreparationStartDates.Location = new System.Drawing.Point(425, 7);
            this.comboBoxPreparationStartDates.Name = "comboBoxPreparationStartDates";
            this.comboBoxPreparationStartDates.Size = new System.Drawing.Size(146, 21);
            this.comboBoxPreparationStartDates.TabIndex = 4;
            this.comboBoxPreparationStartDates.SelectedIndexChanged += new System.EventHandler(this.comboBoxPreparationStartDates_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.dateTimePicker);
            this.panel1.Controls.Add(this.comboBoxPreparationStartDates);
            this.panel1.Controls.Add(this.comboBoxRoms);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1728, 33);
            this.panel1.TabIndex = 5;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(344, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Выборка";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // zedGraphControlFilter
            // 
            this.zedGraphControlFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraphControlFilter.Location = new System.Drawing.Point(0, 33);
            this.zedGraphControlFilter.Name = "zedGraphControlFilter";
            this.zedGraphControlFilter.ScrollGrace = 0D;
            this.zedGraphControlFilter.ScrollMaxX = 0D;
            this.zedGraphControlFilter.ScrollMaxY = 0D;
            this.zedGraphControlFilter.ScrollMaxY2 = 0D;
            this.zedGraphControlFilter.ScrollMinX = 0D;
            this.zedGraphControlFilter.ScrollMinY = 0D;
            this.zedGraphControlFilter.ScrollMinY2 = 0D;
            this.zedGraphControlFilter.Size = new System.Drawing.Size(1728, 812);
            this.zedGraphControlFilter.TabIndex = 6;
            this.zedGraphControlFilter.UseExtendedPrintDialog = true;
            // 
            // SampleDiagram
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1728, 845);
            this.Controls.Add(this.zedGraphControlFilter);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SampleDiagram";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Выборочная форма";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox comboBoxRoms;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private System.Windows.Forms.ComboBox comboBoxPreparationStartDates;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private ZedGraph.ZedGraphControl zedGraphControlFilter;
    }
}