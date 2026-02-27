using UnityEngine;
using UnityEngine.AI;

public class gnomomover : MonoBehaviour
{
    private NavMeshAgent agent;
    private Camera cam;
    public Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        cam = Camera.main;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Lanza el rayo contra la escena
            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
                
            }
        }
       

         if (agent.velocity.magnitude > 0.1f)
                animator.SetBool("isWalking", true);
        else
            animator.SetBool("isWalking", false);
        }
}