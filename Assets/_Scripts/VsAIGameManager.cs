using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe
{
    public class VsAIGameManager : MonoBehaviour
    {
        #region Attributes

        [Header("UI")]
        [SerializeField] internal TextMeshProUGUI _currentPlayerTurnText;
        [SerializeField] internal Button _exitButton;
        [SerializeField] internal Button _restartButton;

        #endregion


        #region UnityCallbacks

        private void Awake()
        {
            _exitButton.onClick.AddListener(() => GameManager.Instance.Exit());
            _restartButton.onClick.AddListener(() => GameManager.Instance.Restart());
        }


        private void Update() => GameManager.Instance.UpdateUI(_currentPlayerTurnText, GameManager.Instance.currentPlayer);


        internal void MakePlay(Cells cell)
        {
            GameManager.Instance.matchCells[cell].player = GameManager.Instance.currentPlayer;

            GameManager.Instance.UpdateCellValue(cell, GameManager.Instance.currentPlayer);

            PlayerData pd = GameManager.Instance._players[GameManager.Instance.currentPlayer];
            pd.currentScore |= cell;
            GameManager.Instance._players[GameManager.Instance.currentPlayer] = pd;

            GameManager.Instance.boardScore |= cell;

            if (GameManager.Instance.CheckWinner(pd.currentScore))
            {
                _currentPlayerTurnText.text = $"{GameManager.Instance.currentPlayer} won!";
                GameManager.Instance.currentPlayer = Player.None;

                foreach (var item in GameManager.Instance.matchCells.Values)
                    item.GetComponent<Button>().interactable = false;
            }

            else if (GameManager.Instance.boardScore == Cells.Full)
            {
                _currentPlayerTurnText.text = "DRAW!";
                GameManager.Instance.currentPlayer = Player.None;
            }

            else
            {
                GameManager.Instance.currentPlayer = GameManager.Instance.currentPlayer == GameManager.Instance._player1 ? GameManager.Instance._player2 : GameManager.Instance._player1;

                if (GameManager.Instance.currentPlayer == GameManager.Instance._player2)
                    StartCoroutine(nameof(AIPlay));
            }
        }

        private IEnumerator AIPlay()
        {
            yield return new WaitForSeconds(1f);

            Cells bestMove = FindBestMove();

            if (bestMove != Cells.None)
                MakePlay(bestMove);
        }

        private Cells FindBestMove()
        {
            foreach (var cell in GameManager.Instance.matchCells)
                if (cell.Value.player == Player.None) return cell.Key;

            return Cells.None;
        }

        #endregion
    }
}