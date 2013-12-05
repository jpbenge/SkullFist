using System.Collections.Generic;
using System.Linq;

namespace EZData
{
    public delegate void VariableContextChangeDelegate();
    
    public abstract class VariableContext : Context
    {
        private readonly List<IBinding> _dependencies = new List<IBinding>();

        protected override void AddBindingDependency(IBinding binding)
        {
            if (binding == null)
                return;

            if (_dependencies.Contains(binding))
                return;
            _dependencies.Add(binding);
        }

        public List<IBinding> GetDependenciesAndCleanup()
        {
            var result = _dependencies.ToList();
            _dependencies.Clear();
            return result;
        }
    }

    public class VariableContext<T> : VariableContext
    {
        public event VariableContextChangeDelegate OnChange;

        private T _value;
        public T Value
        {
            get { return _value; }
            set
            {
                var dependencies = GetDependenciesAndCleanup();
                _value = value;
                foreach (var dependency in dependencies)
                {
                    dependency.OnContextChange();
                }

                if (OnChange != null)
                    OnChange();
            }
        }

        public VariableContext(T value)
        {
            _value = value;
        }

        public VariableContext(T value, VariableContextChangeDelegate onChange)
        {
            _value = value;
            OnChange += onChange;
        }
    }
}
