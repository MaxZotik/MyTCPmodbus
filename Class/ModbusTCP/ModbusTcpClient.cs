using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyTCPmodbus.Class.ModbusTCP
{
    internal class ModbusTcpClient
    {
        public string IpAddress { get; set; }
        private Socket socket;
        private IPAddress iPAddress;
        private IPEndPoint endPoint;
        public bool IsConnect { get; set; }

        /// <summary>
        /// Конструктор экземпляра
        /// </summary>
        /// <param name="ipAddress">IP адрес "string"</param>
        /// <param name="port">Номер порта, по умолчанию - "502"</param>
        public ModbusTcpClient(string ipAddress, int port = 502)
        {
            this.IpAddress = ipAddress;
            iPAddress = IPAddress.Parse(ipAddress);
            endPoint = new IPEndPoint(iPAddress, port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IsConnect = false;
        }

        public void ConnectTCP()
        {
            
            try
            {
                socket.Connect(this.endPoint);
                socket.SendTimeout = Constant.DEFAULT_TIME_OUT;
                socket.ReceiveTimeout = Constant.DEFAULT_TIME_OUT;
                IsConnect = true;
                PrintConsole.Print($"Соединение по IP: {IpAddress} установлено!", StatusMessage.Inform);
            }
            catch (SocketException se)
            {
                PrintConsole.Print($"Соединение по IP: {IpAddress}! {se.Message}", StatusMessage.Error);
                IsConnect = false;
            }
            catch (Exception ex)
            {
                PrintConsole.Print($"Соединение по IP: {IpAddress} не установлено! {ex.Message}", StatusMessage.Error);
                IsConnect = false;
            }
        }

        /// <summary>
        /// Метод отправки и получения пакета Modbus
        /// </summary>
        /// <param name="packet">Пакет для оправки byte[]</param>
        /// <returns>Пакет пулученый byte[]</returns>
        /// <exception cref="IOException">ModbusTCP ошибка - 128</exception>
        private byte[] SendReceive(byte[] packet)
        {
            try
            {
                for (int i = 0; i < packet.Length; i++)
                {
                    PrintConsole.Print($"SendReceive2 - packet: length: {packet.Length} - i: {i} - value: {packet[i]}", StatusMessage.Action);
                }

                int count = packet[packet.Length - 1] * 2 + 9;

                byte[] mbap = new byte[count];


                socket.Send(packet);
                socket.Receive(mbap, 0, mbap.Length, SocketFlags.None);

                for (int i = 0; i < mbap.Length; i++)
                {
                    PrintConsole.Print($"SendReceive2 - mbap: length: {mbap.Length} - i: {i} - value: {mbap[i]}", StatusMessage.Action);
                }

                return mbap;
            }
            catch (SocketException se)
            {
                IsConnect = false;
                PrintConsole.Print($"Соединение по IP: {IpAddress} разорвано! {se.Message}", StatusMessage.Error);
                return new byte[1] { 0 };
            }
            catch (Exception ex)
            {
                IsConnect = false;
                PrintConsole.Print($"Соединение по IP: {IpAddress} разорвано! {ex.Message}", StatusMessage.Error);
                return new byte[1] { 0 };
            }
        }

        /// <summary>
        /// Метод чтения данных от Modbus
        /// </summary>
        /// <param name="function"></param>
        /// <param name="register"></param>
        /// <param name="count"></param>
        /// <returns>Пакет byte[]</returns>
        private byte[] Read(byte function, ushort register, ushort count)
        {
            PacketModbus packetModbus = new PacketModbus();

            byte[] rtn;
            byte[] packet = packetModbus.MakePacket(function, register, count);

            for (int i = 0; i < packet.Length; i++)
            {
                PrintConsole.Print($"Read - packet: length: {packet.Length} - i: {i} - value: {packet[i]}", StatusMessage.Action);
            }

            byte[] mbap = packetModbus.MakeMBAP();

            for (int i = 0; i < mbap.Length; i++)
            {
                PrintConsole.Print($"Read - mbap: length: {mbap.Length} - i: {i} - value: {mbap[i]}", StatusMessage.Action);
            }

            byte[] response = SendReceive(mbap.Concat(packet).ToArray());

            //if (response[0] == 0)
            //{
            //    return response;
            //}


            rtn = new byte[response[8]];
            Array.Copy(response, 9, rtn, 0, rtn.Length);

            for (int i = 0; i < rtn.Length; i++)
            {
                PrintConsole.Print($"Read - rtn: length: {rtn.Length} - i: {i} - value: {rtn[i]}", StatusMessage.Action);
            }

            return rtn;
        }


        
        /// <summary>
        /// Метод получения расчетных параметров МВК
        /// </summary>
        /// <param name="register">Адрес регистра</param>
        /// <param name="endians">Последовательность передачи байт</param>
        /// <param name="count">Количество требуемых регистров. По умолчанию = 1</param>
        /// <returns>Возвращает расчетный параметр МВК</returns>
        public float[] ReadHoldingFloat(ushort register, Endians endians = Endians.Endians_2301, ushort count = 1)
        {
            try
            {
                byte[] rVal = Read(Constant.FUNC_FOR_READ, register, (ushort)(count * Constant.USHORT_LENGTH));

                PrintConsole.Print($"ReadHoldingFloat - rVal: {rVal.Length}", StatusMessage.Error);

                float[] values = new float[rVal.Length / 4];

                PrintConsole.Print($"ReadHoldingFloat - values: {values.Length}", StatusMessage.Error);

                for (int i = 0; i < rVal.Length; i += Constant.FLOAT_LENGTH)
                {
                    if (endians == Endians.Endians_2301)
                    {
                        values[i / 4] = BitConverter.ToSingle(new byte[] { rVal[i + 1], rVal[i], rVal[i + 3], rVal[i + 2] }, 0);
                    }
                    else if (endians == Endians.Endians_0123)
                    {
                        values[i / 4] = BitConverter.ToSingle(new byte[] { rVal[i + 3], rVal[i + 2], rVal[i + 1], rVal[i] }, 0);
                    }
                    else
                    {
                        values[i / 4] = BitConverter.ToSingle(new byte[] { rVal[i], rVal[i + 1], rVal[i + 2], rVal[i + 3] }, 0);
                    }
                }

                for (int i = 0; i < values.Length; i++)
                {
                    PrintConsole.Print($"{values[i]}", StatusMessage.Action);
                }

                return values;
            }
            catch (Exception ex)
            {
                PrintConsole.Print($"Ошибка преобразования полученных расчетных параметров! {ex.Message}", StatusMessage.Error);
            }




            return Array.Empty<float>();
        }

    }
}
