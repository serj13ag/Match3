using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundUi : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private TMP_Text _levelNameText; // TODO move to ui controller

    public void Init()
    {
        _canvas.worldCamera = Camera.main;

        UpdateSceneNameText();
    }

    private void UpdateSceneNameText()
    {
        Scene scene = SceneManager.GetActiveScene();

        _levelNameText.text = scene.name;
    }
}