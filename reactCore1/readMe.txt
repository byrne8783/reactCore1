Meanwhile :
https://blogs.taiga.nl/martijn/2017/11/24/building-and-asp-net-core-mvc-app-with-npm-and-webpack-asp-net-core-2-0-edition/      the basis
https://codeburst.io/how-to-use-webpack-in-asp-net-core-projects-a-basic-react-template-sample-25a3681a5fc2
https://github.com/natemcmaster/aspnetcore-webpack-hmr-demo
https://www.red-gate.com/simple-talk/dotnet/net-development/using-auth-cookies-in-asp-net-core/
provide guidance

Right now :
want to get Bootstrap/jquery/its two siblings/popper.js/toottip.js integrated into webPack so it generates the dist files
I have jQuery integrated with a manual build ('npm run wbp') and have a plugin to automatically load jQuery into the js bundle, 
I have Bootstrap installed and loading as a module, and its styling the nav bar and menu! I had to update this stuff for BootStrap 4 
I have site.css loading as a module, however I do not have it doing an automatic build based on the source file save
I ignored problems getting the 'babel-loader' to transpile ES6 code since I want to go for Typescript 3.0 anyway
I have my css ( practically site.css and bootstrap ) being bundled into a file called AllStyles.css by using "mini-css-extract-plugin" on the styles loades with css-loader.
	I won't get HMR on style changes - to do that I would have to use an ( advanced? ) config, and I'm not ready for that.
I have my webpack config set up to permit definition of a 'production' build.  I don't have one yet, but I don't need it at the moment
I have funky HMR going - at least for javascript.  Use 'dotnet watch run' CLI command to have it in action
I also have regular ( WebPackDevMiddleware based) funky HMR going, I.E. without 'dotnet watch run'
I have Typescript 4.x running and functioning in the HMR, but Typescript itself seems to have an indntity crises between .0 and .1
I've decided to separate out my 'source' from what is shipped to the browser.  So 'client/' is the thingy I edit
I've transferred my style definitions to sass and style-loader.  So, I'm getting funky HMR when I change the .scss.   We're sucking diesel.
 
I need to get my head around Typescript. I'm not getting output where I wanted it: e.g. wtf happened trying to sort out type defs for tooltip on jQuery .

get your head around the 'uglify' thingys in sample-25a3681a5fc2 and get a webpack production build implemented with minification and uglification
	slowly , not very surely.  Struggling with the 'separate config files ' approach and have left it dead as webpack.<common,dev,prod>.js.  
	So, basically i'm running with webpack.config.js
	Have made a connection between the webPack env variable and the VS build using launchSettings.json with conditional targets in the .csproj file???

Added a bit of Login as a prelude to Authorization.
	Basically I'm running a Login thru UserController.Login . Happily, with Firefox , Edge and in Chrome....  



Onward and Upward

		And I'de like to move on to Core 3.1....
		so Im having a go at this https://dev.to/jayotterbein/how-to-get-hot-module-reload-with-custom-webpack-in-asp-net-core-3-1-5a7m
		**********************************
		got rid of this        "webpack-dev-middleware": "^3.7.3",  "webpack-hot-middleware": "^2.25.0",
		I'm not getting the funky HMR - webpack-dev-server is being asked, but dos'nt know its his.  
		And I'm not getting the timeout logout message - thats because I did'nt write any code for it. I just wrote the loading icon timer.
		Have a look again at that web-socket line in webpack.config.js




		And from there to .Net 5
Then I'de love to remember what I was trying to do.....


Ah yes.  Add React to your projects-a-basic-react-template-sample-25a3681a5fc2
		Figure out how to respond when an '[Authorize]' view like Personal is requested...
		Then I'de like to make the Logout dialog do an auto-logout on a Timeout,  make the modal bit of Login and Logout load dynamically...

	I'm getting bootstrap css from the CDN in _layout.css. - I'm using the CopyWebPackPlugin to copy these files from node-modules to /lib as a fallback
															-  then I'de like to figure out how to get version correspondence between what I copy to /lib and what i get from the cdn hmm!

Do the whole thing again, just to be able to understand it, with guidance from https://blogs.taiga.nl/martijn/2018/06/14/lean-asp-net-core-2-1-manually-setup-a-razor-pages-project-with-bootstrap-npm-and-webpack/ 

Figure out the various ways ( if any! ) React plays with Razor and optimise the HTML accordingly

Then have a look at the overall project component structure - after all thats where you want to get to

Write a React component, such as a 'tag helper' , maybe even a calendar component.  
Here's a primer https://blog.flowandform.agency/create-a-custom-calendar-in-react-3df1bfd0b728



