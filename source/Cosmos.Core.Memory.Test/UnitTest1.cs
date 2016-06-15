﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Native = System.UInt32;

namespace Cosmos.Core.Memory.Test {
  [TestClass]
  public class UnitTest1 {
    [TestMethod]
    unsafe public void TestMethod1() {
      var xRAM = new byte[128 * 1024 * 1024];
      xRAM[0] = 1;
      fixed (byte* xPtr = xRAM) {
        RAT.Debug = true;
        RAT.Init(xPtr, (Native)xRAM.LongLength);

        Native xRatPages = RAT.GetPageCount(RAT.PageType.RAT);
        Assert.IsTrue(xRatPages > 0);
        
        Native xFreePages = RAT.GetPageCount(RAT.PageType.Empty);
        Assert.IsTrue(xFreePages > 0);

        var x1 = (Int32*)Heap.Alloc(sizeof(Int32));
        xFreePages = RAT.GetPageCount(RAT.PageType.Empty);
        Assert.IsTrue(xFreePages > 0);
      }
    }
  }
}
