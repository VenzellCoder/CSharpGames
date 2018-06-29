using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Tanks.SinglePlayer
{
	[Serializable]
	[CreateAssetMenu(fileName = "DifficultyLevelList", menuName = "Difficulty List")]
	public class DifficultyLevelList : ScriptableObject
	{
		[SerializeField]
		protected List<DifficultyLevelDetails> m_DifficultyLevels;

		public DifficultyLevelDetails this [int index]
		{
			get { return m_DifficultyLevels[index]; }
		}

		public int Count
		{
			get{ return m_DifficultyLevels.Count; }
		}

		public DifficultyLevelDetails GetDifficultyLevelDetails(int index)
		{
			return m_DifficultyLevels [index];
		}
	}
}

