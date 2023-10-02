using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MainMenu
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Transform viewsContainer;
        [SerializeField] private MenuViewUI defaultView;
        private List<MenuViewUI> _views;

        private void Start()
        {
            _views = viewsContainer.GetComponentsInChildren<MenuViewUI>().ToList();
            
            ShowPage(defaultView);
        }

        public void ShowPage(MenuViewUI targetPage)
        {
            foreach (var page in _views)
            {
                page.gameObject.SetActive(false);
            }
            
            targetPage.gameObject.SetActive(true);
        }
        public void ShowPage(string pageName)
        {
            var targetPage = _views.First(x => x.name == pageName);

            ShowPage(targetPage);
        }   
        
        public void Quit()
        {
            Application.Quit();
        }
    }
}