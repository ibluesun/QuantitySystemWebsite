Runtime Sequences
=================

This unique feature is only included in the quantity system runtime which permits the user to form a mathematical sequences.

Declaration
-----------

The sequence in mathematics is used to generate values, the simplest example for a sequence::


	Qs> S[n] ..> 2*n
	Qs> S[0]
	    DimensionlessQuantity: 0 <1>
	Qs> S[1]
	    DimensionlessQuantity: 2 <1>
	Qs> S[10]
	    DimensionlessQuantity: 20 <1>	

Another valid declaration is to define the sequence without any counters at all::

	Qs> P[] ..> 10;20;30
	Qs> P[1]
	    DimensionlessQuantity: 20 <1>
	Qs> P[30]
	    DimensionlessQuantity: 30 <1>

The last sequence expression is repeated when calling the sequence with an index that exceeds the declared ones.
In previous example the 30 value were called at the index 30 because of that behaviour. The runtime will always gets the latest
value occured on the latest indexed expression in the definition.


Fibonaccy Example::
^^^^^^^^^^^^^^^^^^

	Qs> fib[n] ..> 0; 1; fib[n-1] + fib[n-2]			#fibonaccy sequence
	Qs> fib[0]
	    DimensionlessQuantity: 0 <1>
	Qs> fib[1]
		DimensionlessQuantity: 1 <1>
	Qs> fib[5]
		DimensionlessQuantity: 5 <1>
	Qs> fib[6]
		DimensionlessQuantity: 8 <1>
	Qs> fib[1..10]
		QsVector: 1<1> 1<1> 2<1> 3<1> 5<1> 8<1> 13<1> 21<1> 34<1> 55<1>
	Qs> fib[0..10]
		QsVector: 0<1> 1<1> 1<1> 2<1> 3<1> 5<1> 8<1> 13<1> 21<1> 34<1> 55<1>



Sequence with Parameters
-------------------
While sequence can have one sequence number that serves as a counter, the sequence can also have a parameters.

when calling the sequence without specifying a parameter the sequence returns a function expression that can be stored into a function variable.




Sequence Calling
----------------
In addition to calling the sequence with indices sequence has another calling techniques that resemble mathematical series.

#. [n ++ m]: summation of series from `n` to `m`
#. [n ** m]: multiplication of series from `n` to `m`
#. [n !! m]: Average value between elements from `n` to `m`
#. [n !% m]: Standard Deviation between elements from `n` to `m`
#. [n .. m]: Returns Vector if the elements results were scalars, and matrix if the elements results were vectors.



consider the sin series to get the sin value of any number. The sequence declaration of sin is::

	math:my_sin[n](x) ..> ((-1)^n*x^(2*n+1))/(2*n+1)!		#Sin sequence

This sequence has one counter and one parameter. One can encapsulate the sequence calls into a function

While the function
math:sin(x) = math:sin[0++50](x)							#Sin function

