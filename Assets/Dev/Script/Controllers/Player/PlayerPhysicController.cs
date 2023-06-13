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

        private bool _canPay = true;

        #endregion

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlantTileController>(out PlantTileController plantTileController))
            {
                var gemStackable = plantTileController.GetGemObject();
                Debug.Log("Trigger: " + gemStackable);
                if (gemStackable)
                {
                    if (!gemStackable.IsCollected && gemStackable.IsCollectable)
                    {
                        CollectMoney(gemStackable);
                        plantTileController.OnResetTile();
                    }
                }
            }
        }
        private void CollectMoney(Stackable stackable)
        {
            stackController.SetStackHolder(stackable.transform);
            stackController.GetStack(stackable.gameObject);
            stackable.SetIsCollected(true);
        }

        #region Paying Interaction

        #endregion
    }
}
