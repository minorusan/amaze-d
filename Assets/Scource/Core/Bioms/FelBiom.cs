using UnityEngine;
using System.Collections;


namespace Core.Bioms
{
	public class FelBiom : BiomBase
	{
		public float Rocky;
		public int Grain;
		private DiamondSquareTerrain _algorithm;


		protected override void Start ()
		{
			
			_algorithm = new DiamondSquareTerrain ();
			base.Start ();
		}



		protected override void GenerateTerrain ()
		{
			_currentMap = _algorithm.GetHeightMap ((int)_selfMapLength, Grain, Rocky);
			base.GenerateTerrain ();
			Rocky += Rocky + 0.03f <= 30 ? 0.03f : 0;
			Grain += Grain + 1 <= 200 ? 1 : 0;
		}
	}

}
