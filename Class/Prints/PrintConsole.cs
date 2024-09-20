using MyTCPmodbus.Class.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTCPmodbus.Class.Prints
{
    internal static class PrintConsole
    {
        public static void Print(string text, StatusMessage status = StatusMessage.Inform) 
        { 
            Console.WriteLine($"|{status}| - |{text}|");
        }

        public static void PrintMVK(MVK mvk)
        {
            Console.WriteLine($"IP: {mvk.IP} | Endian: {mvk.Endian} | Channel: {mvk.Channel} | Address: {mvk.Address} | Value: {mvk.Value}");
        }
    }
}
