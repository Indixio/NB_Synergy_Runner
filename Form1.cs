using MFiles.Extensibility;
using MFiles.VAF.Common;
using MFiles.VAF.Extensions;
using MFilesAPI;
using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static MFiles.VAF.Configuration.ValidationResultForValidation;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;
using System.Diagnostics;

namespace Fusionneur_de_pratiques
{
    public partial class Form1 : Form
    {
        public string username;
        public string password;
        public string serverName;
        public string protocol;
        public string endpoint;
        public string GUID;
        public int mfAuth;
        public DialogResult result = DialogResult.Cancel;

        private VaultConnectionSettings settings;
        private VaultConnectionEx connector;

        public Vault selectedVault;
        public Vault selectedVaultServer;

        public int clientOTID;
        public int eClientOTID;

        public int hubClassID;
        public int employeNBClassID;
        public int equipeClientClassID;

        public int pratiquePrpID;
        public int clientPrpID;
        public int equipeClientPrpID;
        public int intervenantsPrpID;

        public int pratiquePrincipaleID;
        public int pratiqueSecondaireID;

        ObjVerEx pratPrimObj;
        ObjVerEx pratSecObj;

        StreamWriter sw;
        Stopwatch stopwatch;

        public List<string> checkedOutIDs = new List<string>();
        public Form1()
        {
            InitializeComponent();
#if DEBUG

#else
            using (var childForm = new Login(this))
            {
                childForm.ShowDialog();
                if (result == DialogResult.Cancel) Environment.Exit(0);
                /*while (result == DialogResult.Cancel)
                {
                    MessageBox.Show("Veuillez remplir les champs de texte!", "Information manquantes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    childForm.ShowDialog();
                }*/
                this.Hide();
            }
#endif
            PrepareForm();
        }

        private void PrepareForm()
        {
            //this.vaultCombo.DisplayMember = "Name";
            this.pratiquePrimCombo.DisplayMember = "Title";
            this.pratiqueSecCombo.DisplayMember = "Title";
            this.totalCount.BringToFront();
            //logger = LogManager.GetCurrentClassLogger();
            if (!Directory.Exists("..\\Log")) Directory.CreateDirectory("..\\Log");
            string dateTime = $"{DateTime.Today.Year}{DateTime.Today.Month}{DateTime.Now.Day}";
            if (!File.Exists($"..\\Log\\Log_Fusionneur_{dateTime}.txt")) File.CreateText($"..\\Log\\Log_Fusionneur_{dateTime}.txt").Dispose();
            sw = File.AppendText($"..\\Log\\Log_Fusionneur_{dateTime}.txt");
            sw.AutoFlush = true;
            Console.SetOut(sw);
            stopwatch = new Stopwatch();
            this.loadVaults();
        }



