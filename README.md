# Consistent Travel Time
Changes the travel time to a planet to be consistent in all situations.


# Spoiler
There is an inconsistency in the travel times to systems.  If you do not want to know about it, skip this mod.  Once you see it, you cannot unsee it.  I've said too much.  It may be too late for you.

# Battletech Travel Times
Vanilla Battletech has two different times to travel to a planet:

* When ending a navigation, the time to the planet is the "Jump Distance", which is anywhere from 1 to 16 days.
* When aborting a jump and traveling to the system, the time to planet is the "Planet Cost", which is generally 3 days.

This mod allows the user to use either the Planet Cost or Jump Distance and defaults to Planet Cost.  This mod does not affect the time required to travel from the planet to the jump ship, which is always Jump Distance.

# Settings

|Setting|Default|Description|
|--|--|--|
|UseLowerAmount| true| For PlanetCost strategy only.  If false, will always use the planet's travel cost (3 days).  Otherwise, will use the lower of the travel cost and jump distance.
|PlanetTravelStrategy|PlanetCost|Determines how many days it will take to travel to a planet when navigation is canceled.  Can be `PlanetCost` or `JumpDistance`. See the [Planet Travel Strategy Settings](#planet-travel-strategy-settings) section.


## Planet Travel Strategy Settings

|Strategy|Description|
|--|--|
|PlanetCost|Will be the same time as canceling navigation and going directly to the planet.  Generally 3 days.
|JumpDistance|Will be the Jump Distance, which is the same time as navigating directly to the planet without canceling.  Generally 1-16+ days.|

# Strategy Descriptions
## Planet Cost
The default setting for this mod is PlanetCost.

The PlanetCost strategy setting changes the number of days required to reach the planet to always be as if the player planned to overshoot a system, canceled navigation, and then traveled to the local system.

### Plant Cost Opinions
Some users may consider the Planet Cost strategy as cheating or an exploit.  Others may consider this strategy as a vanilla Min/Max QoL change that takes out the work of needing to over navigate and cancel.

Either way, it's a single player game so I say play it however you like.

### Note
Currently the total days estimated for a navigation does not take into account the reduced travel time to the target planet.  This is a UI mismatch and may be addressed in the future.

## Jump Distance
The JumpDistance strategy setting forces a canceled navigation to always require the same number of days whether the player navigates directly to the planet or cancels a navigation and then travels to the planet.  


# Compatibility
Safe to add and remove from existing saves.

May conflict with mods that change the travel time to planets or overrides game functionality used to create navigation plans.

Mods which use the standard .json merge will not conflict.

# Source
Source can be found at https://github.com/NBKRedSpy/ConsistentTravelTime
