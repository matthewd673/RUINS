using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using neon2d.Physics;

namespace RUINS
{
    public class PhysicsObject
    {

        public int x;
        public int y;
        public int width;
        public int height;
        public int weight;
        public int type;

        public bool shouldDestroy;
        public bool triggered; //this is only used for certain special objects

        public bool shouldFall = true;

        public bool shouldRoll = false; //for use with the ramps
        public bool rollDirection; //false = left, true = right
        public int rollAmount; //how much its rolled
        public int maxRollAmount = 32; //how far it can roll on one push

        //for more realistic gravity
        public double additionalWeight = 0;

        //create a rect for each object
        public Rect objectRect;

        //TYPES:
        //0 = objective rock
        //1 = rock
        //2 = platform (stationary)

        public PhysicsObject(int x, int y, int width, int height, int type)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.type = type;

            //weight is determined based off type
            switch(this.type)
            {
                case 0:
                    //rock
                    this.weight = 5;
                    maxRollAmount = 128;
                    break;

                case 1:
                    //also rock
                    this.weight = 5;
                    maxRollAmount = 128;
                    break;

                case 2:
                    //platform (can't move)
                    this.weight = 0; //(never falls)
                    shouldFall = false;
                    maxRollAmount = 0; //just for safety
                    break;

                case 3:
                    //falling platform (moves sometimes)
                    this.weight = 8;
                    shouldFall = false;
                    maxRollAmount = 0;
                    break;

                case 4:
                    //lava
                    this.weight = 3;
                    maxRollAmount = 32; //1 square wide
                    break;

                case 5:
                    //left ramp
                    this.weight = 0;
                    shouldFall = false;
                    maxRollAmount = 0;
                    break;

                case 6:
                    //right ramp
                    this.weight = 0;
                    shouldFall = false;
                    maxRollAmount = 0;
                    break;

                case 7:
                    this.weight = 0;
                    shouldFall = false;
                    maxRollAmount = 0;
                    break;

            }
            objectRect = new Rect(this.x, this.y, this.width, this.height);
        }

