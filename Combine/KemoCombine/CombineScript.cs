using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Combine
{
    // "Combine"だと定義が重複するらしい
    public class CombineScript : MonoBehaviour
    {
        [SerializeField] private EventSystem eventSystem;
        
        // Start is called before the first frame update
        void Start()
        {
            if (DM.SelectedKemono?.Id == DM.SelectedKemono2?.Id) DM.SelectedKemono2 = null;
        }

        // Update is called once per frame
        void Update()
        {
            // 左のケモノの画像をクリック
            if (eventSystem.currentSelectedGameObject != null)
            {
                if (eventSystem.currentSelectedGameObject.CompareTag("KemonoView"))
                {
                    DM.IsSelectingKemono2 = false;
                    SceneManager.LoadScene("Scenes/Combine/ShowKemonosForCombine");
                }
                if (eventSystem.currentSelectedGameObject.CompareTag("KemonoView2"))
                {
                    DM.IsSelectingKemono2 = true;
                    SceneManager.LoadScene("Scenes/Combine/ShowKemonosForCombine");
                }
            }
        }
        
        public void PressBackButton()
        {
            SceneManager.LoadScene("Scenes/Field");
        }
        
        public void PressCombineButton()
        {
            if (DM.SelectedKemono != null && DM.SelectedKemono2 != null)
            {
                SceneManager.LoadScene("Scenes/Combine/KemoAfterCombine");
            }
        }
    }
}
