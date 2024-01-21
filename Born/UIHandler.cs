using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Born
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
        
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void SetUI()
        {
            image.sprite = Util.CreateSpriteFromBytes(DM.BornKemono.Image);
            nameText.text = DM.BornKemono.Name;
            hpText.text = $"HP {DM.BornKemono.Hp}/{DM.BornKemono.Hp}";
            attackText.text = $"こうげき {DM.BornKemono.Attack}";
            defenceText.text = $"ぼうぎょ {DM.BornKemono.Defence}";
            descriptionText.text = DM.BornKemono.Description;
            conceptText.text = String.Join(",", DM.BornKemono.Concepts);
        }
    }
}
