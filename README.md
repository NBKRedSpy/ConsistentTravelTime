# Consistent Travel Time
Changes the travel time to a planet to be consistent in all situations.


# Spoiler
There is an inconsistency in the travel times to systems.  If you do not want to know about it, skip this mod.  Once you see it, you cannot unsee it.  I've said too much.  It may be too late for you.

# Battletech Travel Times
Battletech has two different times to travel to the planet:

* When ending a navigation, the time to the planet is the "Jump Distance", which is anywhere from 1 to 16 days.
* When aborting a jump and traveling to the system, the time to planet is the "Planet Cost", which is generally 3 days.

## Travel Time Changes

By default, this mod will change the time to planet to be the lower of the "jump distance" and the "planet cost".

For example, a planet with a jump distance of 7 days will take 3 days to travel.  A planet with a jump distance of 1 day will take 1 day to travel to the planet.

Battletech's default travel time from planet to jumpship is not affected.

# Settings

|Setting|Default|Description|
|--|--|--|
|UseLowerAmount| true| If false, will always use the planet's travel cost (3 days).  Otherwise, will use the lower of the travel cost and jump distance.


# Compatibility
Safe to add and remove from existing saves.

May conflict with mods that change the travel to planet times.  However, mods which use the standard .json merge will not conflict.
