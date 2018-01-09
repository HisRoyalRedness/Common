using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Media;
using System.ComponentModel;
using HisRoyalRedness.com.ColourConstants;

/*
    Some additional colour space definitions (other than the built-in sRGB)

    Keith Fletcher
    Nov 2017

    This file is Unlicensed.
    See the foot of the file, or refer to <http://unlicense.org>
*/

namespace HisRoyalRedness.com
{
    using ColourPrimitive = Double;

    [DebuggerDisplay("X: {X}, Y: {Y}, Z: {Z}, Illum: {Illuminant}")]
    public struct CIEXYZColour
    {
        // https://en.wikipedia.org/wiki/CIE_1931_color_space

        public CIEXYZColour(float x, float y, float z, bool isLimited = true)
            : this((ColourPrimitive)x, (ColourPrimitive)y, (ColourPrimitive)z, Illuminants.D65, isLimited)
        { }

        public CIEXYZColour(double x, double y, double z, bool isLimited = true)
            : this((ColourPrimitive)x, (ColourPrimitive)y, (ColourPrimitive)z, Illuminants.D65, isLimited)
        { }

        public CIEXYZColour(ColourPrimitive x, ColourPrimitive y, ColourPrimitive z, Illuminants illuminant, bool isLimited = true)
        {
            if (isLimited)
            {
                if (x > 1.0 || x < 0.0)
                    throw new ArgumentOutOfRangeException(nameof(x));
                if (y > 1.0 || y < 0.0)
                    throw new ArgumentOutOfRangeException(nameof(y));
                if (z > 1.0 || z < 0.0)
                    throw new ArgumentOutOfRangeException(nameof(z));
            }
            X = x;
            Y = y;
            Z = z;
            Illuminant = illuminant;
            IsLimited = isLimited;
        }

        public ColourPrimitive X { get; private set; }
        public ColourPrimitive Y { get; private set; }
        public ColourPrimitive Z { get; private set; }
        public Illuminants Illuminant { get; private set; }
        public bool IsLimited { get; private set; }

        public static implicit operator ColourVector(CIEXYZColour colour) => new ColourVector(colour.X, colour.Y, colour.Z);
    }

    public enum Illuminants
    {
        [Description("Incandescent/tungsten")]
        A = 0,
        [Description("Old direct sunlight at noon")]
        B,
        [Description("Old daylight")]
        C,
        [Description("ICC profile PCS")]
        D50,
        [Description("Mid-morning daylight")]
        D55,
        [Description("Daylight, sRGB, Adobe-RGB")]
        D65,
        [Description("North sky daylight")]
        D75,
        [Description("Equal energy")]
        E,
        [Description("Daylight Fluorescent")]
        F1,
        [Description("Cool fluorescent")]
        F2,
        [Description("White Fluorescent")]
        F3,
        [Description("Warm White Fluorescent")]
        F4,
        [Description("Daylight Fluorescent")]
        F5,
        [Description("Lite White Fluorescent")]
        F6,
        [Description("Daylight fluorescent, D65 simulator")]
        F7,
        [Description("Sylvania F40, D50 simulator")]
        F8,
        [Description("Cool White Fluorescent")]
        F9,
        [Description("Ultralume 50, Philips TL85")]
        F10,
        [Description("Ultralume 40, Philips TL84")]
        F11,
        [Description("Ultralume 30, Philips TL83")]
        F12
    }

    public static class ColourSpaceExtensions
    {
        // https://en.wikipedia.org/wiki/SRGB

        //public static SRGBColour ToRGB(this HSVColour hsv)
        //{

        //}

        // Assume xyz uses values in the range from 0.0 to 1.0
        public static Color ToRGB(this CIEXYZColour xyz)
        {
            var d65_xyz = xyz;
            if (d65_xyz.Illuminant != Illuminants.D65)
            {
                // Todo: Convert to D65

            }

            // Convert to linear rgb
            var linear_rgb = _sRGB2CIEXYZ_D65_2deg * d65_xyz;

            // Gamma correct and clip to 0-255
            return Color.FromRgb(
                ClipPrimitiveToByte(GammaCorrectRGB2XYZ(linear_rgb.X)),
                ClipPrimitiveToByte(GammaCorrectRGB2XYZ(linear_rgb.Y)),
                ClipPrimitiveToByte(GammaCorrectRGB2XYZ(linear_rgb.Z)));
        }

