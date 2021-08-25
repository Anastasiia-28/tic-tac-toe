using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StartPanel : MonoBehaviour
{
    [SerializeField]
    private Button playerX;
    [SerializeField]
    private Button playerO;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();

        playerX.onClick.AddListener(() => gameManager.SetPlayerSide(PlayerSign.X));
        playerO.onClick.AddListener(() => gameManager.SetPlayerSide(PlayerSign.O));
    }
}
