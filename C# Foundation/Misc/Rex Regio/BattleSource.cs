﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Rex_Regio
{
    class BattleSource
    {
        private int ChampChoice;
        private Champion playerMagnus = new Magnus();
        private Champion playerLegibus = new Legibus();
        private Champion playerMysterio = new Mysterio();
        private string champName;
        private Dragon TheDragon = new Dragon();
        private Random DragonBrains = new Random();
        private int DragonStategy;

        private int[] PlayerStats;
        private int[] DragonStats;

        private int TurnNumber;
        private bool PlayerTurn;

        public BattleSource(int ChampChoiceInput)
        {
            ChampChoice = ChampChoiceInput;
            champName = ChampMenu.GetChampName();
            
            Start();
        }

        // The Game Battle Source ---------------------------------------------
        public void Start()
        {
            Log.ResetAllText();
            Log.Ini();
            ThePlayer().Init();
            ThePlayer().ApplyBuffs();
            TheDragon.Init();
            TheDragon.ApplyBuffs();
            Console.CursorVisible = false;

            // -------- The Battle
            bool gameOver = false;
            do
            {
                // ---- Player Turn
                TurnNumber = 3;
                do
                {
                    PlayerTurn = true;
                    DisplayInterface();
                    Console.WriteLine("\n" +
                        "\n--------------" +
                        "\n-= Manual" +
                        "\n'Enter' to attack (1 turn)" +
                        "\n'P' to drink potion (1 turn)" +
                        "\n'Tab' to switch weapons (1 turn)" +
                        "\n'Space' to change location (2 turns)" +
                        "\n'R' to rest (All turns left)" +
                        "\n'L' to see the entire Log (0 turns)");
                    var userInput = Console.ReadKey(true).Key;
                    if (userInput == ConsoleKey.Enter) 
                    {
                        // If both are in the same location
                        if (PlayerStats[9] == DragonStats[9])
                        {
                            // If player has enough stamina
                            if(ThePlayer().StaminaPenaltyCalc())
                            {
                                // Procede with the attack
                                TheDragon.ReactAttack(PlayerStats[5]);
                                ThePlayer().PlayAttack(DragonStats[6]);
                                TurnNumber--;
                            }
                            else
                            {
                                Log.PlayerNoStamina();
                            }
                        }
                        // If player has Spears equipped
                        else if (PlayerStats[8] == 3)
                        {
                            if (ThePlayer().StaminaPenaltyCalc())
                            {
                                TheDragon.ReactAttack(PlayerStats[5]);
                                ThePlayer().PlayAttack(DragonStats[6]);
                                TurnNumber--;
                            }
                            else Log.PlayerNoStamina();
                        }
                        // If player doesn't have Spears equipped
                        else
                        {
                            Log.PlayerDiffLocation();
                            Console.WriteLine("\n--Can't attack! Must be in the same location!");
                        }
                    }
                    else if (userInput == ConsoleKey.P)
                    {
                        ThePlayer().DrinkPotion();
                        TurnNumber--;
                    }
                    else if (userInput == ConsoleKey.Tab)
                    {
                        ThePlayer().SwitchWeapon();
                        ThePlayer().ApplyBuffs();
                        TurnNumber--;
                    }
                    else if (userInput == ConsoleKey.Spacebar)
                    {
                        if (TurnNumber < 2) Console.WriteLine("\n--Not enough turns to switch location!");
                        else
                        {
                            ThePlayer().SwitchLocation();
                            ThePlayer().ApplyBuffs();
                            TurnNumber--;
                            TurnNumber--;
                        }
                    }
                    else if (userInput == ConsoleKey.R)
                    {
                        ThePlayer().Rest(TurnNumber);
                        TurnNumber = 0;
                    }
                    else if (userInput == ConsoleKey.L)
                    {
                        Log.LoadBig();
                    }
                    else if (userInput == ConsoleKey.Escape) gameOver = true;

                    if (TheDragon.CheckIfAlive() == false) break;
                } while (TurnNumber > 0);
                if (TheDragon.CheckIfAlive() == false) break;

                // ---- Dragon Turn
                TurnNumber = 3;
                DragonStategy = 3;
                do
                {
                    PlayerTurn = false;
                    DisplayInterface();
                    System.Threading.Thread.Sleep(2500);

                    // -- Dragon "AI"

                    //// If both at Rocks 
                    //if (DragonStats[9] == PlayerStats[9])
                    //{
                    //    //If Spike is not equipped
                    //    if (DragonStats[8] != 2)
                    //    {
                    //        TheDragon.SwitchStance(2);
                    //        TurnNumber--;
                    //    }
                    //    if (TheDragon.StaminaPenaltyCalc())
                    //    {
                    //        ThePlayer().ReactAttack(DragonStats[5]);
                    //        TheDragon.PlayAttack(PlayerStats[6]);
                    //        TurnNumber--;
                    //    }
                    //    else
                    //    {
                    //        TheDragon.Rest(TurnNumber);
                    //        TurnNumber--;
                    //    }

                    //}

                    //// If just player at Rocks
                    //if (PlayerStats[9] == 1 && DragonStats[9] != 1)
                    //{
                    //    if (DragonStats[8] != 3)
                    //    {
                    //        int decision = DragonBrains.Next(1, 11);
                    //        if (decision > 6)
                    //        {
                    //            TheDragon.SwitchStance(3);
                    //            TurnNumber--;
                    //        }
                    //        else
                    //        {
                    //            TheDragon.SwitchLocation(1);
                    //            TurnNumber--;
                    //        }
                    //    }
                    //    if (DragonStats[8] == 3)
                    //    {
                    //        if (TheDragon.StaminaPenaltyCalc())
                    //        {
                    //            ThePlayer().ReactAttack(DragonStats[5]);
                    //            TheDragon.PlayAttack(PlayerStats[6]);
                    //            TurnNumber--;
                    //        }
                    //        else
                    //        {
                    //            TheDragon.Rest(TurnNumber);
                    //            TurnNumber--;
                    //        }
                    //    }
                    //}

                    //// If both at Huts
                    //if(PlayerStats[9] == 2 && DragonStats[9] == 2)
                    //{

                    //}

                    // Default Primitive Behavior
                    if (TheDragon.StaminaPenaltyCalc())
                    {
                        ThePlayer().ReactAttack(DragonStats[5]);
                        TheDragon.PlayAttack(PlayerStats[6]);
                    }
                    else TheDragon.Rest(TurnNumber);
                    TurnNumber--;


                    if (ThePlayer().CheckIfAlive() == false) break;
                } while (TurnNumber > 0);

                if (ThePlayer().CheckIfAlive() == false) gameOver = true;
                if (TheDragon.CheckIfAlive() == false) gameOver = true;
            } while (gameOver == false);
            DisplayInterface();
            Console.WriteLine("\n\nGAME OVER!");
        }

        // Battle Turn Counter ---------------------------------------------
        public string ShowTurn()
        {
            if (PlayerTurn == true) return "You";
            else if (PlayerTurn == false) return "Dragon";
            else throw new Exception("\n\nError!\nShow Turn has invalid value!");
        }

        // The Interface ---------------------------------------------
        public void DisplayInterface()
        {
            XL.LongSpace();
            LoadInterfacePlayerStats();
            LoadInterfaceDragonStats();
            InterfaceUI();
        }

        public void InterfaceUI()
        {
            var output = new[]
            {
                $"\n---------------------------------------------------------------------------------------------------------------------\n",
                $"DRAGON                                           TURN                                           {champName.ToUpper()}\n" +
                $"                                                {ShowTurn()} ({TurnNumber}x)\n",
                $"Health: {DragonStats[4]}/{DragonStats[0]}   \t\t\t\t\t\t\t\t\t\tHealth: {PlayerStats[4]}/{PlayerStats[0]}\n",
                $"Attack: {DragonStats[5]}/{DragonStats[1]}\t\t\t\t\t\t\t\t\t\t\tAttack: {PlayerStats[5]}/{PlayerStats[1]}\n",
                $"Defence: {DragonStats[6]}/{DragonStats[2]}\t\t\t\t\t\t\t\t\t\t\tDefence: {PlayerStats[6]}/{PlayerStats[2]}\n",
                $"Stamina: {DragonStats[7]}/{DragonStats[3]}  \t\t\t\t\t\t\t\t\t\tStamina: {PlayerStats[7]}/{PlayerStats[3]}\n",
                $"Location: {ShowLocationDragon()}\t\t\t\t\t\t\t\t\t\t\tLocation: {ShowLocationPlayer()}\n",
                $"Stance: {ShowStance()}\t\t\t\t\t\t\t\t\t\t\tWeapon: {ShowWeapon()}\n" +
                $"Fury: {DragonStats[10]}%\t\t\t\t\t\t\t\t\t\t\tPotions: {PlayerStats[10]}\n",
                $"\n" +
                $"Log:\n",
            };

            foreach (var line in output)
            {
                Console.Write(line);
            }
            Log.LoadSmall();
        }

        public void LoadInterfacePlayerStats()
        {
            PlayerStats = ThePlayer().GetStatsInterface();
        }

        public void LoadInterfaceDragonStats()
        {
            DragonStats = TheDragon.GetStatsInterface();
        }

        // The Interface -- Player Stats
        public string ShowWeapon()
        {
            if (PlayerStats[8] == 1) return "Longsword";
            else if (PlayerStats[8] == 2) return "Battle-Axe";
            else if (PlayerStats[8] == 3) return "Spears";
            else throw new Exception("\n\n--Error!\nShow weapon in interface has invalid value!");
        }

        public string ShowLocationPlayer()
        {
            if (PlayerStats[9] == 1) return "Rocks";
            else if (PlayerStats[9] == 2) return "Huts";
            else if (PlayerStats[9] == 3) return "Trees";
            else throw new Exception("\n\n--Error!\nShow player location in interface has invalid value!");
        }

        // The Interface -- Dragon Stats
        public string ShowStance()
        {
            if (DragonStats[8] == 1) return "Claws";
            else if (DragonStats[8] == 2) return "Spear";
            else if (DragonStats[8] == 3) return "Fire";
            else throw new Exception("\n\n--Error!\nShow stance in interface has invalid value!");
        }

        public string ShowLocationDragon()
        {
            if (DragonStats[9] == 1) return "Rocks";
            else if (DragonStats[9] == 2) return "Huts";
            else if (DragonStats[9] == 3) return "Trees";
            else throw new Exception("\n\n--Error!\nShow dragon location in interface has invalid value!");
        }

        // The Player Champion Choice ---------------------------------------------
        public Champion ThePlayer()
        {
            if (ChampChoice == 1) return playerMagnus;
            else if (ChampChoice == 2) return playerLegibus;
            else if (ChampChoice == 3) return playerMysterio;
            else throw new Exception("\n\n-- Error!\nChampion choice output is invalid!");
        }
    }
}
