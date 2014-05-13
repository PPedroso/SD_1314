using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;

class Sum{
	static void Main(string[] args){
		StreamReader fin = new StreamReader(args[0]);
		
		int sum = 0;
		string line;
		while ((line=fin.ReadLine()) != null) 
		{ 
			sum += int.Parse(line);
		} 
		
		Thread.Sleep(5*1000);
		
		StreamWriter fout = new StreamWriter(args[1]);
		fout.WriteLine(sum);
		fin.Close();
		fout.Close();
	
	}
}