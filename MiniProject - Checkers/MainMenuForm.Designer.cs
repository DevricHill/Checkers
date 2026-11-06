namespace MiniProject___Checkers
{
    partial class MainMenuForm
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
            label1 = new Label();
            btnStart = new Button();
            btnExit = new Button();
            btnHelp = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Snap ITC", 48F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(300, 100);
            label1.Name = "label1";
            label1.Size = new Size(369, 82);
            label1.TabIndex = 0;
            label1.Text = "Checkers";
            // 
            // btnStart
            // 
            btnStart.Location = new Point(200, 225);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(552, 40);
            btnStart.TabIndex = 1;
            btnStart.Text = "Start";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // btnExit
            // 
            btnExit.Location = new Point(200, 350);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(552, 40);
            btnExit.TabIndex = 2;
            btnExit.Text = "Exit";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += btnExit_Click;
            // 
            // btnHelp
            // 
            btnHelp.Location = new Point(200, 287);
            btnHelp.Name = "btnHelp";
            btnHelp.Size = new Size(552, 40);
            btnHelp.TabIndex = 3;
            btnHelp.Text = "Help";
            btnHelp.UseVisualStyleBackColor = true;
            btnHelp.Click += btnHelp_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.InactiveCaption;
            ClientSize = new Size(984, 561);
            Controls.Add(btnHelp);
            Controls.Add(btnExit);
            Controls.Add(btnStart);
            Controls.Add(label1);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button btnStart;
        private Button btnExit;
        private Button btnHelp;
    }
}
