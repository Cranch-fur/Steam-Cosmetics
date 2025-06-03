
namespace SteamCosmetics
{
    partial class Form_Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Main));
            this.panel_WindowHeader = new System.Windows.Forms.Panel();
            this.button_WindowMinimize = new System.Windows.Forms.Button();
            this.button_WindowClose = new System.Windows.Forms.Button();
            this.pictureBox_WindowBorder = new System.Windows.Forms.PictureBox();
            this.pictureBox_CharacterPortrait = new System.Windows.Forms.PictureBox();
            this.comboBox_CharacterSelect = new System.Windows.Forms.ComboBox();
            this.label_CharacterSelectTitle = new System.Windows.Forms.Label();
            this.textBox_Head = new System.Windows.Forms.TextBox();
            this.label_HeadTitle = new System.Windows.Forms.Label();
            this.label_TorsoBodyTitle = new System.Windows.Forms.Label();
            this.textBox_TorsoBody = new System.Windows.Forms.TextBox();
            this.label_LegsWeaponTitle = new System.Windows.Forms.Label();
            this.textBox_LegsWeapon = new System.Windows.Forms.TextBox();
            this.button_UpdateCustomizations = new System.Windows.Forms.Button();
            this.button_LoadBackup = new System.Windows.Forms.Button();
            this.trackBar_LoadoutSlot = new System.Windows.Forms.TrackBar();
            this.label_LoadoutSlot = new System.Windows.Forms.Label();
            this.panel_WindowHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_WindowBorder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_CharacterPortrait)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_LoadoutSlot)).BeginInit();
            this.SuspendLayout();
            // 
            // panel_WindowHeader
            // 
            this.panel_WindowHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(39)))), ((int)(((byte)(42)))));
            this.panel_WindowHeader.Controls.Add(this.button_WindowMinimize);
            this.panel_WindowHeader.Controls.Add(this.button_WindowClose);
            this.panel_WindowHeader.Location = new System.Drawing.Point(0, 0);
            this.panel_WindowHeader.Name = "panel_WindowHeader";
            this.panel_WindowHeader.Size = new System.Drawing.Size(650, 23);
            this.panel_WindowHeader.TabIndex = 2;
            this.panel_WindowHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel_WindowHeader_MouseDown);
            // 
            // button_WindowMinimize
            // 
            this.button_WindowMinimize.BackColor = System.Drawing.Color.DodgerBlue;
            this.button_WindowMinimize.FlatAppearance.BorderSize = 0;
            this.button_WindowMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_WindowMinimize.ForeColor = System.Drawing.Color.White;
            this.button_WindowMinimize.Location = new System.Drawing.Point(602, 0);
            this.button_WindowMinimize.Name = "button_WindowMinimize";
            this.button_WindowMinimize.Size = new System.Drawing.Size(24, 23);
            this.button_WindowMinimize.TabIndex = 1;
            this.button_WindowMinimize.Text = "—";
            this.button_WindowMinimize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_WindowMinimize.UseVisualStyleBackColor = false;
            this.button_WindowMinimize.MouseClick += new System.Windows.Forms.MouseEventHandler(this.button_WindowMinimize_MouseClick);
            // 
            // button_WindowClose
            // 
            this.button_WindowClose.BackColor = System.Drawing.Color.IndianRed;
            this.button_WindowClose.FlatAppearance.BorderSize = 0;
            this.button_WindowClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button_WindowClose.ForeColor = System.Drawing.Color.White;
            this.button_WindowClose.Location = new System.Drawing.Point(626, 0);
            this.button_WindowClose.Name = "button_WindowClose";
            this.button_WindowClose.Size = new System.Drawing.Size(24, 23);
            this.button_WindowClose.TabIndex = 0;
            this.button_WindowClose.Text = "X";
            this.button_WindowClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_WindowClose.UseVisualStyleBackColor = false;
            this.button_WindowClose.MouseClick += new System.Windows.Forms.MouseEventHandler(this.button_WindowClose_MouseClick);
            // 
            // pictureBox_WindowBorder
            // 
            this.pictureBox_WindowBorder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_WindowBorder.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_WindowBorder.Name = "pictureBox_WindowBorder";
            this.pictureBox_WindowBorder.Size = new System.Drawing.Size(650, 248);
            this.pictureBox_WindowBorder.TabIndex = 3;
            this.pictureBox_WindowBorder.TabStop = false;
            // 
            // pictureBox_CharacterPortrait
            // 
            this.pictureBox_CharacterPortrait.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(36)))), ((int)(((byte)(40)))), ((int)(((byte)(42)))));
            this.pictureBox_CharacterPortrait.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox_CharacterPortrait.Location = new System.Drawing.Point(17, 39);
            this.pictureBox_CharacterPortrait.Name = "pictureBox_CharacterPortrait";
            this.pictureBox_CharacterPortrait.Size = new System.Drawing.Size(192, 192);
            this.pictureBox_CharacterPortrait.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_CharacterPortrait.TabIndex = 4;
            this.pictureBox_CharacterPortrait.TabStop = false;
            // 
            // comboBox_CharacterSelect
            // 
            this.comboBox_CharacterSelect.FormattingEnabled = true;
            this.comboBox_CharacterSelect.Location = new System.Drawing.Point(245, 63);
            this.comboBox_CharacterSelect.Name = "comboBox_CharacterSelect";
            this.comboBox_CharacterSelect.Size = new System.Drawing.Size(368, 21);
            this.comboBox_CharacterSelect.TabIndex = 5;
            this.comboBox_CharacterSelect.SelectedIndexChanged += new System.EventHandler(this.comboBox_Characters_SelectedIndexChanged);
            // 
            // label_CharacterSelectTitle
            // 
            this.label_CharacterSelectTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_CharacterSelectTitle.ForeColor = System.Drawing.Color.White;
            this.label_CharacterSelectTitle.Location = new System.Drawing.Point(245, 39);
            this.label_CharacterSelectTitle.Name = "label_CharacterSelectTitle";
            this.label_CharacterSelectTitle.Size = new System.Drawing.Size(368, 21);
            this.label_CharacterSelectTitle.TabIndex = 6;
            this.label_CharacterSelectTitle.Text = "SELECT CHARACTER";
            this.label_CharacterSelectTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox_Head
            // 
            this.textBox_Head.Location = new System.Drawing.Point(245, 115);
            this.textBox_Head.Name = "textBox_Head";
            this.textBox_Head.Size = new System.Drawing.Size(120, 20);
            this.textBox_Head.TabIndex = 7;
            this.textBox_Head.Leave += new System.EventHandler(this.textBox_Head_Leave);
            // 
            // label_HeadTitle
            // 
            this.label_HeadTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_HeadTitle.ForeColor = System.Drawing.Color.White;
            this.label_HeadTitle.Location = new System.Drawing.Point(245, 95);
            this.label_HeadTitle.Name = "label_HeadTitle";
            this.label_HeadTitle.Size = new System.Drawing.Size(120, 17);
            this.label_HeadTitle.TabIndex = 8;
            this.label_HeadTitle.Text = "Head";
            this.label_HeadTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_TorsoBodyTitle
            // 
            this.label_TorsoBodyTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_TorsoBodyTitle.ForeColor = System.Drawing.Color.White;
            this.label_TorsoBodyTitle.Location = new System.Drawing.Point(369, 95);
            this.label_TorsoBodyTitle.Name = "label_TorsoBodyTitle";
            this.label_TorsoBodyTitle.Size = new System.Drawing.Size(120, 17);
            this.label_TorsoBodyTitle.TabIndex = 10;
            this.label_TorsoBodyTitle.Text = "Torso / Body";
            this.label_TorsoBodyTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox_TorsoBody
            // 
            this.textBox_TorsoBody.Location = new System.Drawing.Point(369, 115);
            this.textBox_TorsoBody.Name = "textBox_TorsoBody";
            this.textBox_TorsoBody.Size = new System.Drawing.Size(120, 20);
            this.textBox_TorsoBody.TabIndex = 9;
            this.textBox_TorsoBody.Leave += new System.EventHandler(this.textBox_TorsoBody_Leave);
            // 
            // label_LegsWeaponTitle
            // 
            this.label_LegsWeaponTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_LegsWeaponTitle.ForeColor = System.Drawing.Color.White;
            this.label_LegsWeaponTitle.Location = new System.Drawing.Point(493, 95);
            this.label_LegsWeaponTitle.Name = "label_LegsWeaponTitle";
            this.label_LegsWeaponTitle.Size = new System.Drawing.Size(120, 17);
            this.label_LegsWeaponTitle.TabIndex = 12;
            this.label_LegsWeaponTitle.Text = "Legs / Weapon";
            this.label_LegsWeaponTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox_LegsWeapon
            // 
            this.textBox_LegsWeapon.Location = new System.Drawing.Point(493, 115);
            this.textBox_LegsWeapon.Name = "textBox_LegsWeapon";
            this.textBox_LegsWeapon.Size = new System.Drawing.Size(120, 20);
            this.textBox_LegsWeapon.TabIndex = 11;
            this.textBox_LegsWeapon.Leave += new System.EventHandler(this.textBox_LegsWeapon_Leave);
            // 
            // button_UpdateCustomizations
            // 
            this.button_UpdateCustomizations.Location = new System.Drawing.Point(245, 209);
            this.button_UpdateCustomizations.Name = "button_UpdateCustomizations";
            this.button_UpdateCustomizations.Size = new System.Drawing.Size(332, 23);
            this.button_UpdateCustomizations.TabIndex = 13;
            this.button_UpdateCustomizations.Text = "Update Customizations Selection";
            this.button_UpdateCustomizations.UseVisualStyleBackColor = true;
            this.button_UpdateCustomizations.MouseClick += new System.Windows.Forms.MouseEventHandler(this.button_Update_MouseClick);
            // 
            // button_LoadBackup
            // 
            this.button_LoadBackup.Location = new System.Drawing.Point(585, 209);
            this.button_LoadBackup.Name = "button_LoadBackup";
            this.button_LoadBackup.Size = new System.Drawing.Size(28, 23);
            this.button_LoadBackup.TabIndex = 14;
            this.button_LoadBackup.Text = "↩️";
            this.button_LoadBackup.UseVisualStyleBackColor = true;
            this.button_LoadBackup.MouseClick += new System.Windows.Forms.MouseEventHandler(this.button_LoadBackup_MouseClick);
            // 
            // trackBar_LoadoutSlot
            // 
            this.trackBar_LoadoutSlot.AutoSize = false;
            this.trackBar_LoadoutSlot.LargeChange = 1;
            this.trackBar_LoadoutSlot.Location = new System.Drawing.Point(245, 167);
            this.trackBar_LoadoutSlot.Maximum = 3;
            this.trackBar_LoadoutSlot.Minimum = 1;
            this.trackBar_LoadoutSlot.Name = "trackBar_LoadoutSlot";
            this.trackBar_LoadoutSlot.Size = new System.Drawing.Size(368, 28);
            this.trackBar_LoadoutSlot.TabIndex = 15;
            this.trackBar_LoadoutSlot.Value = 1;
            this.trackBar_LoadoutSlot.Scroll += new System.EventHandler(this.trackBar_LoadoutSlot_Scroll);
            // 
            // label_LoadoutSlot
            // 
            this.label_LoadoutSlot.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_LoadoutSlot.ForeColor = System.Drawing.Color.White;
            this.label_LoadoutSlot.Location = new System.Drawing.Point(245, 149);
            this.label_LoadoutSlot.Name = "label_LoadoutSlot";
            this.label_LoadoutSlot.Size = new System.Drawing.Size(368, 17);
            this.label_LoadoutSlot.TabIndex = 16;
            this.label_LoadoutSlot.Text = "Loadout Slot #1";
            this.label_LoadoutSlot.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(47)))), ((int)(((byte)(51)))));
            this.ClientSize = new System.Drawing.Size(650, 248);
            this.Controls.Add(this.label_LoadoutSlot);
            this.Controls.Add(this.trackBar_LoadoutSlot);
            this.Controls.Add(this.button_LoadBackup);
            this.Controls.Add(this.button_UpdateCustomizations);
            this.Controls.Add(this.label_LegsWeaponTitle);
            this.Controls.Add(this.textBox_LegsWeapon);
            this.Controls.Add(this.label_TorsoBodyTitle);
            this.Controls.Add(this.textBox_TorsoBody);
            this.Controls.Add(this.label_HeadTitle);
            this.Controls.Add(this.textBox_Head);
            this.Controls.Add(this.label_CharacterSelectTitle);
            this.Controls.Add(this.comboBox_CharacterSelect);
            this.Controls.Add(this.pictureBox_CharacterPortrait);
            this.Controls.Add(this.panel_WindowHeader);
            this.Controls.Add(this.pictureBox_WindowBorder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form_Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Steam Cosmetics";
            this.Load += new System.EventHandler(this.Form_Main_Load);
            this.panel_WindowHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_WindowBorder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_CharacterPortrait)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_LoadoutSlot)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel_WindowHeader;
        private System.Windows.Forms.Button button_WindowMinimize;
        private System.Windows.Forms.Button button_WindowClose;
        private System.Windows.Forms.PictureBox pictureBox_WindowBorder;
        private System.Windows.Forms.PictureBox pictureBox_CharacterPortrait;
        private System.Windows.Forms.ComboBox comboBox_CharacterSelect;
        private System.Windows.Forms.Label label_CharacterSelectTitle;
        private System.Windows.Forms.TextBox textBox_Head;
        private System.Windows.Forms.Label label_HeadTitle;
        private System.Windows.Forms.Label label_TorsoBodyTitle;
        private System.Windows.Forms.TextBox textBox_TorsoBody;
        private System.Windows.Forms.Label label_LegsWeaponTitle;
        private System.Windows.Forms.TextBox textBox_LegsWeapon;
        private System.Windows.Forms.Button button_UpdateCustomizations;
        private System.Windows.Forms.Button button_LoadBackup;
        private System.Windows.Forms.TrackBar trackBar_LoadoutSlot;
        private System.Windows.Forms.Label label_LoadoutSlot;
    }
}

