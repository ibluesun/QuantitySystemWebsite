Functions
=========

Declaration
-----------
Functions in the runtime declared as it is written in mathematical contexts. Functions is not an imperative functions and only used to store mathematical expressions or a map that transform the value from the function domain to its result::

	Qs> f(x) = x^2

Functions can have more than one parameters::

	Qs> f(x,y,z) = x^2+y^2+z^2

Overloaded functions are implemented with parameters names::

	Qs> f(u,v) = u+v^3
	Qs> f(i,j) = i^3-j/exp(j)
	Qs> f(i,j,k) = i/j/k

Function Calling
----------------

Functions are called in the context normally as the rest of other languages, however, if the function is overloaded by parameter names
then the user should specify the desired function to be called by injecting the parameter name.

The runtime will choose the best function suits the named parameters specified::

	Qs> a = f(6,5)
	    
	Qs> f(6,5)						# call f(u,v)
	    DimensionlessQuantity: 131 <1>

	Qs> f(j=5,i=6)					# call f(i,j)
	    DimensionlessQuantity: 215.966310265005 <1>

	Qs> f(v=5,u=6)					# call f(u,v) .. notice the same result.
	    DimensionlessQuantity: 131 <1>

	Qs> f(5,4,2)					# call f(x,y,z)
	    DimensionlessQuantity: 45 <1>

	Qs> f(5,4,k=2)					# call f(i,j,k)  because the inculsion of k in the parameters
	    DimensionlessQuantity: 0.625 <1>


Function Operations
-------------------
Functions in the runtime has an interesting behaviours :). 
Functions can be added, multiplied, subtracted, and divided (thanks to the strong symbolic algebra project implemented by me also :D)

Whenever an operation done to the functions the runtime will grab the parameters and adding them into the new parameters

consider the following example continuing from the last one::

	Qs> @f(u,v) + @f(i,j)  #summing two functions
        DimensionlessQuantity: _(u,v,i,j) = u+v^3+i^3-j/exp(j) <1>

	Qs> @k = @f(u,v) + @f(i,j)  #summing two functions with storing the new function into new name
	Qs> @k
        DimensionlessQuantity: _(u,v,i,j) = u+v^3+i^3-j/exp(j) <1>

	Qs> k(3,4,5,2)    #calling the new function
        DimensionlessQuantity: 191.729329433527 <1>

Function Derivation
-------------------
Another strong ability of the runtime that helps in blending the symbolic calculations with numerical calculations more 
is the ability to derive the function using a syntax that is nearly looks like the normal mathematical expressions
of the partial differntiation.
By suffixing veritcal bar `|` and a symbolic variable that starts with `$`::

	Qs> @K|$x								# No x parameter exists
	    DimensionlessQuantity: _(u,v,i,j) = 0 <1>

	Qs> @K|$u								# differentiate with respect to u
	    DimensionlessQuantity: _(u,v,i,j) = 1 <1>

	Qs> @K|$v
	    DimensionlessQuantity: _(u,v,i,j) = 3*v^2 <1>

	Qs> @K|$i
	    DimensionlessQuantity: _(u,v,i,j) = 3*i^2 <1>

	Qs> @K|$j
	    DimensionlessQuantity: _(u,v,i,j) = -1/exp(j)+j/exp(j) <1>


The differentiation can take more than one variable in the same expression::

	Qs> g(x,y,z) = x^3*y^2*z^4+log(x^2*z^4)
	Qs> @g|$x
	    DimensionlessQuantity: _(x,y,z) = 3*y^2*z^4*x^2+2/x <1>

	Qs> @g|$y
	    DimensionlessQuantity: _(x,y,z) = 2*x^3*z^4*y <1>

	Qs> @g|$z
	    DimensionlessQuantity: _(x,y,z) = 4*x^3*y^2*z^3+4/z <1>

	Qs> @g|$z|$x
	    DimensionlessQuantity: _(x,y,z) = 12*y^2*z^3*x^2 <1>

	Qs> @g|$z|$x|$y
	    DimensionlessQuantity: _(x,y,z) = 24*z^3*x^2*y <1>

	Qs> @g|$x|$y|$y
	    DimensionlessQuantity: _(x,y,z) = 6*z^4*x^2 <1>

	Qs> @g|$x|$y|$z^2
	    DimensionlessQuantity: _(x,y,z) = 72*x^2*y*z^2 <1>


Examples
--------

::

	Cr(n,k) = n!/(k!*(n-k)!)			#Combinications
	Pr(n,r) = n!/(n-r)!					#Permutations

Note: The exlamination mark `!` used to get the factorial






