using TMPro;

namespace Commands
{
    public class LevelPanelCommand
    {

        #region Self Variables

        #region Private Variables

        private readonly TextMeshProUGUI _goldText;

        #endregion

        #endregion

        public LevelPanelCommand(ref TextMeshProUGUI goldText)
        {
            _goldText = goldText;
        }

        public void SetGoldScoreText(int gemValue)
        {
            _goldText.text = gemValue.ToString();
        }
    }
}