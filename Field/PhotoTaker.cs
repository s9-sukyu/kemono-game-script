using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Field
{
    public class PhotoTaker : MonoBehaviour
    {
        [SerializeField] private RenderTexture renderTexture;
        [SerializeField] private Transform tra;
        [SerializeField] private GameObject obj;
        private Camera _mainCamera;
        
        private bool needShot = false;
        
        // Start is called before the first frame update
        void Start()
        {
            _mainCamera = GetComponent<Camera>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        
        private void OnPostRender()
        {
            if (needShot)
            {
                var renderTextureActive = RenderTexture.active;
                _mainCamera.targetTexture = DM.RenderTex;
                Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
                texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
                texture.Apply();
                DM.BackgroundSprite = Sprite.Create(texture, new Rect(0, 0, Screen.width, Screen.height), Vector2.zero);
                _mainCamera.targetTexture = renderTextureActive;

                needShot = false;
            }
        }

        public IEnumerator ShotTexture()
        {
            needShot = true;
            yield return new WaitWhile(() => needShot);
        }
    }
}
