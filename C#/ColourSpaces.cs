using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Media;

namespace HisRoyalRedness.com
{
    using ColourPrimitive = Double;

    [DebuggerDisplay("{X}, {Y}, {Z}: {Illuminant}")]
    public struct CIEXYZColour
    {
        // https://en.wikipedia.org/wiki/CIE_1931_color_space

        public CIEXYZColour(ColourPrimitive x, ColourPrimitive y, ColourPrimitive z)
            : this(x, y, z, Illuminants.D65)
        { }

        public CIEXYZColour(ColourPrimitive x, ColourPrimitive y, ColourPrimitive z, Illuminants illuminant)
        {
            X = x;
            Y = y;
            Z = z;
            Illuminant = illuminant;
        }

        public ColourPrimitive X { get; private set; }
        public ColourPrimitive Y { get; private set; }
        public ColourPrimitive Z { get; private set; }
        public Illuminants Illuminant { get; private set; }
    }

    public enum Illuminants
    {
        A = 0,              // Incandescent/tungsten
        B,                  // Old direct sunlight at noon
        C,                  // Old daylight
        D50,                // ICC profile PCS
        D55,                // Mid-morning daylight
        D65,                // Daylight, sRGB, Adobe-RGB
        D75,                // North sky daylight
        E,                  // Equal energy
        F1,                 // Daylight Fluorescent
        F2,                 // Cool fluorescent
        F3,                 // White Fluorescent
        F4,                 // Warm White Fluorescent
        F5,                 // Daylight Fluorescent
        F6,                 // Lite White Fluorescent
        F7,                 // Daylight fluorescent, D65 simulator
        F8,                 // Sylvania F40, D50 simulator
        F9,                 // Cool White Fluorescent
        F10,                // Ultralume 50, Philips TL85
        F11,                // Ultralume 40, Philips TL84
        F12                 // Ultralume 30, Philips TL83
    }

    #region ColourMatrix
    public struct ColourMatrix
    {
        public ColourMatrix(
            ColourPrimitive m11, ColourPrimitive m12, ColourPrimitive m13,
            ColourPrimitive m21, ColourPrimitive m22, ColourPrimitive m23,
            ColourPrimitive m31, ColourPrimitive m32, ColourPrimitive m33)
        {
            M11 = m11; M12 = m12; M13 = m13;
            M21 = m21; M22 = m22; M23 = m23;
            M31 = m31; M32 = m32; M33 = m33;

            _determinant = new Lazy<ColourPrimitive>(() => m11 * m22 * m33 + m12 * m23 * m31 + m13 * m21 * m32 - m31 * m22 * m13 - m32 * m23 * m11 - m33 * m21 * m12);
            _inverse = new Lazy<ColourMatrix>(() =>
            {
                // https://en.wikipedia.org/wiki/Invertible_matrix#Inversion_of_3_%C3%97_3_matrices
                var A = (m22 * m33 - m23 * m32);
                var B = (m23 * m31 - m21 * m33);
                var C = (m21 * m32 - m22 * m31);
                var D = (m13 * m32 - m12 * m33);
                var E = (m11 * m33 - m13 * m31);
                var F = (m12 * m31 - m11 * m32);
                var G = (m12 * m23 - m13 * m22);
                var H = (m13 * m21 - m11 * m23);
                var I = (m11 * m22 - m12 * m21);
                var det = m11 * A + m12 * B + m13 * C; // Rule of Sarrus
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

        public static CIEXYZColour operator *(ColourMatrix m, CIEXYZColour v)
            => new CIEXYZColour(
                m.M11 * v.X + m.M12 * v.Y + m.M13 * v.Z,
                m.M21 * v.X + m.M22 * v.Y + m.M23 * v.Z,
                m.M31 * v.X + m.M32 * v.Y + m.M33 * v.Z);

        public static ColourMatrix operator *(ColourMatrix m, ColourPrimitive value)
            => new ColourMatrix(
                m.M11 * value, m.M12 * value, m.M13 * value,
                m.M21 * value, m.M22 * value, m.M23 * value,
                m.M31 * value, m.M32 * value, m.M33 * value);

        public static ColourMatrix operator *(ColourPrimitive value, ColourMatrix m)
            => m * value;

        public static ColourMatrix operator /(ColourMatrix m, ColourPrimitive value)
            => m * (1.0 / value);

        public ColourPrimitive Determinant => _determinant.Value;
        public ColourMatrix Inverse => _inverse.Value;

        readonly Lazy<ColourPrimitive> _determinant;
        readonly Lazy<ColourMatrix> _inverse;
    }
    #endregion ColourMatrix

    public static class ColourSpaceExtensions
    {
        // https://en.wikipedia.org/wiki/SRGB


        // Assume xyz uses values in the range from 0.0 to 1.0
        public static Color ToRGB(this CIEXYZColour xyz)
        {
            var d65_xyz = xyz;
            if (d65_xyz.Illuminant != Illuminants.D65)
            {
                // Todo: Convert to D65 white

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
                // Todo: Convert from D65 white
            }
            return xyz;
        }

        #region Internal conversion and correction
        static byte ClipPrimitiveToByte(ColourPrimitive primitive)
            => primitive >= (ColourPrimitive)1.0
                ? (byte)255
                : (primitive <= (ColourPrimitive)0.0
                    ? (byte)0
                    : (byte)(primitive * 255));
        static ColourPrimitive ByteToPrimitive(byte value)
            => (ColourPrimitive)(value / 255.0);

        static ColourPrimitive GammaCorrectRGB2XYZ(ColourPrimitive primitive)
            => primitive >= 0.0031308
                ? 1.055 * (Math.Pow(primitive, (1.0 / 2.4))) - 0.055
                : 12.92 * primitive;
        static ColourPrimitive GammaCorrectXYZ2RGB(ColourPrimitive primitive)
            => primitive >= 0.04045
                ? Math.Pow(((primitive + 0.055) / (1 + 0.055)), 2.4)
                : primitive / 12.92;
        #endregion Internal conversion and correction

        #region Scaling
        public static CIEXYZColour Scale(this CIEXYZColour xyz, ColourPrimitive xFactor, ColourPrimitive yFactor, ColourPrimitive zFactor)
            => new CIEXYZColour(xyz.X * xFactor, xyz.Y * yFactor, xyz.Z * zFactor);
        public static CIEXYZColour Scale(this CIEXYZColour xyz, ColourPrimitive factor)
            => new CIEXYZColour(xyz.X * factor, xyz.Y * factor, xyz.Z * factor);
        #endregion Scaling

        #region Conversion matrices
        static readonly ColourMatrix _sRGB2CIEXYZ_D65_2deg = new ColourMatrix(
             3.2406, -1.5372, -0.4986,
            -0.9689,  1.8758,  0.0415,
             0.0557, -0.2040,  1.0570);
        static readonly ColourMatrix _CIEXYZ2sRGB_D65_2deg = _sRGB2CIEXYZ_D65_2deg.Inverse;
        #endregion Conversion matrices
    }
}
