using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.UI.Michsky.UI.ModernUIPack;
using Michsky.UI.ModernUIPack;
using Normal.Realtime;

public class RoomSelector : MonoBehaviour
{
    public List<RoomUI> roomList = new List<RoomUI>();
    public int index = 0;

    public Image normalImage;
    public Image highlightedImage;
    public UIGradient highlightedGradient;
    public UIGradient backgroundGradient;

    [System.Serializable]
    public class RoomUI
    {
        public Sprite roomIcon;
        public Color gradientStart;
        public Color gradientEnd;

        public RoomUI(Sprite roomIcon, Color gradientStart, Color gradientEnd)
        {
            this.roomIcon = roomIcon;
            this.gradientStart = gradientStart;
            this.gradientEnd = gradientEnd;
        }
    }

    public void ForwardClick()
    {
        if (index < roomList.Count-1 && index >= 0)
        {
            index += 1;
            UpdateRoomUI();
        }
    }

    public void PreviousClick()
    {
        if (index < roomList.Count && index >= 1)
        {
            index -= 1;
            UpdateRoomUI();
        }
    }

    public void UpdateRoomUI()
    {
        gameObject.GetComponent<RoomRouter>().roomIndex = index;

        normalImage.sprite = roomList[index].roomIcon;
        highlightedImage.sprite = roomList[index].roomIcon;

        UnityEngine.Gradient gradient = new UnityEngine.Gradient() { colorKeys = new GradientColorKey[] { new GradientColorKey(roomList[index].gradientStart, 0), new GradientColorKey(roomList[index].gradientEnd, 1) } };

        highlightedGradient.EffectGradient = gradient;
        backgroundGradient.EffectGradient = gradient;
    }
}
