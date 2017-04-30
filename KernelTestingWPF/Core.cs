using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Controls;

namespace KernelTestingWPF
{
    class Core
    {
        static List<bool> killCores = new List<bool>();
        TextBlock tb;//
        List<ListView> listViews; // staying null or something
        public const int QUEUE_SIZE = 100;
        public const bool DEBUG = false;
        public const bool SLP_DEBUG = false;
        public static int SPEED_MULTIPLIER = 1; // used for increase/decrease execution speed scale
        private List<Instruction> processorQueue; // queue of instructions to execute
        private int[] registers; // registers for use by the processor
        private bool isFast; // determines whether this is a fast or slow core
        private Thread process; // the actual process
        int id = -1; // malarky

        public Core(bool isFast, TextBlock tb, List<ListView> lv)
        {
            listViews = lv;
            this.tb = tb;
            processorQueue = new List<Instruction>();
            registers = new int[Instruction.NUM_REGISTERS];
            for (int i = 0; i < registers.Length; i++)
            {
                registers[i] = Instruction.CHEESE;
            }
            this.isFast = isFast;
            process = new Thread(ExecuteInstructions);
            
            // VERY MUCH malarky
            killCores.Add(new bool());
            killCores[killCores.Count - 1] = false;
            id = killCores.Count - 1;
        }

        public bool IsFull() // we're not using this except for that one policy that cares abot queue size
        {
            return processorQueue.Count >= QUEUE_SIZE;
        }
        public bool IsEmpty()
        {
            return processorQueue.Count == 0;
        }

        public bool Enqueue(Instruction instruction) // used by scheduler when finished // CHANGE THIS
        {
            processorQueue.Add(instruction);
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
                if (killCores[id])
                    return;
                while (processorQueue.Count > 0) // if there are processes, do one then sleep
                {
                    Instruction i = Dequeue();
                    if (i != null)
                        ProcessSingleInstruction(i);
                    if (SLP_DEBUG) Console.WriteLine("Core|Sleep");
                    Thread.Sleep(500);
                    // VERY MUCH malarky
                    if (killCores[id])
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

            Thread.Sleep(isFast ? Instruction.I_OP_TIME_F[(int)instruction.type] * 1000 :
                Instruction.I_OP_TIME_S[(int)instruction.type] * 1000);

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
            if (id >= 0 && id < listViews.Count)
                new Thread(() => {
                    listViews[id].Dispatcher.BeginInvoke((Action)(() =>
                        listViews[id].Items.RemoveAt(1))); // not the title!
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
            killCores[id] = true;
            process.Abort();
        }
    }
}
