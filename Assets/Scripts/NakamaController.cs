using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Nakama;
using Nakama.TinyJson;

public class NakamaController : MonoBehaviour
{
    public static NakamaController Instance;

    private IClient client;
    private ISocket socket;
    private ISession session;
    private IMatchmakerMatched matched;
    private IMatch match;
    private  IUserPresence self;
    public List<IUserPresence> opponents = new List<IUserPresence>();
    private PlayerNetworkController playerNetworkController;

     void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        client = new Client("http", "127.0.0.1", 7350, "defaultkey");
        socket = client.NewSocket();
        socket.Connected += () => Debug.Log("Socket connected");
        socket.Closed += () => Debug.Log("Socket connected");
    }

    public async void Login(string email, string password)
    {
        try
        {
            session = await client.AuthenticateEmailAsync(email, password);
            Debug.Log(session);
            await socket.ConnectAsync(session);
            SceneManager.LoadScene(1);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
            return;
        }
    }

    public async void MatchMake()
    {
        socket.ReceivedMatchmakerMatched += (matched) => {
            this.matched = matched;
            SceneManager.LoadScene(2);
        };
        await socket.AddMatchmakerAsync("*", 2, 2);
    }

    public async void JoinMatch()
    {
        if (matched != null)
        {
            try
            {
                match = await socket.JoinMatchAsync(matched);
                self = match.Self;
                opponents.AddRange(match.Presences);
                SceneManager.LoadScene(3);
            }
            catch (System.Exception e)
            {
                Debug.Log(e.Message);
                return;
            }
        }
    }

    public void UpdatePosition(Transform t)
    {
        var data = new Dictionary<string, float[]>
        {
            { "pos", new float[] {t.position.x, t.position.y, t.position.z} },
            { "rot", new float[] {t.rotation.w, t.rotation.x, t.rotation.y, t.rotation.z}}
        }.ToJson();

        socket.SendMatchStateAsync(match.Id, 1, data);
    }

    public void ShootOpponent(int damage)
    {
        var data = new Dictionary<string, int> { {"damage", damage} }.ToJson();

        socket.SendMatchStateAsync(match.Id, 2, data);
    }

    public void InitGame(PlayerNetworkController pnc)
    {
        playerNetworkController = pnc;
        var enc = System.Text.Encoding.UTF8;
        socket.ReceivedMatchState += newState => {
            var data = enc.GetString(newState.State);
            if (newState.OpCode == 1)
            {
                playerNetworkController.UpdateOpponentsPos(data);
            } else if (newState.OpCode == 2)
            {
                playerNetworkController.transform.GetComponent<Target>().TakeDamageOther(data);
            }
        };
    }

    void OnApplicationQuit()
    {
        socket?.CloseAsync();
    }
}
