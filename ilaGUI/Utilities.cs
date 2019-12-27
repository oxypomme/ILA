using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace ilaGUI
{
    public partial class App
    {
        public static Color GetDarkThemeColor(Color c)
        {
            return Colors.White;
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

        public (double, double, double) getHSL(Color c)
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

        /*private double TransformLuminosity(double luminosity)
     {
         double haloLuminosity = Color.FromArgb(255, 246, 246, 246);
         double themeBackgroundLuminosity = App.DarkBackground.L;

         if (themeBackgroundLuminosity < .5) //LuminosityInversionThreshold = 0.5
         {
             haloLuminosity = 1.0 - haloLuminosity;
             luminosity = 1.0 - luminosity;
         }

         if (luminosity < haloLuminosity)
         {
             return themeBackgroundLuminosity * luminosity / haloLuminosity;
         }

         return (1.0 - themeBackgroundLuminosity) * (luminosity - 1.0) / (1.0 - haloLuminosity) + 1.0;
     }*/
    }
}