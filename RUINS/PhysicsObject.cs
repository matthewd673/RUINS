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

        public int lastX = 0;
        public int lastY = 0;

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

            lastX = x;
            lastY = y;

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

            /*
            if (fallCheckTime > 9)
            {
                shouldFall = true;
                fallCheckTime = 0;
            }
            */

            //reactive collision detection
            for(int i = 0; i < Gameplay.physicsObjects.Count; i++)
            {
                PhysicsObject placeholder = (PhysicsObject)Gameplay.physicsObjects[i];
                if(objectRect.intersects(placeholder.objectRect) && placeholder.objectRect != objectRect)
                {
                    //x = lastX;
                    //y = lastY;
                    //Console.WriteLine("moved back!");
                }
            }

            /*
             * basic physics checking
             */
            bool didCollide = false;
            for (int i = 0; i < Gameplay.physicsObjects.Count; i++)
            {
                PhysicsObject placeholder = (PhysicsObject)Gameplay.physicsObjects[i];
                if(objectRect.intersects(placeholder.objectRect) && placeholder.objectRect != objectRect)
                {
                    didCollide = true;

                    switch (placeholder.type)
                    {

                        case 0:
                            if (type == 4)
                                placeholder.shouldDestroy = true;
                            if (type == 3)
                                shouldDestroy = true;
                            break;

                        case 1:
                            if (type == 4)
                                placeholder.shouldDestroy = true;
                            if (type == 3)
                                shouldDestroy = true;
                            break;

                        case 2:
                            if (type == 3)
                                shouldDestroy = true;
                            shouldFall = false;
                            break;

                        case 3:
                            if (type == 0 || type == 1)
                            {
                                placeholder.triggered = true;
                                shouldFall = true;
                            }
                            additionalWeight = 0;

                            break;
                        case 4:
                            if(type == 0 || type == 1 || type == 3)
                                shouldDestroy = true;
                            break;
                        case 5:
                            if (type == 3)
                                shouldDestroy = true;
                            shouldRoll = true;
                            rollAmount = 0;
                            rollDirection = false;
                            x += 16;
                            break;

                        case 6:
                            if (type == 3)
                                shouldDestroy = true;
                            shouldRoll = true;
                            rollAmount = 0;
                            rollDirection = true;
                            x += 16;
                            break;

                        case 7:
                            if (type == 0)
                                victory = true;
                            else if (type != 0)
                                shouldDestroy = true;
                            break;
                    }

                }
            }
            if (!didCollide && canFall)
                shouldFall = true;

            if (shouldFall && canFall)
            {
                for(int i = 0; i <= (weight + (int)additionalWeight); i++)
                {
                    Rect future = new Rect(x, y++, 32, 32);
                    Program.s.render(new Shape.Rectangle(32, 32, false), (int)future.x, (int)future.y, 1, Brushes.Yellow);
                    bool safeMove = true;
                    for(int j = 0; j < Gameplay.physicsObjects.Count; j++)
                    {
                        PhysicsObject placeholder = (PhysicsObject)Gameplay.physicsObjects[j];
                        if(future.intersects(placeholder.objectRect) && placeholder.objectRect != objectRect)
                        {
                            switch (placeholder.type)
                            {

                                case 0:
                                    if (type == 4)
                                        placeholder.shouldDestroy = true;
                                    if (type == 3)
                                        shouldDestroy = true;
                                    shouldFall = false;
                                    y = placeholder.y - 33;
                                    break;

                                case 1:
                                    if (type == 4)
                                        placeholder.shouldDestroy = true;
                                    if (type == 3)
                                        shouldDestroy = true;
                                    shouldFall = false;
                                    y = placeholder.y - 33;
                                    break;

                                case 2:
                                    if (type == 3)
                                        shouldDestroy = true;
                                    shouldFall = false;
                                    y = placeholder.y - 33;
                                    break;

                                case 3:
                                    if (type == 0 || type == 1)
                                    {
                                        placeholder.triggered = true;
                                        shouldFall = true;
                                    }
                                    additionalWeight = 0;

                                    break;
                                case 4:
                                    if (type == 0 || type == 1 || type == 3)
                                        shouldDestroy = true;
                                    break;
                                case 5:
                                    if (type == 3)
                                        shouldDestroy = true;
                                    shouldRoll = true;
                                    rollAmount = 0;
                                    rollDirection = false;
                                    x += 16;
                                    break;

                                case 6:
                                    if (type == 3)
                                        shouldDestroy = true;
                                    shouldRoll = true;
                                    rollAmount = 0;
                                    rollDirection = true;
                                    x += 16;
                                    break;

                                case 7:
                                    if (type == 0)
                                        victory = true;
                                    else if (type != 0)
                                        shouldDestroy = true;
                                    break;
                            }

                            if (placeholder.shouldFall)
                                shouldFall = true;

                        }
                    }
                    if(safeMove)
                    {
                        lastY = y;
                        y++;
                    }
                    else
                    {
                        lastY = y;
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
                        Rect future = new Rect(x--, y, 32, 32);
                        Program.s.render(new Shape.Rectangle(32, 32, false), (int)future.x, (int)future.y, 1, Brushes.Blue);
                        bool safeMove = true;
                        for (int j = 0; j < Gameplay.physicsObjects.Count; j++)
                        {
                            PhysicsObject placeholder = (PhysicsObject)Gameplay.physicsObjects[j];
                            if (future.intersects(placeholder.objectRect) && placeholder.objectRect != objectRect)
                            {
                                switch (placeholder.type)
                                {

                                    case 0:
                                        if (type == 4)
                                            placeholder.shouldDestroy = true;
                                        if (type == 3)
                                            shouldDestroy = true;
                                        shouldRoll = false;
                                        x = placeholder.x + 33;
                                        break;

                                    case 1:
                                        if (type == 4)
                                            placeholder.shouldDestroy = true;
                                        if (type == 3)
                                            shouldDestroy = true;
                                        shouldRoll = false;
                                        x = placeholder.x + 33;
                                        break;

                                    case 2:
                                        if (type == 3)
                                            shouldDestroy = true;
                                        shouldRoll = false;
                                        x = placeholder.x + 33;
                                        break;

                                    case 3:
                                        if (type == 0 || type == 1)
                                        {
                                            placeholder.triggered = true;
                                            shouldFall = true;
                                        }
                                        additionalWeight = 0;
                                        x = placeholder.x + 33;
                                        break;

                                    case 4:
                                        if (type == 0 || type == 1 || type == 3)
                                            shouldDestroy = true;
                                        break;

                                    case 5:
                                        if (type == 3)
                                            shouldDestroy = true;
                                        shouldRoll = true;
                                        rollAmount = 0;
                                        rollDirection = false;
                                        x += 16;
                                        break;

                                    case 6:
                                        if (type == 3)
                                            shouldDestroy = true;
                                        shouldRoll = true;
                                        rollAmount = 0;
                                        rollDirection = true;
                                        x += 16;
                                        break;

                                    case 7:
                                        if (type == 0)
                                            victory = true;
                                        else if (type != 0)
                                            shouldDestroy = true;
                                        break;
                                }
                            }
                        }
                        if (safeMove)
                        {
                            lastX = x;
                            x--;
                        }
                        else
                        {
                            lastX = x;
                            x = (int)future.x + 1;
                            shouldRoll = false;
                        }
                    }
                    else
                    {
                        //going right
                        Rect future = new Rect(x + i, y, 32, 32);
                        Program.s.render(new Shape.Rectangle(32, 32, false), (int)future.x, (int)future.y, 1, Brushes.Blue);
                        bool safeMove = true;
                        for (int j = 0; j < Gameplay.physicsObjects.Count; j++)
                        {
                            PhysicsObject placeholder = (PhysicsObject)Gameplay.physicsObjects[j];
                            if (future.intersects(placeholder.objectRect) && placeholder.objectRect != objectRect)
                            {
                                Console.WriteLine(type + "->" + placeholder.type);
                                switch (placeholder.type)
                                {

                                    case 0:
                                        if (type == 4)
                                            placeholder.shouldDestroy = true;
                                        if (type == 3)
                                            shouldDestroy = true;
                                        shouldRoll = false;
                                        x = placeholder.x - 33;
                                        break;

                                    case 1:
                                        if (type == 4)
                                            placeholder.shouldDestroy = true;
                                        if (type == 3)
                                            shouldDestroy = true;
                                        shouldFall = false;
                                        x = placeholder.x - 33;
                                        break;

                                    case 2:
                                        if (type == 3)
                                            shouldDestroy = true;
                                        shouldRoll = false;
                                        x = placeholder.x - 33;
                                        break;

                                    case 3:
                                        if (type == 0 || type == 1)
                                        {
                                            placeholder.triggered = true;
                                            shouldFall = true;
                                        }
                                        additionalWeight = 0;
                                        shouldRoll = false;
                                        x = placeholder.x - 33;
                                        break;

                                    case 4:
                                        if (type == 0 || type == 1 || type == 3)
                                            shouldDestroy = true;
                                        break;

                                    case 5:
                                        if (type == 3)
                                            shouldDestroy = true;
                                        shouldRoll = true;
                                        rollAmount = 0;
                                        rollDirection = false;
                                        x += 16;
                                        break;

                                    case 6:
                                        if (type == 3)
                                            shouldDestroy = true;
                                        shouldRoll = true;
                                        rollAmount = 0;
                                        rollDirection = true;
                                        x += 16;
                                        break;

                                    case 7:
                                        if (type == 0)
                                            victory = true;
                                        else if (type != 0)
                                            shouldDestroy = true;
                                        break;
                                }
                            }
                        }
                        if (safeMove)
                        {
                            lastX = x;
                            x++;
                        }
                        else
                        {
                            lastX = x;
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
