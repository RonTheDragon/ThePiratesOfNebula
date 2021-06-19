using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffect : MonoBehaviour , IpooledObject
{
    public Vector2 v2;
    public float Livetime=0;
    public ParticleSystem particle;
    public int Amount;
    RectTransform r;

    public void OnObjectSpawn()
    {
      StartCoroutine(Disapear());
      if (particle!=null)particle.Emit(Amount);
      StartCoroutine(PlayTheSound());
    }

    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (r != null)
        {
            r.anchoredPosition = new Vector2()
            {
                x = r.anchoredPosition.x + v2.x * Time.deltaTime,
                y = r.anchoredPosition.y + v2.y * Time.deltaTime

            };
        }
    }
    IEnumerator Disapear()
    {
        yield return new WaitForSeconds(Livetime);
        gameObject.SetActive(false);
    }
    IEnumerator PlayTheSound()
    {
        yield return new WaitForSeconds(0.01f);
        GetComponent<AudioManager>()?.PlaySound(Sound.Activation.ParticleSpawn);
    }
}
