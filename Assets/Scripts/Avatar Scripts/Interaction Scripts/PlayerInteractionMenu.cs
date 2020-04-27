using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Normal.Realtime.Examples {
    public class PlayerInteractionMenu : MonoBehaviour
    {
        private ModifyInteraction interactionModifier;
        private GameObject localAvatar;
        private int selfId;
        private int otherId;

        public void ExitMenu()
        {
            Destroy(transform.parent.gameObject);
        }

        public void Start()
        {
            localAvatar = ActionRouter.GetLocalAvatar();
            interactionModifier = localAvatar.GetComponent<ModifyInteraction>();
            selfId = localAvatar.GetComponent<ThirdPersonUserControl>().getID();
           
        }

        private int getOtherID()
        {
            return ActionRouter.GetCurrentCharacter().GetComponent<ThirdPersonUserControl>().getID();
        }

        public void PerformAction(string interactionString) {
            System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
            int cur_time = (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;

            Debug.Log("Interaction Perform: " + interactionString);
            //Debug.Log(interactionModifier);
            //Debug.Log(selfId);
            //Debug.Log(Utils.interactionMap);
            interactionModifier.SendNewValue(selfId.ToString() + " " + getOtherID().ToString() + " " + Utils.interactionMap.Reverse[interactionString] + " " + cur_time.ToString());
            ActionRouter.GetLocalAvatar().GetComponent<ThirdPersonUserControl>().ReactToInteractionChange(ActionRouter.GetCurrentCharacter(), interactionString);
        }

    }
}