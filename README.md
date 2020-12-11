# Particles simulation

This project implements several algorithms from the *Particle-based Viscoelastic Fluid Simulation* article in C# Unity.

The idea is to spawn hundreds of particles that simulate a fluid behaviour at runtime. 
These particles take into account :
* physics rules relative to the gravity
* double density relaxation between neighboring particles
* viscosity impulses between pairs of particles

The rules of physics also take into account the mass of each particle. (the color of particles is a color gradient between blue and red from the lightest to the heaviest depending on their mass)

To increase the performances of the system, a particle does not consider every other particles as neighbors. Instead, every particles are placed in a virtual infinite grid generated at runtime. Particles only consider neighboring particles based on their position in the grid. Thus, the time complexity of the system is (almost) linear instead of quadratic.

![simulation](https://github.com/vclimpont/particules-simulation/blob/main/Images/simulation.gif)

![final state](https://github.com/vclimpont/particules-simulation/blob/main/Images/massState.PNG)

## Source

Particle-based Viscoelastic Fluid Simulation : http://www.ligum.umontreal.ca/Clavet-2005-PVFS/pvfs.pdf
