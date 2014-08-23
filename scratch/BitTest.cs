using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class BitTest64
{
    public static bool testBitIdx(this UInt64 val, int idx)
    {
        if (idx > 63 || idx < 0) throw new Exception("invalid idx of " + idx);
        return testAnySet(val, ((UInt64)1 << idx));
    }

    public static UInt64 setBitIdx(this UInt64 val, int idx)
    {
        if (idx > 63 || idx < 0) throw new Exception("invalid idx of " + idx);
        return setBit(val, ((UInt64)1 << idx));
    }

    public static UInt64 clearBitIdx(this UInt64 val, int idx)
    {
        if (idx > 63 || idx < 0) throw new Exception("invalid idx of " + idx);
        return clearBit(val, ((UInt64)1 << idx));
    }

    public static bool testAllSet(this UInt64 val, UInt64 mask)
    {
        return (val & mask) == mask;
    }

    public static bool testAnySet(this UInt64 val, UInt64 mask)
    {
        return (val & mask) != 0;
    }

    public static UInt64 setBit(this UInt64 val, UInt64 mask)
    {
        return (val | mask);
    }

    public static UInt64 clearBit(this UInt64 val, UInt64 mask)
    {
        return (val & (~mask));
    }
}

public static class BitTest32
{
    public static bool testBitIdx(this UInt32 val, int idx)
    {
        if (idx > 31 || idx < 0) throw new Exception("invalid idx of " + idx);
        return testAnySet(val, (((UInt32) 1) << idx));
    }

    public static UInt32 setBitIdx(this UInt32 val, int idx)
    {
        if (idx > 31 || idx < 0) throw new Exception("invalid idx of " + idx);
        return setBit(val, ((UInt32)1) << idx);
    }

    public static UInt32 clearBitIdx(this UInt32 val, int idx)
    {
        if (idx > 31 || idx < 0) throw new Exception("invalid idx of " + idx);
        return clearBit(val, ((UInt32)1) << idx);
    }

    public static bool testAllSet(this UInt32 val, UInt32 mask)
    {
        return (val & mask) == mask;
    }

    public static bool testAnySet(this UInt32 val, UInt32 mask)
    {
        return (val & mask) != 0;
    }

    public static UInt32 setBit(this UInt32 val, UInt32 mask)
    {
        return (val | mask);
    }

    public static UInt32 clearBit(this UInt32 val, UInt32 mask)
    {
        return (val & (~mask));
    }
}


public static class BitTest16
{
    public static bool testBitIdx(this UInt16 val, int idx)
    {
        if (idx > 15 || idx < 0) throw new Exception("invalid idx of " + idx);
        return testAnySet(val, (UInt16)((1) << idx));
    }

    public static UInt16 setBitIdx(this UInt16 val, int idx)
    {
        if (idx > 15 || idx < 0) throw new Exception("invalid idx of " + idx);
        return setBit(val, (UInt16)(1<< idx));
    }

    public static UInt16 clearBitIdx(this UInt16 val, int idx)
    {
        if (idx > 15 || idx < 0) throw new Exception("invalid idx of " + idx);
        return clearBit(val, (UInt16)(1 << idx));
    }

    public static bool testAllSet(this UInt16 val, UInt16 mask)
    {
        return (val & mask) == mask;
    }

    public static bool testAnySet(this UInt16 val, UInt16 mask)
    {
        return (val & mask) != 0;
    }

    public static UInt16 setBit(this UInt16 val, UInt16 mask)
    {
        return (UInt16)(val | mask);
    }

    public static UInt16 clearBit(this UInt16 val, UInt16 mask)
    {
        return (UInt16)(val & (~mask));
    }
}


public static class BitTest8
{
    public static bool testBitIdx(this byte val, int idx)
    {
        if (idx > 7 || idx < 0) throw new Exception("invalid idx of " + idx);
        return testAnySet(val, (byte)((1) << idx));
    }

    public static byte setBitIdx(this byte val, int idx)
    {
        if (idx > 7 || idx < 0) throw new Exception("invalid idx of " + idx);
        return setBit(val, (byte)(1 << idx));
    }

    public static byte clearBitIdx(this byte val, int idx)
    {
        if (idx > 7 || idx < 0) throw new Exception("invalid idx of " + idx);
        return clearBit(val, (byte)(1 << idx));
    }

    public static bool testAllSet(this byte val, byte mask)
    {
        return (val & mask) == mask;
    }

    public static bool testAnySet(this byte val, byte mask)
    {
        return (val & mask) != 0;
    }

    public static byte setBit(this byte val, byte mask)
    {
        return (byte)(val | mask);
    }

    public static byte clearBit(this byte val, byte mask)
    {
        return (byte)(val & (~mask));
    }
}