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
        public static int percentFast, percentSlow;
        public static List<Core> fastCores = new List<Core>();
        public static List<Core> slowCores = new List<Core>();
        public static Queue<Instruction> fastInstructions = new Queue<Instruction>();
        public static Queue<Instruction> slowInstructions = new Queue<Instruction>();
        private int fastAvaiableCount = 0;
        private int slowAvailableCount = 0;
        

        public static void ChangeSpeed(float dspeed)
        {
            SpeedMultiplier += dspeed;
        }
        public static void SetRatio(int fast, int slow)
        {
            percentFast = fast;
            percentSlow = slow;
        }

        public static void InitializeCores(int numFast, int numSlow, string filename, int policy,TextBlock txtInfo) // malarky
        {
            

            totalCoreNum = numFast + numSlow;

            int index = 0;
            

            for (int i = 0; i < numFast; i++,index++) //add fast cores
            {            
                cores.Add(new Core(true, txtInfo, i));
            }

            for (int i = 0; i < numSlow; i++,index++) // add slow cores
            {
                cores.Add(new Core(false, txtInfo, i));
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

        public static int GetMinPercentageIndex()
        {
            int index = -1;
            List<float> queueRatio = new List<float>();
            List<int> queueSum = new List<int>();

            float percentShownFast = 0;
            float percentShownSlow = 0;

            float percentCores = 0;
            float percentSystem= percentFast / (percentFast + percentSlow);
            //float percentSystemSlow = percentSlow / (percentFast + percentSlow);
            float percentSum = 0;

            int minFast, minSlow, minFastIndex, minSlowIndex;
            minFast = minSlow = 100000;
            minFastIndex = minSlowIndex = -1;
            for (int i = 0; i < cores.Count; i++)
            {
                queueSum.Add(cores[i].GetQueueAmount()); // get queue count
            }
            for (int i = 0; i < cores.Count; i++)
            {
                queueRatio.Add(queueSum[i] / (queueSum.Count >= 1 ? queueSum.Count : 1)); // convert it to percentage
                percentSum += queueRatio[i]; // sum percentages
            }
            for (int i = 0; i < cores.Count && queueRatio.Count >= 1; i++)
            {
                if (cores[i].GetIsFast())
                {
                    if (queueSum[i] <= minFast)
                    {
                        minFast = queueSum[i];
                        minFastIndex = i;
                    }
                    percentShownFast += queueRatio[i]; // add the ratio to totalDesired
                }
                else
                {
                    if (queueSum[i] <= minSlow)
                    {
                        minSlow = queueSum[i];
                        minSlowIndex = i;
                    }
                    percentShownSlow += queueRatio[i]; // add the ratio to totalDesired
                }
            }
            float percentShown = (percentShownFast + percentShownSlow >= 0) ? (percentShownFast / (percentShownFast + percentShownSlow)) : 0;

            if (percentShown <= percentSystem)
            {
                index = minFastIndex;
            }
            else
            {
                index = minSlowIndex;
            }

            return index;
        }

        public static void FastSlowBrother()
        { 
}

        // used for that stuff
        /*
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
        // */

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
