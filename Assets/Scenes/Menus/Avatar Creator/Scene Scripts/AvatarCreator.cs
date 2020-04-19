using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMA;
using UMA.CharacterSystem;
using UnityEngine.UI;
using System;
using System.IO;
using Michsky.UI.ModernUIPack;
using UnityEngine.SceneManagement;

public class AvatarCreator : MonoBehaviour
{
    public DynamicCharacterAvatar avatar;
    public static string sceneString;
    //public Slider heightSlider;
    //public Slider weightSlider;

    private Dictionary<string, DnaSetter> DNA;
    public string activeDNASlot;

    //public List<string> hairStylesMale = new List<string>();
    //private int currentHairStyleMale;

    //public List<string> hairStylesFemale = new List<string>();
    //private int currentHairStyleFemale;

    public string avatarRecipe;

    void OnEnable()
    {
        avatar.CharacterUpdated.AddListener(Updated);
        //heightSlider.onValueChanged.AddListener(ModifyHeight);
        //weightSlider.onValueChanged.AddListener(ModifyWeight);
    }

    void OnDisable()
    {
        avatar.CharacterUpdated.RemoveListener(Updated);
        //heightSlider.onValueChanged.RemoveListener(ModifyHeight);
        //weightSlider.onValueChanged.RemoveListener(ModifyWeight);
    }

    public void ModifySex(bool isMale)
    {
        if (isMale && avatar.activeRace.name != "HumanMaleDCS") avatar.ChangeRace("HumanMaleDCS");
        if (!isMale && avatar.activeRace.name != "HumanFemaleDCS") avatar.ChangeRace("HumanFemaleDCS");
    }

    public void SetActiveDNASlot(string _activeDNASlot)
    {
        activeDNASlot = _activeDNASlot;
    }

    public void SliderChange(float value)
    {
        DNA[activeDNASlot].Set(value);
        avatar.BuildCharacter();
    }

    void Updated(UMAData data)
    {
        DNA = avatar.GetDNA();
    }

    public void Done()
    {
        SaveAvatar();

        Loading.sceneString = sceneString;
        SceneManager.LoadScene("Loading");

    }

    public void ModifySkinColor(Color color)
    {
        avatar.SetColor("Skin", color);
        avatar.UpdateColors(true);
    }

    //public void ModifyHair(bool pressedPlus)
    //{
    //    if(avatar.activeRace.name == "HumanMaleHD")
    //    {
    //        if (pressedPlus)
    //            currentHairStyleMale++;
    //        else
    //            currentHairStyleMale--;

    //        currentHairStyleMale = Mathf.Clamp(currentHairStyleMale, 0, hairStylesMale.Count - 1);

    //        if (hairStylesMale[currentHairStyleMale] == "None")
    //            avatar.ClearSlot("Hair");
    //        else
    //            avatar.SetSlot("Hair", hairStylesMale[currentHairStyleMale]);

    //        avatar.BuildCharacter();
    //    }
    //    if (avatar.activeRace.name == "HumanFemaleHD")
    //    {
    //        if (pressedPlus)
    //            currentHairStyleFemale++;
    //        else
    //            currentHairStyleFemale--;

    //        currentHairStyleFemale = Mathf.Clamp(currentHairStyleFemale, 0, hairStylesFemale.Count - 1);

    //        if (hairStylesFemale[currentHairStyleFemale] == "None")
    //            avatar.ClearSlot("Hair");
    //        else
    //            avatar.SetSlot("Hair", hairStylesFemale[currentHairStyleFemale]);

    //        avatar.BuildCharacter();
    //    }
    //}

    public void ToggleWindowVisibility(string windowName)
    {
        GameObject window = GameObject.Find(windowName);
        Animator windowAnimator = window.GetComponent<Animator>();
        if(windowAnimator != null)
        {
            bool isOpen = windowAnimator.GetBool("Open");
            windowAnimator.SetBool("Open", !isOpen);
            if(isOpen)
                window.transform.SetAsFirstSibling();
            else
                window.transform.SetAsLastSibling();
        }
    }

    public void SaveAvatar()
    {
        avatarRecipe = avatar.GetCurrentRecipe();
        PlayerPrefs.SetString("playerRecipe", avatarRecipe);
    }

    public void LoadAvatar()
    {
        avatarRecipe = PlayerPrefs.GetString("playerRecipe");
        avatar.ClearSlots();
        avatar.LoadFromRecipeString(avatarRecipe);
    }
}