        public static CIEXYZColour ToCIEXYZ(this Color rgb, Illuminants illuminant = Illuminants.D65)
        {
            var linear_rgb = new CIEXYZColour(
                GammaCorrectXYZ2RGB(ByteToPrimitive(rgb.R)),
                GammaCorrectXYZ2RGB(ByteToPrimitive(rgb.G)),
                GammaCorrectXYZ2RGB(ByteToPrimitive(rgb.B)));

            var xyz = _CIEXYZ2sRGB_D65_2deg * linear_rgb;
            if (xyz.Illuminant != illuminant)
            {
                // Todo: Convert from D65
            }
            return xyz;
        }

        #region Internal conversion and correction
        static byte ClipPrimitiveToByte(ColourPrimitive primitive)
            => primitive >= ColourSpaceConstants.ONE
                ? (byte)255
                : (primitive <= ColourSpaceConstants.ZERO
                    ? (byte)0
                    : (byte)(primitive * ColourSpaceConstants.TWO_FIVE_FIVE));
        static ColourPrimitive ByteToPrimitive(byte value)
            => (ColourPrimitive)value / ColourSpaceConstants.TWO_FIVE_FIVE;


        // Constants (properly defined as ColourPrimitive), needed when converting XYZ to and from RGB
        const ColourPrimitive XYZ_RGB_A = (ColourPrimitive)0.0031308;
        const ColourPrimitive XYZ_RGB_B = (ColourPrimitive)0.04045;
        const ColourPrimitive XYZ_RGB_C = (ColourPrimitive)0.055;
        const ColourPrimitive XYZ_RGB_D = ColourSpaceConstants.ONE + XYZ_RGB_C;
        const ColourPrimitive XYZ_RGB_E = (ColourPrimitive)2.4;
        const ColourPrimitive XYZ_RGB_F = ColourSpaceConstants.ONE / XYZ_RGB_E;
        const ColourPrimitive XYZ_RGB_G = (ColourPrimitive)12.92;

        static ColourPrimitive GammaCorrectRGB2XYZ(ColourPrimitive primitive)
            => primitive >= XYZ_RGB_A
                ? XYZ_RGB_D * (ColourPrimitive)Math.Pow(primitive, XYZ_RGB_F) - XYZ_RGB_C
                : XYZ_RGB_G * primitive;
        static ColourPrimitive GammaCorrectXYZ2RGB(ColourPrimitive primitive)
            => primitive >= XYZ_RGB_B
                ? (ColourPrimitive)Math.Pow(((primitive + XYZ_RGB_C) / XYZ_RGB_D), XYZ_RGB_E)
                : primitive / XYZ_RGB_G;
        #endregion Internal conversion and correction

        #region Scaling
        public static CIEXYZColour Scale(this CIEXYZColour xyz, ColourPrimitive xFactor, ColourPrimitive yFactor, ColourPrimitive zFactor)
            => new CIEXYZColour(xyz.X * xFactor, xyz.Y * yFactor, xyz.Z * zFactor);
        public static CIEXYZColour Scale(this CIEXYZColour xyz, ColourPrimitive factor)
            => new CIEXYZColour(xyz.X * factor, xyz.Y * factor, xyz.Z * factor);
        #endregion Scaling

        #region Conversion matrices
        static readonly ColourMatrix _sRGB2CIEXYZ_D65_2deg = new ColourMatrix(
            (ColourPrimitive)( 3.2406), (ColourPrimitive)(-1.5372), (ColourPrimitive)(-0.4986),
            (ColourPrimitive)(-0.9689), (ColourPrimitive)( 1.8758), (ColourPrimitive)( 0.0415),
            (ColourPrimitive)( 0.0557), (ColourPrimitive)(-0.2040), (ColourPrimitive)( 1.0570));
        static readonly ColourMatrix _CIEXYZ2sRGB_D65_2deg = _sRGB2CIEXYZ_D65_2deg.Inverse;
        #endregion Conversion matrices

        #region Min and Max
        public static ColourPrimitive Min(this ColourVector v) => Math.Min(v.X, Math.Min(v.Y, v.Z));
        public static ColourPrimitive Max(this ColourVector v) => Math.Max(v.X, Math.Max(v.Y, v.Z));
        #endregion Min and Max
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
