using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using MicroRuleEngine;
using Newtonsoft.Json;
using System;
using UnityEngine.Audio;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject playerOnePrefab;
    [SerializeField] GameObject playerTwoPrefab;
    private GameObject pOneInstance;
    private GameObject pTwoInstance;
    [SerializeField]
    private AudioMixer fxMixer;

    private Func<SaveGameData, bool> p1Wins;
    private Func<SaveGameData, bool> p2Wins;

    [SerializeField] private GameObject UI;
    private UIController UIController;//Controller for UI elements

    [SerializeField] private AudioSource backgroundMusic;
    //Key to store and retrieve background music volume setting
    private const string musicVolumeKey = "music_volume";
    private const string sfxVolumeKey = "sfx_volume";

    // Start is called before the first frame update
    void Start()
    {
        this.UIController = this.UI.GetComponent<UIController>();  
        this.LoadMusicVolume();
        //this.CreateRules();        

        StartGame(new Vector3(14.9899979f,6.25f,20.0499992f), this.playerOnePrefab.transform.rotation, 
            new Vector3(17.4899979f,6.25f,20.0499992f), this.playerTwoPrefab.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.pOneInstance != null && this.pTwoInstance != null)
        {
            SaveGameData players = this.CreateSaveGameData();

            UIController.PlayerWins(players);
        }       
    }

    //Tell the UI controller to update the slider's position
    private void SetMusicSliderValue(float value)
    {
        this.UIController.MusicSliderValue(value);
    }

    private void SetFXSliderValue(float value)
    {
        this.UIController.FXSliderValue(value);
    }

    //Get the last saved value for the background music's volume
    public void LoadMusicVolume()
    {
        if (PlayerPrefs.HasKey(musicVolumeKey))
        {
            float value = PlayerPrefs.GetFloat(musicVolumeKey);
            this.backgroundMusic.volume = value;
            this.SetMusicSliderValue(value);
        }
        else
        {
            float value = this.backgroundMusic.volume;
            this.SetMusicSliderValue(value);
        }

        if (PlayerPrefs.HasKey(sfxVolumeKey))
        {
            float value = PlayerPrefs.GetFloat(sfxVolumeKey);
            this.SetFXSliderValue(value);
        }
        else
        {
            float value;
            this.fxMixer.GetFloat("Volume", out value);
            this.SetFXSliderValue(value);
        }
    }

    //Change the background music's volume
    public void VolumeChange(float value)
    {
        this.backgroundMusic.volume = value;
    }

    public void FXChange(float value)
    {
        this.fxMixer.SetFloat("Volume", value);
    }

    //Save the current value for the background music's volume
    public void SaveSettings(float musicValue, float fxValue)
    {
        PlayerPrefs.SetFloat(musicVolumeKey, musicValue);
        PlayerPrefs.SetFloat(sfxVolumeKey, fxValue);
        PlayerPrefs.Save();
    }

    //Load a new game
    public void NewGame()
    {
        SceneManager.LoadScene(0);//Reload the scene
        Time.timeScale = 1;
    }

    //Create an instance of SaveGameData to be serialized as a binary file
    private SaveGameData CreateSaveGameData()
    {
        List<float> playerOnePos = new List<float>();
        playerOnePos.Add(pOneInstance.transform.position.x);
        playerOnePos.Add(pOneInstance.transform.position.y);
        playerOnePos.Add(pOneInstance.transform.position.z);
        List<float> playerOneRot = new List<float>();
        playerOneRot.Add(pOneInstance.transform.rotation.x);
        playerOneRot.Add(pOneInstance.transform.rotation.y);
        playerOneRot.Add(pOneInstance.transform.rotation.z);
        playerOneRot.Add(pOneInstance.transform.rotation.w);

        List<float> playerTwoPos = new List<float>();
        playerTwoPos.Add(pTwoInstance.transform.position.x);
        playerTwoPos.Add(pTwoInstance.transform.position.y);
        playerTwoPos.Add(pTwoInstance.transform.position.z);
        List<float> playerTwoRot = new List<float>();
        playerTwoRot.Add(pTwoInstance.transform.rotation.x);
        playerTwoRot.Add(pTwoInstance.transform.rotation.y);
        playerTwoRot.Add(pTwoInstance.transform.rotation.z);
        playerTwoRot.Add(pTwoInstance.transform.rotation.w);

        return new SaveGameData(playerOnePos, playerOneRot, playerTwoPos, playerTwoRot);
    }

    //Serialize an instance of SaveGameData and save it as a binary file
    public void SaveGame()
    {
        SaveGameData save = this.CreateSaveGameData();

        //Save as binary so player can't cheat
        var bf = new BinaryFormatter();
        var filePath = Application.persistentDataPath + "/gamesave.data";

        var fs = File.Create(filePath);
        bf.Serialize(fs, save);
    }

    //Load a binary file and deserialize it a SaveGameData instance to put the balls and score as they where on the saved game
    public void LoadGame()
    {        
        var filePath = Application.persistentDataPath + "/gamesave.data";

        if (File.Exists(filePath))
        {           
            var bf = new BinaryFormatter();
            var fs = File.Open(filePath, FileMode.Open);
            var saveData = (SaveGameData)bf.Deserialize(fs);

            LoadPlayers(saveData);
        }
    }

    void LoadPlayers(SaveGameData saveData)
    {
        Vector3 playerOneVec = new Vector3();
        playerOneVec.x = saveData.playerOnePos[0];
        playerOneVec.y = saveData.playerOnePos[1];
        playerOneVec.z = saveData.playerOnePos[2];

        Quaternion playerOneQuat = new Quaternion();
        playerOneQuat.x = saveData.playerOneRot[0];
        playerOneQuat.y = saveData.playerOneRot[1];
        playerOneQuat.z = saveData.playerOneRot[2];
        playerOneQuat.w = saveData.playerOneRot[3];

        Vector3 playerTwoVec = new Vector3();
        playerTwoVec.x = saveData.playerTwoPos[0];
        playerTwoVec.y = saveData.playerTwoPos[1];
        playerTwoVec.z = saveData.playerTwoPos[2];

        Quaternion playerTwoQuat = new Quaternion();
        playerTwoQuat.x = saveData.playerTwoRot[0];
        playerTwoQuat.y = saveData.playerTwoRot[1];
        playerTwoQuat.z = saveData.playerTwoRot[2];
        playerTwoQuat.w = saveData.playerTwoRot[3];

        Destroy(pOneInstance);
        Destroy(pTwoInstance);

        StartGame(playerOneVec, playerOneQuat, playerTwoVec, playerTwoQuat);
    }

    void StartGame(Vector3 playerOnePos, Quaternion playerOneRot, Vector3 playerTwoPos, Quaternion playerTwoRot)
    {
        this.pOneInstance=Instantiate(playerOnePrefab, playerOnePos, playerOneRot);
        this.pTwoInstance=Instantiate(playerTwoPrefab, playerTwoPos, playerTwoRot);
    }

    //Quit game
    public void QuitGame()
    {
        Application.Quit();
    }

    private void CreateRules()
    {
        string jsonContents = File.ReadAllText(@".\Assets\Scripts\PlayerRules.json");
        List<Rule> rulesList = JsonConvert.DeserializeObject<List<Rule>>(jsonContents);
        MRE engine = new MRE();
        this.p1Wins = engine.CompileRule<SaveGameData>(rulesList[0]);
        this.p2Wins = engine.CompileRule<SaveGameData>(rulesList[1]);
    }
}
