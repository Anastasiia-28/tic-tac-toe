using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { None, Start, Gameplay, Terminal }
public enum PlayerSign { None, X, O }
public class GameManager : MonoBehaviour
{
    public event Action<GameState> OnGameStateChange = null;
    public event Action<PlayerSign> OnTerminalState = null;

    [SerializeField]
    private List<Cell> cells;

    private int maxDeph = 5;

    private Cell minimaxMove;
    private Cell[,] gameBoard;

    private GameState gameState;

    public PlayerSign PlayerSign { get; set; } = PlayerSign.None;
    public PlayerSign ComputerSign { get; set; } = PlayerSign.None;
    public GameState GameState
    {
        get
        {
            return gameState;
        }
        set
        {
            gameState = value;
            OnGameStateChange(gameState);
        }
    }

    public void Start()
    {
        GameState = GameState.Start;

        gameBoard = new Cell[3, 3];
        int index = 0;

        Cell cell;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                cell = cells[index];

                cell.Init(this);
                gameBoard[i, j] = cell;

                index++;
            }
        }
    }

    public bool CheckIsStandoff()
    {
        foreach (Cell cell in gameBoard)
            if (cell.CellState == PlayerSign.None)
                return false;
        return true;
    }

    public bool CheckIsGameOver()
    {
        if (IsGameOver(PlayerSign.X))
        {
            SetTerminalState(PlayerSign.X);
            return true;
        }
        else if (IsGameOver(PlayerSign.O))
        {
            SetTerminalState(PlayerSign.O);
            return true;
        }
        else if (CheckIsStandoff())
        {
            SetTerminalState(PlayerSign.None);
            return true;
        }

        return false;

        void SetTerminalState(PlayerSign sign)
        {
            GameState = GameState.Terminal;
            OnTerminalState(sign);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetPlayerSide(PlayerSign playerSign)
    {
        PlayerSign = playerSign;
        ComputerSign = playerSign == PlayerSign.X ? PlayerSign.O : PlayerSign.X;
        GameState = GameState.Gameplay;
    }

    public void MakeAIMove()
    {
        MiniMax(0, true, int.MinValue, int.MaxValue);
        minimaxMove.SetState(ComputerSign);
    }

    private bool IsGameOver(PlayerSign type)
    {
        int row = 0;
        for (int i = 0; i < 3; i++)
        {
            if (gameBoard[row, i].CellState == type)
            {
                if (i == 2) return true;
            }
            else
            {
                i = -1;
                row++;

                if (row == 3) break;
            }
        }

        int col = 0;
        for (int i = 0; i < 3; i++)
        {
            if (gameBoard[i, col].CellState == type)
            {
                if (i == 2) return true;
            }
            else
            {
                i = -1;
                col++;

                if (col == 3) break;
            }
        }

        if (gameBoard[0, 0].CellState == type && gameBoard[1, 1].CellState == type && gameBoard[2, 2].CellState == type)
            return true;

        if (gameBoard[0, 2].CellState == type && gameBoard[1, 1].CellState == type && gameBoard[2, 0].CellState == type)
            return true;

        return false;
    }

    private int MiniMax(int depth, bool isMaximizer, int alpha, int beta)
    {
        if (IsGameOver(PlayerSign.X))
            return -1;
        if (IsGameOver(PlayerSign.O))
            return 1;
        if (CheckIsStandoff())
            return 0;
        if (depth >= maxDeph)
            return 0;

        int bestValue;
        List<int> scores = new List<int>();
        List<Cell> moves = new List<Cell>();

        if (isMaximizer)
        {
            bestValue = int.MinValue;

            foreach (Cell cell in gameBoard)
            {
                if (cell.CellState == PlayerSign.None)
                {
                    cell.SetState(ComputerSign);

                    int score = MiniMax(depth + 1, false, alpha, beta);

                    scores.Add(score);
                    moves.Add(cell);

                    cell.ResetState();

                    if (score >= bestValue)
                    {
                        bestValue = score;
                    }

                    alpha = Math.Max(bestValue, alpha);

                    if (alpha >= beta)
                        break;
                }
            }

            minimaxMove = moves[scores.IndexOf(scores.Max())];
            return bestValue;
        }
        else
        {
            bestValue = int.MaxValue;

            foreach (Cell cell in gameBoard)
            {
                if (cell.CellState == PlayerSign.None)
                {
                    cell.SetState(PlayerSign);

                    int score = MiniMax(depth + 1, true, alpha, beta);

                    cell.ResetState();

                    if (score <= bestValue)
                    {
                        bestValue = score;
                    }

                    beta = Math.Min(bestValue, beta);

                    if (alpha >= beta)
                        break;
                }
            }
            return bestValue;
        }
    }
}
