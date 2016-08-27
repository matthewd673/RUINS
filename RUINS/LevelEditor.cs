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

        public static bool customMap;

        public static void initMap(string mapName)
        {
            for (int i = 0; i <= 20; i++)
            {
                for(int j = 0; j <= 20; j++)
                {
                    Rect newRect = new Rect(i * 32, j * 32, 32, 32);
                    clickTiles.Add(newRect);
                }
            }

            if (mapName == "CUSTOM")
                customMap = true;

            if(!customMap)
                map = PngToArray.createArray(mapName);
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
                        Program.s.render(new Shape.Rectangle(32, 32), (int)placeholder.x, (int)placeholder.y, 3, Brushes.Orange);
                        int mapValue = 0;
                        if((int)(placeholder.x / 32) < 20 && (int)(placeholder.y / 32) < 20)
                            mapValue = map[(int)placeholder.x / 32, (int)placeholder.y / 32];
                        //identify tile
                        switch(mapValue)
                        {
                            case 0:
                                Program.s.render("Tile: Nothing", 0, 672, Brushes.White);
                                break;
                            case 1:
                                Program.s.render("Tile: Objective Rock", 0, 672, Brushes.White);
                                break;
                            case 2:
                                Program.s.render("Tile: Rock", 0, 672, Brushes.White);
                                break;
                            case 3:
                                Program.s.render("Tile: Platform", 0, 672, Brushes.White);
                                break;
                            case 4:
                                Program.s.render("Tile: Falling Platform", 0, 672, Brushes.White);
                                break;
                            case 5:
                                Program.s.render("Tile: Lava", 0, 672, Brushes.White);
                                break;
                            case 6:
                                Program.s.render("Tile: Left-facing Ramp", 0, 672, Brushes.White);
                                break;
                            case 7:
                                Program.s.render("Tile: Right-facing Ramp", 0, 672, Brushes.White);
                                break;
                            case 8:
                                Program.s.render("Tile: Goal", 0, 672, Brushes.White);
                                break;
                        }
                    }
                }
            }
            
        }

    }
}
