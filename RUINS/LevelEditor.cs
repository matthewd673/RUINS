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
        public static bool[,] presetMap = new bool[20, 20];
        public static ArrayList clickTiles = new ArrayList();

        public static bool customMap;

        //for input
        //yes i know this is a clumsy method
        //im changing it soon
        public static bool[] keyDown = new bool[11];
        public static Keys[] keyArray = new Keys[] { Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9 };

        //the amount of each resource available
        //this will be different between planing mode and level edit mode
        public static int[,] resources = new int[10, 5];

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

            for(int i = 0; i < 20; i++)
            {
                for(int j = 0; j < 20; j++)
                {
                    if (map[i, j] != 0)
                        presetMap[i, j] = true;
                }
            }

            //TEMPORARY!!!
            for(int i = 0; i < 9; i++)
            {
                resources[i, 0] = 5;
            }
        }

        public static void edit()
        {
            //change these numbers later to prevent overflow
            for (int i = 0; i <= 20; i++)
            {
                for (int j = 0; j <= 20; j++)
                {
                    //fill in the tiles
                    //Program.s.render(new Shape.Rectangle(32, 32), i * 32, j * 32, 1, Brushes.Gray);
                }
            }

            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    //these values are consistent with the PhysicsObject "type"s + 1 to account for empty space
                    switch (map[i, j])
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

            int blockX = -1;
            int blockY = -1;

            for (int i = 0; i < clickTiles.Count; i++)
            {
                //safety first!
                if (clickTiles[i].GetType() == typeof(Rect))
                {
                    Rect placeholder = (Rect)clickTiles[i];
                    int mouseX = Program.s.getMouseX();
                    int mouseY = Program.s.getMouseY();
                    if (placeholder.intersects(new Vector2i(mouseX, mouseY)))
                    {
                        int mapValue = 0;
                        bool presetValue = true;
                        if ((int)(placeholder.x / 32) < 20 && (int)(placeholder.y / 32) < 20)
                        {
                            mapValue = map[(int)placeholder.x / 32, (int)placeholder.y / 32];
                            presetValue = presetMap[(int)placeholder.x / 32, (int)placeholder.y / 32];
                            blockX = (int)placeholder.x / 32;
                            blockY = (int)placeholder.y / 32;
                        }
                        Brush idTextColor = Brushes.White;
                        if (presetValue)
                            idTextColor = Brushes.Red;
                        //identify tile
                        switch (mapValue)
                        {
                            case 0:
                                Program.s.render("Tile: Nothing", 0, 672, idTextColor);
                                break;
                            case 1:
                                Program.s.render("Tile: Objective Rock", 0, 672, idTextColor);
                                break;
                            case 2:
                                Program.s.render("Tile: Rock", 0, 672, idTextColor);
                                break;
                            case 3:
                                Program.s.render("Tile: Platform", 0, 672, idTextColor);
                                break;
                            case 4:
                                Program.s.render("Tile: Falling Platform", 0, 672, idTextColor);
                                break;
                            case 5:
                                Program.s.render("Tile: Lava", 0, 672, idTextColor);
                                break;
                            case 6:
                                Program.s.render("Tile: Left-facing Ramp", 0, 672, idTextColor);
                                break;
                            case 7:
                                Program.s.render("Tile: Right-facing Ramp", 0, 672, idTextColor);
                                break;
                            case 8:
                                Program.s.render("Tile: Goal", 0, 672, idTextColor);
                                break;
                        }
                        if (!presetValue)
                            Program.s.render(new Shape.Rectangle(32, 32), (int)placeholder.x, (int)placeholder.y, 3, Brushes.Orange);
                        else
                            Program.s.render(new Shape.Rectangle(32, 32), (int)placeholder.x, (int)placeholder.y, 3, Brushes.Red);
                    }
                }
            }

            //render the instructions
            if (customMap)
                Program.s.render("Welcome to the Level Editor!", 0, 692, Brushes.Orange);
            else
                Program.s.render("Welcome to the Planning Phase! Modify the level with your limitted resources to solve the puzzle", 0, 692, Brushes.Orange);
            Program.s.render("Hover over a square and press a button [1-9] to place/remove a block", 0, 712, Brushes.Orange);
            if (!customMap)
                Program.s.render("However, you cannot remove/overwrite preset blocks (outlined in red), try to work around them to solve the puzzle.", 0, 727, Brushes.Orange);
            if (!customMap)
                Program.s.render("Press [ENTER] to run the level. If you fail, you'll be brought back here", 0, 742, Brushes.Orange);
            else
                Program.s.render("Press [ENTER] to copy the level code to your clipboard. Press CTRL+V in a text editor and share it with friends!", 0, 742, Brushes.Orange);
            
            //render the resources list
            Program.s.render("Resources Available:", 672, 0, Brushes.Orange);
            Program.s.render("[1] Obj. Rock (x" + resources[0, 0].ToString() + ")", 672, 20, Brushes.White);
            Program.s.render("[2] Rock (x" + resources[1, 0].ToString() + ")", 672, 40, Brushes.White);
            Program.s.render("[3] Platform (x" + resources[2, 0].ToString() + ")", 672, 60, Brushes.White);
            Program.s.render("[4] Falling Plat. (x" + resources[3, 0].ToString() + ")", 672, 80, Brushes.White);
            Program.s.render("[5] Lava (x" + resources[4, 0].ToString() + ")", 672, 100, Brushes.White);
            Program.s.render("[6] Left Ramp (x" + resources[5, 0].ToString() + ")", 672, 120, Brushes.White);
            Program.s.render("[7] Right Ramp (x" + resources[6, 0].ToString() + ")", 672, 140, Brushes.White);
            Program.s.render("[8] Goal (x" + resources[7, 0].ToString() + ")", 672, 160, Brushes.White);
            Program.s.render("[9] Delete (∞)", 672, 180, Brushes.White);

            //detect input
            //maybe move this up later
            for(int i = 0; i< 9; i++)
            {
                if (Program.s.readKeyDown(keyArray[i]))
                    keyDown[i] = true;
            }
            
            if (Program.s.readKeyDown(Keys.Enter))
            {
                //submit the level to Gameplay
                Gameplay.physicsObjects.Clear();
                Gameplay.initGameplay(map);
                Program.currentScreen = 1;
            }

            if (Program.s.keyUp())
            {
                for (int i = 0; i < 11; i++)
                {
                    keyDown[i] = false;
                }
            }

            for (int i = 0; i < 10; i++)
            {
                if (keyDown[i])
                {
                    if (i < 9)
                    {
                        if (blockX != -1 && blockY != -1)
                        {
                            if (resources[i, 0] > 0 && presetMap[blockX, blockY] != true)
                            {
                                //place the thing
                                bool alreadyPlaced = false;
                                if (map[blockX, blockY] == i)
                                    alreadyPlaced = true;
                                map[blockX, blockY] = i;
                                if (!customMap && !alreadyPlaced)
                                    resources[i, 0]--;
                            }
                        }
                    }
                }
            }
            
        }

    }
}
