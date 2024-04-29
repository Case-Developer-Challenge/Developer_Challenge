using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Project_2
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private Button startButton, restartButton, completedButton;
        [SerializeField] private BlockController blockControllerPrefab;
        [SerializeField] private List<Material> blockMaterialList;
        [Header("Balance")] [SerializeField] private Vector2 blockMoveSpeedRandomRange;
        [SerializeField] private float delayAtStart, delayBetween;
        [SerializeField] private float blockDistanceToCreate;
        private GameState _gameState = GameState.Idle;
        private float _waitDuration;
        private float _currentBlockSpeed;
        private bool _isBlockFromRight;
        private BlockController _createdBlock;
        private int _createdBlockCount = 1;
        private float _middlePoint = 0;
        private void OnEnable()
        {
            startButton.onClick.AddListener(StartButtonClicked);
            restartButton.onClick.AddListener(RestartButtonClicked);
            completedButton.onClick.AddListener(CompleteButtonClicked);
        }
        private void OnDisable()
        {
            startButton.onClick.RemoveListener(StartButtonClicked);
            restartButton.onClick.RemoveListener(RestartButtonClicked);
            completedButton.onClick.RemoveListener(CompleteButtonClicked);
        }
        private void Update()
        {
            if (_waitDuration > 0)
            {
                _waitDuration -= Time.deltaTime;
                return;
            }

            if (_gameState == GameState.Waiting)
            {
                _gameState = GameState.BlockMoving;
                CreateBlock();
            }
            else if (_gameState == GameState.BlockMoving)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    // Mathf.Abs(_middlePoint - _createdBlock.transform.position.z)
                    var blockPos = _createdBlock.transform.position;
                    _middlePoint = blockPos.z;
                    characterController.MoveToPoint(blockPos + Vector3.up / 2, CharacterArrived);
                    _gameState = GameState.CharacterMoving;
                }
            }
        }
        private void FixedUpdate()
        {
            if (_gameState == GameState.BlockMoving)
                _createdBlock.transform.position += Vector3.right * (_currentBlockSpeed * Time.fixedDeltaTime);
        }
        private void CharacterArrived()
        {
            _gameState = GameState.Waiting;
            _waitDuration = delayBetween;
        }
        private void CreateBlock()
        {
            _isBlockFromRight = !_isBlockFromRight;
            _currentBlockSpeed = (_isBlockFromRight ? -1 : 1) * Random.Range(blockMoveSpeedRandomRange.x, blockMoveSpeedRandomRange.y);
            var blockXPos =  (_isBlockFromRight ? 1 : -1) * (blockDistanceToCreate + _middlePoint);
            var blockZPos = _createdBlockCount * blockControllerPrefab.transform.localScale.z;
            _createdBlock = Instantiate(blockControllerPrefab,
                new Vector3(blockXPos, 0, blockZPos), Quaternion.identity);
            _createdBlock.SetMaterial(blockMaterialList[Random.Range(0, blockMaterialList.Count)]);
            _createdBlockCount += 1;
        }
        private void CompleteButtonClicked()
        {
            completedButton.gameObject.SetActive(false);
            _waitDuration = delayAtStart;
        }
        private void RestartButtonClicked()
        {
            SceneManager.LoadScene(0);
        }
        private void StartButtonClicked()
        {
            startButton.gameObject.SetActive(false);
            _waitDuration = delayAtStart;
            _gameState = GameState.Waiting;
        }
        private enum GameState
        {
            Idle,
            Waiting,
            BlockMoving,
            CharacterMoving,
            Failed,
            Completed
        }
    }
}