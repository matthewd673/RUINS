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
    public class MainMenu
    {

        public static Prop logo;
        public static Prop levelEditorButton;
        public static Prop level1Card;

        public static void initMenu()
        {
            //the graphics
            string prefix = Environment.CurrentDirectory + @"\res\";
            logo = new Prop(new Bitmap(prefix + "logo.png"));
            levelEditorButton = new Prop(new Bitmap(prefix + "level-editor-button.png"), 350, 80);
            //THIS IS TEMPORARY
            level1Card = new Prop(new Bitmap(prefix + "level1-card.png"), 200, 400);
        }

        public static void menu()
        {
            Program.s.render(logo, 250, 20);
            Program.s.render(level1Card, 300, 230);
            Program.s.render(levelEditorButton, 225, 650);
        }

    }
}
