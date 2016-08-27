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
                    break;

                case 1:
                    //also rock
                    this.weight = 5;
                    break;

                case 2:
                    //platform (can't move)
                    this.weight = 0; //(never falls)
                    break;

                case 3:
                    //falling platform (moves sometimes)
                    this.weight = 0;
                    break;

                case 4:
                    //lava
                    this.weight = 3;
                    break;           

            }
            objectRect = new Rect(this.x, this.y, this.width, this.height);
        }

        public void update()
        {
            //check collisions
            //but only for certain physicsobjects
            //THE LAVA DOES NOT DETECT COLLISIONS WITH ROCKS
            //the lava detects collisions with platforms, the rocks detect collisions with lava
            if (type == 0 || type == 1 || type == 3 || type == 4)
            {
                for (int i = 0; i < Program.physicsObjects.Count; i++)
                {
                    //check the type
                    //mostly for safety purposes
                    if (Program.physicsObjects[i].GetType() == typeof(PhysicsObject))
                    {
                        PhysicsObject placeholder = (PhysicsObject)Program.physicsObjects[i];
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
                                        break;

                                    case 1:
                                        //its another rock
                                        break;

                                    case 2:
                                        //its a platform
                                        Console.WriteLine("platform");
                                        shouldFall = false;
                                        y--;
                                        if(type == 3)
                                        {
                                            //falling platform
                                            shouldDestroy = true;
                                        }
                                        break;

                                    case 4:
                                        //its lava
                                        if(type == 0 || type == 1 || type == 3)
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
                        weight = 8;
                        break;
                }
            }
            if(shouldFall)
                y += weight;
            if(y > 640)
                shouldDestroy = true;

            objectRect.x = x;
            objectRect.y = y;
        }

    }
}
