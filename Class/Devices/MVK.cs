using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTCPmodbus.Class.Devices
{
    internal class MVK
    {
        public string IP {  get; set; }

        public string Endian { get; set; }

        public int Channel { get; set; }

        public int Address { get; set; }

        public float Value { get; set; }

        /// <summary>
        /// Свойтво содержит список объектов устройств MVK
        /// </summary>
        public static List<MVK> MvkList { get; set; }

        /// <summary>
        /// Свойство содержит список IP адресов устройств MVK
        /// </summary>
        public static List<string> MvkIpList { get; set; }

        static MVK()
        {
            MvkList = new List<MVK>();
            MvkIpList = new List<string>();
            GetMvklist();
            GetMvkIplist();
        }

        public MVK(string ip, string endian, int channel, int address, float value = 0.0f) 
        { 
            IP = ip;
            Endian = endian;
            Channel = channel;
            Address = address;
            Value = value;
        }

        static void GetMvklist()
        {
            MvkList.Add(new MVK("192.168.8.30", "0123", 1, 8284));
            MvkList.Add(new MVK("192.168.8.30", "0123", 1, 8296));
            MvkList.Add(new MVK("192.168.8.30", "0123", 1, 8294));
            MvkList.Add(new MVK("192.168.8.30", "0123", 1, 8306));
            MvkList.Add(new MVK("192.168.8.30", "0123", 1, 8288));
            MvkList.Add(new MVK("192.168.8.30", "0123", 1, 8302));
        }

        private static void GetMvkIplist()
        {
            MvkIpList.Add(MvkList[0].IP);

            for (int i = 0; i < MvkList.Count; i++)
            {
                bool index = false;

                for (int k = 0; k < MvkIpList.Count; k++)
                {
                    if (MvkIpList[k] == MvkList[i].IP)
                    {
                        index = false;
                        break;
                    }

                    index = true;
                }

                if (index)
                {
                    MvkIpList.Add(MvkList[i].IP);
                }
            }
        }
    }
}
