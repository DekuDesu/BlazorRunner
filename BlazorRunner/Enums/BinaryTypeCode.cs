using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRunner.Runner
{
    public enum BinaryTypeCode
    {
        Empty = 0b0000_0000_0000_0000_0000,
        Boolean = 0b0000_0000_0000_0000_0001,
        Byte = 0b0000_0000_0000_0000_0010,
        Char = 0b0000_0000_0000_0000_0100,
        DateTime = 0b0000_0000_0000_0000_1000,
        DBNull = 0b0000_0000_0000_0001_0000,
        Decimal = 0b0000_0000_0000_0010_0000,
        Double = 0b0000_0000_0000_0100_0000,
        Int16 = 0b0000_0000_0000_1000_0000,
        Int32 = 0b0000_0000_0001_0000_0000,
        Int64 = 0b0000_0000_0010_0000_0000,
        Object = 0b0000_0000_0100_0000_0000,
        SByte = 0b0000_0000_1000_0000_0000,
        Single = 0b0000_0001_0000_0000_0000,
        String = 0b0000_0010_0000_0000_0000,
        UInt16 = 0b0000_0100_0000_0000_0000,
        UInt32 = 0b0000_1000_0000_0000_0000,
        UInt64 = 0b0001_0000_0000_0000_0000,
    }
}
