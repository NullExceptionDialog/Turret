using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX.XInput;
using System.IO;
using System.Configuration;
using System.Runtime.Versioning;



namespace Turret
{


    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if(timer1.Enabled == false)
            {
                timer1.Enabled = true;
            }
            if(timer1.Enabled == true)
            { 
              //Won't do anything :/  
            }
        }

            int posx = 0;
            int posy = 0;

        string gun = "";
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            gun = listBox1.Text;
        }
        float verticalLimit = 150;
        float verticalAmount = 0;

        float horizontalLimit = 360;
        float horizontalAmount = 0;

        float heightLimit = 150;
        float height = 0;
        public void timer1_Tick(object sender, EventArgs e)
        {

            Console.Clear();
            var controller = new Controller(UserIndex.One);
            if (!controller.IsConnected)
            {
                Console.WriteLine("Hey, it seems like your controller is either broken, not plugged in or its switch is on D (Logitech F series only)");
                MessageBox.Show("Hey, it seems like your controller is either broken, not plugged in or its switch is on D (Logitech F series only)");
                throw new Exception("App.ControllerNotFoundException");
                //Application.Exit();
            }

            // Read the state of the controller
            var state = controller.GetState();
            var gamepad = state.Gamepad;

            //THESE POSITIONS ARE ON A SQUARE GRID ;)

            string Direction = "";
            //I'M SO SORRY FOR MAKING THIS ABOMINATION BUT I HAD TO
            if (gamepad.LeftThumbY > 2048 && gamepad.LeftThumbX < 2048 && gamepad.LeftThumbX > -2048)
            {
                Direction = "Forward";
                posy += 1;
            }
            else if (gamepad.LeftThumbY < -2048 && gamepad.LeftThumbX < 2048 && gamepad.LeftThumbX > -2048)
            {
                Direction = "Backward";
                posy -= 1;
            }
            else if (gamepad.LeftThumbX > 2048 && gamepad.LeftThumbY < 2048 && gamepad.LeftThumbY > -2048)
            {
                Direction = "Right";
                posx += 1;
            }
            else if (gamepad.LeftThumbX < -2048 && gamepad.LeftThumbY < 2048 && gamepad.LeftThumbY > -2048)
            {
                Direction = "Left";
                posx -= 1;
            }
            else if (gamepad.LeftThumbX < -25000 && gamepad.LeftThumbY > 2048)
            {
                Direction = "ForwardLeft";
                posx += 1;
                posy += 1;
            }
            else if (gamepad.LeftThumbX < -25000 && gamepad.LeftThumbY < -2048)
            {
                Direction = "BackwardLeft";
                posx -= 1;
                posy -= 1;
            }
            else if (gamepad.LeftThumbX > 25000 && gamepad.LeftThumbY > 2048)
            {
                Direction = "ForwardRight";
                posx += 1;
                posy += 1;
            }
            else if (gamepad.LeftThumbX > 25000 && gamepad.LeftThumbY < -2048)
            {
                Direction = "BackwardRight";
                posx += 1;
                posy -= 1;
            }
            else if (gamepad.LeftThumbX < 2048 && gamepad.LeftThumbX > -2048 && gamepad.LeftThumbY < 2048 && gamepad.LeftThumbY > -2048) 
            {
                Direction = "Middle";
                posx += 0;
                posy += 0;
            }
            else
            {
                Direction = "Unsure";
                MessageBox.Show("Warning: The position data is not %100 accurate, WARN UNSURE");
            }

            string Direction1 = "";
            //I'M SO SORRY FOR MAKING THIS ABOMINATION AGAIN BUT I HAD TO
            if (gamepad.RightThumbY > 2048 && gamepad.RightThumbX < 2048 && gamepad.RightThumbX > -2048)
            {
                Direction1 = "Forward";
                posy += 1;
            }
            else if (gamepad.RightThumbY < -2048 && gamepad.RightThumbX < 2048 && gamepad.RightThumbX > -2048)
            {
                Direction1 = "Backward";
                posy -= 1;
            }
            else if (gamepad.RightThumbX > 2048 && gamepad.RightThumbY < 2048 && gamepad.RightThumbY > -2048)
            {
                Direction1 = "Right";
                posx += 1;
            }
            else if (gamepad.RightThumbX < -2048 && gamepad.RightThumbY < 2048 && gamepad.RightThumbY > -2048)
            {
                Direction1 = "Left";
                posx -= 1;
            }
            else if (gamepad.RightThumbX < -25000 && gamepad.RightThumbY > 2048)
            {
                Direction1 = "ForwardLeft";
                posy += 1;
                posx -= 1;
            }
            else if (gamepad.RightThumbX < -25000 && gamepad.RightThumbY < -2048)
            {
                Direction1 = "BackwardLeft";
                posy -= 1;
                posx -= 1;
            }
            else if (gamepad.RightThumbX > 25000 && gamepad.RightThumbY > 2048)
            {
                Direction1 = "ForwardRight";
                posy += 1;
                posx += 1;
            }
            else if (gamepad.RightThumbX > 25000 && gamepad.RightThumbY < -2048)
            {
                Direction1 = "BackwardRight";
                posy -= 1;
                posx += 1;
            }
            else if (gamepad.RightThumbX < 2048 && gamepad.RightThumbX > -2048 && gamepad.RightThumbY < 2048 && gamepad.RightThumbY > -2048)
            {
                Direction1 = "Middle";
            }
            else
            {
                Direction1 = "Unsure";
                MessageBox.Show("Warning: The position data is not %100 accurate, WARN UNSURE");
            }

            Thread.Sleep(25); //Debounce it, it doesn't need to be instantaneous, just work.

            string Direction2 = "";

            //ALL OF THE VALUES ARE MILIMETERS (1/1000TH OF A METER) OR 1/3000TH OF A FEET EXCEPT HEIGHT WHICH IS CM (1/100TH OF A METER) OR 1/300TH OF A FEET



            if (gamepad.Buttons == GamepadButtonFlags.DPadUp && verticalAmount < verticalLimit)
            {
                if (verticalAmount == verticalLimit)
                {
                    return;
                }
                else
                {
                    verticalAmount += 2f;
                }            
                Direction2 = "Tilting up";
            }

            if(gamepad.Buttons == GamepadButtonFlags.DPadDown && verticalAmount < verticalLimit + 1 && verticalAmount > -verticalLimit - 1)
            {
                if (verticalAmount > verticalLimit)
                {
                    return;
                }
                else
                {
                    verticalAmount -= 2f;
                }
                Direction2 = "Tilting down";
            }

            if(gamepad.Buttons == GamepadButtonFlags.DPadLeft && horizontalAmount < horizontalLimit + 1 && horizontalAmount > -horizontalLimit - 1)
            {
                if (horizontalAmount > horizontalLimit)
                {
                    return;
                }
                else
                {
                    horizontalAmount -= 5.0f;

                }
                Direction2 = "Moving left";
            }

            if (gamepad.Buttons == GamepadButtonFlags.DPadRight && horizontalAmount < horizontalLimit + 1)
            {
                if (horizontalAmount == horizontalLimit)
                {
                    return;
                }
                else
                {
                horizontalAmount += 5.0f;

                }
                Direction2 = "Moving right";
            }


            if(gamepad.Buttons == GamepadButtonFlags.Start && height < heightLimit)
            {
                if (height == heightLimit)
                {
                    return;
                }
                else
                {
                    height += 15f;
                }
                Direction2 = "Increasing height";
            }
            
            if (gamepad.Buttons == GamepadButtonFlags.Back && height < heightLimit + 1 && height > 0 + 1 )
            {
                if (height > heightLimit)
                {
                    return;
                }
                else
                {
                    height -= 15f;
                }
                Direction2 = "Decreasing height";
            }



            bool TriggerPulled = false;
            if(gamepad.RightTrigger == 255)
            {
                TriggerPulled = true;
            }
            else
            {
                TriggerPulled = false;
            }


            Console.WriteLine("Buttons: " + gamepad.Buttons);
            Console.WriteLine("Left Trigger: " + gamepad.LeftTrigger);
            Console.WriteLine("Right Trigger: " + gamepad.RightTrigger);
            Console.WriteLine("Left Thumbstick: " + gamepad.LeftThumbX + ", " + gamepad.LeftThumbY);
            Console.WriteLine("Right Thumbstick: " + gamepad.RightThumbX + ", " + gamepad.RightThumbY);
            // Print the state of the buttons and triggers

            label1.Text = "Camera Direction : " + Direction1.ToString() + "X" + posx + " y" + posy;
            label2.Text = "Movement : " + Direction.ToString();
            label3.Text = "Has shot? : " + TriggerPulled.ToString();
            label4.Text = "Weapon : " + gun.ToString();
            label5.Text = "Turret Location : " + Direction2.ToString() + " TiltY : " + verticalAmount.ToString() + "mm MoveX : " + horizontalAmount +"deg height : " + height + "cm";

        }

     

        private void label3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Easter egg?!?!");
            throw new Exception("System.Desktop.MissingDesktopFilesException : WARNING, YOUR DESKTOP FILES ARE MISSING (!) THIS WAS A PRANK :D ENJOY THE REST OF YOUR DAY! :) :)");
        }

    }
}

// CREATED BY AHMET E. C. AKA NULL.EXCEPTIONDIALOG [NULLABLETURKISH(X/TW)],
// MADE WITH LOVE FROM TÜRKIYE, 20242207 2024-22-07
//                                  https://www.GitHub.com/NullExceptionDialog
