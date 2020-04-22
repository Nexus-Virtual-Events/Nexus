using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Michsky.UI.ModernUIPack
{
    public class ScrollManagerSystem : MonoBehaviour
    {

        [Header("RESOURCES")]
        public Scrollbar listScrollbar;

        [HideInInspector] public bool enabeScrolling = false;

        void Update()
        {
            if (enabeScrolling == false)
                listScrollbar.value = Mathf.Lerp(listScrollbar.value, 1, 0.1f);
        }

        public void EnableScrolling()
        {
            enabeScrolling = true;
        }

        public void DisableScrolling()
        {
            enabeScrolling = false;
        }
    }
}