using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using TMPro;

namespace Normal.Realtime.Examples {
    public class RecipeSync : RealtimeComponent
    {

        public string _playerRecipe;
        private RecipeSyncModel _model;
        private ThirdPersonUserControl userControlScript;

    private RecipeSyncModel model
        {
            set
            {
                if (_model != null)
                {
                    // Unregister from events
                    _model.avatarRecipeDidChange -= RecipeDidChange;
                }

                // Store the model
                _model = value;

                if (_model != null)
                {
                    // Update the mesh render to match the new model
                    UpdateRecipe();

                    // Register for events so we'll know if the color changes later
                    _model.avatarRecipeDidChange += RecipeDidChange;
                }
            }
        }

        private void RecipeDidChange(RecipeSyncModel model, string value)
        {
            // Update the mesh renderer
            UpdateRecipe();
        }

        private void UpdateRecipe()
        {
            // Get the color from the model and set it on the mesh renderer.
            userControlScript = GetComponent<ThirdPersonUserControl>();
            userControlScript.avatarRecipe = _model.avatarRecipe;
        }

        public void SetRecipe(string recipe)
        {
            Debug.Log("Set Recipe called");
            // Set the color on the model
            // This will fire the colorChanged event on the model, which will update the renderer for both the local player and all remote players.
            _model.avatarRecipe = recipe;
        }
    }
}