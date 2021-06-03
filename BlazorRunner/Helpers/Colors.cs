using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace BlazorRunner.Runner.Helpers
{
    public static class Colors
    {
        public static readonly string[] Rainbow = {
            "rgb(75,0,130)",
            "rgb(0,0,255)",
            "rgb(0,128,0)",
            "rgb(255,255,0)",
            "rgb(255,165,0)",
            "rgb(255,0,0)",
        };

        public static readonly string[] RainbowComplementaries = {
            GetComplimentaryColor(75,0,130),
            GetComplimentaryColor(0,0,255),
            GetComplimentaryColor(0,128,0),
            GetComplimentaryColor(255,255,0),
            GetComplimentaryColor(255,165,0),
            GetComplimentaryColor(255,0,0),
        };

        public static readonly string[] PastelRainbow = {
            "rgb(204,153,201)",
            "rgb(158,193,207)",
            "rgb(158,224,158)",
            "rgb(253,253,151)",
            "rgb(254,177,68)",
            "rgb(255,102,99)"
        };

        public static readonly string[] PastelRainbowComplementaries = {
            GetComplimentaryColor(204,153,201),
            GetComplimentaryColor(158,193,207),
            GetComplimentaryColor(158,224,158),
            GetComplimentaryColor(253,253,151),
            GetComplimentaryColor(254,177,68),
            GetComplimentaryColor(255,102,99),
        };

        public static string[] AllColors
        {
            get
            {
                if (_AllColors is null)
                {
                    _AllColors = PastelRainbow.ToList().Concat(Rainbow.ToList()).ToArray();
                }
                return _AllColors;
            }
        }

        private static string[] _AllColors = null;

        public static string[] AllComplementaries
        {
            get
            {
                if (_AllComplementaries is null)
                {
                    _AllComplementaries = PastelRainbowComplementaries.ToList().Concat(RainbowComplementaries.ToList()).ToArray();
                }
                return _AllComplementaries;
            }
        }

        private static string[] _AllComplementaries = null;


        public static (string, string) GetRandomColorPair(int index)
        {
            double h = index % 360d;

            double s = 0.89d;

            double l = 0.68d;

            Color rgb = ColorFromHSL(h / 360f, s, l);

            return (ToCssString(rgb), GetComplimentaryColor(rgb.R, rgb.G, rgb.B));
        }

        public static string ToCssString(Color c)
        {
            return $"rgb({c.R},{c.G},{c.B})";
        }

        public static string GetComplimentaryColor(int r, int g, int b)
        {
            Color c = Color.FromArgb(r, g, b);

            float hue = c.GetHue();
            float light = c.GetBrightness();
            float saturation = c.GetSaturation();

            // rotate the hue 180deg

            hue += 180f;

            if (hue > 360f)
            {
                hue -= 360f;
            }

            Color complimentary = ColorFromHSL(hue / 360f, saturation, light);

            return ToCssString(complimentary);
        }

        private static Color ColorFromHSL(double h, double s, double l)
        {

            double v;

            double r, g, b;



            r = l;   // default to gray

            g = l;

            b = l;

            v = (l <= 0.5) ? (l * (1.0 + s)) : (l + s - l * s);

            if (v > 0)

            {

                double m;

                double sv;

                int sextant;

                double fract, vsf, mid1, mid2;



                m = l + l - v;

                sv = (v - m) / v;

                h *= 6.0;

                sextant = (int)h;

                fract = h - sextant;

                vsf = v * sv * fract;

                mid1 = m + vsf;

                mid2 = v - vsf;

                switch (sextant)

                {

                    case 0:

                        r = v;

                        g = mid1;

                        b = m;

                        break;

                    case 1:

                        r = mid2;

                        g = v;

                        b = m;

                        break;

                    case 2:

                        r = m;

                        g = v;

                        b = mid1;

                        break;

                    case 3:

                        r = m;

                        g = mid2;

                        b = v;

                        break;

                    case 4:

                        r = mid1;

                        g = m;

                        b = v;

                        break;

                    case 5:

                        r = v;

                        g = m;

                        b = mid2;

                        break;

                }

            }
            return Color.FromArgb(Convert.ToByte(r * 255.0f), Convert.ToByte(g * 255.0f), Convert.ToByte(b * 255.0f));

        }

    }
}
