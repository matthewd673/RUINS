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
    class Program
    {

        public static Window w;
        public static Scene s;
        public static Game g;

        //keep track of what screen we're on
        public static int currentScreen = 2; //0 = menu, 1 = game, 2 = editor

        //graphics
        public static Prop objectiveRock;
        public static Prop rock;
        public static Prop platform;
        public static Prop fallingPlatform;
        public static Prop lava;
        public static Prop fallingLava;

        //PhysicsObjects
        public static PhysicsObject testRock;
        public static PhysicsObject testPlatform;
        public static PhysicsObject testFallingPlatform;
        public static PhysicsObject testLava;

        public static ArrayList physicsObjects = new ArrayList();
        public static ArrayList eventLog = new ArrayList();

        static void Main(string[] args)
        {
            w = new Window(640, 640, "RUINS - Ludum Dare 36 entry");
            s = new Scene(w);
            g = new Game(w, s, new Action(update));

            //initialize graphics
            string prefix = Environment.CurrentDirectory + @"\res\";
            objectiveRock = new Prop(new Bitmap(prefix + "objective-rock.png"), 32, 32);
            rock = new Prop(new Bitmap(prefix + "rock.png"), 32, 32);
            platform = new Prop(new Bitmap(prefix + "platform.png"), 32, 32);
            fallingPlatform = new Prop(new Bitmap(prefix + "falling-platform.png"), 32, 32);
            lava = new Prop(new Bitmap(prefix + "lava.png"), 32, 32);
            fallingLava = new Prop(new Bitmap(prefix + "falling-lava.png"), 32, 32);

            //temporary code
            testRock = new PhysicsObject(0, 0, 32, 32, 1);
            //physicsObjects.Add(testRock);
            addEvent("Objective Rock spawned!");

            testPlatform = new PhysicsObject(0, 200, 32, 32, 2);
            physicsObjects.Add(testPlatform);
            addEvent("Platform built");

            testFallingPlatform = new PhysicsObject(82, 50, 32, 32, 3);
            testFallingPlatform.triggered = true;
            physicsObjects.Add(testFallingPlatform);
            addEvent("Falling platform built");

            testLava = new PhysicsObject(0, 0, 32, 32, 3);
            physicsObjects.Add(testLava);
            addEvent("Lava poured");

            LevelEditor.initMap();

            g.runGame();
        }

        public static void update()
        {
            if (currentScreen == 0)
                MainMenu.menu();

            if (currentScreen == 1)
                Gameplay.play();

            if (currentScreen == 2)
                LevelEditor.edit();
        }

        public struct gameEvent
        {
            public int weight;
            public string eventString;
            
            public gameEvent(string eventString)
            {
                weight = 0;
                this.eventString = eventString;
            }
        }

        public static void addEvent(string eventString)
        {
            gameEvent ge = new gameEvent(eventString);
            eventLog.Add(ge);
            for (int i = 0; i < eventLog.Count; i++)
            {
                //safety
                if (eventLog[i].GetType() == typeof(gameEvent))
                {
                    gameEvent placeholder = (gameEvent)eventLog[i];
                    //move it down a notch
                    if (placeholder.weight < 10)
                    {
                        placeholder.weight++;
                        //save changes
                        eventLog[i] = placeholder;
                    }
                    else
                    {
                        //remove it
                        eventLog.Remove(eventLog[i]);
                    }
                }
            }
        }
    }
}
