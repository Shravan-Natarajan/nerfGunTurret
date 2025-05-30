﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

//Note for self: Works with a mouse for now, hopefully. Next prototype will be scaling up to facial.
namespace gunToShootMe
{
    public partial class Form1 : Form
    {
        public Stopwatch yoBlueToothDeviceHasConnected { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            yoBlueToothDeviceHasConnected = Stopwatch.StartNew();   //Starts stopwatch
            ArduinoPort.Open();                                     //Opens for commands
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            Random fireOrNot = new Random();    //Creates variable for randomization
            int checker= fireOrNot.Next(2000);  //Randomizes and stores that result as an int
            int fireBoolInit = 0;               //Initializes bool for the write

            if (checker==22) {
                fireBoolInit = 1;               //1/2000 chance of equaling 2000. If so, it changes the "bool" to true.
            }
            writeToPort(new Point(e.X, e.Y),fireBoolInit);  //Writes it to the arduino.
        }

        public void writeToPort(Point coordinates, int fireBool)
        {
            if (fireBool == 1) {
                fireBool = 15; //Placeholder coordinates
            }
            if (yoBlueToothDeviceHasConnected.ElapsedMilliseconds % 15 == 0) //Every 15 milliseconds, write to arduino
            {
                //note for self: if x, y, or firing are wrong way, do max value - whatever it is. IE, 180-coords.blahblahblah
                ArduinoPort.Write(String.Format("X{0}Y{1}Z{2}",
                    (coordinates.X / (Size.Width / 180)), //Fire at the given coordinates (with math involved for accuracy)
                    (coordinates.Y / (Size.Width / 180)), //Fire at the given coordinates (with math involved for accuracy)
                    (fireBool / (Size.Width / 180))       //Fire if bool gives the "green light"
                    ));
            }
        }
    }
}
