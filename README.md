# Sample Project (Fighting Game)
It is a sample project to show a part of my coding approaches and knowledge, also some architectures (and patterns) which I've been using here.

You can check the project docs [here](https://github.com/ATHellboy/SampleProject-FightingGame/tree/master/Docs).

I didn't make custom character controller for characters and I used Unity Physics and Rigidbody because of time but it supports slopes.

There are some stuff that are good to be checked besides my coding knowledge. They are:

* Combat system with doc [here](https://github.com/ATHellboy/SampleProject-FightingGame/blob/master/Docs/CombatSystem.pdf).

* Object Pooling system with doc [here](https://github.com/ATHellboy/SampleProject-FightingGame/blob/master/Docs/ObjectPoolingSystem.pdf).

* ScriptableObjectDropdown attribute. it has a separate repository that you can check it out [here](https://github.com/ATHellboy/ScriptableObjectDropdown). Also ScriptableObjectMultiSelectDropdown attribute which I didn't use in this project is [here](https://github.com/ATHellboy/ScriptableObjectMultiSelectDropdown).

* It is not a big deal but don't forget to check my State Machine too. Its brief doc is [here](https://github.com/ATHellboy/SampleProject-FightingGame/blob/master/Docs/StateMachine.pdf).

I used [VContainer](https://github.com/hadashiA/VContainer) for DI and [MessagePipe](https://github.com/Cysharp/MessagePipe) for the event system (Besides Unity built-in events). On previous implementation, I used [Extenject](https://github.com/Mathijs-Bakker/Extenject).

# Controls
Player One | Movement : WASD | Jump : Space | Attack : B | Powerup : N | Switch Character : M

Player Two | Movement: Arrow Keys | Jump : 0 | Attack : 1 | Powerup : 2 | Switch Character : 3

Bunny Complex Attack : Attack Button (In Air)

Kitty Complex Attack: Attack Button (After Double Jump)

# MIT License

Copyright (c) 2019 Alireza Tarahomi

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
