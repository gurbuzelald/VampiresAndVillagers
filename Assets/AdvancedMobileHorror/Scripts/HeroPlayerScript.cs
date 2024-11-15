using System.Collections.Generic;
using UnityEngine;

namespace AdvancedHorrorFPS
{
    public class HeroPlayerScript : MonoBehaviour
    {
        public static HeroPlayerScript Instance;
        public GameObject LadderPointInCamera;
        public FirstPersonController firstPersonController;
        public CharacterController characterController;
        public Transform DemonComingPoint;
        public List<int> Keys_Grabbed = new List<int>();
        [HideInInspector]
        public GameObject Carrying_Ladder = null;
        public GameObject FPSHands;
        public FlashLightScript FlashLight;
        public bool isHoldingBox = false;

        public HealthComponent healthComponent;

        void Start()
        {
            healthComponent.OnPlayerDead += HandlePlayerDead;

            healthComponent.OnPlayerGetDamage += HandlePlayerGetDamage;

            Time.timeScale = 1;
        }

        public void GetDamage(int Damage)
        {
            healthComponent.GetDamage(Damage);

            GameCanvas.Instance.Show_Blood_Effect();
        }


        public void HandlePlayerDead()
        {
            DeactivatePlayer();

            TouchpadFPSLook.Instance.fCamShakeImpulse = 1;

            GameCanvas.Instance.Show_GameOverPanel();

            if (AdvancedGameManager.Instance.showFPSHands)
            {
                FPSHands.gameObject.SetActive(false);
            }
        }

        public void HandlePlayerGetDamage()
        {
            TouchpadFPSLook.Instance.fCamShakeImpulse = 0.5f;
        }


        private void Awake()
        {
            Instance = this;
        }

        public void Grab_Key(int ID)
        {
            Keys_Grabbed.Add(ID);
        }

        public void Get_Health()
        { 
            healthComponent.AddHealth(50);
        }

        public void ActivatePlayer()
        {
            transform.eulerAngles = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;
            firstPersonController.enabled = true;
            characterController.enabled = true;
            transform.eulerAngles = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;
        }

        public void DeactivatePlayer()
        {
            firstPersonController.enabled = false;
            characterController.enabled = false;
        }

        public void ActivatePlayerInputs()
        {
            firstPersonController.enabled = true;
            characterController.enabled = true;
        }
    }
}