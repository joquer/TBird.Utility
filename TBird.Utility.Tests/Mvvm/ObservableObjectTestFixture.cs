namespace TBird.Utility.Test.Mvvm
{
    using System.ComponentModel;

    using NUnit.Framework;

    using TBird.Utility.Mvvm;

    [TestFixture]
    public class ObservableObjectTest
    {
        public class TestObject : ObservableObject
        {
            private int testIntValue;

            public int TestIntValue
            {
                get
                {
                    return this.testIntValue;
                }

                set
                {
                    this.testIntValue = value;
                    this.RaisePropertyChanged("TestIntValue");
                }
            }
        }

        public class TestObjectGeneric : ObservableObject
        {
            private int testIntValue;

            public int TestIntValue
            {
                get
                {
                    return this.testIntValue;
                }

                set
                {
                    this.testIntValue = value;
                    this.RaisePropertyChanged(() => this.TestIntValue);
                }
            }
        }

        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }


        private bool propertyChangeCalled;

        private string propertyChangeName;

        [Test]
        public void RaisePropertyChanged_SetProperty_RaisesEvent()
        {
            TestObject t = new TestObject();
            t.PropertyChanged += this.TestPropertyChanged;
            t.TestIntValue = 42;
            Assert.AreEqual(t.TestIntValue, 42);
            Assert.IsTrue(this.propertyChangeCalled);
            Assert.AreEqual(this.propertyChangeName, "TestIntValue");
        }

        [Test]
        public void RaisePropertyChangedGeneric_SetProperty_RaisesEvent()
        {
            TestObjectGeneric t = new TestObjectGeneric();
            t.PropertyChanged += this.TestPropertyChanged;
            t.TestIntValue = 42;
            Assert.AreEqual(t.TestIntValue, 42);
            Assert.IsTrue(this.propertyChangeCalled);
            Assert.AreEqual(this.propertyChangeName, "TestIntValue");
        }

        private void TestPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.propertyChangeCalled = true;
            this.propertyChangeName = e.PropertyName;
        }
    }
}
