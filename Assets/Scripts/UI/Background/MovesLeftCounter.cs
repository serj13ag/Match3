using EventArgs;
using Services;
using TMPro;
using UnityEngine;

namespace UI.Background
{
    public class MovesLeftCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _movesLeftText;

        public void Init(IMovesLeftService movesLeftService)
        {
            UpdateMovesLeftText(movesLeftService.MovesLeft);

            movesLeftService.OnMovesLeftChanged += OnMovesLeftChanged;
        }

        private void OnMovesLeftChanged(object sender, MovesLeftChangedEventArgs e)
        {
            UpdateMovesLeftText(e.MovesLeft);
        }

        private void UpdateMovesLeftText(int movesLeft)
        {
            _movesLeftText.text = movesLeft.ToString();
        }
    }
}