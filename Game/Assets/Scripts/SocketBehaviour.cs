﻿using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using DotsCore;
using NativeWebSocket;
using UnityEngine;
using UnityEngine.UI;

public class SocketBehaviour : MonoBehaviour
{
    public Text gameStateLabel;
    public Image gameStateLabelContainer;
    public SpriteRenderer boardRenderer;
    public GameInfoUi gameInfoUi;
    
    public ServerConnection Connection = new ServerConnection("ws://localhost:8080");
    
    public ClientState ClientState { get; set; }
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Connecting");
        Connection.Connect();

        boardRenderer.enabled = false;
        gameInfoUi.SetVisible(false);
    }

    // Update is called once per frame
    void Update()
    {
        Connection.Update();

        Connection.ClientStateUpdated += state =>
        {
            ClientState = state;
            if (state.State == ClientState.StateEnum.Matchmaking)
            {
                gameStateLabel.text = "Matchmaking...";
            }
            else if (state.State == ClientState.StateEnum.Playing)
            {
                boardRenderer.enabled = true;
                gameInfoUi.SetVisible(true);
                gameInfoUi.SetNames(state);
                gameStateLabelContainer.gameObject.SetActive(false);
            }
        };
    }
    
    private void OnApplicationQuit()
    {
        Connection.Close();
    }
}
