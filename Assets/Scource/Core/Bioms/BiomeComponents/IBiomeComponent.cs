using System;


namespace Core.Bioms.BiomeComponents
{
	public interface IBiomeComponent
	{
		void InitComponent (BiomBase owner);

		void UpdateComponent ();

		void SetActive (bool active);
	}
}

