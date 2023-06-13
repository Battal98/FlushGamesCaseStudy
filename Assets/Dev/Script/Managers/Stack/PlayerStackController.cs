using Data.UnityObject;
using DG.Tweening;
using Interfaces;
using PoolModule.Enums;
using PoolModule.Interfaces;
using PoolModule.Signals;
using Signals;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Controllers
{
    public class PlayerStackController : AStacker, IGetPoolObject, IReleasePoolObject
    {
        [SerializeField]
        private List<Vector3> nextPositions = new List<Vector3>();

        private int _stackListConstCount;

        private bool _canRemove = true;
        private PlayerStackData _playerStackData;
        private const string DataPath = "Data/CD_PlayerStack";

        private void Awake()
        {
            DOTween.Init(true, true, LogBehaviour.Verbose).SetCapacity(500, 125);
            _playerStackData = GetPlayerStackData();
        }
        private new List<GameObject> StackList
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
        public override async void GetStack(GameObject stackableObj)
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

        public void OnRemoveAllStack()
        {
            if (!_canRemove)
                return;
            _canRemove = false;
            _stackListConstCount = StackList.Count;
            RemoveAllStack();
        }

        private async void RemoveAllStack()
        {
            if (StackList.Count == 0)
            {
                _canRemove = true;
                return;
            }
            if (StackList.Count >= _stackListConstCount - 6)
            {
                RemoveStackAnimation(StackList[StackList.Count - 1]);
                StackList.TrimExcess();
                await Task.Delay(201);
                RemoveAllStack();
            }
            else
            {
                for (int i = 0; i < StackList.Count; i++)
                {
                    RemoveStackAnimation(StackList[i]);
                    StackList.TrimExcess();
                }
                _canRemove = true;
            }
        }

        private void RemoveStackAnimation(GameObject removedStack)
        {
            removedStack.transform.rotation = Quaternion.LookRotation(transform.forward);
            //CoreGameSignals.Instance.onUpdateMoneyScore.Invoke(+10);
            StackList.Remove(removedStack);
            removedStack.transform.DOLocalMove(transform.localPosition, .1f).OnComplete(() =>
            {
                ReleaseObject(removedStack, PoolType.Money);
            });
        }

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
