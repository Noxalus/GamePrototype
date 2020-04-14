using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using ModTool;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;

namespace GamePrototype
{
    /// <summary>
    /// Example mod manager. This menu displays all mods and lets you enable/disable them.
    /// </summary>
    public class ModMenu : MonoBehaviour
    {
        /// <summary>
        /// The content panel where the menu items will be parented
        /// </summary>
        [SerializeField]
        private Transform _menuContentPanel = null;

        /// <summary>
        /// The prefab for the mod menu item
        /// </summary>
        [SerializeField]
        private ModItem _modItemPrefab = null;

        /// <summary>
        /// Button that will start loading enabled mods
        /// </summary>
        [SerializeField]
        private Button _loadButton = null;

        /// <summary>
        /// Are the enabled mods loaded?
        /// </summary>
        private bool _isLoaded;

        private ModManager _modManager;
        private Dictionary<Mod, ModItem> _modItems;
        private Scene _defaultScene;

        void Start()
        {
            _defaultScene = SceneManager.GetActiveScene();

            _modItems = new Dictionary<Mod, ModItem>();

            _modManager = ModManager.instance;

#if UNITY_EDITOR
            _modManager.AddSearchDirectory(Path.Combine(Application.dataPath, @"..\Builds\StandaloneWindows\Mods"));
#endif

            _modManager.refreshInterval = 2;

            foreach (Mod mod in _modManager.mods)
                OnModFound(mod);

            _modManager.ModFound += OnModFound;
            _modManager.ModRemoved += OnModRemoved;
            _modManager.ModLoaded += OnModLoaded;
            _modManager.ModUnloaded += OnModUnloaded;

            Application.runInBackground = true;
        }

        private void OnModFound(Mod mod)
        {
            ModItem modItem = Instantiate(_modItemPrefab);
            modItem.Initialize(mod, _menuContentPanel);
            modItem.SetToggleInteractable(!_isLoaded);
            _modItems.Add(mod, modItem);
        }

        private void OnModRemoved(Mod mod)
        {
            ModItem modItem;

            if (_modItems.TryGetValue(mod, out modItem))
            {
                _modItems.Remove(mod);
                Destroy(modItem.gameObject);
            }
        }

        private void SetTogglesInteractable(bool interactable)
        {
            foreach (ModItem item in _modItems.Values)
            {
                item.SetToggleInteractable(interactable);
            }
        }

        /// <summary>
        /// Toggle load or unload all mods.
        /// </summary>
        public void LoadButton()
        {
            if (_isLoaded)
            {
                Unload();
            }
            else
            {
                Load();
            }
        }

        private void Load()
        {

            // Load mods
            foreach (Mod mod in _modItems.Keys)
            {
                if (mod.isEnabled)
                    mod.LoadAsync();
            }

            SetTogglesInteractable(false);

            _loadButton.GetComponentInChildren<TextMeshProUGUI>().text = "Unload";

            _isLoaded = true;

        }

        private void Unload()
        {
            // Unload all mods - this will unload their scenes and destroy all their instantiated objects as well
            foreach (Mod mod in _modItems.Keys)
            {
                mod.Unload();
            }

            SetTogglesInteractable(true);

            _loadButton.GetComponentInChildren<TextMeshProUGUI>().text = "Load";

            _isLoaded = false;
        }

        private void OnModLoaded(Mod mod)
        {
            Debug.Log("Loaded Mod: " + mod.name);

            // Load first scene when a mod is loaded
            ModScene scene = mod.scenes.FirstOrDefault();

            if (scene != null)
            {
                scene.Loaded += ModSceneLoaded;
                scene.LoadAsync();
            }
        }

        private void ModSceneLoaded(Resource obj)
        {
            (obj as ModScene).Loaded -= ModSceneLoaded;

            SceneManager.SetActiveScene(_defaultScene);
        }

        private void OnModUnloaded(Mod mod)
        {
            Debug.Log("Unloaded Mod: " + mod.name);
        }
    }
}