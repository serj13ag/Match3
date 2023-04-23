using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _levelNameText;

        public void UpdateSceneNameText()
        {
            Scene scene = SceneManager.GetActiveScene();

            _levelNameText.text = scene.name;
        }
    }
}