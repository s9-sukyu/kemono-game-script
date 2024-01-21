using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Field
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private GameObject menuGameObject;
        [SerializeField] private PhotoTaker photoTaker;
        private bool IsMenuOpen = false;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown("Menu"))
            {
                if (IsMenuOpen)
                {
                    CloseMenu();
                }
                else
                {
                    OpenMenu();
                }
            }
        }

        public void ToggleMenu()
        {
            if (IsMenuOpen) CloseMenu();
            else OpenMenu();
        }

        void OpenMenu()
        {
            IsMenuOpen = true;
            _player.movable = false;
            
            menuGameObject.SetActive(true);
        }

        void CloseMenu()
        {
            IsMenuOpen = false;
            _player.movable = true;
            
            menuGameObject.SetActive(false);
        }

        public async void PressedKemonoForBattle()
        {
            var (err, playerKemono) = await APIHandler.GetKemonosBattle();
            if (err != null)
            {
                return;
            }

            DM.SelectedKemono = playerKemono;
            _player.SavePlayerPosition();
            SceneManager.LoadScene("Scenes/ShowDetailKemono");
        }

        public void PressedKemonoList()
        {
            _player.SavePlayerPosition();
            StartCoroutine(LoadKemonoList());
        }

        private IEnumerator LoadKemonoList()
        {
            yield return StartCoroutine(photoTaker.ShotTexture());
            _player.SavePlayerPosition();
            SceneManager.LoadScene("Scenes/ShowKemonos");
        }

        public void PressedKemonoBear()
        {
            _player.SavePlayerPosition();
            SceneManager.LoadScene("Scenes/KemoBear");
        }

        public void PressedKemonoCombine()
        {
            _player.SavePlayerPosition();
            SceneManager.LoadScene("Scenes/Combine/KemoCombine");
        }

        public void PressedLogout()
        {
            _player.SavePlayerPosition();
            SceneManager.LoadScene("Scenes/Login");
        }
    }
}
