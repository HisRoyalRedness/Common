using HisRoyalRedness.com.ColourConstants;
using System;
using System.Collections.Generic;
using System.Text;

/*
    Colour components. These are added as fields to the various colour structs, 
    and take care of range checking, conversion etc.

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

    #region BaseColourComponent
    public abstract class BaseColourComponent<TType, TComponent>
        where TType : struct, IComparable<TType>
        where TComponent : BaseColourComponent<TType, TComponent>, new()
    {
        public BaseColourComponent()
        { }

        public BaseColourComponent(TType value)
        {
            _value = value;
        }

        public TType Value
        {
            get { return _value; }
            set { _value = Normalise(value); }
        }
        TType _value = default(TType);

        protected virtual TType Normalise(TType value)
        {
            if (value.CompareTo(MinValue) < 0)
                return MinValue;
            else if (value.CompareTo(MaxValue) > 0)
                return MaxValue;
            else
                return value;
        }

        public abstract TType MinValue { get; }
        public abstract TType MaxValue { get; }

        public override string ToString() => Value.ToString();

        public static implicit operator TType(BaseColourComponent<TType, TComponent> colour) => colour.Value;
        public static implicit operator BaseColourComponent<TType, TComponent>(TType colour) => new TComponent { Value = colour };
    }
    #endregion BaseColourComponent

    #region RGBColourComponent
    public sealed class RGBColourComponent : BaseColourComponent<byte, RGBColourComponent>
    {
        public RGBColourComponent()
            : base(0)
        { }

        public RGBColourComponent(byte value)
            : base(value)
        { }

        public override byte MinValue => 0;
        public override byte MaxValue => 255;
    }
    #endregion RGBColourComponent

    #region UnitColourComponent
    public sealed class UnitColourComponent : BaseColourComponent<ColourPrimitive, UnitColourComponent>
    {
        public UnitColourComponent()
            : base(ColourSpaceConstants.ZERO)
        { }

        public UnitColourComponent(ColourPrimitive value)
            : base(value)
        { }

        public override ColourPrimitive MinValue => ColourSpaceConstants.ZERO;
        public override ColourPrimitive MaxValue => ColourSpaceConstants.ONE;
    }
    #endregion UnitColourComponent

    #region DegreeColourComponent
    public sealed class DegreeColourComponent : BaseColourComponent<ColourPrimitive, DegreeColourComponent>
    {
        public DegreeColourComponent()
            : base(ColourSpaceConstants.ZERO)
        { }

        public DegreeColourComponent(ColourPrimitive value)
            : base(value)
        { }

        public override ColourPrimitive MinValue => ColourSpaceConstants.ZERO;
        public override ColourPrimitive MaxValue => ColourSpaceConstants.THREE_SIXTY;

        protected override ColourPrimitive Normalise(ColourPrimitive value)
        {
            while (value < MinValue)
                value += ColourSpaceConstants.THREE_SIXTY;
            while (value >= MaxValue)
                value -= ColourSpaceConstants.THREE_SIXTY;
            return value;
        }
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
