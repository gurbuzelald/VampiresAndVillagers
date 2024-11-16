using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

namespace AdvancedHorrorFPS
{
    public class MainMenuCanvas : MonoBehaviour
    {
        public string SceneName_GamePlay = "";
        public GameObject Panel_MainMenu;
        public GameObject Panel_Settings;

        public Image image_Progress;
        public GameObject Panel_Loading;
        public Text text_Progress;
        public GameObject ButtonStart;
        float progress = 0f;
        AsyncOperation asyncLoad;

        private void Start()
        {
            Time.timeScale = 1;
        }

        public void HandlePressedExitGame()
        {
            Application.Quit();
        }

        public void HandlePressedPlayGame()
        {
            Panel_MainMenu.SetActive(false);
            StartCoroutine(StartToLoadTheGame());
        }

        IEnumerator StartToLoadTheGame()
        {
            Panel_Loading.SetActive(true);
            yield return new WaitForSeconds(1);
            asyncLoad = SceneManager.LoadSceneAsync(SceneName_GamePlay);
            asyncLoad.allowSceneActivation = false;
            while (progress <= 1f)
            {
                image_Progress.fillAmount = progress;
                text_Progress.text = "%" + Mathf.Round(progress * 100f);
                progress += .01f;
                yield return new WaitForSeconds(.01f);
            }
            ButtonStart.SetActive(true);
            text_Progress.transform.parent.gameObject.SetActive(false);
        }

        public void Click_Start()
        {
            asyncLoad.allowSceneActivation = true;
        }

        public void HandlePressedSettingsButton()
        {
            Panel_Settings.SetActive(true);
            Panel_MainMenu.SetActive(false);
        }

        public void HandlePressedSettingsExitButton()
        {
            Panel_Settings.SetActive(false);
            Panel_MainMenu.SetActive(true);
        }
    }
}