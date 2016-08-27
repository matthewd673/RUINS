using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RUINS
{
    public class PngToArray
    {
        
        /// <param name="pngName">Use a 20x20 png</param>
        public static int[,] createArray(string pngName)
        {
            int[,] tempArray = new int[20, 20];
            Bitmap png = new Bitmap(Environment.CurrentDirectory + @"\res\" + pngName + ".png");
            for(int i = 0; i < png.Width; i++)
            {
                for(int j = 0; j < png.Height; j++)
                {
                    //1. check pixel color
                    //2. fill in corresponding value in array
                    Color currentPixel = png.GetPixel(i, j);

                    //LEGEND:
                    //0, 0, 0 = platform
                    //64, 64, 64 = falling platform
                    //255, 0, 0 = lava
                    //0, 255, 0 = goal
                    if (currentPixel.R == 0 && currentPixel.G == 0 && currentPixel.B == 0)
                    {
                        tempArray[i, j] = 1;
                    }
                }
            }
            return tempArray;
        }

    }
}
