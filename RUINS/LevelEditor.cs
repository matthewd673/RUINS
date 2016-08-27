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
            //fill the space in with a demo level
            //this is very temporary, but LevelEditor is best equipped for the test
            map = PngToArray.createArray("test-level");
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

            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    //these values are consistent with the PhysicsObject "type"s + 1 to account for empty space
                    switch(map[i, j])
                    {

                        case 1:
                            //objective rock
                            Program.s.render(Program.objectiveRock, i * 32, j * 32);
                            break;

                        case 2:
                            //rock
                            Program.s.render(Program.rock, i * 32, j * 32);
                            break;

                        case 3:
                            //platform
                            Program.s.render(Program.platform, i * 32, j * 32);
                            break;

                        case 4:
                            //falling platform
                            Program.s.render(Program.fallingPlatform, i * 32, j * 32);
                            break;

                        case 5:
                            //lava
                            Program.s.render(Program.lava, i * 32, j * 32);
                            break;

                        case 6:
                            //left ramp
                            Program.s.render(Program.leftRamp, i * 32, j * 32);
                            break;

                        case 7:
                            //right ramp
                            Program.s.render(Program.rightRamp, i * 32, j * 32);
                            break;

                        case 8:
                            //goal
                            Program.s.render(Program.goal, i * 32, j * 32);
                            break;
                    }
                }
            }
            
            for (int i = 0; i < clickTiles.Count; i++)
            {
                //safety first!
                if(clickTiles[i].GetType() == typeof(Rect))
                {
                    Rect placeholder = (Rect)clickTiles[i];
                    int mouseX = Program.s.getMouseX();
                    int mouseY = Program.s.getMouseY();
                    if(placeholder.intersects(new Vector2i(mouseX, mouseY)))
                    {
                        Program.s.render(new Shape.Rectangle(32, 32, true), (int)placeholder.x, (int)placeholder.y, 1, Brushes.Orange);
                    }
                }
            }
            
        }

    }
}
