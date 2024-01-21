using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ShowKemonos
{
    public class ShowKemonos : MonoBehaviour
    {
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
                
                //kemonoImage.sprite = Util.CreateSpriteFromBytes(kemono.Image);
                nameText.text = kemono.Name;
                hpText.text = $"HP {kemono.Hp}/{kemono.Hp}";
                attackText.text = $"こうげき {kemono.Attack}";
                defenceText.text = $"ぼうぎょ {kemono.Defence}";
                descriptionText.text = kemono.Description;
                conceptText.text = String.Join(",", kemono.Concepts);

                if (kemono.IsPlayer)
                {
                    var kemonoOutline = kemonoViewInstance.transform.GetChild(0).GetComponent<Outline>();
                    kemonoOutline.enabled = true;
                }
            }
            
            for (int i = 0; i < _kemonos.Length; i++)
            {
                var kemono = _kemonos[i];
                var kemonoImage = _kemonoImages[i];
                var texture = await APIHandler.GetKemonoImage(kemono.Id);
                
                kemonoImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                _kemonos[i].Image = texture.GetRawTextureData();
            }
        }

        // Update is called once per frame
        void Update()
        {
            CheckPressKemono();
        }

        public void PressBackButton()
        {
            SceneManager.LoadScene("Scenes/Field");
        }

        void CheckPressKemono()
        {
            // 重ければ実装変更
            if (eventSystem.currentSelectedGameObject != null && eventSystem.currentSelectedGameObject.CompareTag("KemonoView"))
            {
                DM.SelectedKemono = FindKemonoFromGuid(new Guid(eventSystem.currentSelectedGameObject.name));
                SceneManager.LoadScene("Scenes/ShowDetailKemono");
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
