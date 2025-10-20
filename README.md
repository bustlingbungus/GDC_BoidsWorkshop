# Boids Workshop

This repository showcases an implementation of the Boids algorithm in a simple scene in Unity. This project was created to be showcased at the October 11, 2025 Boids workshop, hosted by [USF GameDev Club](https://discord.gg/7cgvAnCjh9). 

## Optimization

The project contains two versions of the algorithm: `Boid.cs`, which is more intuitive, and is what will be presented at the workshop. There is also `BoidOptimized.cs` and `BoidManager.cs`, which is a more refined version of the algorithm, that should run faster, and has lower memory complexity (O(n), down from O(n<sup>2</sup>)).

The main idea behind the optimization is to take the array of all boids away from each individual boid, and have them all reference one boid array. The optimized script also contains various micro-optimizations that should marginally speed things up (albeit without lowering time complexity from O(n<sup>2</sup>)).

## Branches

- `main`: all code, prefabs, and assets, as well as a sample scene with about 60 boid prefabs (using the optimized script) instantiated.
- `workhsop-final`: Only the unoptimized boid script, without the boid manager.
- `workshop-base`: Empty scene, only containing mesh assets, and the camera movement script.
- `optimized`: Only the optimized boid and boid manager scripts.