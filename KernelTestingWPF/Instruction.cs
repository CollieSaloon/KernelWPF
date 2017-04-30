using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KernelTestingWPF
{
    class Instruction
    {
        // Similar to MIPS, but without memory and only dealing with ints

        // Definitions:
        public const int NUM_REGISTERS = 10; // number of registers for each processor
        public const int CHEESE = 1010101; // this value indicates register is invalid

        public int timeStamp = -1;
        public static int runningTime = 0;

        public static string GetTypeString(I_TYPE i)
        {
            switch (i)
            {
                case I_TYPE.PRINT:
                    return "Print";
                case I_TYPE.PRINT_REG:
                    return "Print Register";
                case I_TYPE.PRINT_CHAR:
                    return "Print Char";
                case I_TYPE.SET_REG:
                    return "Set Register";
                case I_TYPE.SET_REG_REG:
                    return "Set Register^2";
                case I_TYPE.ADD:
                    return "Add";
                case I_TYPE.SUB:
                    return "Subtract";
                case I_TYPE.MUL:
                    return "Multiply";
                case I_TYPE.DIV:
                    return "Divide";
                default:
                    return "Invalid instruction";
            }
        }

        public enum I_TYPE // Instruction types
        {
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
        };
        
        public static int[] RAND_LO = { -250, -250, -250, -250, -250, -100, -100, -50, -50 };
        public static int[] RAND_HI = { 500, 500, 500, 700, 700, 1000, 1000, 1500, 1500 };
        public static int[] I_OP_TIME_F = { 1, 1, 1, 1, 1, 2, 2, 4, 4 }; // operation time of each instruction on a fast core
        public static int[] I_OP_TIME_S = { 3, 3, 3, 3, 3, 7, 7, 10, 10 }; // operation time of each instruction on a slow core
        public I_TYPE type;
        public int priority; // can be set by the scheduler
        public string instruction; // a hard copy of the actual instruction
        public int arg1, arg2, arg3; // the arguments

        public Instruction()
        {
            priority = 0; // neutral
            arg1 = arg2 = arg3 = CHEESE;
            instruction = "";
            type = I_TYPE.NUM_TYPES;

            runningTime++;
            timeStamp = runningTime;
        }
    }
}


// Terry A. Davis
/*
printc 67
printc 73
printc 65
// */
