
Introduction
============

The **Quantity System** is a platform that contains two foundational libraries. *Quantity System Framework* and the *Quantity System Runtime*

**Quantity System Framework** is a nearly complete solution for adding the physical units functionality into any CLR compatible language (Quantity System itself is written with C#)

**Quantity System Runtime** is a dynamic language intended for scientists and engineers to aid them in making a clear calculations by aiding of units, and based on the underlieng quantity dimension.

one of the most distinguishable factors of quantity system is its unique handling of quantities by implying the use of dimensional analysis for all the quantities it contains.

historically a lot of attempts has been made to add the units of measurments into the programming languages either dynamically in runtime or in compile time. 

http://jscience.org/ for example has added the support of units of measurments.

F# also has introduced the concept of units of measurements to be used in a compile time approach.

another noticable project called Frink in http://futureboy.us/frinkdocs/ employs also the units of measurements.

These projects however, despite of their understanding of the quantity dimensions of the basic quantities, didn't introduce 
a solution that is a dimensionally unique .. but instead introduced a solution that depends on the units itself 
to distringush between different quantities.

This approach may help in keeping the consistency of summing different units .. but it doesn't help in predicting the quantiy that
comes out from the rest of mathematical operations i.e. multiplication and division of quantities .. and in turn prevent the quantity type check in calculations.

The approach of treating quantities with their units instead of their dimensions was unavoidable due to the look alike dimensions
of quantities such that Work and Torque. The dimension similarties between these quantities and other quantities led any attempt to make a truly quantity validation an impossible process.

In this study and this project, we are keeping a high profile of the engineering requirements for keeping the quantity types consistency during calculations. This study introduced a new solution for differentiating between 
the similar quantities yet different in their physical meaning to keep the consisteny of quantities during calculations.
