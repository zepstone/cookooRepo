using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Origin.Gui {
    public class RealyCommand : System.Windows.Input.ICommand {

        private Action ExecuteAction;

        public RealyCommand(Action Execute) {
            ExecuteAction = Execute;
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter) {
            ExecuteAction();
        }
    }
}
