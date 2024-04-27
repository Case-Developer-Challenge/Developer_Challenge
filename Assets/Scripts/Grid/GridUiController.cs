using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Grid
{
    public class GridUiController : MonoBehaviour
    {
        [SerializeField] private Button rebuildButton;
        [SerializeField] private TMP_InputField gridCountInputField;
        [SerializeField] private string matchDesc;
        [SerializeField] private TMP_Text matchCount;
        private void Awake()
        {
            SetMatchCount(0);
        }
        private void OnEnable()
        {
            rebuildButton.onClick.AddListener(OnRebuildClicked);
            GridGameManager.instance.MatchCountChange += OnMatchCountChange;
        }
        private void OnDisable()
        {
            rebuildButton.onClick.RemoveListener(OnRebuildClicked);
            GridGameManager.instance.MatchCountChange -= OnMatchCountChange;
        }
        private void OnRebuildClicked()
        {
            if (int.TryParse(gridCountInputField.text, out var newGridCount)) 
                GridGameManager.instance.SetNewGridCount(newGridCount);
        }
        private void OnMatchCountChange(int newValue)
        {
            SetMatchCount(newValue);
        }
        private void SetMatchCount(int newValue)
        {
            matchCount.text = string.Format(matchDesc, newValue);
        }
    }
}