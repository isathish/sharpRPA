namespace sharpRPA.UI.Forms.Supplement_Forms
{
    partial class frmImageCapture
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.uiAccept = new sharpRPA.UI.CustomControls.UIPictureButton();
            this.uiClose = new sharpRPA.UI.CustomControls.UIPictureButton();
            this.pnlMouseContainer = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiAccept)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiClose)).BeginInit();
            this.pnlMouseContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(843, 363);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            // 
            // uiAccept
            // 
            this.uiAccept.BackColor = System.Drawing.Color.Transparent;
            this.uiAccept.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiAccept.DisplayText = "Accept";
            this.uiAccept.DisplayTextBrush = System.Drawing.Color.Black;
            this.uiAccept.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.uiAccept.Image = global::sharpRPA.Properties.Resources.navigate;
            this.uiAccept.IsMouseOver = false;
            this.uiAccept.Location = new System.Drawing.Point(3, 3);
            this.uiAccept.Name = "uiAccept";
            this.uiAccept.Size = new System.Drawing.Size(48, 48);
            this.uiAccept.TabIndex = 1;
            this.uiAccept.TabStop = false;
            this.uiAccept.Click += new System.EventHandler(this.uiAccept_Click);
            // 
            // uiClose
            // 
            this.uiClose.BackColor = System.Drawing.Color.Transparent;
            this.uiClose.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiClose.DisplayText = "Close";
            this.uiClose.DisplayTextBrush = System.Drawing.Color.Black;
            this.uiClose.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.uiClose.Image = global::sharpRPA.Properties.Resources.error;
            this.uiClose.IsMouseOver = false;
            this.uiClose.Location = new System.Drawing.Point(53, 3);
            this.uiClose.Name = "uiClose";
            this.uiClose.Size = new System.Drawing.Size(48, 48);
            this.uiClose.TabIndex = 2;
            this.uiClose.TabStop = false;
            this.uiClose.Click += new System.EventHandler(this.uiClose_Click);
            // 
            // pnlMouseContainer
            // 
            this.pnlMouseContainer.Controls.Add(this.uiAccept);
            this.pnlMouseContainer.Controls.Add(this.uiClose);
            this.pnlMouseContainer.Location = new System.Drawing.Point(12, 12);
            this.pnlMouseContainer.Name = "pnlMouseContainer";
            this.pnlMouseContainer.Size = new System.Drawing.Size(105, 55);
            this.pnlMouseContainer.TabIndex = 3;
            this.pnlMouseContainer.MouseEnter += new System.EventHandler(this.pnlMouseContainer_MouseEnter);
            // 
            // frmImageCapture
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 363);
            this.Controls.Add(this.pnlMouseContainer);
            this.Controls.Add(this.pictureBox1);
            this.Name = "frmImageCapture";
            this.Text = "Capture Image";
            this.Load += new System.EventHandler(this.frmImageCapture_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmImageCapture_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiAccept)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiClose)).EndInit();
            this.pnlMouseContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private CustomControls.UIPictureButton uiAccept;
        private CustomControls.UIPictureButton uiClose;
        public System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel pnlMouseContainer;
    }
}