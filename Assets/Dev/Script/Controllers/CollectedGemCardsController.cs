using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    public class CollectedGemCardsController : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _gemNameText;
        [SerializeField]
        private TextMeshProUGUI _totalGoldText;
        [SerializeField]
        private TextMeshProUGUI _gemCountText;
        [SerializeField]
        private Image _gemImage;


        private string _gemName;
        private int _gemCount;
        private int _totalGold;

        public void SetVariables(CollectedGemDatas collectedGemDatas)
        {
            _gemName = collectedGemDatas.GemName;
            var objCom = collectedGemDatas.SpriteObj.GetComponent<Image>();
            _gemImage.sprite = objCom.sprite;
            _gemCount = collectedGemDatas.GemCount;
            _totalGold = collectedGemDatas.TotalGold;
            WriteDatas();
        }

        private void WriteDatas()
        {
            _gemNameText.text = "Gem Type: " + _gemName;
            _gemCountText.text = "Gem Count: " + _gemCount.ToString();
            _totalGoldText.text = "Gem Total Gold: " + _totalGold.ToString();
        }

        public void ChangeGemCountAndGoldScore(int gemCount, int totalCount)
        {
            _gemCountText.text = "Gem Count: " + gemCount.ToString();
            _totalGoldText.text = "Gem Total Gold: " + totalCount.ToString();
        }
    } 
}
