﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using neon2d;

namespace RUINS
{
    public class MainMenu
    {

        public static Prop logo;
        public static Prop levelEditorButton;
        public static Prop customLevelButton;
        public static Prop leftButton;
        public static Prop rightButton;
        public static Prop defaultCard;
        public static Prop level1Card;
        public static Prop level2Card;
        public static Prop level3Card;
        public static Prop level4Card;
        public static Prop level5Card;

        //yes its sloppy
        //no I'm not using an array again
        public static bool leftDown;
        public static bool rightDown;
        public static bool enterDown;
        public static bool oneDown;
        public static bool twoDown;

        public static int currentCard = 0;
        public static int maxCards = 5;

        public static void initMenu()
        {
            //the graphics
            string prefix = Environment.CurrentDirectory + @"\res\";
            logo = new Prop(new Bitmap(prefix + "logo.png"));
            levelEditorButton = new Prop(new Bitmap(prefix + "level-editor-button.png"), 350, 80);
            customLevelButton = new Prop(new Bitmap(prefix + "custom-level-button.png"), 350, 80);
            leftButton = new Prop(new Bitmap(prefix + "left-arrows.png"), 64, 64);
            rightButton = new Prop(new Bitmap(prefix + "right-arrows.png"), 64, 64);
            defaultCard = new Prop(new Bitmap(prefix + "default-card.png"), 200, 400);
            level1Card = new Prop(new Bitmap(prefix + "level1-card.png"), 200, 400);
            level2Card = new Prop(new Bitmap(prefix + "level2-card.png"), 200, 400);
            level3Card = new Prop(new Bitmap(prefix + "level3-card.png"), 200, 400);
            level4Card = new Prop(new Bitmap(prefix + "level4-card.png"), 200, 400);
            level5Card = new Prop(new Bitmap(prefix + "level5-card.png"), 200, 400);
        }

        public static void menu()
        {
            Program.s.render(logo, 250, 20);
            Program.s.render(leftButton, 200, 400);
            Program.s.render(rightButton, 525, 400);
            Program.s.render(levelEditorButton, 10, 650);
            Program.s.render(customLevelButton, 425, 650);
            Program.s.render(Program.keyPressCooldown.ToString(), 0, 0, Brushes.White);

            switch(currentCard)
            {
                case 0:
                    Program.s.render(level1Card, 300, 230);
                    break;

                case 1:
                    Program.s.render(level2Card, 300, 230);
                    break;

                case 2:
                    Program.s.render(level3Card, 300, 230);
                    break;

                case 3:
                    Program.s.render(level4Card, 300, 230);
                    break;

                case 4:
                    Program.s.render(level5Card, 300, 230);
                    break;

                default:
                    Program.s.render(defaultCard, 300, 230);
                    break;
            }

            //check input
            if (Program.s.readKeyDown(Keys.Left) && Program.keyPressCooldown == 0)
            {
                Program.keyPressCooldown = Program.cooldownLength;
                leftDown = true;
            }
            else
            {
                leftDown = false;
            }

            if (Program.s.readKeyDown(Keys.Right) && Program.keyPressCooldown == 0)
            {
                Program.keyPressCooldown = Program.cooldownLength;
                rightDown = true;
            }
            else
            {
                rightDown = false;
            }

            if (Program.s.readKeyDown(Keys.Enter) && Program.keyPressCooldown == 0)
            {
                Program.keyPressCooldown = Program.cooldownLength;
                enterDown = true;
            }
            else
            {
                enterDown = false;
            }

            if (Program.s.readKeyDown(Keys.D1) && Program.keyPressCooldown == 0)
            {
                Program.keyPressCooldown = Program.cooldownLength;
                oneDown = true;
            }
            else
            {
                oneDown = false;
            }
            
            if (Program.s.readKeyDown(Keys.D2) && Program.keyPressCooldown == 0)
            {
                Program.keyPressCooldown = Program.cooldownLength;
                twoDown = true;
            }
            else
            {
                twoDown = false;
            }

            if (Program.s.keyUp())
            {
                leftDown = false;
                rightDown = false;
                enterDown = false;
                oneDown = false;
                twoDown = false;
                Program.keyPressCooldown = 0;
            }

            if(leftDown)
            {
                currentCard--;
                if (currentCard == -1)
                    currentCard = maxCards - 1;
            }
            if (rightDown)
            {
                currentCard++;
                if (currentCard == maxCards)
                    currentCard = 0;
            }

            if (enterDown)
            {
                Program.currentScreen = 2;
                LevelEditor.initMap("level" + (currentCard + 1).ToString());
            }

            if (oneDown)
            {
                Program.currentScreen = 2;
                LevelEditor.initMap("CUSTOM");
            }

            if (twoDown)
            {
                Program.currentScreen = 2;
                LevelEditor.initWithCustomMap();
            }
        }

    }
}
