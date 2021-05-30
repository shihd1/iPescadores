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

### Slideshow
![alt text](https://github.com/shihd1/iPescadores/blob/master/ImageFolder/Capstone%20Project%20-%20Penghu.jpg?raw=true)
![alt text](https://github.com/shihd1/iPescadores/blob/master/ImageFolder/Capstone%20Project%20-%20Penghu%20(1).jpg?raw=true)
![alt text](https://github.com/shihd1/iPescadores/blob/master/ImageFolder/Capstone%20Project%20-%20Penghu%20(2).jpg?raw=true)
![alt text](https://github.com/shihd1/iPescadores/blob/master/ImageFolder/Capstone%20Project%20-%20Penghu%20(3).jpg?raw=true)

### Future Work
- Find out the true scaling factor
- Apply statistical methods to analyze the margin of error of the approach
- Optimize AI program parameters
- Plot values onto map
- Playtesting
- Optimize user movement within the game
- Expand upon the collectibles
- Promotion
