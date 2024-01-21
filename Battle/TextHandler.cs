using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Battle
{
    public class TextHandler : MonoBehaviour
    {
        [SerializeField] private TMP_Text mainText;
        [SerializeField] private TMP_Text playerNameText;
        [SerializeField] private TMP_Text enemyNameText;
        [SerializeField] private TMP_Text playerHpText;
        [SerializeField] private TMP_Text enemyHpText;
        [SerializeField] private Transform playerHpBar;
        [SerializeField] private Transform enemyHpBar;
        [SerializeField] private TMP_Text playerAttackText;
        [SerializeField] private TMP_Text enemyAttackText;
        [SerializeField] private TMP_Text playerDefenceText;
        [SerializeField] private TMP_Text enemyDefenceText;

        [SerializeField] private ScrollRect scrollRect;
        private ContentSizeFitter _contentSizeFitter;

        private const float ReadSpeed = 40;
        private const float ReadSpeedFast = 100;
        private float _visibleCharacters = 0;

        public bool ReadAllText() => (int)_visibleCharacters >= mainText.text.Length;
    
        void Start()
        {
            mainText.maxVisibleCharacters = 0;
            scrollRect.verticalNormalizedPosition = 0;
            _contentSizeFitter = mainText.GetComponent<ContentSizeFitter>();
        }

        public void UpdateStatusText()
        {
            playerNameText.text = DM.PlayerKemono.Name;
            enemyNameText.text = DM.EnemyKemono.Name;
            playerHpText.text = $"HP {DM.PlayerKemono.Hp}/{DM.PlayerKemono.MaxHp}";
            enemyHpText.text = $"HP {DM.EnemyKemono.Hp}/{DM.EnemyKemono.MaxHp}";
            playerHpBar.localScale = new Vector3((float)DM.PlayerKemono.Hp / DM.PlayerKemono.MaxHp, 1, 1);
            enemyHpBar.localScale = new Vector3((float)DM.EnemyKemono.Hp / DM.EnemyKemono.MaxHp, 1, 1);
            playerAttackText.text = $"こうげき {DM.PlayerKemono.Attack}";
            enemyAttackText.text = $"こうげき {DM.EnemyKemono.Attack}";
            playerDefenceText.text = $"ぼうぎょ {DM.PlayerKemono.Defence}";
            enemyDefenceText.text = $"ぼうぎょ {DM.EnemyKemono.Defence}";
        }

        public void AddText(string text)
        {
            if (mainText.text == "")
            {
                mainText.text += text;
            }
            else
            {
                mainText.text += "\n" + text;
            }
            _contentSizeFitter.SetLayoutVertical();
        }

        public void ReadText()
        {
            _visibleCharacters += Time.deltaTime * ReadSpeed;
            mainText.maxVisibleCharacters = (int)_visibleCharacters;
        }
    }
}
