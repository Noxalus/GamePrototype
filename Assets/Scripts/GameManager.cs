using UnityEngine;
using UnityEngine.UI;

namespace GamePrototype
{
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField]
        private ItemPlacer _itemPlacer = null;

        [SerializeField]
        private Text _selectedItemText = null;

        private void Start()
        {
            _itemPlacer.OnItemChanged += OnItemChanged;
            _itemPlacer.OnItemPlaced += OnItemPlaced;
        }

        private void OnItemChanged(string itemName)
        {
            _selectedItemText.text = $"Selected item: {itemName}";
        }

        private void OnItemPlaced(GameObject instance)
        {
            Debug.Log($"Placed an item at {instance.transform.position.ToString()}");
        }

        public void SetItem(GameObject item)
        {
            _itemPlacer.SetItem(item);
        }
    }
}