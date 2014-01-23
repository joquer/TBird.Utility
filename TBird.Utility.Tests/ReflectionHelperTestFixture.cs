namespace TBird.Utility.Enumerations.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class ReflectionHelperTestFixture
    {
        private class BaseClass
        {
            public void BaseClassMethod()
            {
            }
        }

        private class SubClass : BaseClass
        {
            public void SubClassMethod()
            {
            }
        }

        [Test]
        public void GetMethod_FindClassMethod_Found()
        {
            Assert.IsNotNull(ReflectionHelper.FindMethod(typeof(SubClass), "SubClassMethod"));
        }

        [Test]
        public void GetMethod_FindBaseMethod_Found()
        {
            Assert.IsNotNull(ReflectionHelper.FindMethod(typeof(SubClass), "BaseClassMethod"));
        }

        [Test]
        public void GetMethod_FindInvalidMethod_NotFound()
        {
            Assert.IsNull(ReflectionHelper.FindMethod(typeof(SubClass), "BadMethodName"));
        }
    }
}
