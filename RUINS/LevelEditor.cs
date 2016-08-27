using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using neon2d;
using neon2d.Physics;
using neon2d.Math;

namespace RUINS
{
    public class LevelEditor
    {

        public static int[,] map = new int[20, 20];
        public static ArrayList clickTiles = new ArrayList();

        public static void initMap()
        {
            for (int i = 0; i <= 20; i++)
            {
                for(int j = 0; j <= 20; j++)
                {
                    Rect newRect = new Rect(i * 32, j * 32, 32, 32);
                    clickTiles.Add(newRect);
                }
            }
        }

        public static void edit()
        {
            //change these numbers later to prevent overflow
            for(int i = 0; i <= 20; i++)
            {
                for(int j = 0; j <= 20; j++)
                {
                    //fill in the tiles
                    Program.s.render(new Shape.Rectangle(32, 32), i * 32, j * 32, 1, Brushes.Gray);
                }
            }

            //NOTE TO SELF: Work on this later
            for(int i = 0; i < clickTiles.Count; i++)
            {
                //safety first!
                if(clickTiles[i].GetType() == typeof(Rect))
                {
                    Rect placeholder = (Rect)clickTiles[i];
                    int mouseX = Program.s.getMouseX();
                    int mouseY = Program.s.getMouseY();
                    MessageBox.Show(mouseX.ToString() + "; " + mouseY.ToString());
                    if(placeholder.intersects(new Vector2i(mouseX, mouseY)))
                    {
                        Program.s.render(new Shape.Rectangle(32, 32, true), (int)placeholder.x, (int)placeholder.y, 1, Brushes.Orange);
                    }
                }
            }
        }

    }
}
