using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Interfaces
{
    public abstract class AStacker : MonoBehaviour
    {
        public List<Stackable> StackList = new List<Stackable>();

        public virtual void SetStackHolder(Transform otherTransform)
        {
            otherTransform.SetParent(transform);
        }

        public virtual void GetStack(Stackable stackableObj)
        {

        }

        public virtual void GetStack(GameObject stackableObj, Transform otherTransform)
        {

        }
    }
}