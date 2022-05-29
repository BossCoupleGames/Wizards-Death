using UnityEngine;

namespace _Scripts
{
    public class MainMenuCanvas : StaticInstance<MainMenuCanvas>
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private string sameScene;

        public void PullDown()
        {
            //audio
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        public void StartGame()
        {
            //auido click
            LevelManager.Instance.LoadScene(sameScene);
        }

       
        
    }
}