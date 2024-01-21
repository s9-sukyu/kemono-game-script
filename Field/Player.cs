using System;
using System.Collections;
using Battle;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Field
{
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        
        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rigidbody;
        private Vector2 _move;
        
        private const float WalkSpeed = 5;
        public delegate void WarpDelegate(int fieldId);

        public WarpDelegate WarpFunc;
        private bool _encounterAble = false;
        public bool movable = true;
        
        async void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            transform.position = DM.PlayerPos;

            SetEncounterAbleFalseForSecs();

            DM.PlayerKemono = await APIHandler.GetUsersKemonosMe();
            animator.SetInteger("Animal", DM.PlayerKemono.Kind);
            animator.SetInteger("Color", DM.PlayerKemono.Color);
        }

        void Update()
        {
            if (movable)
            {
                _move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                if (_move != Vector2.zero)
                {
                    animator.SetFloat("Horizontal", _move.x);
                    animator.SetFloat("Vertical", _move.y);
                }
            }
            else
            {
                _move = Vector2.zero;
            }
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = _move * WalkSpeed;
        }
        
        private int GetfieldId()
        {
            var fieldId = (int)(transform.position.x / 100);
            return fieldId;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!_encounterAble) return;
            if (GetfieldId() != 0) BattleIfEnemy(collision);
            WarpIfWarp(collision);
        }

        // ここに書くのは非自明
        private void BattleIfEnemy(Collision2D collision)
        {
            var enemy = collision.gameObject.GetComponent<Enemy>() as Enemy;
            
            if (enemy != null)
            {
                DM.IsBattle = true;
                DM.EnemyKemonoId = enemy.EnemyId;
                SavePlayerPosition();
                
                SceneManager.LoadScene("Scenes/Battle/Battle");
            }
        }

        private void WarpIfWarp(Collision2D collision)
        {
            var warp = collision.gameObject.GetComponent<WarpData>();

            if (warp != null)
            {
                WarpFunc(warp.fieldId);
                SetEncounterAbleFalseForSecs();
            }
        }
        
        private void SetEncounterAbleFalseForSecs()
        {
            _encounterAble = false;
            _spriteRenderer.color = new Color(1, 1, 1, 0.5f);
            StartCoroutine(SetEncounterAbleTrue());
        }

        IEnumerator SetEncounterAbleTrue()
        {
            yield return new WaitForSeconds(0.5f);
            _encounterAble = true;
            _spriteRenderer.color = new Color(1, 1, 1, 1f);
        }

        public async void SavePlayerPosition()
        {
            DM.PlayerPos = transform.position;
            var fieldId = (int)(transform.position.x / 100);
            await APIHandler.PostUserKemonosPosition(fieldId, (int)transform.position.x, (int)transform.position.y);
        }
    }
}
