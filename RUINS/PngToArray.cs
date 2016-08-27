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
                    //255, 255, 0 = rock;
                    //100, 100, 100 = left ramp
                    //150, 150, 150 = right ramp
                    //0, 0, 255 = objective rock

                    if (currentPixel.R == 0 && currentPixel.G == 0 && currentPixel.B == 255)
                    {
                        tempArray[i, j] = 1;
                    }

                    if (currentPixel.R == 255 && currentPixel.G == 255 && currentPixel.B == 0)
                    {
                        tempArray[i, j] = 2;
                    }

                    if (currentPixel.R == 0 && currentPixel.G == 0 && currentPixel.B == 0)
                    {
                        tempArray[i, j] = 3;
                    }

                    if (currentPixel.R == 64 && currentPixel.G == 64 && currentPixel.B == 64)
                    {
                        tempArray[i, j] = 4;
                    }

                    if (currentPixel.R == 255 && currentPixel.G == 0 && currentPixel.B == 0)
                    {
                        tempArray[i, j] = 5;
                    }

                    if (currentPixel.R == 100 && currentPixel.G == 100 && currentPixel.B == 100)
                    {
                        tempArray[i, j] = 6;
                    }

                    if (currentPixel.R == 150 && currentPixel.G == 150 && currentPixel.B == 150)
                    {
                        tempArray[i, j] = 7;
                    }

                    if (currentPixel.R == 0 && currentPixel.G == 255 && currentPixel.B == 0)
                    {
                        tempArray[i, j] = 8;
                    }
                    
                }
            }
            return tempArray;
        }

    }
}
