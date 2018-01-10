using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

/*
    A generated file, from a template, that fills out all the boring,
    repeatetive stuff for each colour component type

    Keith Fletcher
    Jan 2018

    This file is Unlicensed.
    See the foot of the file, or refer to <http://unlicense.org>
*/

namespace HisRoyalRedness.com
{
#if COLOUR_SINGLE
    using ColourPrimitive = Single;
#else
    using ColourPrimitive = Double;
#endif

    #region ByteColourComponent
    [DebuggerDisplay("{DisplayString}")]
    public partial struct ByteColourComponent
    {
        public ByteColourComponent(int value)
        {
            _value = Clip(value);
        }

        public ByteColourComponent(float value)
        {
            _value = Clip(value);
        }

        public ByteColourComponent(double value)
        {
            _value = Clip(value);
        }

        #region Typed constants
        const float MIN_FLOAT = (float)MIN_VAL;
        const double MIN_DOUBLE = (double)MIN_VAL;
        const int MIN_INT = (int)MIN_VAL;

        const float MAX_FLOAT = (float)MAX_VAL;
        const double MAX_DOUBLE = (double)MAX_VAL;
        const int MAX_INT = (int)MAX_VAL;
        #endregion Typed constants

		#region Value clipping
        static byte Clip(float value)
            => value > MAX_FLOAT
                ? MAX_VAL
                : (value < MIN_FLOAT
                    ? MIN_VAL
                    : (byte)value);

        static byte Clip(double value)
            => value > MAX_DOUBLE
                ? MAX_VAL
                : (value < MIN_DOUBLE
                    ? MIN_VAL
                    : (byte)value);

        static byte Clip(int value)
            => value > MAX_INT
                ? MAX_VAL
                : (value < MIN_INT
                    ? MIN_VAL
                    : (byte)value);
        #endregion Value clipping

        public static byte MinValue => MIN_VAL;
        public static byte MaxValue => MAX_VAL;

        public byte Value => _value;
        readonly byte _value;

        public override string ToString() => _value.ToString();

        public static implicit operator byte(ByteColourComponent component) => component.Value;
        public static implicit operator ByteColourComponent(byte value) => new ByteColourComponent(value);

        #region Add and subtract
        public static ByteColourComponent operator +(ByteColourComponent a, ByteColourComponent b)
            => new ByteColourComponent((int)a.Value + (int)b.Value);
        public static ByteColourComponent operator +(ByteColourComponent a, int b)
            => new ByteColourComponent((int)a.Value + b);
        public static ByteColourComponent operator +(ByteColourComponent a, float b)
            => new ByteColourComponent((float)a.Value + b);
        public static ByteColourComponent operator +(ByteColourComponent a, double b)
            => new ByteColourComponent((double)a.Value + b);
        public static ByteColourComponent operator +(int a, ByteColourComponent b)
            => new ByteColourComponent(a + (int)b.Value);
        public static ByteColourComponent operator +(float a, ByteColourComponent b)
            => new ByteColourComponent(a + (float)b.Value);
        public static ByteColourComponent operator +(double a, ByteColourComponent b)
            => new ByteColourComponent(a + (double)b.Value);

        public static ByteColourComponent operator -(ByteColourComponent a, ByteColourComponent b)
            => new ByteColourComponent(a.Value - b.Value);
        public static ByteColourComponent operator -(ByteColourComponent a, int b)
            => new ByteColourComponent((int)a.Value - b);
        public static ByteColourComponent operator -(ByteColourComponent a, float b)
            => new ByteColourComponent((float)a.Value - b);
        public static ByteColourComponent operator -(ByteColourComponent a, double b)
            => new ByteColourComponent((double)a.Value - b);
        public static ByteColourComponent operator -(int a, ByteColourComponent b)
            => new ByteColourComponent(a - (int)b.Value);
        public static ByteColourComponent operator -(float a, ByteColourComponent b)
            => new ByteColourComponent(a - (float)b.Value);
        public static ByteColourComponent operator -(double a, ByteColourComponent b)
            => new ByteColourComponent(a - (double)b.Value);
        #endregion Add and subtract
    }
    #endregion ByteColourComponent

    #region UnitColourComponent
    [DebuggerDisplay("{DisplayString}")]
    public partial struct UnitColourComponent
    {
        public UnitColourComponent(int value)
        {
            _value = Clip(value);
        }

        public UnitColourComponent(float value)
        {
            _value = Clip(value);
        }

