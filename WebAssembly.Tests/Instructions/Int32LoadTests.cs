﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace WebAssembly.Instructions
{
	using Compiled;

	/// <summary>
	/// Tests the <see cref="Int32Load"/> instruction.
	/// </summary>
	[TestClass]
	public class Int32LoadTests
	{
		/// <summary>
		/// Tests compilation and execution of the <see cref="Int32Load"/> instruction.
		/// </summary>
		[TestMethod]
		public void Int32Load_Compiled_Offset0()
		{
			var module = new Module();
			module.Memories.Add(new Memory(1, 1));
			module.Types.Add(new Type
			{
				Parameters = new ValueType[]
				{
					ValueType.Int32,
				},
				Returns = new[]
	{
					ValueType.Int32,
				}
			});
			module.Functions.Add(new Function
			{
			});
			module.Exports.Add(new Export
			{
				Name = nameof(MemoryReadTestBase<int>.Test),
			});
			module.Codes.Add(new FunctionBody
			{
				Code = new Instruction[]
				{
					new GetLocal(),
					new Int32Load(),
					new End(),
				},
			});

			Instance<MemoryReadTestBase<int>> compiled;
			using (var memory = new MemoryStream())
			{
				module.WriteToBinary(memory);
				memory.Position = 0;

				compiled = Compiler.FromBinary<MemoryReadTestBase<int>>(memory)();
			}

			using (compiled)
			{
				Assert.IsNotNull(compiled);
				Assert.AreNotEqual(IntPtr.Zero, compiled.Start);
				Assert.AreNotEqual(IntPtr.Zero, compiled.End);

				var exports = compiled.Exports;
				Assert.AreEqual(0, exports.Test(0));

				var testData = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 }
					.Concat(Encoding.Unicode.GetBytes("🐩")) //Must be aligned to 16 bits for compatibility with JavaScript Uint16Array.
					.ToArray()
					;
				Marshal.Copy(testData, 0, compiled.Start, testData.Length);
				Assert.AreEqual(67305985, exports.Test(0));
				Assert.AreEqual(84148994, exports.Test(1));
				Assert.AreEqual(100992003, exports.Test(2));
				Assert.AreEqual(117835012, exports.Test(3));
				Assert.AreEqual(134678021, exports.Test(4));
				Assert.AreEqual(1023936262, exports.Test(5));
				Assert.AreEqual(-667088889, exports.Test(6));
				Assert.AreEqual(702037256, exports.Test(7));
				Assert.AreEqual(-601237443, exports.Test(8));

				Assert.AreEqual(0, exports.Test((int)Memory.PageSize - 4));

				MemoryAccessOutOfRangeException x;

				x = ExceptionAssert.Expect<MemoryAccessOutOfRangeException>(() => exports.Test((int)Memory.PageSize - 3));
				Assert.AreEqual(Memory.PageSize - 3, x.Offset);
				Assert.AreEqual(4u, x.Length);

				x = ExceptionAssert.Expect<MemoryAccessOutOfRangeException>(() => exports.Test((int)Memory.PageSize - 2));
				Assert.AreEqual(Memory.PageSize - 2, x.Offset);
				Assert.AreEqual(4u, x.Length);

				x = ExceptionAssert.Expect<MemoryAccessOutOfRangeException>(() => exports.Test((int)Memory.PageSize - 1));
				Assert.AreEqual(Memory.PageSize - 1, x.Offset);
				Assert.AreEqual(4u, x.Length);

				x = ExceptionAssert.Expect<MemoryAccessOutOfRangeException>(() => exports.Test((int)Memory.PageSize));
				Assert.AreEqual(Memory.PageSize, x.Offset);
				Assert.AreEqual(4u, x.Length);

				ExceptionAssert.Expect<OverflowException>(() => exports.Test(unchecked((int)uint.MaxValue)));
			}

			Assert.AreEqual(IntPtr.Zero, compiled.Start);
			Assert.AreEqual(IntPtr.Zero, compiled.End);
		}

		/// <summary>
		/// Tests compilation and execution of the <see cref="Int32Load"/> instruction.
		/// </summary>
		[TestMethod]
		public void Int32Load_Compiled_Offset1()
		{
			var module = new Module();
			module.Memories.Add(new Memory(1, 1));
			module.Types.Add(new Type
			{
				Parameters = new ValueType[]
				{
					ValueType.Int32,
				},
				Returns = new[]
	{
					ValueType.Int32,
				}
			});
			module.Functions.Add(new Function
			{
			});
			module.Exports.Add(new Export
			{
				Name = nameof(MemoryReadTestBase<int>.Test),
			});
			module.Codes.Add(new FunctionBody
			{
				Code = new Instruction[]
				{
					new GetLocal(),
					new Int32Load
					{
						Offset = 1,
					},
					new End(),
				},
			});

			Instance<MemoryReadTestBase<int>> compiled;
			using (var memory = new MemoryStream())
			{
				module.WriteToBinary(memory);
				memory.Position = 0;

				compiled = Compiler.FromBinary<MemoryReadTestBase<int>>(memory)();
			}

			using (compiled)
			{
				Assert.IsNotNull(compiled);
				Assert.AreNotEqual(IntPtr.Zero, compiled.Start);
				Assert.AreNotEqual(IntPtr.Zero, compiled.End);

				var exports = compiled.Exports;
				Assert.AreEqual(0, exports.Test(0));

				var testData = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 }
					.Concat(Encoding.Unicode.GetBytes("🐩")) //Must be aligned to 16 bits for compatibility with JavaScript Uint16Array.
					.ToArray()
					;
				Marshal.Copy(testData, 0, compiled.Start, testData.Length);
				Assert.AreEqual(84148994, exports.Test(0));
				Assert.AreEqual(100992003, exports.Test(1));
				Assert.AreEqual(117835012, exports.Test(2));
				Assert.AreEqual(134678021, exports.Test(3));
				Assert.AreEqual(1023936262, exports.Test(4));
				Assert.AreEqual(-667088889, exports.Test(5));
				Assert.AreEqual(702037256, exports.Test(6));
				Assert.AreEqual(-601237443, exports.Test(7));
				Assert.AreEqual(14428632, exports.Test(8));

				Assert.AreEqual(0, exports.Test((int)Memory.PageSize - 5));

				MemoryAccessOutOfRangeException x;

				x = ExceptionAssert.Expect<MemoryAccessOutOfRangeException>(() => exports.Test((int)Memory.PageSize - 4));
				Assert.AreEqual(Memory.PageSize - 3, x.Offset);
				Assert.AreEqual(4u, x.Length);

				x = ExceptionAssert.Expect<MemoryAccessOutOfRangeException>(() => exports.Test((int)Memory.PageSize - 3));
				Assert.AreEqual(Memory.PageSize - 2, x.Offset);
				Assert.AreEqual(4u, x.Length);

				x = ExceptionAssert.Expect<MemoryAccessOutOfRangeException>(() => exports.Test((int)Memory.PageSize - 2));
				Assert.AreEqual(Memory.PageSize - 1, x.Offset);
				Assert.AreEqual(4u, x.Length);

				x = ExceptionAssert.Expect<MemoryAccessOutOfRangeException>(() => exports.Test((int)Memory.PageSize - 1));
				Assert.AreEqual(Memory.PageSize, x.Offset);
				Assert.AreEqual(4u, x.Length);

				ExceptionAssert.Expect<OverflowException>(() => exports.Test(unchecked((int)uint.MaxValue)));
			}

			Assert.AreEqual(IntPtr.Zero, compiled.Start);
			Assert.AreEqual(IntPtr.Zero, compiled.End);
		}
	}
}