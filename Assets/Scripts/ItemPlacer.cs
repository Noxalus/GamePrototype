using UnityEngine;

namespace GamePrototype
{
    public class ItemPlacer : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera = null;

        [SerializeField]
        private Material _ghostMaterial = null;

        public delegate void ItemPlacedEventHandler(GameObject instance);
        public event ItemPlacedEventHandler OnItemPlaced;

        public delegate void CurrentItemChangedEventHandler(string itemName);
        public event CurrentItemChangedEventHandler OnItemChanged;

        private GameObject _currentItem = null;
        private GameObject _ghostItem = null;
        private Renderer _ghostRenderer = null;

        public void SetItem(GameObject item)
        {
            if (_ghostRenderer != null)
            {
                Destroy(_ghostItem);
            }

            _currentItem = item;

            _ghostItem = Instantiate(item);
            _ghostRenderer = _ghostItem.GetComponentInChildren<Renderer>();
            _ghostRenderer.material = _ghostMaterial;
            _ghostRenderer.enabled = false;

            OnItemChanged?.Invoke(item.name);
        }

        void Update()
        {
            if (_currentItem != null)
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                bool isOnGround = false;

                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                {
                    _ghostRenderer.enabled = true;
                    _ghostItem.transform.position = hit.point;
                    isOnGround = true;
                }
                else
                {
                    _ghostRenderer.enabled = false;
                }

                if (isOnGround && Input.GetKeyDown(KeyCode.Mouse0))
                {
                    GameObject instance = Instantiate(
                        _currentItem,
                        _ghostItem.transform.position,
                        _ghostItem.transform.rotation
                    );

                    OnItemPlaced?.Invoke(instance);
                }
            }
        }
    }
}