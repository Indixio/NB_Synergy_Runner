using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace Fusionneur_de_pratiques
{
    public partial class Login : Form
    {
        Form1 parentForm;
        public Login(Form1 parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm; 
            PrepareForm();
        }

        public void PrepareForm()
        {
            this.warningLabel.Text = string.Empty;
            this.passwordTB.PasswordChar = '*';
        }

        private void connexionConfirmButton_Click(object sender, EventArgs e)
        {
            if (this.usernameTB.Text == string.Empty || 
                this.passwordTB.Text == string.Empty || 
                this.serverTB.Text == string.Empty ||
                this.mfauthCB.Text == string.Empty ||
                this.protocolCB.Text == string.Empty ||
                this.guidTB.Text == string.Empty)
            {
                this.warningLabel.Text = "Veuillez remplir les champs de texte!";
                return;
            }

            this.warningLabel.Text = string.Empty;

            parentForm.username = this.usernameTB.Text;
            parentForm.password = this.passwordTB.Text;
            parentForm.serverName = this.serverTB.Text;
            parentForm.protocol = this.protocolCB.Text.Split(' ')[0];
            parentForm.endpoint = this.endpointTB.Text;
            parentForm.GUID = this.guidTB.Text;
            parentForm.mfAuth = this.mfauthCB.SelectedIndex;
            parentForm.result = DialogResult.OK;

            this.Dispose();
        }
    }
}
