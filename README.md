<h1 align="center">Soft Body Physics </h1>
The soft physics deformation algorithm works by simulating wave-like vertex displacement in response to collisions. When an object impacts the mesh, the script calculates a wave function that propagates from the collision point, using sine and exponential functions to create a natural, physics-inspired deformation. The wave's amplitude is modulated by collision force, gravity, and vertical position, ensuring more dramatic movement at the top of the object. A smoothing interpolation technique prevents abrupt shape changes by gradually transitioning vertices between their original and deformed positions, creating a fluid, organic motion that mimics the soft, responsive behavior of semi-rigid materials. The algorithm continuously dampens the wave's intensity over time, allowing the mesh to gradually return to its original shape while maintaining a realistic, dynamic response to physical interactions.

<h2 align="center">Demonstrations</h2>
<table align="center">
  <tr>
    <td><img src="https://github.com/user-attachments/assets/6b54b303-3b45-411a-8813-4a1382f0336d" alt="Going down stairs" width="400"></td>
    <td><img src="https://github.com/user-attachments/assets/ab34e5f0-74a6-4362-a53b-00ccf2c4533d" alt="Demonstration" width="400"></td>
  </tr>
</table>

## Setup:
1. Import the *Soft* folder into a Unity project.
2. Two prefabs have already been setup and can be used immediately by dragging them into the scene.

## Using Other Meshes:
1. Other meshes can be created in a appropriate 3D modeling software
3. The mesh must be composed of quads—Unity will triangulate these when the model is imported
4. More topology = higher resolution deformation
5. Ensure **Read/Write** is enabled in the model import settings
6. Attach the *Soft_Physics* script to the mesh
7. Enable **Convex** in the mesh collider component
8. Configure the settings for the script in the inspector to your liking—this may take some time
