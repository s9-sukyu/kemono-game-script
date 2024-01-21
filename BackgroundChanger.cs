using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundChanger : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    // Start is called before the first frame update
    void Start()
    {
        if (DM.BackgroundSprite != null)
        {
            backgroundImage.sprite = DM.BackgroundSprite;
            backgroundImage.color = Color.white;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
