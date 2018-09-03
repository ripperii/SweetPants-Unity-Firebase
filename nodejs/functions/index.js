const functions = require('firebase-functions');
const admin = require('firebase-admin');
admin.initializeApp();

const key = "G78aLMP02kaZMaUsqor2Mg";
const express = require('express');
const cookieParser = require('cookie-parser')();
const cors = require('cors')({ origin: true });
const moment = require('moment');
const bodyParser = require('body-parser');
const app = express();

// User Authentication Method
const validateFirebaseIdToken = (req, res, next) => {
    console.log('Check if request is authorized with Firebase ID token');
    console.log(req.url);

    if ((!req.headers.authorization || !req.headers.authorization.startsWith('Bearer ')) &&
        !req.cookies.__session) {
        console.error('No Firebase ID token was passed as a Bearer token in the Authorization header.',
            'Make sure you authorize your request by providing the following HTTP header:',
            'Authorization: Bearer <Firebase ID Token>',
            'or by passing a "__session" cookie.');
        res.status(403).send('Unauthorized');
        return;
    }

    let idToken;
    if (req.headers.authorization && req.headers.authorization.startsWith('Bearer ')) {
        console.log('Found "Authorization" header');
        // Read the ID Token from the Authorization header.
        idToken = req.headers.authorization.split('Bearer ')[1];
    } else {
        console.log('Found "__session" cookie');
        // Read the ID Token from cookie.
        idToken = req.cookies.__session;
    }
    admin.auth().verifyIdToken(idToken).then((decodedIdToken) => {
        console.log('ID Token correctly decoded', decodedIdToken);
        req.user = decodedIdToken;
        return next();
    }).catch((error) => {
        console.error('Error while verifying Firebase ID token:', error);
        res.status(403).send('Unauthorized');
    });
};

// HTTP Request triggers
exports.cronResetCounters = functions.https.onRequest((req, res) => {

    if (req.method === 'PUT' || req.query.key !== key) {
        res.status(403).send('Forbidden!');
    }
    var db = admin.database();
    var ref = db.ref("players");

    // Attach an asynchronous callback to read the data
    ref.once("value", snapshot => {

        snapshot.forEach(player => {

            player.child("ShopTimerSkipedCount").ref.set(0);

        });

    }, (errorObject) => {
            console.log("The read failed: " + errorObject.code);
        });
    res.sendStatus(200);
});

