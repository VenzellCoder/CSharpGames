using UnityEngine;
using UnityEngine.Networking;
using Tanks.Shells;
using Tanks.CameraControl;
using Tanks.Data;
using Tanks.Effects;
using Tanks.TankControllers;

namespace Tanks.Explosions
{
	public class CrashManager : NetworkBehaviour
	{
		/// <summary> 
		/// Reimplemented singleton. Can't use generics on NetworkBehaviours
		/// </summary>
		public static CrashManager s_Instance
		{
			get;
			protected set;
		}

		/// <summary>
		/// Gets whether an instance of this singleton exists
		/// </summary>
		public static bool s_InstanceExists { get { return s_Instance != null; } }

		protected virtual void Awake()
		{
			if (s_Instance != null)
			{
				Destroy(gameObject);
			}
			else
			{
				s_Instance = this;
			}
		}
			
		/// <summary>
		/// Clear instance
		/// </summary>
		protected virtual void OnDestroy()
		{
			if (s_Instance == this)
			{
				s_Instance = null;
			}
		}

		// <VENZELL> передаём серверу данные о танке, который нужно ранить. Сервер распространяет данные по клиентам.
		[Command]
		public void CmdCrash(GameObject crashParticipant)
		{
			IDamageObject targetHealth = crashParticipant.GetComponentInParent<IDamageObject> ();

			if (targetHealth != null && targetHealth.isAlive)
			{
				targetHealth.Damage (10);

				if (isServer)
				{
					//RpcCrash (crashParticipant);
				}
			}
		}

		// <VENZELL> Метод ниже для мультиплеера
		/*
		[ClientRpc]
		private void RpcCrash(GameObject crashParticipant)
		{
			IDamageObject targetHealth = crashParticipant.GetComponentInParent<IDamageObject> ();
			targetHealth.Damage (10);
		}
		*/



		private void DoShakeForExplosion(Vector3 explosionPosition, ExplosionSettings explosionConfig)
		{
			// Do screen shake on main camera
			if (ScreenShakeController.s_InstanceExists)
			{
				ScreenShakeController shaker = ScreenShakeController.s_Instance;

				float shakeMagnitude = explosionConfig.shakeMagnitude;
				shaker.DoShake(explosionPosition, 1f, 0.5f, 0.0f, 1.0f);
			}
		}
	}
}
