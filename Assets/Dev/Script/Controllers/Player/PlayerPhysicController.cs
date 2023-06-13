using Signals;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers
{
	public class PlayerPhysicController : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        #endregion

        #region Serialized Variables

        [SerializeField]
        private StackManager moneyStackerController;

        #endregion

        #region Private Variables

        private bool _canPay = true;

        #endregion

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Stackable>(out Stackable stackable))
            {
                Debug.Log("Trigger");
                if (!stackable.isCollected)
                {
                    CollectMoney(stackable);
                }
            }
        }
        private void CollectMoney(Stackable stackable)
        {
            moneyStackerController.SetStackHolder(stackable.transform);
            moneyStackerController.GetStack(stackable.gameObject);
            stackable.isCollected = true;
        }

        #region Paying Interaction

        #endregion
    }
}
