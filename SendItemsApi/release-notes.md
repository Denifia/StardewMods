[← back to readme](readme.md)

# Release notes
## 0.2.0
* All code is now async for performance
* Moved from LiteDb to Azure Table Storage
* Changed mail endpoint paths, names and parameters to be more clear
* Creating mail on the server has changed verbs from POST to PUT
* Added a /api/mail/count endpoint to check how much mail is on the server
* Added a /api/health endpoint that just returns true

## Prior to 0.2.0
* Intital cut of api
* No release notes available