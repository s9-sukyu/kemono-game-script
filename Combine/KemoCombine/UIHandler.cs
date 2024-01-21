using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Combine
{
    public class UIHandler : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text hpText;
        [SerializeField] private TMP_Text attackText;
        [SerializeField] private TMP_Text defenceText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private TMP_Text conceptText;
        
        [SerializeField] private Image image2;
        [SerializeField] private TMP_Text nameText2;
        [SerializeField] private TMP_Text hpText2;
        [SerializeField] private TMP_Text attackText2;
        [SerializeField] private TMP_Text defenceText2;
        [SerializeField] private TMP_Text descriptionText2;
        [SerializeField] private TMP_Text conceptText2;
        
        // Start is called before the first frame update
        void Start()
        {
            // Note: 使用されたケモノが選択されっぱなしになることのないように！
            SetKemono();
            SetKemono2();
        }

        public void SetKemono()
        {
            if (DM.SelectedKemono == null) return;
            
            image.sprite = Util.CreateSpriteFromRawBytes(DM.SelectedKemono.Image);
            nameText.text = DM.SelectedKemono.Name;
            hpText.text = $"HP {DM.SelectedKemono.Hp}/{DM.SelectedKemono.Hp}";
            attackText.text = $"こうげき {DM.SelectedKemono.Attack}";
            defenceText.text = $"ぼうぎょ {DM.SelectedKemono.Defence}";
            descriptionText.text = DM.SelectedKemono.Description;
            conceptText.text = String.Join(",", DM.SelectedKemono.Concepts);
        }
        
        public void SetKemono2()
        {
            if (DM.SelectedKemono2 == null) return;
            
            image2.sprite = Util.CreateSpriteFromRawBytes(DM.SelectedKemono2.Image);
            nameText2.text = DM.SelectedKemono2.Name;
            hpText2.text = $"HP {DM.SelectedKemono2.Hp}/{DM.SelectedKemono2.Hp}";
            attackText2.text = $"こうげき {DM.SelectedKemono2.Attack}";
            defenceText2.text = $"ぼうぎょ {DM.SelectedKemono2.Defence}";
            descriptionText2.text = DM.SelectedKemono2.Description;
            conceptText2.text = String.Join(",", DM.SelectedKemono2.Concepts);
        }
    }
}
