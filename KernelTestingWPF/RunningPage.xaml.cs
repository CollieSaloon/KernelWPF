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
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class RunningPage : Page
    {
        List<ListView> listviews = new List<ListView>();
        int numCore = 1;
     

        public RunningPage()
        {
            InitializeComponent();
            setPage();
       
        }

        private void setPage()
        {
           
            
        }

        private void GoToReportButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ReportPage());
        }

        private void AddListViewButton_Click(object sender, RoutedEventArgs e)
        {
            AddListView();
        }

        private void ClearScrollView()
        {
            numCore = 1;
            ScrollStackPanel.Children.Clear();
            myScrollView.Content = ScrollStackPanel;

        }

        private void AddListView()
        {
            ListView newListView = new ListView();
            newListView.HorizontalContentAlignment = HorizontalAlignment.Center;
            newListView.Width = 150;
            newListView.Items.Add("Core (Type) #" + numCore);
            listviews.Add(newListView);
            ScrollStackPanel.Children.Add(newListView);

            numCore++;

            myScrollView.Content = ScrollStackPanel;
        }

        private void ClearScrollPanelButton_Click(object sender, RoutedEventArgs e)
        {
            ClearScrollView();
        }
    }
}
