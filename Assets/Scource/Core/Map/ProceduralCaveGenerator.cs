using System;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using Core.Interactivity.Movement;
using UnityEngine;


namespace Core.Map
{
	public class ProceduralCaveGenerator
	{
		#region PRIVATE

		private static Node[,] _map;
		private static int _width;
		private static int _height;
		private static int _randomFillPercent = 80;

		#endregion




		public static void GenerateCaveFromNodes (ref Node[,] nodes, int fillPercentage, int smoothIterations = 0)
		{
			_randomFillPercent = fillPercentage;
			_map = nodes;
			_width = _map.GetLength (0);
			_height = _map.GetLength (1);
			RandomFillMap ();

			for (int i = 0; i < smoothIterations; i++)
			{
				SmoothMap ();
			}
		}

		static void RandomFillMap ()
		{
			

			System.Random pseudoRandom = new System.Random ();

			for (int x = 0; x < _width; x++)
			{
				for (int y = 0; y < _height; y++)
				{
					if (x == 0 || x == _width - 1 || y == 0 || y == _height - 1)
					{
						_map [x, y].CurrentCellType = ECellType.Walkable;
					}
					else
					{
						_map [x, y].CurrentCellType = (pseudoRandom.Next (0, 100) < _randomFillPercent) ? ECellType.Blocked : ECellType.Walkable;
					}
				}
			}
		}

		static void SmoothMap ()
		{
			for (int x = 0; x < _width; x++)
			{
				for (int y = 0; y < _height; y++)
				{
					int neighbourWallTiles = GetSurroundingWallCount (x, y);

					if (neighbourWallTiles > 4)
						_map [x, y].CurrentCellType = ECellType.Blocked;
					else
					if (neighbourWallTiles < 4)
						_map [x, y].CurrentCellType = ECellType.Walkable;

				}
			}
		}

		static int GetSurroundingWallCount (int gridX, int gridY)
		{
			int wallCount = 0;
			for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
			{
				for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
				{
					if (neighbourX >= 0 && neighbourX < _width && neighbourY >= 0 && neighbourY < _height)
					{
						if (neighbourX != gridX || neighbourY != gridY)
						{
							wallCount += _map [neighbourX, neighbourY].CurrentCellType == ECellType.Blocked ? 0 : 1;
						}
					}
					else
					{
						wallCount++;
					}
				}
			}

			return wallCount;
		}

	}
}

