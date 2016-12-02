using UnityEngine;
using System.Collections;
using Core.Bioms;
using UnityEngine.UI;


namespace Utils
{
	public class Scales : MonoBehaviour
	{
		public BiomBase firstBiome;
		public BiomBase secondBiome;
		public Image firstBiomeIndicator;
		public Image secondBimeIndicator;


		// Update is called once per frame
		void Update ()
		{
			

			if (firstBiome.BiomPower < secondBiome.BiomPower)
			{
				float difference = (float)firstBiome.BiomPower / (float)secondBiome.BiomPower;
				firstBiomeIndicator.fillAmount = difference;
				secondBimeIndicator.fillAmount = 1 - difference;
			}
			else
			{
				float difference = (float)secondBiome.BiomPower / (float)firstBiome.BiomPower;
				secondBimeIndicator.fillAmount = difference;
				firstBiomeIndicator.fillAmount = 1 - difference;
			}

			if (firstBiome.BiomPower == secondBiome.BiomPower)
			{
				secondBimeIndicator.fillAmount = 0.5f;
				firstBiomeIndicator.fillAmount = 0.5f;
			}
		}
	}

}
