namespace TBird.Utility.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;
    using System.Linq;

    using TBird.Utility.Mvvm;

    public class AboutBoxViewModel : ViewModelBase
    {
        private readonly ObservableCollection<AssemblyInfo> assemblyList = new ObservableCollection<AssemblyInfo>();

        private string title;

        private string applicationName;

        private string productName;

        private string companyName;

        private string version;

        private string copyright;

        public AboutBoxViewModel()
        {
            this.LoadAssemblies();
        }

        public IEnumerable<AssemblyInfo> AssemblyList
        {
            get
            {
                return this.assemblyList;
            }
        }

        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                if (this.title != value)
                {
                    this.title = value;
                    this.RaisePropertyChanged("Title");
                }
            }
        }

        public string ApplicationName
        {
            get
            {
                return this.applicationName;
            }

            set
            {
                if (this.applicationName != value)
                {
                    this.applicationName = value;
                    this.RaisePropertyChanged("ApplicationName");
                }
            }
        }

        public string ProductName
        {
            get
            {
                return this.productName;
            }

            set
            {
                if (this.productName != value)
                {
                    this.productName = value;
                    this.RaisePropertyChanged("ProductName");
                }
            }
        }

        public string CompanyName
        {
            get
            {
                return this.companyName;
            }

            set
            {
                if (this.companyName != value)
                {
                    this.companyName = value;
                    this.RaisePropertyChanged("CompanyName");
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
                    this.version = value;
                    this.RaisePropertyChanged("Version");
                }
            }
        }

        public string Copyright
        {
            get
            {
                return this.copyright;
            }

            set
            {
                if (this.copyright != value)
                {
                    this.copyright = value;
                    this.RaisePropertyChanged("Copyright");
                }
            }
        }

        private void LoadAssemblies()
        {
            Assembly exeAssembly = Assembly.GetEntryAssembly();

            AssemblyTitleAttribute titleAtt =
                exeAssembly.GetCustomAttribute(typeof(AssemblyTitleAttribute)) as AssemblyTitleAttribute;
            if (titleAtt != null)
            {
                this.ApplicationName = titleAtt.Title;
                this.Title = string.Format(CultureInfo.CurrentCulture, "About {0}", titleAtt.Title);
            }

            AssemblyProductAttribute productAtt =
                exeAssembly.GetCustomAttribute(typeof(AssemblyProductAttribute)) as AssemblyProductAttribute;
            this.ProductName = productAtt != null ? productAtt.Product : string.Empty;

            AssemblyCompanyAttribute companyAtt =
                exeAssembly.GetCustomAttribute(typeof(AssemblyCompanyAttribute)) as AssemblyCompanyAttribute;
            this.CompanyName = companyAtt != null ? companyAtt.Company : string.Empty;

            AssemblyCopyrightAttribute copyrightAtt =
                exeAssembly.GetCustomAttribute(typeof(AssemblyCopyrightAttribute)) as AssemblyCopyrightAttribute;
            this.Copyright = copyrightAtt != null ? copyrightAtt.Copyright : string.Empty;

            this.Version = exeAssembly == null ? "0.0.0.0" : exeAssembly.GetName().Version.ToString();

            foreach (Assembly assem in AppDomain.CurrentDomain.GetAssemblies())
            {
                AssemblyFileVersionAttribute fileAtt =
                    assem.GetCustomAttribute(typeof(AssemblyFileVersionAttribute)) as AssemblyFileVersionAttribute;
                this.assemblyList.Add(
                    new AssemblyInfo() { Name = assem.GetName().Name, Version = fileAtt != null ? fileAtt.Version : assem.GetName().Version.ToString() });
            }
        }
    }
}
