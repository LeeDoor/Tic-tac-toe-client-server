using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Client_project.Model;
using Prism;
using Prism.Commands;

namespace Client_project.ViewModel
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        public DelegateCommand[] CellCommands { get; set; }

        private void SetActiveButton(int buttonId)
        {
            MessageBox.Show(buttonId.ToString());
        }

        public MainWindowVM()
        {
            CellCommands = new DelegateCommand[9];
            for(int i = 0; i < 9; i++)
            {
                int id = i;
                CellCommands[id] = new DelegateCommand(() =>
                {
                    SetActiveButton(id);
                });
            }

            ConnectToServer();
        }
        private async void ConnectToServer()
        {
            await ServerManager.StartAsync();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
