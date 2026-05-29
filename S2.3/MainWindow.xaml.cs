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

        // _placeholderTextBox : placeholders affichés par défaut dans les TextBox
        private string[] _placeholderTextBox = ["Adresse IPv4 (déc.)", "/CIDR", "Adresse IPv4 (binaire)", "Masque standard (déc.)"];

        // TxtBox_GotFocus : efface le placeholder et met le texte en noir
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

        // TxtBox_LostFocus : restaure le placeholder et met le texte en gris
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

        // TxtBox_PreviewTextInput : valide la saisie selon la TextBox
        private void TxtBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox txtBox = (TextBox)sender;
            int result;
            switch (txtBox.Name)
            {
                case "TxtDec":
                    if (TxtBin.Text != _placeholderTextBox[2])
                    {
                        e.Handled = true;
                        MessageBox.Show("Saisie bloquée car il y a déjà une saisie binaire.");
                    }
                    else if (!int.TryParse(e.Text, out result) && e.Text != ".")
                    {
                        e.Handled = true;
                        MessageBox.Show("Saisie bloquée car le caractère saisi n'est pas un chiffre ni un point.");
                    }
                    break;

                case "TxtCIDR":
                    if (TxtMask.Text != _placeholderTextBox[3])
                    {
                        e.Handled = true;
                        MessageBox.Show("Saisie bloquée car il y a déjà une saisie de masque.");
                    }
                    else if (!int.TryParse(e.Text, out result) && e.Text != "/")
                    {
                        e.Handled = true;
                        MessageBox.Show("Saisie bloquée car le caractère saisi n'est pas un chiffre ni un slash.");
                    }
                    else if (txtBox.CaretIndex == 0 && e.Text != "/")
                    {
                        txtBox.Text = "/";
                        txtBox.CaretIndex = 1;
                    }
                    break;

                case "TxtBin":
                    if (TxtDec.Text != _placeholderTextBox[0])
                    {
                        e.Handled = true;
                        MessageBox.Show("Saisie bloquée car il y a déjà une saisie décimale.");
                    }
                    else if (e.Text != "0" && e.Text != "1" && e.Text != ".")
                    {
                        e.Handled = true;
                        MessageBox.Show("Saisie bloquée car le caractère saisi n'est pas un 0, un 1 ou un point.");
                    }
                    break;

                case "TxtMask":
                    if (TxtCIDR.Text != _placeholderTextBox[1])
                    {
                        e.Handled = true;
                        MessageBox.Show("Saisie bloquée car il y a déjà une saisie de CIDR.");
                    }
                    else if (!int.TryParse(e.Text, out result) && e.Text != ".")
                    {
                        e.Handled = true;
                        MessageBox.Show("Saisie bloquée car le caractère saisi n'est pas un chiffre ni un point.");
                    }
                    break;

                default:
                    break;
            }

        }

        // _placeholderTextBlock : noms des TextBlock à vider lors du reset
        private string[] _placeholderTextBlock = new string[] { "TxtClass", "TxtNetwork", "TxtFirstIP", "TxtLastIP", "TxtBroadcast", "TxtIP", "TxtMachines" };

        // BtnCalcul_Click : calcule réseau, broadcast, IPs et met à jour l'interface
        private void BtnCalcul_Click(object sender, RoutedEventArgs e)
        {
            byte[] ipBytes = new byte[4];
            byte[] maskBytes = new byte[4];
            int cidr = 24;
            bool ipValide = false;



            // Cas 1 : saisie décimale
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

                    // Construit la représentation binaire octet par octet
                    string versionBinaire = "";
                    for (int i = 0; i < 4; i++)
                    {
                        versionBinaire += Convert.ToString(ipBytes[i], 2).PadLeft(8, '0');
                        if (i < 3) versionBinaire += ".";
                    }
                    TxtBin.Text = versionBinaire;
                    TxtBin.Foreground = Brushes.Black;
                }
            }
            // Cas 2 : saisie binaire
            else if (TxtBin.Text != _placeholderTextBox[2] && !string.IsNullOrWhiteSpace(TxtBin.Text))
            {
                string binNettoye = TxtBin.Text.Replace(".", "").Trim();

                // Vérifie que la chaîne ne contient que des '0' et '1'
                bool queDuBinaire = true;
                foreach (char c in binNettoye)
                {
                    if (c != '0' && c != '1') queDuBinaire = false;
                }

                if (binNettoye.Length == 32 && queDuBinaire)
                {
                    // Découpe la chaîne en 4 octets de 8 bits et convertit en bytes
                    for (int i = 0; i < 4; i++)
                    {
                        string unOctetBin = binNettoye.Substring(i * 8, 8);
                        ipBytes[i] = Convert.ToByte(unOctetBin, 2);
                    }
                    ipValide = true;

                    TxtDec.Text = ipBytes[0] + "." + ipBytes[1] + "." + ipBytes[2] + "." + ipBytes[3];
                    TxtDec.Foreground = Brushes.Black;
                }
            }

            // Si l'IP n'est pas valide, réinitialise et quitte
            if (!ipValide)
            {
                MessageBox.Show("L'adresse IP saisie est invalide.", "Erreur");
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

            // Cas 1 : saisie CIDR (/24)
            if (TxtCIDR.Text != _placeholderTextBox[1] && !string.IsNullOrWhiteSpace(TxtCIDR.Text))
            {
                string cidrTexte = TxtCIDR.Text.Replace("/", "").Trim();
                if (int.TryParse(cidrTexte, out cidr) && cidr >= 0 && cidr <= 32)
                {
                    masqueValide = true;

                    // Calcule chaque octet du masque selon le CIDR
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

                    TxtMask.Text = maskBytes[0] + "." + maskBytes[1] + "." + maskBytes[2] + "." + maskBytes[3];
                    TxtMask.Foreground = Brushes.Black;
                }
            }
            // Cas 2 : saisie masque décimal (255.255.255.0)
            else if (TxtMask.Text != _placeholderTextBox[3] && !string.IsNullOrWhiteSpace(TxtMask.Text))
            {
                string[] morceauxMasque = TxtMask.Text.Split('.');
                if (morceauxMasque.Length == 4 &&
                    byte.TryParse(morceauxMasque[0], out maskBytes[0]) &&
                    byte.TryParse(morceauxMasque[1], out maskBytes[1]) &&
                    byte.TryParse(morceauxMasque[2], out maskBytes[2]) &&
                    byte.TryParse(morceauxMasque[3], out maskBytes[3]))
                {
                    // Vérifie la continuité des bits à 1 dans le masque
                    bool aRencontreZero = false;
                    bool structureValide = true;

                    for (int i = 0; i < 4; i++)
                    {
                        byte b = maskBytes[i];
                        for (int bit = 0; bit < 8; bit++)
                        {
                            bool bitEstAUn = (b & 128) == 128;

                            if (aRencontreZero && bitEstAUn)
                            {
                                structureValide = false;
                                break;
                            }

                            if (!bitEstAUn)
                            {
                                aRencontreZero = true;
                            }

                            b = (byte)(b << 1);
                        }
                        if (!structureValide) break;
                    }

                    if (!structureValide)
                    {
                        masqueValide = false;
                    }
                    else
                    {
                        masqueValide = true;

                        // Compte les bits à 1 pour récupérer le CIDR
                        int compteurCidr = 0;
                        for (int i = 0; i < 4; i++)
                        {
                            byte b = maskBytes[i];
                            for (int bit = 0; bit < 8; bit++)
                            {
                                if ((b & 128) == 128) compteurCidr++;
                                b = (byte)(b << 1);
                            }
                        }
                        cidr = compteurCidr;
                        TxtCIDR.Text = "/" + cidr;
                        TxtCIDR.Foreground = Brushes.Black;
                    }
                }
            }

            // Si le masque est invalide, réinitialise et quitte
            if (!masqueValide)
            {
                MessageBox.Show("Le masque de sous-réseau saisi est invalide.", "Erreur");
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

            // 1. Détermine la classe d'adresse (privée, loopback, multicast, etc.)
            string resultatClasse = "";

            if (ipBytes[0] == 127)
            {
                resultatClasse = "A (Loopback - RFC 5735)";
            }
            else if (ipBytes[0] == 169 && ipBytes[1] == 254)
            {
                resultatClasse = "B (APIPA - RFC 5735)";
            }
            else if (ipBytes[0] >= 224 && ipBytes[0] <= 239)
            {
                resultatClasse = "D (Multicast - RFC 5735)";
            }
            else if (ipBytes[0] == 100 && (ipBytes[1] >= 64 && ipBytes[1] <= 127))
            {
                resultatClasse = "A (CGN - RFC 6598)";
            }
            else if (ipBytes[0] == 10)
            {
                resultatClasse = "A (Privée non-routable)";
            }
            else if (ipBytes[0] == 172 && (ipBytes[1] >= 16 && ipBytes[1] <= 31))
            {
                resultatClasse = "B (Privée non-routable)";
            }
            else if (ipBytes[0] == 192 && ipBytes[1] == 168)
            {
                resultatClasse = "C (Privée non-routable)";
            }
            // Classes standards
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

            // Calcule l'adresse réseau (ip & masque) octet par octet
            byte[] netBytes = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                netBytes[i] = (byte)(ipBytes[i] & maskBytes[i]);
            }
            TxtNetwork.Text = netBytes[0] + "." + netBytes[1] + "." + netBytes[2] + "." + netBytes[3];

            // Calcule le broadcast (réseau | masque inversé) octet par octet
            byte[] broadBytes = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                byte masqueInverse = (byte)(~maskBytes[i] & 0xFF);
                broadBytes[i] = (byte)(netBytes[i] | masqueInverse);
            }
            TxtBroadcast.Text = broadBytes[0] + "." + broadBytes[1] + "." + broadBytes[2] + "." + broadBytes[3];

            // Calcule première/dernière IP et nombre d'hôtes selon le CIDR
            if (cidr <= 30)
            {
                // Première IP = réseau + 1
                TxtFirstIP.Text = netBytes[0] + "." + netBytes[1] + "." + netBytes[2] + "." + (netBytes[3] + 1);

                // Dernière IP = broadcast - 1
                TxtLastIP.Text = broadBytes[0] + "." + broadBytes[1] + "." + broadBytes[2] + "." + (broadBytes[3] - 1);

                double totalIP = Math.Pow(2, 32 - cidr);
                TxtIP.Text = totalIP.ToString("N0");
                TxtMachines.Text = (totalIP - 2).ToString("N0");
            }
            else
            {
                // /31 et /32 : pas d'hôtes utilisables
                TxtFirstIP.Text = "N/A";
                TxtLastIP.Text = "N/A";
                TxtIP.Text = Math.Pow(2, 32 - cidr).ToString("N0");
                TxtMachines.Text = "0";
            }

            // Affiche le bouton Reset et réajuste le bouton Calcul
            BtnReset.Visibility = Visibility.Visible;
            BtnCalcul.HorizontalAlignment = HorizontalAlignment.Left;
            BtnCalcul.Margin = new Thickness(100, 0, 0, 0);
        }

        // BtnReset_Click : remet tous les champs à leur état initial
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            TxtDec.Text = _placeholderTextBox[0];
            TxtCIDR.Text = _placeholderTextBox[1];
            TxtBin.Text = _placeholderTextBox[2];
            TxtMask.Text = _placeholderTextBox[3];
            TxtDec.Foreground = Brushes.Gray;
            TxtCIDR.Foreground = Brushes.Gray;
            TxtBin.Foreground = Brushes.Gray;
            TxtMask.Foreground = Brushes.Gray;
            
            foreach (string name in _placeholderTextBlock)
            {
                TextBlock txtBlock = (TextBlock)this.FindName(name);
                if (txtBlock != null)
                {
                    txtBlock.Text = "";
                }
            }
            
            BtnReset.Visibility = Visibility.Collapsed;
            BtnCalcul.HorizontalAlignment = HorizontalAlignment.Center;
            BtnCalcul.Margin = new Thickness(0);
        }
    }
}