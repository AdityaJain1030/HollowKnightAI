## Observation Space

- Needs to show all attacks, ground, knight, etc.
- Needs to be less than 100x100
- May or may not show health
- Give off last 5 frames as game state condensed in one

## Action space

- Try to rip off dota action space
- Basically have controller inputs + delay for each

Hollow knight controls

movement - press or dont press (continuous)
dash - press (recharges after x time)
focus/cast - press or hold (can be discreet and continuous)
super dash - Must be held and relseased
Attack - press (must be recharged and can be held)


Each message from client (game)
obs_size, stacked_observation_frames, knight_health, knight_dead, boss_dead, action_size

each message from server (python)
action_taken


NUM_ACTIONS:
Up/Down/None * Left/Right/None * cast/attack/none * jump/none * dash/none

3 * 3 * 3 * 2 * 2 = 128
a * b * c * d * e

index -> a,b,c,d,e

ALSO IMPLEMENT WEBSOCKETS IN A COROUTINE WITH A CUSTOM YEILD INSTRUCTION FOR WAITING FOR WS MESSAGES