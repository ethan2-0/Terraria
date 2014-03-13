<h1>OpenTerraria</h1><br/>
OpenTerraria is an attempt to create an opensource Terraria, hence the name.<br/>
It is developed in .NET, and will have some success on Wine 1.7.11/Mono 3.2.6 with the windowscodecs winetrick installed (but don't count on it).<br/>
A list of known bugs is maintained here:<br/>
<ul>
	<li>On Wine/Mono, performance is horrible.</li>
	<li>On Wine/Mono, Wine starts saying <code>fixme:gdiplus:GdipDrawPath graphics object has no HDC</code> repeatedly.</li>
	<li>In a dual-monitor configuration, if OpenTerraria starts up on the smaller monitor, it will not look right on the larger monitor.</li>
	<li>The collision algorithm acts weird sometimes - Recently, this has been improved, but not perfected</li>
	<li>When you go outside of the generated world, the game becomes much laggier.</li>
	<li>Recepies with more than one ingredient render weirdly</li>
	<li>Recepies with outputs of items with long names render weirdly</li>
</ul>
Copyright (c) 2014 ethan20. All rights reserved.