using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using MicroRuleEngine;
using Newtonsoft.Json;
using System;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject controller;
    private GameController gameControl;//Reference to the game controller
    [SerializeField] public GameObject settingsMenu;//The settings menu that can be accessed pressing Escape
    [SerializeField] private Slider volumeSlider;//Slider to change music volume
    [SerializeField] private Slider sfxSlider;//Slider to change sound effects
    [SerializeField] private Button saveSettings;//Save music volume setting
    [SerializeField] private Button newGame;
    [SerializeField] private Button saveGame;
    [SerializeField] private Button loadGame;
    [SerializeField] private Button quitGame;
    [SerializeField] private GameObject gameWonObj;
    [SerializeField] private TextMeshProUGUI gameWonText;

    private Func<SaveGameData, bool> p1Wins;
    private Func<SaveGameData, bool> p2Wins;

    // Start is called before the first frame update
    void Start()
    {
        this.settingsMenu.SetActive(false);//Settings menu is always inactive on game start
        this.gameWonObj.SetActive(false);
        this.gameControl = this.controller.GetComponent<GameController>();
        //Add listeners for the slider and buttons
        this.volumeSlider.onValueChanged.AddListener(delegate { VolumeValueChange(); });
        this.sfxSlider.onValueChanged.AddListener(delegate { FXValueChange(); });
        this.saveSettings.onClick.AddListener(delegate { SaveSetingsClicked(); });
        this.newGame.onClick.AddListener(delegate { NewGameClicked(); });
        this.saveGame.onClick.AddListener(delegate { SaveGameClicked(); });
        this.loadGame.onClick.AddListener(delegate { LoadGameClicked(); });
        this.quitGame.onClick.AddListener(delegate { QuitGameClicked(); });

        this.CreateRules();        

    }

    private void Update() 
    {
        //Open or close settings menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {          
            if (!this.GetSettingsMenuActive())//If not on pause, pause the game
            {
                this.SetSettingsMenuActive(true);
                Time.timeScale = 0;
            }
            else if(this.GetSettingsMenuActive())
            {
                this.SetSettingsMenuActive(false);
                Time.timeScale = 1;
            }
        } 
    }

    private void CreateRules()
    {
        string jsonContents = File.ReadAllText(@".\Assets\Scripts\PlayerRules.json");
        List<Rule> rulesList = JsonConvert.DeserializeObject<List<Rule>>(jsonContents);
        MRE engine = new MRE();
        this.p1Wins = engine.CompileRule<SaveGameData>(rulesList[0]);
        this.p2Wins = engine.CompileRule<SaveGameData>(rulesList[1]);
    }

    public void PlayerWins(SaveGameData players)
    {
        if (this.p1Wins(players))
        {
            GameWon(1);
            Time.timeScale = 0;
        }

        if (this.p2Wins(players))
        {
            GameWon(2);
            Time.timeScale = 0;
        }
    }

    //Change the slider's value
    public void MusicSliderValue(float value)
    {
        this.volumeSlider.value = value;
    }

    public void FXSliderValue(float value)
    {
        this.sfxSlider.value = value;
    }

    //Change the SettingsMenu to active or inactive (visible or not)
    public void SetSettingsMenuActive(bool status)
    {      
        this.settingsMenu.SetActive(status);
    }

    //Check status of the settings menu
    public bool GetSettingsMenuActive()
    {
        return this.settingsMenu.activeSelf;
    }

    //Tell the game controller to change the music volume
    private void VolumeValueChange()
    {
        this.gameControl.VolumeChange(this.volumeSlider.value);
    }

    private void FXValueChange()
    {
        this.gameControl.FXChange(this.sfxSlider.value);
    }

    //Tell the game controller to save the settings
    private void SaveSetingsClicked()
    {
        this.gameControl.SaveSettings(this.volumeSlider.value, this.sfxSlider.value);
    }

    //Tell the game controller to load a new game
    private void NewGameClicked()
    {
        this.gameControl.NewGame();
    }

    //Tell the game controller to save the current game
    private void SaveGameClicked()
    {
        this.gameControl.SaveGame();
    }

    //Tell the game controller to load the last saved game
    private void LoadGameClicked()
    {
        this.gameControl.LoadGame();
    }

    //Tell the game controller to quit the game
    private void QuitGameClicked()
    {
        this.gameControl.QuitGame();
    }

    public void GameWon(int player)
    {
        Debug.Log("Won");
        if (player == 1)
        {
            gameWonObj.SetActive(true);
            this.gameWonText.text = "Player One Won!";
        }
        else if (player == 2)
        {
            gameWonObj.SetActive(true);
            this.gameWonText.text = "Player Two Won!";
        }
    }
}
