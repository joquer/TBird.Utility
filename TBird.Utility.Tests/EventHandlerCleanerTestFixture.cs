namespace TBird.Utility.Test
{
    using System;

    using NUnit.Framework;

    using TBird.Utility.Mvvm;

    public class BaseEventTest
    {
        public event EventHandler<EventArgs> BaseEventHandler;

        public int HandlerCount { get; set; }

        public virtual void AddHandlers()
        {
            this.BaseEventHandler += this.BaseMethod;
            this.HandlerCount++;
        }

        public void RemoveBaseHandler()
        {
            if (this.BaseEventHandler != null)
            {
                this.BaseEventHandler -= this.BaseMethod;
                this.HandlerCount--;
            }
        }

        public void BaseMethod(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OnEvent()
        {
            if (this.BaseEventHandler != null)
            {
                this.BaseEventHandler(this, new EventArgs());
            }
        }
    }

    public class SubEventTest : BaseEventTest
    {
        public event EventHandler<EventArgs> SubEventHandler;

        public override void AddHandlers()
        {
            this.SubEventHandler += this.SubMethod;
            this.HandlerCount++;
            base.AddHandlers();
        }

        public void RemoveSubHandler()
        {
            if (this.SubEventHandler != null)
            {
                this.SubEventHandler -= this.SubMethod;
                this.HandlerCount--;
            }
        }

        public void SubMethod(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OnSubEvent()
        {
            if (this.SubEventHandler != null)
            {
                this.SubEventHandler(this, new EventArgs());
            }
        }
    }

    public class SubSubEventTest : SubEventTest
    {
        public event EventHandler<EventArgs> SubSubEventHandler;

        public override void AddHandlers()
        {
            this.SubSubEventHandler += this.SubSubMethod;
            this.HandlerCount++;
            base.AddHandlers();
        }

        public void RemoveSubSubHandler()
        {
            if (this.SubSubEventHandler != null)
            {
                this.SubSubEventHandler -= this.SubSubMethod;
                this.HandlerCount--;
            }
        }

        public void SubSubMethod(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public void OnSubSubEvent()
        {
            if (this.SubSubEventHandler != null)
            {
                this.SubSubEventHandler(this, new EventArgs());
            }
        }
    }

    /// <summary>
    /// This is a test class for EventHandlerCleanerTest and is intended
    /// to contain all EventHandlerCleanerTest Unit Tests
    /// </summary>
    [TestFixture]
    public class EventHandlerCleanerTest
    {
        /// <summary>
        /// A test for CountEventHandlers
        /// </summary>
        [Test]
        public void CountEventHandlers_BasicCountTest()
        {
            SubSubEventTest testObj = new SubSubEventTest();
            Assert.AreEqual(testObj.HandlerCount, EventHandlerCleaner.CountEventHandlers(testObj));
            testObj.AddHandlers();
            Assert.AreEqual(testObj.HandlerCount, EventHandlerCleaner.CountEventHandlers(testObj));
        }

        [Test]
        public void RemoveEventHandlers_RemoveAllHandlers()
        {
            EventHandlerCleaner.AllowHandlerRemoval = true;
            SubSubEventTest testObj = new SubSubEventTest();
            Assert.AreEqual(testObj.HandlerCount, EventHandlerCleaner.CountEventHandlers(testObj));
            testObj.AddHandlers();
            Assert.AreEqual(testObj.HandlerCount, EventHandlerCleaner.CountEventHandlers(testObj));
            EventHandlerCleaner.AllowHandlerRemoval = true;
            EventHandlerCleaner.RemoveEventHandlers(testObj);
            Assert.AreEqual(0, EventHandlerCleaner.CountEventHandlers(testObj));
        }

        [Test]
        public void RemoveEventHandlers_RemoveSingleHandlers()
        {
            EventHandlerCleaner.AllowHandlerRemoval = true;
            SubSubEventTest testObj = new SubSubEventTest();
            Assert.AreEqual(testObj.HandlerCount, EventHandlerCleaner.CountEventHandlers(testObj));
            testObj.AddHandlers();
            Assert.AreEqual(testObj.HandlerCount, EventHandlerCleaner.CountEventHandlers(testObj));
            testObj.RemoveSubSubHandler();
            Assert.AreEqual(testObj.HandlerCount, EventHandlerCleaner.CountEventHandlers(testObj));
            testObj.RemoveSubHandler();
            Assert.AreEqual(testObj.HandlerCount, EventHandlerCleaner.CountEventHandlers(testObj));
            testObj.RemoveBaseHandler();
            Assert.AreEqual(testObj.HandlerCount, EventHandlerCleaner.CountEventHandlers(testObj));
            Assert.AreEqual(0, EventHandlerCleaner.CountEventHandlers(testObj));
        }
    }
}
    
