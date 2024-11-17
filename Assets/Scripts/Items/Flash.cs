using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AdvancedHorrorFPS;

public class Flash :ItemEntity
{
        public Light Light;
        public float BlueBattery = 100;
        public Action<bool> OnFlashLightActivateStateChanged;
        public Action<int, int> OnFlashLightAmountChanged;
        public int maxAmontBattery = 100;

        void Awake()
        {
            BlueBattery = UnityEngine.Random.RandomRange(maxAmontBattery/2, maxAmontBattery);
        }

        public void FlashLight_Decision(bool decision)
        {
            Light.enabled = decision;
        }

        private void OnEnable()
        {
            OnFlashLightActivateStateChanged?.Invoke(true);

            OnFlashLightAmountChanged?.Invoke((int)BlueBattery, maxAmontBattery);
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
            isGrabbed = true;
        }
        private void Update()
        {
            if (!isGrabbed)
            {
                return;
             }
  
            if (Input.GetKeyDown(KeyCode.Q))
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
                BlueBattery -= Time.deltaTime * 0.3f;

                OnFlashLightAmountChanged?.Invoke((int)BlueBattery, maxAmontBattery);
            }
        }        
    }