app.use(cors);
app.use(bodyParser.json()); // for parsing application/json
app.use(bodyParser.urlencoded({ extended: true })); // for parsing application/x-www-form-urlencoded
app.use(cookieParser);
app.use(validateFirebaseIdToken);
app.post("/adventurer", (req, res) => {

    console.log(req.body);
    var playerId = req.user.user_id;
    var seconds = req.body.seconds;

    var db = admin.database();
    var Adventurers = db.ref("adventurers");
    var items = db.ref("items");
    var player = db.ref("players/" + playerId);
    var playerShowcaseItems = player.child("ShowcaseItems");
    var playerShopTimer = player.child("ShopTimer");
    var pendingAdventurer = player.child("PendingAdventurer");

    var adventurer;

    Promise.all([Adventurers.once("value"), items.once("value"), player.once("value")])
        .then(snaps => {

            var applicableAdventurers = [];

            Adventurers = snaps[0];
            items = snaps[1];
            playerShowcaseItems = snaps[2].child("ShowcaseItems");

            console.log("Diff: " + moment().diff(moment(snaps[2].child("ShopTimer").val(), 'x')));

            if (moment().diff(moment(snaps[2].child("ShopTimer").val(), 'x'), 'seconds') < 0) {
                res.status(200).send('Timer has not run out!!!');
            }
            Adventurers.forEach((childSnapshot) => {
                var levelRange = childSnapshot.child("levelRange");

                if (req.body.level >= levelRange.child("minLevel").val() && req.body.level <= levelRange.child("maxLevel").val()) {
                    applicableAdventurers.push(childSnapshot);
                }
            });

            adventurer = applicableAdventurers[Math.floor(Math.random() * applicableAdventurers.length)];
            var id = adventurer.key;
            var type = req.body.type === 0 ? getRndInteger(1, 2) : req.body.type;

            switch (type)
            {
                case 1:

                    var adventurerItems = adventurer.child("items");
                    var temp = {};
                    adventurerItems.forEach((it) => {
                        if (Math.random() <= it.val().chance) {
                            temp[it.key] = getRndInteger(it.val().minAmount, it.val().maxAmount);
                        }
                    });
                    if (!isEmpty(temp)) {
                        adventurer = adventurer.val();

                        adventurer.items = temp;
                        adventurer.type = 1;

                        delete adventurer.order;
                        delete adventurer.buyLine;
                        delete adventurer.orderLine;
                        console.log("Type: " + adventurer.type);

                        break;
                    }

                case 2:
                    var applicableItems = [];
                    console.log(playerShowcaseItems.val());
                    playerShowcaseItems.forEach((item) => {
                        console.log(items.child(item.val().item).child("subtype").val());
                        if (adventurer.child("seekedItemType").val() === items.child(item.val().item).child("subtype").val()) {
                            applicableItems.push(item);
                        }
                    });
                    if (applicableItems.length > 0) {
                        var item = applicableItems[Math.floor(Math.random() * applicableItems.length)];

                        adventurer = adventurer.val();

                        adventurer.buyItem = item.key;
                        adventurer.line = adventurer.buyLine;
                        adventurer.type = 2;

                        delete adventurer.items;
                        delete adventurer.order;
                        console.log("Type: " + adventurer.type);
                        break;
                    }
                case 3:
                    adventurer = adventurer.val();
                    for (var i = 0; i < adventurer.order.length; i++)
                    {
                        adventurer.order[i].amount = getRndInteger(adventurer.order[i].minAmount, adventurer.order[i].maxAmount);
                        delete adventurer.order[i].minAmount;
                        delete adventurer.order[i].maxAmount;
                    }
                    adventurer.line = adventurer.orderLine;
                    adventurer.type = 3;

                    delete adventurer.items;
                    delete adventurer.seekedItemType;
                    console.log("Type: " + adventurer.type);

                    break;
                default:
                    res.status(200).send("Invalid type!");
                    break;
            }
            adventurer.id = id;
            
            delete adventurer.levelRange;
            delete adventurer.statReq;
            delete adventurer.seekedItemType;
            delete adventurer.buyLine;
            delete adventurer.orderLine;

            adventurer.currentTime = moment().format('x');
            adventurer.shopTimer = moment().add(seconds, 's').format('x');

            Promise.all([playerShopTimer.set(adventurer.shopTimer), pendingAdventurer.set(adventurer)])
                .then((all) => {
                    return res.status(200).send(adventurer);
                })
                .catch((error) => {
                    console.error(catched);
                    return res.status(200).send(catched);
                });

        })
        .catch(caught => {
            console.error(caught);
            return res.status(200).send(caught);
        });
});
app.get("/date", (req, res) =>
{
    if (req.method === 'PUT') {
        res.status(403).send('Forbidden!');
    }

    cors(req, res, () => {

        let format = req.query.format;

        if (!format) {
            format = req.body.format;
        }

        const formattedDate = moment().format(format);
        console.log('Sending Formatted date:', formattedDate);
        res.status(200).send(formattedDate);
    });
});
app.get("/order", (req, res) =>
{
    if (req.method === 'PUT')
    {
        res.status(403).send('Forbidden!');
    }
    var db = admin.database();
    var playerId = req.user.user_id;
    var player = db.ref("players/" + playerId);
    var playerOrders = player.child("Orders");
    var pendingAdventurer = player.child("PendingAdventurer");

    pendingAdventurer.once('value', snapshot => {

        var adventurer = snapshot.val();

        if (adventurer.type !== 3)
        {
            res.status(404).send("Adventurer is of wrong type");
        }

        var order = {};
        order.timer = moment().add(req.query.timer, 's').format('x');
        order.adventurerid = adventurer.id;
        order.items = [];

        for (var i in adventurer.order.items) {
            order.items.push({
                itemType: i.orderItemType,
                item: i.orderItem,
                amount: i.amount
            });
        }

        playerOrders.push(order)
            .then(snap =>
            {
                res.status(200).send('Done');
            })
            .catch(caught =>
            {
                console.log(caught);
                res.status(404).send(caugth);
            });

    })
    .catch(caught =>
    {
        console.error(caught);
        res.status(404).send(caught);
    });
});
exports.app = functions.https.onRequest(app);

exports.CreateAccount = functions.auth.user().onCreate((userRecord, context) => {

    var db = admin.database();
    var ref = db.ref("players/" + userRecord.uid + "/");
    var userTemplate = db.ref("UserTemplate");

    userTemplate.once("value", snapshot => {
        return ref.set(snapshot.val());
    });
    /*
    return ref.set({
        "Username": "",
        "XP": 0,
        "ShopTimer": 0,
        "ShopTimerSkipedCount": 0,
        "RewardableVideosWatched": 0,
        "Inventory":
        {
            "1": 50,
            "2": 1000
        },
        "Skills":
        {
            "Smithing": 0,
            "Alchemy": 0,
            "Herbalism": 0,
            "LeatherWorking": 0,
            "Spooling": 0,
            "Tailoring": 0,
            "Mining": 0,
            "WoodWorking": 0,
            "GemCutting": 0,
            "JewelCrafting": 0,
            "WeaponSmithing": 0,
            "ArmorSmithing": 0,
            "Dependencies":
            {
                    "WeaponSmithing": ["Smithing"],
                    "ArmorSmithing": ["Smithing"],
                    "Tailoring": ["Spooling"],
                    "Smithing": ["Mining"],
                    "Alchemy": ["Herbalist"]
            }
        }
    });
    */
});

//Helping functions

function getRndInteger(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

function isEmpty(myObject) {
    for (var key in myObject) {
        if (myObject.hasOwnProperty(key)) {
            return false;
        }
    }
    return true;
}