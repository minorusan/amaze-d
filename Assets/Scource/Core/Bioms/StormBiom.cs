using UnityEngine;
using System.Collections;


namespace Core.Bioms
{
	public class StormBiom : BiomBase
	{
		public float Rocky;
		public int Grain;
		private DiamondSquareTerrain _algorithm;


		protected override void Start ()
		{
			_algorithm = new DiamondSquareTerrain ();
			base.Start ();
			BiomType = EBiomType.Storm;
		}



		protected override void PerformTerrainGeneration ()
		{
			base.PerformTerrainGeneration ();
			Debug.Log ("Calculating new hieghts");
			CurrentMap = _algorithm.GetHeightMap ((int)_shaper.MapLength, Grain, Rocky);
			_shaper.GenerateTerrain ();
			Rocky += Rocky + 0.03f <= 30 ? 0.03f : 0;
			Grain += Grain + 1 <= 200 ? 1 : 0;
		}

	}

}
