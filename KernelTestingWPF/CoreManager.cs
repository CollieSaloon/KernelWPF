using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace KernelTestingWPF
{
    class CoreManager
    {

        static public List<Core> cores = new List<Core>();
        private static int totalCoreNum = 0;


        public static void InitializeCores(int numFast, int numSlow, string filename, int policy,TextBlock txtInfo) // malarky
        {
            

            totalCoreNum = numFast + numSlow;

            int index = 0;

            

            for (int i = 0; i < numFast; i++,index++) //add fast cores
            {
                //ListView listView = new ListView();
             //   listView.Width = 120;
                //listView.HorizontalAlignment = HorizontalAlignment.Center;
                cores.Add(new Core(true, txtInfo, i));
                //sp.Children.Add(listView);
            }

            for (int i = 0; i < numSlow; i++,index++) // add slow cores
            {
         //       ListView listView = new ListView();
         ////       listView.Width = 120;
         //       listView.HorizontalAlignment = HorizontalAlignment.Center;
                cores.Add(new Core(false, txtInfo, i));
                //sp.Children.Add(listView);
            }

                   
        }

        public static void StartCores()
        {
            foreach (Core c in cores)
                c.Start();
        }

        public static void StopCores()
        {
            foreach (Core c in cores)
            {
                c.Stop();
            }
        }

        public static void EnqueueAt(int index, Instruction instruction)
        {
            cores[index].Enqueue(instruction);
        }

        public static int TotalCoreNum()
        {
            return totalCoreNum;
        }

        public static bool IsFast(int index)
        {
            return cores[index].GetIsFast();
        }
    }
}
