using UnityEngine;

namespace Normal.Realtime.Examples
{
    public class ChooseRecipe : MonoBehaviour
    {

        private string _playerRecipe;
        private RecipeSync _recipeSync;

        private RealtimeView _realtimeView;
        private RealtimeTransform _realtimeTransform;

        private void Start()
        {
            // Get a reference to the color sync component
            _recipeSync = GameObject.FindObjectOfType<RecipeSync>();
            _playerRecipe = PlayerPrefs.GetString("playerRecipe");
        }

        private void Awake()
        {
            _realtimeView = GetComponent<RealtimeView>();
            _realtimeTransform = GetComponent<RealtimeTransform>();
        }

        public void Update()
        {
            if (!_realtimeView.isOwnedLocally)
                return;

            _realtimeTransform.RequestOwnership();

            if (_recipeSync == null || _playerRecipe == null)
            {
                _recipeSync = GameObject.FindObjectOfType<RecipeSync>();
                _playerRecipe = PlayerPrefs.GetString("playerRecipe");
            }
            else
            {
                _recipeSync.SetRecipe(_playerRecipe);
            }
        }
    }
}
