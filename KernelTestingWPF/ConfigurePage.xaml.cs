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

            cbPolicy.Items.Add("Policy 1");
            cbPolicy.Items.Add("Policy 2");
            cbPolicy.Items.Add("Policy 3");

            cbFastCores.SelectedIndex = 0;
            cbSlowCores.SelectedIndex = 0;
            cbPolicy.SelectedIndex = 0;
        }

        private void clearText()
        {
            txtInfo.Text = "";
            txtProgram.Text = "";
        }

        private void GoToRunningButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RunningPage());
        }

        private void FindFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();

            if (openFile.ShowDialog() == true)
            {
                txtProgram.Text = openFile.FileName + "\n\n";
                txtProgram.Text += File.ReadAllText(openFile.FileName);
            }
        }

    }
}
