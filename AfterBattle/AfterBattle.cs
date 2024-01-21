using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AfterBattle
{
    public class AfterBattle : MonoBehaviour
    {
        [SerializeField] private Button catchButton;
        [SerializeField] private TextMeshProUGUI catchButtonText;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public async void PressedCatchButton()
        {
            var res = await APIHandler.IsTruePostKemonosCatch();

            if (res)
            {
                SceneManager.LoadScene("Scenes/Field");
            }
            else
            {
                // 先にだれかが捕まえていたりすると捕まえられない
                catchButton.interactable = false;
                catchButtonText.text = "しっぱいした！";
            }
        }

        public async void PressedConceptButton()
        {
            await APIHandler.PostKemonoIdExtract();
            SceneManager.LoadScene("Scenes/Field");
        }
    }
}
