using System;
using System.Collections;
using System.Collections.Generic;
using MEC;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pinball {
  public class LuncherPanel : MonoBehaviour {
    [SerializeField]
    TMP_InputField playerNameInput;
    [SerializeField]
    Button playNowButton;
    [SerializeField]
    Luncher luncher;
    [SerializeField]
    GameObject connectPanel, inputPanel;
    [SerializeField]
    TextMeshProUGUI connectingText;
    
    const string PlayerNameKey = "PlayerNameKey";

    private void Awake() {
      string playerName = GetPlayerName();
      if (string.IsNullOrEmpty(playerName)) {
        playNowButton.interactable = false;
      } else {
        playerNameInput.text = playerName;
        playNowButton.interactable = true;
      }
      playerNameInput.onValueChanged.AddListener((value) => {
        playNowButton.interactable = !string.IsNullOrEmpty(value);
      });
      playNowButton.onClick.AddListener(() => {
        SetPlayerName(playerNameInput.text);
        PhotonNetwork.NickName = playerNameInput.text;
        luncher.Connect(connectPanel, inputPanel, connectingText.gameObject);
      });
    }
    
    string GetPlayerName() {
      if (!PlayerPrefs.HasKey(PlayerNameKey)) {
        return "";
      }
      return PlayerPrefs.GetString(PlayerNameKey);
    }
    
    void SetPlayerName(string playerName) {
      PlayerPrefs.SetString(PlayerNameKey, playerName);
    }
  }
}