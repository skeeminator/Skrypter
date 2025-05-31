namespace Builder
{
    partial class SilentForm
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
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SilentForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.StubSettings = new MetroFramework.Controls.MetroTabControl();
            this.metroTabPage1 = new MetroFramework.Controls.MetroTabPage();
            this.ResetBtn = new System.Windows.Forms.Button();
            this.SelectedPath_Box = new System.Windows.Forms.ComboBox();
            this.BuildBtn = new System.Windows.Forms.Button();
            this.SelectFile2 = new MetroFramework.Controls.MetroButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.File2Box = new MetroFramework.Controls.MetroTextBox();
            this.SelectFile1 = new MetroFramework.Controls.MetroButton();
            this.File1Box = new MetroFramework.Controls.MetroTextBox();
            this.metroTabPage2 = new MetroFramework.Controls.MetroTabPage();
            this.SelectIconBtn = new System.Windows.Forms.Button();
            this.CloneAssemblyBtn = new System.Windows.Forms.Button();
            this.ImageIcon_Box = new System.Windows.Forms.PictureBox();
            this.UseAmsiEtw_Chk = new MetroFramework.Controls.MetroCheckBox();
            this.PolymorphicChk = new MetroFramework.Controls.MetroCheckBox();
            this.SelfDelete_Chk = new MetroFramework.Controls.MetroCheckBox();
            this.ObfuscatorChk = new MetroFramework.Controls.MetroCheckBox();
            this.HideFile_Box = new MetroFramework.Controls.MetroCheckBox();
            this.metroTabPage3 = new MetroFramework.Controls.MetroTabPage();
            this.CreditsLabel = new System.Windows.Forms.Label();
            this.LabelSupp3 = new System.Windows.Forms.Label();
            this.LabelSupp1 = new System.Windows.Forms.Label();
            this.AuthorLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.StubSettings.SuspendLayout();
            this.metroTabPage1.SuspendLayout();
            this.metroTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageIcon_Box)).BeginInit();
            this.metroTabPage3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Builder.Properties.Resources.skrypter;
            this.pictureBox1.Location = new System.Drawing.Point(10, 13);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(194, 165);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // StubSettings
            // 
            this.StubSettings.Controls.Add(this.metroTabPage1);
            this.StubSettings.Controls.Add(this.metroTabPage2);
            this.StubSettings.Controls.Add(this.metroTabPage3);
            this.StubSettings.Location = new System.Drawing.Point(20, 194);
            this.StubSettings.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.StubSettings.Name = "StubSettings";
            this.StubSettings.SelectedIndex = 1;
            this.StubSettings.Size = new System.Drawing.Size(912, 550);
            this.StubSettings.Style = MetroFramework.MetroColorStyle.Purple;
            this.StubSettings.TabIndex = 1;
            this.StubSettings.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // metroTabPage1
            // 
            this.metroTabPage1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.metroTabPage1.Controls.Add(this.ResetBtn);
            this.metroTabPage1.Controls.Add(this.SelectedPath_Box);
            this.metroTabPage1.Controls.Add(this.BuildBtn);
            this.metroTabPage1.Controls.Add(this.SelectFile2);
            this.metroTabPage1.Controls.Add(this.label2);
            this.metroTabPage1.Controls.Add(this.label1);
            this.metroTabPage1.Controls.Add(this.File2Box);
            this.metroTabPage1.Controls.Add(this.SelectFile1);
            this.metroTabPage1.Controls.Add(this.File1Box);
            this.metroTabPage1.HorizontalScrollbarBarColor = true;
            this.metroTabPage1.HorizontalScrollbarSize = 19;
            this.metroTabPage1.Location = new System.Drawing.Point(8, 42);
            this.metroTabPage1.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.metroTabPage1.Name = "metroTabPage1";
            this.metroTabPage1.Size = new System.Drawing.Size(896, 500);
            this.metroTabPage1.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTabPage1.TabIndex = 0;
            this.metroTabPage1.Text = "Binder";
            this.metroTabPage1.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroTabPage1.VerticalScrollbarBarColor = true;
            this.metroTabPage1.VerticalScrollbarSize = 20;
            // 
            // ResetBtn
            // 
            this.ResetBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.ResetBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ResetBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ResetBtn.ForeColor = System.Drawing.Color.MediumPurple;
            this.ResetBtn.Location = new System.Drawing.Point(374, 273);
            this.ResetBtn.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.ResetBtn.Name = "ResetBtn";
            this.ResetBtn.Size = new System.Drawing.Size(508, 50);
            this.ResetBtn.TabIndex = 23;
            this.ResetBtn.Text = "Reset All Parameters";
            this.ResetBtn.UseVisualStyleBackColor = false;
            this.ResetBtn.Click += new System.EventHandler(this.ResetBtn_Click);
            // 
            // SelectedPath_Box
            // 
            this.SelectedPath_Box.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.SelectedPath_Box.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SelectedPath_Box.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SelectedPath_Box.ForeColor = System.Drawing.Color.MediumPurple;
            this.SelectedPath_Box.FormattingEnabled = true;
            this.SelectedPath_Box.Items.AddRange(new object[] {
            "%temp%",
            "%programdata%",
            "%appdata%",
            "%localappdata%",
            "%public%"});
            this.SelectedPath_Box.Location = new System.Drawing.Point(24, 277);
            this.SelectedPath_Box.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.SelectedPath_Box.Name = "SelectedPath_Box";
            this.SelectedPath_Box.Size = new System.Drawing.Size(326, 40);
            this.SelectedPath_Box.TabIndex = 11;
            // 
            // BuildBtn
            // 
            this.BuildBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.BuildBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BuildBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BuildBtn.ForeColor = System.Drawing.Color.LightGreen;
            this.BuildBtn.Location = new System.Drawing.Point(24, 348);
            this.BuildBtn.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.BuildBtn.Name = "BuildBtn";
            this.BuildBtn.Size = new System.Drawing.Size(857, 108);
            this.BuildBtn.TabIndex = 8;
            this.BuildBtn.Text = "Build";
            this.BuildBtn.UseVisualStyleBackColor = false;
            this.BuildBtn.Click += new System.EventHandler(this.BuildBtn_Click);
            // 
            // SelectFile2
            // 
            this.SelectFile2.Location = new System.Drawing.Point(766, 206);
            this.SelectFile2.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.SelectFile2.Name = "SelectFile2";
            this.SelectFile2.Size = new System.Drawing.Size(116, 44);
            this.SelectFile2.Style = MetroFramework.MetroColorStyle.Green;
            this.SelectFile2.TabIndex = 7;
            this.SelectFile2.Text = "...";
            this.SelectFile2.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.SelectFile2.Click += new System.EventHandler(this.SelectFile2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(16, 167);
            this.label2.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(346, 36);
            this.label2.TabIndex = 6;
            this.label2.Text = "Select the 2nd file (optional):";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(16, 58);
            this.label1.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(228, 36);
            this.label1.TabIndex = 5;
            this.label1.Text = "Select the first file:";
            // 
            // File2Box
            // 
            this.File2Box.AllowDrop = true;
            this.File2Box.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.File2Box.CustomBackground = true;
            this.File2Box.CustomForeColor = true;
            this.File2Box.ForeColor = System.Drawing.Color.White;
            this.File2Box.Location = new System.Drawing.Point(16, 206);
            this.File2Box.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.File2Box.Name = "File2Box";
            this.File2Box.PromptText = "File 2";
            this.File2Box.Size = new System.Drawing.Size(737, 44);
            this.File2Box.Style = MetroFramework.MetroColorStyle.Red;
            this.File2Box.TabIndex = 4;
            this.File2Box.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.File2Box.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.File2Box.UseStyleColors = true;
            this.File2Box.DragDrop += new System.Windows.Forms.DragEventHandler(this.File2Box_DragDrop);
            this.File2Box.DragEnter += new System.Windows.Forms.DragEventHandler(this.File2Box_DragEnter);
            // 
            // SelectFile1
            // 
            this.SelectFile1.Location = new System.Drawing.Point(766, 96);
            this.SelectFile1.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.SelectFile1.Name = "SelectFile1";
            this.SelectFile1.Size = new System.Drawing.Size(116, 44);
            this.SelectFile1.Style = MetroFramework.MetroColorStyle.Green;
            this.SelectFile1.TabIndex = 3;
            this.SelectFile1.Text = "...";
            this.SelectFile1.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.SelectFile1.Click += new System.EventHandler(this.SelectFile1_Click);
            // 
            // File1Box
            // 
            this.File1Box.AllowDrop = true;
            this.File1Box.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.File1Box.CustomBackground = true;
            this.File1Box.CustomForeColor = true;
            this.File1Box.ForeColor = System.Drawing.Color.White;
            this.File1Box.Location = new System.Drawing.Point(16, 96);
            this.File1Box.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.File1Box.Name = "File1Box";
            this.File1Box.PromptText = "File 1";
            this.File1Box.Size = new System.Drawing.Size(737, 44);
            this.File1Box.Style = MetroFramework.MetroColorStyle.Red;
            this.File1Box.TabIndex = 2;
            this.File1Box.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.File1Box.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.File1Box.UseStyleColors = true;
            this.File1Box.DragDrop += new System.Windows.Forms.DragEventHandler(this.File1Box_DragDrop);
            this.File1Box.DragEnter += new System.Windows.Forms.DragEventHandler(this.File1Box_DragEnter);
            // 
            // metroTabPage2
            // 
            this.metroTabPage2.BackColor = System.Drawing.SystemColors.ControlText;
            this.metroTabPage2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.metroTabPage2.Controls.Add(this.SelectIconBtn);
            this.metroTabPage2.Controls.Add(this.CloneAssemblyBtn);
            this.metroTabPage2.Controls.Add(this.ImageIcon_Box);
            this.metroTabPage2.Controls.Add(this.UseAmsiEtw_Chk);
            this.metroTabPage2.Controls.Add(this.PolymorphicChk);
            this.metroTabPage2.Controls.Add(this.SelfDelete_Chk);
            this.metroTabPage2.Controls.Add(this.ObfuscatorChk);
            this.metroTabPage2.Controls.Add(this.HideFile_Box);
            this.metroTabPage2.HorizontalScrollbarBarColor = true;
            this.metroTabPage2.HorizontalScrollbarSize = 19;
            this.metroTabPage2.Location = new System.Drawing.Point(8, 42);
            this.metroTabPage2.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.metroTabPage2.Name = "metroTabPage2";
            this.metroTabPage2.Size = new System.Drawing.Size(896, 500);
            this.metroTabPage2.Style = MetroFramework.MetroColorStyle.Green;
            this.metroTabPage2.TabIndex = 3;
            this.metroTabPage2.Text = "Stub Settings";
            this.metroTabPage2.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroTabPage2.VerticalScrollbarBarColor = true;
            this.metroTabPage2.VerticalScrollbarSize = 20;
            // 
            // SelectIconBtn
            // 
            this.SelectIconBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.SelectIconBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SelectIconBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectIconBtn.ForeColor = System.Drawing.Color.LightGreen;
            this.SelectIconBtn.Location = new System.Drawing.Point(48, 356);
            this.SelectIconBtn.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.SelectIconBtn.Name = "SelectIconBtn";
            this.SelectIconBtn.Size = new System.Drawing.Size(220, 75);
            this.SelectIconBtn.TabIndex = 21;
            this.SelectIconBtn.Text = "Select Icon";
            this.SelectIconBtn.UseVisualStyleBackColor = false;
            this.SelectIconBtn.Click += new System.EventHandler(this.SelectIconBtn_Click);
            // 
            // CloneAssemblyBtn
            // 
            this.CloneAssemblyBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            this.CloneAssemblyBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CloneAssemblyBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.75F);
            this.CloneAssemblyBtn.ForeColor = System.Drawing.Color.LightGreen;
            this.CloneAssemblyBtn.Location = new System.Drawing.Point(286, 356);
            this.CloneAssemblyBtn.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.CloneAssemblyBtn.Name = "CloneAssemblyBtn";
            this.CloneAssemblyBtn.Size = new System.Drawing.Size(220, 75);
            this.CloneAssemblyBtn.TabIndex = 20;
            this.CloneAssemblyBtn.Text = "Clone Assembly";
            this.CloneAssemblyBtn.UseVisualStyleBackColor = false;
            this.CloneAssemblyBtn.Click += new System.EventHandler(this.CloneAssemblyBtn_Click);
            // 
            // ImageIcon_Box
            // 
            this.ImageIcon_Box.BackColor = System.Drawing.Color.Transparent;
            this.ImageIcon_Box.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ImageIcon_Box.Location = new System.Drawing.Point(652, 271);
            this.ImageIcon_Box.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.ImageIcon_Box.Name = "ImageIcon_Box";
            this.ImageIcon_Box.Size = new System.Drawing.Size(186, 165);
            this.ImageIcon_Box.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ImageIcon_Box.TabIndex = 19;
            this.ImageIcon_Box.TabStop = false;
            // 
            // UseAmsiEtw_Chk
            // 
            this.UseAmsiEtw_Chk.Appearance = System.Windows.Forms.Appearance.Button;
            this.UseAmsiEtw_Chk.AutoSize = true;
            this.UseAmsiEtw_Chk.CustomForeColor = true;
            this.UseAmsiEtw_Chk.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.UseAmsiEtw_Chk.FontSize = MetroFramework.MetroLinkSize.Tall;
            this.UseAmsiEtw_Chk.ForeColor = System.Drawing.Color.Crimson;
            this.UseAmsiEtw_Chk.Location = new System.Drawing.Point(28, 271);
            this.UseAmsiEtw_Chk.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.UseAmsiEtw_Chk.Name = "UseAmsiEtw_Chk";
            this.UseAmsiEtw_Chk.Size = new System.Drawing.Size(189, 25);
            this.UseAmsiEtw_Chk.Style = MetroFramework.MetroColorStyle.Green;
            this.UseAmsiEtw_Chk.TabIndex = 18;
            this.UseAmsiEtw_Chk.Text = "AMSI/ETW PATCHES";
            this.UseAmsiEtw_Chk.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.UseAmsiEtw_Chk.UseVisualStyleBackColor = true;
            // 
            // PolymorphicChk
            // 
            this.PolymorphicChk.Appearance = System.Windows.Forms.Appearance.Button;
            this.PolymorphicChk.AutoSize = true;
            this.PolymorphicChk.CustomForeColor = true;
            this.PolymorphicChk.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.PolymorphicChk.FontSize = MetroFramework.MetroLinkSize.Tall;
            this.PolymorphicChk.ForeColor = System.Drawing.Color.Crimson;
            this.PolymorphicChk.Location = new System.Drawing.Point(349, 19);
            this.PolymorphicChk.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.PolymorphicChk.Name = "PolymorphicChk";
            this.PolymorphicChk.Size = new System.Drawing.Size(216, 25);
            this.PolymorphicChk.Style = MetroFramework.MetroColorStyle.Green;
            this.PolymorphicChk.TabIndex = 19;
            this.PolymorphicChk.Text = "Polymorphic Encryption";
            this.PolymorphicChk.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.PolymorphicChk.UseVisualStyleBackColor = true;
            // 
            // SelfDelete_Chk
            // 
            this.SelfDelete_Chk.Appearance = System.Windows.Forms.Appearance.Button;
            this.SelfDelete_Chk.AutoSize = true;
            this.SelfDelete_Chk.CustomForeColor = true;
            this.SelfDelete_Chk.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.SelfDelete_Chk.FontSize = MetroFramework.MetroLinkSize.Tall;
            this.SelfDelete_Chk.ForeColor = System.Drawing.Color.Crimson;
            this.SelfDelete_Chk.Location = new System.Drawing.Point(28, 102);
            this.SelfDelete_Chk.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.SelfDelete_Chk.Name = "SelfDelete_Chk";
            this.SelfDelete_Chk.Size = new System.Drawing.Size(112, 25);
            this.SelfDelete_Chk.Style = MetroFramework.MetroColorStyle.Green;
            this.SelfDelete_Chk.TabIndex = 16;
            this.SelfDelete_Chk.Text = "Self Delete";
            this.SelfDelete_Chk.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.SelfDelete_Chk.UseVisualStyleBackColor = true;
            // 
            // ObfuscatorChk
            // 
            this.ObfuscatorChk.Appearance = System.Windows.Forms.Appearance.Button;
            this.ObfuscatorChk.AutoSize = true;
            this.ObfuscatorChk.CustomForeColor = true;
            this.ObfuscatorChk.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.ObfuscatorChk.FontSize = MetroFramework.MetroLinkSize.Tall;
            this.ObfuscatorChk.ForeColor = System.Drawing.Color.Crimson;
            this.ObfuscatorChk.Location = new System.Drawing.Point(28, 185);
            this.ObfuscatorChk.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.ObfuscatorChk.Name = "ObfuscatorChk";
            this.ObfuscatorChk.Size = new System.Drawing.Size(117, 25);
            this.ObfuscatorChk.Style = MetroFramework.MetroColorStyle.Green;
            this.ObfuscatorChk.TabIndex = 15;
            this.ObfuscatorChk.Text = "Obfuscator";
            this.ObfuscatorChk.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.ObfuscatorChk.UseVisualStyleBackColor = true;
            // 
            // HideFile_Box
            // 
            this.HideFile_Box.Appearance = System.Windows.Forms.Appearance.Button;
            this.HideFile_Box.AutoSize = true;
            this.HideFile_Box.CustomForeColor = true;
            this.HideFile_Box.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.HideFile_Box.FontSize = MetroFramework.MetroLinkSize.Tall;
            this.HideFile_Box.ForeColor = System.Drawing.Color.Crimson;
            this.HideFile_Box.Location = new System.Drawing.Point(28, 19);
            this.HideFile_Box.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.HideFile_Box.Name = "HideFile_Box";
            this.HideFile_Box.Size = new System.Drawing.Size(104, 25);
            this.HideFile_Box.Style = MetroFramework.MetroColorStyle.Green;
            this.HideFile_Box.TabIndex = 14;
            this.HideFile_Box.Text = "Hide Files";
            this.HideFile_Box.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.HideFile_Box.UseVisualStyleBackColor = true;
            // 
            // metroTabPage3
            // 
            this.metroTabPage3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.metroTabPage3.Controls.Add(this.CreditsLabel);
            this.metroTabPage3.Controls.Add(this.LabelSupp3);
            this.metroTabPage3.Controls.Add(this.LabelSupp1);
            this.metroTabPage3.Controls.Add(this.AuthorLabel);
            this.metroTabPage3.Controls.Add(this.groupBox1);
            this.metroTabPage3.Controls.Add(this.pictureBox2);
            this.metroTabPage3.HorizontalScrollbarBarColor = true;
            this.metroTabPage3.HorizontalScrollbarSize = 19;
            this.metroTabPage3.Location = new System.Drawing.Point(8, 42);
            this.metroTabPage3.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.metroTabPage3.Name = "metroTabPage3";
            this.metroTabPage3.Size = new System.Drawing.Size(896, 500);
            this.metroTabPage3.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTabPage3.TabIndex = 2;
            this.metroTabPage3.Text = "About";
            this.metroTabPage3.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroTabPage3.VerticalScrollbarBarColor = true;
            this.metroTabPage3.VerticalScrollbarSize = 20;
            // 
            // CreditsLabel
            // 
            this.CreditsLabel.AutoSize = true;
            this.CreditsLabel.BackColor = System.Drawing.Color.Transparent;
            this.CreditsLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CreditsLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CreditsLabel.Font = new System.Drawing.Font("Consolas", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CreditsLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(215)))), ((int)(((byte)(255)))));
            this.CreditsLabel.Location = new System.Drawing.Point(163, 381);
            this.CreditsLabel.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.CreditsLabel.Name = "CreditsLabel";
            this.CreditsLabel.Size = new System.Drawing.Size(662, 28);
            this.CreditsLabel.TabIndex = 9;
            this.CreditsLabel.Text = "dnlib | MetroFramework | Evilbytecode (AMSI Patch)\r\n";
            this.CreditsLabel.Click += new System.EventHandler(this.CreditsLabel_Click);
            // 
            // LabelSupp3
            // 
            this.LabelSupp3.AutoSize = true;
            this.LabelSupp3.BackColor = System.Drawing.Color.Transparent;
            this.LabelSupp3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LabelSupp3.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LabelSupp3.ForeColor = System.Drawing.Color.White;
            this.LabelSupp3.Location = new System.Drawing.Point(39, 381);
            this.LabelSupp3.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.LabelSupp3.Name = "LabelSupp3";
            this.LabelSupp3.Size = new System.Drawing.Size(116, 28);
            this.LabelSupp3.TabIndex = 8;
            this.LabelSupp3.Text = "Credits:";
            // 
            // LabelSupp1
            // 
            this.LabelSupp1.AutoSize = true;
            this.LabelSupp1.BackColor = System.Drawing.Color.Transparent;
            this.LabelSupp1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LabelSupp1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LabelSupp1.ForeColor = System.Drawing.Color.White;
            this.LabelSupp1.Location = new System.Drawing.Point(41, 350);
            this.LabelSupp1.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.LabelSupp1.Name = "LabelSupp1";
            this.LabelSupp1.Size = new System.Drawing.Size(103, 28);
            this.LabelSupp1.TabIndex = 7;
            this.LabelSupp1.Text = "Author:";
            // 
            // AuthorLabel
            // 
            this.AuthorLabel.AutoSize = true;
            this.AuthorLabel.BackColor = System.Drawing.Color.Transparent;
            this.AuthorLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AuthorLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AuthorLabel.Font = new System.Drawing.Font("Consolas", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.AuthorLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(215)))), ((int)(((byte)(255)))));
            this.AuthorLabel.Location = new System.Drawing.Point(146, 350);
            this.AuthorLabel.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.AuthorLabel.Name = "AuthorLabel";
            this.AuthorLabel.Size = new System.Drawing.Size(285, 28);
            this.AuthorLabel.TabIndex = 4;
            this.AuthorLabel.Text = "Skeeminator | SkeemAI";
            this.AuthorLabel.Click += new System.EventHandler(this.AuthorLabel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(312, 29);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.groupBox1.Size = new System.Drawing.Size(569, 285);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(167)))), ((int)(((byte)(167)))), ((int)(((byte)(167)))));
            this.label3.Location = new System.Drawing.Point(16, 27);
            this.label3.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(456, 234);
            this.label3.TabIndex = 0;
            this.label3.Text = resources.GetString("label3.Text");
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = global::Builder.Properties.Resources.skeemlabs_logo;
            this.pictureBox2.Location = new System.Drawing.Point(16, 46);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(284, 267);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 2;
            this.pictureBox2.TabStop = false;
            // 
            // SilentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 773);
            this.Controls.Add(this.StubSettings);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.MaximizeBox = false;
            this.Name = "SilentForm";
            this.Padding = new System.Windows.Forms.Padding(40, 115, 40, 38);
            this.Resizable = false;
            this.ShadowType = MetroFramework.Forms.MetroForm.MetroFormShadowType.Flat;
            this.Style = MetroFramework.MetroColorStyle.Green;
            this.Text = "Skrypter | @SkeemLabs ";
            this.TextAlign = System.Windows.Forms.VisualStyles.HorizontalAlign.Center;
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.Load += new System.EventHandler(this.SilentForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.StubSettings.ResumeLayout(false);
            this.metroTabPage1.ResumeLayout(false);
            this.metroTabPage1.PerformLayout();
            this.metroTabPage2.ResumeLayout(false);
            this.metroTabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageIcon_Box)).EndInit();
            this.metroTabPage3.ResumeLayout(false);
            this.metroTabPage3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private MetroFramework.Controls.MetroTabControl StubSettings;
        private MetroFramework.Controls.MetroTabPage metroTabPage1;
        private MetroFramework.Controls.MetroTabPage metroTabPage3;
        private MetroFramework.Controls.MetroTextBox File1Box;
        private System.Windows.Forms.Label label1;
        private MetroFramework.Controls.MetroTextBox File2Box;
        private MetroFramework.Controls.MetroButton SelectFile1;
        private MetroFramework.Controls.MetroButton SelectFile2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BuildBtn;
        private System.Windows.Forms.ComboBox SelectedPath_Box;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label AuthorLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label LabelSupp1;
        private System.Windows.Forms.Label CreditsLabel;
        private System.Windows.Forms.Label LabelSupp3;
        private System.Windows.Forms.Label label3;
        private MetroFramework.Controls.MetroTabPage metroTabPage2;
        private MetroFramework.Controls.MetroCheckBox SelfDelete_Chk;
        private MetroFramework.Controls.MetroCheckBox ObfuscatorChk;
        private MetroFramework.Controls.MetroCheckBox HideFile_Box;
        private System.Windows.Forms.PictureBox ImageIcon_Box;
        private MetroFramework.Controls.MetroCheckBox UseAmsiEtw_Chk;
        private System.Windows.Forms.Button SelectIconBtn;
        private System.Windows.Forms.Button CloneAssemblyBtn;
        private System.Windows.Forms.Button ResetBtn;
        private MetroFramework.Controls.MetroCheckBox PolymorphicChk;
    }
}

