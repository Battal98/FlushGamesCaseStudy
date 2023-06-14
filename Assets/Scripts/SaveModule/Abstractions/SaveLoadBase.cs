using SaveLoadModule.Interfaces;
using SaveLoadModule.Enums;

namespace SaveLoadModule.Abstractions
{
    public abstract class SaveLoadBase : ISavable
    {
        public virtual SaveLoadType GetKey()
        {
            return SaveLoadType.CollectedGemData;
        }

        public virtual void ScoreData()
        {

        }
    }

}