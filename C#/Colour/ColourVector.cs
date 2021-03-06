﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

/*
    A 3x1 vector for colour manipulation

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

    [DebuggerDisplay("{DisplayString}")]
    public struct ColourVector
    {
        public ColourVector(
            float x, float y, float z)
        {
            X = (ColourPrimitive)x;
            Y = (ColourPrimitive)y;
            Z = (ColourPrimitive)z;
        }

        public ColourVector(
            double x, double y, double z)
        {
            X = (ColourPrimitive)x;
            Y = (ColourPrimitive)y;
            Z = (ColourPrimitive)z;
        }

        public ColourPrimitive X { get; private set; }
        public ColourPrimitive Y { get; private set; }
        public ColourPrimitive Z { get; private set; }

        //// Aliases
        //public ColourPrimitive R => X;
        //public ColourPrimitive G => Y;
        //public ColourPrimitive B => Z;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        string DisplayString => $"X: {(ColourPrimitive)X:0.000}, Y: {(ColourPrimitive)Y:0.000}, Z: {(ColourPrimitive)Z:0.000}";
        public override string ToString() => $"X: {(ColourPrimitive)X}, Y: {(ColourPrimitive)Y}, Z: {(ColourPrimitive)Z}";

        #region Add, subtract, multiply, divide
        public static ColourVector operator +(ColourVector a, ColourVector b)
            => new ColourVector(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        public static ColourVector operator -(ColourVector a, ColourVector b)
            => new ColourVector(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public static ColourVector operator *(ColourVector v, float value)
            => new ColourVector(
                v.X * (ColourPrimitive)value, v.Y * (ColourPrimitive)value, v.Z * (ColourPrimitive)value);

        public static ColourVector operator *(ColourVector v, double value)
            => new ColourVector(
                v.X * (ColourPrimitive)value, v.Y * (ColourPrimitive)value, v.Z * (ColourPrimitive)value);

        public static ColourVector operator *(float value, ColourVector v)
            => new ColourVector(
                v.X * (ColourPrimitive)value, v.Y * (ColourPrimitive)value, v.Z * (ColourPrimitive)value);

        public static ColourVector operator *(double value, ColourVector v)
            => new ColourVector(
                v.X * (ColourPrimitive)value, v.Y * (ColourPrimitive)value, v.Z * (ColourPrimitive)value);

        public static ColourVector operator /(ColourVector v, float value)
            => new ColourVector(
                v.X / (ColourPrimitive)value, v.Y / (ColourPrimitive)value, v.Z / (ColourPrimitive)value);

        public static ColourVector operator /(ColourVector v, double value)
            => new ColourVector(
                v.X / (ColourPrimitive)value, v.Y / (ColourPrimitive)value, v.Z / (ColourPrimitive)value);
        #endregion Add, subtract, multiply, divide
    }
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
