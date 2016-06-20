# HoloWorld
This is a Unity Project for HoloLens used in the #HoloTouring conference sessions Real World Holographic App Development by Rene Schulte presented at DevSum 2016, Unite Europe 2016 and NDC Oslo 2016.

The demo project is a simple rigid body physics-based game with a plane and some cubes on top which the user can shoot spheres/balls at from the camera (the user's head) which then bounce off in the real-world environment.  
The project implements the basic HoloLens input and output paradigms like GGV:
* Gaze: The user can aim in the scene which is indicated with a Gaze cursor.
* Gesture: The user can air-tap to shoot a sphere.
* Voice: The user can shoot as well using a voice command. Other voice commands allow to hide the plane or reset the scene.
* Spatial Sound: Collision sound is played back at the collision points in 3D space for an immersive spatial sound experience. 
* Spatial Mapping: The HoloToolkit spatial mapping collider and renderer components are used to let the objects interact physically correct with the real-world. The spatial mesh is also used to occlude the virtual objects with the real-world environment. 


More info here:

https://kodierer.blogspot.de/2016/05/holotouring-stop-1-content-for-devsum.html
https://kodierer.blogspot.de/2016/06/holotouring-stop-2-content-for-unite.html
https://kodierer.blogspot.de/2016/06/holotouring-stop-3-content-for-ndc-oslo.html
