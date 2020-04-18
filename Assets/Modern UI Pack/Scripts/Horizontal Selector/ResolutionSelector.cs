using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace Michsky.UI.ModernUIPack
{
    public class ResolutionSelector : MonoBehaviour
    {
        private TextMeshProUGUI label;
        private TextMeshProUGUI labeHelper;
        private Animator selectorAnimator;

        [Header("SETTINGS")]
        public string selectorTag = "Tag Text";
        public int defaultIndex = 0;
        public bool saveValue;
        public bool invokeAtStart;
        public bool invertAnimation;
        public bool loopSelection;
        public int index = 0;
        public static Resolution[] resolutions;

        [Header("ITEMS")]
        public List<Item> itemList = new List<Item>();

        [System.Serializable]
        public class Item
        {
            public string itemTitle = "Item Title";
            //public UnityEvent onValueChanged;


            public Item(string itemTitle)
            {
                this.itemTitle = itemTitle;
                //this.onValueChanged = onValueChanged;
            }
        }

        public void IncrementResolution()
        {
            if (index < resolutions.Length)
            {
                Resolution resolution = resolutions[index - 1];
                Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
                Debug.Log(resolution.height);

            }

        }

        public void DecrementResolution()
        {
            if (index > 0)
            {
                Resolution resolution = resolutions[index + 1];
                Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
                Debug.Log(resolution.height);
            }

        }

        void Start()
        {
            selectorAnimator = gameObject.GetComponent<Animator>();
            label = transform.Find("Text").GetComponent<TextMeshProUGUI>();
            labeHelper = transform.Find("Text Helper").GetComponent<TextMeshProUGUI>();

            if (saveValue == true)
                defaultIndex = PlayerPrefs.GetInt(selectorTag + "HSelectorValue");

            //if (invokeAtStart == true)
            //    itemList[index].onValueChanged.Invoke();

            label.text = itemList[defaultIndex].itemTitle;
            labeHelper.text = label.text;
            index = defaultIndex;
        }

        private void Update()
        {
            if(label.text == "")
            {
                label.text = itemList[defaultIndex].itemTitle;
                labeHelper.text = label.text;
            }
        }

        public void PreviousClick()
        {

            if (loopSelection == false)
            {
                if (index != 0)
                {
                    labeHelper.text = label.text;

                    if (index == 0)
                        index = itemList.Count - 1;

                    else
                        index--;

                    label.text = itemList[index].itemTitle;

                    DecrementResolution();

                    selectorAnimator.Play(null);
                    selectorAnimator.StopPlayback();

                    if (invertAnimation == true)
                        selectorAnimator.Play("Forward");
                    else
                        selectorAnimator.Play("Previous");

                    if (saveValue == true)
                        PlayerPrefs.SetInt(selectorTag + "HSelectorValue", index);
                }
            }

            else
            {
                labeHelper.text = label.text;

                if (index == 0)
                    index = itemList.Count - 1;

                else
                    index--;

                label.text = itemList[index].itemTitle;
                DecrementResolution();

                selectorAnimator.Play(null);
                selectorAnimator.StopPlayback();

                if (invertAnimation == true)
                    selectorAnimator.Play("Forward");
                else
                    selectorAnimator.Play("Previous");

                if (saveValue == true)
                    PlayerPrefs.SetInt(selectorTag + "HSelectorValue", index);
            }
            Debug.Log(Screen.currentResolution.height);


        }

        public void ForwardClick()
        {

            if (loopSelection == false)
            {
                if (index != itemList.Count - 1)
                {
                    labeHelper.text = label.text;

                    if ((index + 1) >= itemList.Count)
                        index = 0;

                    else
                        index++;

                    label.text = itemList[index].itemTitle;
                    IncrementResolution();

                    selectorAnimator.Play(null);
                    selectorAnimator.StopPlayback();

                    if (invertAnimation == true)
                        selectorAnimator.Play("Previous");
                    else
                        selectorAnimator.Play("Forward");

                    if (saveValue == true)
                        PlayerPrefs.SetInt(selectorTag + "HSelectorValue", index);
                }
            }

            else
            {
                labeHelper.text = label.text;

                if ((index + 1) >= itemList.Count)
                    index = 0;

                else
                    index++;

                label.text = itemList[index].itemTitle;
                IncrementResolution();

                selectorAnimator.Play(null);
                selectorAnimator.StopPlayback();

                if (invertAnimation == true)
                    selectorAnimator.Play("Previous");
                else
                    selectorAnimator.Play("Forward");

                if (saveValue == true)
                    PlayerPrefs.SetInt(selectorTag + "HSelectorValue", index);
            }
            Debug.Log(Screen.currentResolution.height);
        }
    }
}