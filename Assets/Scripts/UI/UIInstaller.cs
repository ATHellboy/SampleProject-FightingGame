using Zenject;

namespace AlirezaTarahomi.FightingGame.UI
{
    public class UIInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<UIGameOverPanel>().FromComponentInHierarchy().AsSingle();
        }
    }
}