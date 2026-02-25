using UnityEngine;

public class blendTree : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidad = 5f;

    [Header("Rotación")]
    [SerializeField] private float velocidadRotacion = 10f;

    [Header("Animator")]
    [SerializeField] private Animator animator;
    [SerializeField] private float damping = 0.1f;

    void Update()
    {
        // 1️⃣ Input
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        // 2️⃣ Dirección
        Vector3 direccion = new Vector3(inputX, 0, inputY);
        direccion = Vector3.ClampMagnitude(direccion, 1f);

        // 3️⃣ ROTAR hacia la dirección de movimiento
        if (direccion != Vector3.zero)
        {
            Quaternion rotacionObjetivo = Quaternion.LookRotation(direccion);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                rotacionObjetivo,
                velocidadRotacion * Time.deltaTime
            );
        }

        // 4️⃣ MOVER en la dirección en la que está mirando
        transform.Translate(Vector3.forward * direccion.magnitude * velocidad * Time.deltaTime);

        // 5️⃣ Animator
        animator.SetFloat("velx", direccion.x, damping, Time.deltaTime);
        animator.SetFloat("vely", direccion.z, damping, Time.deltaTime);
    }
}