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
        public static int currentScreen = 2; //0 = menu, 1 = game, 2 = editor, 3 = results screen

        //graphics
        //NOTE: SPRING IS TEMPORARY
        public static Prop objectiveRock;
        public static Prop rock;
        public static Prop platform;
        public static Prop fallingPlatform;
        public static Prop lava;
        public static Prop fallingLava;
        public static Prop leftRamp;
        public static Prop rightRamp;
        public static Prop spring;
        public static Prop goal;

        public static int keyPressCooldown;
        public static int cooldownLength = 30;

        static void Main(string[] args)
        {
            w = new Window(800, 800, "RUINS - Ludum Dare 36 entry");
            w.gamewindow.MaximizeBox = false;
            s = new Scene(w);
            g = new Game(w, s, new Action(update), 30);

            //initialize graphics
            string prefix = Environment.CurrentDirectory + @"\res\";
            objectiveRock = new Prop(new Bitmap(prefix + "objective-rock.png"), 32, 32);
            rock = new Prop(new Bitmap(prefix + "rock.png"), 32, 32);
            platform = new Prop(new Bitmap(prefix + "platform.png"), 32, 32);
            fallingPlatform = new Prop(new Bitmap(prefix + "falling-platform.png"), 32, 32);
            lava = new Prop(new Bitmap(prefix + "lava.png"), 32, 32);
            fallingLava = new Prop(new Bitmap(prefix + "falling-lava.png"), 32, 32);
            leftRamp = new Prop(new Bitmap(prefix + "ramp-left.png"), 32, 32);
            rightRamp = new Prop(new Bitmap(prefix + "ramp-right.png"), 32, 32);
            spring = new Prop(new Bitmap(prefix + "spring.png"), 32, 32);
            goal = new Prop(new Bitmap(prefix + "goal.png"), 32, 32);

            //MainMenu.initMenu();
            LevelEditor.initMap("level1");

            g.runGame();
        }

        public static void update()
        {
            keyPressCooldown--;
            if (keyPressCooldown < 0)
                keyPressCooldown = 0;
            if (currentScreen == 0)
                MainMenu.menu();

            if (currentScreen == 1)
                Gameplay.play();

            if (currentScreen == 2)
                LevelEditor.edit();

            if (currentScreen == 3)
                Results.displayResults();
        }
    }
}
