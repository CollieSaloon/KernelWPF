using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace KernelTestingWPF
{
    class Scheduler
    {
        public const bool PRS_DEBUG = true;
        public enum P_TYPE // policy types
        {
            ALL_FAST, // allocate every instruction to fast cores
            ALL_SLOW, // allocate every instruction to slow cores
            TYPE_DEPENDENT, // allocate inputs and registers to slow cores, outputs and math to fast
            CORE_AVAILABILITY_F, // allocate as many as possible to fast, then slow
            CORE_AVAILABILITY_S, // allocate as many as possible to slow, then fast
            NUM_TYPES // placeholder for size
        };

        string filename;
        Thread scheduler;
        List<Instruction> totalQueue;
        private int policy; // scheduling policy used by this core
        public int Policy
        {
            get; set;
        }

        public Scheduler(List<Core> allCores, string filename, int policy = 0)
        {
            totalQueue = new List<Instruction>();
            this.policy = policy;
            scheduler = new Thread((() => { DoScheduling(allCores); }));
            this.filename = filename;
        }

        public void Start()
        {
            scheduler.Start();
        }

        public void DoScheduling(List<Core> allCores)
        {
            //if (PRS_DEBUG) Console.WriteLine("Scheduler|DoScheduling: please enter a filename:");
            //string s = Console.ReadLine();
            ParseForInstructions(filename);

            while (true)
            {
                //Console.WriteLine("Scheduler|DoScheduling: starting to empty my queue.");
                while (totalQueue.Count > 0)
                {
                    switch (policy)
                    {
                        case (int)P_TYPE.ALL_FAST:
                            //for all cores, if it is a fast core and it is not full, allocate this instruction
                            for (int i = 0; i < allCores.Count; i++)
                            {
                                if (allCores[i].GetIsFast() && !allCores[i].IsFull())
                                {
                                    allCores[i].Enqueue(totalQueue[0]);
                                    totalQueue.RemoveAt(0); // dequeue
                                    break;
                                }
                            }
                            break;
                        case (int)P_TYPE.ALL_SLOW:
                            break;
                        case (int)P_TYPE.TYPE_DEPENDENT:
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public void ParseForInstructions(string filename)
        {
            //filename = "C:\\CKT_INFO\\code.txt"; // malarky
            FileStream stream;
            try
            {
                stream = File.Open(filename, FileMode.Open, FileAccess.Read);
            }
            catch
            {
                Console.WriteLine("Can't read file! Kill me.");
                return;
            }
            StreamReader reader = new StreamReader(stream);
            if (PRS_DEBUG) Console.WriteLine("Scheduler|Parser: reading from file \"" + filename + "\"");
            while (!reader.EndOfStream)
            {
                string s = reader.ReadLine();
                //if (PRS_DEBUG) Console.WriteLine("Scheduler|Parser: read Line: \"" + s + "\"");

                string[] sSplit = s.Split(' ');
                //if (PRS_DEBUG) Console.WriteLine("Length of this Line: " + sSplit.Length);
                //if (PRS_DEBUG) Console.Write("Type of this Line: ");
                //if (PRS_DEBUG) Console.WriteLine(sSplit[0]);
                if (sSplit.Length <= 0 || sSplit.Length > 4)
                {
                    Console.WriteLine("0Invalid line!");
                    continue;
                }
                else if (sSplit.Length == 2)
                {
                    if (sSplit[0] == "printr")
                    {
                        Instruction temp = new Instruction();

                        temp.instruction = s;
                        temp.type = Instruction.I_TYPE.PRINT_REG;
                        temp.arg1 = Int32.Parse(sSplit[1]);
                        temp.arg2 = Instruction.CHEESE;
                        temp.arg3 = Instruction.CHEESE;
                        if (PRS_DEBUG) Console.WriteLine("type: {0}, arg1: {1}, arg2: {2}, arg3: {3}", temp.type, temp.arg1, temp.arg2, temp.arg3);

                        totalQueue.Add(temp);
                    }
                    else if (sSplit[0] == "printc")
                    {
                        Instruction temp = new Instruction();

                        temp.instruction = s;
                        temp.type = Instruction.I_TYPE.PRINT_CHAR;
                        temp.arg1 = Int32.Parse(sSplit[1]);
                        temp.arg2 = Instruction.CHEESE;
                        temp.arg3 = Instruction.CHEESE;
                        if (PRS_DEBUG) Console.WriteLine("type: {0}, arg1: {1}, arg2: {2}, arg3: {3}", temp.type, temp.arg1, temp.arg2, temp.arg3);

                        totalQueue.Add(temp);
                    }
                    else if (sSplit[0] == "print")
                    {
                        Instruction temp = new Instruction();

                        temp.instruction = s;
                        temp.type = Instruction.I_TYPE.PRINT;
                        temp.arg1 = Int32.Parse(sSplit[1]);
                        temp.arg2 = Instruction.CHEESE;
                        temp.arg3 = Instruction.CHEESE;
                        if (PRS_DEBUG) Console.WriteLine("type: {0}, arg1: {1}, arg2: {2}, arg3: {3}", temp.type, temp.arg1, temp.arg2, temp.arg3);

                        totalQueue.Add(temp);
                    }
                }
                else if (sSplit.Length == 3)
                {
                    if (sSplit[0] == "setr")
                    {
                        Instruction temp = new Instruction();

                        temp.instruction = s;
                        temp.type = Instruction.I_TYPE.SET_REG;
                        temp.arg1 = Int32.Parse(sSplit[1]);
                        temp.arg2 = Int32.Parse(sSplit[2]);
                        temp.arg3 = Instruction.CHEESE;
                        if (PRS_DEBUG) Console.WriteLine("type: {0}, arg1: {1}, arg2: {2}, arg3: {3}", temp.type, temp.arg1, temp.arg2, temp.arg3);

                        totalQueue.Add(temp);
                    }
                    else if (sSplit[0] == "setrr")
                    {
                        Instruction temp = new Instruction();

                        temp.instruction = s;
                        temp.type = Instruction.I_TYPE.SET_REG_REG;
                        temp.arg1 = Int32.Parse(sSplit[1]);
                        temp.arg2 = Int32.Parse(sSplit[2]);
                        temp.arg3 = Instruction.CHEESE;
                        if (PRS_DEBUG) Console.WriteLine("type: {0}, arg1: {1}, arg2: {2}, arg3: {3}", temp.type, temp.arg1, temp.arg2, temp.arg3);

                        totalQueue.Add(temp);
                    }
                }
                else if (sSplit.Length == 4)
                {
                    if (sSplit[0] == "add")
                    {
                        Instruction temp = new Instruction();

                        temp.instruction = s;
                        temp.type = Instruction.I_TYPE.ADD;
                        temp.arg1 = Int32.Parse(sSplit[1]);
                        temp.arg2 = Int32.Parse(sSplit[2]);
                        temp.arg3 = Int32.Parse(sSplit[3]);
                        if (PRS_DEBUG) Console.WriteLine("type: {0}, arg1: {1}, arg2: {2}, arg3: {3}", temp.type, temp.arg1, temp.arg2, temp.arg3);

                        totalQueue.Add(temp);
                    }
                    else if (sSplit[0] == "sub")
                    {
                        Instruction temp = new Instruction();

                        temp.instruction = s;
                        temp.type = Instruction.I_TYPE.SUB;
                        temp.arg1 = Int32.Parse(sSplit[1]);
                        temp.arg2 = Int32.Parse(sSplit[2]);
                        temp.arg3 = Int32.Parse(sSplit[3]);
                        if (PRS_DEBUG) Console.WriteLine("type: {0}, arg1: {1}, arg2: {2}, arg3: {3}", temp.type, temp.arg1, temp.arg2, temp.arg3);

                        totalQueue.Add(temp);
                    }
                    else if (sSplit[0] == "mul")
                    {
                        Console.WriteLine("\n\nScheduler|ParseForInstructions: MUL MUL MUL MUL MUL!");
                        Instruction temp = new Instruction();

                        temp.instruction = s;
                        temp.type = Instruction.I_TYPE.MUL;
                        temp.arg1 = Int32.Parse(sSplit[1]);
                        temp.arg2 = Int32.Parse(sSplit[2]);
                        temp.arg3 = Int32.Parse(sSplit[3]);
                        if (PRS_DEBUG) Console.WriteLine("type: {0}, arg1: {1}, arg2: {2}, arg3: {3}", temp.type, temp.arg1, temp.arg2, temp.arg3);

                        totalQueue.Add(temp);
                    }
                    else if (sSplit[0] == "div")
                    {
                        Instruction temp = new Instruction();

                        temp.instruction = s;
                        temp.type = Instruction.I_TYPE.DIV;
                        temp.arg1 = Int32.Parse(sSplit[1]);
                        temp.arg2 = Int32.Parse(sSplit[2]);
                        temp.arg3 = Int32.Parse(sSplit[3]);
                        if (PRS_DEBUG) Console.WriteLine("type: {0}, arg1: {1}, arg2: {2}, arg3: {3}", temp.type, temp.arg1, temp.arg2, temp.arg3);

                        totalQueue.Add(temp);
                    }
                }
            }
            if (PRS_DEBUG) Console.WriteLine("Scheduler|Parser: finished reading file \"" + filename + "\"\n");
        }
    }
}
