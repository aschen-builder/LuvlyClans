using Jotunn.Configs;
using Jotunn.Managers;
using System;
using UnityEngine;
using UnityEngine.UI;
using Log = Jotunn.Logger;

namespace LuvlyClans.Client.GUI
{
    public class ClansMenu
    {
        private GameObject menuPanel;
        private GameObject memberScrollView;

        private ButtonConfig menuButton;

        public ClansMenu()
        {
            AddInputs();
        }

        private ButtonConfig ButtonBuilder(string name, KeyCode key, bool active)
        {
            return new ButtonConfig
            {
                Name = name,
                Key = key,
                ActiveInGUI = active,
            };
        }

        public void AddInputs()
        {
            menuButton = ButtonBuilder("Show Clans", KeyCode.Insert, true);
            
        }

        public void ToggleMenu()
        {
            if (menuPanel == null)
            {
                if (GUIManager.Instance == null)
                {
                    Log.LogWarning("GUIManager Instance is null");
                    return;
                }

                if (GUIManager.PixelFix == null)
                {
                    Log.LogWarning("GUIManager PixelFix is null");
                    return;
                }

                menuPanel = GUIManager.Instance.CreateWoodpanel(GUIManager.PixelFix.transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0, 0), 750, 500);
                menuPanel.SetActive(true);

                memberScrollView = GUIManager.Instance.CreateScrollView(menuPanel.transform, false, true, 0.5f, 0.25f, GUIManager.Instance.ValheimScrollbarHandleColorBlock, Color.black, 800, 300);
                memberScrollView.SetActive(true);

                memberScrollView.AddComponent(typeof(GameObject));

                GameObject addMember = GUIManager.Instance.CreateButton("Add Member", menuPanel.transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(-200, -145), 150, 50);
                GameObject leaveClan = GUIManager.Instance.CreateButton("Leave Clan", menuPanel.transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(-200, -200), 150, 50);

                addMember.SetActive(true);
                leaveClan.SetActive(true);
            }
        }
    }
}
