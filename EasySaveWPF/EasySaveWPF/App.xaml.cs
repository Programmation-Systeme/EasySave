using System.Configuration;
using System.Data;
using System.Windows;

namespace EasySaveWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string UniqueMutexName = "EasySaveWPF_Mutex";
        private static Mutex mutex;

        protected override void OnStartup(StartupEventArgs e)
        {
            // Créer un mutex global avec un nom unique
            mutex = new Mutex(true, UniqueMutexName, out bool createdNew);

            // Vérifier si le mutex a été créé avec succès
            if (!createdNew)
            {
                // Si le mutex existe déjà, une autre instance de l'application est en cours d'exécution
                MessageBox.Show("L'application est déjà en cours d'exécution.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0); // Fermer l'application
            }

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Libérer le mutex lors de la fermeture de l'application
            mutex.ReleaseMutex();
            mutex.Dispose();

            base.OnExit(e);
        }
    }

}
