# Birdheim VeinMining Compat

## What
A lightweight compatibility patch that allows **Birdheim** (by *hansenbansen*) and **VeinMining** (by *BranchOfYore*) to work together without restricting your bird's flight range.

---

## Why
To keep the world loaded around your bird while flying far away from your character, **Birdheim** monitors Harmony patches on core Valheim world-loading and cleanup methods (such as `ZNetScene.RemoveObjects`). 

If Birdheim detects an unrecognized mod hooking into these methods, it triggers a built-in safety mechanism (`LoadingGuard`) and hard-caps your bird's maximum flight distance to **100 meters** around your body—regardless of your Bird Bond skill level or config settings—logging the following warning:
> `[Warning: Birdheim] flights stay within 100 m of the body, in one loaded area: local.valheim.sharedmining on ZNetScene.RemoveObjects`

Because **VeinMining** (`BranchOfYore-VeinMining`, internal BepInEx GUID `local.valheim.sharedmining`) hooks `ZNetScene.RemoveObjects` solely to handle destroyed ore segments and network synchronization, this is a false positive. Both mods are functionally compatible, yet Birdheim's safeguard blocks long-distance flights whenever VeinMining is installed.

---

## How
Instead of modifying either mod's assembly files, this plugin runs once during BepInEx startup (`Awake`) after Birdheim has loaded:

1. **Reflection Lookup:** Using Harmony's `AccessTools`, it locates Birdheim's internal `LoadingGuard` class without requiring a hard assembly reference (keeping it resilient against future Birdheim updates).
2. **Whitelist Injection:** It accesses the static `LoadingGuard.Known` collection—the same internal whitelist Birdheim uses for other known-compatible mods—and adds VeinMining's internal GUID (`"local.valheim.sharedmining"`) to it.
3. **Result:** When `LoadingGuard.Check()` evaluates active Harmony patches upon loading a world, VeinMining's `ZNetScene.RemoveObjects` hook is recognized as safe. Birdheim disables the 100 m restriction, suppresses the warning, and restores your configured flight range under `[Bond]`.

---

## Installation & Multiplayer
* **Client-side:** Because Birdheim runs its `LoadingGuard` check and area-loading logic locally on each player's client, this patch only needs to be installed on the **client** (for every player using both Birdheim and VeinMining).
* **Dedicated Servers:** Installing it on a server is not required, though keeping it in a shared server modpack will not cause any issues.
* *(Note: Multiplayer behavior has not been extensively tested yet, as this patch was developed and verified in singleplayer, but since it only modifies a local client-side whitelist, it should work identically in multiplayer sessions.)*

---

## Notes & Disclaimer
* **Minimal Footprint & Safety:** This mod does not patch any game methods directly, alter save data, or change how either mod operates. All it does is use Harmony's reflection utilities once on startup to append a single string (`"local.valheim.sharedmining"`) to an existing list in memory. Still, as with any Valheim mod, **always back up your worlds and characters** and use at your own risk.
* **Future Birdheim Updates:** If the Birdheim team officially adds `local.valheim.sharedmining` to their built-in `LoadingGuard.Known` whitelist in a future update, this compatibility patch will become obsolete (and harmless, as it checks whether the entry is already present before adding it). Conversely, should the Birdheim developers ever determine that VeinMining causes genuine issues with Birdheim's world-loading logic, an appropriate warning/disclaimer will be added here.

---

## Credits
* **[Birdheim](https://thunderstore.io/c/valheim/p/HansenbansenTeam/Birdheim/)** by **hansenbansen** — for the original bird companion & flight mod.
* **[VeinMining](https://thunderstore.io/c/valheim/p/BranchOfYore/VeinMining/)** by **BranchOfYore** — for the original multiplayer vein-mining mod.

---

## Contact & Bug Reports
* **GitHub Issues:** [Report a bug or suggest a change](https://github.com/Lupsy00/BirdheimVeinMiningCompat)
