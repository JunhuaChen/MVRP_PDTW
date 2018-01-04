Instances for the pickup and delivery problem with time windows used in the paper:

S. Ropke, J.-F. Cordeau, Branch-and-Cut-and-Price for the Pickup and Delivery Problem with Time Windows, submitted, 2007.

The instances use the same format as the DARP instances created by J.-F. Cordeau for the paper: 
J.-F. Cordeau, "A Branch-and-Cut Algorithm for the Dial-a-Ride Problem", Operations Research 54, 573-586, 2006. 
This is why that there are some superfluous entries in the instance files.

The first line in each instance contains five numbers. The first, third and fifth of these numbers can be ignored. The second number is the number of requests (n) and the fourth is the capacity of the vehicles.

After the first lines follows 2n+2 lines defining the nodes in the instance. Nodes 0 and 2n+1 correspond to the start and end depot, nodes 1,...,n corresponds to pickup nodes and nodes n+1, ..., 2n corresponds to delivery nodes. Nodes i and n+i forms a request (1 <= i <= n).

Each line defining a node contains the following data

<id> <x> <y> <service time> <demand> <TW start> <TW end>

where id is the node id, x and y are the coordinates of the node, demand is positive for pickups and negative for deliveries, <TW start> and <TW end> define the start and end of the time window for the node.


Travel costs and travel times were calculated using the Euclidean distance function and were represented as double precision floating point numbers.



The zip file also contains two classes of instances that were not used in the paper, these are the XX and YY instance. Only instance XX45 and YY45 where used in computational tests. The other XX and YY instances were created in the same way as XX45 and Y45 (see the paper).

