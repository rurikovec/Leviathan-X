﻿using System;

namespace _02__The_Garden_Application
{
    class Program
    {
        static void Main(string[] args)
        {
            Garden theGarden = new Garden("theGarden");
            
            Flower Fyellow = new Flower("yellow");
            theGarden.Add(Fyellow);
            Flower Fblue = new Flower("blue");
            theGarden.Add(Fblue);

        }
    }
}
