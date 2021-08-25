using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerminalPanel : MonoBehaviour
{
    [SerializeField]
    private Text terminalTitle;
    [SerializeField]
    private Button restartButton;

    private GameManager gameManager;

    private const string gameOverText = "{0} Win!";
    private const string standoffText = "Standoff!";

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnEnable()
    {
        restartButton.onClick.AddListener(gameManager.RestartGame);

        gameManager.OnTerminalState += SetTitle;
    }

    private void OnDisable()
    {
        gameManager.OnTerminalState -= SetTitle;
    }

    public void SetTitle(PlayerSign playerSign)
    {
        string result = "";
        switch (playerSign)
        {
            case PlayerSign.X:
            case PlayerSign.O:
                result = string.Format(gameOverText, playerSign);
                break;
            case PlayerSign.None:
                result = standoffText;
                break;
        }
        terminalTitle.text = result;
    }
}
