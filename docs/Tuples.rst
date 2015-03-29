Tuples
======

Quantity System Runtime supports tuples but in a very revolutional way.

Tuple is a list of values more specifically quantity system values that can be grouped together in the same variable.

Tuple also can contain another tuples.

Declaration
-----------

Normal Declaration
^^^^^^^^^^^^^^^^^^

:: 

	Qs> T = (20<kg>, 40<m>, "TextValue", (5,3))
	    FlowingTuple (20<kg>, 40<m>, "TextValue", QsTuple[2 Elements])
	Qs> T[0]
	    Mass: 20 kg
	Qs> T[2]
	    TextValue

Declaration with Numerical Identifiers
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

Tuples can be declared with a numerical identifiers ::

	Qs> H = (10:"First Value", 20:"Second Value", 22:50<kg>)
	    FlowingTuple ("First Value", "Second Value", 50<kg>)
	Qs> H:20
	    Second Value
	Qs> H:22
	    Mass: 50 kg

Declaration with Textual Identifiers
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

Tuples can be declared with named elements, and this technique it resemble the key/value structral concept ::

	Qs> N = (Name!"Ahmed Sadek", Age!35, City!"Cairo", Country!"Egypt", Father!(Name!"Sadek", Occupation!"Retired" ))
	    FlowingTuple (Name!"Ahmed Sadek", Age!35<1>, City!"Cairo", Country!"Egypt", Father!QsTuple[2 Elements])
	Qs> N!City
	    Cairo
	Qs> N!Father
	    FlowingTuple (Name!"Sadek", Occupation!"Retired")
	Qs> N!Father!Name
	    Sadek
	Qs> N!Father!Occupation
	    Retired
	Qs> N!Age
	    DimensionlessQuantity: 35 <1>

Declaration with Naming and Identifier
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

To complete the tuple declaration picture, the full syntax of tuple declaration looks like:: 

	Qs> GearBox = (0:Neutral!"", 1:FirstShift!"Maximum Power", 2:SecondShift!"Getting Serious", 3:ThirdShift!"Are you nuts?!!", 4:FourthShift!"Let me down", 5:FifthShift!"Maximum Speed")
	    FlowingTuple (Neutral!"", FirstShift!"Maximum Power", SecondShift!"Getting Serious", ThirdShift!"Are you nuts?!!", FourthShift!"Let me down", FifthShift!"Maximum Speed")
	Qs> GearBox:3
	    Are you nuts?!!
	Qs> GearBox!SecondShift
	    Getting Serious

	
