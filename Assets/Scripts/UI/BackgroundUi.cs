using Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class BackgroundUi : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private TMP_Text _levelNameText; // TODO move to ui controller

        public void Init(CameraService cameraService)
        {
            _canvas.worldCamera = cameraService.MainCamera;

            UpdateSceneNameText();
        }

        private void UpdateSceneNameText()
        {
            Scene scene = SceneManager.GetActiveScene();

            _levelNameText.text = scene.name;
        }
    }
}