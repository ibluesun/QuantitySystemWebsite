Quantity System Framework
=========================

The framework of quantity system contains the physical quantities beside their representing units in a strongly typed way
that defines the quantity name and its underlieng dimension and its corresponding units.



QuantityDimension
-----------------
This class is the representation of the 

AnyQuantity<T>
--------------
This class is the base class of all base quantities defined in the framework. The base quantities are those quantities serves as the most abstracted physical quantities such as Length, and Mass.

This class contains all the mathematical operations algorithms required to carry out the calculations between different 
inherited quantities.

`<T>` The T type is the storage type that is internally stored in the quantity. This architecture design
was selected after a thoughtful considerations based on the idea that the quantity of something should not 
be limited to one numerical type aka double precession for example.

This architecture allows the extension of the framework to use any numerical storage required such as Big Integers in addition any user specific storage.

In this direction .. the one is able to sum a mass of integer to the mass of double `Mass<double>+Mass<int>`
as long as there is an op_add operator between the double and integer types (which ofcourse it exists)

The framework however is optimized for the `<T>` of value types such as all numerical based types.

Base Quantities
---------------

#. Mass
#. Length
#. Time
#. ElectricCurrent
#. Temperature
#. AmountOfSubstance
#. LuminousIntensity
#. Currency

The first seven quantities are the nature physical quantities, however, the last quantity is the currency which holds all the currency units in the world.

It worth mentioning also that the framework should implement a special quantity for the data storage as of bit, byte ,.. etc.



DerivedQuantity<T>
------------------
This class is the direct parent of all derived quantities defined in the framework. The class contains the required algorithms to discover the returned quantity from calculations plus 
it acts as a generic quantity for the quantities that doesn't have a strongly typed class.

Following code illustrate how Force Quantity is being declared using this base class::

    public class Force<T> : DerivedQuantity<T>
    {
        public Force()
            : base(1, new Mass<T>(), new Acceleration<T>())
        {
        }

        public Force(float exponent)
            : base(exponent, new Mass<T>(exponent), new Acceleration<T>(exponent))
        {
        }

        public static implicit operator Force<T>(T value)
        {
            Force<T> Q = new Force<T>();

            Q.Value = value;

            return Q;
        }

    }







