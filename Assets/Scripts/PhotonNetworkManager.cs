using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PhotonNetworkManager : MonoBehaviour {

        public static PhotonNetworkManager instance =null;
        private List<GameObject> roomPrefabs = new List<GameObject>();
        public GameObject roomPrefab;
        public InputField roomName;
        public InputField maxCount;

        void Awake()
        {
            if(instance == null)
            {
                instance=this;
                DontDestroyOnLoad(gameObject.transform);
            }
            else if(instance != this)
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            PhotonNetwork.ConnectUsingSettings("1.0");
            SceneManager.sceneLoaded += (scene,loadscene) =>
            {
                if(SceneManager.GetActiveScene().name == "Game")
                {
                    Spawn();
                }
            };
        }

        void Spawn()
        {
            GameObject g = PhotonNetwork.Instantiate("dragon",new Vector3(0.64f,0.49f,5.18f),Quaternion.identity,0);
            //This part loaded the character into game, we can load different character as long as changing the the first parameter
            //GameObject.FindGameObjectWithTag("MainCamera").GetComponent<>().target = g.transform;
        }

        public void ButtonEvents(string EVENT)
        {
            switch(EVENT)
            {
                case "CreateRoom":
                    if(PhotonNetwork.JoinLobby())
                    {
                        RoomOptions RO =new RoomOptions();
                        RO.MaxPlayers = byte.Parse(maxCount.text);
                        PhotonNetwork.CreateRoom(roomName.text,RO,TypedLobby.Default);
                    }
                    break;

                case "RefreshButton":
                    if(PhotonNetwork.JoinLobby())
                    RefreshRoomList();
                    break;

                case "JoinRandomRoom":
                    if(PhotonNetwork.JoinLobby())
                    JoinRandomRoomButton();
                    break;

            }
        }

        void RefreshRoomList()
        {
            if(roomPrefabs.Count > 0)
            {
                for(int i=0;i < roomPrefabs.Count;i++)
                {
                    Destroy(roomPrefabs[i]);
                }

                roomPrefabs.Clear();
            }

            for(int i=0;i < PhotonNetwork.GetRoomList().Length;i++)
            {
                Debug.Log(PhotonNetwork.GetRoomList()[i].name);
                GameObject g =Instantiate(roomPrefab);
                g.transform.SetParent(roomPrefab.transform.parent);
                g.GetComponent<RectTransform>().localScale =roomPrefab.GetComponent<RectTransform>().localScale;
                g.GetComponent<RectTransform>().position = new Vector3(roomPrefab.GetComponent<RectTransform>().position.x,roomPrefab.GetComponent<RectTransform>().position.y -(i*80),roomPrefab.GetComponent<RectTransform>().position.z);
                g.transform.Find("Room_Name_Text").GetComponent<Text>().text = PhotonNetwork.GetRoomList()[i].name;
                g.transform.Find("Room_Info").GetComponent<Text>().text = PhotonNetwork.GetRoomList()[i].playerCount + "/" + PhotonNetwork.GetRoomList()[i].maxPlayers;
                g.transform.Find("JoinButton").GetComponent<Button>().onClick.AddListener(() => { JoinRoom(g.transform.Find("Room_Name_Text").GetComponent<Text>().text);});
                g.SetActive(true);
                roomPrefabs.Add(g);
            }

        }

        void JoinRandomRoomButton()
        {
            if(PhotonNetwork.GetRoomList().Length > 0)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                Debug.LogError("JoinRandomRoomButton has problem!!");
            }
        }

        void JoinRoom(string roomName)
        {
            bool availableRoom = false;

            foreach(RoomInfo RI in PhotonNetwork.GetRoomList())
            {
                if(roomName == RI.name)
                {
                    availableRoom = true;
                    break;
                }
                else
                {
                    availableRoom = false;
                }
            }

            if(availableRoom)
            {
                PhotonNetwork.JoinRoom(roomName);
            }
            else
            {
                Debug.LogError("JoinRoom has problem!!");
            }
        }

        void OnGUI()
        {
            GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        }

        void OnJoinedLobby()
        {
            Debug.Log("Joined Lobby!");
            Invoke("RefreshRoomList",0.1f);
        }

        void OnPhotonJoinRoomFailed()
        {
            Debug.Log("Join Room Failed!");
        }

		void OnJoinedRoom()
		{
		    Debug.Log("Join Room!");
		    SceneManager.LoadScene("Game");
		}

		void OnCreatedRoom()
        {
            Debug.Log("Created Room!");
        }

}
