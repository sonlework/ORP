using UnityEngine;

namespace ActionPlatformerKit
{
    public class WinPanel : MonoBehaviour
    {
        public void OnClickReturnMenu()
        {
            if (PlatformGameManager.HasInstance)
            {
                PlatformGameManager.Instance.ReturnMenu();
            }
        }
        public void OnClickShop()
        {

        }
        public void OnClickNextStage()
        {

        }
    }
}
