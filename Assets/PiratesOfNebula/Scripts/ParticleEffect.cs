using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour , IpooledObject
{
    public Vector2 v2;
    public float Livetime=0;

    public void OnObjectSpawn()
    {
      StartCoroutine(Disapear());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2()
        {
        x = GetComponent<RectTransform>().anchoredPosition.x + v2.x * Time.deltaTime,
        y = GetComponent<RectTransform>().anchoredPosition.y + v2.y * Time.deltaTime
       
      };
    }
    IEnumerator Disapear()
    {
        yield return new WaitForSeconds(Livetime);
        gameObject.SetActive(false);
    }
}
