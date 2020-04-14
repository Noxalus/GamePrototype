using UnityEngine;
using UnityEngine.UI;
using ModTool;
using TMPro;

namespace GamePrototype
{
    public class ModItem : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _modName = null;

        [SerializeField]
        private TextMeshProUGUI _modType = null;

        [SerializeField]
        private Toggle _toggle = null;

        private Mod _mod;

        /// <summary>
        /// Initialze this ModItem with a Mod and ModMenu.
        /// </summary>
        /// <param name="mod"></param>
        /// <param name="modMenu"></param>
        public void Initialize(Mod mod, Transform menuContentPanel)
        {
            _mod = mod;

            transform.SetParent(menuContentPanel, false);

            _modName.text = mod.name;
            _modType.text = mod.contentType.ToString();

            _toggle.isOn = mod.isEnabled;

            _toggle.onValueChanged.AddListener(value => Toggle(value));
        }

        /// <summary>
        /// Toggle whether the mod should be loaded
        /// </summary>
        public void Toggle(bool isEnabled)
        {
            _mod.isEnabled = isEnabled;
        }

        /// <summary>
        /// Enable or disable this ModItem's toggle.
        /// </summary>
        /// <param name="interactable"></param>
        public void SetToggleInteractable(bool interactable)
        {
            _toggle.interactable = interactable;
        }
    }
}