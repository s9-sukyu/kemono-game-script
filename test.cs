using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private RenderTexture renderTexture;
    [SerializeField] private GameObject testPrefab;
    private bool grab = true;
    private Camera _mainCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            grab = true;
    }

    private void OnPostRender()
    {
        Debug.Log("onpostrenderer");
        if (grab)
        {
            var currentRT = RenderTexture.active;
            //RenderTexture.active = renderTexture;
            _mainCamera.targetTexture = renderTexture;
            
            Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
            texture.Apply();
        
            spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, Screen.width, Screen.height), Vector2.zero);
            // spriteRenderer.material.mainTexture = texture;
            grab = false;
            Debug.Log("done");

            //RenderTexture.active = currentRT;
            _mainCamera.targetTexture = currentRT;
            
            /*
            var obj = Instantiate(testPrefab);
            var sr = obj.GetComponent<SpriteRenderer>();
            sr.sprite = null;
            sr.color = Color.blue;
            */
            // sr.sprite = Sprite.Create(texture, new Rect(0, 0, Screen.width, Screen.height), Vector2.zero);
        }
    }
}
