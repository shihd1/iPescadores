# iPescadores

iPescadores is a mobile game that utilizes crowdsourcing techniques to produce an updating map of the varying degrees of pollution in Penghu. 

### Motivation
The core idea is to encourage users to take pictures of trashed areas by rewarding them with advancements in-game.
The pictures that are provided by the users are then passed into a classification model to separate the pictures based on the levels of pollution.
Accurate estimates of trash volume leads to quick and accurate estimates of the number of volunteers and supplies needed, thereby maximizing the efficiency of cleanup efforts.

### Community-based Design
This game encourages players to make new friends. Once another player is added as a friend, both lands can be connected together to form a larger ecosystem.
I had to find a way for users’ game information to be uploaded to a server. I built an application server( Nodejs / express framework ) to provide Restful APIs. 
It realized the interactive communication between users’ device and server. 
The NOSQL Database (MongoDB) has flexible schema and document based structure making it easy to organize the data and handle schema changes in requirements. 
I choose MongoDB as my database.

### Display
Before uploading the photo:
![alt text](https://github.com/shihd1/iPescadores/blob/master/ImageFolder/HexagonBefore.PNG?raw=true)

After uploading the photo:
![alt text](https://github.com/shihd1/iPescadores/blob/master/ImageFolder/HexagonAfter.PNG?raw=true)

### Aspirations/Vision
Use of state-of-art technique: Mask-RCNN
New research on creating planes from a single image: https://single-view-mpi.github.io/ 
I plan to try a combination of Mask R-CNN (which would recognize trash and apply masks) and multiplane images (to predict the depth) to create a rough estimate on the amount of trash.
