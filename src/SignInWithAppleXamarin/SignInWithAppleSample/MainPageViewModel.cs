using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SignInWithAppleSample
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        private string _token;
        public string Token
        {
            get => _token;
            set => SetProperty(ref _token, value);
        }
        private string _userId;
        public string UserId
        {
            get => _userId;
            set => SetProperty(ref _userId, value);
        }
        private string _realUserStatus;
        public string RealUserStatus
        {
            get => _realUserStatus;
            set => SetProperty(ref _realUserStatus, value);
        }
        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}