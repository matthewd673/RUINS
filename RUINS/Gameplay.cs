using System;
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

        public static void play()
        {
            for (int i = 0; i < Program.physicsObjects.Count; i++)
            {
                //just a saftey measure
                if (Program.physicsObjects[i].GetType() == typeof(PhysicsObject))
                {
                    //get the object into a useable state
                    PhysicsObject placeholder = (PhysicsObject)Program.physicsObjects[i];

                    //modify the object
                    placeholder.update();

                    //check for destruction, etc.
                    if (placeholder.shouldDestroy)
                    {
                        Program.physicsObjects.Remove(Program.physicsObjects[i]);
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
                    }

                    //save back our changes (if not destroyed)
                    if (!placeholder.shouldDestroy)
                        Program.physicsObjects[i] = placeholder;
                }
            }

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

    }
}
