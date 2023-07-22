using System.Collections.Generic;
using Fusion;
using Network;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MainMenuUIHandler : MonoBehaviour
{
    [FormerlySerializedAs("inputField")] public TMP_InputField nameInputField;
    public TMP_InputField sessionInputField;

    public List<SessionInfo> SessionInfos = new List<SessionInfo>();

    private void Start()
    {
        if (PlayerPrefs.HasKey("PlayerNickname"))
        {
            nameInputField.text = PlayerPrefs.GetString("PlayerNickname");
        }
        NetworkRunnerHandler networkRunnerHandler = FindObjectOfType<NetworkRunnerHandler>();
        networkRunnerHandler.OnJoinLobby();
    }

    public void OnJoinGameClicked()
    {
        PlayerPrefs.SetString("PlayerNickname", nameInputField.text);
        PlayerPrefs.Save();

        NetworkRunnerHandler networkRunnerHandler = FindObjectOfType<NetworkRunnerHandler>();
        
        networkRunnerHandler.JoinGame(sessionInputField.text);
    }

    public void OnCreateNewGameClicked()
    {
        PlayerPrefs.SetString("PlayerNickname", nameInputField.text);
        PlayerPrefs.Save();
        NetworkRunnerHandler networkRunnerHandler = FindObjectOfType<NetworkRunnerHandler>();

        networkRunnerHandler.CreateGame(sessionInputField.text, "Game");
    }
}
