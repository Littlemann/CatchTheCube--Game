using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlatformController : MonoBehaviour
{
    public int objectCount = 0;

    [SerializeField] ParticleSystem confettiSfx;
    [SerializeField] int neededObjectCount;
    [SerializeField] Text objectCountCheckText;
    [SerializeField] GameObject plane, gecitSag,gecitSol;
    [SerializeField] AudioClip wonSfx;



    Player player;
    GameManager gameManager;
    LevelController levelController;

    


    private bool islemlerBasladiMi;
    void Start()
    {
        player = FindObjectOfType<Player>();
        gameManager = FindObjectOfType<GameManager>();
        levelController = FindObjectOfType<LevelController>();
        islemlerBasladiMi = false;

    }
    void Update()
    {
        objectCountCheckText.text = (objectCount.ToString() + "/" + neededObjectCount.ToString());
    }

    public void ObjeZemineÇarptý()
    {
        if(!islemlerBasladiMi)
        { 
            StartCoroutine(Couru());
            islemlerBasladiMi = true;
        }
    }


    IEnumerator Couru()
    {
        yield return new WaitForSeconds(2.2f);

        if(objectCount >= neededObjectCount)
        {
            plane.transform.DOLocalMove(new Vector3(0, -transform.position.y, 0), 1f);
            plane.transform.DOScale(new Vector3(1, 1, 1.05f), 1f).SetEase(Ease.OutBack);
            yield return new WaitForSeconds(1f);
            gecitSag.GetComponent<Animator>().enabled = true;
            gecitSol.GetComponent<Animator>().enabled = true;
            yield return new WaitForSeconds(1f);
            var confetti = Instantiate(confettiSfx, new Vector3(transform.position.x, transform.position.y + 5.5f, transform.position.z + 5.5f),Quaternion.identity);
            confetti.transform.parent = transform;
            AudioSource.PlayClipAtPoint(wonSfx, transform.position);
            player.SizeUp();
            gameManager.passedPlatform++;
            gameManager.GetLevelSteps();
        }
        else
        {
            levelController.SetButtons(true);
            islemlerBasladiMi = true;
        }
    }
}
