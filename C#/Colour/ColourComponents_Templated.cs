using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

/*
    A generated file, from a template, that fills out all the boring,
    repetitive stuff for each colour component type

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
    public partial struct ByteColourComponent : IEquatable<ByteColourComponent>, IComparable, IComparable<ByteColourComponent>
    {
        #region Constructors
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

        #endregion Constructors

        #region Typed constants
        const int MIN_INT = (int)MIN_VAL;
        const int MAX_INT = (int)MAX_VAL;
        const float MIN_FLOAT = (float)MIN_VAL;
        const float MAX_FLOAT = (float)MAX_VAL;
        const double MIN_DOUBLE = (double)MIN_VAL;
        const double MAX_DOUBLE = (double)MAX_VAL;
        #endregion Typed constants

        #region Value clipping
        static byte Clip(int value)
            => value > MAX_INT
                ? MAX_VAL
                : (value < MIN_INT
                    ? MIN_VAL
                    : (byte)value);

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

        #endregion Value clipping

        public static byte MinValue => MIN_VAL;
        public static byte MaxValue => MAX_VAL;

        public byte Value => _value;
        readonly byte _value;

        /// <summary>
        /// Determines whether values outside the range of <see cref="ByteColourComponent"></see> are
        /// clipped to either the minimum or maximum value, or whether they wrap around.
        /// </summary>
        public static bool IsWrappingValue => false;

        public override string ToString() => _value.ToString();

        #region Implicit casts
        public static implicit operator byte(ByteColourComponent component) => component.Value;
        public static implicit operator ByteColourComponent(int value) => new ByteColourComponent(value);
        public static implicit operator ByteColourComponent(float value) => new ByteColourComponent(value);
        public static implicit operator ByteColourComponent(double value) => new ByteColourComponent(value);
        #endregion Implicit casts

        #region Add and subtract
        public static ByteColourComponent operator +(ByteColourComponent a, ByteColourComponent b)
            => new ByteColourComponent((int)a.Value + (int)b.Value);
        public static ByteColourComponent operator -(ByteColourComponent a, ByteColourComponent b)
            => new ByteColourComponent((int)a.Value - (int)b.Value);
        #endregion Add and subtract

        #region IEquality
        public bool Equals(ByteColourComponent other) => Value == other.Value;
        public override bool Equals(object obj) => obj is ByteColourComponent && this == (ByteColourComponent)obj;

        public override int GetHashCode() => Value.GetHashCode();

        public static bool operator ==(ByteColourComponent a, ByteColourComponent b) => a.Value == b.Value;
        public static bool operator !=(ByteColourComponent a, ByteColourComponent b) => !(a.Value == b.Value);
        #endregion IEquality

        #region Comparison
        public static int Compare(ByteColourComponent left, ByteColourComponent right)
            => left.Value > right.Value
                ? 1
                : (left.Value < right.Value
                    ? -1
                    : 0);

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (!(obj is ByteColourComponent))
                throw new ArgumentException("Object must be of type ByteColourComponent.");
            return Compare(this, (ByteColourComponent)obj);}

        public int CompareTo(ByteColourComponent other) => Compare(this, other.Value);

        public static bool operator <(ByteColourComponent left, ByteColourComponent right) => Compare(left, right.Value) < 0;
        public static bool operator <=(ByteColourComponent left, ByteColourComponent right) => Compare(left, right.Value) <= 0;
        public static bool operator >(ByteColourComponent left, ByteColourComponent right) => Compare(left, right.Value) > 0;
        public static bool operator >=(ByteColourComponent left, ByteColourComponent right) => Compare(left, right.Value) >= 0;
        #endregion Comparison
    }
    #endregion ByteColourComponent

    #region UnitColourComponent
    [DebuggerDisplay("{DisplayString}")]
    public partial struct UnitColourComponent : IEquatable<UnitColourComponent>, IComparable, IComparable<UnitColourComponent>
    {
        #region Constructors
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

        #endregion Constructors

        #region Typed constants
        const int MIN_INT = (int)MIN_VAL;
        const int MAX_INT = (int)MAX_VAL;
        const float MIN_FLOAT = (float)MIN_VAL;
        const float MAX_FLOAT = (float)MAX_VAL;
        const double MIN_DOUBLE = (double)MIN_VAL;
        const double MAX_DOUBLE = (double)MAX_VAL;
        #endregion Typed constants

        #region Value clipping
        static ColourPrimitive Clip(int value)
            => value > MAX_INT
                ? MAX_VAL
                : (value < MIN_INT
                    ? MIN_VAL
                    : (ColourPrimitive)value);

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

        #endregion Value clipping

        public static ColourPrimitive MinValue => MIN_VAL;
        public static ColourPrimitive MaxValue => MAX_VAL;

        public ColourPrimitive Value => _value;
        readonly ColourPrimitive _value;

        /// <summary>
        /// Determines whether values outside the range of <see cref="UnitColourComponent"></see> are
        /// clipped to either the minimum or maximum value, or whether they wrap around.
        /// </summary>
        public static bool IsWrappingValue => false;

        public override string ToString() => _value.ToString();

        #region Implicit casts
        public static implicit operator ColourPrimitive(UnitColourComponent component) => component.Value;
        public static implicit operator UnitColourComponent(int value) => new UnitColourComponent(value);
        public static implicit operator UnitColourComponent(float value) => new UnitColourComponent(value);
        public static implicit operator UnitColourComponent(double value) => new UnitColourComponent(value);
        #endregion Implicit casts

        #region Add and subtract
        public static UnitColourComponent operator +(UnitColourComponent a, UnitColourComponent b)
            => new UnitColourComponent(a.Value + b.Value);
        public static UnitColourComponent operator -(UnitColourComponent a, UnitColourComponent b)
            => new UnitColourComponent(a.Value - b.Value);
        #endregion Add and subtract

        #region IEquality
        public bool Equals(UnitColourComponent other) => Value == other.Value;
        public override bool Equals(object obj) => obj is UnitColourComponent && this == (UnitColourComponent)obj;

        public override int GetHashCode() => Value.GetHashCode();

        public static bool operator ==(UnitColourComponent a, UnitColourComponent b) => a.Value == b.Value;
        public static bool operator !=(UnitColourComponent a, UnitColourComponent b) => !(a.Value == b.Value);
        #endregion IEquality

        #region Comparison
        public static int Compare(UnitColourComponent left, UnitColourComponent right)
            => left.Value > right.Value
                ? 1
                : (left.Value < right.Value
                    ? -1
                    : 0);

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (!(obj is UnitColourComponent))
                throw new ArgumentException("Object must be of type UnitColourComponent.");
            return Compare(this, (UnitColourComponent)obj);}

        public int CompareTo(UnitColourComponent other) => Compare(this, other.Value);

        public static bool operator <(UnitColourComponent left, UnitColourComponent right) => Compare(left, right.Value) < 0;
        public static bool operator <=(UnitColourComponent left, UnitColourComponent right) => Compare(left, right.Value) <= 0;
        public static bool operator >(UnitColourComponent left, UnitColourComponent right) => Compare(left, right.Value) > 0;
        public static bool operator >=(UnitColourComponent left, UnitColourComponent right) => Compare(left, right.Value) >= 0;
        #endregion Comparison
    }
    #endregion UnitColourComponent

    #region DegreeColourComponent
    [DebuggerDisplay("{DisplayString}")]
    public partial struct DegreeColourComponent : IEquatable<DegreeColourComponent>, IComparable, IComparable<DegreeColourComponent>
    {
        #region Constructors
        public DegreeColourComponent(int value)
        {
            _value = Wrap(value);
        }

        public DegreeColourComponent(float value)
        {
            _value = Wrap(value);
        }

        public DegreeColourComponent(double value)
        {
            _value = Wrap(value);
        }

        #endregion Constructors

        #region Typed constants
        const int MIN_INT = (int)MIN_VAL;
        const int MAX_INT = (int)MAX_VAL;
        const float MIN_FLOAT = (float)MIN_VAL;
        const float MAX_FLOAT = (float)MAX_VAL;
        const double MIN_DOUBLE = (double)MIN_VAL;
        const double MAX_DOUBLE = (double)MAX_VAL;
        #endregion Typed constants

		#region Value wrapping
        static ColourPrimitive Wrap(int value)
        {
            var newValue = (ColourPrimitive)value;
            while(newValue >= MAX_VAL)
                newValue -= MAX_VAL;
            while(newValue < MIN_VAL)
                newValue += MAX_VAL;
            return newValue;
        }

        static ColourPrimitive Wrap(float value)
        {
            var newValue = (ColourPrimitive)value;
            while(newValue >= MAX_VAL)
                newValue -= MAX_VAL;
            while(newValue < MIN_VAL)
                newValue += MAX_VAL;
            return newValue;
        }

        static ColourPrimitive Wrap(double value)
        {
            var newValue = (ColourPrimitive)value;
            while(newValue >= MAX_VAL)
                newValue -= MAX_VAL;
            while(newValue < MIN_VAL)
                newValue += MAX_VAL;
            return newValue;
        }

        #endregion Value wrapping

        public static ColourPrimitive MinValue => MIN_VAL;
        public static ColourPrimitive MaxValue => MAX_VAL;

        public ColourPrimitive Value => _value;
        readonly ColourPrimitive _value;

        /// <summary>
        /// Determines whether values outside the range of <see cref="DegreeColourComponent"></see> are
        /// clipped to either the minimum or maximum value, or whether they wrap around.
        /// </summary>
        public static bool IsWrappingValue => true;

        public override string ToString() => _value.ToString();

        #region Implicit casts
        public static implicit operator ColourPrimitive(DegreeColourComponent component) => component.Value;
        public static implicit operator DegreeColourComponent(int value) => new DegreeColourComponent(value);
        public static implicit operator DegreeColourComponent(float value) => new DegreeColourComponent(value);
        public static implicit operator DegreeColourComponent(double value) => new DegreeColourComponent(value);
        #endregion Implicit casts

        #region Add and subtract
        public static DegreeColourComponent operator +(DegreeColourComponent a, DegreeColourComponent b)
            => new DegreeColourComponent(a.Value + b.Value);
        public static DegreeColourComponent operator -(DegreeColourComponent a, DegreeColourComponent b)
            => new DegreeColourComponent(a.Value - b.Value);
        #endregion Add and subtract

        #region IEquality
        public bool Equals(DegreeColourComponent other) => Value == other.Value;
        public override bool Equals(object obj) => obj is DegreeColourComponent && this == (DegreeColourComponent)obj;

        public override int GetHashCode() => Value.GetHashCode();

        public static bool operator ==(DegreeColourComponent a, DegreeColourComponent b) => a.Value == b.Value;
        public static bool operator !=(DegreeColourComponent a, DegreeColourComponent b) => !(a.Value == b.Value);
        #endregion IEquality

        #region Comparison
        public static int Compare(DegreeColourComponent left, DegreeColourComponent right)
            => left.Value > right.Value
                ? 1
                : (left.Value < right.Value
                    ? -1
                    : 0);

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;
            if (!(obj is DegreeColourComponent))
                throw new ArgumentException("Object must be of type DegreeColourComponent.");
            return Compare(this, (DegreeColourComponent)obj);}

        public int CompareTo(DegreeColourComponent other) => Compare(this, other.Value);

        public static bool operator <(DegreeColourComponent left, DegreeColourComponent right) => Compare(left, right.Value) < 0;
        public static bool operator <=(DegreeColourComponent left, DegreeColourComponent right) => Compare(left, right.Value) <= 0;
        public static bool operator >(DegreeColourComponent left, DegreeColourComponent right) => Compare(left, right.Value) > 0;
        public static bool operator >=(DegreeColourComponent left, DegreeColourComponent right) => Compare(left, right.Value) >= 0;
        #endregion Comparison
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