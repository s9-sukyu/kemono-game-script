using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Born
{
    public class Born : MonoBehaviour
    {
        [SerializeField] private UIHandler _uiHandler;
        // Start is called before the first frame update
        async void Start()
        {
            foreach (var id in DM.ConceptIds)
            {
                await APIHandler.DeleteKemonoConcept(id);
            }
            
            DM.BornKemono = await APIHandler.PostKemonoGenerate(DM.ConceptsForBear);
            
            _uiHandler.SetUI();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void PressedBecameBorn()
        {
            SceneManager.LoadScene("Scenes/KemoBear");
        }
        
        public void PressedGoField()
        {
            SceneManager.LoadScene("Scenes/Field");
        }
    }
}