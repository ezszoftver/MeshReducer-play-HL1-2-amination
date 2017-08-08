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
            this.label_smd_save = new System.Windows.Forms.Label();
            this.progressBar_smd_save = new System.Windows.Forms.ProgressBar();
            this.button_smd_save = new System.Windows.Forms.Button();
            this.label_smd_speed = new System.Windows.Forms.Label();
            this.trackBar_animspeed = new System.Windows.Forms.TrackBar();
            this.comboBox_animations = new System.Windows.Forms.ComboBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.tabOptions = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.button_draw_type = new System.Windows.Forms.Button();
            this.label_percent = new System.Windows.Forms.Label();
            this.label_vertices = new System.Windows.Forms.Label();
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
            this.textBox_obj_filename = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabOBJ.SuspendLayout();
            this.tabSMD.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_animspeed)).BeginInit();
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
            this.tabControl1.Location = new System.Drawing.Point(12, 719);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(760, 200);
            this.tabControl1.TabIndex = 1;
            // 
            // tabOBJ
            // 
            this.tabOBJ.Controls.Add(this.label5);
            this.tabOBJ.Controls.Add(this.textBox_obj_filename);
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
            this.tabOBJ.Click += new System.EventHandler(this.tabOBJ_Click);
            // 
            // label_save_obj
            // 
            this.label_save_obj.AutoSize = true;
            this.label_save_obj.Location = new System.Drawing.Point(495, 48);
            this.label_save_obj.Name = "label_save_obj";
            this.label_save_obj.Size = new System.Drawing.Size(24, 13);
            this.label_save_obj.TabIndex = 5;
            this.label_save_obj.Text = "0 %";
            // 
            // progressBar_save_obj
            // 
            this.progressBar_save_obj.Location = new System.Drawing.Point(284, 43);
            this.progressBar_save_obj.Name = "progressBar_save_obj";
            this.progressBar_save_obj.Size = new System.Drawing.Size(462, 23);
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
            this.button2.Location = new System.Drawing.Point(190, 43);
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
            this.tabSMD.Controls.Add(this.label_smd_save);
            this.tabSMD.Controls.Add(this.progressBar_smd_save);
            this.tabSMD.Controls.Add(this.button_smd_save);
            this.tabSMD.Controls.Add(this.label_smd_speed);
            this.tabSMD.Controls.Add(this.trackBar_animspeed);
            this.tabSMD.Controls.Add(this.comboBox_animations);
            this.tabSMD.Controls.Add(this.button4);
            this.tabSMD.Controls.Add(this.button3);
            this.tabSMD.Location = new System.Drawing.Point(4, 22);
            this.tabSMD.Name = "tabSMD";
            this.tabSMD.Padding = new System.Windows.Forms.Padding(3);
            this.tabSMD.Size = new System.Drawing.Size(752, 174);
            this.tabSMD.TabIndex = 1;
            this.tabSMD.Text = "SMD File";
            this.tabSMD.UseVisualStyleBackColor = true;
            // 
            // label_smd_save
            // 
            this.label_smd_save.AutoSize = true;
            this.label_smd_save.Location = new System.Drawing.Point(412, 130);
            this.label_smd_save.Name = "label_smd_save";
            this.label_smd_save.Size = new System.Drawing.Size(24, 13);
            this.label_smd_save.TabIndex = 12;
            this.label_smd_save.Text = "0 %";
            this.label_smd_save.Click += new System.EventHandler(this.label5_Click);
            // 
            // progressBar_smd_save
            // 
            this.progressBar_smd_save.Location = new System.Drawing.Point(117, 125);
            this.progressBar_smd_save.Name = "progressBar_smd_save";
            this.progressBar_smd_save.Size = new System.Drawing.Size(619, 23);
            this.progressBar_smd_save.TabIndex = 11;
            // 
            // button_smd_save
            // 
            this.button_smd_save.Location = new System.Drawing.Point(6, 125);
            this.button_smd_save.Name = "button_smd_save";
            this.button_smd_save.Size = new System.Drawing.Size(105, 23);
            this.button_smd_save.TabIndex = 10;
            this.button_smd_save.Text = "Save As";
            this.button_smd_save.UseVisualStyleBackColor = true;
            this.button_smd_save.Click += new System.EventHandler(this.button_smd_save_Click);
            // 
            // label_smd_speed
            // 
            this.label_smd_speed.AutoSize = true;
            this.label_smd_speed.Location = new System.Drawing.Point(21, 78);
            this.label_smd_speed.Name = "label_smd_speed";
            this.label_smd_speed.Size = new System.Drawing.Size(41, 13);
            this.label_smd_speed.TabIndex = 9;
            this.label_smd_speed.Text = "Speed:";
            // 
            // trackBar_animspeed
            // 
            this.trackBar_animspeed.Location = new System.Drawing.Point(117, 62);
            this.trackBar_animspeed.Maximum = 100;
            this.trackBar_animspeed.Name = "trackBar_animspeed";
            this.trackBar_animspeed.Size = new System.Drawing.Size(213, 45);
            this.trackBar_animspeed.TabIndex = 8;
            this.trackBar_animspeed.Value = 30;
            this.trackBar_animspeed.Scroll += new System.EventHandler(this.trackBar_animspeed_Scroll);
            // 
            // comboBox_animations
            // 
            this.comboBox_animations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_animations.FormattingEnabled = true;
            this.comboBox_animations.Location = new System.Drawing.Point(117, 35);
            this.comboBox_animations.Name = "comboBox_animations";
            this.comboBox_animations.Size = new System.Drawing.Size(213, 21);
            this.comboBox_animations.TabIndex = 2;
            this.comboBox_animations.SelectedIndexChanged += new System.EventHandler(this.comboBox_animations_SelectedIndexChanged);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(6, 35);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(105, 23);
            this.button4.TabIndex = 1;
            this.button4.Text = "Add Animation";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(6, 6);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(105, 23);
            this.button3.TabIndex = 0;
            this.button3.Text = "Load Reference";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // tabOptions
            // 
            this.tabOptions.Controls.Add(this.label1);
            this.tabOptions.Controls.Add(this.button_draw_type);
            this.tabOptions.Controls.Add(this.label_percent);
            this.tabOptions.Controls.Add(this.label_vertices);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(163, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Drawing Mode:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // button_draw_type
            // 
            this.button_draw_type.Location = new System.Drawing.Point(248, 6);
            this.button_draw_type.Name = "button_draw_type";
            this.button_draw_type.Size = new System.Drawing.Size(159, 23);
            this.button_draw_type.TabIndex = 6;
            this.button_draw_type.Text = "Texture and Wireframe";
            this.button_draw_type.UseVisualStyleBackColor = true;
            this.button_draw_type.Click += new System.EventHandler(this.button_draw_type_Click);
            // 
            // label_percent
            // 
            this.label_percent.AutoSize = true;
            this.label_percent.BackColor = System.Drawing.Color.Transparent;
            this.label_percent.Location = new System.Drawing.Point(163, 44);
            this.label_percent.Name = "label_percent";
            this.label_percent.Size = new System.Drawing.Size(79, 13);
            this.label_percent.TabIndex = 5;
            this.label_percent.Text = "Percent: 100 %";
            this.label_percent.Click += new System.EventHandler(this.label5_Click_1);
            // 
            // label_vertices
            // 
            this.label_vertices.AutoSize = true;
            this.label_vertices.BackColor = System.Drawing.Color.Transparent;
            this.label_vertices.Location = new System.Drawing.Point(163, 57);
            this.label_vertices.Name = "label_vertices";
            this.label_vertices.Size = new System.Drawing.Size(176, 13);
            this.label_vertices.TabIndex = 4;
            this.label_vertices.Text = "Vertices: 0 / 0 ( Current Vertices: 0 )";
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
            this.button_calc.Click += new System.EventHandler(this.button_calc_Click);
            // 
            // label_reduce
            // 
            this.label_reduce.AutoSize = true;
            this.label_reduce.BackColor = System.Drawing.Color.Transparent;
            this.label_reduce.Location = new System.Drawing.Point(276, 75);
            this.label_reduce.Name = "label_reduce";
            this.label_reduce.Size = new System.Drawing.Size(24, 13);
            this.label_reduce.TabIndex = 3;
            this.label_reduce.Text = "0 %";
            // 
            // trackBar_reduce_percent
            // 
            this.trackBar_reduce_percent.Location = new System.Drawing.Point(6, 19);
            this.trackBar_reduce_percent.Maximum = 100;
            this.trackBar_reduce_percent.Minimum = 1;
            this.trackBar_reduce_percent.Name = "trackBar_reduce_percent";
            this.trackBar_reduce_percent.Size = new System.Drawing.Size(481, 45);
            this.trackBar_reduce_percent.TabIndex = 0;
            this.trackBar_reduce_percent.Value = 1;
            this.trackBar_reduce_percent.Scroll += new System.EventHandler(this.trackBar_reduce_percent_Scroll);
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
            this.trackBar_rotate_z.Maximum = 180;
            this.trackBar_rotate_z.Minimum = -180;
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
            this.trackBar_rotate_y.Maximum = 180;
            this.trackBar_rotate_y.Minimum = -180;
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
            this.trackBar_rotate_x.Maximum = 180;
            this.trackBar_rotate_x.Minimum = -180;
            this.trackBar_rotate_x.Name = "trackBar_rotate_x";
            this.trackBar_rotate_x.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar_rotate_x.Size = new System.Drawing.Size(45, 136);
            this.trackBar_rotate_x.TabIndex = 3;
            this.trackBar_rotate_x.Scroll += new System.EventHandler(this.trackBar_rotate_x_Scroll);
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
            // textBox_obj_filename
            // 
            this.textBox_obj_filename.Location = new System.Drawing.Point(6, 45);
            this.textBox_obj_filename.Name = "textBox_obj_filename";
            this.textBox_obj_filename.Size = new System.Drawing.Size(134, 20);
            this.textBox_obj_filename.TabIndex = 6;
            this.textBox_obj_filename.Text = "LOD_1";
            this.textBox_obj_filename.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBox_obj_filename.TextChanged += new System.EventHandler(this.textBox_obj_filename_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(140, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = ".OBJ";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 922);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(800, 960);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mesh Reducer v1.0";
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
            this.tabSMD.ResumeLayout(false);
            this.tabSMD.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_animspeed)).EndInit();
            this.tabOptions.ResumeLayout(false);
            this.tabOptions.PerformLayout();
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
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ComboBox comboBox_animations;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button_smd_save;
        private System.Windows.Forms.Label label_smd_speed;
        private System.Windows.Forms.TrackBar trackBar_animspeed;
        private System.Windows.Forms.Label label_smd_save;
        private System.Windows.Forms.ProgressBar progressBar_smd_save;
        private System.Windows.Forms.Label label_vertices;
        private System.Windows.Forms.Label label_percent;
        private System.Windows.Forms.Button button_draw_type;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_obj_filename;
        private System.Windows.Forms.Label label5;
    }
}

