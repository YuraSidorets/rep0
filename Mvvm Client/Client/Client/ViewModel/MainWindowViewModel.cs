using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;


namespace Client
{ 
    public class MainWindowViewModel : ViewModelBase
    {

        RelayCommand _addClientCommand;
        public ICommand AddClient
        {
            get
            {
                if (_addClientCommand == null)
                    _addClientCommand = new RelayCommand(ExecuteAddClientCommand, CanExecuteAddClientCommand);
                return _addClientCommand;
            }
        }

        public void ExecuteAddClientCommand(object parameter)
        {
            OpenFileDialog opd = new OpenFileDialog();
            opd.ShowDialog();
            string path = opd.FileName;

            ClientTcpWorker rm = new ClientTcpWorker(path, 5050, "::1");
            rm.SendFileDict();
        }

        public bool CanExecuteAddClientCommand(object parameter)
        {
            return true;
        }


    }
}
