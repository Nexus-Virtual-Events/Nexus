using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UMA;
using UMA.CharacterSystem;
using Michsky.UI.ModernUIPack;
using UnityEngine.SceneManagement;

public class AvatarCreator : MonoBehaviour
{
    public DynamicCharacterAvatar avatar;
    public static string sceneString;

    private Dictionary<string, DnaSetter> DNA;

    public string activeDNASlot;
    public string activeColorField;

    public string currentHair = "";
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

    public string avatarRecipe;

    private void Start()
    {
        mensOptions.AddRange(mensHairOptions);
        mensOptions.AddRange(mensTopOptions);

        womensOptions.AddRange(womensHairOptions);
        womensOptions.AddRange(womensTopOptions);
        womensOptions.AddRange(womensBottomOptions);
        womensOptions.AddRange(womensShoeOptions);
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
        }
    }

    public void SetActiveDNASlot(string _activeDNASlot)
    {
        activeDNASlot = _activeDNASlot;
    }

    public void SetActiveColorField(string _activeColorField)
    {
        activeColorField = _activeColorField;
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

    public void ModifyFieldColor(Color color)
    {
        avatar.SetColor(activeColorField, color);
        avatar.UpdateColors(true);
    }

    public void ModifyFieldColorDarkness(float value)
    {
        //avatar.SetColor(activeColorField, avatar.GetColor(activeColorField) * value);
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

    public void ModifyFullOutfit(string fullOutfit)
    {
        if (currentFullOutfit == fullOutfit)
        {
            avatar.ClearSlot("FullOutfit");
            avatar.BuildCharacter();
            currentFullOutfit = "";
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
            avatar.ClearSlot("Chest");
            avatar.BuildCharacter();
            currentTop = "";
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
            avatar.ClearSlot("Legs");
            avatar.BuildCharacter();
            currentBottom = "";
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
            avatar.ClearSlot("Feet");
            avatar.BuildCharacter();
            currentShoes = "";
        }
        else
        {
            currentShoes = shoes;
            avatar.SetSlot("Feet", shoes);
            avatar.BuildCharacter();
        }
    }

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
