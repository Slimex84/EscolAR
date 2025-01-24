using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))] //Obliga a tener una colision
public class ARPortalColision : MonoBehaviour
{
    [SerializeField] private bool _executeOnStart;//Para ejecutar recien comienza

    public UnityEvent OnColision;

    private void Start()
    {
        if (_executeOnStart)
        {
            OnColision.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        OnColision.Invoke();
    }

}