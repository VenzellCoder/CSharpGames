using UnityEngine;
using System.Collections;
using System;

namespace Tanks.SinglePlayer
{

	[Serializable]
	public class DifficultyLevelDetails
	{
		[SerializeField]
		protected string m_Name;

		public string DifficultyLevelName
		{
			get{ return m_Name; }
		}
			
		[SerializeField]
		protected float m_hpModifier;

		public float hpModifier
		{
			get { return m_hpModifier; }
		}

		[SerializeField]
		protected float m_fireRateModifier;

		public float fireRateModifier
		{
			get { return m_fireRateModifier; }
		}

		[SerializeField]
		protected bool m_fireLeading;

		public bool fireLeading
		{
			get { return m_fireLeading; }
		}

		[SerializeField]
		protected float m_moveSpeedModifier;

		public float moveSpeedModifier
		{
			get { return m_moveSpeedModifier; }
		}

	}
}