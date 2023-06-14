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

        #endregion

        #region Private Variables

        private bool _isInteracting = false;
        #endregion

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlantTileController>(out PlantTileController plantTileController))
            {
                if (_isInteracting)
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
            _isInteracting = true;

            yield return new WaitForEndOfFrame();

            plantTileController.OnResetTile();

            _isInteracting = false;
        }
    }
}
