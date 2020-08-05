﻿using Quantum;
using Quantum.Operations;
using System;
using System.Numerics;
using System.Collections.Generic;

namespace QuantumConsole
{
	public static class CompositeExtension
	{
		public static void Uf(this QuantumComputer comp, Register x, Register y)
		{
			// Ziel = |00>
			x.SigmaX(0);
			x.SigmaX(1);
			comp.Toffoli(y[0], x[0], x[1]);
			x.SigmaX(0);
			x.SigmaX(1);
		}
		public static void D2(this QuantumComputer comp, Register x)
		{
			int anzahlBit = 2;
			comp.Walsh(x);
			for (int i = 0; i < anzahlBit; i++)
			{
				x.SigmaX(i);
			}
			x.Hadamard(0);
			x.CNot(0, 1);
			x.Hadamard(0);
			for (int i = 0; i < anzahlBit; i++)
			{
				x.SigmaX(i);
			}
			comp.Walsh(x);
		}
	}
	public class QuantumTest
	{
		public static void Main()
		{
			QuantumComputer comp = QuantumComputer.GetInstance();

			// Eingabe, der Pfad als Liste von Knoten (Knoten s und z können weggelassen werden, da sie immer im Pfad enthalten sein müssen).
			//
			// Knoten werden als 2 Bit Zahlen kodiert:
			// * a = |0000>
			// * b = |0001>
			// * c = |0010>
			// * d = |0011>
			// * e = |0100>
			// * f = |0101>
			// * g = |0110>
			// * h = |0111>
			// * i = |1000>
			// * j = |1001>
			// * k = |1010>
			// * l = |1011>
			// 
			// x1x0 = Erster Knoten des Pfades
			// x3x2 = Zweiter Knoten des Pfades
			int anzahlKnoten = 3;
			int anzahlBitProKnoten = 4;
			int anzahlXBit = anzahlKnoten * anzahlBitProKnoten;
			Register x = comp.NewRegister(0, anzahlXBit);
			// Ausgabe von Uf
			Register y = comp.NewRegister(1, 1);
			
			comp.Walsh(x);
			comp.Hadamard(y);
			
			comp.Uf(x, y);
			comp.D2(x);
		}
	}
}
