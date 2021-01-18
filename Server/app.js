const express = require('express')
const app = express()
const port = 3000
const router = require('./router');

const { MongoClient } = require("mongodb");

// Replace the uri string with your MongoDB deployment's connection string.
const uri = "mongodb://localhost:27017/";

const client = new MongoClient(uri);

//router
//app.use(express.static('public'));
//app.set('view engine', 'ejs');
//app.use('/upload', router);

async function run() {
    try {
        await client.connect();

        const database = client.db("PenghuProject");
        const collection = database.collection("UserInfo");

        // create a document to be inserted
        // const docs = [
        //     { name: "Red", town: "kanto" },
        //     { name: "Blue", town: "kanto" }
        // ];
        const filter = { name: "Blue" };

        // const options = { ordered: true };

        // this option instructs the method to [not] create a document if no documents match the filter
        const options = { upsert: false };

        const updateDoc = {
            $set: {
                town: "kanto",
            }
        };

        //const result = await collection.insertMany(docs, options);
        const result = await collection.updateOne(filter, updateDoc, options);

        console.log(
            `${result.matchedCount} documents were inserted`,
        );
    } finally {
        //await client.close();
    }
}

//General
async function getID(username) {
    try {
        await client.connect();

        const database = client.db("PenghuProject");
        const collection = database.collection("UserInfo");
        const query = { username: username };
        const options = { upsert: false };

        var result = await collection.findOne(query, options);
    } finally {
        return result;
    }
}
async function getPerson(id) {
    try {
        await client.connect();

        const database = client.db("PenghuProject");
        const collection = database.collection("UserInfo");
        const query = { 'id': parseInt(id) };
        const options = { upsert: false };

        var result = await collection.findOne(query, options);
    } finally {
        return result;
    }
}

//Create Account
async function checkUser(username) {
    var result;
    try {
        await client.connect();

        const database = client.db("PenghuProject");
        const collection = database.collection("UserInfo");

        const filter = { username: username };
        //this method instructs the method to not create a document if no documents match the filter
        const options = { upsert: false };

        result = await collection.countDocuments(filter, options);
    } finally {
        return result;
    }
}
async function addUser(id, username, firstname, lastname, password) {
    try {
        await client.connect();

        const database = client.db("PenghuProject");
        const collection = database.collection("UserInfo");

        // create a document to be inserted
        const doc = {
            username: username, firstname: firstname, lastname: lastname, password: password,
            id: id,
            level: 1,
            coins: 0,
            totalXP: 0,
            friendList: [],
            friendRequest: [],
            achievementStatus: [true, false, false],
            numLife: [],
        };

        const result = await collection.insertOne(doc);
    } finally {
        //await client.close();
    }
}

//Sign In
async function signIn(username, password) {
    var end;
    try {
        await client.connect();

        const database = client.db("PenghuProject");
        const collection = database.collection("UserInfo");

        const query = { username: username };
        const options = { upsert: false };

        var result = await collection.findOne(query, options);
        end = result.password == password;
    } finally {
        return [end, result];
    }
}

//Friends
async function sendFriendRequest(userID, friendID) {
    try {
        await client.connect();

        const database = client.db("PenghuProject");
        const collection = database.collection("UserInfo");
        const filter = { id: parseInt(friendID) };
        const options = { upsert: false };

        var result = await collection.findOne(filter, options);
        if (result.friendRequest.indexOf(userID) == -1 && result.friendList.indexOf(userID) == -1) {
            result.friendRequest.push(userID);
        }
        const updateDoc = {
            $set: {
                friendRequest: result.friendRequest
            }
        };
        var result = await collection.updateOne(filter, updateDoc, options);
        end = result.modifiedCount == 1;
    } finally {
        return end;
    }
}
async function addFriend(userID, friendID) {
    try {
        await client.connect();

        const database = client.db("PenghuProject");
        const collection = database.collection("UserInfo");
        const filter = { id: parseInt(userID) };
        const options = { upsert: false };

        var result = await collection.findOne(filter, options);
        result.friendList.push(friendID);
        const updateDoc = {
            $set: {
                friendList: result.friendList
            }
        };
        var result = await collection.updateOne(filter, updateDoc, options);
        end = result.modifiedCount == 1;
    } finally {
        return end;
    }
}
async function removeFriendRequest(userID, friendID) {
    try {
        await client.connect();

        const database = client.db("PenghuProject");
        const collection = database.collection("UserInfo");
        const filter = { id: parseInt(userID) };
        const options = { upsert: false };

        var result = await collection.findOne(filter, options);
        const index = result.friendRequest.indexOf(friendID);
        if (index > -1) {
            result.friendRequest.splice(index, 1)
        }
        console.log(result.friendRequest);
        const updateDoc = {
            $set: {
                friendRequest: result.friendRequest
            }
        };
        var result = await collection.updateOne(filter, updateDoc, options);
        end = result.modifiedCount == 1;
    } finally {
        return end;
    }
}

