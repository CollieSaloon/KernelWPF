using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace KernelTestingWPF
{
    class Core
    {
        TextBlock tb;//
        public ListView listView; // staying null or something
        public const int QUEUE_SIZE = 100;
        public const bool DEBUG = true;
        public const bool SLP_DEBUG = true;
        private List<Instruction> processorQueue; // queue of instructions to execute
        private int[] registers; // registers for use by the processor
        private bool isFast; // determines whether this is a fast or slow core
        private Thread process; // the actual process
        public bool alive = true; //

        private int totalTime = 0; // for report

        public Core(bool isFast, TextBlock tb, int index)
        {
            this.tb = tb;
            processorQueue = new List<Instruction>();
            registers = new int[Instruction.NUM_REGISTERS];
            for (int i = 0; i < registers.Length; i++)
            {
                registers[i] = Instruction.CHEESE;
            }
            this.isFast = isFast;
            process = new Thread(ExecuteInstructions);

            

        }

        public void SetCoreListView(ListView lv, int index)
        {
            listView = lv;

            ListViewItem item = new ListViewItem();
            item.Content = string.Format("{0} Core #{1}", isFast ? "Fast" : "Slow", index);

            listView.Items.Add(item);
        }

        public bool IsFull() // we're not using this except for that one policy that cares abot queue size
        {
            return processorQueue.Count >= QUEUE_SIZE;
        }
        public bool IsEmpty()
        {
            return processorQueue.Count == 0;
        }

        private void AddListViewItem(ListViewItem item)
        {
            //new Thread(() =>
            //{
            //    listView.Dispatcher.BeginInvoke((Action)(() =>
            //        listView.Items.Add(item)));// do some UI thing (terminated by semicolon)
            //}).Start();

           Application.Current.Dispatcher.Invoke(new Action(() =>
           {
               listView.Items.Add(item);
           }));
           
        }

        public bool Enqueue(Instruction instruction) // used by scheduler when finished // CHANGE THIS
        {
            processorQueue.Add(instruction);

            string item;

            

            if (instruction.type >= 0 && instruction.type < Instruction.I_TYPE.SET_REG)
            {
                item = string.Format("{0}: {1}", Instruction.GetTypeString(instruction.type), instruction.arg1);
            }
            else if (instruction.type < Instruction.I_TYPE.ADD)
            {
                item = string.Format("{0}: {1}, {2}", Instruction.GetTypeString(instruction.type), instruction.arg1, instruction.arg2);
            }
            else if (instruction.type <= Instruction.I_TYPE.DIV)
            {
                item = string.Format("{0}: {1}, {2}, {3}", Instruction.GetTypeString(instruction.type), instruction.arg1, instruction.arg2, instruction.arg3);
            }
            else
            {
                item = "malarky";
            }

            Application.Current.Dispatcher.BeginInvoke((Action)(() => { listView.Items.Add(item); }));
            

            return true;
        }

        private Instruction Dequeue() // used internally
        {
            Instruction result = null;
            if (processorQueue.Count >= 1)
            {
                //*
                try
                {
                    result = processorQueue[0];
                    processorQueue.RemoveAt(0);
                }
                catch (IndexOutOfRangeException e)
                {
                    Console.WriteLine(e);
                }
                // */
            }
            return result;
        }

        public Instruction RemoveLast(Instruction instruction) // used by scheduler to reallocate
        {
            Instruction result = processorQueue[processorQueue.Count - 1];
            processorQueue.RemoveAt(processorQueue.Count - 1);
            return result;
        }

        public void ExecuteInstructions()
        {
            while (true)
            {
                // VERY MUCH malarky
                if (!alive)
                    return;
                while (processorQueue.Count > 0) // if there are processes, do one then sleep
                {
                    Instruction i = Dequeue();
                    if (i != null)
                        ProcessSingleInstruction(i);
                    if (SLP_DEBUG) Console.WriteLine("Core|Sleep");
                    Thread.Sleep(500);
                    // VERY MUCH malarky
                    if (!alive)
                        return;
                }
                if (SLP_DEBUG) Console.WriteLine("Core|Sleep");
                Thread.Sleep(500);
            }
        }

        public void ProcessSingleInstruction(Instruction instruction)
        {
            if (instruction == null)
            {
                Console.WriteLine("Core|PSI: instruction is null!");
                return;
            }
            if (DEBUG) Console.WriteLine("Core|PSI: instruction is NOT null.");
            if (DEBUG) Console.WriteLine("Core|PSI:\ntype {0}, arg1: {1}, arg2 {2}, arg3 {3}",
                instruction.type, instruction.arg1, instruction.arg2, instruction.arg3);
            if (DEBUG) Console.Write("Core|PSI: ");

            int time = isFast ? (int)(Instruction.I_OP_TIME_F[(int)instruction.type] * 1000 * CoreManager.SpeedMultiplier) :
                (int)(Instruction.I_OP_TIME_S[(int)instruction.type] * 1000 * CoreManager.SpeedMultiplier);

            Thread.Sleep(time);
            totalTime += time;

            Console.WriteLine("Total time: " + totalTime);

            switch (instruction.type)
            {
                case Instruction.I_TYPE.PRINT:
                    if (DEBUG) Console.WriteLine("PRINT");
                    Output(instruction.arg1);
                    break;
                case Instruction.I_TYPE.PRINT_CHAR:
                    if (DEBUG) Console.WriteLine("PRINT_CHAR");
                    Output(instruction.arg1, true);
                    break;
                case Instruction.I_TYPE.PRINT_REG:
                    if (DEBUG) Console.WriteLine("PRINT_REG");
                    Output(registers[instruction.arg1]);
                    break;
                case Instruction.I_TYPE.SET_REG:
                    if (DEBUG) Console.WriteLine("SET_REG");
                    registers[instruction.arg1] = instruction.arg2;
                    break;
                case Instruction.I_TYPE.SET_REG_REG:
                    if (DEBUG) Console.WriteLine("SET_REG_REG");
                    registers[instruction.arg1] = registers[instruction.arg2];
                    break;
                case Instruction.I_TYPE.ADD:
                    if (DEBUG) Console.WriteLine("ADD");
                    registers[instruction.arg1] =
                        registers[instruction.arg2] + registers[instruction.arg3];
                    break;
                case Instruction.I_TYPE.SUB:
                    if (DEBUG) Console.WriteLine("SUB");
                    registers[instruction.arg1] =
                        registers[instruction.arg2] - registers[instruction.arg3];
                    break;
                case Instruction.I_TYPE.MUL:
                    if (DEBUG) Console.WriteLine("MUL");
                    registers[instruction.arg1] =
                        registers[instruction.arg2] * registers[instruction.arg3];
                    break;
                case Instruction.I_TYPE.DIV:
                    if (DEBUG) Console.WriteLine("DIV");
                    registers[instruction.arg1] =
                        registers[instruction.arg2] / registers[instruction.arg3];
                    break;
                default:
                    if (DEBUG) Console.WriteLine("Unrecognized Instruction!");
                    return;
            }
            // remove a displayed item from the list
            //*
            
                new Thread(() => {
                    listView.Dispatcher.BeginInvoke((Action)(() =>
                        listView.Items.RemoveAt(1))); // not the title!
                }).Start();
                // */
        }

        public void Output(int output, bool asChar = false)
        {
            // This will change to the WPF stuff later
            
            //Console.Write("\n>> ");
            if (!asChar)
            {
                Console.WriteLine(output);
                
                new Thread(() => {
                         tb.Dispatcher.BeginInvoke((Action)(() =>
                            tb.Text += output + "\n"));
                     }).Start();
            }
            else
            {
                Console.WriteLine(((char)output).ToString());
                new Thread(() => {
                        tb.Dispatcher.BeginInvoke((Action)(() =>
                            tb.Text += ((char)output).ToString() + "\n"));
                    }).Start();
            }
            Console.WriteLine();
        }

        public bool GetIsFast()
        {
            return isFast;
        }

        public void Start() // anything else that needs to happen here
        {
            process.Start();
        }

        public void Stop()
        {
            //process = null;
            alive = false;
            process.Abort();
        }
    }
}
