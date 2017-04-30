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
        static public float SpeedMultiplier = 0.5f;
        static public List<Core> cores = new List<Core>();
        private static int totalCoreNum = 0;
        public static int QueueLimit = 1;

        public static void ChangeSpeed(float dspeed)
        {
            SpeedMultiplier += dspeed;
        }

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
            {
                Console.WriteLine("Starting Core!");
                c.Start();
            }
        }

        public static int GetQueueAmount(int index)
        {
            return cores[index].GetQueueAmount();
        }
        public static int GetMinOfType(Instruction.I_TYPE type, bool restrict = false, bool fast = false)
        {
            int index = -1;

            

            return index;
        }

        public static int GetMinPercentage(bool restrict = false, bool fast = false)
        {
            List<float> result = new List<float>();
            List<int> indices = new List<int>();
            int index = -1;

            if (restrict)
            {
                int total = 0;
                for (int i = 0, j = 0; i < cores.Count; i++) // first count
                {
                    if (cores[i].GetIsFast() != fast)
                        continue;

                    result.Add(new float());
                    indices.Add(i);

                    result[j] = cores[i].GetQueueAmount();
                    total += (int)result[j];
                    j++;
                }
                for (int i = 0; i < result.Count; i++) // then get proportions
                {
                    result[i] /= result.Count;
                }
            }
            else
            {
                int total = 0;
                for (int i = 0; i < cores.Count; i++) // first count
                {
                    result.Add(new float());
                    indices.Add(i);
                    result[i] = cores[i].GetQueueAmount();
                    total += (int)result[i];
                }
                for (int i = 0; i < result.Count; i++) // then get proportions
                {
                    result[i] /= result.Count;
                }
            }

            float min = 100000;

            for (int i = 0; i < result.Count; i++)
            {
                if (result[i] < min)
                {
                    min = result[i];
                    index = indices[i];
                }
            }

            return index;
        }

        // used for that stuff
        public static List<float> GetCorePercentages(bool restrict=false, bool fast=false)
        {
            List<float> result = new List<float>();

            if (restrict)
            {
                int total = 0;
                for (int i = 0, j = 0; i < cores.Count; i++) // first count
                {
                    if (cores[i].GetIsFast() != fast)
                        continue;

                    result.Add(new float());
                    result[j] = cores[i].GetQueueAmount();
                    total += (int)result[j];
                    j++;
                }
                for (int i = 0; i < result.Count; i++) // then get proportions
                {
                    result[i] /= total;
                }
            }
            else
            {
                int total = 0;
                for (int i = 0; i < cores.Count; i++) // first count
                {
                    result.Add(new float());
                    result[i] = cores[i].GetQueueAmount();
                    total += (int)result[i];
                }
                for (int i = 0; i < cores.Count; i++) // then get proportions
                {
                    result[i] /= total;
                }
            }

            return result;
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
