using Quantum;
using Quantum.Operations;
using System;
using System.Numerics;
using System.Collections.Generic;

namespace QuantumConsole
{
	public class QuantumTest
	{
		public static void Main()
		{
			QuantumComputer comp = QuantumComputer.GetInstance();
			Register x = comp.NewRegister(0, 3);
			comp.Swap(x[1], x[2]);
			x.CNot(target: 1, control: 0);
		}
	}
}
