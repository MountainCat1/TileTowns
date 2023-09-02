using UnityEngine;
using Zenject;

namespace Buildings
{
    public class Farm : BuildingBehaviour
    {
        [Inject]
        private void Construct(IGameManager gameManager)
        {
            Debug.Log(gameManager.LevelConfig.Name);
        }
        

        private void Awake()
        {
            Debug.Log("Awwake!");
        }
    }
}