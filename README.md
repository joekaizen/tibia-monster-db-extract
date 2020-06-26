# tibia-monster-db-extract
Extracts and converts the monster spawns from CipSoft's "monster.db" file to XML format for Open Tibia servers.

# How to use it
Place the "monster.db" file and the entire "origmap"-folder in the same directory as the executable.
Run the executable and it will generate a file named "output.xml" that will include the spawns.

If (for some reason) some spawns end up with error, they will get the X and Y offsets "99".
So correct them manually, by just searching: x="99" and y="99"
