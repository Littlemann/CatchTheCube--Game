using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zemin : MonoBehaviour
{
    PlatformController platformController;

    bool carptiMi;

    [SerializeField] AudioClip hitSfx;

    void Start()
    {
        carptiMi = false;
        platformController = GetComponentInParent<PlatformController>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Obje"))
        {
            if (!carptiMi)
            {
                platformController.ObjeZemineÇarptý();
                carptiMi = true;
            }
            AudioSource.PlayClipAtPoint(hitSfx, transform.position);
            collision.gameObject.tag = "Untagged";
            platformController.objectCount++;
            Destroy(collision.gameObject, 3f);
        }
    }
}
