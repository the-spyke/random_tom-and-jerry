using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TomAndJerry
{
	class Program
	{
		private static readonly string _wallTexture = "██";
		static void Main(string[] args)
		{
			// 1 = Wall
			// 2 = Tom
			// 3 = Jerry
			var mazeData =
				new string[]
				{
					"0 0 0 0 0 0 0 0 0",
					"0 1 0 1 1 1 1 1 0",
					"0 1 0 0 0 1 0 1 0",
					"0 1 3 1 0 0 0 1 0",
					"0 1 1 1 0 1 1 1 0",
					"0 1 2 0 0 1 0 0 0",
					"0 0 0 1 0 1 1 0 1",
					"1 1 0 1 0 0 0 0 0",
					"1 0 0 0 1 1 0 0 0",
				};

			var mazeWidth = mazeData[0].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length;
			var mazeHeght = mazeData.GetUpperBound(0) + 1;

			MazeCell tomCell = null;
			MazeCell jerryCell = null;

			var maze = new MazeCell[mazeHeght, mazeWidth];

			for (int i = 0; i < mazeHeght; i++)
			{
				var cellTypes = mazeData[i].Split(' ').Select(x => (MazeCellTypes)Convert.ToInt32(x)).ToArray();

				for (int j = 0; j < mazeWidth; j++)
				{
					var currentCell = new MazeCell()
					{
						Type = cellTypes[j],
						X = j,
						Y = i
					};

					maze[i, j] = currentCell;

					if (currentCell.Type == MazeCellTypes.Tom)
					{
						tomCell = currentCell;
					}
					else if (currentCell.Type == MazeCellTypes.Jerry)
					{
						jerryCell = currentCell;
					}
				}
			}

			printMaze(maze);

			var cellQueue = new Queue<MazeCell>();

			cellQueue.Enqueue(tomCell);

			while (cellQueue.Count > 0)
			{
				var currentCell = cellQueue.Dequeue();

				// Uncomment to stop processing on Jerry.
				//if (currentCell.Type == MazeCellTypes.Jerry)
				//{
				//	break;
				//}

				EnqueueNextCells(cellQueue, currentCell, maze);

				// Uncomment to watch step-by-step.
				//printMaze(maze);
				//Console.ReadKey();
			}

			printMaze(maze);

			Console.WriteLine("Path to Jerry: " + (jerryCell.Distance > 0 ? jerryCell.Distance : -1));
			Console.WriteLine();
		}

		static void EnqueueNextCells(Queue<MazeCell> cellQueue, MazeCell currentCell, MazeCell[,] maze)
		{
			var mazeWidth = maze.GetUpperBound(1) + 1;
			var mazeHeght = maze.GetUpperBound(0) + 1;

			if (currentCell.X + 1 < mazeWidth)
			{
				var nextCell = maze[currentCell.Y, currentCell.X + 1];

				EnqueueNextCell(cellQueue, currentCell, nextCell);
			}

			if (currentCell.Y + 1 < mazeHeght)
			{
				var nextCell = maze[currentCell.Y + 1, currentCell.X];

				EnqueueNextCell(cellQueue, currentCell, nextCell);
			}

			if (currentCell.X - 1 >= 0)
			{
				var nextCell = maze[currentCell.Y, currentCell.X - 1];

				EnqueueNextCell(cellQueue, currentCell, nextCell);
			}

			if (currentCell.Y - 1 >= 0)
			{
				var nextCell = maze[currentCell.Y - 1, currentCell.X];

				EnqueueNextCell(cellQueue, currentCell, nextCell);
			}
		}

		static void EnqueueNextCell(Queue<MazeCell> cellQueue, MazeCell currentCell, MazeCell nextCell)
		{
			if (nextCell.Distance == 0 &&
				(nextCell.Type == MazeCellTypes.Road ||
				nextCell.Type == MazeCellTypes.Jerry))
			{
				nextCell.Distance = currentCell.Distance + 1;
				cellQueue.Enqueue(nextCell);
			}
		}

		static void printMaze(MazeCell[,] maze)
		{
			var mazeWidth = maze.GetUpperBound(1) + 1;
			var mazeHeght = maze.GetUpperBound(0) + 1;

			var wallCell = new MazeCell()
			{
				Type = MazeCellTypes.Wall
			};

			for (int i = 0; i < mazeWidth + 2; i++)
			{
				if (i > 0)
				{
					Console.Write(_wallTexture[0]);
				}

				Console.Write(GetCellTexture(wallCell));
			}

			Console.WriteLine();

			for (int i = 0; i < mazeHeght; i++)
			{
				Console.Write(GetCellTexture(wallCell));

				for (int j = 0; j < mazeWidth; j++)
				{
					var cell = maze[i, j];

					if (cell.Type == MazeCellTypes.Wall && (j == 0 || maze[i, j - 1].Type == MazeCellTypes.Wall))
					{
						Console.Write(_wallTexture[0]);
					}
					else
					{
						Console.Write(" ");
					}
					Console.Write(GetCellTexture(cell));
				}

				Console.Write(" ");
				Console.Write(GetCellTexture(wallCell));

				Console.WriteLine();
			}

			for (int i = 0; i < mazeWidth + 2; i++)
			{
				if (i > 0)
				{
					Console.Write(_wallTexture[0]);
				}

				Console.Write(GetCellTexture(wallCell));
			}

			Console.WriteLine();
			Console.WriteLine();
		}

		static string GetCellTexture(MazeCell cell)
		{
			switch (cell.Type)
			{
				case MazeCellTypes.Jerry:
					return "JJ";
				case MazeCellTypes.Wall:
					return _wallTexture;
				case MazeCellTypes.Tom:
					return "TT";
				default:
					return cell.Distance > 0 ? cell.Distance.ToString("00") : "  ";
			}
		}

		class MazeCell
		{
			public MazeCellTypes Type { get; set; }

			public int Distance { get; set; }

			public MazeCell CameFrom { get; set; }

			public int X { get; set; }

			public int Y { get; set; }
		}

		enum MazeCellTypes
		{
			Road,
			Wall,
			Tom,
			Jerry
		}
	}
}
