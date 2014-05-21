namespace OpenTerraria {
    partial class MainForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.PhysicsTimer = new System.Windows.Forms.Timer(this.components);
            this.RenderTimer = new System.Windows.Forms.Timer(this.components);
            this.PausePanel = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.WorkingBar = new System.Windows.Forms.ProgressBar();
            this.FPSTimer = new System.Windows.Forms.Timer(this.components);
            this.PausePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // PhysicsTimer
            // 
            this.PhysicsTimer.Enabled = true;
            this.PhysicsTimer.Interval = 50;
            this.PhysicsTimer.Tick += new System.EventHandler(this.GameTimer_Tick);
            // 
            // RenderTimer
            // 
            this.RenderTimer.Enabled = true;
            this.RenderTimer.Interval = 50;
            this.RenderTimer.Tick += new System.EventHandler(this.RenderTimer_Tick);
            // 
            // PausePanel
            // 
            this.PausePanel.BackColor = System.Drawing.Color.Black;
            this.PausePanel.Controls.Add(this.label4);
            this.PausePanel.Controls.Add(this.label3);
            this.PausePanel.Controls.Add(this.label2);
            this.PausePanel.Controls.Add(this.label1);
            this.PausePanel.Location = new System.Drawing.Point(172, 146);
            this.PausePanel.Name = "PausePanel";
            this.PausePanel.Size = new System.Drawing.Size(200, 289);
            this.PausePanel.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.DarkBlue;
            this.label4.Font = new System.Drawing.Font("Comic Sans MS", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(43, 161);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(115, 29);
            this.label4.TabIndex = 3;
            this.label4.Text = "Load Game";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.DarkBlue;
            this.label3.Font = new System.Drawing.Font("Comic Sans MS", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(43, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 29);
            this.label3.TabIndex = 2;
            this.label3.Text = "Save Game";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.DarkBlue;
            this.label2.Font = new System.Drawing.Font("Comic Sans MS", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(42, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 29);
            this.label2.TabIndex = 1;
            this.label2.Text = "Quit";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Comic Sans MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(71, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Paused";
            // 
            // WorkingBar
            // 
            this.WorkingBar.Location = new System.Drawing.Point(-1, 0);
            this.WorkingBar.Name = "WorkingBar";
            this.WorkingBar.Size = new System.Drawing.Size(100, 23);
            this.WorkingBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.WorkingBar.TabIndex = 1;
            this.WorkingBar.Visible = false;
            // 
            // FPSTimer
            // 
            this.FPSTimer.Enabled = true;
            this.FPSTimer.Interval = 1000;
            this.FPSTimer.Tick += new System.EventHandler(this.FPSTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 566);
            this.Controls.Add(this.WorkingBar);
            this.Controls.Add(this.PausePanel);
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.Text = "OpenTerraria";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.PausePanel.ResumeLayout(false);
            this.PausePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Timer PhysicsTimer;
        public System.Windows.Forms.Timer RenderTimer;
        private System.Windows.Forms.Panel PausePanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ProgressBar WorkingBar;
        private System.Windows.Forms.Timer FPSTimer;

    }
}

