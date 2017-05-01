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
        

        public static bool[] typesAreFast = { false, false, true, false }; // input, output, computational, registers
        public static List<bool> typesAreFastFull = new List<bool>();

        public static int numFast, numSlow;

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
            /*
            PRINT, // print out an int 
            PRINT_REG, // print out an int from a reg
            PRINT_CHAR, // print out a char (passed in as int)
            SET_REG, // takes in a register and a value
            SET_REG_REG, // takes in two registers
            ADD, // takes in three registers
            SUB, // takes in three registers
            MUL, // takes in three registers
            DIV, // takes in three registers
            NUM_TYPES // placeholder for size, also if an instruction is set to this, invalid
            // */
            // 0 - input // NOT USED
            // 1 - output
            // 2 - computation
            // 3 - registers
            typesAreFastFull.Add(typesAreFast[1]);
            typesAreFastFull.Add(typesAreFast[1]);
            typesAreFastFull.Add(typesAreFast[1]);
            typesAreFastFull.Add(typesAreFast[3]);
            typesAreFastFull.Add(typesAreFast[3]);

            typesAreFastFull.Add(typesAreFast[2]);
            typesAreFastFull.Add(typesAreFast[2]);
            typesAreFastFull.Add(typesAreFast[2]);
            typesAreFastFull.Add(typesAreFast[2]);
        }

        public static void StartCores()
        {
            foreach (Core c in cores)
            {
                Console.WriteLine("Starting Core!");
                c.Start();
            }
        }

        public static int GetPowerConsumption(int index)
        {
            return cores[index].totalPower;
        }

        public static int GetQueueAmount(int index)
        {
            return cores[index].GetQueueAmount();
        }
        public static int GetMinOfType(Instruction.I_TYPE type)
        {
            int index = -1;
            int min = 100000;

		for(int i = 0; i < cores.Count; i++)
            {
                int current = cores[i].GetQueueAmount();

                if (cores[i].GetIsFast() && !typesAreFastFull[(int)type])
                    current = 100000; // if it can't be done by preferred core type (fast/slow), cheese it
                else if (!cores[i].GetIsFast() && typesAreFastFull[(int)type])
                    current = 100000;
                
                if (current < min)
                {
                    min = current;
                    index = i;
                }
            }

            return index;
        }

        public static int GetMinPercentageIndex()
        {
            int index = -1;

            float desiredPercentFast = percentFast / 100;
            float desiredPercentSlow = percentSlow / 100;

            // Optimization: if all queues are empty, just throw it in a fast or slow based on desired

            bool allEmpty = false;
            foreach (Core c in cores)
            {
                if (c.GetQueueAmount() > 0)
                {
                    allEmpty = true;
                    break;
                }
            }

            if (allEmpty)
            {
                if (desiredPercentFast > desiredPercentSlow)
                {
                    for(int i = 0; i < cores.Count; i++)
                    {
                        if (cores[i].GetIsFast())
                            return i;
                    }
                }
                else
                {
                    for (int i = 0; i < cores.Count; i++)
                    {
                        if (!cores[i].GetIsFast())
                            return i;
                    }
                }
            }



            return index;
        }

        //fill cores
        public static bool DoFastInstruction()
        {
            bool doFast = false;

            int fastTimeStamp = -1;
            int slowTimeStamp = -1;
            
            if(fastInstructions.Count > 0)
            {
                fastTimeStamp = fastInstructions.Peek().timeStamp;
            }
            else
            {
                doFast = false;
            }

            if(slowInstructions.Count > 0)
            {
                slowTimeStamp = slowInstructions.Peek().timeStamp;
            }
            else
            {
                doFast = true;
            }

		    if(fastTimeStamp > slowTimeStamp)
            {
                doFast = true;
            }
            else if(slowTimeStamp > fastTimeStamp)
            {
                doFast = false;
            }

            return doFast;
		}

      
          
        

        public static void StopCores()
        {
            foreach (Core c in cores)
            {
                c.Stop();
            }
        }

        public static int GetNumProcs(int index)
        {
            return cores[index].numberProcessesRun;
        }
        public static int GetExecTime(int index)
        {
            return cores[index].totalTime;
        }

        public static void EnqueueAt(int index, Instruction instruction)
        { 
            cores[index].Enqueue(instruction);
        }

        //public static bool SlowEnqueue(Instruction instruction)
        //{
        //    bool canSlowQueue = false;

        //    if (checkSlowCoresAvailability() != -1)
        //    {
        //        canSlowQueue = true;
        //    }
        //}

        //public static bool FastEnqueue(Instruction instruction)
        //{
        //    bool canFastQueue = false;
        //    checkFastCoresAvailability();
        //}

        public static int checkSlowCoresAvailability()
        {
            int index = -1;

            for (int i = 0; i < slowCores.Count; i++)
            {
                if(slowCores[i].IsEmpty())
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        public static int checkFastCoresAvailability()
        {
            int index = -1;

            for (int i = 0; i < fastCores.Count; i++)
            {
                if (fastCores[i].IsEmpty())
                {
                    index = i;
                    break;
                }
            }
            return index;
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
