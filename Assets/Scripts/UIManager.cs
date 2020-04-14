using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _modsPanel = null;

    public void ToggleModsPanel()
    {
        _modsPanel.SetActive(!_modsPanel.activeInHierarchy);
    }
}
