using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Combine.ShowKemonoForCombine
{ 
    public class ShowKemonos : MonoBehaviour
    {
        [SerializeField] private Texture2D testTex;
        [SerializeField] private GameObject kemonoViewObject;
        [SerializeField] private GameObject kemonoViewRoot;
        [SerializeField] private EventSystem eventSystem;
        private GetKemonoResponse[] _kemonos;
        private Image[] _kemonoImages;
    
        // Start is called before the first frame update
        async void Start()
        {
            _kemonos = await APIHandler.GetKemonosNoImage();
            _kemonoImages = new Image[_kemonos.Length];

            for (var i=0;i<_kemonos.Length;i++)
            {
                var kemono = _kemonos[i];
                
                var kemonoViewInstance = Instantiate(kemonoViewObject, kemonoViewRoot.transform);

                _kemonoImages[i] = kemonoViewInstance.transform.GetChild(0).GetComponent<Image>();
                var nameText = kemonoViewInstance.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
                var hpText = kemonoViewInstance.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
                var attackText = kemonoViewInstance.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
                var defenceText = kemonoViewInstance.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
                var descriptionText = kemonoViewInstance.transform.GetChild(5).GetComponent<TextMeshProUGUI>();
                var conceptText = kemonoViewInstance.transform.GetChild(6).GetComponent<TextMeshProUGUI>();

                // kemonoImageをクリックした際にidが知りたいのでここで入手
                kemonoViewInstance.transform.GetChild(0).name = kemono.Id.ToString();
                
                nameText.text = kemono.Name;
                hpText.text = $"HP {kemono.Hp}/{kemono.Hp}";
                attackText.text = $"こうげき {kemono.Attack}";
                defenceText.text = $"ぼうぎょ {kemono.Defence}";
                descriptionText.text = kemono.Description;
                conceptText.text = String.Join(",", kemono.Concepts);
            }
            
            for (int i = 0; i < _kemonos.Length; i++)
            {
                var kemono = _kemonos[i];
                var kemonoImage = _kemonoImages[i];
                var texture = await APIHandler.GetKemonoImage(kemono.Id);
                
                kemonoImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                kemono.Image = texture.GetRawTextureData();
            }
        }

        IEnumerator TextureTest(Image i, Sprite s)
        {
            yield return new WaitForSeconds(2);
            i.sprite = s;
        }

        // Update is called once per frame
        void Update()
        {
            CheckPressKemono();
        }

        public void PressBackButton()
        {
            SceneManager.LoadScene("Scenes/Combine/KemoCombine");
        }

        void CheckPressKemono()
        {
            // 重ければ実装変更
            if (eventSystem.currentSelectedGameObject != null && eventSystem.currentSelectedGameObject.CompareTag("KemonoView"))
            {
                if (DM.IsSelectingKemono2)
                {
                    DM.SelectedKemono2 = FindKemonoFromGuid(new Guid(eventSystem.currentSelectedGameObject.name));
                }
                else
                {
                    DM.SelectedKemono = FindKemonoFromGuid(new Guid(eventSystem.currentSelectedGameObject.name));
                }
                
                SceneManager.LoadScene("Scenes/Combine/KemoCombine");
            }
        }

        GetKemonoResponse FindKemonoFromGuid(Guid guid)
        {
            // あまりきれいではないかもしれないが……
            foreach (var kemono in _kemonos)
            {
                if (kemono.Id == guid)
                {
                    return kemono;
                }
            }

            return null;
        }
    }
}