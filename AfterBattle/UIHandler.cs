using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AfterBattle
{
    public class UIHandler : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text hpText;
        //[SerializeField] private Transform hpBar;
        [SerializeField] private TMP_Text attackText;
        [SerializeField] private TMP_Text defenceText;
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private TMP_Text conceptText;
        
        // Start is called before the first frame update
        void Start()
        {
            image.sprite = Util.CreateSpriteFromBytes(DM.EnemyKemono.Image);
            nameText.text = DM.EnemyKemono.Name;
            hpText.text = $"HP {DM.EnemyKemono.Hp}/{DM.EnemyKemono.MaxHp}";
            //hpBar.localScale = new Vector3((float)DM.PlayerKemono.Hp / DM.PlayerKemono.MaxHp, 1, 1);
            attackText.text = $"こうげき {DM.EnemyKemono.Attack}";
            defenceText.text = $"ぼうぎょ {DM.EnemyKemono.Defence}";
            descriptionText.text = DM.EnemyKemono.Description;
            conceptText.text = String.Join(",", DM.EnemyKemono.Concepts);
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
