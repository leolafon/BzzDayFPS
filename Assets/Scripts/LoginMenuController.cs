using UnityEngine;
using UnityEngine.UI;

public class LoginMenuController : MonoBehaviour
{
    [SerializeField] private InputField email;
    [SerializeField] private InputField password;

    public void Login()
    {
        NakamaController.Instance.Login(email.text, password.text);
    }
}
