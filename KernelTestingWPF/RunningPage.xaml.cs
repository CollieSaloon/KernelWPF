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
        // Globals here
        Scheduler scheduler; // malarky
        List<Core> cores; // malarky

        List<ListView> listviews = new List<ListView>();
        int fastCount = 1;
        int slowCount = 1;

        string fileName;
        int numFastCores;
        int numSlowCores;
        int policyType;

        string text;

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        public int NumFastCores
        {
            get { return numFastCores; }
            set { numFastCores = value; }
        }
        public int NumSlowCores
        {
            get { return numSlowCores; }
            set { numSlowCores = value; }

        }
        public int PolicyType
        {
            get { return policyType; }
            set { policyType = value; }
        }

        private NavigationContext nav = new NavigationContext();

        internal NavigationContext Nav { get => nav; set => nav = value; }

        public RunningPage()
        {
            InitializeComponent();
        }

        private void InitializeCoresAndScheduler(int numFast, int numSlow, string filename, int policy) // malarky
        {
            cores = new List<Core>();

            for(int i = 0; i < numFast; i++) // add fast cores
                cores.Add(new Core(true, txtInfo));

            for (int i = 0; i < numFast; i++) // add slow cores
                cores.Add(new Core(false, txtInfo));
            
            scheduler = new Scheduler(cores, filename, policy);//

            foreach (Core c in cores)
                c.Start();

            scheduler.Start();
        }

        protected void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        private void NavigationService_LoadCompleted(object sender, NavigationEventArgs e)
        {

        }

        public void setPage()
        {
            txtInfo.Text = "";
            string[] isolated = fileName.Split('\\');
            txtTitle.Text += isolated[isolated.Length - 1];

            InitializeCoresAndScheduler(numFastCores, numSlowCores, fileName, policyType);
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
            
            ScrollStackPanel.Children.Clear();
            myScrollView.Content = ScrollStackPanel;

        }

        private void AddListView()
        {
            string type = "";
            for (int i = 0; i < cores.Count; i++)
            {
                


                ListView newListView = new ListView();
                newListView.HorizontalContentAlignment = HorizontalAlignment.Center;
                newListView.Width = 150;
                newListView.Items.Add(string.Format("Core {0} #{1}",cores[i].GetIsFast() ? "Fast" : "Slow", cores[i].GetIsFast() ? i : i - fastCount));
                listviews.Add(newListView);
                fastCount++;
                ScrollStackPanel.Children.Add(newListView);

            }

            myScrollView.Content = ScrollStackPanel;
        }

        private void ClearScrollPanelButton_Click(object sender, RoutedEventArgs e)
        {
            ClearScrollView();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
