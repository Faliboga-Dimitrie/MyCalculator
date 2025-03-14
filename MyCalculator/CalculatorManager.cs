using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyCalculator
{
    class CalculatorManager : INotifyPropertyChanged
    {
        private CalculatorEngine _engine;

        private Visibility _mainGridVisibility = Visibility.Visible;
        private Visibility _memoryGridVisibility = Visibility.Collapsed;

        public Visibility MainGridVisibility
        {
            get => _mainGridVisibility;
            set
            {
                _mainGridVisibility = value;
                OnPropertyChanged(nameof(MainGridVisibility));
            }
        }

        public Visibility MemoryGridVisibility
        {
            get => _memoryGridVisibility;
            set
            {
                _memoryGridVisibility = value;
                OnPropertyChanged(nameof(MemoryGridVisibility));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public CalculatorEngine Engine
        {
            get { return _engine; }
        }

        public CalculatorManager()
        {
            _engine = new CalculatorEngine();
        }
    }
}
