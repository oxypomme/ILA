﻿using ILANET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ilaGUI
{
    public partial class App
    {
        public static BitmapImage ConvertBitmapToWPF(System.Drawing.Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            src.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        public static System.Drawing.Bitmap ConvertWPFToBitmap(BitmapSource src)
        {
            //https://stackoverflow.com/a/6484754
            using var outStream = new MemoryStream();
            var enc = new PngBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create(src));
            enc.Save(outStream);
            var bitmap = new System.Drawing.Bitmap(outStream);

            return new System.Drawing.Bitmap(bitmap);
        }

        public static BitmapImage GetBitmapImage(Stream stream)
        {
            //https://stackoverflow.com/a/9564425
            var image = new BitmapImage();
            stream.Position = 0;
            image.BeginInit();
            image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = null;
            image.StreamSource = stream;
            image.EndInit();
            image.Freeze();
            return image;
        }

        public static System.Drawing.Color GetDarkThemeColor(System.Drawing.Color c)
        {
            var wpfColor = Color.FromArgb(c.A, c.R, c.G, c.B);
            wpfColor = GetDarkThemeColor(wpfColor);
            return ToClassic(GetDarkThemeColor(ToWPF(c)));
        }

        public static Color GetDarkThemeColor(Color c)
        {
            var hsl = GetHSL(c);
            double luminosity = 1 - hsl.Item3;
            double haloLuminosity = 1 - GetHSL(Color.FromArgb(255, 240, 240, 240)).Item3;
            double themeBackgroundLuminosity = GetHSL(DarkThemeColor).Item3;

            if (luminosity < haloLuminosity)
                return GetRGB(c.A, hsl.Item1, hsl.Item2, themeBackgroundLuminosity * luminosity / haloLuminosity);
            else
                return GetRGB(c.A, hsl.Item1, hsl.Item2, (1.0 - themeBackgroundLuminosity) * (luminosity - 1.0) / (1.0 - haloLuminosity) + 1.0);
        }

        public static (double, double, double) GetHSL(Color c)
        {
            double r = c.R / 255.0;
            double g = c.G / 255.0;
            double b = c.B / 255.0;
            double max = Math.Max(Math.Max(r, g), b);
            double min = Math.Min(Math.Min(r, g), b);
            double lum = (max + min) / 2;
            double satur, hue;
            if (min == max)
                satur = 0;
            else if (lum < .5)
                satur = (max - min) / (max + min);
            else
                satur = (max - min) / (2.0 - max - min);
            if (r == max)
                hue = (g - b) / (max - min);
            else if (g == max)
                hue = 2.0 + (b - r) / (max - min);
            else
                hue = 4.0 + (r - g) / (max - min);
            hue = (hue * 60 + 360) % 360;
            return (hue, satur, lum);
        }

        public static Color GetRGB((double, double, double) c) => GetRGB(255, c.Item1, c.Item2, c.Item3);

        public static Color GetRGB(byte alpha, double hue, double satur, double lum)
        {
            double v;
            hue /= 360;
            double r, g, b;

            r = lum;   // default to gray

            g = lum;

            b = lum;

            v = (lum <= 0.5) ? (lum * (1.0 + satur)) : (lum + satur - lum * satur);

            if (v > 0)

            {
                double m;

                double sv;

                int sextant;

                double fract, vsf, mid1, mid2;

                m = lum + lum - v;

                sv = (v - m) / v;

                hue *= 6.0;

                sextant = (int)hue;

                fract = hue - sextant;

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

            Color rgb;

            rgb.R = Convert.ToByte(r * 255.0f);

            rgb.G = Convert.ToByte(g * 255.0f);

            rgb.B = Convert.ToByte(b * 255.0f);
            rgb.A = alpha;
            return rgb;
        }

        public static Control GetValueControl(IValue value)
        {
            if (value is ConstantBool)
                return new Editor.ConstantBool(value as ConstantBool);
            else if (value is ConstantChar)
                return new Editor.ConstantChar(value as ConstantChar);
            else if (value is ConstantFloat)
                return new Editor.ConstantFloat(value as ConstantFloat);
            else if (value is ConstantInt)
                return new Editor.ConstantInt(value as ConstantInt);
            else if (value is ConstantString)
                return new Editor.ConstantString(value as ConstantString);
            else if (value is EnumCall)
                return new Editor.EnumCall(value as EnumCall);
            else if (value is FunctionCall)
                return new Editor.FunctionCall(value as FunctionCall);
            else if (value is Operator)
                return new Editor.Operator(value as Operator);
            else if (value is StructCall)
                return new Editor.StructCall(value as StructCall);
            else if (value is TableCall)
                return new Editor.TableCall(value as TableCall);
            else if (value is Variable)
                return new Editor.Variable();
            else
                return null;
        }

        public static bool isNameAvailable(string name, IExecutable source, Program ilacode)
        {
            foreach (var item in ilacode.Declarations)
            {
                if (item is VariableDeclaration vd)
                    if (vd.CreatedVariable.Name == name)
                        return false;
                if (item is TypeDeclaration td)
                {
                    if (td.CreatedType.Name == name)
                        return false;
                    if (td.CreatedType is EnumType et)
                        foreach (var item2 in et.Values)
                            if (item2 == name)
                                return false;
                }
            }
            foreach (var item in ilacode.Methods)
            {
                if (item.Name == name)
                    return false;
            }
            if (source is Module m)
            {
                foreach (var item in m.Parameters)
                    if (item.ImportedVariable.Name == name)
                        return false;
                foreach (var item in m.Declarations)
                    if (item.CreatedVariable.Name == name)
                        return false;
            }
            return true;
        }

        public static bool isNameAvailable(string name) => isNameAvailable(name, CurrentExecutable, CurrentILAcode);

        public static bool isNameAvailable(string name, IExecutable source) => isNameAvailable(name, source, CurrentILAcode);

        public static bool isNameConventionnal(string name)
        {
            if (name.Length == 0)
                return false;
            if (!char.IsLetter(name.First()))
                return false;
            foreach (var item in name)
                if (!(char.IsLetterOrDigit(item) || item == '_') || "éàèêëîïôöûüçÈÉÊËÂÎÇÀÏÔÖÛÜ".Contains(item))
                    return false;
            return true;
        }

        public static BitmapSource MakeDarkTheme(BitmapSource image)
        {
            var src = ConvertWPFToBitmap(image);
            MakeDarkTheme(src);
            return ConvertBitmapToWPF(MakeDarkTheme(src));
        }

        public static System.Drawing.Bitmap MakeDarkTheme(System.Drawing.Bitmap bitmap)
        {
            bitmap = new System.Drawing.Bitmap(bitmap);
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    bitmap.SetPixel(x, y, GetDarkThemeColor(bitmap.GetPixel(x, y)));
                }
            }
            return bitmap;
        }

        public static bool recursiveSearch(IEnumerable<IDropableInstruction> array, IDropableInstruction toFind)
        {
            foreach (var item in array)
            {
                if (item == toFind)
                    return true;
                if (item is InstructionBlock ib)
                    if (recursiveSearch(ib.Instructions, toFind))
                        return true;
            }
            return false;
        }

        public static System.Drawing.Color ToClassic(Color c) => System.Drawing.Color.FromArgb(c.A, c.R, c.G, c.B);

        public static Color ToWPF(System.Drawing.Color c) => Color.FromArgb(c.A, c.R, c.G, c.B);
    }
}