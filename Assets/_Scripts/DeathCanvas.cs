using UnityEngine;

namespace _Scripts
{
    public class DeathCanvas : StaticInstance<DeathCanvas>
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private string sameScene;
        [SerializeField] private string mainMenu;

        public void PullUp()
        {
            //audio
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        public void TryAgain()
        {
            //auido click
            LevelManager.Instance.LoadScene(sameScene);
        }

        public void Quit()
        {
            // audio click
            LevelManager.Instance.LoadScene(mainMenu);
        }
    } 
}