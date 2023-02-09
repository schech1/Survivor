using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] TMP_Text levelText;
    [SerializeField] TMP_Text resource1Text;
    [SerializeField] TMP_Text resource2Text;
    [SerializeField] TMP_Text resource3Text;
    [SerializeField] TMP_Text timerText;
    [SerializeField] SOLevelData levelData;
    [SerializeField] SOPlayerData playerData;
    float timer;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        levelText.text = playerData.currentPlayerHealth.ToString();
        resource1Text.text = playerData.resource[0].ToString();
        resource2Text.text = playerData.resource[1].ToString();
        resource3Text.text = playerData.resource[2].ToString();
        timerText.text = (levelData.roundTime - timer).ToString();

        timer += Time.deltaTime;

        if (timer > levelData.roundTime)
        {
            GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in allEnemies)
            {
                Destroy(enemy);
            }

            timer = 0;

        }
    }
}
