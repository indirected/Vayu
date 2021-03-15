using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//using UnityEngine.Networking;

namespace Prototype.NetworkLobby
{
    //Main menu, mainly only a bunch of callback called by the UI (setup throught the Inspector)
    public class LobbyMainMenu : MonoBehaviour 
    {
        public LobbyManager lobbyManager;

        public RectTransform lobbyServerList;
        public RectTransform lobbyPanel;      
        public InputField ipInput;
        public InputField matchNameInput;
        public Button AutoJoinButton;
        public GameObject JoinSubPanel;
        public Text SearchTxt;
        public Text SearchingTxt;
        public void OnEnable()
        {
            lobbyManager.topPanel.ToggleVisibility(true);
            ipInput.onEndEdit.RemoveAllListeners();
            ipInput.onEndEdit.AddListener(onEndEditIP);
            //matchNameInput.onEndEdit.RemoveAllListeners();
            //matchNameInput.onEndEdit.AddListener(onEndEditGameName);
        }

        public void OnClickHost()
        {
            lobbyManager.StartHost();
            lobbyManager.Discovery.broadcastData = lobbyManager.networkPort.ToString();
            //lobbyManager.Discovery.StopBroadcast();
            lobbyManager.Discovery.Initialize();
            lobbyManager.Discovery.StartAsServer();
        }

        public void OnClickAutoJoin()
        {
            SearchTxt.enabled = false;
            SearchingTxt.enabled = true;
            AutoJoinButton.interactable = false;
            lobbyManager.Discovery.Initialize();
            lobbyManager.Discovery.StartAsClient();
            StartCoroutine(ReEnableJoinButton());
        }
        public void OnClickJoin()
        {
            CreateClient(ipInput.text);
            FindObjectOfType<LobbyManager>().StartGameButton.gameObject.SetActive(false);
        }
        public void OnClickDedicated()
        {
            lobbyManager.ChangeTo(null);
            lobbyManager.StartServer();
            lobbyManager.backDelegate = lobbyManager.StopServerClbk;
            lobbyManager.SetServerInfo("Dedicated Server", lobbyManager.networkAddress);
        }

        public void OnClickCreateMatchmakingGame()
        {
            lobbyManager.StartMatchMaker();
            lobbyManager.matchMaker.CreateMatch(
                matchNameInput.text,
                (uint)lobbyManager.maxPlayers,
                true,
				"", "", "", 0, 0,
				lobbyManager.OnMatchCreate);

            lobbyManager.backDelegate = lobbyManager.StopHost;
            lobbyManager._isMatchmaking = true;
            lobbyManager.DisplayIsConnecting();

            lobbyManager.SetServerInfo("Matchmaker Host", lobbyManager.matchHost);
        }

        public void OnClickOpenServerList()
        {
            lobbyManager.StartMatchMaker();
            lobbyManager.backDelegate = lobbyManager.SimpleBackClbk;
            lobbyManager.ChangeTo(lobbyServerList);
        }

        void onEndEditIP(string text)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OnClickJoin();
            }
        }

        void onEndEditGameName(string text)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OnClickCreateMatchmakingGame();
            }
        }
        public void CreateClient(string Address)
        {
            lobbyManager.ChangeTo(lobbyPanel);
            lobbyManager.networkAddress = Address;
            lobbyManager.StartClient();
            lobbyManager.backDelegate = lobbyManager.StopClientClbk;
            lobbyManager.DisplayIsConnecting();
            lobbyManager.SetServerInfo("Connecting...", lobbyManager.networkAddress);
        }
        IEnumerator ReEnableJoinButton()
        {
            yield return new WaitForSecondsRealtime(10);
            SearchTxt.enabled = true;
            SearchingTxt.enabled = false;
            //AutoJoinButton.GetComponentInChildren<Text>().text = Fa.faConvert("?????");
            AutoJoinButton.interactable = true;
            lobbyManager.Discovery.StopBroadcast();
        }
        public void OnClickJoinButton()
        {
            JoinSubPanel.SetActive(true);
        }
        public void OnClickBack()
        {
            JoinSubPanel.SetActive(false);
            if (SearchingTxt.enabled)
            {
                AutoJoinButton.interactable = true;
                lobbyManager.Discovery.StopBroadcast();
                SearchTxt.enabled = true;
                SearchingTxt.enabled = false;
            }

        }
    }
}
