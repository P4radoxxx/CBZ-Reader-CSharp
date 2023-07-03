using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;
using System.Reflection;

namespace TFTIC_P4radoxCBZReader
{
    public partial class MainWindow : Form
    {

        public MainWindow()
        {
            
            InitializeComponent();
            Application.ApplicationExit += new EventHandler(this.QuitParInterfaceFenetre);
        }

        // Liste des chemins des images extraites, seront recupérés dans les fonctions d'affichages
        public List<string> AdresseImgExtraites = new List<string>();
        int IndexListImg;

        private void Form1_Load(object sender, EventArgs e)
        {


            // Code à executer au chargement de la MainWindow(), probablement rien...

        }


        
        
        
        private void ouvrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string dossierTemp = Path.GetTempPath();
            string cheminDossierTemp = Path.Combine(dossierTemp, "TempCBZFolder"); // Appdata/local/temp/TempCBZFolder

            // Crée un repertoire temporaire pour extraire nos images
            Directory.CreateDirectory(cheminDossierTemp);

            // Check si le rep existe
            if (!Directory.Exists(cheminDossierTemp))
            {
                MessageBox.Show("Impossible de créer un dossier temporaire :( ");
            }

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Choix du fichier CBZ";
            openFileDialog.Filter = "Fichiers CBZ|*.cbz";




            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string chemin = openFileDialog.FileName;

                try
                {
                    System.IO.Compression.ZipFile.ExtractToDirectory(chemin, cheminDossierTemp);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erreur pendant l'extraction de l'archive: " + ex.Message);
                }

                DirectoryInfo directoryInfo = new DirectoryInfo(cheminDossierTemp);

                // Traverse tous les répertoires  au cas ou les fichiers seraient dans un sous rep.
                FileInfo[] CbzImage = directoryInfo.GetFiles("*", SearchOption.AllDirectories);

                foreach (FileInfo CbzImages in CbzImage)
                {
                    string extension = CbzImages.Extension.ToLower();
                    if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif")
                    {
                        AdresseImgExtraites.Add(CbzImages.FullName);
                        
                    }
                }

                // Debug
                //Console.WriteLine($"{AdresseImgExtraites.Count} Images chargées dans la liste");

                MessageBox.Show(" Archive extraite ! Appuyez sur la flêche suivante pour commencer la lecture :3");
            }
        }




        private void quitterToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string dossierTemp = Path.GetTempPath();
            string cheminDossierTemp = Path.Combine(dossierTemp, "TempCBZFolder");

            // Vide la PictureBox
            DemoImages.Image?.Dispose();
            DemoImages.Image = null;

            // JUste pour s'assurer que la PB soit bien vide avant de supprimer le dossier temp et de quitter l'app.
            System.Threading.Thread.Sleep(2000);

            if (Directory.Exists(cheminDossierTemp))
            {
                Directory.Delete(cheminDossierTemp, true);
            }
            Application.Exit();
   
        }







        // Fonction affichage image précedente
        private void button1_Click(object sender, EventArgs e)
        {
            if (IndexListImg > 0)
            {
                IndexListImg--;
                DemoImages.Image?.Dispose();
                DemoImages.Image = Image.FromFile(AdresseImgExtraites[IndexListImg]);
            }
            else
            {
                MessageBox.Show("Pas d'image avant celle la!");
            }
        }

        // Image suivante
        private void button2_Click(object sender, EventArgs e)
        {
            if (IndexListImg < AdresseImgExtraites.Count - 1)
            {
                IndexListImg++;
                DemoImages.Image?.Dispose();
                DemoImages.Image = Image.FromFile(AdresseImgExtraites[IndexListImg]);
            }
            else
            {
                MessageBox.Show("Plus d'images après celle la !");
            }
        }




        // Affichage box TFTIC
        private void tFTICToolStripMenuItem_Click(object sender, EventArgs e)
        {

            MessageBox.Show("Hello TFTIC from Cyril 'P4radox' Bouvier");

        }



        // Trigger d'event winform pour l'objet PictureBox, useless, la PB ne sera utilisée que comme support d'affichage à travers ses fonctions.
        private void DemoImages_Click(object sender, EventArgs e)
        {

        }


        // Delete le dossier temporaire quand l'app est quittée depuis l'interface de la fenêtre, pasted from above...
        private void QuitParInterfaceFenetre(object sender, EventArgs e)
        {
            string dossierTemp = Path.GetTempPath();
            string cheminDossierTemp = Path.Combine(dossierTemp, "TempCBZFolder");

            
            DemoImages.Image?.Dispose();
            DemoImages.Image = null;

            
            System.Threading.Thread.Sleep(2000);

            if (Directory.Exists(cheminDossierTemp))
            {
                Directory.Delete(cheminDossierTemp, true);
            }
        }

    }
}
