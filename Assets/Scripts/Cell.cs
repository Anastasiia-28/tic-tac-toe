using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField]
    private GameObject viewX;
    [SerializeField]
    private GameObject view0;

    private GameManager gameManager;

    public PlayerSign CellState { get; set; } = PlayerSign.None;

    public void Init(GameManager gameManager)
    {
        this.gameManager = gameManager;

        ResetState();
    }

    private void OnMouseDown()
    {
        if (gameManager.GameState != GameState.Gameplay)
            return;

        if (CellState == PlayerSign.None)
        {
            SetState(gameManager.PlayerSign);

            if (!gameManager.CheckIsGameOver())
            {
                gameManager.MakeAIMove();

                gameManager.CheckIsGameOver();
            }
        }
    }

    public bool SetState(PlayerSign cellState)
    {
        if (CellState == PlayerSign.None && cellState != PlayerSign.None)
        {
            CellState = cellState;
            switch (CellState)
            {
                case PlayerSign.X:
                    viewX.SetActive(true);
                    break;
                case PlayerSign.O:
                    view0.SetActive(true);
                    break;
            }
            return true;
        }
        return false;
    }

    public void ResetState()
    {
        CellState = PlayerSign.None;

        viewX.SetActive(false);
        view0.SetActive(false);
    }
}
