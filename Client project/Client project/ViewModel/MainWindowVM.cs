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
        ServerManager serverManager = null!;

        public DelegateCommand[] CellCommands { get; set; }
        private string[] fieldValues;
        public string[] FieldValues {
            get => fieldValues; 
            set
            {
                fieldValues = value;
                OnPropertyChanged(nameof(FieldValues));
            }
        }
        private bool isActive;
        public bool IsActive
        {
            get => isActive;
            set
            {
                isActive = value;
                OnPropertyChanged(nameof(IsActive));
            }
        }

        private void ButtonPress(int buttonId)
        {
            MessageBox.Show(buttonId.ToString());
        }

        private async void ConnectToServer()
        {
            serverManager = new();
            await serverManager.StartAsync(this);
        }

        public void UpdateField(string field)
        {
            string[] values = new string[9];
            for(int i = 0; i < 9; i++)
            {
                values[i] = field[i] == 'N'?" " : field[i].ToString();
            }
            FieldValues = values;
        }

        public MainWindowVM()
        {
            FieldValues = new string[9];

            CellCommands = new DelegateCommand[9];
            for(int i = 0; i < 9; i++)
            {
                int id = i;
                CellCommands[id] = new DelegateCommand(() =>
                {
                    NetworkSendGet.SendByteArray(serverManager.stream, new byte[] { (byte)id });
                });
            }

            ConnectToServer();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
