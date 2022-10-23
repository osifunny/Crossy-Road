using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject grass;
    [SerializeField] GameObject road;
    [SerializeField] int extent = 7;
    [SerializeField] int frontDistance = 10;
    [SerializeField] int backDistance = -5;
    [SerializeField] int maxSameTerrainRepeat = 3;
    [SerializeField] GameObject scoreMain;
    [SerializeField] GameObject retryButton;
    [SerializeField] GameObject mainButton;
    [SerializeField] GameObject soundToggle;
    Dictionary<int,TerrainBlock> map = new Dictionary<int, TerrainBlock>();
    TMP_Text gameOverText;
    TMP_Text highScore;
    private int playerLastMaxTravel;
    public AudioSource audioSource;

    private void Start(){
        SoundSystem();
        gameOverText = gameOverPanel.GetComponentInChildren<TMP_Text>();
        for(int z = backDistance; z <= 0; z++) CreateTerrain(grass, z);
        for(int z = 1; z <= frontDistance; z++){
            var prefab = GetNextRandomTerrainPrefab(z);
            CreateTerrain(prefab, z);
        }
        player.SetUp(backDistance,extent);
    }

    private void Update(){
        if(player.IsDie && !gameOverPanel.activeInHierarchy) StartCoroutine(ShowGameOverPanel());
        if(player.MaxTravel != playerLastMaxTravel){
            playerLastMaxTravel = player.MaxTravel;
            var randTbPrefab = GetNextRandomTerrainPrefab(player.MaxTravel + frontDistance);
            CreateTerrain(randTbPrefab, player.MaxTravel + frontDistance);
            var lastTB = map[player.MaxTravel-1+backDistance];
            map.Remove(player.MaxTravel-1+backDistance);
            Destroy(lastTB.gameObject);
            player.SetUp(player.MaxTravel+backDistance,extent);
        }
    }

    private void SoundSystem(){
        float bsound = PlayerPrefs.GetFloat("Sound");
        //Debug.Log(bsound);
        if(bsound == 1){
            audioSource.volume = 1;
            soundToggle.GetComponent<Toggle>().isOn = true;
        }
        else{
            soundToggle.GetComponent<Toggle>().isOn = false;
            audioSource.volume = 0;
        
        }
    }

    IEnumerator ShowGameOverPanel(){
        yield return new WaitForSeconds(2);
        scoreMain.SetActive(false);
        soundToggle.SetActive(false);
        gameOverText.text = "Your Score : " + player.MaxTravel;
        retryButton.SetActive(true);
        gameOverPanel.SetActive(true);
        mainButton.SetActive(true);
    }

    private void CreateTerrain(GameObject prefab, int zPos){
        var go = Instantiate(prefab, new Vector3(0,0,zPos), Quaternion.identity);
            var tb = go.GetComponent<TerrainBlock>();
            tb.Build(extent);
            map.Add(zPos,tb);
    }

    private GameObject GetNextRandomTerrainPrefab(int nextPos){
        bool isUniform = true;
        var tbRef = map[nextPos-1];
        for(int distance = 2; distance <= maxSameTerrainRepeat; distance++){
            if(map[nextPos-distance].GetType() != tbRef.GetType()){
                isUniform = false;
                break;
            }
        }
        if(isUniform){
            if(tbRef is Grass) return road;
            else return grass;
        }
        return Random.value > 0.5f? road: grass;
    }

    public void audioSound(bool on){
        if (on) audioSource.volume = 1;
        else audioSource.volume = 0;
        PlayerPrefs.SetFloat("Sound", audioSource.volume);
    }
}
