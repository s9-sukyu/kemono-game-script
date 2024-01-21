using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Bear {
    public class Bear : MonoBehaviour
    {
        //private List<Toggle> _toggles = new List<Toggle>();
        private List<GameObject> _togglesGameObjects = new List<GameObject>();
        [SerializeField] private Transform toggleRoot;
        [SerializeField] private GameObject togglePrefab;
        
        // Start is called before the first frame update
        async void Start()
        {
            var concepts = await APIHandler.GetKemonoConcepts();

            foreach (var concept in concepts)
            {
                var toggleInstance = Instantiate(togglePrefab, toggleRoot.transform);
                
                var conceptText = toggleInstance.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
                conceptText.text = concept.Concept;
                
                var conceptData = toggleInstance.GetComponent<ConceptData>();
                conceptData.Id = concept.Id;
                conceptData.Concept = concept.Concept;
                _togglesGameObjects.Add(toggleInstance);
            }
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void BearButton()
        {
            DM.ConceptIds = new List<Guid>();
            
            var conceptsString = "";
            foreach (var toggleGameObjects in _togglesGameObjects)
            {
                var toggle = toggleGameObjects.GetComponent<Toggle>();
                if (toggle.isOn)
                {
                    var conceptData = toggleGameObjects.GetComponent<ConceptData>();
                    if (conceptsString == "") conceptsString += conceptData.Concept;
                    else conceptsString += "," + conceptData.Concept;
                    
                    DM.ConceptIds.Add(conceptData.Id);
                }
            }

            DM.ConceptsForBear = conceptsString;
            SceneManager.LoadScene("Scenes/KemoBorn");
        }

        public void BackButton()
        {
            SceneManager.LoadScene("Scenes/Field");
        }
    }
}