﻿using System;
using UnityEngine;


namespace Core.Bioms
{
	public enum EBiomType
	{
		None,
		Storm,
		Fel
	}

	public struct BiomShaperData
	{
		public GameObject Plane;
		public BiomBase Owner;
		public float Padding;
		public float GrowthSpeed;
	}
}