        public UnitColourComponent(double value)
        {
            _value = Clip(value);
        }

        #region Typed constants
        const float MIN_FLOAT = (float)MIN_VAL;
        const double MIN_DOUBLE = (double)MIN_VAL;
        const int MIN_INT = (int)MIN_VAL;

        const float MAX_FLOAT = (float)MAX_VAL;
        const double MAX_DOUBLE = (double)MAX_VAL;
        const int MAX_INT = (int)MAX_VAL;
        #endregion Typed constants

		#region Value clipping
        static ColourPrimitive Clip(float value)
            => value > MAX_FLOAT
                ? MAX_VAL
                : (value < MIN_FLOAT
                    ? MIN_VAL
                    : (ColourPrimitive)value);

        static ColourPrimitive Clip(double value)
            => value > MAX_DOUBLE
                ? MAX_VAL
                : (value < MIN_DOUBLE
                    ? MIN_VAL
                    : (ColourPrimitive)value);

        static ColourPrimitive Clip(int value)
            => value > MAX_INT
                ? MAX_VAL
                : (value < MIN_INT
                    ? MIN_VAL
                    : (ColourPrimitive)value);
        #endregion Value clipping

        public static ColourPrimitive MinValue => MIN_VAL;
        public static ColourPrimitive MaxValue => MAX_VAL;

        public ColourPrimitive Value => _value;
        readonly ColourPrimitive _value;

        public override string ToString() => _value.ToString();

        public static implicit operator ColourPrimitive(UnitColourComponent component) => component.Value;
        public static implicit operator UnitColourComponent(ColourPrimitive value) => new UnitColourComponent(value);

        #region Add and subtract
        public static UnitColourComponent operator +(UnitColourComponent a, UnitColourComponent b)
            => new UnitColourComponent(a.Value + b.Value);
        public static UnitColourComponent operator +(UnitColourComponent a, int b)
            => new UnitColourComponent(a.Value + (ColourPrimitive)b);
        public static UnitColourComponent operator +(UnitColourComponent a, float b)
            => new UnitColourComponent(a.Value + (ColourPrimitive)b);
        public static UnitColourComponent operator +(UnitColourComponent a, double b)
            => new UnitColourComponent(a.Value + (ColourPrimitive)b);
        public static UnitColourComponent operator +(int a, UnitColourComponent b)
            => new UnitColourComponent((ColourPrimitive)a + b.Value);
        public static UnitColourComponent operator +(float a, UnitColourComponent b)
            => new UnitColourComponent((ColourPrimitive)a + b.Value);
        public static UnitColourComponent operator +(double a, UnitColourComponent b)
            => new UnitColourComponent((ColourPrimitive)a + b.Value);

        public static UnitColourComponent operator -(UnitColourComponent a, UnitColourComponent b)
            => new UnitColourComponent(a.Value - b.Value);
        public static UnitColourComponent operator -(UnitColourComponent a, int b)
            => new UnitColourComponent(a.Value - (ColourPrimitive)b);
        public static UnitColourComponent operator -(UnitColourComponent a, float b)
            => new UnitColourComponent(a.Value - (ColourPrimitive)b);
        public static UnitColourComponent operator -(UnitColourComponent a, double b)
            => new UnitColourComponent(a.Value - (ColourPrimitive)b);
        public static UnitColourComponent operator -(int a, UnitColourComponent b)
            => new UnitColourComponent((ColourPrimitive)a - b.Value);
        public static UnitColourComponent operator -(float a, UnitColourComponent b)
            => new UnitColourComponent((ColourPrimitive)a - b.Value);
        public static UnitColourComponent operator -(double a, UnitColourComponent b)
            => new UnitColourComponent((ColourPrimitive)a - b.Value);
        #endregion Add and subtract
    }
    #endregion UnitColourComponent

    #region DegreeColourComponent
    [DebuggerDisplay("{DisplayString}")]
    public partial struct DegreeColourComponent
    {
        public DegreeColourComponent(int value)
        {
            _value = Clip(value);
        }

        public DegreeColourComponent(float value)
        {
            _value = Clip(value);
        }

        public DegreeColourComponent(double value)
        {
            _value = Clip(value);
        }

        #region Typed constants
        const float MIN_FLOAT = (float)MIN_VAL;
        const double MIN_DOUBLE = (double)MIN_VAL;
        const int MIN_INT = (int)MIN_VAL;

