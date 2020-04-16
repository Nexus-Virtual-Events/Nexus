using UnityEngine;

namespace Normal.Realtime.Examples
{
    public class ChooseRecipe : MonoBehaviour
    {

        private string _playerRecipe;
        private string _prevRecipe = "";

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
            Debug.Log(">>>>>>>>>");
            Debug.Log(!_realtimeView.isOwnedLocally);
            Debug.Log("playerRecipe");
            Debug.Log(_playerRecipe.Substring(0, 210));
            Debug.Log("prevRecipe");
            if (_prevRecipe.Length > 200)
            {
                Debug.Log(_prevRecipe.Substring(0, 210));
            }
            else
            {
                Debug.Log(" ");
            }
           
            Debug.Log(">>>>>>");

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
                Debug.Log(">>>>>>>>>");
                Debug.Log("From inside else");
                Debug.Log("playerRecipe");
                Debug.Log(_playerRecipe.Substring(0, 210));
                Debug.Log("prevRecipe");
                if (_prevRecipe.Length > 200)
                {
                    Debug.Log(_prevRecipe.Substring(0, 210));
                }
                else { 
                    Debug.Log(" ");
                }
                Debug.Log(">>>>>>");


                if (_playerRecipe != _prevRecipe)
                {
                    Debug.Log("recipe different");
                    _recipeSync.SetRecipe(_playerRecipe);
                    _prevRecipe = _playerRecipe;
                }
            }
        }
    }
}
