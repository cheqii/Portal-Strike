# 3D Game Card

A game card with a 3D effect like holographic cards have.
It has three-dimensional objects behind that you only can see through the card's window.
It uses stencil shaders with masks to accomplish this.

by BadToxic (http://badtoxic.de)

— Content —

- *StencilMask* shader
- *StencilObject* shader
- *GameCard* prefab, ready to use
- *game-card-frame* frame sprite template
- *Rotate* C# script for rotation

— Requirements —

_Shader_: Works on all platforms supported by Unity. *DX9* shader model *2.0*.
Shader credits go to [Alan Zucconi](https://www.alanzucconi.com/2015/12/09/3873/)

— Usage —

Replace the 3D objects in the card with whatever you want and use the _StencilObject_ shader for the materials. The objects will only be visible through the card window using the _StencilMask_ shader.
Use different ids in the __StencilMask_ shader variable if you want to have multiple object-mask pairs. (Objects only visible through the correct window.)
Design the card frame as you want and replace the texts for your needs.

— Support —

Need help? Join my discord server: https://discord.gg/8QMCm2d

— Follow me —

Support me with likes and follows/subscriptions:
[Instagram](https://www.instagram.com/xybadtoxic)
[Twitter](https://twitter.com/BadToxic)
[YouTube](https://www.youtube.com/user/BadToxic)
[Facebook](https://www.facebook.com/XyBadToxic)