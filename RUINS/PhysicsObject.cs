using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using neon2d;
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

        public bool canFall = true;
        public bool canRoll = true;

        //for more realistic gravity
        public double additionalWeight = 0;

        //create a rect for each object
        public Rect objectRect;

        public bool victory = false;

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
                    canFall = false;
                    canRoll = false;
                    maxRollAmount = 0; //just for safety
                    break;

                case 3:
                    //falling platform (moves sometimes)
                    this.weight = 8;
                    canFall = false;
                    canRoll = false;
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
                    canFall = false;
                    canRoll = false;
                    maxRollAmount = 0;
                    break;

                case 6:
                    //right ramp
                    this.weight = 0;
                    canFall = false;
                    canRoll = false;
                    maxRollAmount = 0;
                    break;

                case 7:
                    this.weight = 0;
                    canFall = false;
                    canRoll = false;
                    maxRollAmount = 0;
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

            //NEW AND IMPROVED PHYSICS:
            //this is very much a work in progress
            if(shouldFall && canFall)
            {
                for(int i = 0; i <= (weight + (int)additionalWeight); i++)
                {
                    Rect future = new Rect(x, y + i, 32, 32);
                    //Program.s.render(new Shape.Rectangle(32, 32, false), (int)future.x, (int)future.y, 1, Brushes.Yellow);
                    bool safeMove = true;
                    for(int j = 0; j < Gameplay.physicsObjects.Count; j++)
                    {
                        PhysicsObject placeholder = (PhysicsObject)Gameplay.physicsObjects[j];
                        if(future.intersects(placeholder.objectRect) && placeholder.objectRect != objectRect)
                        {
                            safeMove = false;
                            switch (placeholder.type)
                            {
                                case 3:
                                    placeholder.triggered = true;
                                    additionalWeight = 0;
                                    shouldFall = true;
                                    safeMove = true;
                                    break;
                                case 4:
                                    shouldDestroy = true;
                                    break;
                                case 5:
                                    shouldRoll = true;
                                    rollAmount = 0;
                                    rollDirection = false;
                                    x += 16;
                                    safeMove = true;
                                    break;

                                case 6:
                                    shouldRoll = true;
                                    rollAmount = 0;
                                    rollDirection = true;
                                    x += 16;
                                    safeMove = true;
                                    break;

                                case 7:
                                    if(type == 0)
                                        victory = true;
                                    break;
                            }
                            
                        }
                    }
                    if(safeMove)
                    {
                        y++;
                    }
                    else
                    {
                        y = (int)future.y - 1;
                        shouldFall = false;
                        additionalWeight = 0;
                        break;
                    }
                }
            }

            if (shouldRoll && canRoll)
            {
                for (int i = 0; i <= (13 - weight); i++)
                {
                    if (!rollDirection)
                    {
                        //going left
                        Rect future = new Rect(x - 1, y, 32, 32);
                        //Program.s.render(new Shape.Rectangle(32, 32, false), (int)future.x, (int)future.y, 1, Brushes.Blue);
                        bool safeMove = true;
                        for (int j = 0; j < Gameplay.physicsObjects.Count; j++)
                        {
                            PhysicsObject placeholder = (PhysicsObject)Gameplay.physicsObjects[j];
                            if (future.intersects(placeholder.objectRect) && placeholder.objectRect != objectRect)
                            {
                                safeMove = false;
                                switch(placeholder.type)
                                {
                                    case 4:
                                        shouldDestroy = true;
                                        break;
                                    case 5:
                                        if(shouldFall)
                                            safeMove = true;
                                        rollAmount = 0;
                                        break;

                                    case 6:
                                        if(shouldFall)
                                            safeMove = true;
                                        rollAmount = 0;
                                        break;

                                    case 7:
                                        if(type == 0)
                                            victory = true;
                                        break;
                                        
                                }
                            }
                        }
                        if (safeMove)
                        {
                            x--;
                        }
                        else
                        {
                            x = (int)future.x + 1;
                            shouldRoll = false;
                        }
                    }
                    else
                    {
                        //going right
                        Rect future = new Rect(x + 1, y, 32, 32);
                        //Program.s.render(new Shape.Rectangle(32, 32, false), (int)future.x, (int)future.y, 1, Brushes.Blue);
                        bool safeMove = true;
                        for (int j = 0; j < Gameplay.physicsObjects.Count; j++)
                        {
                            PhysicsObject placeholder = (PhysicsObject)Gameplay.physicsObjects[j];
                            if (future.intersects(placeholder.objectRect) && placeholder.objectRect != objectRect)
                            {
                                Console.WriteLine(type + "->" + placeholder.type);
                                safeMove = false;
                            }
                        }
                        if (safeMove)
                        {
                            x++;
                        }
                        else
                        {
                            x = (int)future.x - 1;
                            shouldRoll = false;
                        }
                    }
                }
            }

            if (triggered)
            {
                switch(type)
                {
                    case 3:
                        canFall = true;
                        shouldFall = true;
                        y += 8;
                        break;
                }
            }
            if(y > 640)
                shouldDestroy = true;

            objectRect.x = x;
            objectRect.y = y;

            if (victory == true)
                Program.currentScreen = 3;
        }

    }
}
