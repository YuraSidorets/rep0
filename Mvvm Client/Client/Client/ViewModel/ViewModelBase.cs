using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Client
{
    public abstract class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        protected ViewModelBase()
        { 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void Dispose()
        {
            this.OnDispose();
        }

        protected virtual void OnDispose()
        { 
        }
    }
}
