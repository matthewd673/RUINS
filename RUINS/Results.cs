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
    public class Results
    {

        public static Prop victoryDialog;
        public static Prop failureDialog;

        public static bool enterDown;

        public static bool shouldDisplay = true;

        public static void initResults()
        {
            victoryDialog = new Prop(new Bitmap(Environment.CurrentDirectory + @"\res\victory-dialog.png"));
            failureDialog = new Prop(new Bitmap(Environment.CurrentDirectory + @"\res\failure-dialog.png"));
        }

        public static void displayResults(bool victory)
        {
            if (victory)
                Program.s.render(victoryDialog, 200, 200);
            if (!victory)
                Program.s.render(failureDialog, 200, 200);

            if(Program.s.readKeyDown(Keys.Enter) && Program.keyPressCooldown == 0)
            {
                enterDown = true;
                Program.keyPressCooldown = Program.cooldownLength;
            }
            else
            {
                enterDown = false;
            }

            if(Program.s.keyUp())
            {
                enterDown = false;
                Program.keyPressCooldown = 0;
            }

            if(enterDown)
            {
                shouldDisplay = false;
                MainMenu.initMenu();
                Program.currentScreen = 0;
                shouldDisplay = true;
            }
        }

    }
}