        public void update()
        {
            /*NOTE:
             * These physics are mediocre at best,
             * sloppy and fairly bad at worst.
             * Please don't use this in your own projects.
             */
            //check collisions
            //but only for certain physicsobjects
            //THE LAVA DOES NOT DETECT COLLISIONS WITH ROCKS
            //the lava detects collisions with platforms, the rocks detect collisions with lava
            if (type == 0 || type == 1 || type == 3 || type == 4)
            {
                for (int i = 0; i < Gameplay.physicsObjects.Count; i++)
                {
                    //check the type
                    //mostly for safety purposes
                    if (Gameplay.physicsObjects[i].GetType() == typeof(PhysicsObject))
                    {
                        PhysicsObject placeholder = (PhysicsObject)Gameplay.physicsObjects[i];
                        if (placeholder.objectRect != objectRect)
                        {
                            if (objectRect.intersects(placeholder.objectRect))
                            {
                                Console.WriteLine("collision!");
                                //they have collided!
                                switch (placeholder.type)
                                {
                                    case 0:
                                        //its the objective rock
                                        shouldFall = false;
                                        /*
                                        if(type == 3)
                                        {
                                            shouldDestroy = true;
                                        }
                                        */
                                        if(shouldRoll)
                                        {
                                            shouldRoll = false;
                                            rollAmount = 0;
                                            if(!rollDirection)
                                            {
                                                //rolling left
                                                x = placeholder.x + 32;
                                            }
                                            else
                                            {
                                                //rolling right
                                                x = placeholder.x - 32;
                                            }
                                        }
                                        break;

                                    case 1:
                                        //its another rock
                                        shouldFall = false;
                                        if (shouldRoll)
                                        {
                                            shouldRoll = false;
                                            rollAmount = 0;
                                            if (!rollDirection)
                                            {
                                                //rolling left
                                                x = placeholder.x + 32;
                                            }
                                            else
                                            {
                                                //rolling right
                                                x = placeholder.x - 32;
                                            }
                                        }
                                        break;

                                    case 2:
                                        //Good enough
                                        shouldFall = false;
                                        if (placeholder.y - y < 8)
                                        {
                                            shouldRoll = false;
                                            if (!rollDirection)
                                            {
                                                //rolling left
                                                x = placeholder.x + 32;
                                            }
                                            else
                                            {
                                                x = placeholder.x - 32;
                                            }
                                        }
                                        if (Gameplay.currentMap[(x / 32) + 1, (y / 32)] == 0)
                                        {
                                            if(x > placeholder.x + 31)
                                            {
                                                shouldFall = true;
                                            }
                                        }
                                        if(Gameplay.currentMap[(x / 32) - 1, (y / 32)] == 0)
                                        {
                                            if(x < placeholder.x - 3)
                                            {
                                                shouldFall = true;
                                            }
                                        }

                                        if(!shouldFall)
                                        {
                                            y = placeholder.y - 32;
                                        }

                                        if(type == 3)
                                        {
                                            //falling platform
                                            shouldDestroy = true;
                                        }
                                        
                                        break;

                                    case 3:
                                        //its a falling platform
                                        placeholder.triggered = true;
                                        if (placeholder.y - y < 8)
                                        {
                                            shouldRoll = false;
                                            if (!rollDirection)
                                            {
                                                //rolling left
                                                x = placeholder.x + 32;
                                            }
                                            else
                                            {
                                                x = placeholder.x - 32;
                                            }
                                        }
                                        break;

                                    case 4:
                                        //its lava
                                        if(type == 0 || type == 1 || type == 3)
                                        {
                                            shouldDestroy = true;
                                        }
                                        break;

                                    case 5:
                                        //its a left ramp
                                        if(type == 0 || type == 1)
                                        {
                                            if (shouldFall)
                                            {
                                                rollAmount = 0;
                                                shouldRoll = true;
                                                rollDirection = false;
                                            }
                                        }
                                        if(type == 3)
                                        {
                                            shouldDestroy = true;
                                        }
                                        if(type == 4)
                                        {
                                            //push it to the block to the left
                                            if (shouldFall)
                                            {
                                                rollAmount = 0;
                                                shouldRoll = true;
                                                rollDirection = false;
                                            }
                                        }
                                        break;

                                    case 6:
                                        //its a right ramp
                                        if(type == 0 || type == 1)
                                        {
                                            if (shouldFall)
                                            {
                                                rollAmount = 0;
                                                shouldRoll = true;
                                                rollDirection = true;
                                            }
                                        }
                                        if(type == 3)
                                        {
                                            shouldDestroy = true;
                                        }
                                        if(type == 4)
                                        {
                                            //push to the right
                                            if (shouldFall)
                                            {
                                                rollAmount = 0;
                                                shouldRoll = true;
                                                rollDirection = true;
                                            }
                                        }
                                        break;

                                    case 7:
                                        //its the goal!
                                        if (type == 0)
                                        {
                                            //you win!
                                            Program.s.render("YOU WIN", 100, 100, System.Drawing.Brushes.Yellow);
                                            Console.WriteLine("VICTORY!!!!!");
                                            shouldFall = false;
                                        }
                                        else
                                        {
                                            shouldDestroy = true;
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            //this is SUPER primitive
            if(triggered)
            {
                switch(type)
                {
                    case 3:
                        shouldFall = true;
                        break;
                }
            }
            if(shouldFall)
                y += (weight + (int)additionalWeight);
            if(y > 640)
                shouldDestroy = true;

            if(shouldRoll)
            {
                if(!rollDirection)
                {
                    //roll left
                    if(rollAmount < maxRollAmount)
                    {
                        rollAmount += 13 - weight;
                        x -= (13 - weight);
                    }
                    else
                    {
                        shouldRoll = false;
                    }
                }
                else
                {
                    //roll right
                    if(rollAmount < maxRollAmount)
                    {
                        rollAmount += 13 - weight;
                        x += (13 - weight);
                    }
                    else
                    {
                        shouldRoll = false;
                    }
                }
            }

            objectRect.x = x;
            objectRect.y = y;
        }

    }
}
