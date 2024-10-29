using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace MyTCPmodbus.Class.Devices
{
    internal class MvkDspCounter
    {
        public string IP { get; set; }

        public string Endian { get; set; }

        public int Address { get; set; }

        public UInt32 Value { get; set; }

        /// <summary>
        /// Свойтво содержит список объектов устройств MVK
        /// </summary>
        public static List<MvkDspCounter> MvkDspCounterList { get; set; }

        /// <summary>
        /// Свойство содержит список IP адресов устройств MVK
        /// </summary>
        public static List<string> MvkDspCounterIpList { get; set; }

        static MvkDspCounter()
        {
            MvkDspCounterList = new List<MvkDspCounter>();
            MvkDspCounterIpList = new List<string>();
            GetMvklist();
        }

        public MvkDspCounter(string ip, string endian, int address, UInt32 value = 0)
        {
            IP = ip;
            Endian = endian;
            Address = address;
            Value = value;
        }

        static void GetMvklist()
        {
            MvkDspCounterList.Add(new MvkDspCounter("192.168.8.30", "0123", 4132));
        }
    }
}
