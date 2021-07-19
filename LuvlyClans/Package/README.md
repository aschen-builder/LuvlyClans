I like teams. I like to fight. I like to fight as a team. This plugin is an attempt to satisfy what I like (and what I need).

Provides in-game clans and mechanics built around those clans (permissions, damage, status effects, visibility, and more).

BE ROCK HARD, BRODER!

- [FEATURES](#features)
    - [In Release:](#in-release)
    - [In QA:](#in-qa)
    - [In Development:](#in-development)
    - [In Backlog:](#in-backlog)
- [Dependencies](#dependencies)
- [INSTALLATION](#installation)
- [CONFIGURATION](#configuration)
  - [SERVER](#server)
- [CHANGELOG](#changelog)
  - [v0.1.0](#v010)
  - [v0.0.5](#v005)
  - [v0.0.4](#v004)


## FEATURES
#### In Release:
- Door Access
- Container Access
- Minimap Public Pin Visibility


#### In QA:
- Portal Access
- Ship Controlls Access
- HoverName Hidden per Clan
- Friendly Fire reduction per Clan
- Multi-world db centralization (need feedback and cluster specs PLEASE FOR THE LOVE OF FREYA)

#### In Development:
- ABACL and Behavior Config (Server & Client versions)
- Clan Management GUI
- Redis integration


#### In Backlog:
- Clan Skill Tree
- Clan Items
- Clan Game Chat
- Clan Ranks


## Dependencies
Built with Jotunn (thanks Jules and co):
- BepInEx
- Jotunn

Future releases will be focused on keeping dependencies to a minimum, and they will only be added when absolutley needed or feature output outweighs development time needed.


## INSTALLATION
Both the client and the server need to have this mod installed. Simply install with your preferred mod manager, or manually download, unzip, and copy `LuvlyClans.dll` to `/BepInEx/plugins`.


## CONFIGURATION

### SERVER
Once the plugin is installed, you will need to create your clan "database" in the BepInEx config directory (generally just `/BepInEx/config`).

In the config directory, create a file named `luvly.clans.json`. Use the following template to setup your clans:

```
{
  "m_clans": [
    {
      "m_clanName": "LuvlyZakus",
      "m_members": [
        {
          "m_playerName": "Luvsdev",
          "m_playerSID": 76561198064558302,
          "m_playerRank": 4
        },
        {
          "m_playerName": "Notchar",
          "m_playerSID": 76561198060094387,
          "m_playerRank": 4
        },
      ]
    },
    {
      "m_clanName": "ShittyGMs",
      "m_members": [
        {
          "m_playerName": "Pissyamuro",
          "m_playerSID": 76561198132416693,
          "m_playerRank": 4
        }
      ]
    },
    { "m_clanName": "Wildlings", "m_members": [] }
  ]
}
```

All players need to have their exact in-game name listed for the playerID relationship to be made.

The Wildlings clan is the default "solo-player" clan and will have future features (status effects, etc) built around the solo and less protected play style. Use as you would like for now.


## CHANGELOG

### v0.1.0
- Bigger and badder, babbyyy!
- Rebuilt the entire thing to account for offline/non-region players
- Added server-side clan state management, making the server absolute GOD
- Made everything JSON because who doesn't fucking love JSON?
### v0.0.5
- Realized I'm a fucking idiot
- Refactored logic for determining piece ownership
- Refactored and reduced Global Character/Player utils (still more to do, thanks ASharpPen you're a beast)
### v0.0.4
- Fixed nullPointException on ShipControlls
- Fixed TeleportWorld patches and added truthy logging
