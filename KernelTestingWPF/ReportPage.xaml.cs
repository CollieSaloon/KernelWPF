
using System;
using System.Collections.Generic;
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
    /// Interaction logic for ReportPage.xaml
    /// </summary>
    public partial class ReportPage : Page
    {
        public ReportPage()
        {
            InitializeComponent();
            SetPage();
            
        }

        private void SetPage()
        {
            listViewReport1.Items.Add("Core");
            listViewReport2.Items.Add("Total Execution Time");
            listViewReport3.Items.Add("Total Power Consumption");
            listViewReport4.Items.Add("Processes Executed");

            for (int i = 0; i < CoreManager.TotalCoreNum(); i++)
            {
                listViewReport1.Items.Add((CoreManager.IsFast(i) ? "Fast" : "Slow") + " Core #" + (i + 1 - (!CoreManager.IsFast(i) ? CoreManager.numFast : 0)));
                listViewReport2.Items.Add(CoreManager.GetExecTime(i) + " ms");
                listViewReport3.Items.Add(CoreManager.GetPowerConsumption(i) + " W");
                listViewReport4.Items.Add(CoreManager.GetNumProcs(i));
            }
        }

        private void GoToConfigureButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
