using Quantum;
using Quantum.Operations;
using System;
using System.Numerics;
using System.Collections.Generic;

namespace QuantumConsole
{
	public static class CompositeExtension
	{
		public static void Uf(this QuantumComputer comp, Register x, Register y, Register z)
		{
			// Prüfe, ob erster Knoten (x1x0) mit Startknoten verbunden ist.
			// Wenn erster Knoten des Pfades Knoten a (00) ist, dann setze Ancilla Bit y[0]
			x.SigmaX(0);
			x.SigmaX(1);
			comp.Toffoli(y[0], x[0], x[1]);
			x.SigmaX(0);
			x.SigmaX(1); // Ein nachfolgendes SigmaX hebt dieses auf.
			// Wenn erster Knoten des Pfades Knoten b (01) ist, dann setze y[0]
			x.SigmaX(1); // Ein vorheriges SigmaX hebt dieses auf.
			comp.Toffoli(y[0], x[0], x[1]);
			x.SigmaX(1); // Ein nachfolgendes SigmaX hebt dieses auf.
			
			// Prüfe, ob letzter Knoten (x1x0) mit Zielknoten verbunden ist.
			// Wenn letzter Knoten des Pfades Knoten a (00) ist, dann setze Ancilla Bit y[1]
			x.SigmaX(0);
			x.SigmaX(1); // Ein vorheriges SigmaX hebt dieses auf.
			comp.Toffoli(y[1], x[0], x[1]);
			x.SigmaX(0); // Ein nachfolgendes SigmaX hebt dieses auf.
			x.SigmaX(1);
			// Wenn letzter Knoten des Pfades Knoten c (10) ist, dann setze Ancilla Bit y[1]
			x.SigmaX(0); // Ein vorheriges SigmaX hebt dieses auf.
			comp.Toffoli(y[1], x[0], x[1]);
			x.SigmaX(0);
			// Wenn letzter Knoten des Pfades Knoten d (11) ist, dann setze Ancilla Bit y[1]
			comp.Toffoli(y[1], x[0], x[1]);
			
			// Wenn y[0] gesetzt ist, hat der Pfad eine Verbindung zum Startknoten.
			// Wenn y[1] gesetzt ist, hat der Pfad eine Verbindung zum Endknoten.
			// Wenn beider der Fall ist, ist der Pfad valide.
			comp.Toffoli(z[0], y[0], y[1]);
			
			// Ancilla Bits wieder entschränken
			comp.Toffoli(y[1], x[0], x[1]);
			x.SigmaX(0);
			comp.Toffoli(y[1], x[0], x[1]);
			x.SigmaX(1);
			comp.Toffoli(y[1], x[0], x[1]);
			x.SigmaX(0);
			comp.Toffoli(y[0], x[0], x[1]);
			x.SigmaX(0);
			comp.Toffoli(y[0], x[0], x[1]);
			x.SigmaX(1);
			x.SigmaX(0);
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
			// * a = |00>
			// * b = |01>
			// * c = |10>
			// * d = |11>
			// 
			// x2x1x0 = Knoten des Pfades
			int anzahlKnoten = 1;
			int anzahlBitProKnoten = 2;
			int anzahlXBit = anzahlKnoten * anzahlBitProKnoten;
			Register x = comp.NewRegister(0, anzahlXBit);
			// Ancilla Bits
			Register y = comp.NewRegister(0, 2);
			// Ausgabe von Uf
			Register z = comp.NewRegister(1, 1);
			
			comp.Walsh(x);
			comp.Walsh(z);
			
			comp.Uf(x, y, z);
			comp.D2(x);
		}
	}
}
