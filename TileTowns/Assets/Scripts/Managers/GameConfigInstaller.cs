using Data;
using UnityEngine;
using Zenject;

[CreateAssetMenu(menuName = "Data/Game Config Installer")]
public class GameConfigInstaller : ScriptableObjectInstaller<GameConfigInstaller>
{
    [SerializeField] private GameConfig gameConfig;
    [SerializeField] private TileFeatureMap tileFeatureMap;
    [SerializeField] private GameSounds gameSounds;
    [SerializeField] private LevelSet levelSet;
    public override void InstallBindings()
    {
        Container.Bind<IGameConfig>().To<GameConfig>().FromInstance(gameConfig).AsSingle();
        Container.Bind<ITileFeatureMap>().To<TileFeatureMap>().FromInstance(tileFeatureMap).AsSingle();
        Container.Bind<IGameSounds>().To<GameSounds>().FromInstance(gameSounds).AsSingle();
        Container.Bind<ILevelSet>().To<LevelSet>().FromInstance(levelSet).AsSingle();
    }
}