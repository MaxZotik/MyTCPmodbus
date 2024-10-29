using MyTCPmodbus.Class.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTCPmodbus.Class.ModbusTCP
{
    internal class ModbusTcpWork
    {
        public ModbusTcpClient ModbusClient { get; set; }

        public static List<ModbusTcpWork> ModbusTcpWorksList {  get; set; }

        private RepositoryChannelDevice repositoryChannelDevice;

        private MvkDspCounter MvkDspCounter {  get; set; }

        static ModbusTcpWork()
        {
            ModbusTcpWorksList = new List<ModbusTcpWork>();
            InitList();
        }

        static void InitList()
        {
            for (int i = 0; i < MVK.MvkIpList.Count; i++)
            {
                ModbusTcpWorksList.Add(new ModbusTcpWork(new ModbusTcpClient(MVK.MvkIpList[i])));
            }
            
        }

        //"192.168.8.30"
        public ModbusTcpWork(ModbusTcpClient modbusClient)
        {
            repositoryChannelDevice = new RepositoryChannelDevice(modbusClient.IpAddress);
            ModbusClient = modbusClient;
            MvkDspCounter = MvkDspCounter.MvkDspCounterList[0];
        }


        private void ReadDataWithMVK(string endian, int address, int count, int channel)
        {
            try
            {
                float[] value = ModbusClient.ReadHoldingFloat(ushort.Parse(address.ToString()),
                endian == "3210" ? Endians.Endians_3210 : endian == "0123" ? Endians.Endians_0123 : Endians.Endians_2301, (ushort)count);

                repositoryChannelDevice.SetValueDevice(value, channel);

                //if (value.Length == 0)
                //{
                //    mvk.Value = value[0];
                //}

                repositoryChannelDevice.PrintDevice(channel);
            }
            catch (Exception ex)
            {
                PrintConsole.Print($"Ошибка преобразования полученных параметров МВК! - {ex.Message}", StatusMessage.Error);
            }
        }

        private void ReadCounterWithMVK()
        {
            try
            {
                UInt32 value = ModbusClient.ReadHoldingUIntt32(ushort.Parse(MvkDspCounter.Address.ToString()),
                MvkDspCounter.Endian == "3210" ? Endians.Endians_3210 : MvkDspCounter.Endian == "0123" ? Endians.Endians_0123 : Endians.Endians_2301);


                if (value != 0 && value != MvkDspCounter.Value)
                {
                    MvkDspCounter.Value = value;
                    PrintConsole.Print($"ReadCounterWithMVK: Счетчика выполненых процедур: {value}", StatusMessage.Inform);
                }

            }
            catch (Exception ex)
            {
                PrintConsole.Print($"Ошибка преобразования полученных параметров счетчика выполненых процедур МВК! - ReadCounterWithMVK - {ex.Message}", StatusMessage.Error);
            }
        }


        private void ModbusWorkStart()
        {
            //for (int i = 0; i < 100000; i++)
            //{
                if (ModbusClient.IsConnect)
                {
                    for (int i = 0; i < 100000; i++)
                    {
                        ReadCounterWithMVK();
                        Thread.Sleep(500);

                    if (!ModbusClient.IsConnect)
                        ModbusClient.ConnectTCP();

                }
                
                        
                    


                    //foreach (var dicKey in repositoryChannelDevice.DatabaseDictionaryChannel)
                    //{
                    //    int[] addressAndCount = repositoryChannelDevice.GetFirstAddressAndCountRegister(dicKey.Key);
                    //    PrintConsole.Print($"ModbusWorkStart: register: {addressAndCount[0]}, count: {addressAndCount[1]}", StatusMessage.Inform);

                    //    ReadDataWithMVK(repositoryChannelDevice.GetEndianMvk(dicKey.Key), addressAndCount[0], addressAndCount[1], dicKey.Key);
                    //}


                    PrintConsole.Print($"----------------------------------", StatusMessage.Inform);
                }
                else
                {
                    //Thread.Sleep(1000);
                    ModbusClient.ConnectTCP();
                }

            //    Thread.Sleep(500);
            //}         
        }

        public void Start()
        {
            int i = 0;

            while (true)
            {
                if (i == 2)
                    return;

                ModbusWorkStart();
                Thread.Sleep(500);

                i++;
            }
        }
    }
}
