using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

/*
    A 3x3 matrix for performing colour transformations

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
    public struct ColourMatrix
    {
        public ColourMatrix(
            float m11, float m12, float m13,
            float m21, float m22, float m23,
            float m31, float m32, float m33)
        {
            M11 = (ColourPrimitive)m11; M12 = (ColourPrimitive)m12; M13 = (ColourPrimitive)m13;
            M21 = (ColourPrimitive)m21; M22 = (ColourPrimitive)m22; M23 = (ColourPrimitive)m23;
            M31 = (ColourPrimitive)m31; M32 = (ColourPrimitive)m32; M33 = (ColourPrimitive)m33;

            _determinant = new Lazy<ColourPrimitive>(() =>
                (ColourPrimitive)m11 * (ColourPrimitive)m22 * (ColourPrimitive)m33 +
                (ColourPrimitive)m12 * (ColourPrimitive)m23 * (ColourPrimitive)m31 +
                (ColourPrimitive)m13 * (ColourPrimitive)m21 * (ColourPrimitive)m32 -
                (ColourPrimitive)m31 * (ColourPrimitive)m22 * (ColourPrimitive)m13 -
                (ColourPrimitive)m32 * (ColourPrimitive)m23 * (ColourPrimitive)m11 -
                (ColourPrimitive)m33 * (ColourPrimitive)m21 * (ColourPrimitive)m12);
            _inverse = new Lazy<ColourMatrix>(() =>
            {
                // https://en.wikipedia.org/wiki/Invertible_matrix#Inversion_of_3_%C3%97_3_matrices
                var A = ((ColourPrimitive)m22 * (ColourPrimitive)m33 - (ColourPrimitive)m23 * (ColourPrimitive)m32);
                var B = ((ColourPrimitive)m23 * (ColourPrimitive)m31 - (ColourPrimitive)m21 * (ColourPrimitive)m33);
                var C = ((ColourPrimitive)m21 * (ColourPrimitive)m32 - (ColourPrimitive)m22 * (ColourPrimitive)m31);
                var D = ((ColourPrimitive)m13 * (ColourPrimitive)m32 - (ColourPrimitive)m12 * (ColourPrimitive)m33);
                var E = ((ColourPrimitive)m11 * (ColourPrimitive)m33 - (ColourPrimitive)m13 * (ColourPrimitive)m31);
                var F = ((ColourPrimitive)m12 * (ColourPrimitive)m31 - (ColourPrimitive)m11 * (ColourPrimitive)m32);
                var G = ((ColourPrimitive)m12 * (ColourPrimitive)m23 - (ColourPrimitive)m13 * (ColourPrimitive)m22);
                var H = ((ColourPrimitive)m13 * (ColourPrimitive)m21 - (ColourPrimitive)m11 * (ColourPrimitive)m23);
                var I = ((ColourPrimitive)m11 * (ColourPrimitive)m22 - (ColourPrimitive)m12 * (ColourPrimitive)m21);
                var det = (ColourPrimitive)m11 * A + (ColourPrimitive)m12 * B + (ColourPrimitive)m13 * C; // Rule of Sarrus
                return new ColourMatrix(
                  A / det, D / det, G / det,
                  B / det, E / det, H / det,
                  C / det, F / det, I / det);
            });
        }

        public ColourMatrix(
            double m11, double m12, double m13,
            double m21, double m22, double m23,
            double m31, double m32, double m33)
        {
            M11 = (ColourPrimitive)m11; M12 = (ColourPrimitive)m12; M13 = (ColourPrimitive)m13;
            M21 = (ColourPrimitive)m21; M22 = (ColourPrimitive)m22; M23 = (ColourPrimitive)m23;
            M31 = (ColourPrimitive)m31; M32 = (ColourPrimitive)m32; M33 = (ColourPrimitive)m33;

            _determinant = new Lazy<ColourPrimitive>(() =>
                (ColourPrimitive)m11 * (ColourPrimitive)m22 * (ColourPrimitive)m33 +
                (ColourPrimitive)m12 * (ColourPrimitive)m23 * (ColourPrimitive)m31 +
                (ColourPrimitive)m13 * (ColourPrimitive)m21 * (ColourPrimitive)m32 -
                (ColourPrimitive)m31 * (ColourPrimitive)m22 * (ColourPrimitive)m13 -
                (ColourPrimitive)m32 * (ColourPrimitive)m23 * (ColourPrimitive)m11 -
                (ColourPrimitive)m33 * (ColourPrimitive)m21 * (ColourPrimitive)m12);
            _inverse = new Lazy<ColourMatrix>(() =>
            {
                // https://en.wikipedia.org/wiki/Invertible_matrix#Inversion_of_3_%C3%97_3_matrices
                var A = ((ColourPrimitive)m22 * (ColourPrimitive)m33 - (ColourPrimitive)m23 * (ColourPrimitive)m32);
                var B = ((ColourPrimitive)m23 * (ColourPrimitive)m31 - (ColourPrimitive)m21 * (ColourPrimitive)m33);
                var C = ((ColourPrimitive)m21 * (ColourPrimitive)m32 - (ColourPrimitive)m22 * (ColourPrimitive)m31);
                var D = ((ColourPrimitive)m13 * (ColourPrimitive)m32 - (ColourPrimitive)m12 * (ColourPrimitive)m33);
                var E = ((ColourPrimitive)m11 * (ColourPrimitive)m33 - (ColourPrimitive)m13 * (ColourPrimitive)m31);
                var F = ((ColourPrimitive)m12 * (ColourPrimitive)m31 - (ColourPrimitive)m11 * (ColourPrimitive)m32);
                var G = ((ColourPrimitive)m12 * (ColourPrimitive)m23 - (ColourPrimitive)m13 * (ColourPrimitive)m22);
                var H = ((ColourPrimitive)m13 * (ColourPrimitive)m21 - (ColourPrimitive)m11 * (ColourPrimitive)m23);
                var I = ((ColourPrimitive)m11 * (ColourPrimitive)m22 - (ColourPrimitive)m12 * (ColourPrimitive)m21);
                var det = (ColourPrimitive)m11 * A + (ColourPrimitive)m12 * B + (ColourPrimitive)m13 * C; // Rule of Sarrus
                return new ColourMatrix(
                  A / det, D / det, G / det,
                  B / det, E / det, H / det,
                  C / det, F / det, I / det);
            });
        }

        public ColourPrimitive M11 { get; private set; }
        public ColourPrimitive M12 { get; private set; }
        public ColourPrimitive M13 { get; private set; }
        public ColourPrimitive M21 { get; private set; }
        public ColourPrimitive M22 { get; private set; }
        public ColourPrimitive M23 { get; private set; }
        public ColourPrimitive M31 { get; private set; }
        public ColourPrimitive M32 { get; private set; }
        public ColourPrimitive M33 { get; private set; }

        #region Multiply, divide
        public static ColourVector operator *(ColourMatrix m, ColourVector v)
            => new ColourVector(
                m.M11 * v.X + m.M12 * v.Y + m.M13 * v.Z,
                m.M21 * v.X + m.M22 * v.Y + m.M23 * v.Z,
                m.M31 * v.X + m.M32 * v.Y + m.M33 * v.Z);

        public static ColourMatrix operator *(ColourMatrix m, float value)
            => new ColourMatrix(
                m.M11 * (ColourPrimitive)value, m.M12 * (ColourPrimitive)value, m.M13 * (ColourPrimitive)value,
                m.M21 * (ColourPrimitive)value, m.M22 * (ColourPrimitive)value, m.M23 * (ColourPrimitive)value,
                m.M31 * (ColourPrimitive)value, m.M32 * (ColourPrimitive)value, m.M33 * (ColourPrimitive)value);

        public static ColourMatrix operator *(ColourMatrix m, double value)
            => new ColourMatrix(
                m.M11 * (ColourPrimitive)value, m.M12 * (ColourPrimitive)value, m.M13 * (ColourPrimitive)value,
                m.M21 * (ColourPrimitive)value, m.M22 * (ColourPrimitive)value, m.M23 * (ColourPrimitive)value,
                m.M31 * (ColourPrimitive)value, m.M32 * (ColourPrimitive)value, m.M33 * (ColourPrimitive)value);

        public static ColourMatrix operator *(float value, ColourMatrix m)
            => m * (ColourPrimitive)value;

        public static ColourMatrix operator *(double value, ColourMatrix m)
            => m * (ColourPrimitive)value;

        public static ColourMatrix operator /(ColourMatrix m, float value)
            => m * ((ColourPrimitive)1.0 / (ColourPrimitive)value);

        public static ColourMatrix operator /(ColourMatrix m, double value)
            => m * ((ColourPrimitive)1.0 / (ColourPrimitive)value);
        #endregion Multiply, divide

        public ColourPrimitive Determinant => _determinant.Value;
        public ColourMatrix Inverse => _inverse.Value;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        string DisplayString => $"[ " +
            $"[{(ColourPrimitive)M11:0.000}, {(ColourPrimitive)M12:0.000}, {(ColourPrimitive)M13:0.000}]  " +
            $"[{(ColourPrimitive)M21:0.000}, {(ColourPrimitive)M22:0.000}, {(ColourPrimitive)M23:0.000}]  " +
            $"[{(ColourPrimitive)M31:0.000}, {(ColourPrimitive)M32:0.000}, {(ColourPrimitive)M33:0.000}] " +
            $"]";
        public override string ToString() => $"[ " + 
            $"[{(ColourPrimitive)M11}, {(ColourPrimitive)M12}, {(ColourPrimitive)M13}]  " +
            $"[{(ColourPrimitive)M21}, {(ColourPrimitive)M22}, {(ColourPrimitive)M23}]  " +
            $"[{(ColourPrimitive)M31}, {(ColourPrimitive)M32}, {(ColourPrimitive)M33}]  " +
            $"]";

        readonly Lazy<ColourPrimitive> _determinant;
        readonly Lazy<ColourMatrix> _inverse;
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
