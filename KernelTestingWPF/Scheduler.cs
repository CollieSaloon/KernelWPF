using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace KernelTestingWPF
{
    /*
        new Thread(() => {
                        tb.Dispatcher.BeginInvoke((Action)(() =>
                            // do some UI thing (terminated by semicolon)
                    }).Start();
    // */
    class Scheduler
    {
        public const bool PRS_DEBUG = true;
        public enum P_TYPE // policy types
        {
            RATIO_BASED,
            FAST_SLOW_BUFFER,
            TYPE_BASED,
            LIMITED_QUEUE,
            JANK
        };

        //bool alive = true; // VERY MUCH malarky

        string filename;
        Thread scheduler;
        List<Instruction> totalQueue;
        public ListView listViewInstructions;
        public ListView slowListView;
        public ListView fastListView;

        private int policy; // scheduling policy used by this core
        public int Policy
        {
            get; set;
        }

        

        public Scheduler(string filename, int policy = 0)
        {
            totalQueue = new List<Instruction>();
            this.policy = policy;   
            scheduler = new Thread((() => { DoScheduling(); }));
            //scheduler.SetApartmentState(ApartmentState.STA);
            this.filename = filename;
        }
        ~Scheduler()
        {
            Stop();
        }

        public void Start()
        {
            scheduler.Start();
        }
        public void Stop()
        {
            //alive = false;
            scheduler.Abort();
        }

        public void DoScheduling()
        { 
            //if (PRS_DEBUG) Console.WriteLine("Scheduler|DoScheduling: please enter a filename:");
            //string s = Console.ReadLine();
            ParseForInstructions(filename);

            while (true)
            {

                Thread.Sleep(2000);
                //Console.WriteLine("Scheduler|DoScheduling: starting to empty my queue.");
                while (totalQueue.Count > 0)
                {
                    Instruction instruction = null;
                    int index = -1;

                    switch (policy)
                    {
                        case (int)P_TYPE.LIMITED_QUEUE: // done
                            for(int i = 0; i < CoreManager.TotalCoreNum(); i++)
                            {
                                if (totalQueue.Count <= 0)
                                    break;
                                if (CoreManager.GetQueueAmount(i) >= CoreManager.QueueLimit)
                                    continue;
                                instruction = totalQueue[0];
                                CoreManager.EnqueueAt(i, instruction);
                                totalQueue.RemoveAt(0);
                                listViewInstructions.Dispatcher.Invoke(new Action(() =>
                                {
                                    listViewInstructions.Items.RemoveAt(1);
                                }));
                            }
                            break;
                        case (int)P_TYPE.RATIO_BASED: // close
                            index = CoreManager.GetMinPercentageIndex(); // !FIX
                            instruction = totalQueue[0];
                            CoreManager.EnqueueAt(index, instruction);
                            totalQueue.RemoveAt(0);
                            listViewInstructions.Dispatcher.Invoke(new Action(() =>
                            {
                                listViewInstructions.Items.RemoveAt(1);
                            }));
                            break;
                        case (int)P_TYPE.FAST_SLOW_BUFFER:
                            FillFastSlowBuffers();

                            FastSlowBrother();
                            break;
                        case (int)P_TYPE.TYPE_BASED: // done
                            instruction = totalQueue[0];
                            index = CoreManager.GetMinOfType(instruction.type);
                            CoreManager.EnqueueAt(index, instruction);
                            totalQueue.RemoveAt(0);
                            listViewInstructions.Dispatcher.Invoke(new Action(() =>
                            {
                                listViewInstructions.Items.RemoveAt(1);
                            }));
                            break;
                    }
                    if(policy != (int)P_TYPE.FAST_SLOW_BUFFER)
                    {
                        Thread.Sleep(800);
                    }
                     // malarky
                }
            }
        }

        private void FastSlowBrother()
        {
            while (CoreManager.fastInstructions.Count > 0 || CoreManager.slowInstructions.Count > 0)
            {
                if (CoreManager.DoFastInstruction())
                {
                    QueueFastInstruction();
                    fastListView.Dispatcher.Invoke(new Action(() =>
                    {
                        fastListView.Items.RemoveAt(0);
                    }));
                }
                else
                {
                    QueueSlowInstruction();
                    slowListView.Dispatcher.Invoke(new Action(() =>
                    {
                        slowListView.Items.RemoveAt(0);
                    }));
                }

                Thread.Sleep(800);
            }
        }

        private void QueueFastInstruction()
        {
            int check = CoreManager.checkFastCoresAvailability();

            if(check != -1)
            {
                CoreManager.EnqueueAt(check, CoreManager.fastInstructions.Dequeue());
            }
            else if(CoreManager.checkSlowCoresAvailability() != -1)
            {
                CoreManager.EnqueueAt(CoreManager.checkSlowCoresAvailability(), CoreManager.fastInstructions.Dequeue());
            }


        }

        private void QueueSlowInstruction()
        {
            int check = CoreManager.checkSlowCoresAvailability();

            if (check != -1)
            {
                CoreManager.EnqueueAt(check, CoreManager.slowInstructions.Dequeue());
            }
            else if (CoreManager.checkFastCoresAvailability() != -1)
            {
                CoreManager.EnqueueAt(CoreManager.checkFastCoresAvailability(), CoreManager.slowInstructions.Dequeue());
            }
        }

        private void FillFastSlowBuffers()
        {
            while(totalQueue.Count > 0)
            {
                Thread.Sleep(500);
                Instruction instruction = totalQueue[0];
                if(CoreManager.typesAreFastFull[(int)instruction.type])
                {
                    CoreManager.fastInstructions.Enqueue(instruction);
                    fastListView.Dispatcher.Invoke(new Action(() =>
                    {
                        AddFastListViewItem(instruction.instruction);
                    }));
                }
                else
                {
                    CoreManager.slowInstructions.Enqueue(instruction);
                     slowListView.Dispatcher.Invoke(new Action(() =>
                    {
                        AddSlowListViewItem(instruction.instruction);
                    }));
                }

                listViewInstructions.Dispatcher.Invoke(new Action(() =>
                {
                    listViewInstructions.Items.RemoveAt(1);
                }));

                totalQueue.RemoveAt(0);
            }
        }

        private void AddSlowListViewItem(string s)
        {
            ListViewItem item = new ListViewItem();

            item.HorizontalAlignment = HorizontalAlignment.Stretch;

            if (slowListView.Items.Count % 2 == 1)
            {
                item.Background = new SolidColorBrush(Colors.AliceBlue);
            }

            item.Content = s;

            slowListView.Items.Add(item);

        }

        private void AddFastListViewItem(string s)
        {
            ListViewItem item = new ListViewItem();

            item.HorizontalAlignment = HorizontalAlignment.Stretch;
           
                if (fastListView.Items.Count % 2 == 1)
                {
                    item.Background = new SolidColorBrush(Colors.IndianRed);
                    item.Foreground = new SolidColorBrush(Colors.White);
                }         

            item.Content = s;

            fastListView.Items.Add(item);

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
                int ts = -1;
                //if (PRS_DEBUG) Console.WriteLine("Scheduler|Parser: read Line: \"" + s + "\"");
                //listViewInstructions.Dispatcher.BeginInvoke(((Action)() => { listViewInstructions.Items.Add(s); )));// malarky

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

                        ts = temp.timeStamp;
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

                        ts = temp.timeStamp;
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

                        ts = temp.timeStamp;
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

                        ts = temp.timeStamp;
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

                        ts = temp.timeStamp;
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

                        ts = temp.timeStamp;
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

                        ts = temp.timeStamp;
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

                        ts = temp.timeStamp;
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

                        ts = temp.timeStamp;
                        totalQueue.Add(temp);
                    }
                }

                listViewInstructions.Dispatcher.Invoke(new Action(() =>
                {
                    listViewInstructions.Items.Add(ts + ": " + s);//
                }));
            }
            if (PRS_DEBUG) Console.WriteLine("Scheduler|Parser: finished reading file \"" + filename + "\"\n");
        }
    }
}
