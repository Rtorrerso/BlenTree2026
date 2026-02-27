using UnityEngine;
using UnityEngine.AI;

public class patrullaje : MonoBehaviour
{
    [Header("Ruta")]
    [SerializeField] private Transform patrolRoute;   // Arrastra aquí el PatrolRoute (padre)
    [SerializeField] private bool loop = true;        // true = vuelve al inicio
    [SerializeField] private bool randomOrder = false;// true = puntos aleatorios

    [Header("Llegada")]
    [SerializeField] private float reachDistance = 0.3f;  // tolerancia de llegada
    [SerializeField] private float waitTime = 0.5f;       // espera en cada punto

    private NavMeshAgent agent;
    private Transform[] points;
    private int index = 0;
    private float waitTimer = 0f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        if (patrolRoute == null)
        {
            Debug.LogError("PatrolAgent: Asigna 'patrolRoute' (PatrolRoute padre) en el Inspector.");
            enabled = false;
            return;
        }

        // Cargar hijos como puntos
        points = new Transform[patrolRoute.childCount];
        for (int i = 0; i < patrolRoute.childCount; i++)
            points[i] = patrolRoute.GetChild(i);

        if (points.Length == 0)
        {
            Debug.LogError("PatrolAgent: PatrolRoute no tiene hijos (puntos).");
            enabled = false;
            return;
        }

        // Arrancar
        index = randomOrder ? Random.Range(0, points.Length) : 0;
        GoToPoint(index);
    }

    void Update()
    {
        if (points == null || points.Length == 0) return;

        // Si está calculando ruta o no tiene ruta aún, no evalúes llegada
        if (agent.pathPending) return;

        // Llegó (considera stoppingDistance + reachDistance)
        float arriveThreshold = agent.stoppingDistance + reachDistance;
        if (agent.remainingDistance <= arriveThreshold)
        {
            // Si ya está prácticamente detenido
            if (!agent.hasPath || agent.velocity.sqrMagnitude < 0.01f)
            {
                waitTimer += Time.deltaTime;
                if (waitTimer >= waitTime)
                {
                    waitTimer = 0f;
                    AdvanceIndex();
                }
            }
        }
    }

    private void GoToPoint(int i)
    {
        if (i < 0 || i >= points.Length) return;

        // Asegura que el destino esté sobre NavMesh (mejor práctica)
        NavMeshHit hit;
        Vector3 target = points[i].position;

        if (NavMesh.SamplePosition(target, out hit, 1.0f, NavMesh.AllAreas))
            agent.SetDestination(hit.position);
        else
            agent.SetDestination(target);
    }

    private void AdvanceIndex()
    {
        if (randomOrder)
        {
            int next = index;
            if (points.Length > 1)
                while (next == index) next = Random.Range(0, points.Length);
            index = next;
            GoToPoint(index);
            return;
        }

        index++;

        if (index >= points.Length)
        {
            if (!loop)
            {
                enabled = false;
                return;
            }
            index = 0;
        }

        GoToPoint(index);
    }
}