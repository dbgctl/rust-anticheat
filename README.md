# rust anticheat

A proof of concept implementation of an anti-cheating measure for Rust videogame, targeting a possible cheat to override projectile hit-registration to any hitbox, but most commonly head hitbox.
This can be expanded into a server-side detection by using telemetry of protected key-value pairs, then integrity checking those on server.