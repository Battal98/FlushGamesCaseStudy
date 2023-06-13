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
        private PlayerStackController stackController;
        private bool isInteracting = false;

        #endregion

        #region Private Variables

        private bool _canPay = true;

        #endregion

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlantTileController>(out PlantTileController plantTileController))
            {
                if (isInteracting)
                    return;

                var gemStackable = plantTileController.GetGemObject();

                if (!gemStackable)
                    return;

                if (!gemStackable.IsCollected && gemStackable.IsCollectable)
                {
                    CollectMoney(gemStackable);
                    StartCoroutine(TriggerInteraction(plantTileController));
                }
            }

            if (other.TryGetComponent<StoreManager>(out StoreManager storeController))
            {
                CoreGameSignals.Instance.onRemoveStack?.Invoke(storeController.transform);
                storeController.ChangeColor(Color.green);
                //storeController.AddData();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<StoreManager>(out StoreManager storeController))
            {
                storeController.ChangeColor(Color.black);
                stackController.StopTween();
            }
        }
        private void CollectMoney(Stackable stackable)
        {
            stackController.SetStackHolder(stackable.transform);
            stackController.GetStack(stackable);
            stackable.SetIsCollected(true);
        }

        private IEnumerator TriggerInteraction(PlantTileController plantTileController)
        {
            isInteracting = true;

            yield return new WaitForEndOfFrame();

            plantTileController.OnResetTile();

            isInteracting = false;
        }
        #region Paying Interaction

        #endregion
    }
}
