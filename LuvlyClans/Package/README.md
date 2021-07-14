# LUVLY CLANS
A Valheim clans mod built on the back of Enhanced Progress Tracker. This mod allows players to configure in-game clans, which the grants attribute based access controls depending on internal/external tribe relationships.

- [LUVLY CLANS](#luvly-clans)
  - [FEATURES](#features)
      - [In Release:](#in-release)
      - [In Development:](#in-development)
      - [In Backlog:](#in-backlog)
  - [Dependencies](#dependencies)
  - [INSTALLATION](#installation)
  - [CONFIGURATION](#configuration)
  - [CHANGELOG](#changelog)
    - [v0.0.5](#v005)
    - [v0.0.4](#v004)


## FEATURES
#### In Release:
- Shared experience
- Door Access
- Container Access
- Ship Control Access
- Portal Access
- Minimap Public Pin Visibility
- PvP Damage Reducer for FF


#### In Development:
- Hide Non-Clan Player HoverName
- Clan Management GUI


#### In Backlog:
- Clan Skill Tree
- Clan Items
- Clan Game Chat
- Clan Ranks


## Dependencies
This is built on the back of EPT but also requires a few other major deps. The following mods are the only dependencies:
- BepInEx
- Jotunn
- Enhanced Progress Tracker

Future releases will be focused on keeping dependencies to a minimum, and they will only be added when absolutley needed or feature output outweighs development time needed.


## INSTALLATION
Both the client and the server need to have this mod installed. Simply install with your preferred mod manager, or manually download, unzip, and copy `LuvlyClans.dll` to `/BepInEx/plugins`.


## CONFIGURATION
This mod relies on the Tribe configuration files of Enhanced Progress Tracker.

You can view the configuration setup here:

https://github.com/ASharpPen/Valheim.EnhancedProgressTracker#configuration

## CHANGELOG

### v0.0.5
- Realized I'm a fucking idiot
- Refactored logic for determining piece ownership
- Refactored and reduced Global Character/Player utils (still more to do, thanks ASharpPen you're a beast)
### v0.0.4
- Fixed nullPointException on ShipControlls
- Fixed TeleportWorld patches and added truthy logging
