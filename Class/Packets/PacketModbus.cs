using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyTCPmodbus.Class.Packets
{
    internal class PacketModbus
    {
        /// <summary>
        /// Метод возвращает сообщения запроса к modbus по TCP
        /// </summary>
        /// <param name="function">Функциональный код</param>
        /// <param name="register">Адрес первого регистра</param>
        /// <param name="count">Количество требуемых регистров</param>
        /// <returns>Возвращает массив сообщения запроса к modbus по TCP</returns>
        public byte[] MakePacket(byte function, ushort register, ushort count)
        {
            return new byte[] {
                function,                           //Function Code
                (byte)(register >> Constant.BYTE),  //Data Address of the first register high
                (byte)register,                     //Data Address of the first register low
                0,                                  //The total number of registers high
                (byte)count                         //The total number of registers low
            };
        }

        /// <summary>
        /// Метод возвращает заголовок сообщения запроса к modbus по TCP
        /// </summary>
        /// <returns>Возвращает массив заголовок сообщения запроса к modbus по TCP</returns>
        public byte[] MakeMBAP()
        {
            return new byte[] {
                0,                      //Transaction Identifier high byte
                0,                      //Transaction Identifier low byte
                0,                      //Protocol Identifier high byte
                0,                      //Protocol Identifier low byte
                0,                      //Length high byte
                6,                      //Length low byte
                1                       //Unit Identifier
            };
        }

    }
}
