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
using System.Threading;

namespace KernelTestingWPF
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class RunningPage : Page
    {
        Scheduler scheduler; // malarky

        List<ListView> listviews = new List<ListView>();

        string fileName;
        int numFastCores;
        int numSlowCores;
        int policyType;


        public bool inputIsFast, outputIsFast, computationIsFast, registerIsFast;
        public float percentFast, percentSlow;

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

        private void InitializeCoresAndScheduler()
        {
            int fastCount = 0;
            int slowCount = 0;

            scheduler = new Scheduler(fileName, policyType);//
            scheduler.listViewInstructions = listViewInstructions;

            CoreManager.InitializeCores(numFastCores, numSlowCores, fileName, policyType,txtInfo);

            for (int i = 0; i < CoreManager.cores.Count; i++)
            {
                ListView lv = new ListView();

                if(CoreManager.IsFast(i))
                {
                    fastCount++;
                    CoreManager.cores[i].SetCoreListView(lv,fastCount);
                }
                else
                {
                    slowCount++;
                    CoreManager.cores[i].SetCoreListView(lv,slowCount);
                }
                              

                ScrollStackPanel.Children.Add(lv);
            }


            myScrollView.Content = ScrollStackPanel;

            scheduler.Start();

            CoreManager.StartCores();

        }

        protected void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        private void NavigationService_LoadCompleted(object sender, NavigationEventArgs e)
        {

        }

        public void setPage()
        {
            listViewInstructions.Items.Add("Instruction Queue");

            if (fileName == null || fileName == "")
            {
                Console.WriteLine("File DNE!");
            }
            else
            {
                txtInfo.Text = "";
                string[] isolated = fileName.Split('\\');
                txtTitle.Text += isolated[isolated.Length - 1];
                InitializeCoresAndScheduler();
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
            ScrollStackPanel.Children.Clear();
            myScrollView.Content = ScrollStackPanel;
        }

        private ListViewItem changeStatusListViewItem()
        {
            ListViewItem item = new ListViewItem();
            return item;
        }

        private ListViewItem changeExecListViewItem()
        {
            ListViewItem item = new ListViewItem();
            return item;
        }

        private void ClearScrollPanelButton_Click(object sender, RoutedEventArgs e)
        {
            ClearScrollView();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CoreManager.ChangeSpeed(-0.1f);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CoreManager.ChangeSpeed(0.1f);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }
    }
}
