# unity-pathfinding-2d

This is my implementation of AStar pathing algorithm for unity in 2d.  It's not quite perfect but for the majority of your pathing needs it should get the job done!

## Classes

### Navigation Mesh Component

The navigation mesh contains the data about individual tiles, or "nodes".  Each node will detect if any solids overlap it.  A solid in this case any sort of collider.  The inspector values contain a grid width / height for the number of total nodes in the grid, the tile size (in units), and an option to update the navigation mesh every frame. 

#### Sync Every Frame

This is enabled by default, however, if you have a level which is stationary (no colliders move around) you will gain tons of performance by turning this off.  This requires each and every node to perform a collision check each frame, which is slow. 

However, if you need the nav mesh to recalculate each frame, leave this on.  This will allow the algorithm to be run each frame, so that for example if a platform moves left and right across the screen, the path finder can re-route around it.


### Node

The node class simply holds data on an individual node, such as whether it is solid, its center position in world space, etc

### AStarPathMapper

The AStarPathMapper holds all the logic driving the pathing algorithm, and more.  The important piece is the ```FindPath``` method, which will automatically return a list of nodes that will lead your pathfinder to its destination.  Simply pass in two world positions and let the algorithm do all the work :)

## How do i use this

In this unity project, you'll see the PathFinderBehavior, which is a very basic example of how to find a path to a destination every frame.  It simply calls the pathMapper.Findpath() method during its update.  Obviously you can tweak this for better performance.  

In the past i've written unity components that will take the resultant list of nodes, convert it to a stack, and then the behavior will:
- Pop off a node from the stack
- move towards that node's center 
- when it reaches it, it will pop off the next node fro the stack
- if there are no more nodes in the stack, do nothing

### Known Issues

The algorithm has the capability to path through diagonals, however, it currently won't, because it calculates the cost of a diagonal to be the same as moving up and over, etc

### License

This software is licenesd upder the MIT license.  Please see LICENSE.txt for details.