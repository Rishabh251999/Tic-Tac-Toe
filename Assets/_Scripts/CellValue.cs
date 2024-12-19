using System;
using UnityEngine;

namespace TicTacToe
{
    public enum Cells : ushort
    {
        None,
        A1 = 1 << 0,
        B1 = 1 << 1,
        C1 = 1 << 2,
        A2 = 1 << 3,
        B2 = 1 << 4,
        C2 = 1 << 5,
        A3 = 1 << 6,
        B3 = 1 << 7,
        C3 = 1 << 8,

        // winning combinations
        TopRow = A1 + B1 + C1,
        MidRow = A2 + B2 + C2,
        BotRow = A3 + B3 + C3,
        LeftCol = A1 + A2 + A3,
        MidCol = B1 + B2 + B3,
        RightCol = C1 + C2 + C3,
        Diag1 = A1 + B2 + C3,
        Diag2 = A3 + B2 + C1,

        // board is full (winner / draw)
        Full = TopRow + MidRow + BotRow
    }

    public enum GameMode
    {
        None,
        PlayervsAI,
        PlayerVsPlayer
    }

    public struct PlayerData
    {
        public Cells currentScore;
    }

    public enum Player
    {
        None,
        Player1,
        Player2,
    }
}