using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UMA;
using UMA.CharacterSystem;
using UnityEngine.UI;
using System;
using System.IO;

public class AvatarCreator : MonoBehaviour
{
    public DynamicCharacterAvatar avatar;
    public Slider heightSlider;
    public Slider weightSlider;

    private Dictionary<string, DnaSetter> DNA;

    public List<string> hairStylesMale = new List<string>();
    private int currentHairStyleMale;

    public List<string> hairStylesFemale = new List<string>();
    private int currentHairStyleFemale;

    public string avatarRecipe;

    void OnEnable()
    {
        avatar.CharacterUpdated.AddListener(Updated);
        heightSlider.onValueChanged.AddListener(ModifyHeight);
        weightSlider.onValueChanged.AddListener(ModifyWeight);
    }

    void OnDisable()
    {
        avatar.CharacterUpdated.RemoveListener(Updated);
        heightSlider.onValueChanged.RemoveListener(ModifyHeight);
        weightSlider.onValueChanged.RemoveListener(ModifyWeight);
    }

    public void ModifySex(bool isMale)
    {
        if (isMale && avatar.activeRace.name != "HumanMaleHD") avatar.ChangeRace("HumanMaleHD");
        if (!isMale && avatar.activeRace.name != "HumanFemaleHD") avatar.ChangeRace("HumanFemaleHD");
    }

    void Updated(UMAData data)
    {
        DNA = avatar.GetDNA();
        heightSlider.value = DNA["height"].Get();
        weightSlider.value = DNA["belly"].Get();
    }

    public void ModifyHeight(float height)
    {
        DNA["height"].Set(height);
        avatar.BuildCharacter();
    }

    public void ModifyWeight(float weight)
    {
        DNA["belly"].Set(weight);
        avatar.BuildCharacter();
    }

    public void ModifySkinColor(Color color)
    {
        avatar.SetColor("Skin", color);
        avatar.UpdateColors(true);
    }

    public void ModifyHair(bool pressedPlus)
    {
        if(avatar.activeRace.name == "HumanMaleHD")
        {
            if (pressedPlus)
                currentHairStyleMale++;
            else
                currentHairStyleMale--;

            currentHairStyleMale = Mathf.Clamp(currentHairStyleMale, 0, hairStylesMale.Count - 1);

            if (hairStylesMale[currentHairStyleMale] == "None")
                avatar.ClearSlot("Hair");
            else
                avatar.SetSlot("Hair", hairStylesMale[currentHairStyleMale]);

            avatar.BuildCharacter();
        }
        if (avatar.activeRace.name == "HumanFemaleHD")
        {
            if (pressedPlus)
                currentHairStyleFemale++;
            else
                currentHairStyleFemale--;

            currentHairStyleFemale = Mathf.Clamp(currentHairStyleFemale, 0, hairStylesFemale.Count - 1);

            if (hairStylesFemale[currentHairStyleFemale] == "None")
                avatar.ClearSlot("Hair");
            else
                avatar.SetSlot("Hair", hairStylesFemale[currentHairStyleFemale]);

            avatar.BuildCharacter();
        }
    }

    public void SaveAvatar()
    {
        avatarRecipe = avatar.GetCurrentRecipe();
        File.WriteAllText(Application.persistentDataPath + "/avatar.txt", avatarRecipe);
    }

    public void LoadAvatar()
    {
        avatarRecipe = File.ReadAllText(Application.persistentDataPath + "/avatar.txt");
        avatar.ClearSlots();
        avatar.LoadFromRecipeString(avatarRecipe);
    }
}
