using UnityEngine;
using System;

namespace AdvancedHorrorFPS
{
    public class FlashLightScript : MonoBehaviour
    {
        public static FlashLightScript Instance;
        public Transform target;
        public bool isGrabbed = false;
        public float speed = 2.5f;
        public Transform positionTarget;
        public Light Light;
        public float BlueBattery = 100;
        public float BatterySpendNumber = 1;
        RaycastHit hit;
        public AudioSource audioSource;
        public Transform aimPoint;
        public LayerMask layerMask;

        public Action<bool> OnFlashLightActivateStateChanged;

        public Action<int, int> OnFlashLightAmountChanged; 

        void Awake()
        {
            BlueBattery = UnityEngine.Random.RandomRange(70,90);
            Instance = this;
        }
        public void FlashLight_Decision(bool decision)
        {
            Light.enabled = decision;
          //  GameCanvas.Instance.Indicator_BlueLight.SetActive(decision);
        }

        private void OnEnable()
        {
            OnFlashLightActivateStateChanged?.Invoke(true);

            OnFlashLightAmountChanged?.Invoke((int)BlueBattery, 100);
        }

        private void OnDisable()
        {
            OnFlashLightActivateStateChanged?.Invoke(false);
        }

        private void Start()
        {
            Light = GetComponent<Light>();
        }

        public void Grabbed()
        {
            MessageUi.HideItemMessage();

            MessageUi.ShowItemMessage("Press F for Flashlight!");

            positionTarget = GameObject.Find("FlashLightPoint").transform;

            HeroPlayerScript.Instance.FPSHands.SetActive(true);

            isGrabbed = true;
        }
        private void Update()
        {
            if (!isGrabbed) return;


            if (Input.GetKeyUp(KeyCode.F))
            {
                if (Light.enabled)
                {
                    FlashLight_Decision(false);
                    AudioManager.Instance.Play_Flashlight_Close();
                }
                else
                {
                    FlashLight_Decision(true);
                    MessageUi.HideItemMessage();
                    AudioManager.Instance.Play_Flashlight_Open();

                }
            }

            if (Light.enabled)
            {
                BlueBattery -= Time.deltaTime*0.3f;

                OnFlashLightAmountChanged?.Invoke((int)BlueBattery,100);
            }
        }

        public void PlayAudioBlueLight()
        {
            audioSource.Play();
        }

        public void StopAudioBlueLight()
        {
            audioSource.Stop();
        }

        void LateUpdate()
        {
            if (!isGrabbed) return;
            Vector3 dir = target.position - transform.position;
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, speed * Time.deltaTime);
            transform.position = positionTarget.position;





        }
    }
}