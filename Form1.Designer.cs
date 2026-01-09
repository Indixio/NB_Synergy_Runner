namespace Fusionneur_de_pratiques
{
    partial class Form1
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
            this.pratiquePrimCombo = new System.Windows.Forms.ComboBox();
            this.pratiqueSecCombo = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ExecuteButton = new System.Windows.Forms.Button();
            this.totalCount = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.mainProgressBar = new System.Windows.Forms.ProgressBar();
            this.objectTypeLabel = new System.Windows.Forms.Label();
            this.testButton = new System.Windows.Forms.Button();
            this.documentsLabel = new System.Windows.Forms.Label();
            this.eqLabel = new System.Windows.Forms.Label();
            this.CheckedOutLabel = new System.Windows.Forms.Label();
            this.modifyCheckBox = new System.Windows.Forms.CheckBox();
            this.textBoxDocs = new System.Windows.Forms.RichTextBox();
            this.textBoxEQ = new System.Windows.Forms.RichTextBox();
            this.textBoxCO = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // pratiquePrimCombo
            // 
            this.pratiquePrimCombo.FormattingEnabled = true;
            this.pratiquePrimCombo.Location = new System.Drawing.Point(24, 51);
            this.pratiquePrimCombo.Name = "pratiquePrimCombo";
            this.pratiquePrimCombo.Size = new System.Drawing.Size(121, 28);
            this.pratiquePrimCombo.TabIndex = 2;
            // 
            // pratiqueSecCombo
            // 
            this.pratiqueSecCombo.FormattingEnabled = true;
            this.pratiqueSecCombo.Location = new System.Drawing.Point(187, 51);
            this.pratiqueSecCombo.Name = "pratiqueSecCombo";
            this.pratiqueSecCombo.Size = new System.Drawing.Size(121, 28);
            this.pratiqueSecCombo.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Pratique Primaire";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(183, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(153, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Pratique Secondaire";
            // 
            // ExecuteButton
            // 
            this.ExecuteButton.Location = new System.Drawing.Point(187, 102);
            this.ExecuteButton.Name = "ExecuteButton";
            this.ExecuteButton.Size = new System.Drawing.Size(112, 37);
            this.ExecuteButton.TabIndex = 6;
            this.ExecuteButton.Text = "Exécuter";
            this.ExecuteButton.UseVisualStyleBackColor = true;
            this.ExecuteButton.Click += new System.EventHandler(this.ExecuteButton_Click);
            // 
            // totalCount
            // 
            this.totalCount.AutoSize = true;
            this.totalCount.Location = new System.Drawing.Point(189, 201);
            this.totalCount.Name = "totalCount";
            this.totalCount.Size = new System.Drawing.Size(18, 20);
            this.totalCount.TabIndex = 7;
            this.totalCount.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 201);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(163, 20);
            this.label4.TabIndex = 8;
            this.label4.Text = "Nombre total d\'objets:";
            // 
            // mainProgressBar
            // 
            this.mainProgressBar.Location = new System.Drawing.Point(35, 356);
            this.mainProgressBar.Name = "mainProgressBar";
            this.mainProgressBar.Size = new System.Drawing.Size(888, 56);
            this.mainProgressBar.TabIndex = 9;
            this.mainProgressBar.Visible = false;
            // 
            // objectTypeLabel
            // 
            this.objectTypeLabel.AutoSize = true;
            this.objectTypeLabel.Location = new System.Drawing.Point(62, 322);
            this.objectTypeLabel.Name = "objectTypeLabel";
            this.objectTypeLabel.Size = new System.Drawing.Size(40, 20);
            this.objectTypeLabel.TabIndex = 10;
            this.objectTypeLabel.Text = "Test";
            this.objectTypeLabel.Visible = false;
            // 
            // testButton
            // 
            this.testButton.Location = new System.Drawing.Point(24, 102);
            this.testButton.Name = "testButton";
            this.testButton.Size = new System.Drawing.Size(111, 34);
            this.testButton.TabIndex = 11;
            this.testButton.Text = "Recherche";
            this.testButton.UseVisualStyleBackColor = true;
            this.testButton.Click += new System.EventHandler(this.testButton_Click);
            // 
            // documentsLabel
            // 
            this.documentsLabel.AutoSize = true;
            this.documentsLabel.Location = new System.Drawing.Point(361, 18);
            this.documentsLabel.Name = "documentsLabel";
            this.documentsLabel.Size = new System.Drawing.Size(115, 20);
            this.documentsLabel.TabIndex = 12;
            this.documentsLabel.Text = "Objets trouvés:\r\n";
            // 
            // eqLabel
            // 
            this.eqLabel.AutoSize = true;
            this.eqLabel.Location = new System.Drawing.Point(555, 18);
            this.eqLabel.Name = "eqLabel";
            this.eqLabel.Size = new System.Drawing.Size(116, 20);
            this.eqLabel.TabIndex = 13;
            this.eqLabel.Text = "Équipes-Client:\r\n";
            // 
            // CheckedOutLabel
            // 
            this.CheckedOutLabel.AutoSize = true;
            this.CheckedOutLabel.Location = new System.Drawing.Point(757, 18);
            this.CheckedOutLabel.Name = "CheckedOutLabel";
            this.CheckedOutLabel.Size = new System.Drawing.Size(133, 20);
            this.CheckedOutLabel.TabIndex = 14;
            this.CheckedOutLabel.Text = "Objets verrouillés:";
            // 
            // modifyCheckBox
            // 
            this.modifyCheckBox.AutoSize = true;
            this.modifyCheckBox.Location = new System.Drawing.Point(187, 145);
            this.modifyCheckBox.Name = "modifyCheckBox";
            this.modifyCheckBox.Size = new System.Drawing.Size(100, 24);
            this.modifyCheckBox.TabIndex = 15;
            this.modifyCheckBox.Text = "Modifier?";
            this.modifyCheckBox.UseVisualStyleBackColor = true;
            // 
            // textBoxDocs
            // 
            this.textBoxDocs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxDocs.Location = new System.Drawing.Point(365, 41);
            this.textBoxDocs.Name = "textBoxDocs";
            this.textBoxDocs.ReadOnly = true;
            this.textBoxDocs.Size = new System.Drawing.Size(184, 285);
            this.textBoxDocs.TabIndex = 18;
            this.textBoxDocs.Text = "";
            this.textBoxDocs.WordWrap = false;
            // 
            // textBoxEQ
            // 
            this.textBoxEQ.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxEQ.Location = new System.Drawing.Point(559, 41);
            this.textBoxEQ.Name = "textBoxEQ";
            this.textBoxEQ.ReadOnly = true;
            this.textBoxEQ.Size = new System.Drawing.Size(196, 280);
            this.textBoxEQ.TabIndex = 19;
            this.textBoxEQ.Text = "";
            this.textBoxEQ.WordWrap = false;
            // 
            // textBoxCO
            // 
            this.textBoxCO.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxCO.Location = new System.Drawing.Point(761, 41);
            this.textBoxCO.Name = "textBoxCO";
            this.textBoxCO.ReadOnly = true;
            this.textBoxCO.Size = new System.Drawing.Size(186, 280);
            this.textBoxCO.TabIndex = 20;
            this.textBoxCO.Text = "";
            this.textBoxCO.WordWrap = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(959, 450);
            this.Controls.Add(this.textBoxCO);
            this.Controls.Add(this.textBoxEQ);
            this.Controls.Add(this.textBoxDocs);
            this.Controls.Add(this.modifyCheckBox);
            this.Controls.Add(this.CheckedOutLabel);
            this.Controls.Add(this.eqLabel);
            this.Controls.Add(this.documentsLabel);
            this.Controls.Add(this.testButton);
            this.Controls.Add(this.objectTypeLabel);
            this.Controls.Add(this.mainProgressBar);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.totalCount);
            this.Controls.Add(this.ExecuteButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pratiqueSecCombo);
            this.Controls.Add(this.pratiquePrimCombo);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Fusionneur de Pratiques";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox pratiquePrimCombo;
        private System.Windows.Forms.ComboBox pratiqueSecCombo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button ExecuteButton;
        private System.Windows.Forms.Label totalCount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ProgressBar mainProgressBar;
        private System.Windows.Forms.Label objectTypeLabel;
        private System.Windows.Forms.Button testButton;
        private System.Windows.Forms.Label documentsLabel;
        private System.Windows.Forms.Label eqLabel;
        private System.Windows.Forms.Label CheckedOutLabel;
        private System.Windows.Forms.CheckBox modifyCheckBox;
        private System.Windows.Forms.RichTextBox textBoxDocs;
        private System.Windows.Forms.RichTextBox textBoxEQ;
        private System.Windows.Forms.RichTextBox textBoxCO;
    }
}

