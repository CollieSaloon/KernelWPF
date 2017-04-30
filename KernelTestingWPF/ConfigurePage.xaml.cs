using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

namespace KernelTestingWPF
{
    /// <summary>
    /// Interaction logic for ConfigurePage.xaml
    /// </summary>
    public partial class ConfigurePage : Page
    {

        int comboNum = 5;
        string fileName;

       
        

        public ConfigurePage()
        {
            InitializeComponent();
            setPage();
        }

        private void setPage()
        {
            fillComboBoxes();
            clearText();
        }

        private void fillComboBoxes()
        {
            for (int i = 1; i <= comboNum; i++)
            {
                cbFastCores.Items.Add(i);
                cbSlowCores.Items.Add(i);
            }

            cbPolicy.Items.Add("Ratio Based");
            cbPolicy.Items.Add("Fast/Slow Brother");
            cbPolicy.Items.Add("Type-Based");
            cbPolicy.Items.Add("Limited Queue");

            cbInput.Items.Add("Fast");
            cbInput.Items.Add("Slow");
            cbOutput.Items.Add("Fast");
            cbOutput.Items.Add("Slow");
            cbComputations.Items.Add("Fast");
            cbComputations.Items.Add("Slow");
            cbRegister.Items.Add("Fast");
            cbRegister.Items.Add("Slow");


            setComboIndexes();
            
        }

        private void setComboIndexes()
        {
            cbFastCores.SelectedIndex = 0;
            cbSlowCores.SelectedIndex = 0;
            cbPolicy.SelectedIndex = 0;
            cbComputations.SelectedIndex = 0;
            cbInput.SelectedIndex = 0;
            cbOutput.SelectedIndex = 0;
            cbRegister.SelectedIndex = 0;

        }

        private void clearText()
        {
            //txtInfo.Text = "";
            txtProgram.Text = "";
        }

        private void GoToRunningButton_Click(object sender, RoutedEventArgs e)
        {
            RunningPage rp = new RunningPage();

            rp.NumFastCores = cbFastCores.SelectedIndex + 1;
            rp.NumSlowCores = cbSlowCores.SelectedIndex + 1;
            rp.PolicyType = cbPolicy.SelectedIndex;
            rp.FileName = fileName;

            rp.setPage();

            if (fileName != null)
            {
                NavigationService.Navigate(rp);
            }
        }

        private void FindFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            if (openFile.ShowDialog() == true)
            {
                txtProgram.Text = openFile.FileName + "\n\n";
                txtProgram.Text += File.ReadAllText(openFile.FileName);
                fileName = openFile.FileName;
            }
        }

        private void cbPolicy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            HideStackPanels();

            if(cbPolicy.SelectedIndex == 0)
            {
                Policy1StackPanel.Visibility = Visibility.Visible;
            }
            else if(cbPolicy.SelectedIndex == 2)
            {
                Policy3StackPanel.Visibility = Visibility.Visible;
            }
            else if(cbPolicy.SelectedIndex == 3)
            {
                Policy4StackPanel.Visibility = Visibility.Visible;
            }
            else
            {

            }
        }

        private void HideStackPanels()
        {
            Policy1StackPanel.Visibility = Visibility.Collapsed;
            Policy3StackPanel.Visibility = Visibility.Collapsed;
            Policy4StackPanel.Visibility = Visibility.Collapsed;
        }
    }
}