        private void loadVaults()
        {
            settings = new VaultConnectionSettings();

            settings.Server = this.serverName;
            settings.Protocol = this.protocol;
            settings.Port = this.endpoint;
            settings.AuthenticationType = (AuthenticationType)mfAuth;
            settings.UserAccount.Username = this.username;
            settings.UserAccount.Password = this.password;
            settings.UserAccount.Domain = "";

            settings.VaultGUID = this.GUID;


            //connector.GetVault();

            try
            {
#if DEBUG
                mfServerCon = _application.Connect();
#else
                connector = new VaultConnectionEx(settings);
                //Console.Out.WriteLine($"Username: {this.username}, PW: {this.password}, NetworkAddress: {this.serverName}, ProtocolSequence: {this.protocol}, MFAuth: {authTypeToUse}");
                //mfServerCon = _application.Connect(AuthType: authTypeToUse, UserName: this.username, Password: this.password, NetworkAddress: this.serverName, ProtocolSequence: this.protocol, Endpoint: this.endpoint);
                //mfServerCon = _application.ConnectAdministrativeInteractive(ServerDisplayName: serverName, ParentWindow: this.Handle, ReturnNoneIfCancelledByUser: true, DefaultAuthType: MFAuthType.MFAuthTypeSpecificMFilesUser, ProtocolSequence: "grpc-local");
#endif
            }
            catch (Exception ex)
            {
                MessageBox.Show($"La connexion au serveur a échoué!\n{ex.InnerException}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //VaultsOnServer allServerVaults = _application.GetVaults();
            //VaultOnServer serverVault = allServerVaults.GetVaultByName(this.selectedVault.Name);
            //this.selectedVaultServer = serverVault.LogIn();
            this.selectedVaultServer = connector.GetVault();

            this.pratiquePrimCombo.Items.Clear();
            this.pratiqueSecCombo.Items.Clear();

            PopulatePratiques();
            PopulateIDs();
            /*MFilesClientApplication clientApplication = (MFilesClientApplication)new MFilesClientApplicationClass();
            this.vaultCombo.Items.Clear();

            foreach (VaultConnection vaultConnection in (IVaultConnections)clientApplication.GetVaultConnections())
                this.vaultCombo.Items.Add((object)vaultConnection);
            this.deselectVault();*/
        }

        private void deselectVault()
        {
            //this.vaultCombo.Text = "";
            this.selectedVault = (Vault)null;
        }

        private void PopulateIDs()
        {
            clientOTID = this.selectedVaultServer.ObjectTypeOperations.GetObjectTypeIDByAlias("OT.Client");
            eClientOTID = this.selectedVaultServer.ObjectTypeOperations.GetObjectTypeIDByAlias("OT.ClientPratique");

            hubClassID = this.selectedVaultServer.ClassOperations.GetObjectClassIDByAlias("HS.CL.Hub");
            employeNBClassID = this.selectedVaultServer.ClassOperations.GetObjectClassIDByAlias("CL.Employe");
            equipeClientClassID = this.selectedVaultServer.ClassOperations.GetObjectClassIDByAlias("CL.ClientPratique");

            pratiquePrpID = this.selectedVaultServer.PropertyDefOperations.GetPropertyDefIDByAlias("PD.Pratique");
            clientPrpID = this.selectedVaultServer.PropertyDefOperations.GetPropertyDefIDByAlias("PD.Client");
            equipeClientPrpID = this.selectedVaultServer.PropertyDefOperations.GetPropertyDefIDByAlias("PD.ClientpratiqueAdd");
            intervenantsPrpID = this.selectedVaultServer.PropertyDefOperations.GetPropertyDefIDByAlias("PD.Intervenants");
        }

        private void PopulatePratiques()
        {
            MFSearchBuilder searcher = new MFSearchBuilder(this.selectedVaultServer);
            searcher.Class(this.selectedVaultServer.ClassOperations.GetObjectClassIDByAlias("CL.Pratique"));
            searcher.Deleted(false);
            ObjectSearchResults resultsTest = searcher.Find();

            List<ObjVerEx> pratiquesEx = resultsTest.GetAsObjectVersions().ToObjVerExs(this.selectedVaultServer);

            foreach (ObjVerEx pratique in pratiquesEx)
            {
                this.pratiquePrimCombo.Items.Add(pratique);
                this.pratiqueSecCombo.Items.Add(pratique);
            }
        }

        private async void ExecuteButton_Click(object sender, EventArgs e)
        {
            if (this.pratiquePrimCombo.SelectedItem == null || this.pratiqueSecCombo.SelectedItem == null || this.pratiquePrimCombo.SelectedItem == this.pratiqueSecCombo.SelectedItem)
            {
                this.totalCount.Text = "SVP vous assurer que les pratiques sont correctement sélectionnées!";
                UpdateUI();
                return;
            }
            Console.Out.WriteLine($"Starting New Execution on {(DateTime.Now.Hour < 10 ? "0" + DateTime.Now.Hour.ToString() : DateTime.Now.Hour.ToString())}:" +
                $"{(DateTime.Now.Minute < 10 ? "0" + DateTime.Now.Minute.ToString() : DateTime.Now.Minute.ToString())}:" +
                $"{(DateTime.Now.Second < 10 ? "0" + DateTime.Now.Second.ToString() : DateTime.Now.Second.ToString())}");
            stopwatch.Start();

            this.mainProgressBar.Visible = true;
            this.objectTypeLabel.Visible = true;
            this.objectTypeLabel.Text = "";
            this.totalCount.Text = "0";
            this.textBoxDocs.Text = "";
            this.textBoxEQ.Text = "";
            this.textBoxCO.Text = "";
            UpdateUI();

            this.checkedOutIDs = new List<string>();

            this.mainProgressBar.Maximum = 100;
            this.mainProgressBar.Step = 1;
            this.mainProgressBar.Value = 0;

            pratiquePrincipaleID = ((ObjVerEx)pratiquePrimCombo.SelectedItem).ID;
            pratiqueSecondaireID = ((ObjVerEx)pratiqueSecCombo.SelectedItem).ID;

            pratPrimObj = (ObjVerEx)pratiquePrimCombo.SelectedItem;
            pratSecObj = (ObjVerEx)pratiqueSecCombo.SelectedItem;
            try
            {
                await Task.Run(() => mainLoop());
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }

        private async void testButton_Click(object sender, EventArgs e)
        {
            if (this.pratiquePrimCombo.SelectedItem == null || this.pratiqueSecCombo.SelectedItem == null || this.pratiquePrimCombo.SelectedItem == this.pratiqueSecCombo.SelectedItem)
            {
                this.totalCount.Text = "SVP vous assurer que les pratiques sont correctement sélectionnées!";
                UpdateUI();
                return;
            }
            Console.Out.WriteLine($"Starting New Execution on {(DateTime.Now.Hour < 10 ? "0" + DateTime.Now.Hour.ToString() : DateTime.Now.Hour.ToString())}:" +
                $"{(DateTime.Now.Minute < 10 ? "0" + DateTime.Now.Minute.ToString() : DateTime.Now.Minute.ToString())}:" +
                $"{(DateTime.Now.Second < 10 ? "0" + DateTime.Now.Second.ToString() : DateTime.Now.Second.ToString())}");
            stopwatch.Start();

            this.mainProgressBar.Visible = true;
            this.objectTypeLabel.Visible = true;
            this.objectTypeLabel.Text = "";
            this.totalCount.Text = "0";
            this.textBoxDocs.Text = "";
            this.textBoxEQ.Text = "";
            this.textBoxCO.Text = "";
            UpdateUI();

            this.checkedOutIDs = new List<string>();

            this.mainProgressBar.Maximum = 100;
            this.mainProgressBar.Step = 1;
            this.mainProgressBar.Value = 0;

            pratiquePrincipaleID = ((ObjVerEx)pratiquePrimCombo.SelectedItem).ID;
            pratiqueSecondaireID = ((ObjVerEx)pratiqueSecCombo.SelectedItem).ID;

            pratPrimObj = (ObjVerEx)pratiquePrimCombo.SelectedItem;
            pratSecObj = (ObjVerEx)pratiqueSecCombo.SelectedItem;

            await Task.Run(() => searchLoop());
        }

        private void mainLoop()
        {
            const int itemsPerSegment = 10000; // Maximum number of items in each segment.

            var objectTypes = this.selectedVaultServer
                .ObjectTypeOperations
                .GetObjectTypes()
                .Cast<ObjType>()
                .ToList();

            int totalObjects = 0;
            this.mainProgressBar.Invoke(new MethodInvoker(delegate { this.mainProgressBar.Step = 2; }));
            this.mainProgressBar.Invoke(new MethodInvoker(delegate { this.mainProgressBar.Maximum = 2 * objectTypes.Count(); }));
            
            MFSearchBuilder executeSearcher = new MFSearchBuilder(this.selectedVaultServer);
            var segment = 0;
            var objCount = 0;
            var moreItems = true;

            /***** CLIENT *****/

            //Module Client
            executeSearcher.ObjType(clientOTID);
            executeSearcher.Deleted(false);
            
            //ObjectSearchResults resultsSearchBuilder = executeSearcher.Find();

            this.objectTypeLabel.Invoke(new MethodInvoker(delegate { this.objectTypeLabel.Text = "Client"; }));
            UpdateUI();

            List<ObjVerEx> resultsObjVerEx = new List<ObjVerEx>();
            while (moreItems)
            {
                executeSearcher.SegmentedCount(segment, segmentSize: itemsPerSegment);
                executeSearcher.Property(pratiquePrpID, MFDataType.MFDatatypeMultiSelectLookup, pratiqueSecondaireID);

                resultsObjVerEx = executeSearcher.Find().GetAsObjectVersions().ToObjVerExs(this.selectedVaultServer);
                foreach (ObjVerEx result in resultsObjVerEx)
                {
                    //Console.Out.WriteLine($"Current Client: {result.ID}");
                    try
                    {
                        Console.Out.WriteLine($"Processing du client: {result.ID}");
                        objCount++;
                        totalObjects++;
                        if (this.selectedVaultServer.ObjectOperations.IsObjectCheckedOut(result.ObjID, true))
                        {
                            string[] textBoxCOTexts = new string[0];
                            this.textBoxCO.Invoke(new MethodInvoker(delegate { textBoxCOTexts = this.textBoxCO.Text.Split('\n'); }));
                            if (textBoxCOTexts.Where(x => x.Contains($"{result.ObjID.Type}-{result.ObjID.ID}")).FirstOrDefault() == null) this.checkedOutIDs.Add($"{result.ObjID.Type}-{result.ObjID.ID}, Client {result.Title}");
                            continue;
                        }
                        //if (this.modifyCheckBox.Checked) result.CheckOut();

                        //If the object has the principal Practice in its practices
                        PropertyValue pratique = result.GetProperty(pratiquePrpID);
                        Lookups pratiques = pratique.Value.GetValueAsLookups();
                        Lookups newPratiques = new Lookups();
                        if (pratique.Value.GetValueAsLookups().GetLookupIndexByItem(pratiquePrincipaleID) != -1)
                        {
                            foreach (Lookup pratiqueLook in pratiques)
                            {
                                if (pratiqueLook.ItemGUID == pratSecObj.ToLookup().ItemGUID) continue;
                                newPratiques.Add(-1, pratiqueLook);
                            }
                            if (this.modifyCheckBox.Checked) result.SaveProperty(pratiquePrpID, MFDataType.MFDatatypeMultiSelectLookup, newPratiques);
                            else result.SetProperty(pratiquePrpID, MFDataType.MFDatatypeMultiSelectLookup, newPratiques);
                        }
                        else
                        {
                            foreach (Lookup pratiqueLook in pratiques)
                            {
                                if (pratiqueLook.ItemGUID == pratSecObj.ToLookup().ItemGUID)
                                {
                                    Lookup lookup = new Lookup
                                    {
                                        Item = pratPrimObj.ID
                                    };
                                    newPratiques.Add(-1, lookup);
                                    continue;
                                }
                                newPratiques.Add(-1, pratiqueLook);
                            }
                            if (this.modifyCheckBox.Checked) result.SaveProperty(pratiquePrpID, MFDataType.MFDatatypeMultiSelectLookup, newPratiques);
                            else result.SetProperty(pratiquePrpID, MFDataType.MFDatatypeMultiSelectLookup, newPratiques);
                        }
                        //if (this.modifyCheckBox.Checked) result.CheckIn();
                    }
                    catch (Exception ex)
                    {
                        Console.Out.WriteLine($"Error while processing Client (ID:{result.ID}): {ex.Message}");
                        continue;
                    }
                }
                segment++;
                {
                    // If we get one item then there's more results.
                    MFSearchBuilder moreItemsSearcher = new MFSearchBuilder(this.selectedVaultServer);
                    moreItemsSearcher.ObjType(clientOTID);
                    moreItemsSearcher.Deleted(false);
                    moreItemsSearcher.ObjectId(segment * itemsPerSegment);
                    moreItems = 1 == moreItemsSearcher.FindEx(maxResults: 1).Count(); // We only need to know if there is at least one, nothing more.
                }
            }

            this.mainProgressBar.Invoke(new MethodInvoker(delegate { this.mainProgressBar.PerformStep(); }));
            this.totalCount.Invoke(new MethodInvoker(delegate { this.totalCount.Text = totalObjects.ToString(); }));
            if (objCount > 0)
                this.textBoxDocs.Invoke(new MethodInvoker(delegate { this.textBoxDocs.Text += $"Client : {objCount}\n"; }));
            UpdateUI();

            /***** ÉQUIPE-CLIENT *****/

            //Module Equipe-Client
            segment = 0;
            objCount = 0;
            moreItems = true;
            executeSearcher = new MFSearchBuilder(this.selectedVaultServer);
            executeSearcher.ObjType(eClientOTID);
            executeSearcher.Deleted(false);
            
            //resultsSearchBuilder = executeSearcher.Find();

            this.objectTypeLabel.Invoke(new MethodInvoker(delegate { this.objectTypeLabel.Text = "Équipe-Client"; }));
            UpdateUI();

            while (moreItems)
            {
                executeSearcher.SegmentedCount(segment, segmentSize: itemsPerSegment);
                executeSearcher.Property(pratiquePrpID, MFDataType.MFDatatypeMultiSelectLookup, pratiqueSecondaireID);
                resultsObjVerEx = executeSearcher.Find().GetAsObjectVersions().ToObjVerExs(this.selectedVaultServer);

                foreach (ObjVerEx result in resultsObjVerEx)
                {
                    try
                    {
                        if (result.Class != this.equipeClientClassID) continue;

                        Console.Out.WriteLine($"Processing d'équipe-client: {result.ID}");
                        if (this.selectedVaultServer.ObjectOperations.IsObjectCheckedOut(result.ObjID, true))
                        {
                            string[] textBoxCOTexts = new string[0];
                            this.textBoxCO.Invoke(new MethodInvoker(delegate { textBoxCOTexts = this.textBoxCO.Text.Split('\n'); }));
                            if (textBoxCOTexts.Where(x => x.Contains($"{result.ObjID.Type}-{result.ObjID.ID}")).FirstOrDefault() == null) this.checkedOutIDs.Add($"{result.ObjID.Type}-{result.ObjID.ID}, Équipe {result.Title}");
                            continue;
                        }
                        objCount++;
                        totalObjects++;

                        string eqName = result.Title;
                        int referenceCount = 0;

                        //Print how many objects reference the EQ
                        foreach (ObjType objT in objectTypes)
                        {
                            List<ObjVerEx> objectsToChange = result.GetIndirectReferences(objType: objT.ID);
                            foreach (ObjVerEx objVerEx in objectsToChange)
                            {
                                //Place a count of every object referencing the equipe-client
                                if (this.selectedVaultServer.ObjectOperations.IsObjectCheckedOut(objVerEx.ObjID, true))
                                {
                                    string[] textBoxCOTexts = new string[0];
                                    this.textBoxCO.Invoke(new MethodInvoker(delegate { textBoxCOTexts = this.textBoxCO.Text.Split('\n'); }));
                                    if (/*!textBoxCOTexts.Contains($"{objVerEx.ObjID.Type}-{objVerEx.ObjID.ID}")*/textBoxCOTexts.Where(x => x.Contains($"{result.ObjID.Type}-{result.ObjID.ID}")).FirstOrDefault() == null) this.checkedOutIDs.Add($"{objVerEx.ObjID.Type}-{objVerEx.ObjID.ID}, relié à l'Équipe {result.Title}");
                                }
                                referenceCount++;
                            }
                        }

                        PropertyValue clientPV = result.GetProperty(clientPrpID);
                        ObjVerEx client = clientPV.Value.GetValueAsLookup().ToObjVerEx(this.selectedVaultServer);
                        PropertyValue intervenantsClient = new PropertyValue();
                        if (client.TryGetProperty(intervenantsPrpID, out PropertyValue intClient)) intervenantsClient = intClient;

                        PropertyValue eqPV = client.GetProperty(equipeClientPrpID);
                        ObjVerEx eqPrimaire = eqPV.Value.GetValueAsLookups().ToObjVerExs(this.selectedVaultServer).Where(x =>
                                                    x.Title.Contains(pratPrimObj.Title)).FirstOrDefault();
                        PropertyValue intervenantsEQPrim = new PropertyValue();
                        if (eqPrimaire != null && eqPrimaire.TryGetProperty(intervenantsPrpID, out PropertyValue intervenantPrimPV)) intervenantsEQPrim = intervenantPrimPV;

                        //If the client has another client-team with the principal practice
                        if (/*client.TryGetProperty(equipeClientPrpID, out PropertyValue eqPV) && !eqPV.Value.IsNULL() //On sait que equipeClient n'est pas vide sur le client
                            && eqPV.Value.GetValueAsLookups().ToObjVerExs(this.selectedVaultServer).Where(x =>  //If another client-practice in the client has the primary practice
                            x.Title.Contains(pratPrimObj.Title)).FirstOrDefault() != null*/ eqPrimaire != null)
                        {
                            /*ObjVerEx eqPrimaire = eqPV.Value.GetValueAsLookups().ToObjVerExs(this.selectedVaultServer).Where(x =>
                                                    x.Title.Contains(pratPrimObj.Title)).First();*/

                            //Transfer intervenants to primary client-practice
                            if (result.TryGetProperty(intervenantsPrpID, out PropertyValue intervenantSecPV) && !intervenantSecPV.Value.IsNULL()
                                && !intervenantsEQPrim.Value.IsNULL())
                            {
                                Lookups intervenantsLookups = intervenantSecPV.Value.GetValueAsLookups();
                                Lookups adjustedIntervenants = intervenantsEQPrim.Value.GetValueAsLookups();
                                foreach (Lookup intervenant in intervenantsLookups)
                                {
                                    if (adjustedIntervenants.GetLookupIndexByItem(intervenant.Item) == -1)
                                    {
                                        Lookup lookup = new Lookup
                                        {
                                            Item = intervenant.Item
                                        };
                                        adjustedIntervenants.Add(-1, lookup);
                                    }
                                    continue;
                                }
                                if (this.modifyCheckBox.Checked)
                                {
                                    eqPrimaire.SaveProperty(intervenantsPrpID, MFDataType.MFDatatypeMultiSelectLookup, adjustedIntervenants);
                                }
                                else eqPrimaire.SetProperty(intervenantsPrpID, MFDataType.MFDatatypeMultiSelectLookup, adjustedIntervenants);
                            }

                            //Remove client-practice from client
                            Lookups equipesClient = eqPV.Value.GetValueAsLookups();
                            Lookups remainingEquipesClient = new Lookups();
                            foreach (Lookup equipeClient in equipesClient)
                            {
                                if (equipeClient.ItemGUID == result.Info.ObjectGUID) continue;
                                remainingEquipesClient.Add(-1, equipeClient);
                            }
                            if (this.modifyCheckBox.Checked) client.SaveProperty(equipeClientPrpID, MFDataType.MFDatatypeMultiSelectLookup, remainingEquipesClient);
                            else client.SetProperty(equipeClientPrpID, MFDataType.MFDatatypeMultiSelectLookup, remainingEquipesClient);

                        }
                        else
                        {   //Remplace la pratique secondaire par la pratique primaire
                            Lookups pratiques = result.GetProperty(pratiquePrpID).Value.GetValueAsLookups();
                            Lookups newPratiques = new Lookups();
                            foreach (Lookup pratiqueLook in pratiques)
                            {
                                if (pratiqueLook.ItemGUID == pratSecObj.ToLookup().ItemGUID)
                                {
                                    Lookup lookup = new Lookup
                                    {
                                        Item = pratPrimObj.ID
                                    };
                                    newPratiques.Add(-1, lookup);
                                    continue;
                                }
                                newPratiques.Add(-1, pratiqueLook);
                            }
                            if (this.modifyCheckBox.Checked) result.SaveProperty(pratiquePrpID, MFDataType.MFDatatypeMultiSelectLookup, newPratiques);
                            else result.SetProperty(pratiquePrpID, MFDataType.MFDatatypeMultiSelectLookup, newPratiques);
                        }

                        if (intervenantsEQPrim.Value.Value != null)
                        {
                            //Update les intervenants du client
                            Lookups intervenantsClientPrésent = intervenantsClient.Value.GetValueAsLookups();
                            Lookups intervenantsPrimLookups = intervenantsEQPrim.Value.GetValueAsLookups();
                            foreach (Lookup iPLook in intervenantsPrimLookups)
                            {
                                if (intervenantsClientPrésent.GetLookupIndexByItem(iPLook.Item) == -1)
                                {
                                    Lookup lookup = new Lookup
                                    {
                                        Item = iPLook.Item
                                    };
                                    intervenantsClientPrésent.Add(-1, lookup);
                                }
                                continue;
                            }

                            if (this.modifyCheckBox.Checked) client.SaveProperty(intervenantsPrpID, MFDataType.MFDatatypeMultiSelectLookup, intervenantsClientPrésent);
                            else client.SetProperty(intervenantsPrpID, MFDataType.MFDatatypeMultiSelectLookup, intervenantsClientPrésent);
                        }

                        //if (this.modifyCheckBox.Checked) result.CheckIn();
                        this.textBoxEQ.Invoke(new MethodInvoker(delegate { this.textBoxEQ.Text += $"{eqName}: {referenceCount}\n"; }));
                        UpdateUI();
                        continue;
                    }
                    catch (Exception ex)
                    {
                        Console.Out.WriteLine($"Error while processing Équipe-Client (ID:{result.ID}): {ex.Message}");
                        continue;
                    }
                }
                segment++;
                {
                    // If we get one item then there's more results.
                    MFSearchBuilder moreItemsSearcher = new MFSearchBuilder(this.selectedVaultServer);
                    moreItemsSearcher.ObjType(eClientOTID);
                    moreItemsSearcher.Deleted(false);
                    moreItemsSearcher.ObjectId(segment * itemsPerSegment);
                    moreItems = 1 == moreItemsSearcher.FindEx(maxResults: 1).Count(); // We only need to know if there is at least one, nothing more.
                }
            }

            this.mainProgressBar.Invoke(new MethodInvoker(delegate { this.mainProgressBar.PerformStep(); }));
            this.totalCount.Invoke(new MethodInvoker(delegate { this.totalCount.Text = totalObjects.ToString(); }));
            if (objCount > 0)
                this.textBoxDocs.Invoke(new MethodInvoker(delegate { this.textBoxDocs.Text += $"Équipe-Client : {objCount}\n"; }));
            UpdateUI();

            /*****THE REST OF THE OBJECT TYPES *****/

            // Iterate over the object types to count the objects.
            foreach (var objectType in objectTypes)
            {
                if (objectType.ID == clientOTID) continue;
                this.objectTypeLabel.Invoke(new MethodInvoker(delegate { this.objectTypeLabel.Text = objectType.NameSingular; }));
                UpdateUI();

                // Create the basic search conditions collection.
                objCount = 0;
                executeSearcher = new MFSearchBuilder(this.selectedVaultServer);

                // Add a condition for the object type we're interested in.

                executeSearcher.ObjType(objectType.ID);
                executeSearcher.Deleted(false);

                // Create variables for the segment information.

                segment = 0; // Start; this will increment as we go.
                moreItems = true; // Whether there are more items to load.

                ObjVerChanges primChanges = new ObjVerChanges(pratPrimObj);
                ObjVerEx lastestPrimPractice = primChanges.NewVersion;

                // Whilst there are items in the results, we need to loop.
                while (moreItems)
                {
                    // Execute a search within the object id segment.
                    {
                        executeSearcher.SegmentedCount(segment, segmentSize: itemsPerSegment);

                        executeSearcher.Property(pratiquePrpID, MFDataType.MFDatatypeMultiSelectLookup, pratiqueSecondaireID);
                        resultsObjVerEx = executeSearcher.Find().GetAsObjectVersions().ToObjVerExs(this.selectedVaultServer);

                        // Execute the search and increment the count.

                        foreach (ObjVerEx result in resultsObjVerEx)
                        {
                            try
                            {
                                Console.Out.WriteLine($"Processing de {objectType.NameSingular}: {result.ID}");
                                objCount++;
                                totalObjects++;
                                if (result.Class == hubClassID || objectType.NameSingular == "Tâche" || result.Class == employeNBClassID || result.Class == equipeClientClassID) continue;
                                if (this.selectedVaultServer.ObjectOperations.IsObjectCheckedOut(result.ObjID, true))
                                {
                                    string[] textBoxCOTexts = new string[0];
                                    this.textBoxCO.Invoke(new MethodInvoker(delegate { textBoxCOTexts = this.textBoxCO.Text.Split('\n'); }));
                                    if (textBoxCOTexts.Where(x => x.Contains($"{result.ObjID.Type}-{result.ObjID.ID}")).FirstOrDefault() == null) this.checkedOutIDs.Add($"{result.ObjID.Type}-{result.ObjID.ID}, {objectType.NameSingular} {result.Title}");
                                    continue;
                                }
                                //if (this.modifyCheckBox.Checked) result.CheckOut();

                                //If the object has the principal Practice in its practices
                                PropertyValue pratique = result.GetProperty(pratiquePrpID);
                                Lookups pratiques = pratique.Value.GetValueAsLookups();
                                if (pratique.Value.GetValueAsLookups().GetLookupIndexByItem(pratiquePrincipaleID) != -1)
                                {
                                    Lookups newPratiques = new Lookups();
                                    foreach (Lookup pratiqueLook in pratiques)
                                    {
                                        if (pratiqueLook.ItemGUID == pratSecObj.ToLookup().ItemGUID) continue;
                                        newPratiques.Add(-1, pratiqueLook);
                                    }
                                    if (this.modifyCheckBox.Checked) result.SaveProperty(pratiquePrpID, MFDataType.MFDatatypeMultiSelectLookup, newPratiques);
                                    else result.SetProperty(pratiquePrpID, MFDataType.MFDatatypeMultiSelectLookup, newPratiques);
                                }
                                else
                                {
                                    Lookups newPratiques = new Lookups();
                                    foreach (Lookup pratiqueLook in pratiques)
                                    {
                                        if (pratiqueLook.ItemGUID == pratSecObj.ToLookup().ItemGUID)
                                        {
                                            Lookup lookup = new Lookup
                                            {
                                                Item = pratPrimObj.ID
                                            };
                                            newPratiques.Add(-1, lookup);
                                            continue;
                                        }
                                        newPratiques.Add(-1, pratiqueLook);
                                    }
                                    if (this.modifyCheckBox.Checked) result.SaveProperty(pratiquePrpID, MFDataType.MFDatatypeMultiSelectLookup, newPratiques);
                                    else result.SetProperty(pratiquePrpID, MFDataType.MFDatatypeMultiSelectLookup, newPratiques);
                                }
                                //if (this.modifyCheckBox.Checked) result.CheckIn();
                            }
                            catch (Exception ex)
                            {
                                Console.Out.WriteLine($"Error while processing Object (Type: {objectType.NameSingular}, ID:{result.ID}): {ex.Message}");
                                continue;
                            }
                        }
                        // Move to the next segment.
                        segment++;
                    }

                    // Are there any more items?
                    {
                        // If we get one item then there's more results.
                        MFSearchBuilder moreItemsSearcher = new MFSearchBuilder(this.selectedVaultServer);
                        moreItemsSearcher.ObjType(objectType.ID);
                        moreItemsSearcher.Deleted(false);
                        moreItemsSearcher.ObjectId(segment * itemsPerSegment);
                        moreItems = 1 == moreItemsSearcher.FindEx(maxResults: 1).Count(); // We only need to know if there is at least one, nothing more.
                    }

                }
                this.mainProgressBar.Invoke(new MethodInvoker(delegate { this.mainProgressBar.PerformStep(); }));
                this.totalCount.Invoke(new MethodInvoker(delegate { this.totalCount.Text = totalObjects.ToString(); }));
                if (objCount > 0)
                    this.textBoxDocs.Invoke(new MethodInvoker(delegate { this.textBoxDocs.Text += $"{objectType.NameSingular} : {objCount}\n"; }));
            }
            this.objectTypeLabel.Invoke(new MethodInvoker(delegate { this.objectTypeLabel.Text = "Processus complété!"; }));
            UpdateUI();
            stopwatch.Stop();
            Console.Out.WriteLine($"Total Elapsed Time: {stopwatch.Elapsed}");
            stopwatch.Reset();
        }

        private void searchLoop()
        {
            const int itemsPerSegment = 10000; // Maximum number of items in each segment.

            var objectTypes = this.selectedVaultServer
                .ObjectTypeOperations
                .GetObjectTypes()
                .Cast<ObjType>()
                .ToList();

            int totalObjects = 0;
            this.mainProgressBar.Invoke(new MethodInvoker(delegate { this.mainProgressBar.Step = 2; }));
            //this.mainProgressBar.Step = 2;
            this.mainProgressBar.Invoke(new MethodInvoker(delegate { this.mainProgressBar.Maximum = 2 * objectTypes.Count(); }));
            //this.mainProgressBar.Maximum = 2 * objectTypes.Count();

            MFSearchBuilder executeSearcher = new MFSearchBuilder(this.selectedVaultServer);
            var segment = 0;
            var objCount = 0;


            //Operate on Clients
            executeSearcher.ObjType(clientOTID);
            executeSearcher.Deleted(false);
            executeSearcher.SegmentedCount(segment, segmentSize: itemsPerSegment);
            executeSearcher.Property(pratiquePrpID, MFDataType.MFDatatypeMultiSelectLookup, pratiqueSecondaireID);
            //ObjectSearchResults resultsSearchBuilder = executeSearcher.Find();

            this.objectTypeLabel.Invoke(new MethodInvoker(delegate { this.objectTypeLabel.Text = "Client"; }));

            List<ObjVerEx> resultsObjVerEx = executeSearcher.Find().GetAsObjectVersions().ToObjVerExs(this.selectedVaultServer);
            foreach (ObjVerEx result in resultsObjVerEx)
            {
                Console.Out.WriteLine($"Recherche du Client: {result.ID}");
                objCount++;
                totalObjects++;
                if (this.selectedVaultServer.ObjectOperations.IsObjectCheckedOut(result.ObjID, true))
                {
                    string[] textBoxCOTexts = new string[0];
                    this.textBoxCO.Invoke(new MethodInvoker(delegate { textBoxCOTexts = this.textBoxCO.Text.Split('\n'); }));
                    if (!textBoxCOTexts.Contains($"{result.ObjID.Type}-{result.ObjID.ID}")) this.checkedOutIDs.Add($"{result.ObjID.Type}-{result.ObjID.ID}");
                    continue;
                }
            }
            this.mainProgressBar.Invoke(new MethodInvoker(delegate { this.mainProgressBar.PerformStep(); }));
            this.totalCount.Invoke(new MethodInvoker(delegate { this.totalCount.Text = totalObjects.ToString(); }));
            if (objCount > 0)
                this.textBoxDocs.Invoke(new MethodInvoker(delegate { this.textBoxDocs.Text += $"Clients : {objCount}\n"; }));
            UpdateUI();
            //Operate on Equipe-Clients
            segment = 0;
            objCount = 0;

            executeSearcher = new MFSearchBuilder(this.selectedVaultServer);
            executeSearcher.ObjType(eClientOTID);
            executeSearcher.Deleted(false);
            executeSearcher.SegmentedCount(segment, segmentSize: itemsPerSegment);
            executeSearcher.Property(pratiquePrpID, MFDataType.MFDatatypeMultiSelectLookup, pratiqueSecondaireID);
            //resultsSearchBuilder = executeSearcher.Find();

            this.objectTypeLabel.Invoke(new MethodInvoker(delegate { this.objectTypeLabel.Text = "Équipe-Client"; }));

            resultsObjVerEx = executeSearcher.Find().GetAsObjectVersions().ToObjVerExs(this.selectedVaultServer);
            foreach (ObjVerEx result in resultsObjVerEx)
            {
                Console.Out.WriteLine($"Recherche de l'équipe-client: {result.ID}");
                objCount++;
                totalObjects++;
                if (this.selectedVaultServer.ObjectOperations.IsObjectCheckedOut(result.ObjID, true))
                {
                    string[] textBoxCOTexts = new string[0];
                    this.textBoxCO.Invoke(new MethodInvoker(delegate { textBoxCOTexts = this.textBoxCO.Text.Split('\n'); }));
                    if (!textBoxCOTexts.Contains($"{result.ObjID.Type}-{result.ObjID.ID}")) this.checkedOutIDs.Add($"{result.ObjID.Type}-{result.ObjID.ID}"); 
                    continue;
                }
                if (result.TryGetProperty(clientPrpID, out PropertyValue clientPV) && !clientPV.Value.IsNULL())
                {
                    string eqName = result.Title;
                    int referenceCount = 0;
                    foreach (ObjType objT in objectTypes)
                    {
                        List<ObjVerEx> objectsToChange = result.GetIndirectReferences(objType: objT.ID);
                        foreach (ObjVerEx objVerEx in objectsToChange)
                        {
                            //Place a count of every object referencing the equipe-client
                            if (this.selectedVaultServer.ObjectOperations.IsObjectCheckedOut(objVerEx.ObjID, true))
                            {
                                string[] textBoxCOTexts = new string[0];
                                this.textBoxCO.Invoke(new MethodInvoker(delegate { textBoxCOTexts = this.textBoxCO.Text.Split('\n'); }));
                                if (!textBoxCOTexts.Contains($"{objVerEx.ObjID.Type}-{objVerEx.ObjID.ID}")) this.checkedOutIDs.Add($"{objVerEx.ObjID.Type}-{objVerEx.ObjID.ID}");
                            }
                            referenceCount++;
                        }
                    }
                    this.textBoxEQ.Invoke(new MethodInvoker(delegate { this.textBoxEQ.Text += $"{eqName}: {referenceCount}\n"; }));
                    UpdateUI();
                }
            }
            this.mainProgressBar.Invoke(new MethodInvoker(delegate { this.mainProgressBar.PerformStep(); }));
            this.totalCount.Invoke(new MethodInvoker(delegate { this.totalCount.Text = totalObjects.ToString(); }));
            if (objCount > 0)
                this.textBoxDocs.Invoke(new MethodInvoker(delegate { this.textBoxDocs.Text += $"Équipe-Clients : {objCount}\n"; }));
            UpdateUI();

            // Iterate over the object types to count the objects.
            foreach (var objectType in objectTypes)
            {
                if (objectType.ID == clientOTID || objectType.ID == eClientOTID) continue; 
                
                this.objectTypeLabel.Invoke(new MethodInvoker(delegate { this.objectTypeLabel.Text = objectType.NameSingular; }));
                UpdateUI();

                objCount = 0;
                executeSearcher = new MFSearchBuilder(this.selectedVaultServer);

                executeSearcher.ObjType(objectType.ID);
                executeSearcher.Deleted(false);
                // Create variables for the segment information.

                segment = 0; // Start; this will increment as we go.
                var moreItems = true; // Whether there are more items to load.


                // Whilst there are items in the results, we need to loop.
                while (moreItems)
                {
                    // Execute a search within the object id segment.
                    {

                        executeSearcher.SegmentedCount(segment, segmentSize: itemsPerSegment);

                        executeSearcher.Property(pratiquePrpID, MFDataType.MFDatatypeMultiSelectLookup, pratiqueSecondaireID);

                        resultsObjVerEx = executeSearcher.Find().GetAsObjectVersions().ToObjVerExs(this.selectedVaultServer);

                        // Execute the search and increment the count.

                        foreach (ObjVerEx result in resultsObjVerEx)
                        {
                            Console.Out.WriteLine($"Recherche de {objectType.NameSingular}: {result.ID}");
                            objCount++;
                            totalObjects++;
                            if (result.Class == hubClassID || objectType.NameSingular == "Tâche" || result.Class == employeNBClassID) continue;
                            if (this.selectedVaultServer.ObjectOperations.IsObjectCheckedOut(result.ObjID, true))
                            {
                                string[] textBoxCOTexts = new string[0];
                                this.textBoxCO.Invoke(new MethodInvoker(delegate { textBoxCOTexts = this.textBoxCO.Text.Split('\n'); }));
                                if (!textBoxCOTexts.Contains($"{result.ObjID.Type}-{result.ObjID.ID}")) this.checkedOutIDs.Add($"{result.ObjID.Type}-{result.ObjID.ID}");
                                continue;
                            }
                        }
                        segment++;

                    }

                    // Are there any more items?
                    {
                        MFSearchBuilder moreItemsSearcher = new MFSearchBuilder(this.selectedVaultServer);
                        moreItemsSearcher.ObjType(objectType.ID);
                        moreItemsSearcher.Deleted(false);
                        moreItemsSearcher.ObjectId(segment * itemsPerSegment);
                        moreItems = 1 == moreItemsSearcher.FindEx(maxResults: 1).Count();
                    }

                }

                // Output the stats.
                this.mainProgressBar.Invoke(new MethodInvoker(delegate { this.mainProgressBar.PerformStep(); }));
                this.totalCount.Invoke(new MethodInvoker(delegate { this.totalCount.Text = totalObjects.ToString(); }));
                if (objCount > 0)
                    this.textBoxDocs.Invoke(new MethodInvoker(delegate { this.textBoxDocs.Text += $"{objectType.NameSingular} : {objCount}\n"; }));
            }
            this.objectTypeLabel.Invoke(new MethodInvoker(delegate { this.objectTypeLabel.Text = "Processus complété!"; }));
            UpdateUI();
            stopwatch.Stop();
            Console.Out.WriteLine($"Total Elapsed Time: {stopwatch.Elapsed}");
            stopwatch.Reset();
        }

        private void UpdateUI()
        {
            this.mainProgressBar.Invoke(new MethodInvoker(delegate { this.mainProgressBar.Update(); }));
            this.objectTypeLabel.Invoke(new MethodInvoker(delegate { this.objectTypeLabel.Update(); }));
            this.totalCount.Invoke(new MethodInvoker(delegate { this.totalCount.Update(); }));
            this.textBoxDocs.Invoke(new MethodInvoker(delegate { this.textBoxDocs.Update(); }));
            this.textBoxEQ.Invoke(new MethodInvoker(delegate { this.textBoxEQ.Update(); }));
            foreach (string str in this.checkedOutIDs)
            {
                this.textBoxCO.Invoke(new MethodInvoker(delegate { this.textBoxCO.Text += $"{str}\n"; }));
                //this.CheckedOutLabel.Invoke(new MethodInvoker(delegate { this.CheckedOutLabel.Text += $"{str}\n"; }));
            }
            this.checkedOutIDs.Clear();
            this.textBoxCO.Invoke(new MethodInvoker(delegate { this.textBoxCO.Update(); }));
        }

        private void OBSOLETEDONOTUSE()
        {
            //Replace secondary client-practice references with the primary client-practice
            /*foreach (ObjType objT in objectTypes)
            {
                List<ObjVerEx> objectsToChange = result.GetIndirectReferences(objType: objT.ID);
                foreach (ObjVerEx objVerEx in objectsToChange)
                {
                    referenceCount++;
                    if (this.selectedVaultServer.ObjectOperations.IsObjectCheckedOut(objVerEx.ObjID, true))
                    {
                        string[] textBoxCOTexts = new string[0];
                        this.textBoxCO.Invoke(new MethodInvoker(delegate { textBoxCOTexts = this.textBoxCO.Text.Split('\n'); }));
                        if (!textBoxCOTexts.Contains($"{objVerEx.ObjID.Type}-{objVerEx.ObjID.ID}")) this.checkedOutIDs.Add($"{objVerEx.ObjID.Type}-{objVerEx.ObjID.ID}");
                        continue;
                    }
                    if (!objVerEx.TryGetProperty(equipeClientPrpID, out PropertyValue objEQ) || objEQ.Value.IsNULL()) continue;
                    //if (this.modifyCheckBox.Checked) objVerEx.CheckOut();
                    Lookups currentLookups = objEQ.Value.GetValueAsLookups();
                    Lookups newLookups = new Lookups();
                    foreach (Lookup lookup in currentLookups)
                    {
                        if (lookup.ItemGUID == result.Info.ObjectGUID)
                        {
                            if (currentLookups.GetLookupIndexByItem(result.ID) == -1)
                            {
                                Lookup primEQ = new Lookup()
                                {
                                    Item = eqPrimaire.ID
                                };
                                newLookups.Add(-1, primEQ);
                            }
                            continue;
                        }

                        newLookups.Add(-1, lookup);
                    }
                    if (this.modifyCheckBox.Checked) objVerEx.SaveProperty(equipeClientPrpID, MFDataType.MFDatatypeMultiSelectLookup, newLookups);
                    else objVerEx.SetProperty(equipeClientPrpID, MFDataType.MFDatatypeMultiSelectLookup, newLookups);
                    //if (this.modifyCheckBox.Checked) objVerEx.CheckIn();
                }
            }*/
        }
    }
}
