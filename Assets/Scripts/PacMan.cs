using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacMan : MonoBehaviour
{
    public float speed = 8;
    public bool energized;
    public float delay = 3;

    public AudioClip sirenClip;
    public AudioClip munchClip;
    public AudioClip energizerClip;
    private AudioSource audioSource;

    Animator cororido;

    // Start is called before the first frame update
    void Start()
    {
        energized = false;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = sirenClip;
        audioSource.loop = true;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        // Get user input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        transform.Rotate(0, x * speed * 10 * Time.deltaTime, 0);

        Vector3 move = transform.forward * z;
        gameObject.GetComponent<CharacterController>().Move(move * Time.deltaTime * speed);

    }

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Pellet")
        {
            audioSource.PlayOneShot(munchClip);
            Destroy(other.gameObject);
        }
        if (other.gameObject.tag == "Energizer")
        {
            audioSource.PlayOneShot(energizerClip);
            Destroy(other.gameObject);
            StartCoroutine(Energizer());
        }
        if (other.gameObject.tag == "Ghost")
        {
            if (energized)
                Destroy(other.gameObject);
            else
                Destroy(gameObject);
        }

    }

    IEnumerator Energizer()
    {
        GameObject[] ghosts = GameObject.FindGameObjectsWithTag("Ghost");
        energized = true;
        GameObject[] paredes = GameObject.FindGameObjectsWithTag("Maze");
        foreach (GameObject parede in paredes)
        {
            cororido = parede.GetComponent<Animator>();
            cororido.enabled = true;
        }
        foreach (GameObject ghost in ghosts)
        {
            ghost.GetComponent<Light>().intensity = 20;
            //ghost.GetComponent<MeshRenderer>().material.color = Color.blue;
        }
        yield return new WaitForSeconds(delay);
        energized = false;
        foreach (GameObject ghost in ghosts)
        {
            ghost.GetComponent<Light>().intensity = 0;
            //ghost.GetComponent<MeshRenderer>().material.color = Color.white;
        }
        foreach (GameObject parede in paredes)
        {
            cororido = parede.GetComponent<Animator>();
            cororido.enabled = false;
        }
    }



}
