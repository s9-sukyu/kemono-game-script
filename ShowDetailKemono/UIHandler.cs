using System;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ShowDetailKemono
{
    public class UIHandler : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text isMeText;
        [SerializeField] private TMP_Text isForBattleText;
        [SerializeField] private TMP_Text hpText;
        [SerializeField] private TMP_Text attackText;
        [SerializeField] private TMP_Text defenceText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private TMP_Text conceptText;
        
        // Start is called before the first frame update
        void Start()
        {
            if (DM.SelectedKemono == null)
            {
                Debug.Log("selected kemono is empty");
                return;
            }
            
            image.sprite = Util.CreateSpriteFromRawBytes(DM.SelectedKemono.Image);
            nameText.text = DM.SelectedKemono.Name;
            if (DM.SelectedKemono.IsPlayer)
            {
                isMeText.text = "自分";
            }

            if (DM.SelectedKemono.IsForBattle)
            {
                isForBattleText.text = "戦闘に出す";
            }
            hpText.text = $"HP {DM.SelectedKemono.Hp}/{DM.SelectedKemono.Hp}";
            attackText.text = $"こうげき {DM.SelectedKemono.Attack}";
            defenceText.text = $"ぼうぎょ {DM.SelectedKemono.Defence}";
            descriptionText.text = DM.SelectedKemono.Description;
            conceptText.text = String.Join(",", DM.SelectedKemono.Concepts);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void PressBackButton()
        {
            DM.SelectedKemono = null;
            SceneManager.LoadScene("Scenes/ShowKemonos");
        }

        public async void PressForBattleButton()
        {
            var err = await APIHandler.PostKemonosBattle(DM.SelectedKemono.Id);
            if (err != null)
            {
                Debug.Log(err.Error);
            }
            else
            {
                isForBattleText.text = "戦闘に出す";
                // DM.SelectedKemono = null;
                // SceneManager.LoadScene("Scenes/ShowKemonos");
            }
        }

        public void PressDebugButton()
        {
            Debug.Log(DM.SelectedKemono.Id);
        }
    }
}