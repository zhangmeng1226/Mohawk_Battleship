VERSIONING

The SimpleBots follow a 3-point versioning system of logic/technique/upkeep

	Logic coorelates to the bots capabilities or 'how it thinks'.

	Technique coorelates to specific style of implementation for that logic or 'how it's coded'

	Upkeep coorelates to patches/fixes/tweaks to either of those things.

		For example, consider the first bot created, SimpleBot_1_0_0.

		If SimpleBot_1_0_0 is updated with a new feature, changing the means by which it plays
		and thus effecting it's ability to win, this is a 'logical' change and would therefore 
		require that the new version be SimpleBot 2_0_0. 

			An example of a logic change can be found in the actual implementation of SimpleBot_1 and
			SimpleBot_2. 
			
			SimpleBot_1 makes it shots by sequentially targeting grid cells, row by row.
			SimpleBot_2 shoots randomly. These are completely different approaches to targeting, and is thus 
			a logic change.

		If SimpleBot_1_0_0 were instead updated in a way which changes how it works, but not how it thinks,
		this would be a 'technique' change, requiring a versioning number of 1_1_0.

			An example of a technique change may be found in the plans for SimpleBot_2_0_0 and SimpleBot_2_1_0.

			Both bots shoot randomly, there is no difference in logic. SimpleBot_2_0_0 picks a random location, 
			checks if the location has previously been fired upon. If so, it selects again until an available 
			target is found. This is extremely inefficient, resulting in an ever-growing number of iterations 
			per shot to find an available target.

			SimpleBot_2_1_0 generates and maintains a list of available targets. Randomly selecting locations from 
			the list and subsequently removing them, thus removing the iterative process for significant gains in efficiency.
			
			The two bots achieve the same performance (in win/loss terms), but the difference in technique merits the versioning change.

	The sequence of versioning numbers is not necessarily tied to improvents in gameplay, but rather complexity.

		For example SimpleBot_1 tends to outperform SimpleBot_2, but SimpleBot_2 is a more complicated in terms of logic and code.

		When planning new version releases, the sequence should be viewed from a teaching perspective, allowing the bots 
		to be viewed in sequence as someone new to the framework seeks to advance their understanding.

		This is not true of upkeep version increments, these are always sequential
		 