using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UMA;
using UMA.CharacterSystem;
using Michsky.UI.ModernUIPack;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Collections;
using TMPro;


public class AvatarCreator : MonoBehaviour
{
    public DynamicCharacterAvatar avatar;
    public static string sceneString;

    private Dictionary<string, DnaSetter> DNA;

    public string activeDNASlot;
    public string activeColorField;

    public string currentHair = "";
    public string currentFacialHair = "";
    public string currentFullOutfit = "";
    public string currentTop = "";
    public string currentBottom = "";
    public string currentShoes = "";

    public List<GameObject> mensOptions;
    public List<GameObject> womensOptions;

    public List<GameObject> mensHairOptions;
    public List<GameObject> mensTopOptions;

    public List<GameObject> womensHairOptions;
    public List<GameObject> womensTopOptions;
    public List<GameObject> womensBottomOptions;
    public List<GameObject> womensShoeOptions;

    public List<GameObject> hideWindowsMale;
    public List<GameObject> hideWindowsFemale;

    public string avatarRecipe;

    public Color color;
    public float darkness;
    public GameObject colorPreviewer;
    public GameObject hairColorPreviewer;

    private GameObject activeWindow;

    private bool toggleRepeated;

    public TMP_Text colorSelectorText;

    private void Start()
    {
        mensOptions.AddRange(mensHairOptions);
        mensOptions.AddRange(mensTopOptions);

        womensOptions.AddRange(womensHairOptions);
        womensOptions.AddRange(womensTopOptions);
        womensOptions.AddRange(womensBottomOptions);
        womensOptions.AddRange(womensShoeOptions);

        darkness = 1.0f;

        foreach (GameObject window in hideWindowsMale)
        {
            window.SetActive(false);
        }
        foreach (GameObject window in hideWindowsFemale)
        {
            window.SetActive(true);
        }
    }

    void OnEnable()
    {
        avatar.CharacterUpdated.AddListener(Updated);
    }

    void OnDisable()
    {
        avatar.CharacterUpdated.RemoveListener(Updated);
    }

    public void ModifySex(bool isMale)
    {
        if (isMale && avatar.activeRace.name != "HumanMaleHD")
        {
            avatar.ChangeRace("HumanMaleHD");

            foreach (GameObject wardrobeItem in womensOptions)
            {
                wardrobeItem.SetActive(false);
            }

            foreach (GameObject wardrobeItem in mensOptions)
            {
                wardrobeItem.SetActive(true);
            }

            foreach(GameObject window in hideWindowsMale)
            {
                window.SetActive(false);
            }
            foreach (GameObject window in hideWindowsFemale)
            {
                window.SetActive(true);
            }
        }
        if (!isMale && avatar.activeRace.name != "HumanFemaleHD")
        {
            avatar.ChangeRace("HumanFemaleHD");

            foreach (GameObject wardrobeItem in womensOptions)
            {
                wardrobeItem.SetActive(true);
            }

            foreach (GameObject wardrobeItem in mensOptions)
            {
                wardrobeItem.SetActive(false);
            }
            foreach (GameObject window in hideWindowsMale)
            {
                window.SetActive(true);
            }
            foreach (GameObject window in hideWindowsFemale)
            {
                window.SetActive(false);
            }
        }
    }


    public void SetActiveDNASlot(string _activeDNASlot)
    {
        activeDNASlot = _activeDNASlot;
    }

