using System.Collections.Generic;
using System.Linq;
using UnityEngine;
 
namespace MainMenu
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private List<GameObject> pages;

        public void Quit()
        {
            Application.Quit();
        }
        
        public void ShowPage(string pageName)
        {
            var targetPage = pages.First(x => x.name == pageName);

            foreach (var page in pages)
            {
                page.SetActive(false);
            }
            
            targetPage.SetActive(true);
        }
    }
}