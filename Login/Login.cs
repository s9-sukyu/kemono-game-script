using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Login
{
    public class Login : MonoBehaviour
    {
        [SerializeField] private TMP_InputField usernameField;
        [SerializeField] private TMP_InputField passwordField;
        [SerializeField] private TextMeshProUGUI errorText;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public async void PressedLogin()
        {
            // TODO
            var (err, res) = await APIHandler.PostUserLogin(usernameField.text, passwordField.text);
            if (err != null)
            {
                errorText.text = err.Error;
            }
            else
            {
                DM.UserGuid = res.Id;
                var kemonoMe = await APIHandler.GetUsersKemonosMe();
                Debug.Log($"Logined as {DM.UserGuid}");
                DM.PlayerPos = new Vector3(kemonoMe.X, kemonoMe.Y, 0);
                SceneManager.LoadScene("Field");
            }
        }
        
        public async void PressedRegister()
        {
            var (err, res) = await APIHandler.PostUserSignUp(usernameField.text, passwordField.text);
            if (err != null)
            {
                errorText.text = err.Error;
            }
            else
            {
                DM.UserGuid = res.Id;
                Debug.Log($"Logined as {DM.UserGuid}");
                SceneManager.LoadScene("Field");
            }
        }
    }
}
