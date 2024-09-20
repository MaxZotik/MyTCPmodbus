using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTCPmodbus.Class.Repository
{
    internal class RepositoryChannelDevice
    {
        /// <summary>
        /// Словарь параметров МВК по каналу МВК
        /// </summary>
        public Dictionary<int, List<MVK>> DatabaseDictionaryChannel { get; set; }

        /// <summary>
        /// IP адрес устройства МВК
        /// </summary>
        public string IPaddress { get; set; }

        public RepositoryChannelDevice(string ipAddress)
        {
            IPaddress = ipAddress;
            DatabaseDictionaryChannel = GetChannelDevices();
        }

        /// <summary>
        /// Метод создает и заполняет словарь устройств МВК с заданным IP адресом с ключем по номеру канала МВК
        /// </summary>
        /// <returns>Возвращает словарь устройств МВК по IP адресу с ключем по каналу МВК</returns>
        private Dictionary<int, List<MVK>> GetChannelDevices()
        {
            Dictionary<int, List<MVK>> dict = new Dictionary<int, List<MVK>>();

            for (int i = 0; i < MVK.MvkList.Count; i++)
            {
                if (MVK.MvkList[i].IP == IPaddress)
                {
                    if (!dict.ContainsKey(MVK.MvkList[i].Channel) || dict.Count == 0)
                    {
                        dict.Add(MVK.MvkList[i].Channel, new List<MVK>() { MVK.MvkList[i] });
                    }
                    else
                    {
                        for (int k = 0; k < dict[MVK.MvkList[i].Channel].Count; k++)
                        {
                            if (dict[MVK.MvkList[i].Channel][k].Address < MVK.MvkList[i].Address)
                            {
                                dict[MVK.MvkList[i].Channel].Insert(k, MVK.MvkList[i]);
                                break;
                            }
                            else if (k == dict[MVK.MvkList[i].Channel].Count - 1)
                            {
                                dict[MVK.MvkList[i].Channel].Add(MVK.MvkList[i]);
                                break;
                            }
                        }
                    }
                }
            }

            return dict;
        }


        public void SetValueDevice(float[] value, int channel)
        {
            int address = DatabaseDictionaryChannel[channel][0].Address;
            DatabaseDictionaryChannel[channel][0].Value = value[0];
            int count = 1;

            for (int i = 1; i < DatabaseDictionaryChannel[channel].Count; i++)
            {

                while (count < value.Length)
                {
                    address += 2;

                    if (DatabaseDictionaryChannel[channel][i].Address == address)
                    {
                        DatabaseDictionaryChannel[channel][0].Value = value[count];
                        count++;
                        break;
                    }

                    count++;
                }

            }
        }
    }
}