//Update User Info
async function updateTotalXP(userID, newXP) {
    try {
        await client.connect();

        const database = client.db("PenghuProject");
        const collection = database.collection("UserInfo");
        const filter = { id: parseInt(userID) };
        const options = { upsert: false };
        const updateDoc = {
            $set: {
                totalXP: parseInt(newXP)
            }
        };
        var result = await collection.updateOne(filter, updateDoc, options);
        end = result.modifiedCount == 1;
    } finally {
        return end;
    }
}
async function updateCoins(userID, newCoins) {
    try {
        await client.connect();

        const database = client.db("PenghuProject");
        const collection = database.collection("UserInfo");
        const filter = { id: parseInt(userID) };
        const options = { upsert: false };
        const updateDoc = {
            $set: {
                coins: parseInt(newCoins)
            }
        };
        var result = await collection.updateOne(filter, updateDoc, options);
        end = result.modifiedCount == 1;
    } finally {
        return end;
    }
}
async function updateAchievementStatus(userID, index) {
    try {
        await client.connect();

        const database = client.db("PenghuProject");
        const collection = database.collection("UserInfo");
        const filter = { id: parseInt(userID) };
        const options = { upsert: false };

        var result = await collection.findOne(filter, options);
        result.achievementStatus[index] = true;
        const updateDoc = {
            $set: {
                achievementStatus: result.achievementStatus
            }
        };
        var result = await collection.updateOne(filter, updateDoc, options);
        end = result.modifiedCount == 1;
    } finally {
        return end;
    }
}
async function updateNumLife(userID, index, newLife) {
    try {
        await client.connect();

        const database = client.db("PenghuProject");
        const collection = database.collection("UserInfo");
        const filter = { id: parseInt(userID) };
        const options = { upsert: false };

        var result = await collection.findOne(filter, options);
        result.numLife[index] = newLife;
        const updateDoc = {
            $set: {
                numLife: result.numLife
            }
        };
        var result = await collection.updateOne(filter, updateDoc, options);
        end = result.modifiedCount == 1;
    } finally {
        return end;
    }
}

app.get('/getID/:username', (req, res) => {
    var username = req.params.username;
    getID(username)
        .then((r) => {
            //console.log("------------------------------")
            if (r != null) {
                //console.log("------------------------------A");
                res.send({ 'status': 'success', id: r.id });
            } else {
                //console.log("------------------------------B");
                res.send({ 'status': 'fail' });
            }
        })
        .catch(() => {
            //console.log("------------------------------C");
            res.send({ 'status': 'fail' });
        });
});
app.get('/getLevel/:id', (req, res) => {
    var id = req.params.id;
    getPerson(id)
        .then((r) => {
            //console.log("------------------------------")
            if (r != null) {
                //console.log("------------------------------A");
                res.send({ 'status': 'success', level: r.level });
            } else {
                //console.log("------------------------------B");
                res.send({ 'status': 'fail' });
            }
        })
        .catch(() => {
            //console.log("------------------------------C");
            res.send({ 'status': 'fail' });
        });
})
app.get('/getusername/:id', (req, res) => {
    var id = req.params.id;
    getPerson(id)
        .then((r) => {
            //console.log("------------------------------")
            if (r != null) {
                //console.log("------------------------------A");
                res.send({ 'status': 'success', username: r.username });
            } else {
                //console.log("------------------------------B");
                res.send({ 'status': 'fail' });
            }
        })
        .catch(() => {
            //console.log("------------------------------C");
            res.send({ 'status': 'fail' });
        });
});
app.get('/getXP/:id', (req, res) => {
    var id = req.params.id;
    getPerson(id)
        .then((r) => {
            //console.log("------------------------------")
            if (r != null) {
                //console.log("------------------------------A");
                res.send({ 'status': 'success', totalXP: r.totalXP });
            } else {
                //console.log("------------------------------B");
                res.send({ 'status': 'fail' });
            }
        })
        .catch(() => {
            //console.log("------------------------------C");
            res.send({ 'status': 'fail' });
        });
});
app.get('/getCoins/:id', (req, res) => {
    var id = req.params.id;
    getPerson(id)
        .then((r) => {
            //console.log("------------------------------")
            if (r != null) {
                //console.log("------------------------------A");
                res.send({ 'status': 'success', coins: r.coins });
            } else {
                //console.log("------------------------------B");
                res.send({ 'status': 'fail' });
            }
        })
        .catch(() => {
            //console.log("------------------------------C");
            res.send({ 'status': 'fail' });
        });
});

