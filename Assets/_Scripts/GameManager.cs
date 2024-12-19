using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TicTacToe
{
    public class GameManager : MonoBehaviour
    {
        #region Attributes

        public static GameManager Instance { get; private set; }

        internal readonly Dictionary<Player, PlayerData> _players = new();

        internal readonly Dictionary<Cells, Cell> matchCells = new();

        [Header("GameObject")]
        [SerializeField] private GameObject _vsAIBoard;
        [SerializeField] private GameObject _vsPlayerBoard;
        [Space(5)]
        [SerializeField] private GameObject _menu;
        [SerializeField] private GameObject _loadingPanel;

        [Header("UI")]
        [SerializeField] private Button _vsAIButton;
        [SerializeField] private Button _vsPlayerButton;

        internal Cells boardScore = Cells.None;

        private GameMode _gameMode = GameMode.None;

        internal Player _player1;
        internal Player _player2;

        internal Player _startingPlayer;
        internal Player currentPlayer;

        #endregion


        #region UnityCallbacks

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);

            else
                Instance = this;
        }

        private void Start()
        {
            StartCoroutine(Loading(2.0f));

            _vsAIButton.onClick.AddListener(() => StartCoroutine(SelectGameMode(TicTacToe.GameMode.PlayervsAI)));
            _vsPlayerButton.onClick.AddListener(() => StartCoroutine(SelectGameMode(TicTacToe.GameMode.PlayerVsPlayer)));
        }


        private IEnumerator SelectGameMode(GameMode mode)
        {
            yield return new WaitForSeconds(0.75f);

            _menu.SetActive(false);

            switch (mode)
            {
                case GameMode.PlayervsAI:
                    _gameMode = GameMode.PlayervsAI;
                    _vsAIBoard.SetActive(true);

                    break;

                case GameMode.PlayerVsPlayer:
                    _gameMode = GameMode.PlayerVsPlayer;
                    _vsPlayerBoard.SetActive(true);

                    break;
            }

            StartCoroutine(nameof(StartPlayersData));
        }


        private IEnumerator Loading(float time)
        {
            yield return new WaitForSeconds(time);

            _loadingPanel.SetActive(false); 
        }


        internal IEnumerator StartPlayersData()
        {
            _player1 = Player.Player1;
            _player2 = Player.Player2;

            _startingPlayer = _player1;
            currentPlayer = _player1;

            yield return new WaitForSeconds(0.5f);

            _players.Add(_player1, new PlayerData() { currentScore = Cells.None });
            _players.Add(_player2, new PlayerData() { currentScore = Cells.None });
        }


        internal void UpdateUI(TextMeshProUGUI currentPlayerTurnText, Player currentPlayer)
        {
            if (currentPlayer == Player.None) return;

            currentPlayerTurnText.text = $"{currentPlayer}'s turn";
            currentPlayerTurnText.color = currentPlayer == Player.Player1 ? Color.blue : Color.red;
        }


        internal void UpdateCellValue(Cells cell, Player player) => matchCells[cell].SetPlayer(player);


        internal bool CheckWinner(Cells cell)
        {
            switch (cell)
            {
                case var _ when (cell & Cells.TopRow) == Cells.TopRow:
                case var _ when (cell & Cells.MidRow) == Cells.MidRow:
                case var _ when (cell & Cells.BotRow) == Cells.BotRow:
                case var _ when (cell & Cells.LeftCol) == Cells.LeftCol:
                case var _ when (cell & Cells.MidCol) == Cells.MidCol:
                case var _ when (cell & Cells.RightCol) == Cells.RightCol:
                case var _ when (cell & Cells.Diag1) == Cells.Diag1:
                case var _ when (cell & Cells.Diag2) == Cells.Diag2:
                    return true;

                default:
                    return false;
            }
        }


        private void ResetBoard()
        {
            foreach (var cell in matchCells.Values)
            {
                cell.SetPlayer(Player.None);

                var image = cell.GetComponent<Image>();
                var button = cell.GetComponent<Button>();

                image.color = new Color(1, 1, 1, 0.125f);
                button.interactable = true;
            }

            boardScore = Cells.None;
            _players.Clear();
        }


        internal void Restart()
        {
            ResetBoard();

            StartCoroutine(StartPlayersData());

            currentPlayer = _startingPlayer = _player1;
        }


        internal void Exit()
        {
            ResetBoard();

            _player1 = _player2 = Player.None;
            currentPlayer = _startingPlayer = Player.None;

            if(_gameMode == GameMode.PlayervsAI)
                _vsAIBoard.SetActive(false);

            else if (_gameMode == GameMode.PlayerVsPlayer)
                _vsPlayerBoard.SetActive(false);

            _menu.SetActive(true);
        }

        #endregion
    }
}