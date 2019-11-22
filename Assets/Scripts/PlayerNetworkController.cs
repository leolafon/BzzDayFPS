using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Nakama.TinyJson;

public class PlayerNetworkController : MonoBehaviour
{
    private NakamaController n;
    private Text debugText;
    private Text totoText;
    private GameObject opponent;
    public GameObject opponentPrefab;

    // Start is called before the first frame update
    void Start()
    {
        n = NakamaController.Instance;
        n.InitGame(this);
        debugText = GameObject.Find("DebugText").GetComponent<Text>();
        totoText = GameObject.Find("TotoText").GetComponent<Text>();
    }

    void Update()
    {
        if (n != null) {
            n.UpdatePosition(transform);
        }
    }

    public void UpdateOpponentsPos(string data)
    {
        Debug.LogFormat("Received {0}", data);
        debugText.text = data;

        try
        {
            var toto = data.FromJson<Dictionary<string, float[]>>();

            var pos = new Vector3(toto["pos"][0], toto["pos"][1], toto["pos"][2]);
            var rot = new Quaternion(toto["rot"][0], toto["rot"][1], toto["rot"][2], toto["rot"][3]);
            if (opponent == null) {
                totoText.text = "Instantiate opponent";
                opponent = Instantiate(opponentPrefab, new Vector3(pos.x, pos.y + 5, pos.z), rot);
            } else {
                totoText.text = "Update opponent pos";
                opponent.transform.position = Vector3.Lerp(opponent.transform.position, pos, Time.deltaTime);
                // opponent.transform.rotation = Quaternion.Slerp(opponent.transform.rotation, rot, Time.deltaTime);
            }
        }
        catch (System.Exception e)
        {
            totoText.text = e.Message;
        }
    }
}
