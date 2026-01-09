namespace Fusionneur_de_pratiques
{
    partial class Login
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.usernameTB = new System.Windows.Forms.TextBox();
            this.passwordTB = new System.Windows.Forms.TextBox();
            this.connexionConfirmButton = new System.Windows.Forms.Button();
            this.warningLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.serverTB = new System.Windows.Forms.TextBox();
            this.protocolCB = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.mfauthCB = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.endpointTB = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.guidTB = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(272, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Veuillez entrer votre nom d\'utilisateur:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(253, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Veuillez entrer votre mot de passe:";
            // 
            // usernameTB
            // 
            this.usernameTB.Location = new System.Drawing.Point(16, 48);
            this.usernameTB.Name = "usernameTB";
            this.usernameTB.Size = new System.Drawing.Size(335, 26);
            this.usernameTB.TabIndex = 2;
            // 
            // passwordTB
            // 
            this.passwordTB.Location = new System.Drawing.Point(16, 111);
            this.passwordTB.Name = "passwordTB";
            this.passwordTB.Size = new System.Drawing.Size(335, 26);
            this.passwordTB.TabIndex = 3;
            // 
            // connexionConfirmButton
            // 
            this.connexionConfirmButton.Location = new System.Drawing.Point(628, 248);
            this.connexionConfirmButton.Name = "connexionConfirmButton";
            this.connexionConfirmButton.Size = new System.Drawing.Size(147, 46);
            this.connexionConfirmButton.TabIndex = 4;
            this.connexionConfirmButton.Text = "Confirmer";
            this.connexionConfirmButton.UseVisualStyleBackColor = true;
            this.connexionConfirmButton.Click += new System.EventHandler(this.connexionConfirmButton_Click);
            // 
            // warningLabel
            // 
            this.warningLabel.AutoSize = true;
            this.warningLabel.Location = new System.Drawing.Point(511, 216);
            this.warningLabel.Name = "warningLabel";
            this.warningLabel.Size = new System.Drawing.Size(264, 20);
            this.warningLabel.TabIndex = 5;
            this.warningLabel.Text = "Veuillez remplir les champs de texte!";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 150);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(391, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Veuillez entrer le nom du serveur auquel se connecter:";
            // 
            // serverTB
            // 
            this.serverTB.Location = new System.Drawing.Point(16, 173);
            this.serverTB.Name = "serverTB";
            this.serverTB.Size = new System.Drawing.Size(335, 26);
            this.serverTB.TabIndex = 7;
            // 
            // protocolCB
            // 
            this.protocolCB.FormattingEnabled = true;
            this.protocolCB.Items.AddRange(new object[] {
            "grpc-local",
            "grpc",
            "ncacn_http (HTTPS)",
            "ncalrpc (LPC)",
            "ncacn_spx (SPX)",
            "ncacn_ip_tcp (TCP/IP)"});
            this.protocolCB.Location = new System.Drawing.Point(411, 115);
            this.protocolCB.Name = "protocolCB";
            this.protocolCB.Size = new System.Drawing.Size(335, 28);
            this.protocolCB.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(407, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(368, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "Veuillez choisir le protocole de connexion au coffre:";
            // 
            // mfauthCB
            // 
            this.mfauthCB.FormattingEnabled = true;
            this.mfauthCB.Items.AddRange(new object[] {
            "MFAuthTypeLoggedOnWindowsUser",
            "MFAuthTypeSpecificWindowsUser",
            "MFAuthTypeSpecificMFilesUser",
            "MFAuthTypeUnknown"});
            this.mfauthCB.Location = new System.Drawing.Point(411, 47);
            this.mfauthCB.Name = "mfauthCB";
            this.mfauthCB.Size = new System.Drawing.Size(335, 28);
            this.mfauthCB.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(407, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(354, 20);
            this.label5.TabIndex = 11;
            this.label5.Text = "Veuillez choisir le type d\'authentification à utiliser:";
            // 
            // endpointTB
            // 
            this.endpointTB.Location = new System.Drawing.Point(411, 173);
            this.endpointTB.Name = "endpointTB";
            this.endpointTB.Size = new System.Drawing.Size(335, 26);
            this.endpointTB.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(407, 150);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(407, 20);
            this.label6.TabIndex = 13;
            this.label6.Text = "Entrez l\'Endpoint (Laisser vide pour LPC et gRPC Local):";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 216);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(391, 20);
            this.label7.TabIndex = 14;
            this.label7.Text = "Veuillez entrer le GUID du coffre auquel se connecter:";
            // 
            // guidTB
            // 
            this.guidTB.Location = new System.Drawing.Point(16, 239);
            this.guidTB.Name = "guidTB";
            this.guidTB.Size = new System.Drawing.Size(335, 26);
            this.guidTB.TabIndex = 15;
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(822, 312);
            this.Controls.Add(this.guidTB);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.endpointTB);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.mfauthCB);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.protocolCB);
            this.Controls.Add(this.serverTB);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.warningLabel);
            this.Controls.Add(this.connexionConfirmButton);
            this.Controls.Add(this.passwordTB);
            this.Controls.Add(this.usernameTB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label6);
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Connextion au compte";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox usernameTB;
        private System.Windows.Forms.TextBox passwordTB;
        private System.Windows.Forms.Button connexionConfirmButton;
        private System.Windows.Forms.Label warningLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox serverTB;
        private System.Windows.Forms.ComboBox protocolCB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox mfauthCB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox endpointTB;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox guidTB;
    }
}