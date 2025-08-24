namespace Test_Hotmail_2025
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            txtClientId = new TextBox();
            txtRefreshToken = new TextBox();
            btnRead = new Button();
            dgvInbox = new DataGridView();
            dt_TieuDe = new DataGridViewTextBoxColumn();
            dt_From = new DataGridViewTextBoxColumn();
            dt_subject = new DataGridViewTextBoxColumn();
            dt_preview = new DataGridViewTextBoxColumn();
            txtEmail = new TextBox();
            ((System.ComponentModel.ISupportInitialize)dgvInbox).BeginInit();
            SuspendLayout();
            // 
            // txtClientId
            // 
            txtClientId.Location = new Point(12, 39);
            txtClientId.Name = "txtClientId";
            txtClientId.Size = new Size(252, 23);
            txtClientId.TabIndex = 0;
            txtClientId.Text = "clientID";
            // 
            // txtRefreshToken
            // 
            txtRefreshToken.Location = new Point(12, 68);
            txtRefreshToken.Multiline = true;
            txtRefreshToken.Name = "txtRefreshToken";
            txtRefreshToken.Size = new Size(333, 198);
            txtRefreshToken.TabIndex = 1;
            txtRefreshToken.Text = "refreshToken";
            // 
            // btnRead
            // 
            btnRead.Location = new Point(270, 11);
            btnRead.Name = "btnRead";
            btnRead.Size = new Size(75, 51);
            btnRead.TabIndex = 2;
            btnRead.Text = "Read";
            btnRead.UseVisualStyleBackColor = true;
            btnRead.Click += btnRead_Click;
            // 
            // dgvInbox
            // 
            dgvInbox.AllowUserToAddRows = false;
            dgvInbox.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvInbox.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvInbox.Columns.AddRange(new DataGridViewColumn[] { dt_TieuDe, dt_From, dt_subject, dt_preview });
            dgvInbox.Location = new Point(12, 272);
            dgvInbox.Name = "dgvInbox";
            dgvInbox.Size = new Size(776, 249);
            dgvInbox.TabIndex = 3;
            // 
            // dt_TieuDe
            // 
            dt_TieuDe.HeaderText = "Tiêu Đề";
            dt_TieuDe.Name = "dt_TieuDe";
            // 
            // dt_From
            // 
            dt_From.HeaderText = "From";
            dt_From.Name = "dt_From";
            // 
            // dt_subject
            // 
            dt_subject.HeaderText = "Subject";
            dt_subject.Name = "dt_subject";
            // 
            // dt_preview
            // 
            dt_preview.HeaderText = "Preview";
            dt_preview.Name = "dt_preview";
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(12, 12);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(252, 23);
            txtEmail.TabIndex = 4;
            txtEmail.Text = "Mail";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 533);
            Controls.Add(txtEmail);
            Controls.Add(dgvInbox);
            Controls.Add(btnRead);
            Controls.Add(txtRefreshToken);
            Controls.Add(txtClientId);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dgvInbox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtClientId;
        private TextBox txtRefreshToken;
        private Button btnRead;
        private DataGridView dgvInbox;
        private DataGridViewTextBoxColumn dt_TieuDe;
        private DataGridViewTextBoxColumn dt_From;
        private DataGridViewTextBoxColumn dt_subject;
        private DataGridViewTextBoxColumn dt_preview;
        private TextBox txtEmail;
    }
}
