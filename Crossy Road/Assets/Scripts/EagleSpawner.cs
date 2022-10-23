using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EagleSpawner : MonoBehaviour
{
    [SerializeField] GameObject eaglePrefab;
    [SerializeField] int spawnZPos = 10;
    [SerializeField] Player player;
    [SerializeField] float timeOut = 8;
    float timer = 0;
    int playerLastMaxTravel = 0;

    private void Start(){
    }

    private void SpawnEagle(){
        player.enabled = false;
        var position = new Vector3(player.transform.position.x, 1, player.CurrentTravel+spawnZPos);
        var rotation = Quaternion.Euler(0, 180, 0);
        var eagleObject = Instantiate(eaglePrefab,position, rotation);
        var eagle = eagleObject.GetComponent<Eagle>();
        eagle.SetUpTarget(player);
    }

    private void Update(){
        if(player.MaxTravel != playerLastMaxTravel){
            timer = 0;
            playerLastMaxTravel = player.MaxTravel;
            return;
        }
        if(timer < timeOut){
            timer += Time.deltaTime;
            return;
        }
        if(!player.IsJumping() && !player.IsDie) SpawnEagle();
    }
}
