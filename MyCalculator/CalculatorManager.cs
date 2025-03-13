using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCalculator
{
    class CalculatorManager
    {
        private CalculatorEngine _engine;

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