app.get('/userexist/:username', (req, res) => {
    var username = req.params.username;
    checkUser(username)
        .then((r) => {
            console.log(r);
            if (r == 0) {
                res.send({ 'status': 'success', 'exist': false });
            } else {
                res.send({ 'status': 'success', 'exist': true });
            }
        })
        .catch((r) => {
            res.send({ 'status': 'fail' });
        });
});
app.get('/useradd/:username/:firstname/:lastname/:password', (req, res) => {
    var id = new Date().getTime();
    var username = req.params.username;
    var firstname = req.params.firstname;
    var lastname = req.params.lastname;
    var passsword = req.params.password;
    addUser(id, username, firstname, lastname, passsword)
        .then(() => {
            res.send({ 'status': 'success', id: id });
        })
        .catch(() => {
            res.send({ 'status': 'fail' });
        });
});
app.get('/signin/:username/:password', (req, res) => {
    var username = req.params.username;
    var password = req.params.password;
    signIn(username, password)
        .then((r) => {
            if (r[0] == true) {
                res.send({
                    'status': 'success',
                    id: r[1].id,
                    level: r[1].level,
                    coins: r[1].coins,
                    totalXP: r[1].totalXP,
                    friendRequest: r[1].friendRequest,
                    friendList: r[1].friendList,
                    achievementStatus: r[1].achievementStatus,
                    numLife: r[1].numLife,
                });
            } else {
                res.send({ 'status': 'fail' });
            }
        })
        .catch(() => {
            res.send({ 'status': 'fail' });
        });
});

app.get('/sendfriendrequest/:userID/:friendID', (req, res) => {
    var userID = req.params.userID;
    var friendID = req.params.friendID;
    sendFriendRequest(userID, friendID)
        .then((r) => {
            if (r == true) {
                res.send({ 'status': 'success' });
            } else {
                res.send({ 'status': 'fail' });
            }
        })
        .catch(() => {
            res.send({ 'status': 'fail' });
        })
})
app.get('/addfriend/:userID/:friendID', (req, res) => {
    var userID = req.params.userID;
    var friendID = req.params.friendID;
    addFriend(userID, friendID)
        .then((r) => {
            if (r == true) {
                res.send({ 'status': 'success' });
            } else {
                res.send({ 'status': 'fail' });
            }
        })
        .catch(() => {
            res.send({ 'status': 'fail' });
        })
});
app.get('/removefriendrequest/:userID/:friendID', (req, res) => {
    var userID = req.params.userID;
    var friendID = req.params.friendID;
    removeFriendRequest(userID, friendID)
        .then((r) => {
            if (r == true) {
                res.send({ 'status': 'success' });
            } else {
                res.send({ 'status': 'fail' });
            }
        })
        .catch(() => {
            res.send({ 'status': 'fail' });
        })
});
// app.get('/loadfriend/:id', (req, res) => {
//     var id = req.params.id;
//     getUsername(id)
//         .then((r) => {
//             console.log("------------------------------")
//             if (r != null) {
//                 console.log("------------------------------A");
//                 res.send({ 'status': 'success', username: r.username });
//             } else {
//                 console.log("------------------------------B");
//                 res.send({ 'status': 'fail' });
//             }
//         })
//         .catch(() => {
//             console.log("------------------------------C");
//             res.send({ 'status': 'fail' });
//         });
// });

app.get('/updateTotalXP/:userID/:newXP', (req, res) => {
    var userID = req.params.userID;
    var newXP = req.params.newXP;
    updateTotalXP(userID, newXP)
        .then((r) => {
            if (r == true) {
                res.send({ 'status': 'success' });
            } else {
                res.send({ 'status': 'fail' });
            }
        })
        .catch(() => {
            res.send({ 'status': 'fail' });
        })
});
app.get('/updateCoins/:userID/:newCoins', (req, res) => {
    var userID = req.params.userID;
    var newCoins = req.params.newCoins;
    updateCoins(userID, newCoins)
        .then((r) => {
            if (r == true) {
                res.send({ 'status': 'success' });
            } else {
                res.send({ 'status': 'fail' });
            }
        })
        .catch(() => {
            res.send({ 'status': 'fail' });
        })
});
app.get('/updateAchievementStatus/:userID/:index', (req, res) => {
    var userID = req.params.userID;
    var index = req.params.index;
    updateAchievementStatus(userID, index)
        .then((r) => {
            if (r == true) {
                res.send({ 'status': 'success' });
            } else {
                res.send({ 'status': 'fail' });
            }
        })
        .catch(() => {
            res.send({ 'status': 'fail' });
        })
});
app.get('/updateNumLife/:userID/:index/:newLife', (req, res) => {
    var userID = req.params.userID;
    var index = req.params.index;
    var newLife = req.params.newLife;
    updateNumLife(userID, index, newLife)
        .then((r) => {
            if (r == true) {
                res.send({ 'status': 'success' });
            } else {
                res.send({ 'status': 'fail' });
            }
        })
        .catch(() => {
            res.send({ 'status': 'fail' });
        })
});

app.listen(port, () => {
    console.log(`Example app listening at http://localhost:${port}`)
})