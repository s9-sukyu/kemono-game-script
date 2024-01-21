using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Field
{
    public class Enemy : MonoBehaviour
    {
        public Guid EnemyId = new Guid("00000000-0000-0000-0000-000000000003");

        private static readonly Vector2[] Direction = new [] {Vector2.up, Vector2.down, Vector2.left, Vector2.right, Vector2.zero, };
        private const float WalkSpeed = 2;

        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Animator animator;
        [SerializeField] private TextMeshPro nameText;

        public bool IsBoss = false;

        void Start()
        {
            StartCoroutine(MoveRoutine());
        }

        private void Update()
        {
            
        }

        public void SetName(string kemonoName)
        {
            nameText.text = kemonoName;
        }

        IEnumerator MoveRoutine()
        {
            for (;;)
            {
                yield return StartCoroutine(Move());
            }
        }
        
        IEnumerator Move()
        {
            yield return new WaitForSeconds(3);
            
            if (IsBoss) yield break;

            var direction = Direction[Random.Range(0, 5)];
            rb.velocity = direction * WalkSpeed;
            animator.SetFloat("Horizontal", direction.x);
            animator.SetFloat("Vertical", direction.y);
            
            yield return new WaitForSeconds(1);
            
            rb.velocity = Vector2.zero;
        }
    }
}
