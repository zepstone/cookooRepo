using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Origin.Gui {
    public abstract class VMBase : INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propName) {

            var handle = PropertyChanged;
            if (handle != null) {
                handle(this, new PropertyChangedEventArgs(propName));
            }

        }
    }
}
