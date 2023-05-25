using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using static AutoBattle.Types;

namespace AutoBattle
{
    public class Grid
    {
        public List<GridBox> grids = new List<GridBox>();
        public int xLenght;
        public int yLength;
        public Grid(int Lines, int Columns)
        {
            xLenght = Lines;
            yLength = Columns;
            Console.WriteLine("The battle field has been created\n");
            for (int i = 0; i < xLenght; i++)
            {
                 
                for(int j = 0; j < yLength; j++)
                {
                    GridBox newBox = new GridBox(j, i, false, (yLength * i + j));
                   // Console.Write($"{newBox.Index}\n");
                    grids.Add(newBox);
                }
            }
        }

        // prints the matrix that indicates the tiles of the battlefield
        public void drawBattlefield()
        {
            for (int i = 0; i < xLenght; i++)
            {
                for (int j = 0; j < yLength; j++)
                {
                    GridBox currentgrid = grids[(yLength * i + j)]; ;
                    if (currentgrid.ocupied)
                    {
                        //if()
                        Console.Write("[X]\t");
                    }
                    else
                    {
                        Console.Write($"[ ]\t");
                    }
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
            Console.Write(Environment.NewLine + Environment.NewLine);
        }

    }
}