    public void SetActiveColorField(string _activeColorField)
    {
        colorSelectorText.text = _activeColorField + " Color";
        activeColorField = _activeColorField;
        darkness = 1.0f;
        color = Color.white;
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

    public void ModifyFieldColor(Color _color)
    {
        color = _color;
        colorPreviewer.GetComponent<Image>().color = color;
        ApplyColorSettings();
    }

    public void ModifyFieldColorDarkness(float value)
    {
        darkness = 1.0f - value;
        ApplyColorSettings();
    }

    public void ApplyColorSettings(){
        avatar.SetColor(activeColorField, color * darkness);
        colorPreviewer.GetComponent<Image>().color = color * darkness;
        Color tempColor = colorPreviewer.GetComponent<Image>().color;
        //stupid alpha shit
        tempColor.a = 1f;

        colorPreviewer.GetComponent<Image>().color = tempColor;
        if(activeColorField == "Hair")
        {
            hairColorPreviewer.GetComponent<Image>().color = tempColor;
        }

        avatar.UpdateColors(true);
    }

    public void ModifySkinColor(Color color)
    {
        avatar.SetColor("Skin", color);
        avatar.UpdateColors(true);
    }

    public void ModifyHair(string hairStyle)
    {
        if (currentHair == hairStyle)
        {
            avatar.ClearSlot("Hair");
            avatar.BuildCharacter();
            currentHair = "";
        }
        else
        {
            currentHair = hairStyle;
            avatar.SetSlot("Hair", hairStyle);
            avatar.BuildCharacter();
        }
    }

    public void ModifyFacialHair(string s) {
        if (currentHair == s)
        {
            avatar.ClearSlot("Beard");
            avatar.BuildCharacter();
            currentHair = "";
        }
        else
        {
            currentHair = s;
            avatar.SetSlot("Beard", s);
            avatar.BuildCharacter();
        }
    }


    public void ModifyFullOutfit(string fullOutfit)
    {
        if (currentFullOutfit == fullOutfit)
        {
            avatar.SetSlot("FullOutfit", "BlackSuit1");
            avatar.BuildCharacter();
            //currentFullOutfit = "BlackSuit2";
        }
        else
        {
            currentFullOutfit = fullOutfit;
            avatar.SetSlot("FullOutfit", fullOutfit);
            avatar.BuildCharacter();
        }
    }

    public void ModifyTop(string top)
    {
        if (currentTop == top)
        {
            avatar.SetSlot("Chest", "CropTop2");
            avatar.BuildCharacter();
        }
        else
        {
            currentTop = top;
            avatar.SetSlot("Chest", top);
            avatar.BuildCharacter();
        }
    }

    public void ModifyBottom(string bottom)
    {
        if (currentBottom == bottom)
        {
            avatar.SetSlot("Legs", "Skirt2");
            avatar.BuildCharacter();
        }
        else
        {
            currentBottom = bottom;
            avatar.SetSlot("Legs", bottom);
            avatar.BuildCharacter();
        }
    }

    public void ModifyShoes(string shoes)
    {
        if (currentShoes == shoes)
        {
            avatar.SetSlot("Feet", "Shoes2");
            avatar.BuildCharacter();
        }
        else
        {
            currentShoes = shoes;
            avatar.SetSlot("Feet", shoes);
            avatar.BuildCharacter();
        }
    }



    public void ToggleVisibility(GameObject window)
    {
        if (window != null)
        {
            Animator windowAnimator = window.GetComponent<Animator>();
            if (windowAnimator != null)
            {
                bool isOpen = windowAnimator.GetBool("Open");
                windowAnimator.SetBool("Open", !isOpen);
                if (isOpen)
                    window.transform.SetAsFirstSibling();
                else
                    window.transform.SetAsLastSibling();
            }
        }
    }

    private bool isCoroutineExecuting;
    IEnumerator ExecuteAfterTime(float time, Action task)
    {
        if (isCoroutineExecuting)
            yield break;
        isCoroutineExecuting = true;
        yield return new WaitForSeconds(time);
        task();
        isCoroutineExecuting = false;
    }

    public void ToggleWindowVisibility(string windowName)
    {
        GameObject window = GameObject.Find(windowName);

        if (activeWindow == window)
        {
            ToggleVisibility(window);
            toggleRepeated = true;
    
            StartCoroutine(ExecuteAfterTime(0.05f, () =>
            {
                Camera.main.GetComponent<MouseOrbitImproved>().SwitchTargetBone("Head");
            }));
        }
        else
        {
            if (!toggleRepeated) {
                ToggleVisibility(activeWindow);
            }
            ToggleVisibility(window);
            toggleRepeated = false;
        }

        activeWindow = window;

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