        const float MAX_FLOAT = (float)MAX_VAL;
        const double MAX_DOUBLE = (double)MAX_VAL;
        const int MAX_INT = (int)MAX_VAL;
        #endregion Typed constants

		#region Value clipping
        static ColourPrimitive Clip(int value)
        {
            var newValue = (ColourPrimitive)value;
            while(newValue >= MAX_VAL)
                newValue -= MAX_VAL;
            while(newValue < MIN_VAL)
                newValue += MAX_VAL;
            return newValue;
        }

        static ColourPrimitive Clip(float value)
        {
            var newValue = (ColourPrimitive)value;
            while(newValue >= MAX_VAL)
                newValue -= MAX_VAL;
            while(newValue < MIN_VAL)
                newValue += MAX_VAL;
            return newValue;
        }

        static ColourPrimitive Clip(double value)
        {
            var newValue = (ColourPrimitive)value;
            while(newValue >= MAX_VAL)
                newValue -= MAX_VAL;
            while(newValue < MIN_VAL)
                newValue += MAX_VAL;
            return newValue;
        }
        #endregion Value clipping

        public static ColourPrimitive MinValue => MIN_VAL;
        public static ColourPrimitive MaxValue => MAX_VAL;

        public ColourPrimitive Value => _value;
        readonly ColourPrimitive _value;

        public override string ToString() => _value.ToString();

        public static implicit operator ColourPrimitive(DegreeColourComponent component) => component.Value;
        public static implicit operator DegreeColourComponent(ColourPrimitive value) => new DegreeColourComponent(value);

        #region Add and subtract
        public static DegreeColourComponent operator +(DegreeColourComponent a, DegreeColourComponent b)
            => new DegreeColourComponent(a.Value + b.Value);
        public static DegreeColourComponent operator +(DegreeColourComponent a, int b)
            => new DegreeColourComponent(a.Value + (ColourPrimitive)b);
        public static DegreeColourComponent operator +(DegreeColourComponent a, float b)
            => new DegreeColourComponent(a.Value + (ColourPrimitive)b);
        public static DegreeColourComponent operator +(DegreeColourComponent a, double b)
            => new DegreeColourComponent(a.Value + (ColourPrimitive)b);
        public static DegreeColourComponent operator +(int a, DegreeColourComponent b)
            => new DegreeColourComponent((ColourPrimitive)a + b.Value);
        public static DegreeColourComponent operator +(float a, DegreeColourComponent b)
            => new DegreeColourComponent((ColourPrimitive)a + b.Value);
        public static DegreeColourComponent operator +(double a, DegreeColourComponent b)
            => new DegreeColourComponent((ColourPrimitive)a + b.Value);

        public static DegreeColourComponent operator -(DegreeColourComponent a, DegreeColourComponent b)
            => new DegreeColourComponent(a.Value - b.Value);
        public static DegreeColourComponent operator -(DegreeColourComponent a, int b)
            => new DegreeColourComponent(a.Value - (ColourPrimitive)b);
        public static DegreeColourComponent operator -(DegreeColourComponent a, float b)
            => new DegreeColourComponent(a.Value - (ColourPrimitive)b);
        public static DegreeColourComponent operator -(DegreeColourComponent a, double b)
            => new DegreeColourComponent(a.Value - (ColourPrimitive)b);
        public static DegreeColourComponent operator -(int a, DegreeColourComponent b)
            => new DegreeColourComponent((ColourPrimitive)a - b.Value);
        public static DegreeColourComponent operator -(float a, DegreeColourComponent b)
            => new DegreeColourComponent((ColourPrimitive)a - b.Value);
        public static DegreeColourComponent operator -(double a, DegreeColourComponent b)
            => new DegreeColourComponent((ColourPrimitive)a - b.Value);
        #endregion Add and subtract
    }
    #endregion DegreeColourComponent

}

/*
This is free and unencumbered software released into the public domain.

Anyone is free to copy, modify, publish, use, compile, sell, or
distribute this software, either in source code form or as a compiled
binary, for any purpose, commercial or non-commercial, and by any
means.

In jurisdictions that recognize copyright laws, the author or authors
of this software dedicate any and all copyright interest in the
software to the public domain. We make this dedication for the benefit
of the public at large and to the detriment of our heirs and
successors. We intend this dedication to be an overt act of
relinquishment in perpetuity of all present and future rights to this
software under copyright law.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
OTHER DEALINGS IN THE SOFTWARE.

For more information, please refer to <http://unlicense.org>
*/