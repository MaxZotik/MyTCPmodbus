using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTCPmodbus.Class.Constants
{
    internal static class Constant
    {
        public const byte FUNC_FOR_READ = 3;
        public const byte BYTE = 8;
        public const byte USHORT_LENGTH = 2;
        public const byte FLOAT_LENGTH = 4;

        public const int DEFAULT_TIME_OUT = 1000;
    }
}
