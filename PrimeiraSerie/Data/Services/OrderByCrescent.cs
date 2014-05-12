using System;
using System.Diagnostics; 
using System.IO; 
using System.Threading;
using System.Collections;

class OrderByCrescent 
 {
 // recebe em args dois nomes de ficheiros: 
 // 1 arg - ficheiro com numeros inteiros (1 por linha) 
 // 2 arg - ficheiro onde vai escrever os numeros inteiros ordenados 
	 static void Main(string[] args) 
	 { 
		ArrayList tab = new ArrayList(); 
		StreamReader fin = new StreamReader(args[0]); 
		string line; 
		while ((line=fin.ReadLine()) != null) 
		{ 
			tab.Add(int.Parse(line)); 
		} 
		tab.Sort(); 
		Thread.Sleep(8*1000); // simula tempo de execução longo 
		StreamWriter fout = new StreamWriter(args[1]); 
		Console.WriteLine("marker");
		foreach (object obj in tab) 
		fout.WriteLine((int)obj); 
		fin.Close(); fout.Close(); 
	} 
 } 
