using UnityEngine;

public class LobbyController : MonoBehaviour
{
    public void Play()
    {
        NakamaController.Instance.JoinMatch();
    }
}
