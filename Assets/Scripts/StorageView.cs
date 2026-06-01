using TMPro;
using UnityEngine;

public class StorageView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Storage _storage;

    private void OnEnable()
    {
        _storage.OnCapacityChanged += RefreshView;
    }

    private void OnDisable()
    {
        _storage.OnCapacityChanged -= RefreshView;
    }


    private void RefreshView(int capacity)
    {
        _text.text = $"Capacity minerals: {capacity}";
    }
}
