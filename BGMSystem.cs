using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMSystem : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] bgms;

    public bool StartByDefault = true;
    // Start is called before the first frame update
    void Start()
    {
        if (StartByDefault) Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        PlayById(Random.Range(0, bgms.Length));
    }

    public void PlayById(int id)
    {
        audioSource.clip = bgms[id];
        audioSource.Play();
    }

    public void Stop()
    {
        audioSource.Stop();
    }
}
