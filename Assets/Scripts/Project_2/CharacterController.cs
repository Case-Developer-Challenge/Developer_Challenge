using System;
using System.Collections;
using UnityEngine;

namespace Project_2
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        private Animator _animator;
        private static readonly int MovementSpeed = Animator.StringToHash("MovementSpeed");
        private static readonly int Dance = Animator.StringToHash("Dance");
        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }
        public void StartDance()
        {
            _animator.SetBool(Dance, true);
        }
        public void MoveToPoint(Vector3 target, Action characterArrived)
        {
            _animator.SetBool(Dance, false);
            StartCoroutine(MoveToPointCor(target, characterArrived));
        }
        private IEnumerator MoveToPointCor(Vector3 target, Action characterArrived)
        {
            _animator.SetFloat(MovementSpeed, 1);
            const float tolerance = 0.05f;
            var waitForFixedUpdate = new WaitForFixedUpdate();
            while (Vector3.Distance(transform.position, target) > tolerance)
            {
                var direction = (target - transform.position).normalized;
                transform.position += direction * (moveSpeed * Time.fixedDeltaTime);
                yield return waitForFixedUpdate;
            }

            transform.position = target;
            _animator.SetFloat(MovementSpeed, 0);
            characterArrived?.Invoke();
        }
    }
}