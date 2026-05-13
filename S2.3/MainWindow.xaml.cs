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
                    } else if (!int.TryParse(e.Text, out result) && e.Text != ".")
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
                    } else if (!int.TryParse(e.Text, out result) && e.Text != "/")
                    {
                        // Bloquer la saisie si le caractère saisi n'est pas un chiffre ou un slash
                        e.Handled = true;
                        MessageBox.Show("Saisie bloquée car le caractère saisi n'est pas un chiffre ni un slash.");
                    } else if (txtBox.CaretIndex == 0 && e.Text != "/")
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
        private string[] _placeholderTextBlock = ["TxtClass", "TxtNetwork", "TxtFirstIP", "TxtLastIP", "TxtBroadcast", "TxtIP", "TxtMachines"];

        private void BtnCalcul_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < _placeholderTextBlock.Length; i++)
            {
                TextBlock txtBlock = (TextBlock)this.FindName(_placeholderTextBlock[i]);
                if (txtBlock != null)
                {
                    txtBlock.Text = "Erreur !";
                }
            }
        }
    }
}