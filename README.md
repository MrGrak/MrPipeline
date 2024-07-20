


# MrPipeline

automates the process of adding assets to a mgcb file.
automates the process of loading assets into a Assets.cs file.
this is done via a ProcessContent() method that you can embed into
your Monogame project and call as needed.
this is done by reading a folder of assets and writing the appropriate files.
this isn't a library! dont contact me looking for support or features!
it's just an example, please modify to suit your needs.

the expected workflow is:
- copy paste all your assets (pngs, wavs, fx, bmps, etc) into the MG content folder.
- run MrPipeline to generate a mgcb file and Assets.cs file.
- open generated mgcb file, save + build it.
- add the Assets.cs file to your MG project.
- reference assets via Assets.assset_name.
- build and test your game.


# Bonus

You can run MrPipeline when you add new assets and it will just update the
Assets.cs file, which can be reloaded in your ide to see the changes.


# FAQ

How do I use this tool?
- read, modify, and embed the MrPipeline source code into your codebase OR
- (coming soon) copy paste the MrPipeline.exe into your content folder and run it

Why would you load all the game assets into ram at boot?
- this strategy works well for games that use less than 3-4gbs of ram.
- most indie games dont use much ram, so it makes sense to "just load it all"
- this strategy removes loading content dynamically, so no loading screens
- this strategy reduces number of system calls made (can be limitation on consoles)

Why would you make all the assets static and publically accessible?
- the lifetime of a static instance is the lifetime of the program
- these assets aren't intended to be unloaded until the program closes
- these assets are designed to be easy to work with, so they're public
- you are free to modify source code to suit your architectural preference




TODO:

MrPipeline could become a .exe that we drop in the Content folder and run to
produce the mgcb and Assets.cs files. this way it's very user friendly.
we can provide small compiled .exe and source code for use.




