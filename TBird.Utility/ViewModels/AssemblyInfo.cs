namespace TBird.Utility.ViewModels
{
    using System.Diagnostics.Contracts;

    using TBird.Utility.Mvvm;

    public class AssemblyInfo : ObservableObject
    {
        private string name = string.Empty;

        private string version = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyInfo"/> class.
        /// </summary>
        public AssemblyInfo()
        {
        }

        public AssemblyInfo(string name, string version)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Ensures(!string.IsNullOrEmpty(name));
            Contract.Ensures(this.name == name);
            this.Name = name;
            this.Version = version;
        }

        public string Name
        {
            get
            {
                return this.name ?? string.Empty;
            }

            set
            {
                if (this.Name != value)
                {
                    this.name = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }

        public string Version
        {
            get
            {
                return this.version;
            }

            set
            {
                if (this.version != value)
                {
                    this.version = value ?? string.Empty;
                    this.RaisePropertyChanged("Version");
                }
            }
        }
    }
}
