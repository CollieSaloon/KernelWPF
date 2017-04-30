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
        Scheduler scheduler; // malarky
        List<Core> cores; // malarky
        List<bool> killCores = new List<bool>(); // VERY MUCH malarky

        List<ListView> listviews = new List<ListView>();

        string fileName;
        int numFastCores;
        int numSlowCores;
        int policyType;

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

        public RunningPage()
        {
            InitializeComponent();
        }
        ~RunningPage()
        {
            scheduler.Stop();

            foreach (Core c in cores)
            {
                c.Stop();
            }
        }

        private void InitializeCoresAndScheduler(int numFast, int numSlow, string filename, int policy) // malarky
        {
            cores = new List<Core>();

            for(int i = 0; i < numFast; i++) // add fast cores
                cores.Add(new Core(true, txtInfo, listviews));

            for (int i = 0; i < numSlow; i++) // add slow cores
                cores.Add(new Core(false, txtInfo, listviews));

            for (int i = 0; i < cores.Count; i++)
            {
                killCores.Add(new bool());
                killCores[killCores.Count - 1] = false;
            }
            
            scheduler = new Scheduler(cores, listviews, filename, policy);//

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
            if (fileName == null || fileName == "")
            {
                Console.WriteLine("File DNE!");
            }
            else
            {
                txtInfo.Text = "";
                string[] isolated = fileName.Split('\\');
                txtTitle.Text += isolated[isolated.Length - 1];
                InitializeCoresAndScheduler(numFastCores, numSlowCores, fileName, policyType);
                AddListView();
            }
        }

        private void GoToReportButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ReportPage());
        }

        private void AddListViewButton_Click(object sender, RoutedEventArgs e)
        {
            //AddListView();
        }

        private void ClearScrollView()
        {
            //ScrollStackPanel.Children.Clear();
            //myScrollView.Content = ScrollStackPanel;
        }

        private void AddListView()
        {
            int fastCount = 0;
            int slowCount = 0;

            for (int i = 0; i < cores.Count; i++)
            {
                ListView newListView = new ListView();
                newListView.HorizontalContentAlignment = HorizontalAlignment.Center;
                newListView.Width = 120;

                if (cores[i].GetIsFast())
                {
                    fastCount++;
                    newListView.Items.Add(string.Format("{0} Core #{1}", "Fast", fastCount));//, cores[i].GetId()));
                }
                else
                {
                    slowCount++;
                    newListView.Items.Add(string.Format("{0} Core #{1}", "Slow", slowCount));//, cores[i].GetId()));
                }

                listviews.Add(newListView);
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
