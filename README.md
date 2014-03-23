<h1>OpenTerraria</h1><br/>
OpenTerraria is an attempt to create an opensource Terraria, hence the name.<br/>
It is developed in .NET, and will have some success on Wine 1.7.11/Mono 3.2.6 with the windowscodecs winetrick installed (but don't count on it).<br/>
A list of known bugs is maintained here:<br/>
<ul>
	<li>~~On Wine/Mono, performance is horrible.~~</li>
	<li>~~On Wine/Mono, Wine starts saying <code>fixme:gdiplus:GdipDrawPath graphics object has no HDC</code> repeatedly.~~</li>
	<li>Commit 3c93312c55 broke Wine/Mono compatibility. Eventually, I plan on distributing a <a href="http://dev.mainsoft.com/">Grasshopper</a> version, or <a href="https://github.com/xamarin/XobotOS/tree/master/sharpen">porting OpenTerraria to Java.</a></li>
	<li>In a dual-monitor configuration, if OpenTerraria starts up on the smaller monitor, it will not look right on the larger monitor.</li>
	<li>The collision algorithm acts weird sometimes - Recently, this has been improved, but not perfected</li>
	<li>When you go outside of the generated world, the game becomes much laggier.</li>
	<li>Recepies with more than one ingredient render weirdly</li>
	<li>Recepies with outputs of items with long names render weirdly</li>
	<li>If you place a torch inside of the area of another torch, sometimes it makes the other torch inoperable until another lighting update (and, even then, it doesn't always work).
</ul>
Copyright (c) 2014 ethan20. All rights reserved.