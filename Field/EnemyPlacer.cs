using System;
using System.Collections.Generic;
using UnityEngine;

namespace Field
{
    public class EnemyPlacer : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Texture2D texture;
        [SerializeField] private GameObject enemyPrefab;

        private List<GameObject> _placedKemonos = new List<GameObject>();
        
        // Start is called before the first frame update
        void Start()
        {
            
        }

        public async void PlaceKemono(int fieldId)
        {
            _placedKemonos = new List<GameObject>();
            
            var kemonos = await APIHandler.GetKemonosByFieldId(fieldId);
            if (kemonos == null)
            {
                Debug.Log("kemono is null");
                return;
            }
        
            foreach(var kemono in kemonos)
            {
                // インスタンスの生成。プレハブを作る
                var newEnemy = Instantiate(enemyPrefab);
                Enemy enemyScript = newEnemy.GetComponent<Enemy>();

                if (enemyScript == null)
                {
                    Debug.Log("Could not find enemy script");
                    continue;
                }

                enemyScript.EnemyId = kemono.Id;
                enemyScript.IsBoss = kemono.IsBoss;
                if (kemono.IsPlayer && !kemono.HasChild)
                {
                    if (kemono.OwnerId == DM.UserGuid) continue;
                    
                    var userName = await APIHandler.GetUsers(kemono.OwnerId);
                    enemyScript.SetName(userName.Name);
                }
                newEnemy.name = kemono.Id.ToString().Substring(0, 8);
                newEnemy.transform.position = new Vector3(kemono.X, kemono.Y, 0f);
            
                // スプライトの選択をする
                var animator = newEnemy.GetComponent<Animator>();
                animator.SetInteger("Animal", kemono.Kind);
                animator.SetInteger("Color", kemono.Color);
                
                _placedKemonos.Add(newEnemy);
            }
        }

        public void RemovePreviousKemono()
        {
            if (_placedKemonos == null) return;
            
            foreach (var kemono in _placedKemonos)
            {
                Destroy(kemono);
            }

            _placedKemonos = new List<GameObject>();
        }
    }
}
