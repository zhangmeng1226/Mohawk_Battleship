using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MBC.Core.Threading;
using System.Threading;
using MBC.Core.Controllers;

namespace TestCore
{
    [TestClass]
    public class ThreadTimeoutAborterTest
    {
        [TestMethod]
        public void Thread_Timeout_Aborts()
        {
            ThreadTimeoutAborter aborter = new ThreadTimeoutAborter(Thread.CurrentThread, 50);
            try
            {
                aborter.MonBegin();
                Thread.Sleep(100);
                aborter.MonEnd();

                Assert.Fail("Thread does not get aborted when over time limit.");
            }
            catch (ThreadAbortException ex)
            {
                Thread.ResetAbort();
                Assert.IsTrue(true, "Thread gets aborted exceeding time limit.");
            }
        }

        [TestMethod]
        public void Thread_NoTimeout_Continues()
        {
            ThreadTimeoutAborter aborter = new ThreadTimeoutAborter(Thread.CurrentThread, 50);
            try
            {
                aborter.MonBegin();
                Thread.Sleep(10);
                aborter.MonEnd();
            }
            catch (ThreadAbortException ex)
            {
                Thread.ResetAbort();
                Assert.Fail("Thread aborts before timeout");
            }
            try
            {
                Thread.Sleep(100);
                Assert.IsTrue(true, "Thread continues through timeout as expected.");
            }
            catch (ThreadAbortException ex)
            {
                Assert.Fail("Thread aborts after monitoring stopped.");
            }
        }
    }
}
