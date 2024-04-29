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
        [SerializeField] private GameObject finalGo;
        [SerializeField] private AudioManager audioManager;
        [Header("Balance")] [SerializeField] private Vector2 blockMoveSpeedRandomRange;
        [SerializeField] private float delayAtStart, delayBetween;
        [SerializeField] private float blockDistanceToCreate;
        [SerializeField] private int blocksToFinish;
        [SerializeField] private float perfectDistance;
        private GameState _gameState = GameState.Idle;
        private float _waitDuration;
        private float _currentBlockSpeed;
        private bool _isBlockFromRight;
        private BlockController _createdBlock;
        private int _createdBlockCount = 1;
        private float _middlePoint;
        private float _blockWidth;
        private int _perfectStreak;
        private void OnEnable()
        {
            startButton.onClick.AddListener(StartButtonClicked);
            restartButton.onClick.AddListener(RestartButtonClicked);
            completedButton.onClick.AddListener(CompleteButtonClicked);
            var localScale = blockControllerPrefab.transform.localScale;
            _blockWidth = localScale.x;
            finalGo.transform.position = new Vector3(0, .5f, localScale.z * blocksToFinish);
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
                if (Input.GetMouseButtonDown(0)) StopBlock();
            }
        }
        private void FixedUpdate()
        {
            if (_gameState == GameState.BlockMoving)
                _createdBlock.transform.position += Vector3.right * (_currentBlockSpeed * Time.fixedDeltaTime);
        }
        private void CreateBlock()
        {
            _isBlockFromRight = !_isBlockFromRight;
            _currentBlockSpeed = (_isBlockFromRight ? -1 : 1) * Random.Range(blockMoveSpeedRandomRange.x, blockMoveSpeedRandomRange.y);
            var blockXPos = (_isBlockFromRight ? 1 : -1) * (blockDistanceToCreate + _middlePoint);
            var blockZPos = _createdBlockCount * blockControllerPrefab.transform.localScale.z;
            _createdBlock = Instantiate(blockControllerPrefab,
                new Vector3(blockXPos, 0, blockZPos), Quaternion.identity);
            _createdBlock.SetMaterial(blockMaterialList[Random.Range(0, blockMaterialList.Count)]);
            _createdBlock.SetWidth(_blockWidth);
            _createdBlockCount += 1;
            print(_createdBlockCount);
        }
        private void StopBlock()
        {
            var distance = _createdBlock.transform.position.x - _middlePoint;
            if (Mathf.Abs(distance) < perfectDistance)
            {
                audioManager.PlayPerfectSound(++_perfectStreak);
                var perfectBlockPos = _createdBlock.transform.position;
                perfectBlockPos = new Vector3(_middlePoint, perfectBlockPos.y, perfectBlockPos.z);
                _createdBlock.transform.position = perfectBlockPos;
                characterController.MoveToPoint(perfectBlockPos + Vector3.up / 2, CharacterArrived);
                _gameState = _createdBlockCount == blocksToFinish ? GameState.Completed : GameState.CharacterMoving;

                return;
            }

            _perfectStreak = 0;
            var oldBlockWidth = _blockWidth;
            if (Mathf.Abs(distance) > _blockWidth)
            {
                GameOver();
                return;
            }

            _blockWidth -= Mathf.Abs(distance);
            _middlePoint += distance / 2;
            var createdBlockTransform = _createdBlock.transform;
            var blockScale = createdBlockTransform.localScale;
            var blockPos = createdBlockTransform.position;

            createdBlockTransform.localScale = new Vector3(_blockWidth, blockScale.y, blockScale.z);
            blockPos = new Vector3(_middlePoint, blockPos.y, blockPos.z);
            createdBlockTransform.position = blockPos;

            var directionSign = distance < 0 ? -1 : 1;
            var fallCubeScale = new Vector3(oldBlockWidth - _blockWidth, blockScale.y, blockScale.z);
            var fallBlockPosX = (_middlePoint + (_blockWidth / 2 * directionSign)) + fallCubeScale.x / 2 * directionSign;
            var fallCubePos = new Vector3(fallBlockPosX, blockPos.y, blockPos.z);

            var fallBlock = Instantiate(_createdBlock);
            fallBlock.transform.position = fallCubePos;
            fallBlock.transform.localScale = fallCubeScale;
            fallBlock.Fall();

            characterController.MoveToPoint(blockPos + Vector3.up / 2, CharacterArrived);
            _gameState = _createdBlockCount == blocksToFinish ? GameState.Completed : GameState.CharacterMoving;
        }
        private void GameOver()
        {
            _gameState = GameState.Failed;
            restartButton.gameObject.SetActive(true);
        }
        private void CharacterArrived()
        {
            if (_gameState == GameState.Completed)
            {
                characterController.MoveToPoint(finalGo.transform.position, CharacterWon);
                return;
            }

            _gameState = GameState.Waiting;
            _waitDuration = delayBetween;
        }
        private void CharacterWon()
        {
            characterController.StartDance();
            completedButton.gameObject.SetActive(true);
        }
        private void CompleteButtonClicked()
        {
            SceneManager.LoadScene(0);
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