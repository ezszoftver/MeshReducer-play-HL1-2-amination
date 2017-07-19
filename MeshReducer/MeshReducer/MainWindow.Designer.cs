namespace MeshReducer
{
    partial class MainWindow
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabOBJ = new System.Windows.Forms.TabPage();
            this.label_save_obj = new System.Windows.Forms.Label();
            this.progressBar_save_obj = new System.Windows.Forms.ProgressBar();
            this.label_load_obj = new System.Windows.Forms.Label();
            this.progressBar_load_obj = new System.Windows.Forms.ProgressBar();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tabSMD = new System.Windows.Forms.TabPage();
            this.tabOptions = new System.Windows.Forms.TabPage();
            this.groupBox_options_reduce = new System.Windows.Forms.GroupBox();
            this.button_calc = new System.Windows.Forms.Button();
            this.label_reduce = new System.Windows.Forms.Label();
            this.trackBar_reduce_percent = new System.Windows.Forms.TrackBar();
            this.progressBar_reduce = new System.Windows.Forms.ProgressBar();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.trackBar_rotate_z = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.trackBar_rotate_y = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.trackBar_rotate_x = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabOBJ.SuspendLayout();
            this.tabOptions.SuspendLayout();
            this.groupBox_options_reduce.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_reduce_percent)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_rotate_z)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_rotate_y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_rotate_x)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 1;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(784, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabOBJ);
            this.tabControl1.Controls.Add(this.tabSMD);
            this.tabControl1.Controls.Add(this.tabOptions);
            this.tabControl1.Location = new System.Drawing.Point(12, 351);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(760, 200);
            this.tabControl1.TabIndex = 1;
            // 
            // tabOBJ
            // 
            this.tabOBJ.Controls.Add(this.label_save_obj);
            this.tabOBJ.Controls.Add(this.progressBar_save_obj);
            this.tabOBJ.Controls.Add(this.label_load_obj);
            this.tabOBJ.Controls.Add(this.progressBar_load_obj);
            this.tabOBJ.Controls.Add(this.button2);
            this.tabOBJ.Controls.Add(this.button1);
            this.tabOBJ.Location = new System.Drawing.Point(4, 22);
            this.tabOBJ.Name = "tabOBJ";
            this.tabOBJ.Padding = new System.Windows.Forms.Padding(3);
            this.tabOBJ.Size = new System.Drawing.Size(752, 174);
            this.tabOBJ.TabIndex = 0;
            this.tabOBJ.Text = "OBJ File";
            this.tabOBJ.UseVisualStyleBackColor = true;
            // 
            // label_save_obj
            // 
            this.label_save_obj.AutoSize = true;
            this.label_save_obj.Location = new System.Drawing.Point(415, 48);
            this.label_save_obj.Name = "label_save_obj";
            this.label_save_obj.Size = new System.Drawing.Size(24, 13);
            this.label_save_obj.TabIndex = 5;
            this.label_save_obj.Text = "0 %";
            // 
            // progressBar_save_obj
            // 
            this.progressBar_save_obj.Location = new System.Drawing.Point(103, 43);
            this.progressBar_save_obj.Name = "progressBar_save_obj";
            this.progressBar_save_obj.Size = new System.Drawing.Size(643, 23);
            this.progressBar_save_obj.TabIndex = 4;
            // 
            // label_load_obj
            // 
            this.label_load_obj.AutoSize = true;
            this.label_load_obj.Location = new System.Drawing.Point(415, 11);
            this.label_load_obj.Name = "label_load_obj";
            this.label_load_obj.Size = new System.Drawing.Size(24, 13);
            this.label_load_obj.TabIndex = 3;
            this.label_load_obj.Text = "0 %";
            // 
            // progressBar_load_obj
            // 
            this.progressBar_load_obj.Location = new System.Drawing.Point(103, 6);
            this.progressBar_load_obj.Name = "progressBar_load_obj";
            this.progressBar_load_obj.Size = new System.Drawing.Size(643, 23);
            this.progressBar_load_obj.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(9, 43);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(88, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Save As";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(9, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(88, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Load";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tabSMD
            // 
            this.tabSMD.Location = new System.Drawing.Point(4, 22);
            this.tabSMD.Name = "tabSMD";
            this.tabSMD.Padding = new System.Windows.Forms.Padding(3);
            this.tabSMD.Size = new System.Drawing.Size(752, 174);
            this.tabSMD.TabIndex = 1;
            this.tabSMD.Text = "SMD File";
            this.tabSMD.UseVisualStyleBackColor = true;
            // 
            // tabOptions
            // 
            this.tabOptions.Controls.Add(this.groupBox_options_reduce);
            this.tabOptions.Controls.Add(this.groupBox3);
            this.tabOptions.Location = new System.Drawing.Point(4, 22);
            this.tabOptions.Name = "tabOptions";
            this.tabOptions.Padding = new System.Windows.Forms.Padding(3);
            this.tabOptions.Size = new System.Drawing.Size(752, 174);
            this.tabOptions.TabIndex = 2;
            this.tabOptions.Text = "Options";
            this.tabOptions.UseVisualStyleBackColor = true;
            // 
            // groupBox_options_reduce
            // 
            this.groupBox_options_reduce.Controls.Add(this.button_calc);
            this.groupBox_options_reduce.Controls.Add(this.label_reduce);
            this.groupBox_options_reduce.Controls.Add(this.trackBar_reduce_percent);
            this.groupBox_options_reduce.Controls.Add(this.progressBar_reduce);
            this.groupBox_options_reduce.Location = new System.Drawing.Point(166, 73);
            this.groupBox_options_reduce.Name = "groupBox_options_reduce";
            this.groupBox_options_reduce.Size = new System.Drawing.Size(580, 101);
            this.groupBox_options_reduce.TabIndex = 4;
            this.groupBox_options_reduce.TabStop = false;
            this.groupBox_options_reduce.Text = "Reduce";
            // 
            // button_calc
            // 
            this.button_calc.Location = new System.Drawing.Point(493, 20);
            this.button_calc.Name = "button_calc";
            this.button_calc.Size = new System.Drawing.Size(80, 45);
            this.button_calc.TabIndex = 1;
            this.button_calc.Text = "Calc";
            this.button_calc.UseVisualStyleBackColor = true;
            // 
            // label_reduce
            // 
            this.label_reduce.AutoSize = true;
            this.label_reduce.BackColor = System.Drawing.Color.Transparent;
            this.label_reduce.Location = new System.Drawing.Point(236, 75);
            this.label_reduce.Name = "label_reduce";
            this.label_reduce.Size = new System.Drawing.Size(100, 13);
            this.label_reduce.TabIndex = 3;
            this.label_reduce.Text = "Vertices: 0 / 0 (0 %)";
            // 
            // trackBar_reduce_percent
            // 
            this.trackBar_reduce_percent.Location = new System.Drawing.Point(6, 19);
            this.trackBar_reduce_percent.Maximum = 100;
            this.trackBar_reduce_percent.Name = "trackBar_reduce_percent";
            this.trackBar_reduce_percent.Size = new System.Drawing.Size(481, 45);
            this.trackBar_reduce_percent.TabIndex = 0;
            // 
            // progressBar_reduce
            // 
            this.progressBar_reduce.Location = new System.Drawing.Point(6, 70);
            this.progressBar_reduce.Name = "progressBar_reduce";
            this.progressBar_reduce.Size = new System.Drawing.Size(568, 23);
            this.progressBar_reduce.TabIndex = 2;
            this.progressBar_reduce.Click += new System.EventHandler(this.progressBar_reduce_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.trackBar_rotate_z);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.trackBar_rotate_y);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.trackBar_rotate_x);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(160, 174);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Rotate";
            // 
            // trackBar_rotate_z
            // 
            this.trackBar_rotate_z.Location = new System.Drawing.Point(108, 32);
            this.trackBar_rotate_z.Maximum = 360;
            this.trackBar_rotate_z.Name = "trackBar_rotate_z";
            this.trackBar_rotate_z.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar_rotate_z.Size = new System.Drawing.Size(45, 136);
            this.trackBar_rotate_z.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(108, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Z Axis";
            // 
            // trackBar_rotate_y
            // 
            this.trackBar_rotate_y.Location = new System.Drawing.Point(57, 32);
            this.trackBar_rotate_y.Maximum = 360;
            this.trackBar_rotate_y.Name = "trackBar_rotate_y";
            this.trackBar_rotate_y.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar_rotate_y.Size = new System.Drawing.Size(45, 136);
            this.trackBar_rotate_y.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(57, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Y Axis";
            // 
            // trackBar_rotate_x
            // 
            this.trackBar_rotate_x.Location = new System.Drawing.Point(6, 32);
            this.trackBar_rotate_x.Maximum = 360;
            this.trackBar_rotate_x.Name = "trackBar_rotate_x";
            this.trackBar_rotate_x.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar_rotate_x.Size = new System.Drawing.Size(45, 136);
            this.trackBar_rotate_x.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "X Axis";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mesh Reducer v1.0";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Activated += new System.EventHandler(this.MainWindow_Activated);
            this.Deactivate += new System.EventHandler(this.MainWindow_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainWindow_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainWindow_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainWindow_MouseUp);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabOBJ.ResumeLayout(false);
            this.tabOBJ.PerformLayout();
            this.tabOptions.ResumeLayout(false);
            this.groupBox_options_reduce.ResumeLayout(false);
            this.groupBox_options_reduce.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_reduce_percent)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_rotate_z)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_rotate_y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_rotate_x)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabOBJ;
        private System.Windows.Forms.TabPage tabSMD;
        private System.Windows.Forms.TabPage tabOptions;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label_load_obj;
        private System.Windows.Forms.ProgressBar progressBar_load_obj;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ProgressBar progressBar_reduce;
        private System.Windows.Forms.TrackBar trackBar_rotate_x;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar trackBar_rotate_z;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TrackBar trackBar_rotate_y;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label_reduce;
        private System.Windows.Forms.Label label_save_obj;
        private System.Windows.Forms.ProgressBar progressBar_save_obj;
        private System.Windows.Forms.GroupBox groupBox_options_reduce;
        private System.Windows.Forms.Button button_calc;
        private System.Windows.Forms.TrackBar trackBar_reduce_percent;
    }
}

