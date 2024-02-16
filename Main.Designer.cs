namespace Diagram
{
    partial class Main
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ButtonsPanel = new System.Windows.Forms.Panel();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.Button2 = new System.Windows.Forms.Button();
            this.Button3 = new System.Windows.Forms.Button();
            this.Button1 = new System.Windows.Forms.Button();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanelDownMain = new System.Windows.Forms.TableLayoutPanel();
            this.zedGraphControlMainDown = new ZedGraph.ZedGraphControl();
            this.zedGraphControlMainUp = new ZedGraph.ZedGraphControl();
            this.tableLayoutPanelDownRight = new System.Windows.Forms.TableLayoutPanel();
            this.zedGraphControlDownRight2 = new ZedGraph.ZedGraphControl();
            this.zedGraphControlUpRight2 = new ZedGraph.ZedGraphControl();
            this.zedGraphControlUpLeft2 = new ZedGraph.ZedGraphControl();
            this.zedGraphControlDownLeft2 = new ZedGraph.ZedGraphControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.zedGraphControlUpRight1 = new ZedGraph.ZedGraphControl();
            this.zedGraphControlDownRight1 = new ZedGraph.ZedGraphControl();
            this.zedGraphControlDownLeft1 = new ZedGraph.ZedGraphControl();
            this.zedGraphControlUpLeft1 = new ZedGraph.ZedGraphControl();
            this.button4 = new System.Windows.Forms.Button();
            this.ButtonsPanel.SuspendLayout();
            this.tableLayoutPanelMain.SuspendLayout();
            this.tableLayoutPanelDownMain.SuspendLayout();
            this.tableLayoutPanelDownRight.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ButtonsPanel
            // 
            this.ButtonsPanel.AutoSize = true;
            this.ButtonsPanel.Controls.Add(this.button4);
            this.ButtonsPanel.Controls.Add(this.radioButton1);
            this.ButtonsPanel.Controls.Add(this.Button2);
            this.ButtonsPanel.Controls.Add(this.Button3);
            this.ButtonsPanel.Controls.Add(this.Button1);
            this.ButtonsPanel.Location = new System.Drawing.Point(3, 3);
            this.ButtonsPanel.Name = "ButtonsPanel";
            this.ButtonsPanel.Size = new System.Drawing.Size(421, 29);
            this.ButtonsPanel.TabIndex = 6;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(246, 6);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(91, 17);
            this.radioButton1.TabIndex = 4;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Переставить";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // Button2
            // 
            this.Button2.Location = new System.Drawing.Point(84, 3);
            this.Button2.Name = "Button2";
            this.Button2.Size = new System.Drawing.Size(75, 23);
            this.Button2.TabIndex = 2;
            this.Button2.Text = "11-20";
            this.Button2.UseVisualStyleBackColor = true;
            this.Button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // Button3
            // 
            this.Button3.Location = new System.Drawing.Point(165, 3);
            this.Button3.Name = "Button3";
            this.Button3.Size = new System.Drawing.Size(75, 23);
            this.Button3.TabIndex = 3;
            this.Button3.Text = "21-30";
            this.Button3.UseVisualStyleBackColor = true;
            this.Button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // Button1
            // 
            this.Button1.Location = new System.Drawing.Point(3, 3);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(75, 23);
            this.Button1.TabIndex = 1;
            this.Button1.Text = "1-10";
            this.Button1.UseVisualStyleBackColor = true;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.ButtonsPanel, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.tableLayoutPanelDownMain, 0, 1);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 2;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(1540, 845);
            this.tableLayoutPanelMain.TabIndex = 7;
            // 
            // tableLayoutPanelDownMain
            // 
            this.tableLayoutPanelDownMain.ColumnCount = 2;
            this.tableLayoutPanelDownMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelDownMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelDownMain.Controls.Add(this.zedGraphControlMainDown, 0, 1);
            this.tableLayoutPanelDownMain.Controls.Add(this.zedGraphControlMainUp, 0, 0);
            this.tableLayoutPanelDownMain.Controls.Add(this.tableLayoutPanelDownRight, 1, 1);
            this.tableLayoutPanelDownMain.Controls.Add(this.tableLayoutPanel1, 1, 0);
            this.tableLayoutPanelDownMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelDownMain.Location = new System.Drawing.Point(3, 38);
            this.tableLayoutPanelDownMain.Name = "tableLayoutPanelDownMain";
            this.tableLayoutPanelDownMain.RowCount = 2;
            this.tableLayoutPanelDownMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelDownMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelDownMain.Size = new System.Drawing.Size(1534, 804);
            this.tableLayoutPanelDownMain.TabIndex = 7;
            // 
            // zedGraphControlMainDown
            // 
            this.zedGraphControlMainDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraphControlMainDown.Location = new System.Drawing.Point(3, 405);
            this.zedGraphControlMainDown.Name = "zedGraphControlMainDown";
            this.zedGraphControlMainDown.ScrollGrace = 0D;
            this.zedGraphControlMainDown.ScrollMaxX = 0D;
            this.zedGraphControlMainDown.ScrollMaxY = 0D;
            this.zedGraphControlMainDown.ScrollMaxY2 = 0D;
            this.zedGraphControlMainDown.ScrollMinX = 0D;
            this.zedGraphControlMainDown.ScrollMinY = 0D;
            this.zedGraphControlMainDown.ScrollMinY2 = 0D;
            this.zedGraphControlMainDown.Size = new System.Drawing.Size(761, 396);
            this.zedGraphControlMainDown.TabIndex = 10;
            this.zedGraphControlMainDown.UseExtendedPrintDialog = true;
            // 
            // zedGraphControlMainUp
            // 
            this.zedGraphControlMainUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraphControlMainUp.Location = new System.Drawing.Point(3, 3);
            this.zedGraphControlMainUp.Name = "zedGraphControlMainUp";
            this.zedGraphControlMainUp.ScrollGrace = 0D;
            this.zedGraphControlMainUp.ScrollMaxX = 0D;
            this.zedGraphControlMainUp.ScrollMaxY = 0D;
            this.zedGraphControlMainUp.ScrollMaxY2 = 0D;
            this.zedGraphControlMainUp.ScrollMinX = 0D;
            this.zedGraphControlMainUp.ScrollMinY = 0D;
            this.zedGraphControlMainUp.ScrollMinY2 = 0D;
            this.zedGraphControlMainUp.Size = new System.Drawing.Size(761, 396);
            this.zedGraphControlMainUp.TabIndex = 5;
            this.zedGraphControlMainUp.UseExtendedPrintDialog = true;
            // 
            // tableLayoutPanelDownRight
            // 
            this.tableLayoutPanelDownRight.ColumnCount = 2;
            this.tableLayoutPanelDownRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelDownRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelDownRight.Controls.Add(this.zedGraphControlDownRight2, 1, 1);
            this.tableLayoutPanelDownRight.Controls.Add(this.zedGraphControlUpRight2, 1, 0);
            this.tableLayoutPanelDownRight.Controls.Add(this.zedGraphControlUpLeft2, 0, 0);
            this.tableLayoutPanelDownRight.Controls.Add(this.zedGraphControlDownLeft2, 0, 1);
            this.tableLayoutPanelDownRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelDownRight.Location = new System.Drawing.Point(770, 405);
            this.tableLayoutPanelDownRight.Name = "tableLayoutPanelDownRight";
            this.tableLayoutPanelDownRight.RowCount = 2;
            this.tableLayoutPanelDownRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelDownRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelDownRight.Size = new System.Drawing.Size(761, 396);
            this.tableLayoutPanelDownRight.TabIndex = 7;
            // 
            // zedGraphControlDownRight2
            // 
            this.zedGraphControlDownRight2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraphControlDownRight2.Location = new System.Drawing.Point(383, 201);
            this.zedGraphControlDownRight2.Name = "zedGraphControlDownRight2";
            this.zedGraphControlDownRight2.ScrollGrace = 0D;
            this.zedGraphControlDownRight2.ScrollMaxX = 0D;
            this.zedGraphControlDownRight2.ScrollMaxY = 0D;
            this.zedGraphControlDownRight2.ScrollMaxY2 = 0D;
            this.zedGraphControlDownRight2.ScrollMinX = 0D;
            this.zedGraphControlDownRight2.ScrollMinY = 0D;
            this.zedGraphControlDownRight2.ScrollMinY2 = 0D;
            this.zedGraphControlDownRight2.Size = new System.Drawing.Size(375, 192);
            this.zedGraphControlDownRight2.TabIndex = 6;
            this.zedGraphControlDownRight2.UseExtendedPrintDialog = true;
            // 
            // zedGraphControlUpRight2
            // 
            this.zedGraphControlUpRight2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraphControlUpRight2.Location = new System.Drawing.Point(383, 3);
            this.zedGraphControlUpRight2.Name = "zedGraphControlUpRight2";
            this.zedGraphControlUpRight2.ScrollGrace = 0D;
            this.zedGraphControlUpRight2.ScrollMaxX = 0D;
            this.zedGraphControlUpRight2.ScrollMaxY = 0D;
            this.zedGraphControlUpRight2.ScrollMaxY2 = 0D;
            this.zedGraphControlUpRight2.ScrollMinX = 0D;
            this.zedGraphControlUpRight2.ScrollMinY = 0D;
            this.zedGraphControlUpRight2.ScrollMinY2 = 0D;
            this.zedGraphControlUpRight2.Size = new System.Drawing.Size(375, 192);
            this.zedGraphControlUpRight2.TabIndex = 7;
            this.zedGraphControlUpRight2.UseExtendedPrintDialog = true;
            // 
            // zedGraphControlUpLeft2
            // 
            this.zedGraphControlUpLeft2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraphControlUpLeft2.Location = new System.Drawing.Point(3, 3);
            this.zedGraphControlUpLeft2.Name = "zedGraphControlUpLeft2";
            this.zedGraphControlUpLeft2.ScrollGrace = 0D;
            this.zedGraphControlUpLeft2.ScrollMaxX = 0D;
            this.zedGraphControlUpLeft2.ScrollMaxY = 0D;
            this.zedGraphControlUpLeft2.ScrollMaxY2 = 0D;
            this.zedGraphControlUpLeft2.ScrollMinX = 0D;
            this.zedGraphControlUpLeft2.ScrollMinY = 0D;
            this.zedGraphControlUpLeft2.ScrollMinY2 = 0D;
            this.zedGraphControlUpLeft2.Size = new System.Drawing.Size(374, 192);
            this.zedGraphControlUpLeft2.TabIndex = 8;
            this.zedGraphControlUpLeft2.UseExtendedPrintDialog = true;
            // 
            // zedGraphControlDownLeft2
            // 
            this.zedGraphControlDownLeft2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraphControlDownLeft2.Location = new System.Drawing.Point(3, 201);
            this.zedGraphControlDownLeft2.Name = "zedGraphControlDownLeft2";
            this.zedGraphControlDownLeft2.ScrollGrace = 0D;
            this.zedGraphControlDownLeft2.ScrollMaxX = 0D;
            this.zedGraphControlDownLeft2.ScrollMaxY = 0D;
            this.zedGraphControlDownLeft2.ScrollMaxY2 = 0D;
            this.zedGraphControlDownLeft2.ScrollMinX = 0D;
            this.zedGraphControlDownLeft2.ScrollMinY = 0D;
            this.zedGraphControlDownLeft2.ScrollMinY2 = 0D;
            this.zedGraphControlDownLeft2.Size = new System.Drawing.Size(374, 192);
            this.zedGraphControlDownLeft2.TabIndex = 9;
            this.zedGraphControlDownLeft2.UseExtendedPrintDialog = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.zedGraphControlUpRight1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.zedGraphControlDownRight1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.zedGraphControlDownLeft1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.zedGraphControlUpLeft1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(770, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(761, 396);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // zedGraphControlUpRight1
            // 
            this.zedGraphControlUpRight1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraphControlUpRight1.Location = new System.Drawing.Point(383, 3);
            this.zedGraphControlUpRight1.Name = "zedGraphControlUpRight1";
            this.zedGraphControlUpRight1.ScrollGrace = 0D;
            this.zedGraphControlUpRight1.ScrollMaxX = 0D;
            this.zedGraphControlUpRight1.ScrollMaxY = 0D;
            this.zedGraphControlUpRight1.ScrollMaxY2 = 0D;
            this.zedGraphControlUpRight1.ScrollMinX = 0D;
            this.zedGraphControlUpRight1.ScrollMinY = 0D;
            this.zedGraphControlUpRight1.ScrollMinY2 = 0D;
            this.zedGraphControlUpRight1.Size = new System.Drawing.Size(375, 192);
            this.zedGraphControlUpRight1.TabIndex = 8;
            this.zedGraphControlUpRight1.UseExtendedPrintDialog = true;
            // 
            // zedGraphControlDownRight1
            // 
            this.zedGraphControlDownRight1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraphControlDownRight1.Location = new System.Drawing.Point(383, 201);
            this.zedGraphControlDownRight1.Name = "zedGraphControlDownRight1";
            this.zedGraphControlDownRight1.ScrollGrace = 0D;
            this.zedGraphControlDownRight1.ScrollMaxX = 0D;
            this.zedGraphControlDownRight1.ScrollMaxY = 0D;
            this.zedGraphControlDownRight1.ScrollMaxY2 = 0D;
            this.zedGraphControlDownRight1.ScrollMinX = 0D;
            this.zedGraphControlDownRight1.ScrollMinY = 0D;
            this.zedGraphControlDownRight1.ScrollMinY2 = 0D;
            this.zedGraphControlDownRight1.Size = new System.Drawing.Size(375, 192);
            this.zedGraphControlDownRight1.TabIndex = 7;
            this.zedGraphControlDownRight1.UseExtendedPrintDialog = true;
            // 
            // zedGraphControlDownLeft1
            // 
            this.zedGraphControlDownLeft1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraphControlDownLeft1.Location = new System.Drawing.Point(3, 201);
            this.zedGraphControlDownLeft1.Name = "zedGraphControlDownLeft1";
            this.zedGraphControlDownLeft1.ScrollGrace = 0D;
            this.zedGraphControlDownLeft1.ScrollMaxX = 0D;
            this.zedGraphControlDownLeft1.ScrollMaxY = 0D;
            this.zedGraphControlDownLeft1.ScrollMaxY2 = 0D;
            this.zedGraphControlDownLeft1.ScrollMinX = 0D;
            this.zedGraphControlDownLeft1.ScrollMinY = 0D;
            this.zedGraphControlDownLeft1.ScrollMinY2 = 0D;
            this.zedGraphControlDownLeft1.Size = new System.Drawing.Size(374, 192);
            this.zedGraphControlDownLeft1.TabIndex = 9;
            this.zedGraphControlDownLeft1.UseExtendedPrintDialog = true;
            // 
            // zedGraphControlUpLeft1
            // 
            this.zedGraphControlUpLeft1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraphControlUpLeft1.Location = new System.Drawing.Point(3, 3);
            this.zedGraphControlUpLeft1.Name = "zedGraphControlUpLeft1";
            this.zedGraphControlUpLeft1.ScrollGrace = 0D;
            this.zedGraphControlUpLeft1.ScrollMaxX = 0D;
            this.zedGraphControlUpLeft1.ScrollMaxY = 0D;
            this.zedGraphControlUpLeft1.ScrollMaxY2 = 0D;
            this.zedGraphControlUpLeft1.ScrollMinX = 0D;
            this.zedGraphControlUpLeft1.ScrollMinY = 0D;
            this.zedGraphControlUpLeft1.ScrollMinY2 = 0D;
            this.zedGraphControlUpLeft1.Size = new System.Drawing.Size(374, 192);
            this.zedGraphControlUpLeft1.TabIndex = 6;
            this.zedGraphControlUpLeft1.UseExtendedPrintDialog = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(343, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 5;
            this.button4.Text = "Выборка";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1540, 845);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ButtonsPanel.ResumeLayout(false);
            this.ButtonsPanel.PerformLayout();
            this.tableLayoutPanelMain.ResumeLayout(false);
            this.tableLayoutPanelMain.PerformLayout();
            this.tableLayoutPanelDownMain.ResumeLayout(false);
            this.tableLayoutPanelDownRight.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Panel ButtonsPanel;
        private System.Windows.Forms.Button Button2;
        private System.Windows.Forms.Button Button3;
        private System.Windows.Forms.Button Button1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDownMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDownRight;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RadioButton radioButton1;
        private ZedGraph.ZedGraphControl zedGraphControlMainDown;
        private ZedGraph.ZedGraphControl zedGraphControlMainUp;
        private ZedGraph.ZedGraphControl zedGraphControlDownRight2;
        private ZedGraph.ZedGraphControl zedGraphControlUpRight2;
        private ZedGraph.ZedGraphControl zedGraphControlUpLeft2;
        private ZedGraph.ZedGraphControl zedGraphControlDownLeft2;
        private ZedGraph.ZedGraphControl zedGraphControlUpRight1;
        private ZedGraph.ZedGraphControl zedGraphControlDownRight1;
        private ZedGraph.ZedGraphControl zedGraphControlDownLeft1;
        private ZedGraph.ZedGraphControl zedGraphControlUpLeft1;
        private System.Windows.Forms.Button button4;
    }
}

