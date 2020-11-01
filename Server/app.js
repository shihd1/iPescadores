const e = require('express');
const express = require('express')
const app = express()
const port = 3000

const { MongoClient } = require("mongodb");

// Replace the uri string with your MongoDB deployment's connection string.
const uri = "mongodb://localhost:27017/";

const client = new MongoClient(uri);

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
async function getLevel(InputID) {
    try {
        await client.connect();

        const database = client.db("PenghuProject");
        const collection = database.collection("UserInfo");
        console.log(InputID);
        const query = { 'id': 1604026294571 };
        const options = { upsert: false };

        var result = await collection.findOne(query, options);
    } finally {
        return [result];
    }
}
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
        const doc = { id: id, username: username, firstname: firstname, lastname: lastname, password: password, friendList: [], friendRequest: [], level: 1 };

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
        result.friendRequest.push(userID);
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
async function loadFriend(id) {
    var end;
    try {
        await client.connect();

        const database = client.db("PenghuProject");
        const collection = database.collection("UserInfo");

        const query = { id: id };
        const options = { upsert: false };

        var result = await collection.findOne(query, options);
        end = result.count() == 0;
    } finally {
        return [end, result];
    }
}


app.get('/getLevel/:id', (req, res) => {
    var id = req.params.id;
    getLevel(id)
        .then((r) => {
            res.send({ 'status': 'success', level: r[0].level });
        })
        .catch(() => {
            res.send({ 'status': 'fail' });
        })
})
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
            //console.log("------------------------------")
            if (r[0] == true) {
                //console.log("------------------------------A");
                res.send({ 'status': 'success', id: r[1].id });
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
app.get('/loadfriend/:id', (req, res) => {
    var id = req.params.id;
    loadFriend(id)
        .then((r) => {
            if (r[0] == true) {
                res.send({ 'status': 'success', friends: r[1].friendID });
            } else {
                res.send({ 'status': 'fail' });
            }
        })
        .catch(() => {
            res.send({ 'status': 'fail' });
        })
});
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

app.listen(port, () => {
    console.log(`Example app listening at http://localhost:${port}`)
})