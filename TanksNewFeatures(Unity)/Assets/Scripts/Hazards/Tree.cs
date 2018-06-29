using UnityEngine;
using UnityEngine.Networking;
using Tanks.Explosions;
using Tanks.TankControllers;

namespace Tanks.Hazards
{
	[RequireComponent(typeof(NetworkIdentity))]
	public class Tree : LevelHazard, IDamageObject
	{

		[SerializeField]
		protected float m_DamageThreshold = 10f;

		public bool isAlive { get; protected set; }

		// дерево падает
		private bool isFalling = false;
		// скорость падения 
		public float fallingSpeed = 1f;
		// конечное положение вращения 
		Vector3 targetRot;

		// изменение угла (для интерполяции) 
		float minimum = 0f;
		float maximum = 90f;
		float t = 0f;
		Vector3 treeFallAxis; 

		[ServerCallback]
		protected override void Start()
		{
			base.Start();
			isAlive = true;
		}

		void Update()
		{
			if (isFalling) {

				t += fallingSpeed * Time.deltaTime;
				float ang = Mathf.Lerp(minimum, maximum, t);
				transform.rotation = Quaternion.AngleAxis(ang, treeFallAxis);

				if (t > 1.0f)
				{
					t = 0f;
					isFalling = false;
				}
			}
		}

		// Логика востановления состояния дерева при рестарте такая же, как и у мины
		[ServerCallback]
		private void ExplodeTree()
		{
			isAlive = false;
			RpcExplodeTree();
		}


		[ServerCallback]
		public override void ResetHazard()
		{
			RpcResetTree();
		}
			

		[ServerCallback]
		public override void ActivateHazard()
		{
			isAlive = true;
		}

		// деактивация дерева на клиенте
		[ClientRpc]
		private void RpcExplodeTree()
		{
			GetComponent<CapsuleCollider> ().enabled = false;
			isFalling = true;
			treeFallAxis = new Vector3 (Random.Range (-1f, 1f), 0f, Random.Range (-1f, 1f));
			//m_MineMesh.SetActive(false);
			//GetComponent<AudioSource>().Stop();
		}

		// Резет дерева на клиенте 
		[ClientRpc]
		private void RpcResetTree()
		{
			transform.rotation = Quaternion.identity;
			GetComponent<CapsuleCollider> ().enabled = true;

		}

		public Vector3 GetPosition()
		{
			return transform.position;
		}
			
		public void Damage(float damage)
		{
			if (isAlive && (damage >= m_DamageThreshold))
			{
				ExplodeTree ();
			}
		}

		public void SetDamagedBy(int playerNumber, string explosionId)
		{
			
		}
	}
}
