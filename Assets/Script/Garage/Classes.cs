using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Prototype.NetworkLobby;

public class Classes : MonoBehaviour {
    //public Dropdown ClassDrop;
    public GameObject DPSPrefab;
    public GameObject EngineerPrefab;
    public GameObject TankPrefab;
    public AudioSource ClickAudio;
    [Header("UI")]
    public Text HaveRepairText;
    public Text NotHaveRepairText;
    public Button DPSButton;
    public Button EngineerButton;
    public Button TankButton;
    public Slider SpeedSlider;
    public Slider HealthSlider;
    public Slider ShieldSlider;
    public GameObject LobbyMainPanel;
    public GameObject SelectPanel;
    private void Start()
    {
        OnClickDPS();
    }
    /*
    public void DropDown()
    {
        if(ClassDrop.value == 0)
        {
            DPSPrefab.SetActive(true);
            if(EngineerPrefab.activeInHierarchy) EngineerPrefab.SetActive(false);
            if (TankPrefab.activeInHierarchy) TankPrefab.SetActive(false);
            PlayerPrefs.SetString("Class", "DPS");
        }
        else if (ClassDrop.value == 1)
        {
            EngineerPrefab.SetActive(true);
            if (DPSPrefab.activeInHierarchy) DPSPrefab.SetActive(false);
            if (TankPrefab.activeInHierarchy) TankPrefab.SetActive(false);
            PlayerPrefs.SetString("Class", "Engineer");
        }
        else
        {
            TankPrefab.SetActive(true);
            if (DPSPrefab.activeInHierarchy) DPSPrefab.SetActive(false);
            if (EngineerPrefab.activeInHierarchy) EngineerPrefab.SetActive(false);
            PlayerPrefs.SetString("Class", "Tank");
        }
        PlayClickSound();
    }*/
    public void OnClickOK()
    {
        //SceneManager.LoadScene("NetworkLobby12");
        SelectPanel.SetActive(false);
        LobbyMainPanel.SetActive(true);
    }
    public void PlayClickSound()
    {
        ClickAudio.Play();
        
    }
public void OnClickDPS()
    {
        DPSPrefab.SetActive(true);
        if (EngineerPrefab.activeInHierarchy) EngineerPrefab.SetActive(false);
        if (TankPrefab.activeInHierarchy) TankPrefab.SetActive(false);      
        if (HaveRepairText.enabled) HaveRepairText.enabled = false;
        NotHaveRepairText.enabled = true;
        //Set Sliders Value 
        SpeedSlider.value = 150;
        HealthSlider.value = 80;
        ShieldSlider.value = 40;
        DPSButton.interactable = false;
        TankButton.interactable = true;
        EngineerButton.interactable = true;
        PlayerPrefs.SetString("Class", "DPS");
    }
    public void OnClickEngineer()
    {
        EngineerPrefab.SetActive(true);
        if (DPSPrefab.activeInHierarchy) DPSPrefab.SetActive(false);
        if (TankPrefab.activeInHierarchy) TankPrefab.SetActive(false);
        if (NotHaveRepairText.enabled) NotHaveRepairText.enabled = false;
        HaveRepairText.enabled = true;
        //Set Sliders Value
        SpeedSlider.value = 125;
        HealthSlider.value = 100;
        ShieldSlider.value = 50;
        DPSButton.interactable = true;
        TankButton.interactable = true;
        EngineerButton.interactable = false;
        PlayerPrefs.SetString("Class", "Engineer");
    }
    public void OnClickTank()
    {
        TankPrefab.SetActive(true);
        if (DPSPrefab.activeInHierarchy) DPSPrefab.SetActive(false);
        if (EngineerPrefab.activeInHierarchy) EngineerPrefab.SetActive(false);
        // Repair Feature
        if (HaveRepairText.enabled) HaveRepairText.enabled = false;
        NotHaveRepairText.enabled = true;
        //Set Sliders Value
        SpeedSlider.value = 100;
        HealthSlider.value = 120;
        ShieldSlider.value = 60;
        DPSButton.interactable = true;
        TankButton.interactable = false;
        EngineerButton.interactable = true;
        PlayerPrefs.SetString("Class", "Tank");
    }
    
}
