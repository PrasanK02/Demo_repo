using NUnit.Framework;
using System;
using Cegedim.Automation;

namespace Cegedim {
    [TestFixture()]
    public abstract class BaseTestFixture {
        public MITouch m_miTouch;
    }
}

