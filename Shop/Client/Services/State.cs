using System;
using Shop.Shared.Models;

// Holds global state

namespace Shop.Client.Services
{
    public class State
    {
        private OrderDto _order;

        public OrderDto order
        {
            get
            {
                return _order;
            }
            set
            {
                _order = value;
                NotifyDataChanged();
            }
        }

        private Error _err = new Error();

        public Error err
        {
            get
            {
                return _err;
            }
            set
            {
                _err = value;
                NotifyDataChanged();
            }
        }

        public event Action OnChange;

        private void NotifyDataChanged() => OnChange?.Invoke();
    }

    public class Error
    {
        public Error() { }

        public Error(string m, bool c)
        {
            message = m;
            critical = c;
        }

        public string message { get; set; }
        public bool critical { get; set; }
    }
}
