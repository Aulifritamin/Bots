using UnityEngine;

public class Flag : MonoBehaviour
{
    public void SetPosition(Vector3 position)
    {
        transform.position = position;
        ActivateFlag();
    }

    public void ResetFlag()
    {
        gameObject.SetActive(false);
    }

    public void ActivateFlag()
    {
        gameObject.SetActive(true);
    }

    public Vector3 Position()
    {
        return transform.position;
    }
}
