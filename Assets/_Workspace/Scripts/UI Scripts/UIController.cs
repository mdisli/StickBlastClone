using System;
using _Workspace.Scripts.Board_Scripts;
using _Workspace.Scripts.Diamond;
using _Workspace.Scripts.Level_Scripts;
using _Workspace.Scripts.Managers;
using _Workspace.Scripts.SO_Scripts;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Workspace.Scripts.UI_Scripts
{
    public class UIController : MonoBehaviour
    {
        #region Varaibles

        [Header("UI Elements")] 
        [SerializeField] private TextMeshProUGUI levelTxt;
        [SerializeField] private TextMeshProUGUI scoreTxt;
        [SerializeField] private RectTransform diamondParent;
        [SerializeField] private LevelEndScreenController levelEndScreenController;

        [Header("So References")] 
        [SerializeField] private BoardEventSO boardEventSo;
        [SerializeField] private LevelEventSO levelEventSo;
        
        [Header("Prefab References")]
        [SerializeField] private RectTransform diamondPrefab;

        private Camera _mainCamera;
        private int _targetDiamond;
        #endregion

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            boardEventSo.OnDiamondCollected += BoardEventSo_OnDiamondCollected;
            levelEventSo.OnLevelSelected += LevelEventSo_OnLevelSelected;
            levelEventSo.OnLevelCompleted += LevelEventSo_OnLevelCompleted;
        }

        private void OnDisable()
        {
            boardEventSo.OnDiamondCollected -= BoardEventSo_OnDiamondCollected;
            levelEventSo.OnLevelSelected -= LevelEventSo_OnLevelSelected;
            levelEventSo.OnLevelCompleted -= LevelEventSo_OnLevelCompleted;
        }

        private void LevelEventSo_OnLevelCompleted(int arg0)
        {
            levelEndScreenController.Initialize(true);
        }

        private void LevelEventSo_OnLevelSelected(LevelSO arg0)
        {
            levelEndScreenController.gameObject.SetActive(true);
            Initialize(PlayerPrefsManager.GetCurrentLevel(), arg0.targetDiamond);
        }

        private void BoardEventSo_OnDiamondCollected(DiamondController collectedDiamond)
        {
            Destroy(collectedDiamond.gameObject);
            GenerateAndMoveDiamond(collectedDiamond.transform);
        }

        private void UpdateDiamondText()
        {
            _targetDiamond--;
            _targetDiamond = Mathf.Clamp(_targetDiamond, 0, int.MaxValue);
            scoreTxt.SetText(_targetDiamond.ToString());
            scoreTxt.transform.DOPunchScale(Vector3.one * 0.1f, 0.25f);
        }
        private void GenerateAndMoveDiamond(Transform collectedDiamond)
        {
            RectTransform diamondTransform = Instantiate(diamondPrefab, diamondParent);
            diamondTransform.position = _mainCamera.WorldToScreenPoint(collectedDiamond.position);
            
            DiamondMovementSequence(diamondTransform, diamondParent.position)
                .OnComplete(() =>
                {
                    UpdateDiamondText();
                    Destroy(diamondTransform.gameObject);
                });
        }

        private Sequence DiamondMovementSequence(RectTransform diamond, Vector3 targetPoint)
        {
            Sequence seq = DOTween.Sequence();

            float x = Random.Range(0, 100) < 50 ? 200 : -200;
            float y = Random.Range(0, 100) > 50 ? 200 : -200;
            
            Vector3 step1 = diamond.position;
            step1.x += x;
            step1.y += y;

            seq.Join(diamond.DOPath(new Vector3[] { step1, targetPoint }, 1f, PathType.CatmullRom,
                PathMode.Sidescroller2D, 10, Color.red).SetEase(Ease.InBack));
            
            return seq;
        }

        private void Initialize(int getCurrentLevel, int levelDataTargetDiamond)
        {
            levelTxt.SetText("LVL. " + (PlayerPrefsManager.GetCurrentLevel()+1).ToString());
            scoreTxt.SetText(levelDataTargetDiamond.ToString());
            _targetDiamond = levelDataTargetDiamond;
        }
    }
}