using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoT : MonoBehaviour
{
    GameObject Lens, Border;
    Vector3 expandedScale, contractedScale;
    private bool lensActive = false;
    private float percentage = 0f;
    private float speed = 2.0f;
    float startScale;

    void Start()
    {
        Lens = transform.GetChild(0).gameObject;
        Border = transform.GetChild(1).gameObject;

        Lens.GetComponent<MeshRenderer>().enabled = false;
        Border.GetComponent<MeshRenderer>().enabled = false;

        startScale = Lens.transform.localScale.x;
        expandedScale = new Vector3 (startScale, 0, startScale);
        contractedScale = expandedScale / 2.5f;
    }

    public void LensActivate()
    {
        if (lensActive) {
            StopAllCoroutines();
            StartCoroutine(LensExpand());
            lensActive = !lensActive;
        }
        else {
            StopAllCoroutines();
            StartCoroutine(LensContract());
            lensActive = !lensActive;
        }
    }

    IEnumerator LensContract()
    {
        Lens.GetComponent<MeshRenderer>().enabled = true;
        Border.GetComponent<MeshRenderer>().enabled = true;

        while (percentage < 1) {
            percentage += (speed * Time.deltaTime);
            Lens.transform.localScale = Vector3.Lerp(expandedScale, contractedScale, percentage);
            yield return null;
        }
    }

    IEnumerator LensExpand()
    {
        while (percentage > 0) {
            percentage -= (speed * Time.deltaTime);
            Lens.transform.localScale = Vector3.Lerp(expandedScale, contractedScale, percentage);
            yield return null;
        }

        Lens.GetComponent<MeshRenderer>().enabled = false;
        Border.GetComponent<MeshRenderer>().enabled = false;
    }
}
