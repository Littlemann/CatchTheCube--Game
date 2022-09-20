using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class LevelController : MonoBehaviour
{
    public GameObject currentLevel;
    public GameObject lastLevel;
    [SerializeField] private GameObject[] levels;
    [SerializeField] GameObject continueBut, replayBut;
    GameManager gameManager;
    Transform levelSonuCP;
    Transform oncekiCheck;
    int curLevel;

    [SerializeField]
    TextMeshProUGUI levelText, nextLevelText;
    List<GameObject> sahnedekiLeveller = new List<GameObject>();

    void Awake()
    {
        SetButtons(false);
        if (PlayerPrefs.HasKey("level"))
        {
            PlayerPrefs.SetInt("level", 1);
        }
        gameManager = FindObjectOfType<GameManager>();
        CreateSameLevel();
        
    }

    public void SetButtons(bool tr)
    {
        continueBut.SetActive(tr);
        replayBut.SetActive(tr);
    }
    public void NextLevel() 
    {
        curLevel = PlayerPrefs.GetInt("level") + 1;
        PlayerPrefs.SetInt("level", curLevel);
        levelText.text = curLevel.ToString();
        nextLevelText.text = (curLevel + 1).ToString();
        lastLevel = currentLevel;
        currentLevel = levels[curLevel - 1];

        if (oncekiCheck != null)
        {
            Destroy(oncekiCheck.gameObject);
        }

        levelSonuCP = GameObject.FindGameObjectWithTag("levelSonuCP").transform;
        oncekiCheck = levelSonuCP;

        if (PlayerPrefs.GetInt("level") >= 10)
        {
            currentLevel = Instantiate(levels[Random.Range(0, levels.Length)], oncekiCheck.position, Quaternion.identity);
        }
        else
        {
            currentLevel = Instantiate(levels[PlayerPrefs.GetInt("level") - 1], oncekiCheck.position, Quaternion.identity);
        }

        sahnedekiLeveller.Add(currentLevel);

        if (sahnedekiLeveller.Count >= 2)
        {
            sahnedekiLeveller.Remove(sahnedekiLeveller[0]);
        }
        SetButtons(false);
    }

    public void DestroyCurrentLevel()
    {
        Destroy(currentLevel);
    }

    public void DestroyLastLevel()
    {
        Destroy(lastLevel);
    }

    public void CreateSameLevel()
    {
        
        if (currentLevel != null)
        {
            DestroyCurrentLevel();
        }
        currentLevel = Instantiate(levels[PlayerPrefs.GetInt("level") - 1]);
        gameManager.SendToCharacterStart();
        SetButtons(false);
    }

    public void CreateSameLevelWithCheckPoint()
    {
        DestroyCurrentLevel();
        currentLevel = Instantiate(levels[PlayerPrefs.GetInt("level")-1]);
        gameManager.SendToCharacterPoint();
        SetButtons(false);
    }
}
