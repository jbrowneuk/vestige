# Vestige

<div align="center">
  <img alt="license" src="https://img.shields.io/github/license/jbrowneuk/vestige.svg"/>
  <img alt="language" src="https://img.shields.io/badge/C%23-6-blue.svg"/>
  <img alt="framework" src="https://img.shields.io/badge/MonoGame-3.7-red.svg"/>

  <a href="https://discord.gg/vH4zaDT"><img alt="chat" src="https://img.shields.io/badge/discord-join-7289DA.svg?logo=discord&logoColor=white"/></a>
  <a href="https://twitter.com/jbrowneuk"><img alt="twitter" src="https://img.shields.io/badge/twitter-jbrowneuk-1da1f2.svg?logo=twitter&logoColor=white"/></a>
</div>

----

Vestige is a 2D top-down RPG game engine in the style of the early Lufia games built in C# utilising the [MonoGame](http://www.monogame.net/) framework.

> **Vestige | ˈvɛstɪdʒ |**
>
> a trace or remnant of something that is disappearing or no longer exists

This is the official repository for the project that has become my “forever project”.

## Getting Started
The repository contains a Visual Studio solution containing the game runner project for each supported platform, as well as a shared project containing the game engine.

### Prerequisites
To get running quickly, the minimum development environment requires:

- Visual Studio Community 2017 (or equivalent MonoDevelop)
- MonoGame 3.7

Later versions should also be compatible.

## Contributing
All game code and assets should be placed in the shared engine project. The game runner solutions should only contain the bare minimum required to create the game window and initialise the engine.

Content is controlled by the engine’s `Content.mgcb` pipeline project. Assets do not need to be added to the shared project if they exist as part of the pipeline project.

Art assets with SVG source files were originally generated in Inkscape – please ensure that any tool used to modify them is compatible. The plan is to use Inkscape as part of the compilation step in the future to automatically update rendered assets.