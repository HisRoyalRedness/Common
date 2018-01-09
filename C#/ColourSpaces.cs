using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Windows.Media;
using System.ComponentModel;

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

    #region SRGB
    [DebuggerDisplay("R: {R}, G: {G}, B: {B}, A: {A}, {Hex32}")]
    public struct SRGBColour
    {
        public SRGBColour(RGBColourComponent r, RGBColourComponent g, RGBColourComponent b)
            : this(r, g, b, new RGBColourComponent(255))
        { }

        public SRGBColour(byte r, byte g, byte b)
            : this(new RGBColourComponent(r), new RGBColourComponent(g), new RGBColourComponent(b))
        { }

        public SRGBColour(byte r, byte g, byte b, byte a)
            : this(new RGBColourComponent(r), new RGBColourComponent(g), new RGBColourComponent(b), new RGBColourComponent(a))
        { }

        public SRGBColour(RGBColourComponent r, RGBColourComponent g, RGBColourComponent b, RGBColourComponent a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public string Hex24 => $"#{(byte)R:X2}{(byte)G:X2}{(byte)B:X2}";
        public string Hex32 => $"#{(byte)A:X2}{(byte)R:X2}{(byte)G:X2}{(byte)B:X2}";
        public override string ToString() => Hex32;

        /// <summary>
        /// Red
        /// </summary>
        public RGBColourComponent R { get; set; }
        /// <summary>
        /// Green
        /// </summary>
        public RGBColourComponent G { get; set; }
        /// <summary>
        /// Blue
        /// </summary>
        public RGBColourComponent B { get; set; }
        /// <summary>
        /// Alpha
        /// 0 - 255. 0 = Transparent, 255 = Opaque
        /// </summary>
        public RGBColourComponent A { get; set; }

        public static implicit operator Color(SRGBColour colour) => Color.FromArgb(colour.A, colour.R, colour.G, colour.B);
        public static implicit operator SRGBColour(Color colour) => new SRGBColour(colour.R, colour.G, colour.B, colour.A);
        public static implicit operator ColourVector(SRGBColour colour) => new ColourVector(colour.R, colour.G, colour.B);

    }
    #endregion SRGB

    #region HSVColour
    [DebuggerDisplay("{DisplayString}")]
    public struct HSVColour
    {
        public HSVColour(float h, float s, float v)
            : this(new DegreeColourComponent(h), new UnitColourComponent(s), new UnitColourComponent(v))
        { }

        public HSVColour(float h, float s, float v, float a)
            : this(new DegreeColourComponent(h), new UnitColourComponent(s), new UnitColourComponent(v), new UnitColourComponent(a))
        { }

        public HSVColour(double h, double s, double v)
            : this(new DegreeColourComponent(h), new UnitColourComponent(s), new UnitColourComponent(v))
        { }

        public HSVColour(double h, double s, double v, double a)
            : this(new DegreeColourComponent(h), new UnitColourComponent(s), new UnitColourComponent(v), new UnitColourComponent(a))
        { }

        public HSVColour(DegreeColourComponent h, UnitColourComponent s, UnitColourComponent v)
            : this(h, s, v, new UnitColourComponent(ColourSpaceConstants.ONE))
        { }

        public HSVColour(DegreeColourComponent h, UnitColourComponent s, UnitColourComponent v, UnitColourComponent a)
        {
            H = h;
            S = s;
            V = v;
            A = a;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        string DisplayString => $"H: {(ColourPrimitive)H:0.0°}, S: {(ColourPrimitive)S:0.000}, V: {(ColourPrimitive)V:0.000}, A: {(ColourPrimitive)A:0.000}";
        public override string ToString() => $"H: {(ColourPrimitive)H}, S: {(ColourPrimitive)S}, V: {(ColourPrimitive)V}, A: {(ColourPrimitive)A}";

        /// <summary>
        /// Hue
        /// 0° - 360°. 0° = Red, 120° = Green, 240° = Blue
        /// </summary>
        public DegreeColourComponent H { get; set; }
        /// <summary>
        /// Saturation
        /// 0 - 1. 0 = No saturation (greyscale), 1 = Full saturation (full colour)
        /// </summary>
        public UnitColourComponent S { get; set; }
        /// <summary>
        /// Value
        /// 0 - 1. 0 = Full colour, 1 = Black
        /// </summary>
        public UnitColourComponent V { get; set; }
        public UnitColourComponent A { get; set; }

        public static implicit operator ColourVector(HSVColour colour) => new ColourVector(colour.H, colour.S, colour.V);
    }
    #endregion HSVColour

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

    #region ColourVector
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

        // Aliases
        public ColourPrimitive R => X;
        public ColourPrimitive G => Y;
        public ColourPrimitive B => Z;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        string DisplayString => $"X: {(ColourPrimitive)X:0.000}, Y: {(ColourPrimitive)Y:0.000}, Z: {(ColourPrimitive)Z:0.000}";
        public override string ToString() => $"X: {(ColourPrimitive)X}, Y: {(ColourPrimitive)Y}, Z: {(ColourPrimitive)Z}";


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
    }
    #endregion ColourVector

    #region ColourMatrix
    public struct ColourMatrix
    {
        public ColourMatrix(
            float m11, float m12, float m13,
            float m21, float m22, float m23,
            float m31, float m32, float m33)
            : this(
                  (ColourPrimitive)m11, (ColourPrimitive)m12, (ColourPrimitive)m13,
                  (ColourPrimitive)m21, (ColourPrimitive)m22, (ColourPrimitive)m23,
                  (ColourPrimitive)m31, (ColourPrimitive)m32, (ColourPrimitive)m33,
                  false)
        { }

        public ColourMatrix(
            double m11, double m12, double m13,
            double m21, double m22, double m23,
            double m31, double m32, double m33)
            : this(
                  (ColourPrimitive)m11, (ColourPrimitive)m12, (ColourPrimitive)m13,
                  (ColourPrimitive)m21, (ColourPrimitive)m22, (ColourPrimitive)m23,
                  (ColourPrimitive)m31, (ColourPrimitive)m32, (ColourPrimitive)m33,
                  false)
        { }

        private ColourMatrix(
            ColourPrimitive m11, ColourPrimitive m12, ColourPrimitive m13,
            ColourPrimitive m21, ColourPrimitive m22, ColourPrimitive m23,
            ColourPrimitive m31, ColourPrimitive m32, ColourPrimitive m33,
            bool dummy)
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
                m.M31 * v.X + m.M32 * v.Y + m.M33 * v.Z,
                v.IsLimited);

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

        public ColourPrimitive Determinant => _determinant.Value;
        public ColourMatrix Inverse => _inverse.Value;

        readonly Lazy<ColourPrimitive> _determinant;
        readonly Lazy<ColourMatrix> _inverse;
    }
    #endregion ColourMatrix

    internal static class ColourSpaceConstants
    {
        internal const ColourPrimitive ZERO = (ColourPrimitive)0.0;
        internal const ColourPrimitive ONE = (ColourPrimitive)1.0;
        internal const ColourPrimitive TWO = (ColourPrimitive)2.0;
        internal const ColourPrimitive FOUR = (ColourPrimitive)4.0;
        internal const ColourPrimitive SIXTY = (ColourPrimitive)60.0;
        internal const ColourPrimitive TWO_FIVE_FIVE = (ColourPrimitive)255.0;
        internal const ColourPrimitive THREE_SIXTY = (ColourPrimitive)360.0;
    }

    public static class ColourSpaceExtensions
    {
        // https://en.wikipedia.org/wiki/SRGB

        //public static SRGBColour ToRGB(this HSVColour hsv)
        //{

        //}

        public static HSVColour ToHSV(this SRGBColour srgb)
        {
            var scaledRGB = (ColourVector)srgb / ColourSpaceConstants.TWO_FIVE_FIVE;
            var max = scaledRGB.Max();
            var min = scaledRGB.Min();
            var chroma = max - min;

            var hue = new DegreeColourComponent(ColourSpaceConstants.ZERO);
            var sat = new UnitColourComponent(ColourSpaceConstants.ZERO);
            var val = new UnitColourComponent(max);

            // https://en.wikipedia.org/wiki/HSL_and_HSV#Hue_and_chroma
            // https://en.wikipedia.org/wiki/HSL_and_HSV#Saturation
            // https://en.wikipedia.org/wiki/HSL_and_HSV#Lightness
            if (chroma != 0)
            {
                var h_prime = max == scaledRGB.R
                    ? (scaledRGB.G - scaledRGB.B) / chroma
                    : (max == scaledRGB.G
                        ? (scaledRGB.B - scaledRGB.R) / chroma + ColourSpaceConstants.TWO
                        : (scaledRGB.R - scaledRGB.G) / chroma + ColourSpaceConstants.FOUR);
                hue.Value = ColourSpaceConstants.SIXTY * h_prime;
                sat.Value = chroma / max;
            }
            return new HSVColour(hue, sat, val, (ColourPrimitive)srgb.A / ColourSpaceConstants.TWO_FIVE_FIVE);
        }

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
