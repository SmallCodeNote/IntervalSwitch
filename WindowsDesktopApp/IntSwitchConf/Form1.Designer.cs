namespace IntSwitchConf
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.labelNotifyData = new System.Windows.Forms.Label();
            this.labelWriteData = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.comboBox_DiveceList = new System.Windows.Forms.ComboBox();
            this.button_Connect = new System.Windows.Forms.Button();
            this.label_DeviceName = new System.Windows.Forms.Label();
            this.button_ON = new System.Windows.Forms.Button();
            this.button_OFF = new System.Windows.Forms.Button();
            this.button_AUTO = new System.Windows.Forms.Button();
            this.button_LenON = new System.Windows.Forms.Button();
            this.button_SAVE = new System.Windows.Forms.Button();
            this.button_LenOFF = new System.Windows.Forms.Button();
            this.textBox_LenON = new System.Windows.Forms.TextBox();
            this.textBox_LenOFF = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // labelNotifyData
            // 
            this.labelNotifyData.AutoSize = true;
            this.labelNotifyData.Location = new System.Drawing.Point(37, 38);
            this.labelNotifyData.Name = "labelNotifyData";
            this.labelNotifyData.Size = new System.Drawing.Size(43, 15);
            this.labelNotifyData.TabIndex = 0;
            this.labelNotifyData.Text = "label1";
            // 
            // labelWriteData
            // 
            this.labelWriteData.AutoSize = true;
            this.labelWriteData.Location = new System.Drawing.Point(37, 99);
            this.labelWriteData.Name = "labelWriteData";
            this.labelWriteData.Size = new System.Drawing.Size(43, 15);
            this.labelWriteData.TabIndex = 0;
            this.labelWriteData.Text = "label1";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(46, 185);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(933, 324);
            this.textBox1.TabIndex = 1;
            this.textBox1.WordWrap = false;
            // 
            // comboBox_DiveceList
            // 
            this.comboBox_DiveceList.Font = new System.Drawing.Font("MS UI Gothic", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.comboBox_DiveceList.FormattingEnabled = true;
            this.comboBox_DiveceList.Location = new System.Drawing.Point(105, 25);
            this.comboBox_DiveceList.Name = "comboBox_DiveceList";
            this.comboBox_DiveceList.Size = new System.Drawing.Size(292, 35);
            this.comboBox_DiveceList.TabIndex = 2;
            // 
            // button_Connect
            // 
            this.button_Connect.Location = new System.Drawing.Point(254, 66);
            this.button_Connect.Name = "button_Connect";
            this.button_Connect.Size = new System.Drawing.Size(143, 37);
            this.button_Connect.TabIndex = 3;
            this.button_Connect.Text = "Connect";
            this.button_Connect.UseVisualStyleBackColor = true;
            this.button_Connect.Click += new System.EventHandler(this.button_Connect_Click);
            // 
            // label_DeviceName
            // 
            this.label_DeviceName.AutoSize = true;
            this.label_DeviceName.Location = new System.Drawing.Point(102, 69);
            this.label_DeviceName.Name = "label_DeviceName";
            this.label_DeviceName.Size = new System.Drawing.Size(16, 15);
            this.label_DeviceName.TabIndex = 4;
            this.label_DeviceName.Text = "...";
            // 
            // button_ON
            // 
            this.button_ON.Enabled = false;
            this.button_ON.Location = new System.Drawing.Point(460, 25);
            this.button_ON.Name = "button_ON";
            this.button_ON.Size = new System.Drawing.Size(101, 35);
            this.button_ON.TabIndex = 5;
            this.button_ON.Text = "ON";
            this.button_ON.UseVisualStyleBackColor = true;
            this.button_ON.Click += new System.EventHandler(this.button_ON_Click);
            // 
            // button_OFF
            // 
            this.button_OFF.Enabled = false;
            this.button_OFF.Location = new System.Drawing.Point(460, 66);
            this.button_OFF.Name = "button_OFF";
            this.button_OFF.Size = new System.Drawing.Size(101, 35);
            this.button_OFF.TabIndex = 5;
            this.button_OFF.Text = "OFF";
            this.button_OFF.UseVisualStyleBackColor = true;
            this.button_OFF.Click += new System.EventHandler(this.button_OFF_Click);
            // 
            // button_AUTO
            // 
            this.button_AUTO.Enabled = false;
            this.button_AUTO.Location = new System.Drawing.Point(460, 107);
            this.button_AUTO.Name = "button_AUTO";
            this.button_AUTO.Size = new System.Drawing.Size(101, 35);
            this.button_AUTO.TabIndex = 5;
            this.button_AUTO.Text = "Auto";
            this.button_AUTO.UseVisualStyleBackColor = true;
            this.button_AUTO.Click += new System.EventHandler(this.button_AUTO_Click);
            // 
            // button_LenON
            // 
            this.button_LenON.Enabled = false;
            this.button_LenON.Location = new System.Drawing.Point(594, 25);
            this.button_LenON.Name = "button_LenON";
            this.button_LenON.Size = new System.Drawing.Size(101, 35);
            this.button_LenON.TabIndex = 5;
            this.button_LenON.Text = "LengthON";
            this.button_LenON.UseVisualStyleBackColor = true;
            this.button_LenON.Click += new System.EventHandler(this.button_LenON_Click);
            // 
            // button_SAVE
            // 
            this.button_SAVE.Enabled = false;
            this.button_SAVE.Location = new System.Drawing.Point(594, 107);
            this.button_SAVE.Name = "button_SAVE";
            this.button_SAVE.Size = new System.Drawing.Size(101, 35);
            this.button_SAVE.TabIndex = 5;
            this.button_SAVE.Text = "Save";
            this.button_SAVE.UseVisualStyleBackColor = true;
            this.button_SAVE.Click += new System.EventHandler(this.button_SAVE_Click);
            // 
            // button_LenOFF
            // 
            this.button_LenOFF.Enabled = false;
            this.button_LenOFF.Location = new System.Drawing.Point(594, 69);
            this.button_LenOFF.Name = "button_LenOFF";
            this.button_LenOFF.Size = new System.Drawing.Size(101, 35);
            this.button_LenOFF.TabIndex = 5;
            this.button_LenOFF.Text = "LengthOFF";
            this.button_LenOFF.UseVisualStyleBackColor = true;
            this.button_LenOFF.Click += new System.EventHandler(this.button_LenOFF_Click);
            // 
            // textBox_LenON
            // 
            this.textBox_LenON.Location = new System.Drawing.Point(717, 25);
            this.textBox_LenON.Name = "textBox_LenON";
            this.textBox_LenON.Size = new System.Drawing.Size(98, 22);
            this.textBox_LenON.TabIndex = 6;
            // 
            // textBox_LenOFF
            // 
            this.textBox_LenOFF.Location = new System.Drawing.Point(717, 76);
            this.textBox_LenOFF.Name = "textBox_LenOFF";
            this.textBox_LenOFF.Size = new System.Drawing.Size(98, 22);
            this.textBox_LenOFF.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1014, 548);
            this.Controls.Add(this.textBox_LenOFF);
            this.Controls.Add(this.textBox_LenON);
            this.Controls.Add(this.button_SAVE);
            this.Controls.Add(this.button_AUTO);
            this.Controls.Add(this.button_OFF);
            this.Controls.Add(this.button_LenOFF);
            this.Controls.Add(this.button_LenON);
            this.Controls.Add(this.button_ON);
            this.Controls.Add(this.label_DeviceName);
            this.Controls.Add(this.button_Connect);
            this.Controls.Add(this.comboBox_DiveceList);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.labelWriteData);
            this.Controls.Add(this.labelNotifyData);
            this.Name = "Form1";
            this.Text = "IntervalSwitchConfig";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelNotifyData;
        private System.Windows.Forms.Label labelWriteData;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ComboBox comboBox_DiveceList;
        private System.Windows.Forms.Button button_Connect;
        private System.Windows.Forms.Label label_DeviceName;
        private System.Windows.Forms.Button button_ON;
        private System.Windows.Forms.Button button_OFF;
        private System.Windows.Forms.Button button_AUTO;
        private System.Windows.Forms.Button button_LenON;
        private System.Windows.Forms.Button button_SAVE;
        private System.Windows.Forms.Button button_LenOFF;
        private System.Windows.Forms.TextBox textBox_LenON;
        private System.Windows.Forms.TextBox textBox_LenOFF;
    }
}

