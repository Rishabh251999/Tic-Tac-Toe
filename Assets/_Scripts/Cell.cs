using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe
{
    public class Cell : MonoBehaviour
    {
        #region Attributes

        [Header("UI")]
        private Image _image;
        private Button _button;

        [SerializeField] private Cells cell;

        internal Player player;

        #endregion


        #region UnityCallbacks

        private void Awake()
        {
            _image = GetComponent<Image>();
            _button = GetComponent<Button>();

            GameManager.Instance.matchCells.Add(cell, this);

            _button.onClick.AddListener(() => Play(transform.parent.parent.gameObject));
        }


        public void Play(GameObject gameObject)
        {
            if (GameManager.Instance.currentPlayer == Player.Player1 || GameManager.Instance.currentPlayer == Player.Player2)
            {
                if (gameObject.TryGetComponent<VsPlayerGameManager>(out var vsPlayerManager))
                    vsPlayerManager.MakePlay(cell);

                else if (gameObject.TryGetComponent<VsAIGameManager>(out var vsAIGameManager))
                    vsAIGameManager.MakePlay(cell);
            }
        }


        internal void SetPlayer(Player player)
        {
            this.player = player;
            
            if (player == Player.Player1)
                _image.color = Color.blue;

            else if(player == Player.Player2)
                _image.color = Color.red;

            _button.interactable = false;
        }

        #endregion
    }
}
