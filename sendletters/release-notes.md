[← back to readme](readme.md)

# Release notes
## 0.1.4
* Added commands to manage online friends (sendletters_me, sendletters_friends, sendletters_addfriend, sendletters_removefriend)
* Fixed letter deliver end time from 4pm to 6pm to match advertised timeframes
* Fixed bug where player would see messages they sent to others
* Fixed bug where sending a letter with no item would crash the game

## 0.1.3-Beta
* Sending items to your other farms (saved games) works just fine. If you add your online friends to the json file, you can send to them too but right now that's a bit manual.
* Added UI notification when a letter is sent
* Filtered what you can send in a letter
* Mail is now only delivered between 8am and 6pm on the hour
* Stability fixes

### Known Bugs
* Sometimes too many updates come in at once and the database files (data.*.json) are locked for editing. Need to add locks to stop it from happening.
