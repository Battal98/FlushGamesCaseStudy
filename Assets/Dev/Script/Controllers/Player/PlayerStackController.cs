using Data.UnityObject;
using DG.Tweening;
using Interfaces;
using PoolModule.Enums;
using PoolModule.Interfaces;
using PoolModule.Signals;
using Signals;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

namespace Controllers
{
    public class PlayerStackController : AStacker, IGetPoolObject, IReleasePoolObject
    {
        [SerializeField]
        private List<Vector3> nextPositions = new List<Vector3>();

        private PlayerStackData _playerStackData;
        private const string DataPath = "Data/CD_PlayerStack";

        private void Awake()
        {
            DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(500, 125);
            _playerStackData = GetPlayerStackData();
        }

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents(true);
        }

        private void SubscribeEvents(bool isRegister)
        {
            if (isRegister)
            {
                CoreGameSignals.Instance.onRemoveStack += OnRemoveAllStack;
            }
            else
            {
                CoreGameSignals.Instance.onRemoveStack -= OnRemoveAllStack;
            }
        }
        private void OnDisable()
        {
            SubscribeEvents(false);
        }
        #endregion

        private new List<Stackable> StackList
        {
            get => base.StackList;
            set => base.StackList = value;
        }

        private PlayerStackData GetPlayerStackData()
        {
            return Resources.Load<CD_PlayerStack>(DataPath).PlayerStackData;
        }

        public override void SetStackHolder(Transform otherTransform)
        {
            otherTransform.SetParent(transform);
        }
        public override async void GetStack(Stackable stackableObj)
        {
            stackableObj.transform.rotation = Quaternion.LookRotation(transform.forward);
            StackList.Add(stackableObj);
            var posY = StackList.Count >= 2 ? nextPositions[nextPositions.Count-1].y + _playerStackData.OffsetY : 0;
            var pos = new Vector3(0, posY, 0);
            nextPositions.Add(pos);
            await Task.Delay(10);
            stackableObj.transform.DOLocalMove(nextPositions[nextPositions.Count - 1], 0.3f);
        }

        public void PaymentStackAnimation(Transform transform)
        {
            var moneyObj = GetObject(PoolType.Money);
            moneyObj.transform.position = this.transform.parent.transform.position;
            moneyObj.GetComponent<Collider>().enabled = false;
            moneyObj.transform.rotation = Quaternion.LookRotation(transform.forward);
            moneyObj.transform.DOMove(transform.position, 0.3f).OnComplete(() => ReleaseObject(moneyObj, PoolType.Money));

        }

        #region Remove Jobs

        public void OnRemoveAllStack(Transform target)
        {
            _isCanceledTween = false;
            RemoveAllStack(target);
        }

        private Tween _currentTween;
        private bool _isCanceledTween;// Eklediðimiz deðiþken, mevcut tween hareketini takip etmek için kullanýlacak
        private void RemoveAllStack(Transform target)
        {
            if (StackList.Count <= 0)
                return;

            _currentTween = RemoveStackAnimationTween(StackList[StackList.Count - 1], target);
        }

        private void RemoveStackAnimation(Stackable removedStack, Transform targetTransform)
        {
            removedStack.transform.rotation = Quaternion.LookRotation(transform.forward);
            removedStack.gameObject.transform.SetParent(null);
            //CoreGameSignals.Instance.onUpdateMoneyScore.Invoke(+10);
            CoreGameSignals.Instance.onCalculateGemStackType?.Invoke(removedStack.GemType);

            removedStack.transform.DOLocalMove(targetTransform.localPosition, .1f).OnComplete(() =>
            {
                StackList.Remove(removedStack);
                StackList.TrimExcess();
                ReleaseObject(removedStack.gameObject, PoolType.Money);
                MoveNextObject(StackList[StackList.Count - 1], targetTransform); 
            });

        }
        private Tween RemoveStackAnimationTween(Stackable removedStack, Transform targetTransform)
        {
            removedStack.transform.rotation = Quaternion.LookRotation(transform.forward);
            removedStack.gameObject.transform.SetParent(null);
            //CoreGameSignals.Instance.onUpdateMoneyScore.Invoke(+10);
            CoreGameSignals.Instance.onCalculateGemStackType?.Invoke(removedStack.GemType);

            Tween tween = removedStack.transform.DOLocalMove(targetTransform.localPosition, .1f).OnComplete(() =>
            {
                StackList.Remove(removedStack);
                nextPositions.RemoveAt(nextPositions.Count-1);
                StackList.TrimExcess();
                nextPositions.TrimExcess();
                ReleaseObject(removedStack.gameObject, PoolType.Money);
                if (StackList.Count > 0 && !_isCanceledTween)
                {
                    MoveNextObject(StackList[StackList.Count - 1], targetTransform);
                }
            });

            return tween;
        }

        private void MoveNextObject(Stackable removedStack, Transform targetTransform)
        {
            _currentTween = RemoveStackAnimationTween(removedStack, targetTransform);
        }

        public void StopTween()
        {
            if (_currentTween != null)
            {
                _currentTween.Complete();
                _isCanceledTween = true; // Önceki tween hareketini durdur
            }
        }
        #endregion

        public GameObject GetObject(PoolType poolType)
        {
            return PoolSignals.Instance.onGetObjectFromPool?.Invoke(poolType);
        }

        public void ReleaseObject(GameObject obj, PoolType poolType)
        {
            PoolSignals.Instance.onReleaseObjectFromPool?.Invoke(obj, poolType);
        }
    }
}
