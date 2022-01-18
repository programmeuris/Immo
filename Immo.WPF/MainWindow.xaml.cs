using System;
using System.Collections.Generic;
// using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Immo.Models;
using Immo.DAL;

namespace Immo.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // private data
        private readonly FileOperations _fileOperations = new FileOperations();
        private readonly List<Eigendom> _eigendommen = new List<Eigendom>();
        private EigendomType _type;

        public MainWindow()
        {
            InitializeComponent();
            InitProgram();
        }

        // private methods
        private void InitProgram()
        {
            // read data from file
            try
            {
                _eigendommen.AddRange(_fileOperations.BestandInlezen("eigendommen.txt"));
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                _fileOperations.FoutLoggen(ex);
            }

            // add data to listbox
            lbEigendommen.ItemsSource = _eigendommen;

            // init default type to eigendom
            rbEigendom.IsChecked = true;
            UpdateUI();
            //rbAppartement.IsChecked = false;
            //rbHuis.IsChecked = false;
        }

        private bool ValidateFields()
        {
            var bob = new StringBuilder();

            if (string.IsNullOrWhiteSpace(txtAdres.Text))
            {
                bob.AppendLine("Gelieve een adres in te vullen.");
            }
            if (!double.TryParse(txtOppervlakte.Text.Trim(), out double _))
            {
                bob.AppendLine("Gelieve een (numerieke) oppervlakte in te vullen.");
            }

            // weird selection here because price had to be int
            // and huurprijs was double...
            // used multiple switches for better selection, otherwise i had to copy a lot of code
            // using switch because they autofill when used with enum
            switch (_type)
            {
                case EigendomType.Eigendom:
                case EigendomType.Huis:
                    if (!int.TryParse(txtPrijs.Text.Trim(), out int _))
                    {
                        bob.AppendLine("Gelieve een (numerieke) prijs in te vullen.");
                    }

                    break;
                case EigendomType.Appartement:
                    if (!double.TryParse(txtPrijs.Text.Trim(), out double _))
                    {
                        bob.AppendLine("Gelieve een (numerieke) huurprijs in te vullen.");
                    }

                    break;
            }

            switch (_type)
            {
                case EigendomType.Huis:
                case EigendomType.Appartement:
                    if (!int.TryParse(txtBouwjaar.Text.Trim(), out int _))
                    {
                        bob.AppendLine("Gelieve een (numeriek) bouwjaar in te vullen.");
                    }

                    if (!int.TryParse(txtBewoonbareOppervlakte.Text.Trim(), out int _))
                    {
                        bob.AppendLine("Gelieve een (numerieke) bewoonbare oppervlakte in te vullen.");
                    }
                    break;
            }

            if (!string.IsNullOrWhiteSpace(bob.ToString()))
            {
                MessageBox.Show(bob.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void UpdateUI()
        {
            switch (_type)
            {
                case EigendomType.Eigendom:
                    // disable bouwjaar and bewoonbare opp
                    txtBouwjaar.IsEnabled = false;
                    txtBewoonbareOppervlakte.IsEnabled = false;
                    txtBouwjaar.Text = "";
                    txtBewoonbareOppervlakte.Text = "";

                    lblPrijs.Content = "Prijs";

                    break;
                case EigendomType.Huis:
                    txtBouwjaar.IsEnabled = true;
                    txtBewoonbareOppervlakte.IsEnabled = true;

                    lblPrijs.Content = "Prijs";

                    break;
                case EigendomType.Appartement:
                    txtBouwjaar.IsEnabled = true;
                    txtBewoonbareOppervlakte.IsEnabled = true;

                    lblPrijs.Content = "Huurprijs";

                    break;
                default:
                    break;
            }
        }

        private Eigendom CreateEigendom()
        {
            Eigendom eigendom = null;

            try
            {
                switch (_type)
                {
                    case EigendomType.Eigendom:
                        eigendom = new Eigendom(
                            int.Parse(
                            txtPrijs.Text.Trim()),
                            double.Parse(txtOppervlakte.Text.Trim()),
                            txtAdres.Text.Trim());

                        //return eigendom;
                        break;
                    case EigendomType.Huis:
                        eigendom = new Huis(
                            int.Parse(
                            txtPrijs.Text.Trim()),
                            double.Parse(txtOppervlakte.Text.Trim()),
                            txtAdres.Text.Trim(),
                            int.Parse(txtBouwjaar.Text.Trim()),
                            int.Parse(txtBewoonbareOppervlakte.Text.Trim()));

                        //return eigendom;
                        break;
                    case EigendomType.Appartement:
                        eigendom = new Appartement(
                            int.Parse(
                            txtPrijs.Text.Trim()),
                            double.Parse(txtOppervlakte.Text.Trim()),
                            txtAdres.Text.Trim(),
                            int.Parse(txtBouwjaar.Text.Trim()),
                            int.Parse(txtBewoonbareOppervlakte.Text.Trim()));

                        //return eigendom;
                        break;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                _fileOperations.FoutLoggen(ex);
            }

            return eigendom;
        }

        // event handlers
        private void RDB_Click(object sender, RoutedEventArgs e)
        {
            RadioButton btn = sender as RadioButton;

            switch (btn.Name)
            {
                case "rbEigendom":
                    _type = EigendomType.Eigendom;

                    break;
                case "rbHuis":
                    _type = EigendomType.Huis;

                    break;
                case "rbAppartement":
                    _type = EigendomType.Appartement;

                    break;
            }

            UpdateUI();
        }

        private void BTN_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            switch (btn.Name)
            {
                case "btnRegistreren":
                    if (ValidateFields())
                    {
                        var eigendom = CreateEigendom();
                        if (_fileOperations.EigendomIsUniq(_eigendommen, eigendom))
                        {
                            lbEigendommen.ItemsSource = null;
                            _eigendommen.Add(eigendom);
                            lbEigendommen.ItemsSource = _eigendommen;

                            txtAdres.Text = "";
                            txtBewoonbareOppervlakte.Text = "";
                            txtBouwjaar.Text = "";
                            txtOppervlakte.Text = "";
                            txtPrijs.Text = "";
                        }
                        else
                        {
                            if (eigendom != null)
                            {
                                MessageBox.Show("Deze eigendom is reeds geregistreerd!");
                            }
                        }
                    }
                    break;
                case "btnDetails":
                    if (lbEigendommen.SelectedIndex != -1)
                    {
                        MessageBox.Show(_eigendommen[lbEigendommen.SelectedIndex].Overzicht(), "", MessageBoxButton.OK);
                    }
                    else
                    {
                        MessageBox.Show("Gelieve een item te selecteren");
                    }
                    break;
            }
        }
    }
}
