using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovementBuka : MonoBehaviour
{
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject fpsCam;
    [SerializeField] private GameObject head;
    [SerializeField] private GameObject camera;
    [SerializeField] private float speed;
    [SerializeField] private ParticleSystem particles;

    private Rigidbody rigidbody;
    private Animator animator;
    private bool isScreamer = false;

    

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        if (player != null)
        {
            MoveToPlayer();
            animator.SetBool("Chase", true);
        }
        else
        {
            animator.SetBool("Chase", false);
        }

    }

    private void MoveToPlayer()
    {
        if (!isScreamer)
        {
            speed += Time.deltaTime / 5f;
            transform.position = Vector3.Lerp(transform.position, player.transform.position, speed * Time.deltaTime);
            transform.LookAt(player.transform);
        }
        else
        {
            var lookPos = camera.transform.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 100f * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isScreamer)
        {
            //Instantiate(camera, transform);
            //camera.transform.localPosition = transform.position + transform.forward * 1.8f + transform.up * 1.3f;
            //camera.transform.LookAt(head.transform);
            fpsCam.SetActive(false);
            camera.SetActive(true);
            particles.Stop();
            animator.SetBool("isScreamer", true);
            StartCoroutine(nameof(GameOver));
            StartCoroutine(nameof(PlaySound));
            isScreamer = true;
            FindObjectOfType<AudioLevelManager>().Play("Screamer");
        }
    }

    private IEnumerator PlaySound()
    {
        yield return new WaitForSeconds(0.75f);
        FindObjectOfType<AudioLevelManager>().Play("Punch");
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2f);
        pauseMenu.GameOver();
    }

    //   private NavMeshAgent _agent;

    //   void Start()
    //   {
    //       _agent = GetComponent<NavMeshAgent>();
    //   }

    //private void FixedUpdate() {
    //       _agent.SetDestination(Player.position);
    //   }
}
