# How to use
In game, select a giftable item (or stack) in your hotbar (the thing thats always shown on screen at the bottom)
Make sure what you want to send is the selected item
Press the number pad key that matches with the friend you want to send to
e.g. if your YourFriendsUniqueIds has only one id, then by pressing `1` on the numpad you'll send the selected items to that friend

Warning: If you get a mail and close it without collecting the items, the items are lost forever - it's a bug :)

# Configuration
In the `SendLetters` folder is a `config.json` file
YourUniqueId = anything that makes you're "user" unique to the server (e.g. 1, Freddy, Dude#223)
YourDisplayName = the name you'll sign your letters with
YourFriendsUniqueIds = list of your friends YourUniqueId's (e.g. 1, Freddy, Dude#223)

# SendLetters
Make sure you have SMAPI-1.9ish installed :)
Copy `SendLetters` folder to your mods folder

# WebApi
Install dotnet core
Copy `WebApi` to somewhere on your computer
Open a command prompt to the `WebApi` folder
Run `dotnet denifia.stardew.webapi.dll` to start the server
WebApi will be running on http://localhost:5000
Test it by going to http://localhost:5000/api/players