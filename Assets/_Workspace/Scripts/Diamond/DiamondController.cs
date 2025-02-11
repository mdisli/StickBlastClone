using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Workspace.Scripts.Diamond
{
    public class DiamondController : MonoBehaviour
    {
        public async UniTask MoveToTarget(Vector3 targetPosition)
        {
            await MoveToUISequence(targetPosition).ToUniTask();
        }

        private Sequence MoveToUISequence(Vector3 targetPosition)
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Join(transform.DOMove(targetPosition, 1).SetEase(Ease.InBack));
            
            return sequence;
        }
    }
}