using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private StartPanel startPanel;
    [SerializeField]
    private HUDPanel hudPanel;
    [SerializeField]
    private TerminalPanel terminalPanel;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnEnable()
    {
        gameManager.OnGameStateChange += SetUIState;
    }

    private void OnDisable()
    {
        gameManager.OnGameStateChange -= SetUIState;
    }

    public void SetUIState(GameState state)
    {
        startPanel.gameObject.SetActive(state == GameState.Start);
        hudPanel.gameObject.SetActive(state == GameState.Gameplay);
        terminalPanel.gameObject.SetActive(state == GameState.Terminal);
    }
}
