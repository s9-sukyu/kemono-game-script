using System.Threading.Tasks;
using Field;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Battle
{
    public class Battle : MonoBehaviour
    {
        [SerializeField] private TextHandler textHandler;
        [SerializeField] private GameObject attackButtonObj;
        [SerializeField] private GameObject escapeButtonObj;

        [SerializeField] private Image playerImage;
        [SerializeField] private Image enemyImage;
        [SerializeField] private PhotoTaker photoTaker;

        [SerializeField] private BGMSystem normalBGM;
        [SerializeField] private BGMSystem bossBGM;

        private GetKemonoResponse _playerKemono;
        private GetKemonoResponse _enemyKemono;
        private int turnId = 1;

        async void Start()
        {
            BattleData.State = BattleData.BattleState.PlayerTurnState;

            await LoadKemonos();
            await HandleBattle();

            if (DM.EnemyKemono.IsBoss)
            {
                bossBGM.Play();
            }
            else
            {
                normalBGM.Play();
            }
            
            textHandler.UpdateStatusText();
            
            attackButtonObj.SetActive(true);
            escapeButtonObj.SetActive(true);
        }

        async Task LoadKemonos()
        {
            // TODO 戦うケモノを選ぶ処理が必要

            var (err, playerKemono) = await APIHandler.GetKemonosBattle();
            if (err != null)
            {
                return;
            }

            DM.PlayerKemono = playerKemono;
            if (DM.PlayerKemono == null)
            {
                Debug.Log("PlayerKemono is null");
                return;
            }
            
            DM.EnemyKemono = await APIHandler.GetKemonoByGuid(DM.EnemyKemonoId);
            if (DM.EnemyKemono == null)
            {
                Debug.Log("EnemyKemono is null");
                return;
            }

            playerImage.sprite = Util.CreateSpriteFromBytes(DM.PlayerKemono.Image);
            enemyImage.sprite = Util.CreateSpriteFromBytes(DM.EnemyKemono.Image);
        }

        async Task HandleBattle()
        {
            var res = await APIHandler.PostBattle(DM.PlayerKemono.Id, DM.EnemyKemonoId);
            DM.BattleId = res.BattleId;
        }

        void Update()
        {
            switch (BattleData.State)
            {
                case BattleData.BattleState.PlayerTurnState:
                    PlayerTurn();
                    break;
                case BattleData.BattleState.ReadPlayerTextState:
                    ReadPlayerText();
                    break;
                /*
                case BattleData.BattleState.EnemyTurnState:
                    EnemyTurn();
                    break;
                */
                case BattleData.BattleState.ReadEnemyTextState:
                    ReadEnemyText();
                    break;
            }
        }

        void PlayerTurn()
        {
            // ボタン入力待ち, ボタンの発火にともない行動
            // OnAttackButtonPressed() で処理がおこなわれる
        }
        
        void ReadPlayerText()
        {
            textHandler.ReadText();
            if (textHandler.ReadAllText())
            {
                textHandler.UpdateStatusText();
                BattleData.State = BattleData.BattleState.EnemyTurnState;

                if (DM.EnemyKemono.Hp <= 0)
                {
                    SceneManager.LoadScene("Scenes/Battle/AfterBattle");
                }
                
                EnemyTurn();
            }
        }

        async void EnemyTurn()
        {
            var delta = Random.Range(0.5f, 2f);
            var damage = (int)(DM.EnemyKemono.Attack*delta - DM.PlayerKemono.Defence/2f);
            if (damage < 0) damage = 0;
            DM.PlayerKemono.Hp -= damage;
            
            // APIを叩く
            var res = await APIHandler.PostBattleIdWithDamage(damage);
            
            //textHandler.AddText($"てきの攻撃！\nあなたに {damage} のダメージ");
            textHandler.AddText(res.Text+"\n");
            BattleData.State = BattleData.BattleState.ReadEnemyTextState;

            if (DM.PlayerKemono.Hp <= 0)
            {
                textHandler.AddText("あなたはやられてしまった。\nげーむおーばー。　　　　　　　　");
            }
        }

        void ReadEnemyText()
        {
            textHandler.ReadText();
            if (textHandler.ReadAllText())
            {
                // 分割すると読みやすくなる
                textHandler.UpdateStatusText();
                BattleData.State = BattleData.BattleState.PlayerTurnState;
                attackButtonObj.SetActive(true);
                escapeButtonObj.SetActive(true);
                if (DM.PlayerKemono.Hp <= 0)
                {
                    SceneManager.LoadScene("Scenes/Field");
                }
            }
        }

        /// <summary>
        /// 攻撃ボタンが押されたときの挙動。非同期処理を導入していないので重ければ導入する。
        /// </summary>
        public async void OnAttackButtonPressed()
        {
            textHandler.AddText($"-- Turn {turnId++} --");
            
            attackButtonObj.SetActive(false);
            escapeButtonObj.SetActive(false);

            var delta = Random.Range(0.5f, 2f);
            var damage = (int)(DM.PlayerKemono.Attack*delta - DM.EnemyKemono.Defence/2f);
            if (damage < 0) damage = 0;
            DM.EnemyKemono.Hp -= damage;
            
            // APIを叩く
            var res = await APIHandler.PostBattleIdWithDamage(damage);
            
            // textHandler.AddText($"あなたの攻撃！\nてきに {damage} のダメージ");
            textHandler.AddText(res.Text+"\n");
            BattleData.State = BattleData.BattleState.ReadPlayerTextState;
            
            if (DM.EnemyKemono.Hp <= 0)
            {
                textHandler.AddText("てきを倒した！");
            }
        }
        
        public void OnEscapeButtonPressed()
        {
            // にげる処理
            textHandler.AddText("にげだした。");
            
            // 応答なしになるので、あとまわし
            
            /*
            while (!textHandler.ReadAllText())
            {
                textHandler.UpdateStatus();
            }*/
            
            SceneManager.LoadScene("Scenes/Field");
        }
    }
}
