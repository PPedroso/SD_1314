namespace Client
{
    partial class ClientForm
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
            this.executableTextBox = new System.Windows.Forms.TextBox();
            this.inputPathTextBox = new System.Windows.Forms.TextBox();
            this.outputPathTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.addJobButton = new System.Windows.Forms.Button();
            this.statusTextBox = new System.Windows.Forms.RichTextBox();
            this.logTextBox = new System.Windows.Forms.RichTextBox();
            this.idStatusTextBox = new System.Windows.Forms.TextBox();
            this.findStatusButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // executableTextBox
            // 
            this.executableTextBox.Location = new System.Drawing.Point(95, 15);
            this.executableTextBox.Name = "executableTextBox";
            this.executableTextBox.Size = new System.Drawing.Size(223, 20);
            this.executableTextBox.TabIndex = 0;
            this.executableTextBox.Text = "Sum.exe";
            // 
            // inputPathTextBox
            // 
            this.inputPathTextBox.Location = new System.Drawing.Point(95, 46);
            this.inputPathTextBox.Name = "inputPathTextBox";
            this.inputPathTextBox.Size = new System.Drawing.Size(223, 20);
            this.inputPathTextBox.TabIndex = 1;
            this.inputPathTextBox.Text = "inputNumbers.txt";
            // 
            // outputPathTextBox
            // 
            this.outputPathTextBox.Location = new System.Drawing.Point(95, 75);
            this.outputPathTextBox.Name = "outputPathTextBox";
            this.outputPathTextBox.Size = new System.Drawing.Size(223, 20);
            this.outputPathTextBox.TabIndex = 2;
            this.outputPathTextBox.Text = "inputNumbersSum.txt";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Executable file";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Output path";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Input path";
            // 
            // addJobButton
            // 
            this.addJobButton.Location = new System.Drawing.Point(214, 102);
            this.addJobButton.Name = "addJobButton";
            this.addJobButton.Size = new System.Drawing.Size(104, 27);
            this.addJobButton.TabIndex = 8;
            this.addJobButton.Text = "Add job";
            this.addJobButton.UseVisualStyleBackColor = true;
            this.addJobButton.Click += new System.EventHandler(this.addJobButton_Click);
            // 
            // statusTextBox
            // 
            this.statusTextBox.Location = new System.Drawing.Point(16, 193);
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.Size = new System.Drawing.Size(302, 119);
            this.statusTextBox.TabIndex = 9;
            this.statusTextBox.Text = "";
            // 
            // logTextBox
            // 
            this.logTextBox.Location = new System.Drawing.Point(340, 15);
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.Size = new System.Drawing.Size(257, 297);
            this.logTextBox.TabIndex = 10;
            this.logTextBox.Text = "";
            this.logTextBox.TextChanged += new System.EventHandler(this.logTextBox_TextChanged);
            // 
            // idStatusTextBox
            // 
            this.idStatusTextBox.Location = new System.Drawing.Point(148, 165);
            this.idStatusTextBox.Name = "idStatusTextBox";
            this.idStatusTextBox.Size = new System.Drawing.Size(60, 20);
            this.idStatusTextBox.TabIndex = 11;
            // 
            // findStatusButton
            // 
            this.findStatusButton.Location = new System.Drawing.Point(214, 161);
            this.findStatusButton.Name = "findStatusButton";
            this.findStatusButton.Size = new System.Drawing.Size(104, 26);
            this.findStatusButton.TabIndex = 12;
            this.findStatusButton.Text = "Look up status";
            this.findStatusButton.UseVisualStyleBackColor = true;
            this.findStatusButton.Click += new System.EventHandler(this.findStatusButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 171);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(126, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Find status for job with id:";
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 324);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.findStatusButton);
            this.Controls.Add(this.idStatusTextBox);
            this.Controls.Add(this.logTextBox);
            this.Controls.Add(this.statusTextBox);
            this.Controls.Add(this.addJobButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.outputPathTextBox);
            this.Controls.Add(this.inputPathTextBox);
            this.Controls.Add(this.executableTextBox);
            this.Name = "ClientForm";
            this.Text = "ClientForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox executableTextBox;
        private System.Windows.Forms.TextBox inputPathTextBox;
        private System.Windows.Forms.TextBox outputPathTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button addJobButton;
        private System.Windows.Forms.RichTextBox statusTextBox;
        private System.Windows.Forms.RichTextBox logTextBox;
        private System.Windows.Forms.TextBox idStatusTextBox;
        private System.Windows.Forms.Button findStatusButton;
        private System.Windows.Forms.Label label4;
    }
}