using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace S2._3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Liste de chaînes de caractères utilisée dans les TextBox.
        private string[] _placeholderTextBox = ["Adresse IPv4 (déc.)", "/CIDR", "Adresse IPv4 (binaire)", "Masque standard (déc.)"];

        // Méthode TxtBox_GotFocus :
        // Efface le texte de l'élément TextBox et change la couleur du texte en noir
        // lorsque l'utilisateur clique sur le TextBox
        // et que le texte actuel correspond au texte de la liste _placeholderTextBox.
        private void TxtBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox txtBox = (TextBox)sender;
            for (int i = 0; i < _placeholderTextBox.Length; i++)
            {
                if (txtBox.Text == _placeholderTextBox[i])
                {
                    txtBox.Text = string.Empty;
                    txtBox.Foreground = Brushes.Black;
                    break;
                }
            }
        }

        // Méthode TxtBox_LostFocus :
        // Restaure le texte de l'élément TextBox et change la couleur du texte en gris
        // lorsque l'utilisateur clique en dehors du TextBox
        // et que le texte actuel est vide ou ne contient que des espaces blancs.
        private void TxtBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox txtBox = (TextBox)sender;
            switch (txtBox.Name)
            {
                case "TxtDec":
                    if (string.IsNullOrWhiteSpace(txtBox.Text))
                    {
                        txtBox.Text = _placeholderTextBox[0];
                        txtBox.Foreground = Brushes.Gray;
                    }
                    break;

                case "TxtCIDR":
                    if (string.IsNullOrWhiteSpace(txtBox.Text))
                    {
                        txtBox.Text = _placeholderTextBox[1];
                        txtBox.Foreground = Brushes.Gray;
                    }
                    break;

                case "TxtBin":
                    if (string.IsNullOrWhiteSpace(txtBox.Text))
                    {
                        txtBox.Text = _placeholderTextBox[2];
                        txtBox.Foreground = Brushes.Gray;
                    }
                    break;

                case "TxtMask":
                    if (string.IsNullOrWhiteSpace(txtBox.Text))
                    {
                        txtBox.Text = _placeholderTextBox[3];
                        txtBox.Foreground = Brushes.Gray;
                    }
                    break;

                default:
                    break;
            }
        }

        // Méthode TxtBox_PreviewTextInput :
        // Valide la saisie de l'utilisateur dans les TextBox en fonction de leur nom
        private void TxtBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox txtBox = (TextBox)sender;
            int result;
            switch (txtBox.Name)
            {
                case "TxtDec":
                    if (TxtBin.Text != _placeholderTextBox[2])
                    {
                        // Bloquer la saisie si il y a déjà du texte binaire dans TxtBin
                        e.Handled = true;
                        MessageBox.Show("Saisie bloquée car il y a déjà une saisie binaire.");
                    }
                    else if (!int.TryParse(e.Text, out result) && e.Text != ".")
                    {
                        // Bloquer la saisie si le caractère saisi n'est pas un chiffre ou un point
                        e.Handled = true;
                        MessageBox.Show("Saisie bloquée car le caractère saisi n'est pas un chiffre ni un point.");
                    }
                    break;

                case "TxtCIDR":
                    if (TxtMask.Text != _placeholderTextBox[3])
                    {
                        // Bloquer la saisie si il y a déjà une saisie de masque dans TxtMask
                        e.Handled = true;
                        MessageBox.Show("Saisie bloquée car il y a déjà une saisie de masque.");
                    }
                    else if (!int.TryParse(e.Text, out result) && e.Text != "/")
                    {
                        // Bloquer la saisie si le caractère saisi n'est pas un chiffre ou un slash
                        e.Handled = true;
                        MessageBox.Show("Saisie bloquée car le caractère saisi n'est pas un chiffre ni un slash.");
                    }
                    else if (txtBox.CaretIndex == 0 && e.Text != "/")
                    {
                        // Ajouter un slash au début de la saisie si l'utilisateur ne le fait pas lui-même
                        txtBox.Text = "/";
                        txtBox.CaretIndex = 1;
                    }
                    break;

                case "TxtBin":
                    if (TxtDec.Text != _placeholderTextBox[0])
                    {
                        // Bloquer la saisie si il y a déjà du texte décimal dans TxtDec
                        e.Handled = true;
                        MessageBox.Show("Saisie bloquée car il y a déjà une saisie décimale.");
                    }
                    else if (e.Text != "0" && e.Text != "1" && e.Text != ".")
                    {
                        // Bloquer la saisie si le caractère saisi n'est pas un 0, un 1 ou un point
                        e.Handled = true;
                        MessageBox.Show("Saisie bloquée car le caractère saisi n'est pas un 0, un 1 ou un point.");
                    }
                    break;

                case "TxtMask":
                    if (TxtCIDR.Text != _placeholderTextBox[1])
                    {
                        // Bloquer la saisie si il y a déjà une saisie de CIDR dans TxtCIDR
                        e.Handled = true;
                        MessageBox.Show("Saisie bloquée car il y a déjà une saisie de CIDR.");
                    }
                    else if (!int.TryParse(e.Text, out result) && e.Text != ".")
                    {
                        // Bloquer la saisie si le caractère saisi n'est pas un chiffre ou un point
                        e.Handled = true;
                        MessageBox.Show("Saisie bloquée car le caractère saisi n'est pas un chiffre ni un point.");
                    }
                    break;

                default:
                    break;
            }

        }

        // Liste des noms utilisée dans les TextBlock.
        private string[] _placeholderTextBlock = new string[] { "TxtClass", "TxtNetwork", "TxtFirstIP", "TxtLastIP", "TxtBroadcast", "TxtIP", "TxtMachines" };

        private void BtnCalcul_Click(object sender, RoutedEventArgs e)
        {
            // Variables de base sous forme de tableaux (Niveau BUT 1)
            byte[] ipBytes = new byte[4];
            byte[] maskBytes = new byte[4];
            int cidr = 24;
            bool ipValide = false;



            // Cas 1 : Saisie en Décimal
            if (TxtDec.Text != _placeholderTextBox[0] && !string.IsNullOrWhiteSpace(TxtDec.Text))
            {
                string[] morceaux = TxtDec.Text.Split('.');
                if (morceaux.Length == 4 &&
                    byte.TryParse(morceaux[0], out ipBytes[0]) &&
                    byte.TryParse(morceaux[1], out ipBytes[1]) &&
                    byte.TryParse(morceaux[2], out ipBytes[2]) &&
                    byte.TryParse(morceaux[3], out ipBytes[3]))
                {
                    ipValide = true;

                    // Traduction en binaire avec une boucle simple (Pas de LINQ complexe)
                    string versionBinaire = "";
                    for (int i = 0; i < 4; i++)
                    {
                        versionBinaire += Convert.ToString(ipBytes[i], 2).PadLeft(8, '0');
                        if (i < 3) versionBinaire += "."; // Ajoute le point séparateur d'octet
                    }
                    TxtBin.Text = versionBinaire;
                    TxtBin.Foreground = Brushes.Black;
                }
            }
            // Cas 2 : Saisie en Binaire
            else if (TxtBin.Text != _placeholderTextBox[2] && !string.IsNullOrWhiteSpace(TxtBin.Text))
            {
                string binNettoye = TxtBin.Text.Replace(".", "").Trim();

                // Vérification manuelle que la chaîne contient bien 32 caractères composés uniquement de 0 et de 1
                bool queDuBinaire = true;
                foreach (char c in binNettoye)
                {
                    if (c != '0' && c != '1') queDuBinaire = false;
                }

                if (binNettoye.Length == 32 && queDuBinaire)
                {
                    // Découpage par paquets de 8 bits
                    for (int i = 0; i < 4; i++)
                    {
                        string unOctetBin = binNettoye.Substring(i * 8, 8);
                        ipBytes[i] = Convert.ToByte(unOctetBin, 2); // Conversion de la base 2 vers le byte
                    }
                    ipValide = true;

                    // Affichage de la version décimale correspondante
                    TxtDec.Text = ipBytes[0] + "." + ipBytes[1] + "." + ipBytes[2] + "." + ipBytes[3];
                    TxtDec.Foreground = Brushes.Black;
                }
            }

            // Si l'IP n'a pas pu être validée, on arrête la fonction
            if (!ipValide)
            {
                MessageBox.Show("L'adresse IP saisie est invalide.", "Erreur");
                // Vider tous les TextBox
                TxtDec.Text = _placeholderTextBox[0];
                TxtCIDR.Text = _placeholderTextBox[1];
                TxtBin.Text = _placeholderTextBox[2];
                TxtMask.Text = _placeholderTextBox[3];
                TxtDec.Foreground = Brushes.Gray;
                TxtCIDR.Foreground = Brushes.Gray;
                TxtBin.Foreground = Brushes.Gray;
                TxtMask.Foreground = Brushes.Gray;
                return;
            }


            bool masqueValide = false;

            // Cas 1 : Saisie par le CIDR (Ex: /24)
            if (TxtCIDR.Text != _placeholderTextBox[1] && !string.IsNullOrWhiteSpace(TxtCIDR.Text))
            {
                string cidrTexte = TxtCIDR.Text.Replace("/", "").Trim();
                if (int.TryParse(cidrTexte, out cidr) && cidr >= 0 && cidr <= 32)
                {
                    masqueValide = true;

                    // Algorithme de calcul du masque octet par octet (Logique pure réseau)
                    int bitsRestants = cidr;
                    for (int i = 0; i < 4; i++)
                    {
                        if (bitsRestants >= 8)
                        {
                            maskBytes[i] = 255;
                            bitsRestants -= 8;
                        }
                        else if (bitsRestants > 0)
                        {
                            maskBytes[i] = (byte)(255 << (8 - bitsRestants));
                            bitsRestants = 0;
                        }
                        else
                        {
                            maskBytes[i] = 0;
                        }
                    }

                    // Affichage du masque décimal généré
                    TxtMask.Text = maskBytes[0] + "." + maskBytes[1] + "." + maskBytes[2] + "." + maskBytes[3];
                    TxtMask.Foreground = Brushes.Black;
                }
            }
            // Cas 2 : Saisie par le Masque Standard (Ex: 255.255.255.0)
            else if (TxtMask.Text != _placeholderTextBox[3] && !string.IsNullOrWhiteSpace(TxtMask.Text))
            {
                string[] morceauxMasque = TxtMask.Text.Split('.');
                if (morceauxMasque.Length == 4 &&
                    byte.TryParse(morceauxMasque[0], out maskBytes[0]) &&
                    byte.TryParse(morceauxMasque[1], out maskBytes[1]) &&
                    byte.TryParse(morceauxMasque[2], out maskBytes[2]) &&
                    byte.TryParse(morceauxMasque[3], out maskBytes[3]))
                {
                    masqueValide = true;

                    // Compter le CIDR manuellement en inspectant chaque bit de chaque octet
                    int compteurCidr = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        byte b = maskBytes[i];
                        for (int bit = 0; bit < 8; bit++)
                        {
                            if ((b & 128) == 128) compteurCidr++; // Si le bit le plus à gauche est à 1
                            b = (byte)(b << 1); // Décalage vers la gauche pour analyser le bit suivant
                        }
                    }
                    cidr = compteurCidr;
                    TxtCIDR.Text = "/" + cidr;
                    TxtCIDR.Foreground = Brushes.Black;
                }
            }

            // Si le masque n'a pas pu être validé, on arrête la fonction
            if (!masqueValide)
            {
                MessageBox.Show("Le masque de sous-réseau saisi est invalide.", "Erreur");
                // Vider tous les TextBox
                TxtDec.Text = _placeholderTextBox[0];
                TxtCIDR.Text = _placeholderTextBox[1];
                TxtBin.Text = _placeholderTextBox[2];
                TxtMask.Text = _placeholderTextBox[3];
                TxtDec.Foreground = Brushes.Gray;
                TxtCIDR.Foreground = Brushes.Gray;
                TxtBin.Foreground = Brushes.Gray;
                TxtMask.Foreground = Brushes.Gray;
                return;
            }


            // 1. Détermination de la Classe ET Prise en compte de la RFC 5735 et du document de SAE
            string resultatClasse = "";

            // A. Cas particuliers "À bloquer / Spéciaux" (Page 7 du sujet de SAE)
            if (ipBytes[0] == 127)
            {
                resultatClasse = "A (Loopback - RFC 5735)"; //
            }
            else if (ipBytes[0] == 169 && ipBytes[1] == 254)
            {
                resultatClasse = "B (APIPA - RFC 5735)"; //
            }
            else if (ipBytes[0] >= 224 && ipBytes[0] <= 239)
            {
                resultatClasse = "D (Multicast - RFC 5735)"; //
            }
            else if (ipBytes[0] == 100 && (ipBytes[1] >= 64 && ipBytes[1] <= 127))
            {
                resultatClasse = "A (CGN - RFC 6598)"; //
            }
            // B. Classes privées non-routables (Page 7 du sujet de SAE)
            else if (ipBytes[0] == 10)
            {
                resultatClasse = "A (Privée non-routable)"; //
            }
            else if (ipBytes[0] == 172 && (ipBytes[1] >= 16 && ipBytes[1] <= 31))
            {
                resultatClasse = "B (Privée non-routable)"; //
            }
            else if (ipBytes[0] == 192 && ipBytes[1] == 168)
            {
                resultatClasse = "C (Privée non-routable)"; //
            }
            // C. Classes standards classiques
            else if (ipBytes[0] >= 1 && ipBytes[0] <= 126)
            {
                resultatClasse = "A";
            }
            else if (ipBytes[0] >= 128 && ipBytes[0] <= 191)
            {
                resultatClasse = "B";
            }
            else if (ipBytes[0] >= 192 && ipBytes[0] <= 223)
            {
                resultatClasse = "C";
            }
            else
            {
                resultatClasse = "E (Expérimentale)";
            }

            TxtClass.Text = resultatClasse;

            // 2. Calcul de l'Adresse Réseau : Opération logique ET (ip & masque) sur chaque octet
            byte[] netBytes = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                netBytes[i] = (byte)(ipBytes[i] & maskBytes[i]);
            }
            TxtNetwork.Text = netBytes[0] + "." + netBytes[1] + "." + netBytes[2] + "." + netBytes[3];

            // 3. Calcul du Broadcast : Réseau OU (Masque Inversé) octet par octet
            byte[] broadBytes = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                byte masqueInverse = (byte)(~maskBytes[i] & 0xFF);
                broadBytes[i] = (byte)(netBytes[i] | masqueInverse);
            }
            TxtBroadcast.Text = broadBytes[0] + "." + broadBytes[1] + "." + broadBytes[2] + "." + broadBytes[3];

            // 4. Calcul de la Première IP, Dernière IP et Nombre de Machines
            if (cidr <= 30)
            {
                // Première IP : C'est l'adresse réseau + 1 sur le dernier octet
                TxtFirstIP.Text = netBytes[0] + "." + netBytes[1] + "." + netBytes[2] + "." + (netBytes[3] + 1);

                // Dernière IP : C'est l'adresse de broadcast - 1 sur le dernier octet
                TxtLastIP.Text = broadBytes[0] + "." + broadBytes[1] + "." + broadBytes[2] + "." + (broadBytes[3] - 1);

                // Quantité d'IP (2^(32-CIDR)) et quantité de machines (Total - 2)
                double totalIP = Math.Pow(2, 32 - cidr);
                TxtIP.Text = totalIP.ToString("N0");
                TxtMachines.Text = (totalIP - 2).ToString("N0");
            }
            else
            {
                // Cas particuliers pour les réseaux /31 et /32 où il n'y a pas d'IP machines valides
                TxtFirstIP.Text = "N/A";
                TxtLastIP.Text = "N/A";
                TxtIP.Text = Math.Pow(2, 32 - cidr).ToString("N0");
                TxtMachines.Text = "0";
            }

            // Affichage du bouton Reset
            BtnReset.Visibility = Visibility.Visible;
            // Décalage du bouton Calcul vers la gauche pour faire de la place au bouton Reset
            BtnCalcul.HorizontalAlignment = HorizontalAlignment.Left;
            BtnCalcul.Margin = new Thickness(100, 0, 0, 0);
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            // Vider tous les TextBox
            TxtDec.Text = _placeholderTextBox[0];
            TxtCIDR.Text = _placeholderTextBox[1];
            TxtBin.Text = _placeholderTextBox[2];
            TxtMask.Text = _placeholderTextBox[3];
            TxtDec.Foreground = Brushes.Gray;
            TxtCIDR.Foreground = Brushes.Gray;
            TxtBin.Foreground = Brushes.Gray;
            TxtMask.Foreground = Brushes.Gray;
            // Vider tous les TextBlock
            foreach (string name in _placeholderTextBlock)
            {
                TextBlock txtBlock = (TextBlock)this.FindName(name);
                if (txtBlock != null)
                {
                    txtBlock.Text = "";
                }
            }
            // Cacher le bouton Reset et recentrer le bouton Calcul
            BtnReset.Visibility = Visibility.Collapsed;
            BtnCalcul.HorizontalAlignment = HorizontalAlignment.Center;
            BtnCalcul.Margin = new Thickness(0);
        }
    }
}