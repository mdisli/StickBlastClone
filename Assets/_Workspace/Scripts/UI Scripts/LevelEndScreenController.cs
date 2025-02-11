using _Workspace.Scripts.Managers;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Workspace.Scripts.UI_Scripts
{
    public class LevelEndScreenController : MonoBehaviour
    {
        #region Variables

        [SerializeField] private TextMeshProUGUI levelEndTxt;
        [SerializeField] private TextMeshProUGUI btnTxt;
        [SerializeField] private Button levelEndBtn;
        [SerializeField] private CanvasGroup levelEndScreen;

        #endregion

        public void Initialize(bool isWin)
        {
            levelEndTxt.text = isWin ? "Level Completed!" : "Level Failed!";
            levelEndBtn.onClick.AddListener(isWin ? NextLevel : RetryLevel);
            btnTxt.text = isWin ? "Next Level" : "Retry";
            levelEndScreen.DOFade(1, .35f).SetEase(Ease.Linear);
        }

        private void NextLevel()
        {
            PlayerPrefsManager.SetCurrentLevel(PlayerPrefsManager.GetCurrentLevel() + 1);
            SceneManager.LoadScene(0);
        }
        
        private void RetryLevel()
        {
            SceneManager.LoadScene(0);
        }
    }
}