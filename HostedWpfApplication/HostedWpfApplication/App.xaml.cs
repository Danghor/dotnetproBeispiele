﻿using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using WorldLib;
using HostedWpfApplication.ViewModels;
using HostedWpfApplication.Models;

namespace HostedWpfApplication
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    public static IHost AppHost;

    protected override async void OnStartup(StartupEventArgs e)
    {
      // Host konfigurieren
      AppHost = Host.CreateDefaultBuilder(e.Args)
       .ConfigureServices(ConfigureServices)
       .Build();

      // Host starten
      await AppHost.StartAsync();

      // Hauptfenster einrichten
      var mainWindow =new MainWindow();

      // ViewModel aus DI-Container laden
      mainWindow.DataContext = AppHost.Services.GetService<MainViewModel>();
      mainWindow.Show();
    }

    private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
      // Hier werden die Services für Dependency Injection bereitgestellt
      services.AddSingleton<MainViewModel>();

      services.AddSingleton<IMainController>(p => p.GetRequiredService<MainViewModel>()); // https://andrewlock.net/how-to-register-a-service-with-multiple-interfaces-for-in-asp-net-core-di/


      string path = context.Configuration["MondialPath"];
      services.AddSingleton<World>(new World(path));

      services.AddSingleton<ContinentsViewModel>();
      services.AddSingleton<CountriesViewModel>();

      services.AddSingleton<WorldState>();


    }

    protected override async void OnExit(ExitEventArgs e)
    {
      // Beim Beenden Host stoppen und alles aufräumen
      using (AppHost) await AppHost.StopAsync();
    }
  }
}
