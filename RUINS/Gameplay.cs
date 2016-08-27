using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using neon2d;

namespace RUINS
{
    public class Gameplay
    {
        
        public static int[,] currentMap = new int[20, 20];
        public static ArrayList physicsObjects = new ArrayList();

        public static void initGameplay(int[,] map)
        {
            currentMap = map;

            for(int i = 0; i < 20; i++)
            {
                for(int j = 0; j < 20; j++)
                {
                    switch(map[i, j])
                    {
                        case 1:
                            //objective rock
                            PhysicsObject tempObjectiveRock = new PhysicsObject(i * 32, j * 32, 32, 32, 0);
                            physicsObjects.Add(tempObjectiveRock);
                            break;

                        case 2:
                            //rock
                            PhysicsObject tempRock = new PhysicsObject(i * 32, j * 32, 32, 32, 1);
                            physicsObjects.Add(tempRock);
                            break;

                        case 3:
                            //platform
                            PhysicsObject tempPlatform = new PhysicsObject(i * 32, j * 32, 32, 32, 2);
                            physicsObjects.Add(tempPlatform);
                            break;

                        case 4:
                            //falling platform
                            PhysicsObject tempFallingPlatform = new PhysicsObject(i * 32, j * 32, 32, 32, 3);
                            physicsObjects.Add(tempFallingPlatform);
                            break;

                        case 5:
                            //lava
                            PhysicsObject tempLava = new PhysicsObject(i * 32, j * 32, 32, 32, 4);
                            physicsObjects.Add(tempLava);
                            break;

                        case 6:
                            //left ramp
                            PhysicsObject tempLeftRamp = new PhysicsObject(i * 32, j * 32, 32, 32, 5);
                            physicsObjects.Add(tempLeftRamp);
                            break;

                        case 7:
                            //right ramp
                            PhysicsObject tempRightRamp = new PhysicsObject(i * 32, j * 32, 32, 32, 6);
                            physicsObjects.Add(tempRightRamp);
                            break;

                        case 8:
                            //goal
                            PhysicsObject tempGoal = new PhysicsObject(i * 32, j * 32, 32, 32, 7);
                            physicsObjects.Add(tempGoal);
                            break;
                    }
                }
            }
        }

        public static void play()
        {
            simulate();
            for (int i = 0; i < Program.eventLog.Count; i++)
            {
                //pretty much the same as with PhysicsObjects
                if (Program.eventLog[i].GetType() == typeof(Program.gameEvent))
                {
                    Program.gameEvent placeholder = (Program.gameEvent)Program.eventLog[i];
                    Program.s.render(placeholder.eventString, 0, i * 12, Brushes.White);
                }
            }
        }

        public static void simulate()
        {
            for (int i = 0; i < physicsObjects.Count; i++)
            {
                //just a saftey measure
                if (physicsObjects[i].GetType() == typeof(PhysicsObject))
                {
                    //get the object into a useable state
                    PhysicsObject placeholder = (PhysicsObject)physicsObjects[i];

                    //modify the object
                    placeholder.update();
                    if (placeholder.shouldFall)
                        placeholder.additionalWeight += 0.1;
                    else
                        placeholder.additionalWeight = 0;
                    //gotta cap it somewhere
                    if (placeholder.additionalWeight > placeholder.weight + 3)
                        placeholder.additionalWeight = placeholder.weight + 3;

                    //check for destruction, etc.
                    if (placeholder.shouldDestroy)
                    {
                        physicsObjects.Remove(physicsObjects[i]);
                        Program.addEvent("Object destroyed");
                    }
                    
                    //render PhysicsObjects
                    switch (placeholder.type)
                    {
                        case 0:
                            //objective rock
                            Program.s.render(Program.objectiveRock, placeholder.x, placeholder.y);
                            break;

                        case 1:
                            //rock
                            Program.s.render(Program.rock, placeholder.x, placeholder.y);
                            break;

                        case 2:
                            //platform
                            Program.s.render(Program.platform, placeholder.x, placeholder.y);
                            break;

                        case 3:
                            //falling platform
                            Program.s.render(Program.fallingPlatform, placeholder.x, placeholder.y);
                            break;

                        case 4:
                            //lava
                            //really just a visual effect here
                            if (placeholder.shouldFall)
                                Program.s.render(Program.fallingLava, placeholder.x, placeholder.y);
                            else
                                Program.s.render(Program.lava, placeholder.x, placeholder.y);
                            break;

                        case 5:
                            //left ramp
                            Program.s.render(Program.leftRamp, placeholder.x, placeholder.y);
                            break;

                        case 6:
                            //right ramp
                            Program.s.render(Program.rightRamp, placeholder.x, placeholder.y);
                            break;

                        case 7:
                            //goal
                            Program.s.render(Program.goal, placeholder.x, placeholder.y);
                            break;
                    }

                    //save back our changes (if not destroyed)
                    if (!placeholder.shouldDestroy)
                        physicsObjects[i] = placeholder;
                }
            }
        }

    }
}
