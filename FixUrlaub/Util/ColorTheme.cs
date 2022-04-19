using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FixUrlaub.Util
{
    internal class ColorTheme
    {
        /// <summary>
        /// Primary <see cref="Color"/>, usually used for the background
        /// </summary>
        public Color Primary;
        /// <summary>
        /// Secondary <see cref="Color"/>, usually used for foreground elements or Controls
        /// </summary>
        public Color Secondary;
        /// <summary>
        /// Tertiary <see cref="Color"/>, usually used for little highlighted Elements, when given. Defaults to <see cref="ColorTheme.DefaultHighlight"/> when not given
        /// </summary>
        public Color Tertiary;
        /// <summary>
        /// Used to save additional Colors ("Use Case";<see cref="Color"/>)
        /// </summary>
        public Dictionary<string, Color> Additionals;

        public static readonly Color DefaultBackground = Color.FromArgb(243, 255, 79),
                                        DefaultForeground = Color.FromArgb(89, 51, 40),
                                        DefaultHighlight = Color.FromArgb(8, 2, 74);
        /// <summary>
        /// Yellow Background with dark-brown-red Foreground Elemtns and dark blue Hihglights.<br/>
        /// Evokes the look of a physical piece of paper with a Pen written on it
        /// </summary>
        public static ColorTheme Default
        {
            get => new ColorTheme(DefaultBackground, DefaultForeground, DefaultHighlight);
        }
        /// <summary>
        /// Dark preset ColorTheme with gray background and light gray lines
        /// </summary>
        public static ColorTheme Dark
        {
            get => new ColorTheme(Color.FromArgb(40, 40, 40), Color.FromArgb(200, 200, 200), Color.FromArgb(200, 100, 200));
        }
        /// <summary>
        /// White preset ColorTheme, similar to writing on a white piece of paper
        /// </summary>
        public static ColorTheme White
        {
            get => new ColorTheme(Color.White, Color.Black, Color.Blue);
        }


        /// <summary>
        /// Represents the ColoTheme the Forms will follow.<para/>Initialized without any Colors, it will Default to <see cref="Color.Wheat"/> for Primary and Secondary
        /// </summary>
        public ColorTheme() : this(Color.Yellow, Color.Red) { }
        /// <summary>
        /// Represents the ColorTheme the Forms will follow
        /// </summary>
        /// <param name="P">Primary <see cref="Color"/>, usually used for the background</param>
        /// <param name="S">Secondary <see cref="Color"/>, usually used for foreground elements or Controls</param>
        public ColorTheme(Color P, Color S) : this(P, S, DefaultHighlight) { }
        /// <summary>
        /// Represents the ColorTheme the Forms will follow
        /// </summary>
        /// <param name="P">Primary <see cref="Color"/>, usually used for the background</param>
        /// <param name="S">Secondary <see cref="Color"/>, usually used for foreground elements or Controls</param>
        /// <param name="T">Tertiary <see cref="Color"/>, usually used for little highlighted Elements, when given</param>
        public ColorTheme(Color P, Color S, Color T)
        {
            this.Primary = P;
            this.Secondary = S;
            this.Tertiary = T;
        }
        /// <summary>
        /// Represents the ColorTheme the Forms will follow
        /// </summary>
        /// <param name="P">Primary <see cref="Color"/>, usually used for the background</param>
        /// <param name="S">Secondary <see cref="Color"/>, usually used for foreground elements or Controls</param>
        /// <param name="T">Tertiary <see cref="Color"/>, usually used for little highlighted Elements, when given</param>
        /// <param name="addits">Any amount of additional Colors can be safed here. There are certain Keywords that can be used to do special designs</param>
        public ColorTheme(Color P, Color S, Color T, params KeyValuePair<string, Color>[] addits)
        {
            this.Primary = P;
            this.Secondary = S;
            this.Tertiary = T;
            this.Additionals = new Dictionary<string, Color>();
            foreach (KeyValuePair<string, Color> kvp in addits)
                this.Additionals.Add(kvp.Key, kvp.Value);
        }
        /// <summary>
        /// Represents the ColorTheme the Forms will follow
        /// </summary>
        /// <param name="cf">A ColorTheme in the Form of a string. Can be created by using <see cref="ColorTheme.ToString"/></param>
        public ColorTheme(string cf)
        {
            string trim = cf.Substring(11);
            trim = trim.Trim(')');
            string[] args = trim.Split(';');

            this.Primary = Color.FromArgb(
                                int.Parse(args[0].Split(',')[0]),
                                int.Parse(args[0].Split(',')[1]),
                                int.Parse(args[0].Split(',')[2]));
            this.Secondary = Color.FromArgb(
                                int.Parse(args[1].Split(',')[0]),
                                int.Parse(args[1].Split(',')[1]),
                                int.Parse(args[1].Split(',')[2]));
            this.Tertiary = Color.FromArgb(
                                int.Parse(args[2].Split(',')[0]),
                                int.Parse(args[2].Split(',')[1]),
                                int.Parse(args[2].Split(',')[2]));

            if (args[3] != "null")
            {
                Additionals = new Dictionary<string, Color>();

                for (int i = 3; i < args.Length - 1; i++)
                    Additionals.Add(args[i].Split(']')[0].Substring(1),
                        Color.FromArgb(
                                int.Parse(args[i].Split(']')[1].Split(',')[0]),
                                int.Parse(args[i].Split(']')[1].Split(',')[1]),
                                int.Parse(args[i].Split(']')[1].Split(',')[2])));
            }
        }

        /// <summary>
        /// Can be used to add values to a Color, to shift the tone of it
        /// </summary>
        /// <param name="Base"></param>
        /// <param name="Add"></param>
        /// <returns></returns>
        public static Color AddColor(Color Base, Color Add)
            => Color.FromArgb(
                FixMath.Clamp(Base.A + Add.A, 0, 255),
                FixMath.Clamp(Base.R + Add.R, 0, 255),
                FixMath.Clamp(Base.G + Add.G, 0, 255),
                FixMath.Clamp(Base.B + Add.B, 0, 255));
        /// <summary>
        /// Mix 2 colors by blending the tones together, effectively avereging the Inbetween-Result
        /// </summary>
        /// <param name="C1"></param>
        /// <param name="C2"></param>
        /// <returns></returns>
        public static Color MixColor(Color C1, Color C2)
            => Color.FromArgb(
                (C1.A + C2.A) / 2,
                (C1.R + C2.R) / 2,
                (C1.G + C2.G) / 2,
                (C1.B + C2.B) / 2);
        /// <summary>
        /// Inverts a Color, basically makes dark to light and 
        /// </summary>
        /// <param name="C"></param>
        /// <returns></returns>
        public static Color InvertColor(Color C)
            => Color.FromArgb(
                255 - C.A,
                255 - C.R,
                255 - C.G,
                255 - C.B);
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Example: <c>ColorTheme(238,255,107;061,035,020;008,002,074;[Alert]255,000,000;[Good]000,255,000;)</c></returns>
        public override string ToString()
        {
            if (this.Additionals == null)
                return "ColorTheme(" +
                  this.Primary.R.ToString("000") + "," +
                  this.Primary.G.ToString("000") + "," +
                  this.Primary.B.ToString("000") + ";" +
                  this.Secondary.R.ToString("000") + "," +
                  this.Secondary.G.ToString("000") + "," +
                  this.Secondary.B.ToString("000") + ";" +
                  this.Tertiary.R.ToString("000") + "," +
                  this.Tertiary.G.ToString("000") + "," +
                  this.Tertiary.B.ToString("000") + ";null;)";
            else
            {
                string OUT = "ColorTheme(" +
                  this.Primary.R.ToString("000") + "," +
                  this.Primary.G.ToString("000") + "," +
                  this.Primary.B.ToString("000") + ";" +
                  this.Secondary.R.ToString("000") + "," +
                  this.Secondary.G.ToString("000") + "," +
                  this.Secondary.B.ToString("000") + ";" +
                  this.Tertiary.R.ToString("000") + "," +
                  this.Tertiary.G.ToString("000") + "," +
                  this.Tertiary.B.ToString("000") + ";";

                foreach (KeyValuePair<string, Color> keyValuePair in this.Additionals)
                    OUT += "[" + keyValuePair.Key + "]" +
                        keyValuePair.Value.R.ToString("000") + "," +
                        keyValuePair.Value.G.ToString("000") + "," +
                        keyValuePair.Value.B.ToString("000") + ";";

                return OUT + ")";
            }
        }
    }
}