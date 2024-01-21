using UnityEngine;
using UnityEngine.SceneManagement;

namespace Combine.KemoAfterCombine
{
    public class AfterCombine : MonoBehaviour
    {
        [SerializeField] private Born.UIHandler _uiHandler;
        // Start is called before the first frame update
        async void Start()
        {
            // APIで生成させる
            var (err, bornKemono) = await APIHandler.PostKemonosBreed();
            if (err != null)
            {
                Debug.Log(err);
            }
            else
            {
                DM.BornKemono = bornKemono;
            }

            DM.SelectedKemono = null;
            DM.SelectedKemono2 = null;
            
            // 合成したケモノを表示
            _uiHandler.SetUI();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void PressedCombineAgain()
        {
            SceneManager.LoadScene("Scenes/Combine/KemoCombine");
        }
        
        public void PressedGoField()
        {
            SceneManager.LoadScene("Scenes/Field");
        }
    }
}