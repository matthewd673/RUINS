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

            }
            objectRect = new Rect(this.x, this.y, this.width, this.height);
        }

        public void update()
        {
            //this is SUPER primitive
            if(triggered)
            {
                switch(type)
                {
                    case 3:
                        weight = 5;
                        break;
                }
            }
            y += weight;
            if(y > 800)
                shouldDestroy = true;
        }

    }
}
