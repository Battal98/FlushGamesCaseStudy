using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PopupType
{
    StorePopup,
}

namespace Commands
{
    public class PopupCommands
    {
        private List<GameObject> _popupList = new List<GameObject>();
        public PopupCommands(ref List<GameObject> popupList)
        {
            _popupList = popupList;
        }

        public void OpenPopUp(PopupType type)
        {
            if(!_popupList[(int)type].activeInHierarchy)
            _popupList[(int)type].SetActive(true);
        }

        public void ClosePopUp(PopupType type)
        {
            if (_popupList[(int)type].activeInHierarchy)
                _popupList[(int)type].SetActive(false);
        }

        public void CloseAllPopups()
        {
            _popupList.CloseAllListElements();
        }
    } 
}
