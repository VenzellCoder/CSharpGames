using UnityEngine;
using UnityEngine.Networking;
using Tanks.Explosions;
using Tanks.TankControllers;
using Tanks.CameraControl;

namespace Tanks.Hazards
{
	// <VEVZELL> класс описывает характер разрушения зданий
	[RequireComponent(typeof(NetworkIdentity))]
	public class Building : LevelHazard, IDamageObject
	{

		[SerializeField]
		protected float m_DamageThreshold = 10f;

		public bool isAlive { get; protected set; }
		private bool isFalling = false;

		// параметры скриншейка
		public float magnitude = 2f;
		public float duration = 2f;

		// погружение в землю
		float startFallY = 0f;
		public float finishFallY = -3f;
		float t = 0f;
		public float fallingSpeed = 0.8f;

		public GameObject destroyParticles;
		public ParticleSystem effect;

		Vector3 treeFallAxis; 

		BoxCollider[] boxCollidersArray;
		CapsuleCollider[] capsuleCollidersArray;

		Vector3 startPosition;
		Quaternion startRotation;

		void Awake()
		{
			effect = Instantiate (destroyParticles, transform.position, transform.rotation).GetComponent<ParticleSystem> ();
			effect.enableEmission = false;
		}

		//[ServerCallback]
		protected override void Start()
		{
			if (isServer) 
			{
				base.Start ();
				isAlive = true;
			}
			startPosition = transform.position;
			startRotation = transform.rotation;

			boxCollidersArray = GetComponents<BoxCollider> ();
			capsuleCollidersArray = GetComponents<CapsuleCollider> ();

		}

		void Update()
		{
			if (isFalling) {

				t += fallingSpeed * Time.deltaTime;
				float y = Mathf.Lerp(startFallY, finishFallY, t);
				Vector3 newPos = new Vector3 (transform.position.x, y, transform.position.z);
				transform.transform.position = newPos;


				if (t > 1.0f)
				{
					t = 0f;
					isFalling = false;
				}
			}
		}

		// Логика востановления состояния дерева при рестарте такая же, как и у мины
		[ServerCallback]
		private void ExplodeBuilding()
		{
			isAlive = false;
			RpcExplodeBuilding();

			// Скриншейк
			if (ScreenShakeController.s_InstanceExists)
			{
				ScreenShakeController shaker = ScreenShakeController.s_Instance;

				shaker.DoShake(transform.position, magnitude, duration);
			}
		}
			
		[ServerCallback]
		public override void ResetHazard()
		{
			RpcResetBuilding();
		}


		[ServerCallback]
		public override void ActivateHazard()
		{
			isAlive = true;
		}

		// Деактивация здания на клиенте 
		[ClientRpc]
		private void RpcExplodeBuilding()
		{
			if (boxCollidersArray.Length > 0) 
			{
				foreach (BoxCollider box in boxCollidersArray) 
				{
					if (box != null) 
					{
						box.enabled = false;
					}
				}
			}

			if (capsuleCollidersArray.Length > 0) 
			{
				foreach (CapsuleCollider capsule in capsuleCollidersArray) 
				{
					if (capsule != null) 
					{
						capsule.enabled = false;
					}
				}
			}

			isFalling = true;
			treeFallAxis = new Vector3 (Random.Range (-1f, 1f), 0f, Random.Range (-1f, 1f));

			effect.enableEmission = true;
			Invoke("StopEffect", fallingSpeed);

			//m_MineMesh.SetActive(false);
			//GetComponent<AudioSource>().Stop();
		}

		// Резет здания на клиенте 
		[ClientRpc]
		private void RpcResetBuilding()
		{
			//GetComponent<AudioSource>().Stop();
			//m_MineMesh.SetActive(true);
			//transform.rotation = Quaternion.identity;
			transform.position = startPosition;

			if (boxCollidersArray.Length > 0) 
			{
				foreach (BoxCollider box in boxCollidersArray) 
				{
					if (box != null) 
					{
						box.enabled = true;
					}
				}
			}

			if (capsuleCollidersArray.Length > 0) 
			{
				foreach (CapsuleCollider capsule in capsuleCollidersArray) 
				{
					if (capsule != null) 
					{
						capsule.enabled = true;
					}
				}
			}
			StopEffect ();
		}

		public Vector3 GetPosition()
		{
			return transform.position;
		}


		public void Damage(float damage)
		{
			if (isAlive && (damage >= m_DamageThreshold))
			{
				ExplodeBuilding ();
			}
		}

		public void SetDamagedBy(int playerNumber, string explosionId)
		{

		}

		void StopEffect()
		{
			effect.enableEmission = false;
		}
		
	}
}
