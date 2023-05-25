using System;
using static AutoBattle.Character;
using static AutoBattle.Grid;
using System.Collections.Generic;
using System.Linq;
using static AutoBattle.Types;

namespace AutoBattle
{
    class Program
    {
        static void Main(string[] args)
        {
            Grid grid = new Grid(0, 0);
            string yourName;
            CharacterClass playerCharacterClass;
            CharacterClass enemyCharacterClass;
            GridBox PlayerCurrentLocation;
            GridBox EnemyCurrentLocation;
            Character PlayerCharacter;
            Character EnemyCharacter;
            List<Character> AllPlayers = new List<Character>();
            int currentTurn = 0;
            int numberOfPossibleTiles = grid.grids.Count;
            int rows;
            int columns;
            Setup();


            void Setup()
            {

                NamePlayer();
            }
            void NamePlayer()
            {
                Console.WriteLine("Enter your character name:");
                yourName = Console.ReadLine();
                if (string.IsNullOrEmpty(yourName))
                {
                    Console.WriteLine("Enter with  name valid");
                    NamePlayer();
                }
                else
                {
                    GetPlayerChoice();
                }
            }
            void GetPlayerChoice()
            {           
                //asks for the player to choose between for possible classes via console.
                Console.WriteLine("Choose Between One of this Classes:\n");
                Console.WriteLine("[1] Paladin, [2] Warrior, [3] Cleric, [4] Archer");
                //store the player choice in a variable
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreatePlayerCharacter(Int32.Parse(choice));
                        break;
                    case "2":
                        CreatePlayerCharacter(Int32.Parse(choice));
                        break;
                    case "3":
                        CreatePlayerCharacter(Int32.Parse(choice));
                        break;
                    case "4":
                        CreatePlayerCharacter(Int32.Parse(choice));
                        break;
                    default:
                        GetPlayerChoice();
                        break;
                }
            }

            void CreatePlayerCharacter(int classIndex)
            {

                playerCharacterClass = (CharacterClass)classIndex;
                Console.WriteLine($"Player Class Choice: {playerCharacterClass}");
                PlayerCharacter = new Character(playerCharacterClass);
                PlayerCharacter.Name = yourName + " " + PlayerCharacter.CharacterClassSpecific.characterClass;
                PlayerCharacter.PlayerIndex = 0;

                CreateEnemyCharacter();

            }

            void CreateEnemyCharacter()
            {
                //randomly choose the enemy class and set up vital variables
            
                int randomInteger = GetRandomInt(1,4);
                enemyCharacterClass = (CharacterClass)randomInteger;
                Console.WriteLine($"Enemy Class Choice: {enemyCharacterClass}");
                EnemyCharacter = new Character(enemyCharacterClass);
                EnemyCharacter.Name = "Enemy" + " " + EnemyCharacter.CharacterClassSpecific.characterClass;
                PlayerCharacter.PlayerIndex = 1;
                GridRows();

            }
           
            void GridRows()
            {
                Console.Write(Environment.NewLine);

                Console.WriteLine("Enter the battlefield size (rows):");
                string  entrada = Console.ReadLine();          
                if(int.TryParse(entrada, out int numero))
                {
                    rows = numero;
                }
                else
                {
                    Console.WriteLine("Enter numeber valid (rows):");
                    GridRows();
                }
                GridColumns();
            }

            void GridColumns()
            {
                Console.Write(Environment.NewLine);

                Console.WriteLine("Enter the battlefield size (columns):");
                string entrada = Console.ReadLine();  
                if (int.TryParse(entrada, out int numero))
                {
                    columns = numero;
                }
                else
                {
                    Console.WriteLine("Enter numeber valid (columns):");
                    GridColumns();
                }
           
                grid = new Grid(rows, columns);

                StartGame();
            }

            void StartGame()
            {
                //populates the character variables and targets
                EnemyCharacter.Target = PlayerCharacter;
                PlayerCharacter.Target = EnemyCharacter;
                AllPlayers.Add(PlayerCharacter);
                AllPlayers.Add(EnemyCharacter);
                AlocatePlayers();
                StartTurn();

            }

            void StartTurn()
            {

                if (currentTurn == 0)
                {
                    //AllPlayers.Sort();  
                }

                foreach (Character character in AllPlayers)
                {
                    character.StartTurn(grid);
                }

                currentTurn++;
                HandleTurn();
            }

            void HandleTurn()
            {
                if (PlayerCharacter.Health <= 0)
                {
                    Console.Write(Environment.NewLine + Environment.NewLine);

                    Console.WriteLine($"Player {EnemyCharacter.Name}  \n");
                    Console.WriteLine($"Win \n");
                    Console.WriteLine("endgame");
                    Console.Write(Environment.NewLine + Environment.NewLine);
                    return;
                }
                else if (EnemyCharacter.Health <= 0)
                {
                    Console.Write(Environment.NewLine + Environment.NewLine);
                    // endgame?
                    Console.WriteLine($"Player {PlayerCharacter.Name} \n");
                    Console.WriteLine($"Win \n");
                    Console.WriteLine("endgame");
                    Console.Write(Environment.NewLine + Environment.NewLine);

                    return;
                }
                else
                {
                    Console.Write(Environment.NewLine + Environment.NewLine);
                    Console.WriteLine("Click on any key to start the next turn...\n");
                    Console.Write(Environment.NewLine + Environment.NewLine);

                    ConsoleKeyInfo key = Console.ReadKey();
                    StartTurn();
                }
            }

            int GetRandomInt(int min, int max)
            {
                var rand = new Random();
                int index = rand.Next(min, max);
                return index;
            }

            void AlocatePlayers()
            {
                AlocatePlayerCharacter();

            }

            void AlocatePlayerCharacter()
            {
                int random = 0;
                GridBox RandomLocation = (grid.grids.ElementAt(random));
                Console.Write($"{random}\n");
                if (!RandomLocation.ocupied)
                {
                    PlayerCurrentLocation = RandomLocation;
                    RandomLocation.ocupied = true;
                    grid.grids[random] = RandomLocation;
                    PlayerCharacter.currentBox = grid.grids[random];
                    AlocateEnemyCharacter();
                }
                else
                {
                    AlocatePlayerCharacter();
                }
            }

            void AlocateEnemyCharacter()
            {
                int random = GetRandomInt(grid.grids.Count - 4, grid.grids.Count);
                GridBox RandomLocation = (grid.grids.ElementAt(random));
                Console.Write($"{random}\n");
                if (!RandomLocation.ocupied)
                {
                    EnemyCurrentLocation = RandomLocation;
                    RandomLocation.ocupied = true;
                    grid.grids[random] = RandomLocation;
                    EnemyCharacter.currentBox = grid.grids[random];
                    grid.DrawBattlefield();
                }
                else
                {
                    AlocateEnemyCharacter();
                }


            }

        }
    }
}
