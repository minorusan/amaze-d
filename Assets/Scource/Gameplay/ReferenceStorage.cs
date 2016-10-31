using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Core.Interactivity.AI.Brains;
using Core.Bioms;


namespace Core.Gameplay
{
	public enum EReferenceType
	{
		None,
		Slave
	}

	public class ReferenceStorage
	{
		private Dictionary <EBiomType, HashSet<SlaveBrains>> _registeredSlaves;

		public ReferenceStorage ()
		{
			_registeredSlaves = new Dictionary<EBiomType, HashSet<SlaveBrains>> ();
			_registeredSlaves.Add (EBiomType.Fel, new HashSet<SlaveBrains> ());
			_registeredSlaves.Add (EBiomType.Storm, new HashSet<SlaveBrains> ());
		}

		public void RegisterSlave (SlaveBrains slave, EBiomType owner)
		{
			_registeredSlaves [owner].Add (slave);
		}

		public SlaveBrains[] GetSlavesOfBiome (EBiomType owner)
		{
			return _registeredSlaves [owner].ToArray ();
		}

	}
}

