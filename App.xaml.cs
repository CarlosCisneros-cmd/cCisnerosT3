using Microsoft.Extensions.DependencyInjection;

namespace cCisnerosT3
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new NavigationPage(new Views.vContacto()));
        }
    }
}