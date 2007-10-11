﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CPU = Indy.IL2CPU.Assembler;
using CPUx86 = Indy.IL2CPU.Assembler.X86;

namespace Indy.IL2CPU.IL.X86 {
	public abstract class X86OpCodeMap: OpCodeMap {
		protected override Type GetMethodHeaderOp() {
			return typeof(X86MethodHeaderOp);
		}

		protected override Type GetMethodFooterOp() {
			return typeof(X86MethodFooterOp);
		}

		protected override Type GetInitVmtImplementationOp() {
			return typeof(X86InitVmtImplementationOp);
		}

		protected override Assembly ImplementationAssembly {
			get {
				return typeof(X86OpCodeMap).Assembly;
			}
		}

		protected override Type GetMainEntryPointOp() {
			return typeof(X86MainEntryPointOp);
		}

		protected override Type GetPInvokeMethodBodyOp() {
			return typeof(X86PInvokeMethodBodyOp);
		}

		protected override Type GetCustomMethodImplementationProxyOp() {
			return typeof(X86CustomMethodImplementationProxyOp);
		}

		public override Mono.Cecil.MethodReference GetCustomMethodImplementation(string aOrigMethodName, bool aInMetalMode) {
			switch (aOrigMethodName) {
				case "System_Int32___System_String_get_Length____": {
						if (aInMetalMode) {
							return CustomImplementations.System.StringImplRefs.get_Length_MetalRef;
						} else {
							return CustomImplementations.System.StringImplRefs.get_Length_NormalRef;
						}
					}
				case "System_Char___System_String_get_Chars___System_Int32___": {
						if (aInMetalMode) {
							return CustomImplementations.System.StringImplRefs.get_Chars_MetalRef;
						}
						goto default;
					}
				default:
					return base.GetCustomMethodImplementation(aOrigMethodName, aInMetalMode);
			}
		}

		public override bool HasCustomAssembleImplementation(string aMethodName, bool aInMetalMode) {
			switch (aMethodName) {
				case "System_Object___System_Threading_Interlocked_CompareExchange___System_Object___System_Object__System_Object___": {
						return true;
					}  
				case "System_Int32___System_Threading_Interlocked_CompareExchange___System_Int32___System_Int32__System_Int32___": {
						return true;
					}
				case "System_String___System_String_FastAllocateString___System_Int32___": {
						return true;
					}
				default:
					return base.HasCustomAssembleImplementation(aMethodName, aInMetalMode);
			}
		}

		public override void DoCustomAssembleImplementation(string aMethodName, bool aInMetalMode, Indy.IL2CPU.Assembler.Assembler aAssembler, MethodInformation aMethodInfo) {
			switch (aMethodName) {
				case "System_Object___System_Threading_Interlocked_CompareExchange___System_Object___System_Object__System_Object___": {
						Assemble_System_Threading_Interlocked_CompareExchange__Object(aAssembler, aMethodInfo);
						break;
					}
				case "System_Int32___System_Threading_Interlocked_CompareExchange___System_Int32___System_Int32__System_Int32___": {
						Assemble_System_Threading_Interlocked_CompareExchange__Object(aAssembler, aMethodInfo);
						break;
					}
				default:
					base.DoCustomAssembleImplementation(aMethodName, aInMetalMode, aAssembler, aMethodInfo);
					break;
			}
		}

		private static void Assemble_System_Threading_Interlocked_CompareExchange__Object(Assembler.Assembler aAssembler, MethodInformation aMethodInfo) {
			//arguments:
			//   0: location
			//   1: value
			//   2: comparand
			Ldarg.Ldarg(aAssembler, aMethodInfo.Arguments[2].VirtualAddresses, aMethodInfo.Arguments[2].Size);
			aAssembler.Add(new CPUx86.Pop("eax"));
			Ldarg.Ldarg(aAssembler, aMethodInfo.Arguments[1].VirtualAddresses, aMethodInfo.Arguments[1].Size);
			aAssembler.Add(new CPUx86.Pop("edx"));
			Ldarg.Ldarg(aAssembler, aMethodInfo.Arguments[0].VirtualAddresses, aMethodInfo.Arguments[0].Size);
			aAssembler.Add(new CPUx86.Pop("ecx"));
			aAssembler.Add(new CPUx86.Pushd("[ecx]"));
			aAssembler.Add(new CPUx86.Pop("ecx"));
			aAssembler.Add(new CPUx86.CmpXchg("ecx", "edx"));
			Ldarg.Ldarg(aAssembler, aMethodInfo.Arguments[0].VirtualAddresses, aMethodInfo.Arguments[0].Size);
			aAssembler.Add(new CPUx86.Pop("eax"));
			aAssembler.Add(new CPUx86.Move("[eax]", "ecx"));
			aAssembler.Add(new CPUx86.Pushd("eax"));
		}
	}
}
